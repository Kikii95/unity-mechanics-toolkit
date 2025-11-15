# Integration Guide - Unity Mechanics Toolkit

**Quick, practical examples for integrating each module into your Unity project.**

---

## 🚀 Quick Start (5 minutes)

### Method 1: Unity Package Manager (Recommended)

1. Open Unity → Window → Package Manager
2. Click `+` → Add package from git URL
3. Enter: `https://github.com/yourusername/unity-mechanics-toolkit.git`
4. Click Add

### Method 2: Manual Import

1. Download/clone repository
2. Copy `src/` folder to `Assets/UnityMechanicsToolkit/` in your Unity project
3. Wait for Unity to compile

---

## 📦 Module Integration Examples

### 1. First Person Controller

**Scenario**: Add FPS movement to your player

**Steps**:
```
1. Select your Player GameObject
2. Add Component → Unity Mechanics Toolkit → Movement → First Person Controller
3. Assign Camera (drag Main Camera to Inspector field)
4. Configure settings:
   - Move Speed: 5
   - Jump Force: 8
   - Mouse Sensitivity: 2
5. Play!
```

**Optional integrations**:
- Add `StaminaSystem` component → Sprint drains stamina
- Add `CrouchBehavior` component → Press C to crouch
- Add `LeanBehavior` component → Q/E to lean

**Code access** (optional):
```csharp
FirstPersonController controller = GetComponent<FirstPersonController>();
controller.MoveSpeed = 10f; // Modify at runtime
```

---

### 2. Health System

**Scenario**: Add health to player or enemy

**Steps**:
```
1. Select GameObject (Player/Enemy)
2. Add Component → Unity Mechanics Toolkit → Combat → Health
3. Configure:
   - Max Health: 100
   - Can Regenerate: true (optional)
   - Regen Rate: 5
   - Regen Delay: 3
4. Wire events in Inspector (optional):
   - onDeath → PlayerController.Die()
   - onHealthChanged → HealthUI.UpdateBar()
```

**Code usage**:
```csharp
// Via interface (no hard dependency)
IDamageable target = enemy.GetComponent<IDamageable>();
if (target != null)
{
    target.TakeDamage(50f);
}

// Direct access
Health health = GetComponent<Health>();
float percentage = health.HealthPercentage;
if (percentage < 0.3f)
{
    ShowLowHealthWarning();
}
```

---

### 3. Interaction System

**Scenario**: Make doors, items, and objects interactable

#### Step A: Setup Player Interactor

```
1. Select Player GameObject
2. Add Component → Unity Mechanics Toolkit → Interaction → Raycast Interactor
3. Configure:
   - Player Camera: Drag Main Camera
   - Interaction Range: 3
   - Interactable Layer: Select layer
   - Interact Key: E
```

#### Step B: Make Objects Interactable

**Example: Door**
```
1. Create Door GameObject
2. Add Component → Unity Mechanics Toolkit → Interaction → Door Controller
3. Configure:
   - Door Type: Rotating / Sliding / Auto
   - Rotation Angle: 90 (for rotating)
   - Is Locked: false
4. Add Collider (trigger if needed)
5. Set layer to "Interactable"
6. Play! Approach door, press E to open
```

**Example: Custom Interactable**
```csharp
using UnityMechanicsToolkit.Interaction;

public class PickupItem : InteractableObject
{
    [SerializeField] private Item item;

    public override void OnInteract(GameObject interactor)
    {
        // Custom logic
        InventorySystem inventory = interactor.GetComponent<InventorySystem>();
        if (inventory != null && inventory.AddItem(item))
        {
            Destroy(gameObject);
        }

        base.OnInteract(interactor); // Trigger UnityEvent
    }
}
```

---

### 4. Inventory System

**Scenario**: Add inventory to player

**Steps**:
```
1. Select Player GameObject
2. Add Component → Unity Mechanics Toolkit → Inventory → Inventory System
3. Configure:
   - Inventory Size: 20 slots
4. Wire events:
   - onItemAdded → InventoryUI.Refresh()
   - onItemRemoved → InventoryUI.Refresh()
```

**Create Items** (ScriptableObjects):
```
1. Right-click in Project → Create → Unity Mechanics Toolkit → Inventory → Item
2. Configure:
   - Item Name: "Health Potion"
   - Description: "Restores 50 HP"
   - Max Stack Size: 5
   - Icon: Drag sprite
```

