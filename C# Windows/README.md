# Hex Grid Demo (C#)

This project is a **simple hex‑grid editor demo** intended for teaching purposes.

It is the **C# equivalent** of the Python hex‑grid sample, using a lightweight rendering setup to provide a windowed application without introducing a full game engine.

The project is **provided fully set up**:

* Open the solution in **Visual Studio**
* Build and run
* No additional installation or configuration is required

Students interact with a hex grid using only:

* Number keys **1–4** to select a tile type
* **Left‑click** to toggle a tile
* **Click + drag** to swap tiles

Most of the logic is intentionally hidden inside `HexGrid.cs` and `InputHandler.cs`. Students are expected to mainly work in **`Program.cs`**, treating the other files as external libraries.

---

## 1. Running the Project

1. Open the solution in **Visual Studio**
2. Press **Run** (or `F5`)
3. A window will open displaying the hex grid

No package installation or setup steps are required.

---

## 2. Controls

### Tile Selection

| Key | Tile Type        |
| --: | ---------------- |
|   1 | Player (Red)     |
|   2 | Enemy (Green)    |
|   3 | Obstacle (Brown) |
|   4 | Exit (X)         |

### Mouse

* **Left‑click**: Toggle the selected tile on a hex
* **Click + drag**: Swap two tiles

Notes:

* The **Exit (X)** cannot be moved by dragging
* Only **one Exit** can exist at a time

---

## 3. Project Structure

```text
HexGridDemo/
│
├── Program.cs        # Main loop and configuration (student-facing)
├── HexGrid.cs        # Hex grid logic and rendering (abstracted)
├── InputHandler.cs   # Input handling and UI (abstracted)
├── Vector2IntQR.cs   # Minimal integer coordinate type
└── README.md
```

### Program.cs

This file acts as a **template for a grid‑based application**.

Students are expected to:

* Change grid size
* Adjust configuration values
* Insert gameplay logic
* Add rules and validation

They are **not expected** to modify `HexGrid.cs` or `InputHandler.cs`.

---

## 4. Teaching Intent

This demo is designed to:

* Encourage separation of concerns
* Demonstrate grid‑based interaction
* Expose students to a simple update / draw loop
* Avoid overwhelming students with rendering or math details
* Mirror how engines like **Unity** hide complexity behind systems

It is deliberately minimal and intended to be extended.
