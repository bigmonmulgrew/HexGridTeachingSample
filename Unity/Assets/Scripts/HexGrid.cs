using Unity.Mathematics.Geometry;
using UnityEngine;

/// <summary>
/// Manages a 2D hexagonal grid.
///
/// Unity equivalent of grid.py (logic only).
/// Visuals are delegated to GridTile instances.
/// Rendering is handled by Unity automatically.
/// </summary>
public class HexGrid : MonoBehaviour
{
    [Header("Tile Prefab")]
    [SerializeField] private GridTile tilePrefab;

    private int width;
    private int height;
    private float hexRadius;

    private Vector3 gridOrigin;

    private GridTile[,] tiles;

    // In python we work in pixels directly.
    // In Unity we work in world units, so we need to convert
    // This is not reliable if not using an orthographic camera
    private float Radius => hexRadius * (2f * Camera.main.orthographicSize) / Screen.height;

    // Initialisation
    // Call from game manager Start()
    public void Initialise(int gridWidth, int gridHeight, float radius)
    {
        width = gridWidth;
        height = gridHeight;
        hexRadius = radius;

        tiles = new GridTile[width, height];

        Vector2 gridSize = GetGridWorldSize();
        gridOrigin = new Vector3(
            -gridSize.x * 0.5f,
             gridSize.y * 0.5f,
             0f
        );

        BuildGrid();
    }

    private void BuildGrid()
    {
        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Vector3 worldPos = GridToWorld(q, r);
                GridTile tile = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);

                tile.name = $"Tile ({q}, {r})";

                tile.Initialise(q, r, TileType.Empty);

                tiles[q, r] = tile;
            }
        }
    }

    // =========================
    // Tile access

    public GridTile GetTile(int q, int r)
    {
        if (q < 0 || q >= width || r < 0 || r >= height)
            return null;

        return tiles[q, r];
    }

    public void SetTile(Vector2Int tileCoords, TileType type)
    {
        int q = tileCoords.x;
        int r = tileCoords.y;

        GridTile tile = GetTile(q, r);
        if (tile == null)
        {
            return;
        }


        if (type == TileType.Exit)
        {
            ClearExit();
        }

        if (tile.Type == TileType.Exit)
        {
            tile.SetType(TileType.Empty);

        }

        tile.SetType(type);
    }

    public void SwapTiles(Vector2Int a, Vector2Int b)
    {
        GridTile tileA = GetTile(a.x, a.y);
        GridTile tileB = GetTile(b.x, b.y);

        if (tileA == null || tileB == null)
            return;

        if (tileA.Type == TileType.Exit || tileB.Type == TileType.Exit)
            return;

        TileType temp = tileA.Type;
        tileA.SetType(tileB.Type);
        tileB.SetType(temp);
    }

    private void ClearExit()
    {
        foreach (GridTile tile in tiles)
        {
            if (tile.Type == TileType.Exit)
                tile.SetType(TileType.Empty);
        }
    }

    
    // Coordinate conversion
    public Vector3 GridToWorld(int q, int r)
    {
        float apothem = Mathf.Sqrt(3f) / 2f * Radius;

        float horizontalSpacing = 2f * apothem;
        float verticalSpacing = 2f * Radius;

        float x = q * horizontalSpacing;
        float y = r * (verticalSpacing + Radius) * 0.5f;

        if (r % 2 == 1)
            x += horizontalSpacing * 0.5f;

        return gridOrigin + new Vector3(x, -y, 0f);
    }
    public Vector2Int ScreenToWorld(Vector2 screenPosition)
    {
        // 3D ray test
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridTile tile = hit.collider.GetComponent<GridTile>();
            if (tile != null)
            {
                return tile.GridPosition;
            }
        }


        // 2D ray test as fallback
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, 0f)
        );

        RaycastHit2D hit2D = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit2D.collider != null)
        {
            GridTile tile = hit2D.collider.GetComponent<GridTile>();
            if (tile != null)
            {
                return tile.GridPosition;
            }
        }

        // No tile found at this screen position
        return new Vector2Int(-1, -1); // Invalid coordinates
    }
    private Vector2 GetGridWorldSize()
    {
        float apothem = Mathf.Sqrt(3f) / 2f * Radius;

        float horizontalSpacing = 2f * apothem;
        float verticalSpacing = 2f * Radius;

        float widthWorld =
            (width - 1) * horizontalSpacing + horizontalSpacing * 0.5f;

        float heightWorld =
            (height - 1) * (verticalSpacing + Radius) * 0.5f;

        return new Vector2(widthWorld, heightWorld);
    }
}

