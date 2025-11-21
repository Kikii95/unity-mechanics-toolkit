# Tutorial 04 - Combat Systems

**Goal**: Implement melee combat with weapons, blocking, dodging, and damage systems.

**Time**: 15 minutes

---

## 🎯 What You'll Build

A combat system with:
- Melee weapon attacks (bat, pipe, knife)
- Block mechanic (damage reduction)
- Dodge rolls (i-frames)
- Stamina-based combat
- Durability system
- Health and damage

---

## 📋 Prerequisites

- Unity 2021.3+
- Unity Mechanics Toolkit imported
- Player from Tutorial 01
- Basic scene with enemies or targets

---

## Step 1: Add Health to Player

### 1.1 Health Component

1. **Health System**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Combat → Health
   - Configure:
     - Max Health: `100`
     - Current Health: `100` (auto-filled)
     - Invulnerable: `☐`
     - Can Regenerate: `☐` (or `✓` for regen)
     - Regen Rate: `5` (per second, if enabled)
     - Regen Delay: `3` (seconds after damage)

2. **Test health** (via code/debug)
   ```csharp
   // In any script
   Health playerHealth = FindObjectOfType<Health>();
   playerHealth.TakeDamage(20f); // Test damage
   ```

---

## Step 2: Create Melee Weapon

### 2.1 Build Weapon GameObject

1. **Create Weapon (Bat)**
   - Right-click Player → 3D Object → Cube
   - Name: `Bat`
   - Position relative to Player: `(0.3, 0, 0.5)` (right side, in front)
   - Rotation: `(0, 0, 45)` (tilted)
   - Scale: `(0.1, 1, 0.1)` (long thin stick)

2. **Make it look like a weapon**
   - Create material: `BatMaterial`
   - Color: Gray or brown
   - Assign to Bat

3. **Configure Collider**
   - Bat already has BoxCollider
   - Set `Is Trigger` to `✓` (detects hits without physics)

### 2.2 Add MeleeWeapon Component

1. **MeleeWeapon Component**
   - Select Bat
   - Add Component → Unity Mechanics Toolkit → Combat → Melee Weapon
   - Configure:
     - Attack Key: `Mouse0` (Left Click)
     - Damage: `25`
     - Attack Range: `2` (raycast distance)
     - Attack Cooldown: `1` (seconds between attacks)
     - Stamina Cost: `15`
     - Max Durability: `100`
     - Durability Loss Per Hit: `5`
     - Swing Duration: `0.3` (animation time)

2. **Test weapon**
   - Press Play
   - Left Click to attack
   - Console logs: "Melee Attack!"

---

## Step 3: Create Damage Target (Dummy)

### 3.1 Build Target

1. **Create Dummy**
   - GameObject → 3D Object → Capsule
   - Name: `Dummy`
   - Position: `(3, 1, 0)` (in front of player)
   - Tag: `Enemy` (optional, for targeting)

2. **Make it visible**
   - Create material: `DummyMaterial`
   - Color: Red (hostile)
   - Assign to Dummy

3. **Add Health**
   - Select Dummy
   - Add Component → Unity Mechanics Toolkit → Combat → Health
   - Configure:
     - Max Health: `50`
     - Invulnerable: `☐`

4. **Wire Death Event** (optional)
   - Health → On Death → Dummy GameObject → SetActive(false)
   - Or: On Death → Dummy → Destroy (via script)

### 3.2 Test Combat

1. **Play and attack**
   - Press Play
   - Walk close to dummy (within 2m)
   - Left Click to attack
   - Dummy takes damage (check Health in Inspector)
   - At 0 HP → Dummy dies (deactivates/destroys)

---

## Step 4: Add Block Mechanic

### 4.1 BlockDodge Component

1. **Add to Player**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Combat → Block Dodge
   - Configure Block Settings:
     - Block Key: `Mouse1` (Right Click)
     - Block Damage Reduction: `0.5` (50% less damage)
     - Block Stamina Drain: `10` (per second while blocking)

2. **Test blocking**
   - Press Play
   - Hold Right Click → player blocks
   - Stamina drains while blocking
   - Release → stops blocking

### 4.2 Integrate with Health

Modify your damage source to check for blocking:

```csharp
// Example: Enemy attack script
public void AttackPlayer(GameObject player)
{
    BlockDodge blockDodge = player.GetComponent<BlockDodge>();
    float damage = 20f;

    // Apply block reduction if blocking
    if (blockDodge != null && blockDodge.IsBlocking)
    {
        damage = blockDodge.ApplyBlockReduction(damage);
        Debug.Log("Attack blocked! Reduced damage.");
    }

    Health playerHealth = player.GetComponent<Health>();
    if (playerHealth != null)
    {
        playerHealth.TakeDamage(damage);
    }
}
```

---

## Step 5: Add Dodge Mechanic

### 5.1 Configure Dodge

