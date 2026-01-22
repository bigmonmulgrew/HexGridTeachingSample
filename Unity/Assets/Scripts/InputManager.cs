using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interprets raw Unity input into gameplay intent.
/// 
/// This mirrors the role of input.py in the Python version.
/// Unity already delivers input events, so we only need to
/// poll input state and resolve intent (click vs drag, etc.).
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction select1;
    [SerializeField] private InputAction select2;
    [SerializeField] private InputAction select3;
    [SerializeField] private InputAction select4;

    [SerializeField] private InputAction primaryClick;
    [SerializeField] private InputAction pointerPosition;

    private Vector2 dragStartPixel;
    private Vector2Int dragStartGrid;
    private const float DRAG_THRESHOLD = 10f;

    private GameManager gameManager;
    private HexGrid grid;
    private TileType selectedType = TileType.Player;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        grid = FindFirstObjectByType<HexGrid>();
    }

    private void OnEnable()
    {
        select1.Enable();
        select2.Enable();
        select3.Enable();
        select4.Enable();
        primaryClick.Enable();
        pointerPosition.Enable();
    }

    private void OnDisable()
    {
        select1.Disable();
        select2.Disable();
        select3.Disable();
        select4.Disable();
        primaryClick.Disable();
        pointerPosition.Disable();
    }

    /// <summary>
    /// Equivalent of unity Update() for input handling.
    /// 
    /// Must be called explicitly, typically from GameManager.Update().
    /// </summary>
    public void Tick()
    {
        // Key selection (Python: if keydown)
        HandleKey();

        // Mouse down
        if (primaryClick.WasPressedThisFrame())
        {
            dragStartPixel = pointerPosition.ReadValue<Vector2>();
            dragStartGrid = grid.ScreenToWorld(dragStartPixel);
            if (dragStartGrid == new Vector2Int(-1, -1))
            {
                Debug.Log("Start grid tile invalid");
                return;
            }
        }

        // Mouse up
        if (primaryClick.WasReleasedThisFrame())
        {
            Vector2 dragEndPixel = pointerPosition.ReadValue<Vector2>();
            Vector2Int dragEndHex = grid.ScreenToWorld(dragEndPixel);

            if (dragEndHex == new Vector2Int(-1, -1))
            {
                Debug.Log("Released outside grid or grid tile invalid");
                return;
            }
                

            float distance = Vector2.Distance(dragStartPixel, dragEndPixel);

            if (distance < DRAG_THRESHOLD)
            {
                grid.SetTile(dragStartGrid, selectedType);
            }
            else
            {
                grid.SwapTiles(dragStartGrid, dragEndHex);
            }
        }
    }

    private void HandleKey()
    {
        // Key selection (Python: if keydown)
        if (select1.WasPressedThisFrame()) selectedType = TileType.Player;
        if (select2.WasPressedThisFrame()) selectedType = TileType.Enemy;
        if (select3.WasPressedThisFrame()) selectedType = TileType.Obstacle;
        if (select4.WasPressedThisFrame()) selectedType = TileType.Exit;
    }
}
