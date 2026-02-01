using System;

enum TileType
{
    EMPTY,
    PLAYER,
    ENEMY,
    OBSTACLE,
    EXIT
}

class HexTile
{
    public TileType Type = TileType.EMPTY;
}

class HexGrid
{
    private readonly int width;
    private readonly int height;
    private readonly HexTile[,] tiles;

    public HexGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        tiles = new HexTile[width, height];
        for (int q = 0; q < width; q++)
            for (int r = 0; r < height; r++)
                tiles[q, r] = new HexTile();
    }

    public HexTile GetTile(int q, int r)
    {
        if (q < 0 || q >= width || r < 0 || r >= height)
            return null;
        return tiles[q, r];
    }

    public void SetTile(int q, int r, TileType type)
    {
        var tile = GetTile(q, r);
        if (tile == null) return;

        if (type == TileType.EXIT)
            ClearExit();

        if (tile.Type == TileType.EXIT)
            tile.Type = TileType.EMPTY;

        tile.Type = type;
    }

    public void ClearExit()
    {
        foreach (var tile in tiles)
            if (tile.Type == TileType.EXIT)
                tile.Type = TileType.EMPTY;
    }

    public void SwapTiles((int q, int r) a, (int q, int r) b)
    {
        var ta = GetTile(a.q, a.r);
        var tb = GetTile(b.q, b.r);

        if (ta == null || tb == null) return;
        if (ta.Type == TileType.EXIT || tb.Type == TileType.EXIT) return;

        (ta.Type, tb.Type) = (tb.Type, ta.Type);
    }

    public void Draw()
    {
        for (int r = 0; r < height; r++)
        {
            if (r % 2 == 1)
                Console.Write(" ");

            for (int q = 0; q < width; q++)
            {
                Console.Write(TileChar(tiles[q, r].Type) + " ");
            }
            Console.WriteLine();
        }
    }

    private char TileChar(TileType type) => type switch
    {
        TileType.EMPTY => '.',
        TileType.PLAYER => 'P',
        TileType.ENEMY => 'E',
        TileType.OBSTACLE => '#',
        TileType.EXIT => 'X',
        _ => '?'
    };
}
