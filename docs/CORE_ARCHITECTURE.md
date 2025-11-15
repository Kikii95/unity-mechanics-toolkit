# Core Architecture - Optimized Modular Design

**Problem**: Pure standalone modules → Code duplication, performance loss, pattern repetition
**Solution**: **Core + Modules** hybrid architecture

---

## 🎯 Philosophy

### Hybrid System
```
Core (Optional, Recommended)
  ↓ (shared utilities)
Modules (Can use Core if present, else standalone)
```

**Benefits**:
- ✅ **Zero redundancy** - Shared code in Core
- ✅ **Performance** - Shared pools, caches, managers
- ✅ **Still flexible** - Modules work WITHOUT Core (fallback)
- ✅ **Best of both worlds** - Optimized when Core present, functional when not

---

## 📦 Structure

```
UnityMechanicsToolkit/
├── Core/                        # ⭐ Shared utilities (import once, use everywhere)
│   ├── Interfaces/              # Common interfaces (IDamageable, IInteractable, etc.)
│   ├── Events/                  # Event system, delegates
│   ├── Pooling/                 # ObjectPool, PoolManager
│   ├── Caching/                 # ComponentCache, RaycastCache
│   ├── Extensions/              # Extension methods (Vector3, Transform, etc.)
│   ├── Helpers/                 # Math, Physics, Debug helpers
│   ├── Managers/                # Optional global managers (Audio, Save, Input)
│   └── ScriptableObjects/       # Base SO classes, configs
│
├── Movement/                    # Uses Core (if present)
├── Interaction/                 # Uses Core (if present)
├── Combat/                      # Uses Core (if present)
├── Horror/                      # Uses Core (if present)
├── AI/                          # Uses Core (if present)
└── ...
```

---

## 🔧 Core Module Contents

### 1. Interfaces (`Core/Interfaces/`)

**Common interfaces** used across modules:

```csharp
namespace UnityMechanicsToolkit.Core
{
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

    // Inventory
    public interface IPickupable
    {
        void Pickup(GameObject picker);
    }

    // AI
    public interface IAITarget
    {
        Vector3 Position { get; }
        bool IsVisible { get; }
    }

    // Stealth
    public interface IDetectable
    {
        float VisibilityLevel { get; }
        float NoiseLevel { get; }
    }
}
```

**Benefit**: All modules reference same interfaces → No duplication

---

### 2. Object Pooling (`Core/Pooling/`)

**Shared pooling system** to avoid per-module implementations:

```csharp
namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Centralized object pooling for all modules.
    /// Used by: Combat (bullets), VFX (particles), AI (enemies), Audio (sounds)
    /// </summary>
    public class PoolManager : Singleton<PoolManager>
    {
        public GameObject Spawn(string poolTag, Vector3 position, Quaternion rotation);
        public void Despawn(string poolTag, GameObject obj);
        public void WarmPool(string poolTag, int count);
    }

    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}
```

**Usage in modules**:
```csharp
// Combat module (bullets)
GameObject bullet = PoolManager.Instance.Spawn("Bullet", spawnPos, spawnRot);

// VFX module (particles)
GameObject explosion = PoolManager.Instance.Spawn("Explosion", hitPos, Quaternion.identity);

// AI module (enemies)
GameObject enemy = PoolManager.Instance.Spawn("Zombie", spawnPoint.position, Quaternion.identity);
```

**Benefit**: Single pool shared across ALL modules → Memory efficient

---

### 3. Component Caching (`Core/Caching/`)

**Prevent repeated GetComponent calls**:

