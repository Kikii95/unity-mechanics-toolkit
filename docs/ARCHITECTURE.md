# Unity Mechanics Toolkit - Architecture Guide

**Philosophy**: Ultra-modular, zero-dependency, plug & play components for maximum flexibility and performance.

---

## 🎯 Core Principles

### 1. Modularity First
- **Each system is standalone** - Can be used independently
- **No forced dependencies** - Optional integrations only
- **Import what you need** - Cherry-pick modules
- **No bloat** - Each feature is separate

### 2. Plug & Play Integration
- **Drag & Drop components** - Add to GameObject, configure, done
- **Sensible defaults** - Works out of the box
- **Inspector-friendly** - All settings visible in Unity Inspector
- **Runtime configuration** - Modify at runtime via code

### 3. Performance Optimized
- **Object Pooling** - Reuse objects (bullets, particles, enemies)
- **Caching** - Cache GetComponent calls
- **Minimal GC** - Avoid allocations in Update/FixedUpdate
- **LOD support** - Distance-based optimizations
- **Conditional compilation** - Debug code stripped in builds

### 4. Separation of Concerns
- **Single Responsibility** - Each class does ONE thing
- **Interface-based** - Loose coupling via interfaces
- **Event-driven** - UnityEvents for communication
- **Data-oriented** - ScriptableObjects for configuration

---

## 🏗️ Architecture Patterns

### Pattern 1: Component-Based Design

Each mechanic is a **MonoBehaviour component** that can be added to any GameObject.

```csharp
// Example: Health component
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

    // Clean API
    public void TakeDamage(float amount) { }
    public void Heal(float amount) { }
}
```

**Benefits**:
- Unity-native workflow
- Inspector configuration
- Serialization support
- Easy to understand

---

### Pattern 2: ScriptableObject Configurations

Settings and data stored in **ScriptableObjects** for reusability.

```csharp
[CreateAssetMenu(fileName = "WeaponData", menuName = "Toolkit/Combat/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public int magazineSize;
    public AudioClip shootSound;
}

// Usage
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData data;
}
```

**Benefits**:
- Reusable configurations
- Designer-friendly
- Easy balancing (no code changes)
- Hot-reload in editor

---

### Pattern 3: Interface-Based Communication

Systems communicate via **interfaces** to avoid hard dependencies.

```csharp
// Interface
public interface IDamageable
{
    void TakeDamage(float amount);
    float CurrentHealth { get; }
}

// Implementation
public class Health : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount) { }
    public float CurrentHealth => currentHealth;
}

// Usage (no dependency on Health class)
public class Weapon : MonoBehaviour
{
    void Shoot(RaycastHit hit)
    {
        IDamageable target = hit.collider.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
}
```

**Benefits**:
- Zero coupling
- Testable
- Flexible implementations
- Easy mocking

---

### Pattern 4: Event-Driven Architecture

Use **UnityEvents** for loose coupling between systems.

```csharp
public class Health : MonoBehaviour
{
    public UnityEvent<float> onDamaged;
    public UnityEvent onDeath;

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        onDamaged?.Invoke(amount);

        if (currentHealth <= 0)
            onDeath?.Invoke();
    }
}

// Other systems listen via Inspector or code
public class HealthUI : MonoBehaviour
{
    void Start()
    {
        health.onDamaged.AddListener(UpdateHealthBar);
    }
}
```

**Benefits**:
- Inspector wiring
- No code dependencies
- Easy debugging
- Visual workflow

---

### Pattern 5: Manager Singletons (Optional)

For **optional global systems** (Audio, Save, Input), use lazy singletons.

```csharp
public class AudioManager : Singleton<AudioManager>
{
    public void PlaySound(AudioClip clip, Vector3 position) { }
}

// Usage (only if AudioManager exists in scene)
if (AudioManager.Instance != null)
{
    AudioManager.Instance.PlaySound(shootSound, transform.position);
}
```

