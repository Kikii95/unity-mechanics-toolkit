# Unity Mechanics Toolkit - Quick Start Guide

**Get started in 5 minutes with automatic demo scenes and ready-to-use components.**

---

## 🚀 Installation

### Method 1: Unity Package Manager (Recommended)

1. Open your Unity project
2. Window → Package Manager
3. Click `+` → Add package from git URL
4. Enter: `https://github.com/yourusername/unity-mechanics-toolkit.git`
5. Click `Add`

### Method 2: Manual Import

1. Download the toolkit
2. Copy `src/` folder to your Unity project's `Assets/` folder
3. Unity will auto-import and compile

---

## ⚡ Fastest Way to Test: Auto-Generate Demo Scenes

### Option 1: Complete Horror Showcase (Recommended)

```
Tools > Unity Mechanics Toolkit > Create Demo Scenes > Horror Showcase (Complete)
```

**What you get**:
- Fully configured player (movement, flashlight, sanity, combat)
- Patrolling AI enemy
- Doors, keys, hiding spots
- Dark horror atmosphere
- Ready to play!

**Manual steps required**:
1. Mark static objects as Navigation Static
2. Bake NavMesh (Window → AI → Navigation → Bake)
3. Wire a few events (see console instructions)
4. Press Play!

### Option 2: Test Individual Systems

**Player Movement**:
```
Tools > Unity Mechanics Toolkit > Create Demo Scenes > Player Complete Setup
→ Press Play → Use WASD + Mouse
```

**AI Enemy**:
```
Tools > Unity Mechanics Toolkit > Create Demo Scenes > AI Enemy Setup
→ Bake NavMesh → Wire events → Press Play
```

**Interaction Systems**:
```
Tools > Unity Mechanics Toolkit > Create Demo Scenes > Interaction Systems
→ Press Play → Look at objects → Press E
```

**Combat**:
```
Tools > Unity Mechanics Toolkit > Create Demo Scenes > Combat Systems
→ Press Play → Left Click to attack dummies
```

---

## 📦 Manual Integration (Step-by-Step)

### Create a Horror Game Player (5 minutes)

**Tutorial**: [TUTORIAL_01_PLAYER_SETUP.md](tutorials/TUTORIAL_01_PLAYER_SETUP.md)

**Quick Steps**:
1. Create Empty GameObject → Name: `Player`
2. Add CharacterController
3. Add these components (Unity Mechanics Toolkit menu):
   - FirstPersonController
   - StaminaSystem
   - CrouchBehavior
   - SprintBehavior
   - LeanBehavior
   - FlashlightSystem
   - SanitySystem
4. Create child Camera → Add CameraShake, HeadBobbing, LookBehind
5. Link camera references in Inspector
6. Press Play!

**Controls**: WASD + Mouse, C=Crouch, Shift=Sprint, F=Flashlight, V=LookBehind

---

### Create an AI Enemy (10 minutes)

**Tutorial**: [TUTORIAL_02_ENEMY_AI.md](tutorials/TUTORIAL_02_ENEMY_AI.md)

**Quick Steps**:
1. Create Capsule → Name: `Enemy`
2. Add NavMeshAgent
3. Add these components:
   - AIStateMachine
   - VisionCone
   - PatrolBehavior
   - ChaseBehavior
   - Health
4. Create 4 Empty GameObjects as waypoints
5. Bake NavMesh (Window → AI → Navigation)
6. Wire VisionCone events to AIStateMachine
7. Press Play!

**Behavior**: Patrols waypoints → Detects player → Chases → Attacks

---

### Add Interaction Systems (5 minutes)

**Tutorial**: [TUTORIAL_03_INTERACTION.md](tutorials/TUTORIAL_03_INTERACTION.md)

**Quick Steps**:
1. Add RaycastInteractor to Player Camera
2. Create interactable objects:
   - Door: Add DoorController + HighlightSystem
   - Key: Add ExamineObject + HighlightSystem
   - Closet: Add HidingSpot
3. Wire highlight events to RaycastInteractor
4. Press Play and press E to interact!

**Features**: Doors, examine 3D objects, hiding spots

---

### Add Combat (5 minutes)

**Tutorial**: [TUTORIAL_04_COMBAT.md](tutorials/TUTORIAL_04_COMBAT.md)

**Quick Steps**:
1. Add Health to Player
2. Create weapon (Cube child of Player) → Add MeleeWeapon
3. Add BlockDodge to Player
4. Create dummy target → Add Health
5. Press Play and Left Click to attack!

**Controls**: Left Click=Attack, Right Click=Block, Left Ctrl=Dodge

---

## 🎮 Feature Overview

### Movement Systems (Sprint 1)

| Component | Description | Key Bindings |
|-----------|-------------|--------------|
| FirstPersonController | WASD movement + mouse look | WASD, Mouse |
| StaminaSystem | Stamina resource (sprint/dodge/attack) | Auto |
| CrouchBehavior | Crouch toggle/hold | C |
| SprintBehavior | Sprint with stamina drain | Shift |
| LeanBehavior | Lean left/right (AZERTY) | A/E |

### Horror Systems (Sprint 1)

| Component | Description |
|-----------|-------------|
| FlashlightSystem | Battery-powered light with flicker |
| SanitySystem | Amnesia-style sanity (darkness drains) |

### Camera Systems (Sprint 1)

| Component | Description |
|-----------|-------------|
| CameraShake | Screen shake on events |
| HeadBobbing | Rhythmic walking bob |
| LookBehind | RE-style 180° camera rotation |

### AI Systems (Sprint 1)

| Component | Description |
|-----------|-------------|
| AIStateMachine | State management (Patrol/Chase/Attack) |
| VisionCone | FOV detection with line of sight |
| PatrolBehavior | Waypoint patrol with NavMesh |
| ChaseBehavior | Player chase with attack range |