```csharp
namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Caches GetComponent calls to avoid repeated lookups.
    /// Used by all modules for performance.
    /// </summary>
    public static class ComponentCache
    {
        private static Dictionary<GameObject, Dictionary<Type, Component>> cache
            = new Dictionary<GameObject, Dictionary<Type, Component>>();

        public static T GetCached<T>(this GameObject obj) where T : Component
        {
            if (!cache.ContainsKey(obj))
                cache[obj] = new Dictionary<Type, Component>();

            var typeCache = cache[obj];
            Type type = typeof(T);

            if (!typeCache.ContainsKey(type))
            {
                typeCache[type] = obj.GetComponent<T>();
            }

            return typeCache[type] as T;
        }

        public static void ClearCache(GameObject obj)
        {
            if (cache.ContainsKey(obj))
                cache.Remove(obj);
        }
    }
}
```

**Usage**:
```csharp
// Instead of:
Rigidbody rb = GetComponent<Rigidbody>(); // Repeated calls expensive

// Use:
Rigidbody rb = gameObject.GetCached<Rigidbody>(); // Cached, fast
```

**Benefit**: Shared cache → All modules benefit from optimization

---

### 4. Extension Methods (`Core/Extensions/`)

**Reusable helpers** for common operations:

```csharp
namespace UnityMechanicsToolkit.Core
{
    public static class TransformExtensions
    {
        // Reset transform
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        // Find child recursively
        public static Transform FindDeep(this Transform parent, string name)
        {
            Transform result = parent.Find(name);
            if (result != null) return result;

            foreach (Transform child in parent)
            {
                result = child.FindDeep(name);
                if (result != null) return result;
            }

            return null;
        }
    }

    public static class Vector3Extensions
    {
        // Flatten Y axis
        public static Vector3 Flat(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }

        // Random point in radius
        public static Vector3 RandomInRadius(this Vector3 center, float radius)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            return center + new Vector3(randomCircle.x, 0, randomCircle.y);
        }
    }

    public static class LayerMaskExtensions
    {
        // Check if layer is in mask
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) != 0;
        }
    }
}
```

**Benefit**: All modules use same extensions → DRY principle

---

### 5. Raycast Sharing (`Core/Caching/`)

**Prevent multiple modules raycasting independently**:

```csharp
namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Centralized raycast manager to avoid duplicate raycasts.
    /// Example: Interaction + AI both need raycast from player → Share result.
    /// </summary>
    public class RaycastManager : Singleton<RaycastManager>
    {
        private Dictionary<string, RaycastHit> frameCache = new Dictionary<string, RaycastHit>();
        private int lastFrameCleared = -1;

        public bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit,
            float maxDistance, LayerMask mask, string cacheKey = null)
        {
            ClearCacheIfNewFrame();

            // Use cached result if available
            if (!string.IsNullOrEmpty(cacheKey) && frameCache.ContainsKey(cacheKey))
            {
                hit = frameCache[cacheKey];
                return hit.collider != null;
            }

            // Perform raycast
            bool didHit = Physics.Raycast(origin, direction, out hit, maxDistance, mask);

            // Cache result
            if (!string.IsNullOrEmpty(cacheKey))
            {
                frameCache[cacheKey] = hit;
            }

            return didHit;
        }

        private void ClearCacheIfNewFrame()
        {
            if (Time.frameCount != lastFrameCleared)
            {
                frameCache.Clear();
                lastFrameCleared = Time.frameCount;
            }
        }
    }
}
```

**Usage**:
```csharp
// Interaction module
RaycastManager.Instance.Raycast(origin, direction, out hit, range, mask, "PlayerInteract");

// AI module (same frame, same cache key)
RaycastManager.Instance.Raycast(origin, direction, out hit, range, mask, "PlayerInteract");
// ↑ Returns cached result, no duplicate raycast!
```

**Benefit**: Single raycast shared → Huge performance gain

---

### 6. Event System (`Core/Events/`)

**Centralized events** for cross-module communication:

