# Tutorial 03 - Interaction Systems

**Goal**: Create a complete interaction system with highlighting, examining, hiding, and interactable objects.

**Time**: 15 minutes

---

## 🎯 What You'll Build

An interaction system with:
- Raycast-based object detection
- Visual highlighting on hover
- 3D object examination (rotate/zoom)
- Hiding spots (closets/beds/lockers)
- Doors, items, and custom interactables

---

## 📋 Prerequisites

- Unity 2021.3+
- Unity Mechanics Toolkit imported
- Player from Tutorial 01
- Basic scene with objects to interact with

---

## Step 1: Setup Player Interaction

### 1.1 Add RaycastInteractor

1. **RaycastInteractor Component**
   - Select Player → PlayerCamera
   - Add Component → Unity Mechanics Toolkit → Interaction → Raycast Interactor
   - Configure:
     - Interaction Range: `3` (can interact within 3m)
     - Interaction Layer: `Default` (layer of interactable objects)
     - Interaction Key: `E`
     - Show Interaction Prompt: `✓`
     - Prompt Offset: `(0, -200)` (screen position)

2. **Test detection**
   - Press Play
   - Look at objects (raycast detects automatically)
   - UI prompt appears when looking at interactables

---

## Step 2: Create Basic Interactable Door

### 2.1 Create Door GameObject

1. **Create Door**
   - GameObject → 3D Object → Cube
   - Name: `Door`
   - Scale: `(1, 2, 0.1)` (thin vertical rectangle)
   - Position: `(0, 1, 3)` (in front of player)
   - Add Rigidbody:
     - Use Gravity: `☐`
     - Is Kinematic: `✓`

2. **DoorController Component**
   - Select Door
   - Add Component → Unity Mechanics Toolkit → Interaction → Door Controller
   - Configure:
     - Door Type: `Hinged` (or Sliding)
     - Open Angle: `90` (degrees)
     - Open Speed: `2`
     - Auto Close: `☐` (manual mode)
     - Is Locked: `☐`

3. **Make Door Interactable**
   - Door already implements IInteractable
   - No extra setup needed

4. **Test door**
   - Press Play
   - Look at door (prompt: "Press E to Open")
   - Press E to open/close
   - Door swings smoothly

---

## Step 3: Add Visual Highlighting

### 3.1 HighlightSystem

1. **Add Material to Door**
   - Select Door
   - Create material in Project: `DoorMaterial`
   - Assign to Door's MeshRenderer
   - Shader: `Standard` (must support emission)

2. **HighlightSystem Component**
   - Select Door
   - Add Component → Unity Mechanics Toolkit → Interaction → Highlight System
   - Configure:
     - Highlight Mode: `Emission` (recommended)
     - Emission Color: Yellow `(1, 0.9, 0.3, 1)`
     - Emission Intensity: `2`
     - Transition Speed: `5`

3. **Wire Highlight to Raycast**
   - Select PlayerCamera (RaycastInteractor)
   - Inspector → RaycastInteractor → Events:
     - On Object Hovered → Door → HighlightSystem → Highlight()
     - On Object Unhovered → Door → HighlightSystem → Unhighlight()

4. **Test highlight**
   - Press Play
   - Look at door → glows yellow
   - Look away → glow fades

---

## Step 4: Create Examine Object

### 4.1 Setup Examinable Item

1. **Create Item**
   - GameObject → 3D Object → Cube (or import 3D model)
   - Name: `Key`
   - Scale: `(0.1, 0.3, 0.05)` (small key shape)
   - Position: `(2, 1, 0)` (on table/ground)
   - Add Rigidbody (optional):
     - Use Gravity: `✓`
     - Mass: `0.1`

2. **ExamineObject Component**
   - Select Key
   - Add Component → Unity Mechanics Toolkit → Interaction → Examine Object
   - Configure:
     - Examine Position: `(0, 0, 1)` (in front of camera)
     - Examine Distance: `0.5` (zoom level)
     - Rotate Speed: `100`
     - Zoom Speed: `2`
     - Min Zoom: `0.3`
     - Max Zoom: `1`
     - Exit Key: `Escape`
     - Can Rotate: `✓`
     - Can Zoom: `✓`
     - Description: `"A rusty old key. What does it unlock?"`

3. **Add HighlightSystem** (same as Step 3)
   - Add Component → Highlight System
   - Wire to RaycastInteractor events

4. **Test examine**
   - Press Play
   - Look at key (prompt: "Press E to Examine")
   - Press E → key zooms to camera
   - Mouse drag → rotates key
   - Mouse scroll → zooms in/out
   - ESC → returns to world

---

## Step 5: Create Hiding Spot

### 5.1 Build Closet

1. **Create Closet GameObject**
   - GameObject → Create Empty
   - Name: `Closet`
   - Position: `(5, 0, 0)`

