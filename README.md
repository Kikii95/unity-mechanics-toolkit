# Unity Mechanics Toolkit

Collection of reusable game mechanics for Unity projects. Build your games faster with pre-made, production-ready systems.

## Features

### Movement
- Character controllers (First Person, Third Person, Top-Down)
- Rigidbody movement
- Smooth camera follow
- Jump mechanics (single, double, wall jump)
- Dash/sprint systems

### Interaction
- Raycast-based interaction system
- Trigger-based interaction
- Door mechanics (sliding, rotating, locked)
- Pressure plates & switches
- Object highlighting

### Inventory
- Grid-based inventory
- Item stacking
- Equipment system
- Drag & drop UI
- Save/load support

### Combat
- Health system
- Damage calculation
- Weapon system
- Projectile mechanics
- Hit detection

### AI
- Pathfinding integration
- State machines
- Patrol behaviors
- Chase & attack logic
- Vision cones

### Audio
- Audio manager
- Spatial audio helpers
- Music transitions
- Sound pooling

### Camera
- Follow camera
- Cinemachine helpers
- Camera shake
- Zoom controls

### UI
- Health bars
- Damage numbers
- Tooltips
- Menus & transitions

### Utils
- Object pooling
- Singleton pattern
- Extensions methods
- Math helpers
- Debug tools

## Installation

### Unity Package Manager (Recommended)

1. Open Unity Package Manager
2. Click `+` → `Add package from git URL`
3. Paste: `https://github.com/yourusername/unity-mechanics-toolkit.git`

### Manual Installation

1. Download the repository
2. Copy the `src/` folder into your Unity project's `Assets/` folder
3. Rename to `UnityMechanicsToolkit`

## Quick Start

Check [QUICKSTART.md](./QUICKSTART.md) for detailed setup and usage examples.

## Structure

```
src/
├── Movement/        # Character movement systems
├── Interaction/     # Interaction mechanics
├── Inventory/       # Inventory systems
├── Combat/          # Combat mechanics
├── AI/              # AI behaviors
├── Audio/           # Audio systems
├── Camera/          # Camera utilities
├── UI/              # UI components
└── Utils/           # Shared utilities
```

## Requirements

- Unity 2021.3 or higher
- .NET Standard 2.1

## License

MIT License - See LICENSE file for details

## Contributing

This is a personal toolkit project. Feel free to fork and adapt for your own needs.