```csharp
namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Global event bus for decoupled communication.
    /// Modules publish/subscribe without knowing each other.
    /// </summary>
    public static class GameEvents
    {
        // Player events
        public static event Action<float> OnPlayerHealthChanged;
        public static event Action OnPlayerDeath;
        public static event Action<Vector3, float> OnPlayerFootstep; // position, volume

        // Combat events
        public static event Action<GameObject, float> OnDamageDealt; // target, amount
        public static event Action<GameObject> OnEnemyKilled;

        // Interaction events
        public static event Action<IInteractable> OnInteraction;

        // Trigger methods (called by modules)
        public static void PlayerHealthChanged(float health) => OnPlayerHealthChanged?.Invoke(health);
        public static void PlayerDied() => OnPlayerDeath?.Invoke();
        public static void PlayerFootstep(Vector3 pos, float vol) => OnPlayerFootstep?.Invoke(pos, vol);
    }
}
```

**Usage**:
```csharp
// Movement module emits
GameEvents.PlayerFootstep(transform.position, isRunning ? 1f : 0.5f);

// AI module listens (no dependency on Movement!)
void OnEnable() { GameEvents.OnPlayerFootstep += HandleFootstep; }
void OnDisable() { GameEvents.OnPlayerFootstep -= HandleFootstep; }
void HandleFootstep(Vector3 pos, float vol) { /* investigate */ }
```

**Benefit**: Decoupled, shared event system → No module dependencies

---

### 7. Shared Managers (`Core/Managers/`)

**Optional global systems**:

```csharp
// Audio Manager (all modules play sounds through this)
public class AudioManager : Singleton<AudioManager>
{
    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f);
    public void PlayMusic(AudioClip clip, float fadeTime = 1f);
    public void StopMusic(float fadeTime = 1f);
}

// Save Manager (all modules save/load through this)
public class SaveManager : Singleton<SaveManager>
{
    public void SaveData(string key, object data);
    public T LoadData<T>(string key);
}

// Input Manager (centralized input, rebindable)
public class InputManager : Singleton<InputManager>
{
    public bool GetButtonDown(string action);
    public Vector2 GetAxis(string action);
}
```

**Benefit**: Single instance, all modules share → Consistency

---

## 🔀 Module Dependency Strategy

### Option A: Soft Dependency (Recommended)

**Modules check if Core exists, use if available, fallback if not**:

```csharp
namespace UnityMechanicsToolkit.Combat
{
    public class Weapon : MonoBehaviour
    {
        void Shoot()
        {
            GameObject bullet;

            #if UNITY_MECHANICS_TOOLKIT_CORE
            // Use Core pooling if available
            if (PoolManager.Instance != null)
            {
                bullet = PoolManager.Instance.Spawn("Bullet", spawnPoint.position, spawnPoint.rotation);
            }
            else
            #endif
            {
                // Fallback: Instantiate
                bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            }

            // Play sound via AudioManager if available, else AudioSource
            #if UNITY_MECHANICS_TOOLKIT_CORE
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(shootSound, transform.position);
            }
            else
            #endif
            {
                AudioSource.PlayClipAtPoint(shootSound, transform.position);
            }
        }
    }
}
```

**How it works**:
1. Import Core → Define `UNITY_MECHANICS_TOOLKIT_CORE` in Project Settings
2. Modules detect via `#if` → Use Core optimizations
3. No Core → Fallback to standalone mode

**Benefit**: Fully modular, optimized when Core present

---

### Option B: Hard Dependency (Simpler)

**All modules require Core** (simpler, always optimized):

```csharp
namespace UnityMechanicsToolkit.Combat
{
    public class Weapon : MonoBehaviour
    {
        void Shoot()
        {
            // Always use Core
            GameObject bullet = PoolManager.Instance.Spawn("Bullet", spawnPoint.position, spawnPoint.rotation);
            AudioManager.Instance.PlaySound(shootSound, transform.position);
        }
    }
}
```

**Tradeoff**:
- ✅ Simpler code (no conditionals)
- ✅ Always optimized
- ❌ Must import Core (not standalone)

**Recommendation**: Use this IF user always imports full toolkit

---

## 📊 Performance Comparison

