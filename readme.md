# Cross-Framework Hex Grid Teaching Sample

This repository contains a set of small teaching projects based on the same
core design problem.

The original exercise is a **paper-based design task**, recreated in multiple
frameworks and engines to allow direct comparison of structure, workflow, and
design philosophy.

---

## Purpose

These projects are intended as **teaching samples**, not complete games.

They are designed to:
- Simulate a pen-and-paper game design exercise
- Demonstrate how the same logic can be implemented across different frameworks
- Highlight similarities and differences in application structure
- Encourage discussion around separation of concerns and system design

Each implementation focuses on **clarity and structure**, rather than feature
completeness.

---

## Implementations

Currently included (or planned):

- **Python / Pygame**
- **Unity / C#**
- **C# Standalone** *(coming soon)*

Each framework has its **own folder**, containing:
- A self-contained project
- A dedicated `README.md` with setup instructions
- Notes specific to that framework or engine

---

## Shared Design Philosophy

All implementations follow the same high-level approach:

- A **main loop** or entry point that controls application flow
- External classes/modules that encapsulate:
  - Grid logic
  - Input handling
- Complex or low-level functionality is deliberately hidden behind clear APIs

Students are encouraged to **use** these systems rather than modify them.

---

## Scope and Limitations

These projects are intentionally minimal.

They:
- Do not implement full gameplay systems
- Do not include win/lose conditions
- Do not simulate a complete game loop beyond interaction

Instead, they replicate a **design exercise** intended to provoke thought about:
- Game mechanics
- Rules and constraints
- System extensibility

Projects may optionally be extended with additional functionality as part of
further exercises.

---

## Intended Use

The primary goal of this repository is to act as a **comparison model** for
simple interactive applications across different frameworks.

It can be used for:
- Teaching
- Demonstrations
- Framework comparison
- Discussion-based learning
