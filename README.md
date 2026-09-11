# Project Fallout

A third-person Fallout-inspired game prototype built with **Godot 4.7 and C#**.

Project Fallout is a personal game-development project focused on experimenting with third-person character control, character state machines, navigation, companion behaviour, animation states, and communication between gameplay systems.

The project is currently in an experimental/prototyping stage.

## Overview

The project is built around a small playable environment containing a controllable **Player** character and an NPC named **Noel**.

The codebase uses a combination of:

* Godot scenes and nodes
* C# gameplay scripts
* CharacterBody3D-based movement
* Navigation and pathfinding
* State machines
* Signal buses
* Blackboard-style shared state
* Character statistics
* Animation state handling

The project is structured so that character behaviour is separated into individual states while the character scripts remain responsible for their underlying movement and gameplay logic.

## Technology

| Component    | Technology      |
| ------------ | --------------- |
| Engine       | Godot 4.7       |
| Language     | C#              |
| Renderer     | Forward+        |
| Project type | 3D              |
| .NET         | Godot C# / .NET |
| Resolution   | 1280 × 720      |

The project configuration uses Godot 4.7 with C# and the Forward+ renderer.

## Running the Project

Clone the repository:

```bash
git clone https://github.com/MikeElnathan/Project_Fallout.git
```

Open the project in **Godot 4.7 with .NET/C# support**.

The project uses:

```text
Project_Fallout.csproj
Project_Fallout.sln
project.godot
```

The main scene is:

```text
Main/Game_Manager.tscn
```

The game manager then handles loading the trial level used by the project.

## Controls

| Input               | Action        |
| ------------------- | ------------- |
| W                   | Move Forward  |
| S                   | Move Backward |
| A                   | Move Left     |
| D                   | Move Right    |
| Space               | Jump          |
| Shift               | Run           |
| Middle Mouse Button | Orbit Camera  |

These actions are defined in `project.godot`.

## Project Structure

```text
Project_Fallout/
│
├── Blackboard/
│   ├── Player/
│   └── Noel/
│
├── Kid/
│   ├── Noel.cs
│   ├── Noel.tscn
│   ├── StateMachineNoel.cs
│   ├── Walk_Noel.cs
│   ├── IdleNoel.cs
│   ├── wanderNoel.cs
│   ├── Sleep_Noel.cs
│   ├── Sneak_Noel.cs
│   ├── MoodManager.cs
│   ├── NoelCharacterStats.cs
│   └── SignalBus_Noel.cs
│
├── Main/
│   ├── GameManager.cs
│   └── Game_Manager.tscn
│
├── Player/
│   ├── Player.tscn
│   ├── Script/
│   │   ├── Player.cs
│   │   ├── PlayerStateMachine.cs
│   │   ├── PlayerAnimation.cs
│   │   ├── CameraArm.cs
│   │   ├── Idle.cs
│   │   ├── Walk.cs
│   │   ├── Run.cs
│   │   ├── Jump.cs
│   │   └── SignalBus.cs
│   └── Asset/
│
├── Trial/
│   ├── Trial_Level/
│   └── Prototyping Texture/
│
├── Utility_Script/
│   ├── BaseStateMachine.cs
│   ├── State.cs
│   └── CharacterStat.cs
│
├── Project_Fallout.csproj
├── Project_Fallout.sln
└── project.godot
```

The repository is currently organized into six major gameplay areas: `Main`, `Player`, `Kid`, `Blackboard`, `Trial`, and `Utility_Script`.

---

# Architecture

At the centre of the project is a state-driven character architecture.

The general relationship is:

```text
                    Game Manager
                         │
                         ▼
                    Trial Level
                    ┌────┴────┐
                    ▼         ▼
                 Player     Noel
                    │         │
                    ▼         ▼
              State Machine  State Machine
                    │         │
                    ▼         ▼
                  States     States
                    │         │
                    └────┬────┘
                         │
                  Signal / Blackboard
```

The project contains a reusable base state-machine implementation in `Utility_Script/BaseStateMachine.cs`, with character-specific state machines built on top of it.

## Main

The `Main` directory contains the game's entry point.

### GameManager

```text
Main/GameManager.cs
```

`GameManager` is responsible for the main game-level flow and loading the trial level.

The associated scene is:

```text
Main/Game_Manager.tscn
```

This scene is configured as the project's main scene.

---

# Player

The Player system is contained primarily inside:

```text
Player/
```

The main character script is:

```text
Player/Script/Player.cs
```

