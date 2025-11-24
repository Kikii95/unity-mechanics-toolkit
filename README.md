# Unity Mechanics Toolkit

**Production-ready game mechanics for Unity horror games.** Build complete horror experiences in minutes with pre-configured systems, automatic demo scenes, and comprehensive tutorials.

[![Version](https://img.shields.io/badge/version-1.1.0-blue.svg)](https://github.com/yourusername/unity-mechanics-toolkit/releases)
[![Unity](https://img.shields.io/badge/Unity-2021.3%2B-black.svg)](https://unity.com)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

---

## ⚡ Quick Start (1 Click Demo!)

Open Unity → `Tools > Unity Mechanics Toolkit > Create Demo Scenes > Horror Showcase`

**Instant playable horror game scene** with player, AI enemy, interactions, and atmosphere. No manual setup required!

[See all demo scene generators →](docs/DEMO_SCENES_GENERATOR.md)

---

## 🎯 What's Implemented (Sprint 1 - COMPLETE)

### ✅ 20 Production-Ready Features

**Movement Systems** (5)
- FirstPersonController - WASD + mouse look
- StaminaSystem - Resource management
- CrouchBehavior - Toggle/hold crouch
- SprintBehavior - Stamina-based sprint with FOV effect
- LeanBehavior - Lean left/right (AZERTY support)

**Horror Systems** (2)
- FlashlightSystem - Battery-powered light with flicker
- SanitySystem - Amnesia-style sanity with visual effects

**Camera Systems** (3)
- CameraShake - Screen shake on events
- HeadBobbing - Rhythmic walking bob
- LookBehind - Resident Evil-style 180° rotation

**AI Systems** (4)
- AIStateMachine - State management (Patrol/Chase/Attack)
- VisionCone - FOV detection with line of sight
- PatrolBehavior - NavMesh waypoint patrol
- ChaseBehavior - Target following with attack range

**Interaction Systems** (3)
- RaycastInteractor - Raycast-based object detection
- DoorController - Hinged/sliding doors with locks
- ExamineObject - 3D rotate/zoom inspection
- HidingSpot - Closet/bed/locker hiding
- HighlightSystem - Emission-based object glow

**Combat Systems** (2)
- Health - HP management with events
- MeleeWeapon - Melee attacks with durability
- BlockDodge - Block (damage reduction) + Dodge (i-frames)

**Audio Systems** (2)
- FootstepSystem - Material-based footstep sounds
- HeartbeatAudio - Dynamic heartbeat (sanity/enemies)

**Core Utilities**
- IDamageable, IInteractable interfaces
- GameEvents - Cross-module communication
- Transform/Vector3/GameObject extensions

---

## 🔧 Automatic Demo Scene Generators (NEW!)

**5 Unity Editor tools** to generate complete test scenes instantly:

```
Tools > Unity Mechanics Toolkit > Create Demo Scenes
```

1. **Player Complete Setup** - Fully configured horror player
2. **AI Enemy Setup** - Patrolling enemy with chase behavior
3. **Interaction Systems** - Doors, keys, hiding spots
4. **Combat Systems** - Melee combat with dummies
5. **Horror Showcase** - Complete horror game scene (recommended!)

[Full documentation →](docs/DEMO_SCENES_GENERATOR.md)

---

## 📚 Complete Tutorial Suite

**Step-by-step guides** (10-15 minutes each):

- [Tutorial 01 - Player Setup](tutorials/TUTORIAL_01_PLAYER_SETUP.md) - Complete horror player
- [Tutorial 02 - Enemy AI](tutorials/TUTORIAL_02_ENEMY_AI.md) - Patrol/chase AI
- [Tutorial 03 - Interaction Systems](tutorials/TUTORIAL_03_INTERACTION.md) - Doors, keys, hiding
- [Tutorial 04 - Combat Systems](tutorials/TUTORIAL_04_COMBAT.md) - Melee combat

**Guides**:
- [QUICKSTART.md](QUICKSTART.md) - Get started in 5 minutes
- [ARCHITECTURE.md](docs/ARCHITECTURE.md) - Design principles
- [CREATING_PREFABS.md](docs/CREATING_PREFABS.md) - Reusable prefabs workflow

---

## 🚀 Installation

### Method 1: Unity Package Manager (Recommended)

1. Open Unity Package Manager
2. Click `+` → `Add package from git URL`
3. Paste: `https://github.com/yourusername/unity-mechanics-toolkit.git`

### Method 2: Manual Installation

1. Download or clone this repository
2. Copy `src/` folder to your Unity project's `Assets/`
3. Wait for Unity to compile

---

## 💡 Usage Example

### Fastest Way: Auto-Generate

```
Tools > Unity Mechanics Toolkit > Create Demo Scenes > Horror Showcase
→ Bake NavMesh
→ Wire events (console instructions)
→ Press Play!
```

### Manual Integration

```csharp
// Add to existing player
using UnityMechanicsToolkit.Movement;
using UnityMechanicsToolkit.Horror;

// Player already has CharacterController
gameObject.AddComponent<FirstPersonController>();
gameObject.AddComponent<StaminaSystem>();
gameObject.AddComponent<FlashlightSystem>();
gameObject.AddComponent<SanitySystem>();
```

[See QUICKSTART.md for detailed examples →](QUICKSTART.md)

---

## 📦 Project Structure

```
unity-mechanics-toolkit/
├── src/
│   ├── Core/          # Shared interfaces, events, extensions
│   ├── Movement/      # Player movement (FPS, crouch, sprint, lean)
│   ├── Horror/        # Flashlight, sanity systems
│   ├── Camera/        # Camera effects (shake, bob, look behind)
│   ├── AI/            # State machine, vision, patrol, chase
│   ├── Interaction/   # Doors, examine, hiding, highlight
│   ├── Combat/        # Health, melee, block/dodge
│   ├── Audio/         # Footsteps, heartbeat
│   └── Editor/        # Unity Editor demo scene generators ⭐
├── docs/
│   ├── ROADMAP.md              # 200+ planned features
│   ├── ARCHITECTURE.md         # Design principles
│   ├── INTEGRATION_GUIDE.md    # Component integration
│   ├── DEMO_SCENES_GENERATOR.md # Auto scene creation
│   └── CREATING_PREFABS.md     # Prefab workflow
├── tutorials/
│   ├── TUTORIAL_01_PLAYER_SETUP.md
│   ├── TUTORIAL_02_ENEMY_AI.md
│   ├── TUTORIAL_03_INTERACTION.md
│   └── TUTORIAL_04_COMBAT.md
├── QUICKSTART.md
└── README.md (this file)
```

---

## 🎮 Horror Game Focus

This toolkit is optimized for **horror games** inspired by:
- Silent Hill (sanity, atmosphere)
- Resident Evil (camera, controls)
- Amnesia (hiding, sanity mechanics)
- Dead Space (UI, immersion)
- Outlast (helpless player, chase)

[See full roadmap (200+ features) →](docs/ROADMAP.md)

---

## 🛠️ Requirements

- **Unity**: 2021.3 or higher
- **.NET**: Standard 2.1
- **NavMesh**: For AI pathfinding (built-in)
- **Post Processing**: For sanity effects (optional)

---

## 🎯 Roadmap

**Sprint 1** ✅ **COMPLETE** (v1.0.0)
- Movement, Horror, Camera, AI, Interaction, Combat, Audio (20 features)

**Polish Sprint** ✅ **COMPLETE** (v1.1.0)
- Auto demo scene generators, tutorials, comprehensive docs

**Sprint 2** (Planned)
- Inventory system
- Save/Load system
- Advanced AI (hearing, smell, squad tactics)
- Puzzle systems

**Sprint 3** (Planned)
- VFX systems
- Advanced lighting
- Procedural generation
- Stealth mechanics

**Sprint 4** (Planned)
- Multiplayer foundation
- Narrative system
- Advanced puzzles

[Full roadmap with 200+ features →](docs/ROADMAP.md)

---

## 📊 Stats

- **20 Features** implemented (Sprint 1)
- **~4000 lines** of production C# code
- **10+ documentation** guides
- **4 complete tutorials** (step-by-step)
- **5 demo scene generators** (Unity Editor tools)
- **100% modular** architecture

---

## 🎨 Key Features

### 🔥 Killer Features
- **1-Click Demo Scenes**: Generate complete test scenes automatically
- **AZERTY Keyboard Support**: Lean with A/E keys
- **Hybrid Architecture**: Core + Modules for optimal performance
- **Inspector-Friendly**: All components configured via Unity Inspector
- **Event-Driven**: UnityEvents + C# events for loose coupling
- **Gizmos Visualization**: Editor debugging tools

### 🏗️ Architecture
- SOLID principles
- Component-based design
- Interface-based communication
- Soft dependencies (modules work standalone or with Core)
- Object pooling and caching

[See ARCHITECTURE.md for details →](docs/ARCHITECTURE.md)

---

## 🤝 Contributing

This is a personal toolkit project. Feel free to:
- Fork for your own projects
- Submit issues/suggestions
- Share feedback

---

## 📄 License

MIT License - See [LICENSE](LICENSE) file for details.

---

## 🚀 Get Started Now!

```
1. Install toolkit (UPM or manual)
2. Tools > Unity Mechanics Toolkit > Create Demo Scenes > Horror Showcase
3. Press Play!
```

**Build your horror game in minutes, not months.** 👻

---

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/unity-mechanics-toolkit/issues)
- **Docs**: [Documentation](docs/)
- **Quick Start**: [QUICKSTART.md](QUICKSTART.md)

---

**Made with ❤️ for horror game developers**