**Code usage**:
```csharp
InventorySystem inventory = GetComponent<InventorySystem>();

// Add item
if (inventory.AddItem(healthPotion, 1))
{
    Debug.Log("Item added!");
}

// Check if has item
if (inventory.HasItem(keyItem))
{
    UnlockDoor();
}

// Remove item
inventory.RemoveItem(ammo, 10);
```

---

### 5. AI System (Coming in Sprint 1)

**Scenario**: Add enemy AI with patrol and chase

**Steps** (example, will be implemented):
```
1. Create Enemy GameObject
2. Add Component → Unity Mechanics Toolkit → AI → AI State Machine
3. Add Component → Unity Mechanics Toolkit → AI → Patrol Behavior
4. Add Component → Unity Mechanics Toolkit → AI → Chase Behavior
5. Add Component → Unity Mechanics Toolkit → AI → Vision Cone
6. Configure:
   - Patrol Waypoints: Create empty GameObjects as waypoints
   - Vision Range: 10
   - Vision Angle: 90
   - Patrol Speed: 2
   - Chase Speed: 5
7. Add NavMeshAgent component (Unity built-in)
8. Bake NavMesh (Navigation window)
```

**Events** (optional):
```
VisionCone.onTargetDetected → AIStateMachine.StartChase()
VisionCone.onTargetLost → AIStateMachine.StartSearch()
```

---

### 6. Flashlight System (Coming in Sprint 1)

**Scenario**: Add horror flashlight with battery

**Steps**:
```
1. Select Player or Camera
2. Add Spotlight component (Unity built-in)
3. Add Component → Unity Mechanics Toolkit → Horror → Flashlight System
4. Configure:
   - Light Component: Drag Spotlight
   - Max Battery: 100
   - Drain Rate: 5 (per second)
   - Flicker When Low: true
   - Low Battery Threshold: 20
5. Wire events:
   - onBatteryChanged → UI.UpdateBatteryBar()
   - onBatteryDepleted → ShowWarning()
```

**Code usage**:
```csharp
FlashlightSystem flashlight = GetComponent<FlashlightSystem>();

// Toggle
if (Input.GetKeyDown(KeyCode.F))
{
    flashlight.Toggle();
}

// Add battery
flashlight.AddBattery(50f);

// Check status
if (flashlight.BatteryPercentage < 0.1f)
{
    ShowLowBatteryWarning();
}
```

---

### 7. Sanity System (Coming in Sprint 1)

**Scenario**: Add Amnesia-style sanity mechanic

**Steps**:
```
1. Select Player GameObject
2. Add Component → Unity Mechanics Toolkit → Horror → Sanity System
3. Configure:
   - Max Sanity: 100
   - Drain In Darkness: true
   - Darkness Drain Rate: 2
   - Drain Near Enemy: true
   - Enemy Drain Rate: 5
   - Visual Effects: Enable post-processing
4. Wire events:
   - onSanityChanged → UI.UpdateSanityBar()
   - onSanityLow → ApplyLowSanityEffects()
   - onSanityDepleted → GameOver()
```

**Optional integrations**:
- Lighting System → Detects darkness automatically
- AI System → Detects nearby enemies automatically
- Camera → Applies visual distortion effects

**Code usage**:
```csharp
SanitySystem sanity = GetComponent<SanitySystem>();

// Modify sanity
sanity.DecreaseSanity(10f); // See scary thing
sanity.IncreaseSanity(20f); // Safe area

// Check status
if (sanity.SanityPercentage < 0.3f)
{
    StartHallucinations();
}
```

---

## 🔗 Cross-Module Integration

### Example 1: FPS Player with Health, Stamina, and Inventory

```
Player GameObject
├── FirstPersonController (movement)
├── Health (take damage)
├── StaminaSystem (sprint cost)
├── InventorySystem (carry items)
├── RaycastInteractor (interact with objects)
└── Camera
    ├── FlashlightSystem (horror lighting)
    └── SanitySystem (horror mechanic)
```

**No code required** - Systems auto-detect each other:
- FirstPersonController checks for StaminaSystem → Sprint drains stamina if present
- Health.onDeath → Can trigger SanitySystem effects
- RaycastInteractor → Works with any InteractableObject

---

### Example 2: Enemy with AI, Health, and Vision

```
Enemy GameObject
├── Health (can be damaged)
├── AIStateMachine (behavior control)
├── PatrolBehavior (waypoint patrol)
├── ChaseBehavior (follow player)
├── VisionCone (detect player)
├── HearingSystem (detect sounds)
└── NavMeshAgent (pathfinding)
```

