using UnityEngine;
using TMPro;

/// <summary>
/// Represents a single tile in the hex grid.
///
/// Responsibilities:
/// - Store its own coordinates (q, r)
/// - Store its current TileType
/// - Update its own visuals when the type changes
///
/// This mirrors HexTile + draw_hex behaviour from the Python version,
/// but the rendering is handled by Unity components on the prefab.
/// </summary>
public class GridTile : MonoBehaviour
{
    // Tile Identity
    private int q;
    private int r;
    
    private SpriteRenderer spriteRenderer;   // 2D prefab
    private MeshRenderer meshRenderer;       // 3D prefab
    private TextMeshPro textLabel;           // Used for Exit "X"

    private Color emptyColour = Color.white;    // User predefined colours
    private Color playerColour = Color.green;
    private Color enemyColour = Color.red;
    private Color obstacleColour = new Color(0.59f, 0.29f, 0.0f); // Brown This is rgb(150, 75, 0) Colors are deifined from 0 to 1 in Unity

    /// <summary>
    /// Current type of this tile.
    /// HexGrid reads this when swapping and enforcing rules.
    /// </summary>
    // This is a property with a private setter to prevent external modification.
    public TileType Type { get; private set; } = TileType.Empty;

    /// <summary>
    /// Grid coordinate of this tile (q = column, r = row).
    /// </summary>
    // Read-only property to expose coordinates.
    // => is shorthand for a get-only property. Similar to a lambda expression.
    public Vector2Int GridPosition => new Vector2Int(q, r);

    /// <summary>
    /// Initializes component references and disables the text label by default.
    /// Run once when the prefab is instantiated.
    /// Runs before Initialise().
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
        textLabel = GetComponentInChildren<TextMeshPro>();
        textLabel.enabled = false; // Disable by default; only enable when needed
    }

    /// <summary>
    /// Called once after the tile prefab is instantiated.
    /// Sets coordinates and initial tile type.
    /// </summary>
    public void Initialise(int qCoord, int rCoord, TileType initialType)
    {
        q = qCoord;
        r = rCoord;
        SetType(initialType);
    }

    /// <summary>
    /// Updates the logical type and refreshes visuals.
    /// </summary>
    public void SetType(TileType newType)
    {
        Type = newType;
        ApplyVisuals();
    }

    /// <summary>
    /// Set colour/material and label based on the current tile type.
    /// Prefab decides whether this is 2D (SpriteRenderer) or 3D (MeshRenderer).
    /// </summary>
    private void ApplyVisuals()
    {
        Color colour = GetColourForType(Type);

        // Apply to 2D or 3D renderer (or both, if assigned)
        if (spriteRenderer != null)
        { 
            spriteRenderer.color = colour; 
        }

        if (meshRenderer != null)
        {
            // NOTE: This modifies the instance material at runtime.
            // For an MVP, this is acceptable and simple.
            meshRenderer.material.color = colour;
        }

        // Exit marker
        if (textLabel != null)
        {
            if(Type == TileType.Exit)
            {
                textLabel.enabled = true;
            }
            else
            {
                textLabel.enabled = false;
            }
        }
    }

    private Color GetColourForType(TileType type)
    {
        switch (type)
        {
            case TileType.Player: return playerColour;
            case TileType.Enemy: return enemyColour;
            case TileType.Obstacle: return obstacleColour;
            case TileType.Exit: return emptyColour;   // background remains "empty", label shows X
            case TileType.Empty:
            default: return emptyColour;
        }
    }

}
