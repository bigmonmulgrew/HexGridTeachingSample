using Raylib_cs;
using System.Numerics;

/// <summary>
/// Application entry point.
/// Mirrors the structure of main.py in the pygame version.
/// </summary>
class Program
{
    // =========================
    // CONFIG (students edit)
    // =========================
    const int GRID_WIDTH = 16;
    const int GRID_HEIGHT = 10;
    const int HEX_SIZE = 40;

    const int WINDOW_WIDTH = 1200;
    const int WINDOW_HEIGHT = 800;
    const int FPS = 60;

    static void Main()
    {
        // --- Window setup ---
        Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Hex Grid Demo (Raylib)");
        Raylib.SetTargetFPS(FPS);

        // --- Core objects ---
        HexGrid grid = new HexGrid(GRID_WIDTH, GRID_HEIGHT, HEX_SIZE);
        InputHandler input = new InputHandler(grid);

        // --- Main loop ---
        while (!Raylib.WindowShouldClose())
        {
            // -----------------
            // INPUT
            // -----------------
            input.Update();

            // -----------------
            // DRAW
            // -----------------
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.DarkGray);

            grid.Draw();
            input.DrawUI();

            Raylib.EndDrawing();
        }

        // --- Cleanup ---
        Raylib.CloseWindow();
    }
}
