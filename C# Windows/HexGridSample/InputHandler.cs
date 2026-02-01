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

    private Vector2IntQR? dragStartHex = null;
    private Vector2 dragStartPixel;             // TODO we are uisng Vector 2 for pixel position, which is always int. Vector2Int would be more appropriate.
    private const float DRAG_THRESHOLD = 10f;
    
    public InputHandler(HexGrid grid)
    {
        this.grid = grid;
    }

    /// <summary>
    /// Handles keyboard and mouse input to select tile types, apply tiles, or swap tiles on the grid.
    /// Since this runs on every frame it is more commonly called Update or Tick in other frameworks.
    /// This name is kept to mirror the Python version.
    /// </summary>
    public void HandleEvent()
    {
        HandleKey();

        
        // --- Mouse input ---
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            dragStartPixel = Raylib.GetMousePosition();
            dragStartHex = grid.PixelToHex(dragStartPixel);
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            if (dragStartHex == null) return;   // Early exit if no drag start recorded

            Vector2 endPixel = Raylib.GetMousePosition();
            Vector2IntQR endHex = grid.PixelToHex(endPixel);

            // In python we used distance squared to guarantee a poisitive value.
            // Here we can use Vector2.Distance which returns a positive float directly.
            float distance = Vector2.Distance(dragStartPixel, endPixel);

            if (distance < DRAG_THRESHOLD)
            {
                // Short drag, treat as click
                ApplyTile(dragStartHex);
            }
            else
            {
                // Long drag, treat as swap
                if (endHex != dragStartHex)
                {
                    grid.SwapTiles(dragStartHex, endHex);
                }
                    
            }

            dragStartHex = null;
        }
    }

    private void HandleKey()
    {
        // In the pygame version this is event driven, here we poll input state each frame.
        // --- Key input ---
        if (Raylib.IsKeyPressed(KeyboardKey.One))   selectedType = TileType.PLAYER;
        if (Raylib.IsKeyPressed(KeyboardKey.Two))   selectedType = TileType.ENEMY;
        if (Raylib.IsKeyPressed(KeyboardKey.Three)) selectedType = TileType.OBSTACLE;
        if (Raylib.IsKeyPressed(KeyboardKey.Four))  selectedType = TileType.EXIT;
    }
    private void ApplyTile(Vector2IntQR coords)
    {
        (int q, int r) = coords;

        HexTile tile = grid.GetTile(q, r);
        if (tile == null) return;

        if (selectedType == TileType.EXIT)
        {
            if (tile.Type == TileType.EXIT)
            {
                tile.Type = TileType.EMPTY;
            }
            else
            {
                grid.SetTile(q, r, TileType.EXIT);
            }
        }
        else
        {
            tile.Type = tile.Type == selectedType ? TileType.EMPTY : selectedType;
        }
    }

    public void DrawUI()
    {
        // TODO : This one contains magic numbers and these should be replaced with constants or variables
        string selectedMsg = $"Selected: {selectedType} (1-4)";
        Raylib.DrawText(
            selectedMsg,    // Message string
            10,             // X position (Horizontal)
            10,             // Y position (Vertical)
            20,             // Font size
            Color.White     // Colour
        );

        // This example uses local variables to demonstrate parameter usage
        // TODO : These are still magic numbers, consider replacing them with constants or variables
        int x = 10;
        int y = Raylib.GetScreenHeight() - 30;
        int fontSize = 20;
        string instructions = $"Use 1-4 to select tile type. Left click to set tile, drag to swap tiles.";
        Raylib.DrawText(
            instructions,   // Message string
            x,              // X position (Horizontal)
            y,              // Y position (Vertical)
            fontSize,       // Font size
            Color.White     // Colour
        );
    }
}
