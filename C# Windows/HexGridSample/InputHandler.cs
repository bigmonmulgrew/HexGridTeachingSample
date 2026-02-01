using System;

class InputHandler
{
    private readonly HexGrid grid;
    private TileType selectedType = TileType.PLAYER;

    public InputHandler(HexGrid grid)
    {
        this.grid = grid;
    }

    public bool HandleKey(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.D1: selectedType = TileType.PLAYER; break;
            case ConsoleKey.D2: selectedType = TileType.ENEMY; break;
            case ConsoleKey.D3: selectedType = TileType.OBSTACLE; break;
            case ConsoleKey.D4: selectedType = TileType.EXIT; break;

            case ConsoleKey.T:
                ApplyTile(ReadCoord());
                break;

            case ConsoleKey.S:
                Console.WriteLine("Swap A:");
                var a = ReadCoord();
                Console.WriteLine("Swap B:");
                var b = ReadCoord();
                grid.SwapTiles(a, b);
                break;

            case ConsoleKey.Escape:
                return false;
        }
        return true;
    }

    private (int q, int r) ReadCoord()
    {
        Console.Write("q: ");
        int q = int.Parse(Console.ReadLine());
        Console.Write("r: ");
        int r = int.Parse(Console.ReadLine());
        return (q, r);
    }

    private void ApplyTile((int q, int r) c)
    {
        var tile = grid.GetTile(c.q, c.r);
        if (tile == null) return;

        if (selectedType == TileType.EXIT)
        {
            if (tile.Type == TileType.EXIT)
                tile.Type = TileType.EMPTY;
            else
                grid.SetTile(c.q, c.r, TileType.EXIT);
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
        Console.WriteLine();
        Console.WriteLine($"Selected: {selectedType} (1–4)");
        Console.WriteLine("T = place tile");
        Console.WriteLine("S = swap tiles");
        Console.WriteLine("ESC = quit");
    }
}