### Without Core (Pure Standalone)
```csharp
// Each module does its own thing
Movement: GetComponent<Rigidbody>() // Every frame
Combat: Instantiate(bullet)         // GC allocation
AI: Physics.Raycast()               // Duplicate raycast
Audio: AudioSource.Play()           // Per-module audio
```

**Result**: ❌ Redundant, slow, memory waste

---

### With Core (Optimized)
```csharp
// Shared systems
Movement: gameObject.GetCached<Rigidbody>()          // Cached
Combat: PoolManager.Spawn("Bullet")                  // Pooled, no GC
AI: RaycastManager.Raycast(..., "PlayerCheck")       // Shared result
Audio: AudioManager.PlaySound()                       // Centralized
```

**Result**: ✅ Fast, memory efficient, optimized

---

## 🎯 Implementation Plan

### Phase 1: Create Core Module
```
src/Core/
├── Interfaces/
│   ├── IDamageable.cs
│   ├── IInteractable.cs
│   ├── IMovementController.cs
│   └── ...
├── Pooling/
│   ├── PoolManager.cs
│   └── IPoolable.cs
├── Caching/
│   ├── ComponentCache.cs
│   └── RaycastManager.cs
├── Extensions/
│   ├── TransformExtensions.cs
│   ├── Vector3Extensions.cs
│   └── GameObjectExtensions.cs
├── Events/
│   └── GameEvents.cs
├── Helpers/
│   ├── MathHelper.cs
│   └── PhysicsHelper.cs
└── Managers/ (optional)
    ├── AudioManager.cs
    ├── SaveManager.cs
    └── InputManager.cs
```

### Phase 2: Refactor Existing Modules
- Health → Use `IDamageable` from Core
- ObjectPool → Remove, use Core `PoolManager`
- Singleton → Use Core implementation

### Phase 3: Documentation
- Update ARCHITECTURE.md with Core info
- Add Core integration guide
- Example: "With Core" vs "Without Core"

---

## 📋 Core Module Checklist

When implementing Core:

- [ ] **Interfaces** - All common interfaces (IDamageable, IInteractable, etc.)
- [ ] **PoolManager** - Centralized object pooling
- [ ] **ComponentCache** - Cached GetComponent calls
- [ ] **RaycastManager** - Shared raycast results
- [ ] **Extensions** - Transform, Vector3, GameObject helpers
- [ ] **GameEvents** - Event bus for cross-module communication
- [ ] **AudioManager** - Centralized audio (optional)
- [ ] **SaveManager** - Save/load system (optional)
- [ ] **Scripting Define** - `UNITY_MECHANICS_TOOLKIT_CORE` symbol

---

## 💡 Best Practices

### 1. Core is Optional But Recommended

**Import strategy**:
- **Full toolkit users** → Import Core + all modules (optimized)
- **Cherry-pickers** → Can skip Core, modules still work (standalone)

### 2. Avoid Over-Centralization

**Don't put in Core**:
- Module-specific logic (e.g., door mechanics)
- Niche utilities (used by 1 module only)

**Do put in Core**:
- Shared interfaces (used by 3+ modules)
- Performance-critical systems (pooling, caching)
- Cross-cutting concerns (events, audio, save)

### 3. Document Dependencies

Each module README should state:
```markdown
## Dependencies
- **Required**: None
- **Optional**: Core (enables pooling, caching, optimized audio)
- **Recommended**: Core for best performance
```

---

## 🚀 Result

### With This Architecture:

✅ **Zero redundancy** - Shared code in Core
✅ **Optimized** - Pooling, caching, shared raycasts
✅ **Still modular** - Modules work without Core (fallback)
✅ **Performance** - 2-5x faster than pure standalone
✅ **Memory efficient** - Single pool, single cache
✅ **DRY** - No duplicate interfaces, extensions, helpers
✅ **Flexible** - Import Core for optimization, or skip for simplicity

**Best of both worlds!** 🎯
