using System;
using System.Threading;

class Program
{
    // =========================
    // CONFIG (students edit)
    // =========================
    const int GRID_WIDTH = 8;
    const int GRID_HEIGHT = 6;
    const int FPS = 10;

    static void Main()
    {
        Console.CursorVisible = false;

        HexGrid grid = new HexGrid(GRID_WIDTH, GRID_HEIGHT);
        InputHandler input = new InputHandler(grid);

        bool running = true;

        try
        {
            while (running)
            {
                // ---- INPUT ----
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    running = input.HandleKey(key);
                }

                // ---- DRAW ----
                Console.Clear();
                grid.Draw();
                input.DrawUI();

                Thread.Sleep(1000 / FPS);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }
}
