import pygame
from grid import TileType


class InputHandler:
    def __init__(self, grid):
        self.grid = grid
        self.selected_type = TileType.PLAYER
        self.drag_start = None
        
        # State used to decide between click and drag
        self.drag_start_pixel = None
        self.drag_threshold = 10  # pixels

    def handle_event(self, event):
        # Input handling is split into:
        # - capture intent on mouse down
        # - resolve action on mouse up
        
        if event.type == pygame.KEYDOWN:
            self.handle_key(event.key)

        if event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
            # Start tracking a possible click or drag
            self.drag_start_pixel = event.pos
            self.drag_start_hex = self.grid.pixel_to_hex(event.pos)

        if event.type == pygame.MOUSEBUTTONUP and event.button == 1:
            if not self.drag_start_hex:
                return

            drag_end_hex = self.grid.pixel_to_hex(event.pos)

            # Measure mouse movement distance
            dx = event.pos[0] - self.drag_start_pixel[0]
            dy = event.pos[1] - self.drag_start_pixel[1]
            distance_sq = dx * dx + dy * dy

            if distance_sq < self.drag_threshold * self.drag_threshold:
                # Short movement = click
                self.apply_tile(self.drag_start_hex)
            else:
                # Larger movement = drag
                if drag_end_hex != self.drag_start_hex:
                    self.grid.swap_tiles(self.drag_start_hex, drag_end_hex)

            # Reset drag state
            self.drag_start_hex = None
            self.drag_start_pixel = None


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
        # Applies the currently selected tile type, enforcing grid rules
        
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

        # --- Top-left: current selection ---
        selected_text = f"Selected: {self.selected_type.name} (1–4)"
        selected_label = font.render(selected_text, True, (255, 255, 255))
        surface.blit(selected_label, (10, 10))

        # --- Bottom: instructions --- TODO this is a constant so should be moved to a constant.
        instructions = (
            "Use 1–4 to select tile type. "
            "Left click to set tile, drag to swap tiles."
        )

        instruction_label = font.render(instructions, True, (255, 255, 255))

        # Position at bottom with a small margin
        x = 10
        y = surface.get_height() - instruction_label.get_height() - 10

        surface.blit(instruction_label, (x, y))

