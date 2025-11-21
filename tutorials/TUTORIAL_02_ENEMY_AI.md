# Tutorial 02 - Enemy AI Setup

**Goal**: Create a functional horror game enemy with patrol, chase, and attack behaviors.

**Time**: 15 minutes

---

## 🎯 What You'll Build

An enemy that:
- Patrols between waypoints
- Detects player with vision cone
- Chases player when detected
- Attacks when in range
- Returns to patrol when player escapes

---

## 📋 Prerequisites

- Unity 2021.3+
- Unity Mechanics Toolkit imported
- Navigation system setup (NavMesh baked)
- Player from Tutorial 01

---

## Step 1: Setup Navigation

1. **Create Ground Plane**
   - GameObject → 3D Object → Plane
   - Name: `Ground`
   - Scale: `(10, 1, 10)` (large play area)

2. **Bake NavMesh**
   - Window → AI → Navigation
   - Select Ground plane
   - Check `Navigation Static`
   - Navigation window → Bake tab
   - Click `Bake`
   - Blue overlay appears = walkable surface

---

## Step 2: Create Enemy GameObject

1. **Create Enemy**
   - GameObject → 3D Object → Capsule
   - Name: `Enemy`
   - Position: `(5, 1, 0)`
   - Add Rigidbody:
     - Use Gravity: `✓`
     - Is Kinematic: `✓` (NavMesh controls movement)

2. **Add NavMeshAgent**
   - Select Enemy
   - Add Component → Nav Mesh Agent
   - Configure:
     - Speed: `3.5` (will be overridden by behaviors)
     - Angular Speed: `120`
     - Acceleration: `8`
     - Stopping Distance: `0.5`
     - Auto Braking: `✓`

---

## Step 3: Add AI State Machine

1. **AIStateMachine Component**
   - Select Enemy
   - Add Component → Unity Mechanics Toolkit → AI → AI State Machine
   - Configure:
     - Current State: `Patrol` (starting state)

2. **This is the brain** - all other AI components will use it

---

## Step 4: Add Vision Cone

1. **VisionCone Component**
   - Select Enemy
   - Add Component → Unity Mechanics Toolkit → AI → Vision Cone
   - Configure:
     - Vision Range: `10`
     - Vision Angle: `90` (45° each side)
     - Target Layer: `Default` (player layer)
     - Obstruction Layer: `Default` (walls block vision)
     - Detection Interval: `0.2` (checks every 0.2s)
     - Target Tag: `Player`

2. **Wire Events** (Inspector):
   - On Target Detected → AIStateMachine → StartChase()
   - On Target Lost → AIStateMachine → StartSearch()

3. **Test vision cone**
   - Press Play
   - Gizmos show vision cone (yellow/red)
   - Enemy detects player in cone

---

## Step 5: Add Patrol Behavior

1. **Create Waypoints**
   - Create Empty → Name: `Waypoint1`
   - Position: `(5, 0, 0)`
   - Duplicate (Ctrl+D) → Name: `Waypoint2`
   - Position: `(10, 0, 5)`
   - Duplicate → Name: `Waypoint3`
   - Position: `(5, 0, 10)`
   - Duplicate → Name: `Waypoint4`
   - Position: `(0, 0, 5)`

2. **PatrolBehavior Component**
   - Select Enemy
   - Add Component → Unity Mechanics Toolkit → AI → Patrol Behavior
   - Configure:
     - Waypoints: Size `4`
       - Element 0: Drag `Waypoint1`
       - Element 1: Drag `Waypoint2`
       - Element 2: Drag `Waypoint3`
       - Element 3: Drag `Waypoint4`
     - Waypoint Reached Distance: `0.5`
     - Wait Time At Waypoint: `2` (seconds)
     - Loop Waypoints: `✓`
     - Random Order: `☐` (sequential)
     - Patrol Speed: `2`

3. **Test patrol**
   - Press Play
   - Enemy walks between waypoints
   - Gizmos show patrol path (blue lines)

---

## Step 6: Add Chase Behavior

1. **ChaseBehavior Component**
   - Select Enemy
   - Add Component → Unity Mechanics Toolkit → AI → Chase Behavior
   - Configure:
     - Chase Speed: `5` (faster than patrol)
     - Attack Range: `2` (stops to attack at 2m)
     - Give Up Distance: `20` (gives up if >20m away)
     - Lost Target Search Time: `5` (searches for 5s at last position)

2. **This auto-integrates** with VisionCone and AIStateMachine

3. **Test chase**
   - Press Play
   - Walk into enemy's vision cone
   - Enemy chases player
   - Stops at attack range (2m)

---

## Step 7: Add Health (Make Killable)

