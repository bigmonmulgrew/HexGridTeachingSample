import pygame
from grid import HexGrid, TileType
from input import InputHandler

# =========================
# CONFIG (students edit)
# =========================
GRID_WIDTH = 8
GRID_HEIGHT = 6
HEX_SIZE = 40

WINDOW_WIDTH = 800
WINDOW_HEIGHT = 600
FPS = 60


def main():
    pygame.init()
    screen = pygame.display.set_mode((WINDOW_WIDTH, WINDOW_HEIGHT))
    pygame.display.set_caption("Hex Grid Demo")
    clock = pygame.time.Clock()

    grid = HexGrid(GRID_WIDTH, GRID_HEIGHT, HEX_SIZE)
    input_handler = InputHandler(grid)

    running = True
    while running:
        clock.tick(FPS)

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

            # Input is deliberately simple
            input_handler.handle_event(event)

        screen.fill((30, 30, 30))
        grid.draw(screen)
        input_handler.draw_ui(screen)

        pygame.display.flip()

    pygame.quit()


if __name__ == "__main__":
    main()