**Benefits**:
- Global access (when needed)
- Optional (doesn't break if missing)
- DontDestroyOnLoad support

---

## 📦 Module Structure

Each module follows this structure:

```
src/<Module>/
├── Core/                    # Core classes (required)
│   ├── <Feature>.cs
│   └── I<Feature>.cs       # Interface
├── Data/                   # ScriptableObjects (configs)
│   └── <Feature>Data.cs
├── Events/                 # Custom events
│   └── <Feature>Events.cs
├── UI/                     # UI components (optional)
│   └── <Feature>UI.cs
├── Helpers/                # Utility classes
│   └── <Feature>Helper.cs
└── Examples/               # Example prefabs/scenes
    └── Example<Feature>.unity
```

**Example: Movement Module**

```
src/Movement/
├── Core/
│   ├── FirstPersonController.cs
│   ├── IMovementController.cs
│   ├── CrouchBehavior.cs
│   └── StaminaSystem.cs
├── Data/
│   └── MovementSettings.cs
├── Events/
│   └── MovementEvents.cs
└── Examples/
    └── ExampleFPS.unity
```

---

## 🔗 Dependency Management

### Rule 1: No Hard Dependencies

**❌ BAD** (hard dependency):
```csharp
public class Weapon : MonoBehaviour
{
    private Health targetHealth; // Hard dependency on Health class
}
```

**✅ GOOD** (interface dependency):
```csharp
public class Weapon : MonoBehaviour
{
    private IDamageable target; // Interface, no hard dependency
}
```

---

### Rule 2: Optional Features via Null Checks

**✅ GOOD** (optional integration):
```csharp
public class PlayerController : MonoBehaviour
{
    private StaminaSystem stamina; // Optional

    void Start()
    {
        stamina = GetComponent<StaminaSystem>(); // May be null
    }

    void Sprint()
    {
        if (stamina != null && stamina.CanConsume(sprintCost))
        {
            stamina.Consume(sprintCost);
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed; // Works without stamina too
        }
    }
}
```

---

### Rule 3: Event-Based Cross-Module Communication

When modules need to communicate, use **events** instead of direct references.

**Example: AI hears player footsteps**

```csharp
// Movement module emits event
public class FirstPersonController : MonoBehaviour
{
    public static event Action<Vector3, float> OnFootstep; // position, volume

    void Footstep()
    {
        OnFootstep?.Invoke(transform.position, isRunning ? 1f : 0.5f);
    }
}

// AI module listens (no dependency on Movement)
public class AIHearing : MonoBehaviour
{
    void OnEnable()
    {
        FirstPersonController.OnFootstep += OnPlayerFootstep;
    }

    void OnDisable()
    {
        FirstPersonController.OnFootstep -= OnPlayerFootstep;
    }

    void OnPlayerFootstep(Vector3 position, float volume)
    {
        if (Vector3.Distance(transform.position, position) < hearingRange * volume)
        {
            InvestigateSound(position);
        }
    }
}
```

---

## 🎨 Naming Conventions

### Classes
- **Components**: `<Feature>Controller`, `<Feature>System`, `<Feature>Behavior`
  - `FirstPersonController`, `HealthSystem`, `CrouchBehavior`
- **Interfaces**: `I<Feature>`, `I<Feature>able`
  - `IMovementController`, `IDamageable`, `IInteractable`
- **ScriptableObjects**: `<Feature>Data`, `<Feature>Settings`, `<Feature>Config`
  - `WeaponData`, `MovementSettings`, `AudioConfig`
- **Managers**: `<Feature>Manager` (only for global singletons)
  - `AudioManager`, `SaveManager`, `InputManager`

### Variables
- **Private fields**: `camelCase` with `[SerializeField]` if Inspector-visible
  - `private float moveSpeed;`
  - `[SerializeField] private float jumpForce;`
- **Public properties**: `PascalCase`
  - `public float MaxHealth { get; private set; }`
- **Events**: `on<Action>` (UnityEvent) or `On<Action>` (C# event)
  - `public UnityEvent onDeath;`
  - `public static event Action OnGameOver;`

### Methods
- **Public API**: `PascalCase`, descriptive verbs
  - `TakeDamage()`, `Heal()`, `StartChase()`, `ToggleDoor()`
- **Private helpers**: `PascalCase`
  - `HandleMovement()`, `UpdateStamina()`, `CheckGrounded()`

---

## 🚀 Integration Workflow

### Step 1: Import Module

**Option A: Full Module**
- Copy `src/<Module>/` to `Assets/UnityMechanicsToolkit/<Module>/`

**Option B: Cherry-Pick**
- Copy only needed scripts from `src/<Module>/Core/`

### Step 2: Add Component to GameObject

```
1. Select GameObject (e.g., Player)
2. Add Component → Unity Mechanics Toolkit → <Module> → <Component>
3. Configure in Inspector
4. Play!
```

### Step 3: Wire Events (Optional)

Connect UnityEvents in Inspector for cross-system communication:
- Health.onDeath → PlayerController.Die()
- Weapon.onShoot → AudioManager.PlaySound()

### Step 4: Code Integration (Optional)

For programmatic control:

```csharp
public class PlayerManager : MonoBehaviour
{
    private Health health;
    private StaminaSystem stamina;

    void Start()
    {
        health = GetComponent<Health>();
        stamina = GetComponent<StaminaSystem>();

        health.onDeath.AddListener(OnPlayerDeath);
    }

    void OnPlayerDeath()
    {
        // Custom logic
    }
}
```

---

## ⚡ Performance Best Practices

### 1. Cache Component References

**❌ BAD**:
```csharp
void Update()
{
    GetComponent<Rigidbody>().AddForce(force); // Called every frame!
}
```

**✅ GOOD**:
```csharp
private Rigidbody rb;

void Awake()
{
    rb = GetComponent<Rigidbody>(); // Cache once
}

void Update()
{
    rb.AddForce(force);
}
```

---

### 2. Avoid Allocations in Update/FixedUpdate

**❌ BAD**:
```csharp
void Update()
{
    Vector3 direction = new Vector3(x, y, z); // Allocates every frame
}
```

**✅ GOOD**:
```csharp
private Vector3 direction; // Reuse

void Update()
{
    direction.Set(x, y, z); // No allocation
}
```

---

### 3. Use Object Pooling

For frequently instantiated objects (bullets, particles, enemies):

```csharp
// Use built-in ObjectPool
ObjectPool pool = FindObjectOfType<ObjectPool>();
GameObject bullet = pool.SpawnFromPool("Bullet", spawnPos, spawnRot);

// Return to pool instead of Destroy
pool.ReturnToPool("Bullet", bullet);
```

---

### 4. Layer-Based Raycasts

Specify layers to reduce raycast cost:

```csharp
// ❌ BAD (checks all layers)
Physics.Raycast(origin, direction, out hit);

// ✅ GOOD (checks only specific layer)
int interactableLayer = LayerMask.GetMask("Interactable");
Physics.Raycast(origin, direction, out hit, maxDistance, interactableLayer);
```

---

### 5. Conditional Compilation for Debug Code

```csharp
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Visualize vision cone (editor only)
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
#endif
```

---

## 🧪 Common Interfaces

These interfaces allow cross-module communication:

```csharp
// Combat
public interface IDamageable
{
    void TakeDamage(float amount);
    float CurrentHealth { get; }
}

public interface IKillable
{
    void Die();
    bool IsDead { get; }
}

// Interaction
public interface IInteractable
{
    void Interact(GameObject interactor);
    string GetInteractionPrompt();
    bool CanInteract { get; }
}

// Movement
public interface IMovementController
{
    void Move(Vector3 direction);
    void Jump();
    bool IsGrounded { get; }
}

// AI
public interface IAITarget
{
    Vector3 Position { get; }
    bool IsVisible { get; }
}

// Inventory
public interface IPickupable
{
    void Pickup(GameObject picker);
    Item GetItem();
}

// Stealth
public interface IDetectable
{
    float VisibilityLevel { get; } // 0 = invisible, 1 = fully visible
    float NoiseLevel { get; }
}
```

---

## 📋 Module Checklist

When creating a new module, ensure:

- [ ] **Standalone** - Works without other modules
- [ ] **Interface-based** - Uses interfaces for external communication
- [ ] **ScriptableObject configs** - Settings externalized
- [ ] **UnityEvents** - For Inspector wiring
- [ ] **Null-safe** - Handles missing optional components
- [ ] **Cached references** - GetComponent called in Awake/Start
- [ ] **Performance optimized** - No allocations in Update
- [ ] **Documented** - XML comments on public API
- [ ] **Example scene** - Shows basic usage
- [ ] **Inspector-friendly** - All settings visible/configurable

---

## 🎯 Example: Fully Modular Health System

```csharp
using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Combat
{
    /// <summary>
    /// Modular health system with events and interface support.
    /// Can be used standalone or with other combat modules.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable, IKillable
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;
        [SerializeField] private bool invulnerable = false;

        [Header("Optional Regeneration")]
        [SerializeField] private bool canRegenerate = false;
        [SerializeField] private float regenRate = 5f;
        [SerializeField] private float regenDelay = 3f;

        [Header("Events (Optional)")]
        public UnityEvent<float> onHealthChanged;
        public UnityEvent<float> onDamaged;
        public UnityEvent<float> onHealed;
        public UnityEvent onDeath;

        // Public properties (API)
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public float HealthPercentage => currentHealth / maxHealth;
        public bool IsAlive => currentHealth > 0;
        public bool IsDead => currentHealth <= 0;

        private float timeSinceLastDamage;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            HandleRegeneration();
        }

        // IDamageable implementation
        public void TakeDamage(float amount)
        {
            if (invulnerable || IsDead) return;

            amount = Mathf.Abs(amount);
            currentHealth = Mathf.Max(0, currentHealth - amount);
            timeSinceLastDamage = 0f;

            onDamaged?.Invoke(amount);
            onHealthChanged?.Invoke(currentHealth);

            if (IsDead)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            if (IsDead) return;

            amount = Mathf.Abs(amount);
            float previousHealth = currentHealth;
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            float actualHealAmount = currentHealth - previousHealth;

            if (actualHealAmount > 0)
            {
                onHealed?.Invoke(actualHealAmount);
                onHealthChanged?.Invoke(currentHealth);
            }
        }

        // IKillable implementation
        public void Die()
        {
            if (IsDead)
            {
                onDeath?.Invoke();
            }
        }

        private void HandleRegeneration()
        {
            if (!canRegenerate || currentHealth >= maxHealth || IsDead) return;

            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= regenDelay)
            {
                Heal(regenRate * Time.deltaTime);
            }
        }

        public void SetInvulnerable(bool state)
        {
            invulnerable = state;
        }

        public void SetMaxHealth(float newMax, bool healToMax = false)
        {
            maxHealth = Mathf.Max(1f, newMax);

            if (healToMax)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = Mathf.Min(currentHealth, maxHealth);
            }

            onHealthChanged?.Invoke(currentHealth);
        }
    }
}
```

**Usage examples**:

```csharp
// 1. Drag & Drop - Add Health component in Inspector, configure, done!

// 2. Code access via interface
IDamageable target = hit.collider.GetComponent<IDamageable>();
if (target != null)
{
    target.TakeDamage(50f);
}

// 3. Event wiring (Inspector or code)
health.onDeath.AddListener(() => {
    Debug.Log("Player died!");
    Destroy(gameObject);
});

// 4. Direct API
Health playerHealth = player.GetComponent<Health>();
if (playerHealth.HealthPercentage < 0.3f)
{
    ShowLowHealthWarning();
}
```

---

## 🎓 Design Philosophy Summary

1. **Modularity** - Each feature is self-contained
2. **Flexibility** - Use only what you need
3. **Performance** - Optimized for production
4. **Simplicity** - Minimal setup, maximum productivity
5. **Unity-native** - Follows Unity conventions
6. **Designer-friendly** - Inspector-driven workflow
7. **Developer-friendly** - Clean API, well-documented
8. **Extensibility** - Easy to customize and extend

---

**Next**: See `INTEGRATION_GUIDE.md` for step-by-step integration examples.
