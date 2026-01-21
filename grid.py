import pygame
import math
from enum import Enum


class TileType(Enum):
    EMPTY = 0
    PLAYER = 1
    ENEMY = 2
    OBSTACLE = 3
    EXIT = 4


TILE_COLOURS = {
    TileType.EMPTY: (240, 240, 240),
    TileType.PLAYER: (220, 50, 50),
    TileType.ENEMY: (50, 200, 50),
    TileType.OBSTACLE: (150, 100, 60),
}


class HexTile:
    def __init__(self):
        self.type = TileType.EMPTY


class HexGrid:
    def __init__(self, width, height, size):
        self.width = width
        self.height = height
        self.size = size
        self.tiles = [
            [HexTile() for _ in range(height)]
            for _ in range(width)
        ]

    # -----------------------
    # Tile logic
    # -----------------------
    def get_tile(self, q, r):
        if 0 <= q < self.width and 0 <= r < self.height:
            return self.tiles[q][r]
        return None

    def set_tile(self, q, r, tile_type):
        tile = self.get_tile(q, r)
        if not tile:
            return

        if tile_type == TileType.EXIT:
            self.clear_exit()

        if tile.type == TileType.EXIT:
            tile.type = TileType.EMPTY

        tile.type = tile_type

    def clear_exit(self):
        for col in self.tiles:
            for tile in col:
                if tile.type == TileType.EXIT:
                    tile.type = TileType.EMPTY

    def swap_tiles(self, a, b):
        tile_a = self.get_tile(*a)
        tile_b = self.get_tile(*b)
        if not tile_a or not tile_b:
            return

        if tile_a.type == TileType.EXIT or tile_b.type == TileType.EXIT:
            return  # Exit cannot be moved

        tile_a.type, tile_b.type = tile_b.type, tile_a.type

    # -----------------------
    # Drawing
    # -----------------------
    def draw(self, surface):
        for q in range(self.width):
            for r in range(self.height):
                center = self.hex_to_pixel(q, r)
                tile = self.tiles[q][r]
                self.draw_hex(surface, center, tile)

    def draw_hex(self, surface, center, tile):
        points = self.hex_corners(center)
        colour = TILE_COLOURS.get(tile.type, (240, 240, 240))
        pygame.draw.polygon(surface, colour, points)
        pygame.draw.polygon(surface, (0, 0, 0), points, 2)

        if tile.type == TileType.EXIT:
            font = pygame.font.SysFont(None, 32)
            text = font.render("X", True, (0, 0, 0))
            rect = text.get_rect(center=center)
            surface.blit(text, rect)

    # -----------------------
    # Hex math
    # -----------------------
    def hex_to_pixel(self, q, r):
        x = self.size * (3/2 * q) + 100
        y = self.size * (math.sqrt(3) * (r + q / 2)) + 100
        return int(x), int(y)

    def hex_corners(self, center):
        cx, cy = center
        corners = []
        for i in range(6):
            angle = math.pi / 180 * (60 * i - 30)
            x = cx + self.size * math.cos(angle)
            y = cy + self.size * math.sin(angle)
            corners.append((x, y))
        return corners

    def pixel_to_hex(self, pos):
        mx, my = pos
        mx -= 100
        my -= 100

        q = (2/3 * mx) / self.size
        r = (-1/3 * mx + math.sqrt(3)/3 * my) / self.size
        return round(q), round(r)