**Event wiring**:
```
Health.onDeath → AIStateMachine.Die()
VisionCone.onTargetDetected → AIStateMachine.StartChase()
VisionCone.onTargetLost → AIStateMachine.StartSearch()
HearingSystem.onSoundHeard → AIStateMachine.Investigate()
```

---

### Example 3: Interactable Door with Lock

```
Door GameObject
├── DoorController (open/close)
├── MeshRenderer (visual)
├── Collider (trigger)
└── AudioSource (sound)
```

**Code** (if needed):
```csharp
public class LockedDoor : InteractableObject
{
    [SerializeField] private Item requiredKey;
    private DoorController door;

    void Awake()
    {
        door = GetComponent<DoorController>();
        door.Lock(); // Start locked
    }

    public override void OnInteract(GameObject interactor)
    {
        InventorySystem inventory = interactor.GetComponent<InventorySystem>();

        if (door.IsLocked)
        {
            if (inventory != null && inventory.HasItem(requiredKey))
            {
                inventory.RemoveItem(requiredKey, 1);
                door.Unlock();
                door.OpenDoor();
            }
            else
            {
                Debug.Log("Door is locked! Need key.");
            }
        }
        else
        {
            door.ToggleDoor();
        }
    }
}
```

---

## 🎨 Designer Workflow (No Code)

### Scenario: Create a horror game level (no programming)

**Steps**:

1. **Player Setup** (5 min)
   - Create Capsule → Rename "Player"
   - Add FirstPersonController → Configure
   - Add Health → Set max health 100
   - Add FlashlightSystem → Add Spotlight child
   - Add SanitySystem → Enable effects
   - Add RaycastInteractor → Set interact key

2. **Enemy Setup** (5 min)
   - Create Capsule → Rename "Enemy"
   - Add NavMeshAgent
   - Add Health → Set max health 50
   - Add AIStateMachine
   - Add PatrolBehavior → Create waypoints
   - Add VisionCone → Set range/angle
   - Wire events in Inspector

3. **Environment** (10 min)
   - Create floor, walls (ProBuilder)
   - Add doors → DoorController components
   - Add lights → Some flickering (FlickerLight component)
   - Add items → PickupItem scripts
   - Place hiding spots (closets) → HidingSpot components

4. **Bake Navigation**
   - Window → AI → Navigation
   - Select floor → Mark "Navigation Static"
   - Bake NavMesh

5. **Test Play!**

**No scripting required** - Everything configurable in Inspector.

---

## 💡 Tips & Best Practices

### 1. Layer Setup

Create these layers for proper interaction:
```
- Default
- Player
- Enemy
- Interactable
- Ground
- Obstacle
```

Configure layer collision matrix (Edit → Project Settings → Physics).

---

### 2. Tag Setup

Recommended tags:
```
- Player
- Enemy
- Interactable
- HidingSpot
```

---

### 3. Performance

**For many enemies** (10+):
- Use ObjectPool for spawning
- Disable AI when far from player (LOD)
- Use simpler colliders (capsule > mesh)

**For many interactables**:
- Use trigger colliders (cheaper)
- Disable interactables when player far away

---

### 4. Debugging

Enable Gizmos in Scene view to visualize:
- Vision cones (AI)
- Interaction ranges
- Patrol waypoints
- Audio ranges

---

## 🆘 Troubleshooting

### Issue: "Component not found"
- Ensure module folder is in `Assets/`
- Check Unity console for compile errors
- Reimport package if needed

### Issue: "Interaction not working"
- Check layers (interactor and interactable on correct layers)
- Verify collider on interactable object
- Ensure RaycastInteractor range is sufficient

### Issue: "AI not moving"
- Ensure NavMeshAgent component present
- Bake NavMesh (Window → AI → Navigation)
- Check patrol waypoints are set

### Issue: "Events not firing"
- Verify UnityEvents are wired in Inspector
- Check if conditions are met (e.g., Health > 0 before damage)
- Enable Debug logs to trace execution

---

## 📚 Additional Resources

- **Architecture**: See `ARCHITECTURE.md` for design principles
- **Roadmap**: See `ROADMAP.md` for planned features
- **Examples**: Check `examples/` folder for sample scenes
- **API Reference**: XML documentation in code

---

## 🎯 Next Steps

1. **Import toolkit** into your project
2. **Create test scene** with player + basic movement
3. **Add one module at a time** (start simple)
4. **Experiment** with settings in Inspector
5. **Combine modules** for complex behaviors
6. **Customize** scripts as needed (they're yours!)

**Happy game making!** 🚀
