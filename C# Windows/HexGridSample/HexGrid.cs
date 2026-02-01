using Raylib_cs;
using System;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
/// All supported tile types.
/// <br/><br/>
/// Used to identify the logical role of a tile in the grid.<br/>
/// Rendering and behaviour are based on this value.<br/>
/// Matches grid.py TileType enum.
/// </summary>
enum TileType
{
    EMPTY,
    PLAYER,
    ENEMY,
    OBSTACLE,
    EXIT
}



/// <summary>
/// Simple data container for a hex tile.
/// <br/><br/>
/// This class intentionally contains very little logic.<br/>
/// It acts as a simple data container.
/// </summary>
class HexTile
{
    public TileType Type = TileType.EMPTY;
}

/// <summary>
/// Manages a 2D hex grid:<br/>
/// - Tile storage<br/>
/// - Grid rules<br/>
/// - Hex math<br/>
/// - Drawing<br/>
/// </summary>
class HexGrid
{
    Dictionary<TileType, Color> TILE_COLOURS = new()
{
    { TileType.EMPTY, Color.RayWhite },
    { TileType.PLAYER, Color.Red },
    { TileType.ENEMY, Color.Green },
    { TileType.OBSTACLE, Color.Brown },
    { TileType.EXIT, Color.Yellow }
};

    private readonly int width;
    private readonly int height;
    private readonly int size;
    private readonly HexTile[,] tiles;

    public HexGrid(int width, int height, int size)
    {
        this.width = width;
        this.height = height;
        this.size = size;

        tiles = new HexTile[width, height];
        for (int q = 0; q < width; q++)
            for (int r = 0; r < height; r++)
                tiles[q, r] = new HexTile();
    }

    // -----------------------
    // Tile logic
    // -----------------------
    public HexTile GetTile(int q, int r)
    {
        if (q < 0 || q >= width || r < 0 || r >= height)
        {
            return null;
        }
        return tiles[q, r];
    }

    public void SetTile(int q, int r, TileType type)
    {
        HexTile tile = GetTile(q, r);
        if (tile == null) return;

        if (type == TileType.EXIT)
        {
            ClearExit();
        }

        if (tile.Type == TileType.EXIT)
        {
            tile.Type = TileType.EMPTY;
        }

        tile.Type = type;
    }

    private void ClearExit()
    {
        foreach (HexTile tile in tiles)
        {
            if (tile.Type == TileType.EXIT)
            {
                tile.Type = TileType.EMPTY;
            }
                
        }
            
    }

    public void SwapTiles(Vector2IntQR start, Vector2IntQR end)
    {
        // You can use var here, but it is bad practice.
        // If you know the type use it explicitly. If not then you should probably rethink your code.
        // This is left commented to illustrate the point.
        // It is very rarely beneficial to use var, or it's equivalents in any statically typed languages.
        //var tileA = GetTile(start.q, start.r);    

        HexTile tileA = GetTile(start.q, start.r);
        HexTile tileB = GetTile(end.q, end.r);

        if (tileA == null || tileB == null) return; // Exit early if no tile found

        if (tileA.Type == TileType.EXIT || tileB.Type == TileType.EXIT) return; // Dont' swap exits

        (tileA.Type, tileB.Type) = (tileB.Type, tileA.Type);
    }

    // -----------------------
    // Drawing
    // -----------------------
    public void Draw()
    {
        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Vector2 center = HexToPixel(q, r);
                DrawHex(center, tiles[q, r]);
            }
        }
    }

    private void DrawHex(Vector2 center, HexTile tile)
    {
        Vector2[] corners = HexCorners(center);
        Color colour = TILE_COLOURS[tile.Type];

        // Fan layout: centre + all outer points + repeat first outer point
        Vector2[] fan = new Vector2[3];

        Raylib.DrawPoly(center, 6, size, 30, colour);

        for (int i = 0; i < 6; i++)
        {
            Raylib.DrawLineV(corners[i], corners[(i + 1) % 6], Color.Black);
        }

        if (tile.Type == TileType.EXIT)
        {
            Raylib.DrawText("X", (int)center.X - 6, (int)center.Y - 8, 20, Color.Black);
        }
    }

    // -----------------------
    // Hex math
    // -----------------------
    private Vector2 HexToPixel(int q, int r)
    {
        float radius = size;
        float apothem = (float)(Math.Sqrt(3) / 2 * radius);

        float x = q * (apothem * 2);
        float y = r * (radius * 1.5f);

        if (r % 2 == 1)
        {
            x += apothem;
        }
            

        return new Vector2(x + 100, y + 100);
    }

    private Vector2[] HexCorners(Vector2 center)
    {
        // It would be more common to use center.X and center.Y directly but this maintains parity with the Python version.
        int cx = (int)center.X;
        int cy = (int)center.Y;

        Vector2[] corners = new Vector2[6];     // TODO these are pixel positions so Vector2Int would be more appropriate.

        // Hexagon has 6 corners, calculate each corner's position
        for (int i = 0; i < 6; i++)
        {
            // Convert degrees to radians:
            // 60° per corner, starting rotated by -30°
            //
            // The -30° rotation aligns the hex so that:
            // - A point faces directly upward (pointy-top hex)
            // - Flat edges are on the left and right
            //
            // Without -30°, the hex would be flat-top instead.
            float angle = MathF.PI / 180f * (60 * i - 30);

            // Using polar coordinates to find corner positions
            // size is the radius (distance from center to corner)
            float x = cx + size * MathF.Cos(angle);
            float y = cy + size * MathF.Sin(angle);

            corners[i] = new Vector2((int)x, (int)y);   // Dont forget to cast to int for pixel positions
        }

        return corners;
    }

    /// <summary>
    /// Convert a pixel position to approximate grid coordinates.
    /// <br/><br/>
    /// NOTE:<br/>
    ///    This is a simplified inverse mapping intended for interaction,<br/>
    ///    not for precise math or pathfinding.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2IntQR PixelToHex(Vector2 pos)
    {
        // pos -= new Vector2(100, 100);   // Vector math can be done directly but this maintains parity with the Python version.

        float mx = pos.X;
        float my = pos.Y;

        mx -= 100;
        my -= 100;

        int radius = size;
        float apothem = (float)(Math.Sqrt(3) / 2 * radius);

        float horizontalSpacing = apothem * 2;
        float verticalStep = (2 * radius + radius) / 2;

        // Approximate row
        int r = (int)MathF.Round(my / verticalStep);

        // Undo row offset
        if (r % 2 == 1)
        {
            mx = (int)(mx - horizontalSpacing / 2);
        }

        // Approximate column
        int q = (int)MathF.Round(mx / horizontalSpacing);

        return new Vector2IntQR(q, r);
    }
}