### Interaction Systems (Sprint 1)

| Component | Description |
|-----------|-------------|
| RaycastInteractor | Raycast-based object detection |
| DoorController | Hinged/sliding doors with locks |
| ExamineObject | 3D rotate/zoom inspection |
| HidingSpot | Closet/bed/locker hiding |
| HighlightSystem | Emission-based object glow |

### Combat Systems (Sprint 1)

| Component | Description |
|-----------|-------------|
| Health | HP management with events |
| MeleeWeapon | Melee attacks with durability |
| BlockDodge | Block (damage reduction) + Dodge (roll) |

### Audio Systems (Sprint 1)

| Component | Description |
|-----------|-------------|
| FootstepSystem | Material-based footstep sounds |
| HeartbeatAudio | Dynamic heartbeat (sanity/enemies) |

---

## 🎯 Common Workflows

### Workflow 1: Quick Prototype Horror Game

1. Generate Horror Showcase scene (auto)
2. Bake NavMesh
3. Wire events
4. Customize values (speeds, ranges, etc.)
5. Add your own models/textures
6. Build gameplay loop

**Time**: 10 minutes to playable prototype

---

### Workflow 2: Integrate into Existing Project

1. Import Unity Mechanics Toolkit
2. Add desired components to your existing player/enemies
3. Configure in Inspector
4. Wire events
5. Test and iterate

**Time**: 5-15 minutes per system

---

### Workflow 3: Learn and Customize

1. Generate demo scenes
2. Play and observe behaviors
3. Read tutorials for detailed explanations
4. Modify component values
5. Extend with custom scripts

**Time**: Study at your own pace

---

## 📚 Documentation

### Guides

- [ROADMAP.md](docs/ROADMAP.md) - 200+ planned features
- [ARCHITECTURE.md](docs/ARCHITECTURE.md) - Design principles
- [INTEGRATION_GUIDE.md](docs/INTEGRATION_GUIDE.md) - Integration examples
- [DEMO_SCENES_GENERATOR.md](docs/DEMO_SCENES_GENERATOR.md) - Auto scene creation
- [CREATING_PREFABS.md](docs/CREATING_PREFABS.md) - Reusable prefabs guide

### Tutorials

- [Tutorial 01 - Player Setup](tutorials/TUTORIAL_01_PLAYER_SETUP.md) - Complete player (10 min)
- [Tutorial 02 - Enemy AI](tutorials/TUTORIAL_02_ENEMY_AI.md) - Patrol/chase AI (15 min)
- [Tutorial 03 - Interaction Systems](tutorials/TUTORIAL_03_INTERACTION.md) - Doors, keys, hiding (15 min)
- [Tutorial 04 - Combat Systems](tutorials/TUTORIAL_04_COMBAT.md) - Melee combat (15 min)

---

## 🎨 Customization Tips

### Adjust Movement Feel

```
Player > FirstPersonController
- Move Speed: 5 → 7 (faster)
- Sprint Multiplier: 1.5 → 2 (faster sprint)

Player > CrouchBehavior
- Crouch Speed: 2 → 5 (instant crouch)
```

### Adjust AI Difficulty

```
Enemy > VisionCone
- Vision Range: 10 → 15 (sees farther)
- Vision Angle: 90 → 120 (wider FOV)

Enemy > ChaseBehavior
- Chase Speed: 5 → 7 (faster chase)
- Give Up Distance: 20 → 30 (chases longer)
```

### Adjust Horror Intensity

```
Player > SanitySystem
- Darkness Drain Rate: 2 → 5 (faster drain)
- Recovery Rate: 1 → 0.5 (slower recovery)

Player > FlashlightSystem
- Drain Rate: 5 → 10 (battery drains faster)
```

---

## ⚠️ Troubleshooting

### "Components not found" error
- Ensure Unity Mechanics Toolkit is imported
- Check Console for compilation errors
- Reimport package if needed

### Enemy doesn't move
- Bake NavMesh (Window → AI → Navigation → Bake)
- Mark ground as Navigation Static
- Check NavMeshAgent component present

### Interactions don't work
- Check RaycastInteractor on player camera
- Verify objects have IInteractable components
- Wire highlight events in Inspector

### Combat doesn't damage
- Ensure targets have Health component
- Check weapon Attack Range setting
- Verify weapon collider is Trigger

---

## 🚀 Next Steps

1. **Generate Demo Scenes** → See features in action
2. **Read Tutorials** → Understand how systems work
3. **Integrate** → Add to your project
4. **Customize** → Adjust to your game's needs
5. **Expand** → Combine systems for unique gameplay

---

## 💡 Example: Complete Horror Game in 15 Minutes

1. `Tools > Create Demo Scenes > Horror Showcase`
2. Bake NavMesh
3. Wire VisionCone events
4. Replace placeholder models with your assets
5. Adjust lighting (darker = scarier)
6. Add jump scares (VisionCone → OnTargetDetected → Audio.Play)
7. Build scene layout (import your level)
8. Done!

---

## 🎯 Feature Roadmap

**Sprint 1 (COMPLETE)**: Movement, Horror, Camera, AI, Interaction, Combat, Audio

**Sprint 2 (Coming)**: Inventory, Save/Load, Advanced AI, Puzzles

**Sprint 3 (Coming)**: VFX, Lighting, Procedural Generation, Stealth

**Sprint 4 (Coming)**: Multiplayer, Narrative, Advanced Puzzles

See [ROADMAP.md](docs/ROADMAP.md) for full list (200+ features).

---

**Happy Game Making! 🎮🎉**

For questions, issues, or contributions, see README.md.
