# Hex Grid Demo (Python)

This project is a **simple hex‑grid editor demo** intended for teaching purposes.

Students interact with a hex grid using only:

* Number keys **1–4** to select a tile type
* **Left‑click** to toggle a tile
* **Click + drag** to swap tiles

Most of the logic is intentionally hidden inside `grid.py` and `input.py`. Students are expected to mainly work in **`main.py`**, treating the other files as external libraries.

---

## 1. Prerequisites

You will need:

* **Python 3.10+** (3.11 recommended)
* A terminal (PowerShell, Command Prompt, or Bash, inside visual studio code is fine.)

To check your Python version:

```bash
python --version
```

---

## 2. Create a Virtual Environment (venv)

It is strongly recommended to use a virtual environment for all Python projects.

### Windows

```bash
python -m venv venv
venv\Scripts\activate
```

### macOS / Linux

```bash
python3 -m venv venv
source venv/bin/activate
```

When activated, your terminal prompt should change to show `(venv)`.

---

## 3. Install Requirements

All required libraries are listed in `requirements.txt`.

Install them using:

```bash
pip install -r requirements.txt
```

If `pip` is not recognised, try:

```bash
python -m pip install -r requirements.txt
```

---

## 4. Running the Project

Once the virtual environment is active and dependencies are installed, run:

```bash
python main.py
```

A window will open displaying the hex grid.

---

## 5. Controls

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

## 6. Project Structure

```text
hex_demo/
│
├── main.py        # Main game loop and configuration (student-facing)
├── grid.py        # Hex grid logic and rendering (abstracted)
├── input.py       # Input handling and UI (abstracted)
├── requirements.txt
└── README.md
```

### main.py

This file acts as a **template for a grid‑based game**.

Students are expected to:

* Change grid size
* Insert gameplay logic
* Add rules and validation

They are **not expected** to modify `grid.py` or `input.py`.

---

## 7. Teaching Intent

This demo is designed to:

* Encourage separation of concerns
* Demonstrate grid‑based interaction
* Avoid overwhelming students with rendering or math details
* Mirror how engines like Unity hide complexity behind systems

It is deliberately minimal and intended to be extended.

---

## 8. Common Issues

**The window opens and immediately closes**

* Make sure you are running from a terminal, not double‑clicking the file

**Nothing installs**

* Ensure the virtual environment is activated
* Check that `pip` is installing into the venv

---

## 9. Next Steps (Optional Extensions)

Ideas for further development:

* Validate the grid before starting a game
* Add pathfinding
* Add hover highlighting
* Save / load grid layouts
* Port the logic to Unity

---

If something goes wrong, the first step is always:

```bash
python --version
pip --version
```

Make sure both point to the virtual environment.
