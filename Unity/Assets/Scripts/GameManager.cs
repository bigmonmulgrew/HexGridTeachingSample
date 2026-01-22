using UnityEngine;

/// <summary>
/// Hex Grid Demo (MVP)
///
/// Unity equivalent of main.py in the Python version.
///
/// Responsibilities:
/// - Hold configuration values (editable in Inspector)
/// - Act as the main application loop
/// - Orchestrate grid, execution order of input system and data updates of the grid
/// - Rendering is handled by Unity automatically
///
/// This class intentionally stays simple and readable.
/// </summary>
public class GameManager : MonoBehaviour
{
    
    // CONFIG (students edit)
    [Header("Grid Configuration")]
    [SerializeField] private int gridWidth = 16;     // How many hex tiles wide
    [SerializeField] private int gridHeight = 10;    // How many rows tall
    [Tooltip("Size of each hex tile in pixels (distance from centre to corner)")]
    [SerializeField] private float hexSize = 40f;    // Radius: centre ? corner
    [SerializeField] private Vector2 referenceResolution = new Vector2(1280, 720);

    // Python 
    // FPS = 60
    // No need to limit frame rate in Unity. Unity has two frame rates:
    // - Update() frame rate (default: uncapped)
    // - FixedUpdate() frame rate (default: 50 FPS, used for physics)
    // Neither of these is strictly necessary to limit for this demo.

    // References
    // In python this would be imported modules
    // For unity, these are other components or game objects
    // These will be assigned in Start()
    private InputManager inputManager;
    private HexGrid grid;


    // Start is called before the first frame update 
    private void Start()
    {
        // Unity splits def main() into multiple methods.
        // Start() is called once at the beginning, and represents our one off setup phase.

        // Get references to other components, either on this GameObject
        // or elsewhere in the scene.
        // Searching the whole scene is not optimal for performance
        // but acceptable for this small demo.
        // Never do this in Update() or frequently called methods!
        inputManager = GetComponent<InputManager>();        // Assumes InputManager is on the same GameObject
        grid         = FindFirstObjectByType<HexGrid>();    // Finds HexGrid in the scene

        grid.Initialise(gridWidth, gridHeight, hexSize);
    }

    private void Update()
    {
        // This is the equivalent of our python main loop
        // while running:

        // This is here to follow the python structure,
        // but inputManager.Update() would be called automatically
        // In unity every game component has its own Update() method.
        // This Update method does nothing else and could be removed if following normal Unity conventions.
        if (inputManager != null)
        {
            inputManager.Tick();
        }

        // No need to clear the screen; Unity handles that automatically.
        // No need to draw the grid, Unity handles rendering automatically.
        // No need to draw the UI, Unity handles that automatically.
    }

}
