# Demo Scenes Generator

**Automatic scene creation tools for testing Unity Mechanics Toolkit features.**

---

## 🎯 Overview

Unity Editor scripts that automatically generate preconfigured demo scenes for each system. All GameObjects, components, and references are linked automatically - just press a button!

---

## 📋 Menu Location

All tools are accessible via Unity menu:

```
Tools > Unity Mechanics Toolkit > Create Demo Scenes > [Scene Type]
```

---

## 🎮 Available Demo Scenes

### 1. Player Complete Setup

**Menu**: `Tools > Unity Mechanics Toolkit > Create Demo Scenes > Player Complete Setup`

**Creates**:
- Fully configured player with:
  - Movement (WASD, crouch, sprint, lean)
  - Stamina system
  - Camera effects (shake, head bob, look behind)
  - Flashlight with battery
  - Sanity system
- Large ground plane
- Test obstacles (walls, boxes)
- Proper lighting

**Controls**:
- ZQSD / WASD: Move
- Mouse: Look
- Space: Jump
- C: Crouch
- Shift: Sprint
- A/E: Lean (AZERTY)
- F: Flashlight
- V: Look Behind

**Ready to test**: ✅ Immediately playable

---

### 2. AI Enemy Setup

**Menu**: `Tools > Unity Mechanics Toolkit > Create Demo Scenes > AI Enemy Setup`

**Creates**:
- Patrolling enemy with:
  - AI State Machine
  - Vision Cone detection
  - Patrol Behavior
  - Chase Behavior
  - Health system
- 4 waypoints (patrol path)
- Simple player for testing
- Ground + obstacles

**Manual Steps Required**:
1. Select `Ground`
2. Mark as `Navigation Static` (Inspector)
3. Window → AI → Navigation → Bake
4. Select `Enemy`
5. Wire events in Inspector:
   - VisionCone → On Target Detected → AIStateMachine → StartChase()
   - VisionCone → On Target Lost → AIStateMachine → StartSearch()
6. Drag waypoints into `PatrolBehavior` → Waypoints array

**Ready to test**: ⚠️ Requires NavMesh bake + event wiring

---

### 3. Interaction Systems

**Menu**: `Tools > Unity Mechanics Toolkit > Create Demo Scenes > Interaction Systems`

**Creates**:
- Player with RaycastInteractor
- Door (opens/closes with E)
- Key (examinable, rotatable)
- Closet (hiding spot)
- All with HighlightSystem

**Manual Steps Required**:
1. Select `Closet` → HidingSpot component
2. Assign `Camera Transform` → drag Player/PlayerCamera
3. Assign `Movement Script` → drag Player → FirstPersonController (if exists)

**Controls**:
- Look at objects: Highlights
- E: Interact
- ESC: Exit examine mode

**Ready to test**: ⚠️ Requires HidingSpot manual linking

---

### 4. Combat Systems

**Menu**: `Tools > Unity Mechanics Toolkit > Create Demo Scenes > Combat Systems`

**Creates**:
- Player with:
  - Melee weapon (bat)
  - BlockDodge system
  - Stamina + Health
- 3 dummy targets with health
- Ground + lighting

**Controls**:
- Left Click: Attack
- Right Click: Block
- Left Ctrl: Dodge

**Ready to test**: ✅ Immediately playable

---

### 5. Horror Showcase (Complete)

**Menu**: `Tools > Unity Mechanics Toolkit > Create Demo Scenes > Horror Showcase (Complete)`

**Creates**:
- **Complete horror game scene** combining ALL Sprint 1 features:
  - Fully equipped player (movement, flashlight, sanity, combat)
  - Patrolling enemy with vision cone
  - Doors (locked and unlocked)
  - Key (examinable)
  - Hiding spots (closets)
  - Dark atmosphere lighting
  - Maze-like environment

**Manual Steps Required**:
1. Mark all static objects as `Navigation Static`
2. Window → AI → Navigation → Bake NavMesh
3. Wire VisionCone events (see AI Demo above)
4. Wire HidingSpot references (see Interaction Demo above)

**Ready to test**: ⚠️ Requires NavMesh + manual wiring

---

## 🔧 How to Use

### Basic Workflow

1. **Open Unity**
2. **Click menu**: `Tools > Unity Mechanics Toolkit > Create Demo Scenes > [Choose scene]`
3. **Wait**: Scene generates automatically (1-5 seconds)
4. **Follow console instructions**: Manual steps if needed
5. **Press Play**: Test immediately!

### Example: Testing Player Movement

```
1. Tools > Unity Mechanics Toolkit > Create Demo Scenes > Player Complete Setup
2. Wait for scene creation
3. Press Play
4. Use WASD + Mouse to test all movement features
```

### Example: Testing AI Enemy

