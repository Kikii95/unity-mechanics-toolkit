# Quick Start Guide

## Installation

### Method 1: Unity Package Manager (UPM)

1. Open your Unity project
2. Go to `Window` → `Package Manager`
3. Click `+` button → `Add package from git URL`
4. Enter: `https://github.com/yourusername/unity-mechanics-toolkit.git`
5. Click `Add`

### Method 2: Manual Installation

1. Clone or download this repository
2. Copy the `src/` folder to `Assets/UnityMechanicsToolkit/` in your Unity project
3. Wait for Unity to compile

## Basic Usage Examples

### Movement System

```csharp
using UnityMechanicsToolkit.Movement;

public class PlayerController : MonoBehaviour
{
    private FirstPersonController controller;

    void Start()
    {
        controller = GetComponent<FirstPersonController>();
        controller.moveSpeed = 5f;
        controller.jumpForce = 10f;
    }
}
```

### Interaction System

```csharp
using UnityMechanicsToolkit.Interaction;

public class Door : InteractableObject
{
    public override void OnInteract(GameObject interactor)
    {
        // Open door logic
        Debug.Log("Door opened!");
    }
}
```

### Inventory System

```csharp
using UnityMechanicsToolkit.Inventory;

public class PlayerInventory : MonoBehaviour
{
    private InventorySystem inventory;

    void Start()
    {
        inventory = new InventorySystem(20); // 20 slots
    }

    public void PickupItem(Item item)
    {
        if (inventory.AddItem(item))
        {
            Debug.Log($"Picked up {item.name}");
        }
    }
}
```

### Combat System

```csharp
using UnityMechanicsToolkit.Combat;

public class Enemy : MonoBehaviour
{
    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
        health.maxHealth = 100f;
        health.OnDeath += HandleDeath;
    }

    void HandleDeath()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
```

## Module Overview

### Movement
- **FirstPersonController**: Complete FPS movement
- **ThirdPersonController**: Third-person character control
- **TopDownController**: Top-down 2D/3D movement
- **JumpMechanics**: Customizable jump behaviors

### Interaction
- **InteractableObject**: Base class for interactive objects
- **RaycastInteractor**: Raycast-based interaction
- **TriggerInteractor**: Trigger-based interaction
- **DoorController**: Configurable door mechanics

### Inventory
- **InventorySystem**: Main inventory manager
- **Item**: Base item class
- **Equipment**: Equipment-specific items
- **InventoryUI**: UI integration

### Combat
- **Health**: Health management system
- **DamageDealer**: Damage calculation
- **Weapon**: Weapon base class
- **Projectile**: Projectile physics

### AI
- **AIStateMachine**: Flexible state machine
- **PatrolBehavior**: Patrol waypoint system
- **ChaseBehavior**: Target following
- **VisionCone**: Line-of-sight detection

### Utils
- **ObjectPool**: Object pooling system
- **Singleton<T>**: Singleton pattern
- **ExtensionMethods**: Useful extensions
- **DebugTools**: Development helpers

## Next Steps

1. Explore the `examples/` folder for complete scene setups
2. Check individual module documentation in `docs/`
3. Customize scripts to fit your project needs

## Troubleshooting

### Scripts not compiling?
- Ensure Unity version is 2021.3 or higher
- Check that all dependencies are installed

### Missing references?
- Verify the `src/` folder is correctly placed in `Assets/`
- Reimport the package if needed

## Support

For issues or questions, check the GitHub repository issues page.