2. **Build Closet Mesh** (simple version)
   - Right-click Closet → 3D Object → Cube
   - Name: `Back`
   - Scale: `(1.5, 2, 0.1)`
   - Duplicate → Name: `Left`
   - Rotate Y: `90`, Position: `(-0.7, 0, 0.5)`
   - Duplicate → Name: `Right`
   - Position: `(0.7, 0, 0.5)`
   - Duplicate → Name: `Top`
   - Rotate X: `90`, Scale: `(1.5, 1, 0.1)`, Position: `(0, 1, 0.5)`

3. **Create Trigger Zone**
   - Right-click Closet → 3D Object → Cube
   - Name: `HidingTrigger`
   - Position: `(0, 0.5, 0.5)` (inside closet)
   - Scale: `(1, 1.8, 0.8)` (player-sized)
   - Remove MeshRenderer (invisible)
   - BoxCollider:
     - Is Trigger: `✓`

4. **HidingSpot Component**
   - Select Closet
   - Add Component → Unity Mechanics Toolkit → Interaction → Hiding Spot
   - Configure:
     - Hiding Transform: Drag `HidingTrigger`
     - Enter Key: `E`
     - Exit Key: `E`
     - Hide Camera: `✓`
     - Camera Transform: Drag Player → PlayerCamera
     - Camera Hiding Position: `(0, 1.6, 0.3)` (inside closet view)
     - Disable Player Movement: `✓`
     - Movement Script: Drag Player → FirstPersonController

5. **Test hiding**
   - Press Play
   - Walk to closet (prompt: "Press E to Hide")
   - Press E → player teleports inside, camera adjusts
   - Press E again → exits hiding spot

---

## Step 6: Create Custom Interactable

### 6.1 Script Example: Item Pickup

```csharp
using UnityEngine;
using UnityMechanicsToolkit.Core;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Medkit";
    [SerializeField] private int quantity = 1;

    public bool CanInteract => true;

    public void Interact(GameObject interactor)
    {
        // Add to inventory logic here
        Debug.Log($"Picked up {itemName} x{quantity}");

        // Destroy pickup
        Destroy(gameObject);
    }

    public string GetInteractionPrompt()
    {
        return $"Press E to pick up {itemName}";
    }
}
```

### 6.2 Apply Script

1. **Create Pickup Object**
   - GameObject → 3D Object → Sphere
   - Name: `Medkit`
   - Scale: `(0.3, 0.3, 0.3)`
   - Position: `(3, 0.5, 2)`

2. **Add Script**
   - Select Medkit
   - Add Component → ItemPickup (drag script from Project)
   - Configure:
     - Item Name: `Medkit`
     - Quantity: `1`

3. **Add Highlight** (optional)
   - Add Component → Highlight System
   - Wire to RaycastInteractor

4. **Test pickup**
   - Press Play
   - Look at medkit (prompt: "Press E to pick up Medkit")
   - Press E → item disappears, console logs

---

## Step 7: Advanced - Locked Door with Key

### 7.1 Lock Door

1. **Modify Door**
   - Select Door
   - DoorController:
     - Is Locked: `✓`
     - Required Key Name: `"Rusty Key"`

2. **Test locked door**
   - Press Play
   - Try to open door (prompt: "Locked - Requires Rusty Key")

### 7.2 Create Key Pickup

```csharp
using UnityEngine;
using UnityMechanicsToolkit.Core;
using UnityMechanicsToolkit.Interaction;

public class KeyPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string keyName = "Rusty Key";

    public bool CanInteract => true;

    public void Interact(GameObject interactor)
    {
        // Store key in player (simple example)
        PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddKey(keyName);
        }

        Destroy(gameObject);
    }

    public string GetInteractionPrompt()
    {
        return $"Press E to pick up {keyName}";
    }
}
```

### 7.3 Simple Player Inventory

```csharp
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> keys = new HashSet<string>();

    public void AddKey(string keyName)
    {
        keys.Add(keyName);
        Debug.Log($"Obtained: {keyName}");
    }

    public bool HasKey(string keyName)
    {
        return keys.Contains(keyName);
    }
}
```

### 7.4 Modify DoorController Check

Modify `DoorController.cs` (or create derived class):
```csharp
public override void Interact(GameObject interactor)
{
    if (isLocked)
    {
        PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();
        if (inventory != null && inventory.HasKey(requiredKeyName))
        {
            isLocked = false;
            Debug.Log($"Unlocked door with {requiredKeyName}");
        }
        else
        {
            Debug.Log($"Door is locked. Requires {requiredKeyName}");
            return;
        }
    }

    ToggleDoor();
}
```

---

## ✅ Final Scene Hierarchy