The Player system contains its own state machine:

```text
PlayerStateMachine.cs
```

and individual states:

```text
Idle.cs
Walk.cs
Run.cs
Jump.cs
```

Other supporting scripts include:

```text
PlayerAnimation.cs
CameraArm.cs
SignalBus.cs
```

The Player directory also contains the Player scene and character assets.

### Player State Flow

The Player's behaviour is divided into states.

Conceptually:

```text
Player
  │
  ▼
PlayerStateMachine
  │
  ├── Idle
  ├── Walk
  ├── Run
  └── Jump
```

The Player's movement implementation and its state/animation handling therefore exist as related but separate parts of the Player system.

---

# Noel

Noel is the project's NPC/companion character.

The Noel system is located in:

```text
Kid/
```

The primary character script is:

```text
Kid/Noel.cs
```

Noel's state machine is:

```text
Kid/StateMachineNoel.cs
```

Noel currently has several dedicated behaviour states:

```text
IdleNoel.cs
Walk_Noel.cs
wanderNoel.cs
Sleep_Noel.cs
Sneak_Noel.cs
```

Additional Noel systems include:

```text
MoodManager.cs
NoelCharacterStats.cs
SignalBus_Noel.cs
```

The complete Noel character is represented by:

```text
Noel.tscn
```

These files make up the NPC's movement, behaviour, state, mood, statistics, and communication systems.

### Noel Movement

Noel's physical movement is handled by:

```text
Kid/Noel.cs
```

The state machine determines Noel's current behavioural state, while the Noel character script handles the underlying character movement and navigation.

The general relationship is:

```text
StateMachineNoel
       │
       ▼
  Noel State
       │
       ▼
     Noel.cs
       │
       ▼
Navigation / Velocity
       │
       ▼
  MoveAndSlide()
```

---

# State Machine System

The reusable state-machine foundation is located in:

```text
Utility_Script/
```

with:

```text
BaseStateMachine.cs
State.cs
```

Character-specific state machines then use this common foundation.

```text
                 BaseStateMachine
                       │
             ┌─────────┴─────────┐
             ▼                   ▼
    PlayerStateMachine     StateMachineNoel
             │                   │
             ▼                   ▼
       Player States         Noel States
```

This allows the Player and Noel to maintain separate state sets while sharing the underlying state-machine structure.

---

# Blackboard System

The project contains separate blackboard directories for the Player and Noel:

```text
Blackboard/
├── Player/
└── Noel/
```

The blackboards provide shared state/data that can be accessed by the relevant character systems.

The project also registers the following Godot groups:

```text
Player
Noel
Player_Blackboard
Noel_Blackboard
StateLabel
```

These groups are defined in `project.godot`.

---

# Signal Bus

Both major characters have their own signal-bus implementation.

Player:

```text
Player/Script/SignalBus.cs
```

Noel:

```text
Kid/SignalBus_Noel.cs
```

The signal buses provide a communication mechanism between different parts of the character systems without requiring every component to directly reference every other component.

This is particularly relevant to the relationship between character states, state machines, and blackboard data.

---

# Character Statistics

Shared character-stat functionality is located in:

```text
Utility_Script/CharacterStat.cs
```

Noel has a character-specific statistics implementation:

```text
Kid/NoelCharacterStats.cs
```

This separates general character-stat functionality from Noel-specific data.

---

# Trial Level

The `Trial` directory contains the current playable/prototyping environment.

```text
Trial/
├── Trial_Level/
└── Prototyping Texture/
```

The trial level provides the environment in which the Player and Noel systems are currently being tested.

---

# Development Focus

Project Fallout is primarily a learning and prototyping project.

The project explores:

* Third-person character controllers
* Character state machines
* NPC navigation
* NPC behaviour
* Character animation states
* Blackboard-based communication
* Signal-based communication
* Character statistics
* Camera control
* Godot C# development
* 3D gameplay architecture

The codebase is intentionally organized around these systems so that individual gameplay behaviours can be examined and developed independently.

---

# Repository Status

**Project status:** Prototype / Active Development

This repository represents an ongoing game-development project rather than a completed game.

Systems, scenes, assets, and gameplay behaviour may change as development continues.

---

# License

No license has currently been specified for this repository.

Unless a license is added, the contents of this repository should be considered **all rights reserved**.

---

# Author

**Mike Elnathan**

GitHub:

https://github.com/MikeElnathan

Project:

https://github.com/MikeElnathan/Project_Fallout
