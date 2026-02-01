using Raylib_cs;
using System.Numerics;

/// <summary>
/// Handles keyboard and mouse input.
/// Mirrors input.py behaviour.
/// </summary>
class InputHandler
{
    private readonly HexGrid grid;
    private TileType selectedType = TileType.PLAYER;

    private (int q, int r)? dragStart;
    private Vector2 dragStartPixel;
    private const float DragThreshold = 10f;

    public InputHandler(HexGrid grid)
    {
        this.grid = grid;
    }

    public void Update()
    {
        // --- Key input ---
        if (Raylib.IsKeyPressed(KeyboardKey.One)) selectedType = TileType.PLAYER;
        if (Raylib.IsKeyPressed(KeyboardKey.Two)) selectedType = TileType.ENEMY;
        if (Raylib.IsKeyPressed(KeyboardKey.Three)) selectedType = TileType.OBSTACLE;
        if (Raylib.IsKeyPressed(KeyboardKey.Four)) selectedType = TileType.EXIT;

        // --- Mouse input ---
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            dragStartPixel = Raylib.GetMousePosition();
            dragStart = grid.PixelToHex(dragStartPixel);
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.Left) && dragStart.HasValue)
        {
            Vector2 endPixel = Raylib.GetMousePosition();
            var endHex = grid.PixelToHex(endPixel);

            float distance = Vector2.Distance(dragStartPixel, endPixel);

            if (distance < DragThreshold)
            {
                ApplyTile(dragStart.Value);
            }
            else if (endHex != dragStart.Value)
            { 
                grid.SwapTiles(dragStart.Value, endHex); 
            }

            dragStart = null;
        }
    }

    private void ApplyTile((int q, int r) c)
    {
        var tile = grid.GetTile(c.q, c.r);
        if (tile == null) return;

        if (selectedType == TileType.EXIT)
        {
            if (tile.Type == TileType.EXIT)
            {
                tile.Type = TileType.EMPTY;
            }
            else
            {
                grid.SetTile(c.q, c.r, TileType.EXIT);
            }
        }
        else
        {
            tile.Type =
                tile.Type == selectedType
                ? TileType.EMPTY
                : selectedType;
        }
    }

    public void DrawUI()
    {
        Raylib.DrawText(
            $"Selected: {selectedType} (1–4)",
            10, 10, 20, Color.White
        );

        Raylib.DrawText(
            "Left click to place, drag to swap",
            10, Raylib.GetScreenHeight() - 30, 20, Color.White
        );
    }
}