1. **Health Component**
   - Select Enemy
   - Add Component → Unity Mechanics Toolkit → Combat → Health
   - Configure:
     - Max Health: `50`
     - Invulnerable: `☐`
     - Can Regenerate: `☐`

2. **Wire Death Event** (Inspector):
   - On Death → AIStateMachine → Die()
   - On Death → Enemy GameObject → SetActive(false) (or destroy)

---

## Step 8: Visualize States (Optional)

1. **Create UI Text**
   - GameObject → UI → Text - TextMeshPro
   - Name: `EnemyStateDebug`
   - Position above enemy

2. **Script to display state** (simple example):
```csharp
using TMPro;
using UnityEngine;

public class AIStateDebug : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public TextMeshProUGUI text;

    void Update()
    {
        if (stateMachine != null && text != null)
        {
            text.text = $"State: {stateMachine.CurrentState}";
        }
    }
}
```

---

## ✅ Final Enemy Hierarchy

```
Enemy (Capsule, NavMeshAgent, Rigidbody)
├── AIStateMachine
├── VisionCone
├── PatrolBehavior
├── ChaseBehavior
└── Health
```

---

## 🎮 AI Behavior Flow

```
START
  ↓
PATROL (walk waypoints)
  ↓ (player in vision cone)
CHASE (follow player)
  ↓ (in attack range)
ATTACK (stop and attack)
  ↓ (player escapes)
SEARCH (go to last known position)
  ↓ (timeout)
PATROL (resume patrol)
```

---

## 🧪 Testing Scenarios

### Test 1: Basic Patrol
- [ ] Enemy patrols between waypoints
- [ ] Waits at each waypoint
- [ ] Loops back to first waypoint

### Test 2: Detection
- [ ] Walk into vision cone from front
- [ ] Enemy changes to Chase state
- [ ] Enemy follows player

### Test 3: Attack Range
- [ ] Enemy stops at attack range (2m)
- [ ] Doesn't overshoot player

### Test 4: Escape
- [ ] Run far away (>20m)
- [ ] Enemy gives up chase
- [ ] Returns to patrol

### Test 5: Line of Sight
- [ ] Hide behind wall
- [ ] Enemy loses vision
- [ ] Goes to last known position
- [ ] Searches, then patrols

### Test 6: Health
- [ ] Damage enemy (via script or weapon)
- [ ] Enemy dies at 0 health
- [ ] Death event triggers

---

## 🎨 Polish Ideas

### 1. Add Attack Logic

```csharp
// In AIStateMachine, wire onStateAttack event
public void Attack()
{
    GameObject player = FindObjectOfType<Player>().gameObject;
    IDamageable playerHealth = player.GetComponent<IDamageable>();

    if (playerHealth != null)
    {
        playerHealth.TakeDamage(10f);
    }
}
```

### 2. Add Audio

- Footstep sounds (use FootstepSystem on enemy)
- Growl/Chase sounds
- Attack sounds

### 3. Add Animations

- Idle animation
- Walk animation
- Run animation
- Attack animation

Use Animator with parameters:
- `Speed` (float) - 0 = idle, 1 = walk, 2 = run
- `Attack` (trigger)

### 4. Add Visual Feedback

- Red glow when chasing
- Health bar above enemy
- Damage numbers on hit

---

## 🔧 Common Issues

### Enemy doesn't move
- **Check**: NavMesh baked?
- **Check**: NavMeshAgent component present?
- **Check**: Ground marked as Navigation Static?

### Enemy doesn't detect player
- **Check**: Player tagged as "Player"?
- **Check**: Player on Target Layer?
- **Check**: Player in vision cone? (check Gizmos)

### Enemy chases through walls
- **Check**: Obstruction Layer set?
- **Check**: Walls have colliders?
- **Check**: NavMesh excludes walls?

### Enemy stops mid-patrol
- **Check**: Waypoints set correctly?
- **Check**: Patrol Speed > 0?
- **Check**: NavMeshAgent not stuck?

---

## 🚀 Next Steps

- **Tutorial 03**: Hiding & Interaction Systems
- **Tutorial 04**: Combat & Weapons
- **Tutorial 05**: Complete Horror Level

---

## 🎓 Advanced: Multiple Enemies

To spawn multiple enemies:

1. **Make Enemy a Prefab**
   - Drag Enemy to Project window
   - Creates reusable prefab

2. **Duplicate in Scene**
   - Each enemy needs own waypoints
   - Or share waypoints (all patrol same path)

3. **Enemy Manager** (optional)
   - Script to spawn enemies
   - Track active enemies
   - Adjust difficulty

---

**Congratulations!** You have a functional AI enemy 🎉