```
1. Tools > Unity Mechanics Toolkit > Create Demo Scenes > AI Enemy Setup
2. Select Ground > Inspector > Navigation Static ✓
3. Window > AI > Navigation > Bake
4. Select Enemy > VisionCone > Wire events:
   - On Target Detected → AIStateMachine → StartChase()
   - On Target Lost → AIStateMachine → StartSearch()
5. Drag Waypoint1-4 into Enemy > PatrolBehavior > Waypoints
6. Press Play
7. Walk into enemy vision cone
```

---

## 📦 Scene Contents Reference

### Player Complete Setup

```
Scene
├── Player (CharacterController)
│   ├── StaminaSystem
│   ├── FirstPersonController
│   ├── CrouchBehavior
│   ├── SprintBehavior
│   ├── LeanBehavior
│   ├── SanitySystem
│   └── PlayerCamera
│       ├── CameraShake
│       ├── HeadBobbing
│       ├── LookBehind
│       └── Flashlight (Light + FlashlightSystem)
├── Ground (Plane)
├── Directional Light
└── Obstacles (Walls, Boxes)
```

### AI Enemy Setup

```
Scene
├── Player (Simple capsule for testing)
│   └── Camera
├── Enemy (NavMeshAgent)
│   ├── AIStateMachine
│   ├── VisionCone
│   ├── PatrolBehavior
│   ├── ChaseBehavior
│   └── Health
├── Waypoint1-4 (Empty GameObjects)
├── Ground (Plane)
└── Obstacles
```

### Interaction Systems

```
Scene
├── Player
│   └── PlayerCamera (RaycastInteractor)
├── Door (DoorController + HighlightSystem)
├── Key (ExamineObject + HighlightSystem)
├── Closet (HidingSpot)
│   └── HidingTrigger
├── Ground
└── Directional Light
```

### Combat Systems

```
Scene
├── Player
│   ├── StaminaSystem
│   ├── Health
│   ├── BlockDodge
│   └── Bat (MeleeWeapon)
├── Dummy1-3 (Health)
├── Ground
└── Directional Light
```

### Horror Showcase

```
Scene (Complete Horror Game)
├── Player (Full setup)
│   └── All movement + horror + combat systems
├── Enemy (Full AI)
├── Doors (Locked + Unlocked)
├── Key
├── Closets (Hiding spots)
├── Environment (Walls, maze, props)
└── Horror Lighting (Dim + flickering)
```

---

## ⚠️ Common Issues

### Script Compilation Errors

**Problem**: Editor scripts not appearing in menu

**Solution**:
1. Check Console for errors
2. Ensure all Unity Mechanics Toolkit scripts compile
3. Reimport Editor folder (right-click → Reimport)

### Missing Components

**Problem**: Components not found when creating scene

**Solution**:
- Ensure all Unity Mechanics Toolkit scripts are in correct folders
- Check namespace imports in Editor scripts
- Verify .asmdef files exist

### NavMesh Not Baking

**Problem**: Enemy doesn't move in AI demo

**Solution**:
1. Window → AI → Navigation
2. Bake tab → Click "Bake"
3. Blue overlay should appear on ground
4. If not: Mark Ground as Navigation Static

### Events Not Wired

**Problem**: AI doesn't chase player, hiding spot doesn't work

**Solution**:
- These MUST be wired manually in Inspector
- Unity Editor scripts can't automatically wire UnityEvents
- Follow console instructions after scene creation

---

## 🎨 Customization

You can modify the Editor scripts to:

- Change default values (speeds, ranges, etc.)
- Add more objects to scenes
- Create custom scene layouts
- Add your own demo scenes

**Example**: Modify player speed in `CreatePlayerDemoScene.cs`:

```csharp
var fpc = player.AddComponent<FirstPersonController>();
fpc.moveSpeed = 5f; // Change to 10f for faster movement
```

---

## 🚀 Next Steps

After testing demo scenes:

1. **Understand features**: See how components work together
2. **Integrate into your project**: Copy component setups
3. **Customize**: Adjust values to your game needs
4. **Build gameplay**: Combine systems to create unique mechanics

---

## 📚 Related Documentation

- [Tutorial 01 - Player Setup](../tutorials/TUTORIAL_01_PLAYER_SETUP.md)
- [Tutorial 02 - Enemy AI](../tutorials/TUTORIAL_02_ENEMY_AI.md)
- [Tutorial 03 - Interaction Systems](../tutorials/TUTORIAL_03_INTERACTION.md)
- [Integration Guide](INTEGRATION_GUIDE.md)
- [Architecture](ARCHITECTURE.md)

---

**Quick Test**: `Tools > Unity Mechanics Toolkit > Create Demo Scenes > Player Complete Setup` → Press Play → Done! 🎉
