# Creating Prefabs - Unity Mechanics Toolkit

**Guide to creating reusable prefabs for common game objects.**

---

## 🎯 Overview

Prefabs are reusable GameObject templates. Instead of manually configuring components every time, create a prefab once and reuse it across scenes.

**Benefits**:
- Save time (no reconfiguration)
- Consistency (all instances identical)
- Easy updates (modify prefab → all instances update)

---

## 📦 Essential Prefabs to Create

### 1. Complete Player Prefab

**Name**: `Player_Complete.prefab`

**How to Create**:

1. **Generate Player** (automatic):
   ```
   Tools > Unity Mechanics Toolkit > Create Demo Scenes > Player Complete Setup
   ```

2. **Test Player**:
   - Press Play
   - Verify all systems work (movement, flashlight, sanity, etc.)
   - Adjust values if needed

3. **Create Prefab**:
   - Select `Player` in Hierarchy
   - Drag to Project window → `Assets/Prefabs/`
   - Prefab created! (blue icon)

4. **Reuse**:
   - Drag `Player_Complete.prefab` into any scene
   - Instantly get fully configured player

**Contains**:
- CharacterController
- StaminaSystem
- FirstPersonController
- CrouchBehavior
- SprintBehavior
- LeanBehavior
- SanitySystem
- Health
- BlockDodge
- PlayerCamera (with effects + flashlight)

---

### 2. AI Enemy Prefab

**Name**: `Enemy_Patrol.prefab`

**How to Create**:

1. **Generate Enemy** (automatic):
   ```
   Tools > Unity Mechanics Toolkit > Create Demo Scenes > AI Enemy Setup
   ```

2. **Bake NavMesh**:
   - Mark ground Navigation Static
   - Window → AI → Navigation → Bake

3. **Wire Events**:
   - VisionCone → OnTargetDetected → AIStateMachine.StartChase()
   - VisionCone → OnTargetLost → AIStateMachine.StartSearch()

4. **Test Enemy**:
   - Press Play
   - Verify patrol → chase → attack works

5. **Create Prefab**:
   - Select `Enemy` in Hierarchy
   - Drag to Project window → `Assets/Prefabs/`

6. **Important**: Waypoints
   - **Option A**: Include waypoints in prefab (make children of Enemy)
   - **Option B**: Assign waypoints per instance in scene

**Contains**:
- NavMeshAgent
- AIStateMachine
- VisionCone
- PatrolBehavior
- ChaseBehavior
- Health

**Usage**:
- Drag prefab into scene
- Assign waypoints in Inspector (if not included)
- Done!

---

### 3. Door Prefab (Unlocked)

**Name**: `Door_Unlocked.prefab`

**How to Create**:

1. **Create Door**:
   - GameObject → 3D Object → Cube
   - Scale: `(1, 2, 0.1)` (thin vertical rectangle)
   - Add Rigidbody (Kinematic)

2. **Add Components**:
   - DoorController
   - HighlightSystem

3. **Configure DoorController**:
   - Door Type: Hinged
   - Open Angle: 90
   - Open Speed: 2
   - Is Locked: ☐ (unchecked)

4. **Configure HighlightSystem**:
   - Highlight Mode: Emission
   - Emission Color: Yellow
   - Emission Intensity: 2

5. **Create Material**:
   - Create material: `DoorMaterial`
   - Shader: Standard
   - Assign to door

6. **Create Prefab**:
   - Drag to `Assets/Prefabs/Doors/`

**Variants**:
- `Door_Locked.prefab`: Set `Is Locked = true`, assign `Required Key Name`
- `Door_Sliding.prefab`: Set `Door Type = Sliding`

---

### 4. Door Prefab (Locked)

**Name**: `Door_Locked.prefab`

**Same as above**, but:
- DoorController → Is Locked: ✓
- DoorController → Required Key Name: `"Rusty Key"`
- Material Color: Red (visual cue)

---

### 5. Examinable Key Prefab

**Name**: `Key_Examinable.prefab`

**How to Create**:

1. **Create Key**:
   - GameObject → 3D Object → Cube
   - Scale: `(0.1, 0.3, 0.05)` (small key shape)

2. **Add Components**:
   - ExamineObject
   - HighlightSystem
   - Rigidbody (optional, for physics)

3. **Configure ExamineObject**:
   - Examine Distance: 0.5
   - Rotate Speed: 100
   - Can Rotate: ✓
   - Can Zoom: ✓
   - Description: "A rusty old key. What does it unlock?"

4. **Create Material**:
   - Color: Yellow/Gold
   - Enable Emission (for glow)

5. **Create Prefab**:
   - Drag to `Assets/Prefabs/Interactables/`

**Variants**:
- `Key_Gold.prefab`: Golden material
- `Key_Silver.prefab`: Silver material
- `Note_Examinable.prefab`: Paper with text

---

### 6. Hiding Spot Prefab (Closet)

**Name**: `HidingSpot_Closet.prefab`

**How to Create**:

1. **Generate from Demo**:
   ```
   Tools > Unity Mechanics Toolkit > Create Demo Scenes > Interaction Systems
   ```

2. **Copy Closet**:
   - Select `Closet` in generated scene
   - Ctrl+C, Ctrl+V to duplicate
   - Move to separate scene or keep

3. **Create Prefab**:
   - Drag to `Assets/Prefabs/Interactables/`

4. **Important**: Per-instance setup
   - After placing in scene, wire:
     - HidingSpot → Camera Transform
     - HidingSpot → Movement Script

**Variants**:
- `HidingSpot_Bed.prefab`: Under-bed hiding
- `HidingSpot_Locker.prefab`: Locker hiding

---

### 7. Melee Weapon Prefab (Bat)

