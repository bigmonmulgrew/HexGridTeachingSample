using Raylib_cs;
using System;
using System.Numerics;

/// <summary>
/// All supported tile types.
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
/// </summary>
class HexTile
{
    public TileType Type = TileType.EMPTY;
}

/// <summary>
/// Manages a 2D hex grid:
/// - Tile storage
/// - Grid rules
/// - Hex math
/// - Drawing
/// </summary>
class HexGrid
{
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
        var tile = GetTile(q, r);
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
        foreach (var tile in tiles)
        {
            if (tile.Type == TileType.EXIT)
            {
                tile.Type = TileType.EMPTY;
            }
                
        }
            
    }

    public void SwapTiles((int q, int r) a, (int q, int r) b)
    {
        var ta = GetTile(a.q, a.r);
        var tb = GetTile(b.q, b.r);

        if (ta == null || tb == null) return;
        if (ta.Type == TileType.EXIT || tb.Type == TileType.EXIT) return;

        (ta.Type, tb.Type) = (tb.Type, ta.Type);
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
        Color colour = tile.Type switch
        {
            TileType.PLAYER => Color.Red,
            TileType.ENEMY => Color.Green,
            TileType.OBSTACLE => Color.Brown,
            _ => Color.RayWhite
        };

        Vector2[] points = HexCorners(center);

        Raylib.DrawTriangleFan(points, points.Length, colour);

        for (int i = 0; i < 6; i++)
        {
            Raylib.DrawLineV(points[i], points[(i + 1) % 6], Color.Black);
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
        Vector2[] corners = new Vector2[6];

        for (int i = 0; i < 6; i++)
        {
            float angle = MathF.PI / 180f * (60 * i - 30);
            corners[i] = new Vector2(
                center.X + size * MathF.Cos(angle),
                center.Y + size * MathF.Sin(angle)
            );
        }

        return corners;
    }

    public (int q, int r) PixelToHex(Vector2 pos)
    {
        pos -= new Vector2(100, 100);

        float radius = size;
        float apothem = (float)(Math.Sqrt(3) / 2 * radius);

        int r = (int)MathF.Round(pos.Y / (radius * 1.5f));
        if (r % 2 == 1)
        {
            pos.X -= apothem;
        }

        int q = (int)MathF.Round(pos.X / (apothem * 2));

        return (q, r);
    }
}