1. **BlockDodge Dodge Settings**
   - Select Player → BlockDodge component
   - Configure Dodge Settings:
     - Dodge Key: `Left Control`
     - Dodge Distance: `3` (roll distance)
     - Dodge Duration: `0.5` (seconds)
     - Dodge Cooldown: `1` (seconds)
     - Dodge Stamina Cost: `20`

2. **Test dodge**
   - Press Play
   - Press Left Ctrl → player dodges backward (or direction of movement)
   - During dodge: Player moves rapidly
   - Stamina cost deducted

### 5.2 Invulnerability During Dodge (i-frames)

Modify Health script to check dodge state:

```csharp
// In Health.cs (modify TakeDamage)
public void TakeDamage(float amount)
{
    // Check if dodging (i-frames)
    BlockDodge blockDodge = GetComponent<BlockDodge>();
    if (blockDodge != null && blockDodge.IsDodging)
    {
        Debug.Log("Damage avoided - dodging!");
        return; // No damage during dodge
    }

    // Normal damage logic
    currentHealth -= amount;
    onHealthChanged?.Invoke(currentHealth, maxHealth);

    if (currentHealth <= 0)
    {
        Die();
    }
}
```

---

## Step 6: Weapon Durability System

### 6.1 Durability Mechanics

Already integrated in MeleeWeapon:
- Each hit reduces durability by `Durability Loss Per Hit`
- At 0 durability → weapon breaks (can't attack)

### 6.2 Repair System (Optional)

```csharp
// Add to MeleeWeapon or separate script
public void RepairWeapon(float amount)
{
    currentDurability = Mathf.Min(currentDurability + amount, maxDurability);
    Debug.Log($"Weapon repaired: {currentDurability}/{maxDurability}");
}

// Full repair
public void FullRepair()
{
    currentDurability = maxDurability;
}
```

Call via item pickup:
```csharp
// In RepairKitPickup script
public void Interact(GameObject interactor)
{
    MeleeWeapon weapon = interactor.GetComponentInChildren<MeleeWeapon>();
    if (weapon != null)
    {
        weapon.FullRepair();
        Destroy(gameObject);
    }
}
```

---

## Step 7: Advanced - Combo Attacks

### 7.1 Simple Combo System

```csharp
using UnityEngine;
using UnityMechanicsToolkit.Combat;

public class ComboSystem : MonoBehaviour
{
    [SerializeField] private MeleeWeapon weapon;
    [SerializeField] private float comboWindow = 1.5f;
    [SerializeField] private float[] comboDamageMultipliers = { 1f, 1.2f, 1.5f }; // Light, Medium, Heavy

    private int comboCount = 0;
    private float lastAttackTime;

    void Update()
    {
        // Reset combo if timeout
        if (Time.time > lastAttackTime + comboWindow)
        {
            comboCount = 0;
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PerformCombo();
        }
    }

    void PerformCombo()
    {
        if (comboCount >= comboDamageMultipliers.Length)
        {
            comboCount = 0; // Reset after max combo
        }

        float damageMultiplier = comboDamageMultipliers[comboCount];
        Debug.Log($"Combo {comboCount + 1}: {damageMultiplier}x damage");

        // Apply damage (manual attack trigger)
        // weapon.Attack(); // Or custom logic with multiplier

        comboCount++;
        lastAttackTime = Time.time;
    }
}
```

---

## Step 8: Advanced - Heavy/Light Attacks

```csharp
using UnityEngine;
using UnityMechanicsToolkit.Combat;
using UnityMechanicsToolkit.Core;

public class AdvancedMelee : MonoBehaviour
{
    [Header("Light Attack")]
    [SerializeField] private KeyCode lightAttackKey = KeyCode.Mouse0;
    [SerializeField] private float lightDamage = 15f;
    [SerializeField] private float lightStaminaCost = 10f;
    [SerializeField] private float lightCooldown = 0.5f;

    [Header("Heavy Attack")]
    [SerializeField] private KeyCode heavyAttackKey = KeyCode.Mouse1;
    [SerializeField] private float heavyDamage = 40f;
    [SerializeField] private float heavyStaminaCost = 30f;
    [SerializeField] private float heavyCooldown = 2f;

    private StaminaSystem stamina;
    private float lastLightAttack;
    private float lastHeavyAttack;

    void Awake()
    {
        stamina = GetComponent<StaminaSystem>();
    }

    void Update()
    {
        // Light attack
        if (Input.GetKeyDown(lightAttackKey) && Time.time > lastLightAttack + lightCooldown)
        {
            if (stamina.TryConsume(lightStaminaCost))
            {
                PerformAttack(lightDamage, "Light");
                lastLightAttack = Time.time;
            }
        }

        // Heavy attack (hold Right Click)
        if (Input.GetKeyDown(heavyAttackKey) && Time.time > lastHeavyAttack + heavyCooldown)
        {
            if (stamina.TryConsume(heavyStaminaCost))
            {
                PerformAttack(heavyDamage, "Heavy");
                lastHeavyAttack = Time.time;
            }
        }
    }

    void PerformAttack(float damage, string attackType)
    {
        Debug.Log($"{attackType} Attack: {damage} damage");

        // Raycast for targets
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
```

---

## ✅ Final Combat Setup

```
Player
├── Health
├── StaminaSystem
├── BlockDodge
├── Bat (MeleeWeapon)
└── PlayerCamera

Dummy (Target)
└── Health
```

---

## 🎮 Combat Controls Summary

| Action | Key | Effect |
|--------|-----|--------|
| Light Attack | Left Click | 25 damage, 15 stamina |
| Block | Right Click (Hold) | 50% damage reduction, drains stamina |
| Dodge | Left Ctrl | 3m roll, 20 stamina, i-frames |

---

## 🧪 Testing Scenarios

### Test 1: Basic Attack
- [ ] Walk to dummy (within 2m)
- [ ] Left Click to attack
- [ ] Dummy takes 25 damage
- [ ] Stamina depletes by 15

### Test 2: Blocking
- [ ] Hold Right Click
- [ ] Take damage from enemy
- [ ] Damage reduced by 50%
- [ ] Stamina drains while blocking

### Test 3: Dodge
- [ ] Press Left Ctrl
- [ ] Player rolls (moves 3m)
- [ ] Stamina cost: 20
- [ ] Can't spam (1s cooldown)

### Test 4: Durability
- [ ] Attack 20 times (100 durability / 5 loss = 20 hits)
- [ ] Weapon breaks (0 durability)
- [ ] Can't attack when broken

### Test 5: Stamina Integration
- [ ] Deplete stamina completely
- [ ] Try to attack → blocked (not enough stamina)
- [ ] Try to dodge → blocked
- [ ] Try to block → works but stops when depleted

---

## 🎨 Polish Ideas

### 1. Hit Effects

```csharp
// In MeleeWeapon, add VFX on hit
[SerializeField] private ParticleSystem hitEffect;

void OnHit(Vector3 hitPoint)
{
    if (hitEffect != null)
    {
        Instantiate(hitEffect, hitPoint, Quaternion.identity);
    }
}
```

### 2. Damage Numbers (UI)

```csharp
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public static void Create(Vector3 position, float damage)
    {
        GameObject obj = new GameObject("DamageNumber");
        obj.transform.position = position;

        TextMeshPro text = obj.AddComponent<TextMeshPro>();
        text.text = damage.ToString("F0");
        text.fontSize = 5;
        text.color = Color.red;
        text.alignment = TextAlignmentOptions.Center;

        Destroy(obj, 1f); // Auto-destroy after 1s
    }
}

// Call on damage
DamageNumber.Create(hit.point, damage);
```

### 3. Screen Shake on Hit

```csharp
// In MeleeWeapon
[SerializeField] private CameraShake cameraShake;

void OnAttackHit()
{
    if (cameraShake != null)
    {
        cameraShake.Shake(0.2f, 0.3f);
    }
}
```

### 4. Blood Splatter Decals

```csharp
[SerializeField] private GameObject bloodDecalPrefab;

void OnHit(RaycastHit hit)
{
    Instantiate(bloodDecalPrefab, hit.point, Quaternion.LookRotation(hit.normal));
}
```

---

## 🔧 Common Issues

### Weapon doesn't damage enemies
- **Check**: Enemy has Health component?
- **Check**: Enemy implements IDamageable?
- **Check**: Attack range sufficient (2m)?
- **Check**: Weapon collider is Trigger?

### Can't attack when have stamina
- **Check**: Attack cooldown expired?
- **Check**: Stamina Cost < Current Stamina?
- **Check**: Weapon durability > 0?

### Blocking doesn't reduce damage
- **Check**: Enemy uses BlockDodge.ApplyBlockReduction()?
- **Check**: IsBlocking property checked before damage?

### Dodge doesn't avoid damage
- **Check**: Health checks IsDodging before TakeDamage?
- **Check**: Dodge duration > attack timing?

### Weapon breaks immediately
- **Check**: Max Durability > 0?
- **Check**: Durability Loss Per Hit reasonable?
- **Check**: Not hitting multiple enemies per swing?

---

## 🚀 Next Steps

- **Tutorial 05**: Complete Horror Scene Assembly
- **Integrate**: Combine with enemy AI (Tutorial 02)
- **Expand**: Add ranged weapons, parry system, finishers

---

## 🎓 Advanced: Multiple Weapons

Create weapon switching system:

```csharp
using UnityEngine;
using UnityMechanicsToolkit.Combat;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private MeleeWeapon[] weapons;
    private int currentWeaponIndex = 0;

    void Start()
    {
        SwitchWeapon(0);
    }

    void Update()
    {
        // Scroll wheel to switch
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SwitchWeapon((currentWeaponIndex + 1) % weapons.Length);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SwitchWeapon((currentWeaponIndex - 1 + weapons.Length) % weapons.Length);
        }

        // Number keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
    }

    void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        // Disable all
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        // Enable selected
        weapons[index].gameObject.SetActive(true);
        currentWeaponIndex = index;

        Debug.Log($"Switched to {weapons[index].name}");
    }
}
```

---

**Congratulations!** You have a complete combat system 🎉
