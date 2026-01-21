import pygame
import math
from enum import Enum


class TileType(Enum):
    """
    Enumeration of all supported tile types.

    Used to identify the logical role of a tile in the grid.
    Rendering and behaviour are based on this value.
    """
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
    """
    Represents a single hex tile in the grid.

    This class intentionally contains very little logic.
    It acts as a simple data container.
    """
    def __init__(self):
        """
        Create a new hex tile.

        The tile starts as EMPTY by default.
        """
        self.type = TileType.EMPTY


class HexGrid:
    """
    Manages a 2D hexagonal grid.

    Responsibilities:
    - Store tile data
    - Convert between grid coordinates and screen coordinates
    - Enforce grid-level rules (e.g. only one exit)
    - Draw the grid and tiles
    """

    def __init__(self, width, height, size):
        """
        Create a hex grid.

        Parameters:
            width (int): Number of columns in the grid
            height (int): Number of rows in the grid
            size (int): Hex size (distance from centre to corner)
        """

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
        """
        Get the tile at a given grid coordinate.

        Parameters:
            q (int): Column index
            r (int): Row index

        Returns:
            HexTile | None:
                - The tile at (q, r) if within bounds
                - None if the coordinates are invalid
        """
        if 0 <= q < self.width and 0 <= r < self.height:
            return self.tiles[q][r]
        return None

    def set_tile(self, q, r, tile_type):
        """
        Set the type of a tile at a given grid coordinate.

        Enforces grid rules:
        - Only one EXIT may exist at a time
        - An existing EXIT is cleared if overwritten

        Parameters:
            q (int): Column index
            r (int): Row index
            tile_type (TileType): New tile type
        """
        tile = self.get_tile(q, r)
        if not tile:
            return

        if tile_type == TileType.EXIT:
            self.clear_exit()

        if tile.type == TileType.EXIT:
            tile.type = TileType.EMPTY

        tile.type = tile_type

    def clear_exit(self):
        """
        Remove any existing EXIT tile from the grid.

        This is used internally to enforce the
        'only one exit' rule.
        """
        for col in self.tiles:
            for tile in col:
                if tile.type == TileType.EXIT:
                    tile.type = TileType.EMPTY

    def swap_tiles(self, a, b):
        """
        Swap the tile types between two grid coordinates.

        Exit tiles cannot be moved and will not swap.

        Parameters:
            a (tuple[int, int]): First tile coordinate (q, r)
            b (tuple[int, int]): Second tile coordinate (q, r)
        """
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
        """
        Draw the entire grid to the screen.

        Parameters:
            surface (pygame.Surface): Surface to draw onto
        """
        for q in range(self.width):
            for r in range(self.height):
                center = self.hex_to_pixel(q, r)
                tile = self.tiles[q][r]
                self.draw_hex(surface, center, tile)

    def draw_hex(self, surface, center, tile):
        """
        Draw a single hex tile.

        Parameters:
            surface (pygame.Surface): Surface to draw onto
            center (tuple[int, int]): Pixel position of the hex centre
            tile (HexTile): Tile data to render
        """
        points = self.hex_corners(center)
        colour = TILE_COLOURS.get(tile.type, (240, 240, 240))
        pygame.draw.polygon(surface, colour, points)        # Draw the background
        pygame.draw.polygon(surface, (0, 0, 0), points, 2)  # Draw the border

        # If this is the exit tile write an "X"
        if tile.type == TileType.EXIT:
            font = pygame.font.SysFont(None, 32)
            text = font.render("X", True, (0, 0, 0))
            rect = text.get_rect(center=center)
            surface.blit(text, rect)

    # -----------------------
    # Hex math
    # -----------------------
    def hex_to_pixel(self, q, r):
        """
        Convert grid coordinates to screen pixel coordinates.

        Uses a pointy-top hex layout with odd-column offset.

        Parameters:
            q (int): Column index
            r (int): Row index

        Returns:
            tuple[int, int]: Pixel coordinates (x, y)
        """
        radius = self.size
        apothem = math.sqrt(3) / 2 * radius

        vertical_spacing = 2 * radius
        horizontal_spacing = 2 * apothem

        x = q * horizontal_spacing
        y = r * (vertical_spacing + radius)
        y = y / 2

        if r % 2 == 1:
            x += horizontal_spacing/2 # offset alternate rows

        return int(x + 100), int(y + 100)

    def hex_corners(self, center) -> list[(int,int)]:
        """
        Calculate the six corner points of a hex.

        Parameters:
            center (tuple[int, int]): Centre of the hex in pixels

        Returns:
            list[tuple[float, float]]:
                List of six (x, y) points defining the hex polygon
        """
        cx, cy = center
        corners = []

        # A hexagon has 6 corners, spaced 60 degrees apart
        for i in range(6):
            # Convert degrees to radians:
            # 60° per corner, starting rotated by -30°
            #
            # The -30° rotation aligns the hex so that:
            # - A point faces directly upward (pointy-top hex)
            # - Flat edges are on the left and right
            #
            # Without -30°, the hex would be flat-top instead.
            angle = math.pi / 180 * (60 * i - 30)

            # Using polar coordinates:
            # self.size is the radius (centre → corner)
            x = cx + self.size * math.cos(angle)
            y = cy + self.size * math.sin(angle)

            corners.append((x, y))
            
        return corners

    def pixel_to_hex(self, pos):
        """
        Convert a pixel position to approximate grid coordinates.

        This method is intentionally approximate and relies on rounding.
        It is suitable for demos and interaction, not precise math.

        Parameters:
            pos (tuple[int, int]): Pixel position (x, y)

        Returns:
            tuple[int, int]: Approximated grid coordinate (q, r)
        """
        mx, my = pos
        mx -= 100
        my -= 100

        q = (2/3 * mx) / self.size
        r = (-1/3 * mx + math.sqrt(3)/3 * my) / self.size
        return round(q), round(r)
