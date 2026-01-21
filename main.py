import pygame
from grid import HexGrid, TileType
from input import InputHandler

# =========================
# CONFIG (students edit)
# =========================
GRID_WIDTH = 16      # How many hex tiles wide
GRID_HEIGHT = 10     # How many rows wide
HEX_SIZE = 40       # Size of a hex tile. Radius to a corner

WINDOW_WIDTH = 1200 
WINDOW_HEIGHT = 800
FPS = 60


def main() -> None:     # Explicit return typing (Optional)
    # Basic setup
    pygame.init()
    
    screen = pygame.display.set_mode((WINDOW_WIDTH, WINDOW_HEIGHT))

    pygame.display.set_caption("Hex Grid Demo")
    clock = pygame.time.Clock()

    # Draw the hex grid
    grid = HexGrid(GRID_WIDTH, GRID_HEIGHT, HEX_SIZE)
    input_handler = InputHandler(grid)

    # We use a Try block so that when we hit the keyboard CTRL+X we can exit cleanly.
    try:
        running = True
        # Game update loop
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


    except KeyboardInterrupt:
            # Ctrl+C pressed — exit cleanly
            print("\nExiting (KeyboardInterrupt)")

    finally:
        # Exit application
        pygame.quit()


# Application starts here, but only if main.py is run directly.
if __name__ == "__main__":
    main()
