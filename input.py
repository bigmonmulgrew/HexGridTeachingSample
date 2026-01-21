import pygame
from grid import TileType


class InputHandler:
    def __init__(self, grid):
        self.grid = grid
        self.selected_type = TileType.PLAYER
        self.drag_start = None

    def handle_event(self, event):
        if event.type == pygame.KEYDOWN:
            self.handle_key(event.key)

        if event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
            self.drag_start = self.grid.pixel_to_hex(event.pos)
            self.apply_tile(self.drag_start)

        if event.type == pygame.MOUSEBUTTONUP and event.button == 1:
            if self.drag_start:
                drag_end = self.grid.pixel_to_hex(event.pos)
                if drag_end != self.drag_start:
                    self.grid.swap_tiles(self.drag_start, drag_end)
            self.drag_start = None

    def handle_key(self, key):
        if key == pygame.K_1:
            self.selected_type = TileType.PLAYER
        elif key == pygame.K_2:
            self.selected_type = TileType.ENEMY
        elif key == pygame.K_3:
            self.selected_type = TileType.OBSTACLE
        elif key == pygame.K_4:
            self.selected_type = TileType.EXIT

    def apply_tile(self, coords):
        q, r = coords
        tile = self.grid.get_tile(q, r)
        if not tile:
            return

        if self.selected_type == TileType.EXIT:
            if tile.type == TileType.EXIT:
                tile.type = TileType.EMPTY
            else:
                self.grid.set_tile(q, r, TileType.EXIT)
        else:
            tile.type = (
                TileType.EMPTY if tile.type == self.selected_type
                else self.selected_type
            )

    def draw_ui(self, surface):
        font = pygame.font.SysFont(None, 24)
        text = f"Selected: {self.selected_type.name} (1-4)"
        label = font.render(text, True, (255, 255, 255))
        surface.blit(label, (10, 10))