```
Scene
├── Player (with RaycastInteractor)
│   └── PlayerCamera
├── Door (DoorController + HighlightSystem)
├── Key (ExamineObject + HighlightSystem)
├── Closet (HidingSpot)
│   └── HidingTrigger
├── Medkit (ItemPickup + HighlightSystem)
└── Environment (walls, floor, props)
```

---

## 🎮 Interaction Summary

| Object Type | Component | Interaction |
|-------------|-----------|-------------|
| Door | DoorController | Open/Close, Lock/Unlock |
| Key | ExamineObject | Rotate, Zoom, Inspect |
| Closet | HidingSpot | Enter/Exit hiding |
| Medkit | ItemPickup | Pick up item |
| Custom | IInteractable | Define custom behavior |

---

## 🧪 Testing Scenarios

### Test 1: Basic Interaction
- [ ] Raycast detects objects within range
- [ ] Interaction prompt appears
- [ ] Press E triggers interaction

### Test 2: Visual Feedback
- [ ] Objects glow when hovered
- [ ] Glow fades when looking away
- [ ] Highlight color visible in dark scenes

### Test 3: Door Mechanics
- [ ] Door opens/closes smoothly
- [ ] Locked door shows "Locked" message
- [ ] Unlocking door with key works

### Test 4: Examine System
- [ ] Object zooms to camera
- [ ] Can rotate with mouse
- [ ] Can zoom with scroll
- [ ] ESC returns object to world

### Test 5: Hiding Spot
- [ ] Player enters hiding spot
- [ ] Camera adjusts to hiding view
- [ ] Player movement disabled while hidden
- [ ] Can exit hiding spot

---

## 🎨 Polish Ideas

### 1. Audio Feedback

Add AudioSource to interactables:
```csharp
[SerializeField] private AudioClip interactionSound;
private AudioSource audioSource;

public void Interact(GameObject interactor)
{
    if (audioSource != null && interactionSound != null)
    {
        audioSource.PlayOneShot(interactionSound);
    }

    // Interaction logic...
}
```

### 2. Animation Integration

For doors, add Animator:
```csharp
private Animator animator;

void Awake()
{
    animator = GetComponent<Animator>();
}

private void OpenDoor()
{
    if (animator != null)
    {
        animator.SetTrigger("Open");
    }
}
```

### 3. UI Tooltips

Create Canvas with TextMeshProUGUI:
```csharp
public class InteractionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;

    public void ShowPrompt(string text)
    {
        promptText.text = text;
        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        promptText.gameObject.SetActive(false);
    }
}
```

### 4. Item Preview Icons

In ExamineObject, add UI image:
```csharp
[SerializeField] private Sprite itemIcon;
[SerializeField] private Image iconUI;

private void OnExamineStart()
{
    if (iconUI != null && itemIcon != null)
    {
        iconUI.sprite = itemIcon;
        iconUI.enabled = true;
    }
}
```

---

## 🔧 Common Issues

### Raycast not detecting objects
- **Check**: Object has Collider?
- **Check**: Object on correct layer?
- **Check**: Interaction range sufficient?

### Highlight not working
- **Check**: Material uses Standard shader?
- **Check**: Emission enabled in material?
- **Check**: Events wired in RaycastInteractor?

### Examine object clips through camera
- **Check**: Examine distance > 0.5?
- **Check**: Camera near clip plane < 0.3?
- **Check**: Object scale appropriate?

### Hiding spot doesn't hide player
- **Check**: Hiding Transform set?
- **Check**: Camera Transform assigned?
- **Check**: Movement Script reference correct?

### Door doesn't open
- **Check**: Rigidbody set to Kinematic?
- **Check**: Hinge point correct?
- **Check**: Door not blocked by colliders?

---

## 🚀 Next Steps

- **Tutorial 04**: Combat & Weapons Systems
- **Tutorial 05**: Complete Horror Scene Assembly

---

## 🎓 Advanced: Custom IInteractable

Create your own interactable behaviors:

```csharp
using UnityEngine;
using UnityMechanicsToolkit.Core;

public class ElectricSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light[] lightsToToggle;
    [SerializeField] private AudioClip switchSound;
    private bool isOn = false;

    public bool CanInteract => true;

    public void Interact(GameObject interactor)
    {
        isOn = !isOn;

        foreach (Light light in lightsToToggle)
        {
            light.enabled = isOn;
        }

        // Play sound
        AudioSource.PlayClipAtPoint(switchSound, transform.position);

        Debug.Log($"Lights: {(isOn ? "ON" : "OFF")}");
    }

    public string GetInteractionPrompt()
    {
        return $"Press E to turn lights {(isOn ? "OFF" : "ON")}";
    }
}
```

**Use cases:**
- Light switches
- Levers and buttons
- Computer terminals
- Puzzle mechanisms
- Quest triggers

---

**Congratulations!** You have a complete interaction system 🎉