**Name**: `Weapon_Bat.prefab`

**How to Create**:

1. **Create Bat**:
   - GameObject → 3D Object → Cube
   - Scale: `(0.1, 1, 0.1)` (long stick)
   - Rotation: `(0, 0, 45)` (tilted)

2. **Add Components**:
   - MeleeWeapon
   - BoxCollider (Is Trigger: ✓)

3. **Configure MeleeWeapon**:
   - Attack Key: Mouse0
   - Damage: 25
   - Attack Range: 2
   - Stamina Cost: 15
   - Max Durability: 100

4. **Create Material**:
   - Color: Gray/Brown (wood)

5. **Create Prefab**:
   - Drag to `Assets/Prefabs/Weapons/`

**Usage**:
- Drag into scene as child of Player
- Position at `(0.3, 0, 0.5)` relative to player

**Variants**:
- `Weapon_Pipe.prefab`: Metal pipe (higher damage, lower speed)
- `Weapon_Knife.prefab`: Knife (fast, low damage)
- `Weapon_Axe.prefab`: Axe (slow, high damage)

---

### 8. Combat Dummy Prefab

**Name**: `Dummy_Target.prefab`

**How to Create**:

1. **Create Dummy**:
   - GameObject → 3D Object → Capsule
   - Material: Red (hostile color)

2. **Add Components**:
   - Health (Max Health: 50)

3. **Wire Death Event**:
   - Health → On Death → GameObject.SetActive(false)

4. **Create Prefab**:
   - Drag to `Assets/Prefabs/Targets/`

**Usage**:
- Place in scene for combat testing
- Respawn by calling `SetActive(true)`

---

## 🎨 Prefab Organization

Recommended folder structure:

```
Assets/
└── Prefabs/
    ├── Player/
    │   ├── Player_Complete.prefab
    │   └── Player_Minimal.prefab
    ├── Enemies/
    │   ├── Enemy_Patrol.prefab
    │   ├── Enemy_Aggressive.prefab
    │   └── Enemy_Boss.prefab
    ├── Doors/
    │   ├── Door_Unlocked.prefab
    │   ├── Door_Locked.prefab
    │   └── Door_Sliding.prefab
    ├── Interactables/
    │   ├── Key_Examinable.prefab
    │   ├── Note_Examinable.prefab
    │   ├── HidingSpot_Closet.prefab
    │   └── HidingSpot_Bed.prefab
    ├── Weapons/
    │   ├── Weapon_Bat.prefab
    │   ├── Weapon_Pipe.prefab
    │   └── Weapon_Knife.prefab
    ├── Targets/
    │   └── Dummy_Target.prefab
    └── Environment/
        ├── Wall.prefab
        ├── Floor.prefab
        └── Props/
```

---

## 🔧 Prefab Variants

Unity supports **Prefab Variants** - modified versions of base prefabs.

### Example: Enemy Variants

**Base Prefab**: `Enemy_Base.prefab`
- Standard configuration

**Variant 1**: `Enemy_Fast.prefab` (from base)
- Chase Speed: 5 → 7
- Vision Range: 10 → 12

**Variant 2**: `Enemy_Tank.prefab` (from base)
- Health: 50 → 150
- Chase Speed: 5 → 3

**How to Create Variant**:
1. Drag base prefab into scene
2. Modify values
3. Right-click in Project → Create → Prefab Variant
4. Drag modified instance onto variant

---

## 📋 Prefab Checklist

Before creating a prefab:

- [ ] All components configured correctly
- [ ] References linked (cameras, transforms, etc.)
- [ ] Materials assigned
- [ ] Events wired (if possible)
- [ ] Tested in Play mode
- [ ] Values tuned for your game
- [ ] Named descriptively

---

## 🎯 Advanced: Nested Prefabs

Complex prefabs can contain other prefabs.

**Example**: Room Prefab

```
Room_Bedroom.prefab
├── Walls (models)
├── Floor (model)
├── Door_Locked.prefab (nested)
├── HidingSpot_Closet.prefab (nested)
├── Key_Examinable.prefab (nested)
└── Furniture (props)
```

**Benefits**:
- Update door prefab → all rooms update
- Reuse room prefab in multiple scenes
- Easier level design

---

## 💡 Tips

### Tip 1: Use Prefab Overrides Carefully

- Blue line in Inspector = override from prefab
- Right-click → Revert to restore prefab value
- Right-click → Apply to update prefab with new value

### Tip 2: Test Prefabs in Isolated Scene

- Create `TestScene` for prefab testing
- Drag prefab in, test, adjust, update
- Keeps main scenes clean

### Tip 3: Version Control Prefabs

- Prefabs are assets (like scripts)
- Commit to Git for team collaboration
- Use descriptive commit messages

### Tip 4: Document Prefab Requirements

Add a README in Prefabs folder:
```
Prefabs/README.md

Enemy_Patrol.prefab:
- Requires: NavMesh baked
- Manual: Wire VisionCone events
- Manual: Assign waypoints
```

---

## 🚀 Workflow: Prefab-Based Level Design

1. **Create core prefabs** (Player, Enemy, Doors, etc.)
2. **Build rooms** using prefabs
3. **Create room prefabs** (nested)
4. **Assemble level** from room prefabs
5. **Adjust per-instance values** as needed

**Result**: Fast iteration, consistent quality, easy updates

---

## 📚 Related Documentation

- [Demo Scenes Generator](DEMO_SCENES_GENERATOR.md) - Auto-generate prefabs
- [Integration Guide](INTEGRATION_GUIDE.md) - Component setup
- [Tutorials](../tutorials/) - Detailed walkthroughs

---

**Happy Prefabbing! 📦🎉**
