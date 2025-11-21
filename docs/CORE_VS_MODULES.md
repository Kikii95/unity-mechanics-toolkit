# Core vs Modules - Decision Guide

**Question**: Comment décider si quelque chose va dans **Core** ou dans un **Module** ?

---

## 🎯 La Règle d'Or

### ✅ Mettre dans **CORE** si :

1. **Utilisé par 3+ modules différents**
   - Exemple : `IDamageable` (utilisé par Combat, AI, Interaction, Horror)
   - Exemple : `Vector3.Flat()` (utilisé par Movement, AI, Camera)

2. **Abstraction technique** (pas de logique métier)
   - Interfaces (contrats, pas implémentation)
   - Extensions (helpers, utilitaires)
   - Events (communication)
   - Helpers (math, physics, debug)

3. **Performance-critical partagé**
   - Object pooling (bullets, particles, enemies)
   - Component caching (GetComponent optimization)
   - Raycast sharing (multiple modules need same raycast)

4. **Cross-cutting concern** (traverse tous les modules)
   - Audio system (tous jouent des sons)
   - Save system (tous sauvegardent des données)
   - Input management (tous lisent les inputs)
   - Event bus (tous communiquent)

5. **Aucune logique de gameplay**
   - Juste des outils, pas de features
   - Pas de comportement visible par le joueur

---

### ✅ Mettre dans **MODULE** si :

1. **Logique de gameplay spécifique**
   - Comportements jouables (movement, combat, doors)
   - Systèmes visibles par le joueur

2. **Implémentation concrète** (pas abstraction)
   - Classes avec logique complète
   - MonoBehaviours avec Update/FixedUpdate
   - ScriptableObjects avec données de jeu

3. **Peut être utilisé seul** (standalone)
   - Fonctionne indépendamment
   - Pas juste un helper pour autre chose

4. **Feature complète**
   - Système complet (Health, Weapon, Door, AI)
   - Pas juste un bout de code réutilisable

---

## 📊 Exemples Concrets

### ❌ ERREUR COMMUNE : "Movement est Core car essentiel"

**NON !** Movement est un **MODULE** :
- ✅ Logique de gameplay (déplacement du joueur)
- ✅ Implémentation concrète (FirstPersonController)
- ✅ Peut être utilisé seul (standalone)
- ✅ Pas utilisé par tous les modules (AI ne dépend pas de FirstPersonController)

**Mais DANS Core** (abstractions de Movement) :
- ✅ `IMovementController` (interface) → Abstraction
- ✅ `Vector3Extensions.Flat()` → Utilitaire réutilisable
- ✅ `GameEvents.OnPlayerFootstep` → Event cross-module

---

### ✅ Exemple 1 : Combat System

#### Module : Combat/
```csharp
// ❌ PAS dans Core (logique gameplay)
Combat/
├── Health.cs              // Système santé (logique métier)
├── Weapon.cs              // Arme (gameplay)
├── DamageDealer.cs        // Applique dégâts (gameplay)
└── WeaponData.cs          // Config arme (ScriptableObject)
```

#### Core (abstractions utilisées par Combat)
```csharp
// ✅ DANS Core (abstractions techniques)
Core/
├── Interfaces/
│   ├── IDamageable.cs     // Interface (utilisée par Combat, AI, Interaction)
│   └── IKillable.cs       // Interface (utilisée par Combat, AI, Horror)
├── Pooling/
│   └── PoolManager.cs     // Pool bullets (utilisé par Combat, VFX, AI)
└── Events/
    └── GameEvents.cs      // OnDamageDealt (utilisé par Combat, UI, Audio, AI)
```

**Pourquoi ?**
- Health = Logique complète de santé → **Module**
- IDamageable = Abstraction utilisée par 5+ modules → **Core**
- PoolManager = Performance utility pour bullets, VFX, AI → **Core**

---

### ✅ Exemple 2 : Movement System

#### Module : Movement/
```csharp
// ❌ PAS dans Core (implémentations concrètes)
Movement/
├── FirstPersonController.cs   // FPS movement (gameplay)
├── ThirdPersonController.cs   // TPS movement (gameplay)
├── CrouchBehavior.cs          // Crouch mechanic (gameplay)
├── StaminaSystem.cs           // Stamina drain (gameplay)
└── MovementSettings.cs        // Config (ScriptableObject)
```

#### Core (utilisé par Movement)
```csharp
// ✅ DANS Core (helpers réutilisables)
Core/
├── Interfaces/
│   └── IMovementController.cs // Interface (utilisée par Movement, AI, Input)
├── Extensions/
│   ├── Vector3Extensions.cs   // Flat(), DirectionTo() (utilisé par Movement, AI, Camera)
│   └── TransformExtensions.cs // Helpers (utilisé par Movement, AI, Interaction)
└── Events/
    └── GameEvents.cs          // OnPlayerFootstep (utilisé par Movement, AI, Audio)
```

**Pourquoi ?**
- FirstPersonController = Logique FPS complète → **Module**
- Vector3.Flat() = Helper réutilisable par Movement, AI, Camera → **Core**
- OnPlayerFootstep = Event écouté par AI, Audio → **Core**

---

### ✅ Exemple 3 : Interaction System

#### Module : Interaction/
```csharp
// ❌ PAS dans Core (features gameplay)
Interaction/
├── InteractableObject.cs      // Base class interactables (gameplay)
├── RaycastInteractor.cs       // Raycast interaction (gameplay)
├── DoorController.cs          // Door mechanics (gameplay)
├── LeverController.cs         // Lever mechanics (gameplay)
└── HidingSpot.cs              // Hiding mechanic (gameplay)
```

#### Core (utilisé par Interaction)
```csharp
// ✅ DANS Core (abstractions)
Core/
├── Interfaces/
│   └── IInteractable.cs       // Interface (utilisée par Interaction, AI, UI)
├── Caching/
│   └── RaycastManager.cs      // Shared raycasts (utilisé par Interaction, AI, Combat)
└── Events/
    └── GameEvents.cs          // OnInteraction (utilisé par Interaction, UI, Audio, Objectives)
```

**Pourquoi ?**
- DoorController = Logique porte complète → **Module**
- IInteractable = Abstraction utilisée par UI, AI → **Core**
- RaycastManager = Partagé par Interaction, AI, Combat → **Core**

---

### ✅ Exemple 4 : AI System

#### Module : AI/
```csharp
// ❌ PAS dans Core (features gameplay)
AI/
├── AIStateMachine.cs          // State machine (gameplay)
├── PatrolBehavior.cs          // Patrol logic (gameplay)
├── ChaseBehavior.cs           // Chase logic (gameplay)
├── VisionCone.cs              // Vision detection (gameplay)
├── HearingSystem.cs           // Hearing detection (gameplay)
└── AISettings.cs              // Config (ScriptableObject)
```

#### Core (utilisé par AI)
```csharp
// ✅ DANS Core (partagé)
Core/
├── Interfaces/
│   ├── IDamageable.cs         // AI attack health targets
│   └── IAITarget.cs           // Interface (utilisée par AI, Player, Objectives)
├── Extensions/
│   └── Vector3Extensions.cs   // DirectionTo(), DistanceTo() (AI pathfinding)
├── Caching/
│   ├── ComponentCache.cs      // GetComponent optimization
│   └── RaycastManager.cs      // Shared vision raycasts
└── Events/
    └── GameEvents.cs          // OnAIDetectedPlayer (écouté par UI, Audio, Horror)
```

**Pourquoi ?**
- VisionCone = Logique détection AI → **Module**
- IAITarget = Interface utilisée par Player, Objectives → **Core**
- RaycastManager = Partagé par AI, Interaction, Combat → **Core**

---

## 🎓 Checklist Rapide

Posez-vous ces questions :

### Pour CORE :
- [ ] Est-ce une **abstraction** (interface, helper) ?
- [ ] Est-ce utilisé par **3+ modules** ?
- [ ] Est-ce **performance-critical** partagé (pooling, caching) ?
- [ ] Est-ce un **cross-cutting concern** (audio, save, events) ?
- [ ] Contient-il **ZÉRO logique de gameplay** ?

**Si OUI à 2+ questions** → **Core**

---

### Pour MODULE :
- [ ] Est-ce une **feature de gameplay** visible par le joueur ?
- [ ] Est-ce une **implémentation concrète** (pas abstraction) ?
- [ ] Peut-il être **utilisé seul** (standalone) ?
- [ ] Contient-il de la **logique métier** complète ?

**Si OUI à 2+ questions** → **Module**

---

## 📋 Liste Complète Core vs Modules

### 🔧 CORE (Fondations techniques)

#### Interfaces/
```
✅ IDamageable           (Combat, AI, Interaction utilisent)
✅ IKillable             (Combat, AI, Horror utilisent)
✅ IInteractable         (Interaction, UI, AI utilisent)
✅ IMovementController   (Movement, AI, Input utilisent)
✅ IPoolable             (Combat, VFX, AI utilisent)
✅ IDetectable           (Stealth, AI, Horror utilisent)
✅ IAITarget             (AI, Player, Objectives utilisent)
```

#### Extensions/
```
✅ TransformExtensions   (Reset, FindDeep, DestroyChildren)
✅ Vector3Extensions     (Flat, DirectionTo, RandomInRadius)
✅ GameObjectExtensions  (GetOrAdd, HasComponent, SetLayerRecursively)
✅ LayerMaskExtensions   (Contains)
```

#### Pooling/
```
✅ PoolManager           (Bullets, Particles, Enemies, Audio)
✅ IPoolable             (Interface pooling)
```

#### Caching/
```
✅ ComponentCache        (GetComponent optimization TOUS modules)
✅ RaycastManager        (Shared raycasts Interaction + AI + Combat)
```

#### Events/
```
✅ GameEvents            (Cross-module communication)
   - OnPlayerHealthChanged (écouté par UI, Audio, Horror)
   - OnPlayerFootstep      (écouté par AI, Audio)
   - OnDamageDealt         (écouté par UI, Audio, VFX)
   - OnInteraction         (écouté par UI, Audio, Objectives)
   etc.
```

#### Helpers/
```
✅ MathHelper            (Clamp, Remap, Angle calculations)
✅ PhysicsHelper         (Overlap checks, force calculations)
✅ DebugHelper           (DrawRay, DrawCircle, Log formatting)
```

#### Managers/ (Optionnel)
```
✅ AudioManager          (TOUS modules jouent sons via ça)
✅ SaveManager           (TOUS modules save/load via ça)
✅ InputManager          (TOUS modules lisent inputs via ça)
✅ SceneManager          (TOUS modules chargent scenes via ça)
```

---

### 📦 MODULES (Features gameplay)

#### Movement/
```
❌ FirstPersonController     (FPS movement logic)
❌ ThirdPersonController     (TPS movement logic)
❌ TopDownController         (2D movement logic)
❌ CrouchBehavior            (Crouch mechanic)
❌ SprintBehavior            (Sprint + stamina)
❌ LadderClimbing            (Climb mechanic)
❌ StaminaSystem             (Stamina management)
```

#### Combat/
```
❌ Health                    (Health system)
❌ Weapon                    (Weapon base class)
❌ MeleeWeapon               (Melee combat)
❌ RangedWeapon              (Ranged combat)
❌ DamageDealer              (Damage calculation)
❌ Projectile                (Bullet physics)
```

#### Interaction/
```
❌ InteractableObject        (Base interactable)
❌ RaycastInteractor         (Raycast interaction)
❌ DoorController            (Door mechanics)
❌ LeverController           (Lever mechanics)
❌ HidingSpot                (Hiding mechanic)
❌ PickupItem                (Item pickup)
```

#### Horror/
```
❌ SanitySystem              (Sanity mechanic)
❌ FlashlightSystem          (Flashlight + battery)
❌ DarknessZone              (Dark areas)
❌ JumpScare                 (Jump scare events)
❌ ChaseSequence             (Chase mechanic)
```

#### AI/
```
❌ AIStateMachine            (AI states)
❌ PatrolBehavior            (Patrol logic)
❌ ChaseBehavior             (Chase logic)
❌ VisionCone                (Vision detection)
❌ HearingSystem             (Audio detection)
```

#### Inventory/
```
❌ InventorySystem           (Inventory management)
❌ Item                      (Item base class)
❌ Equipment                 (Equipment system)
❌ CraftingSystem            (Crafting mechanic)
```

#### Camera/
```
❌ CameraShake               (Shake effect)
❌ CameraFollow              (Follow player)
❌ LookBehind                (RE-style look back)
❌ HeadBobbing               (Head bob effect)
```

#### Audio/
```
❌ FootstepSystem            (Material-based footsteps)
❌ AmbienceSystem            (Ambient sounds)
❌ MusicController           (Dynamic music)
```

#### UI/
```
❌ HealthBar                 (Health display)
❌ InventoryUI               (Inventory display)
❌ InteractionPrompt         (Prompt UI)
❌ DamageIndicator           (Damage direction)
```

---

## 💡 Cas Limites (Edge Cases)

### Singleton<T>

**Question** : Core ou Module ?

**Réponse** : **Core** ✅
- Utilisé par TOUS les managers (AudioManager, SaveManager, etc.)
- Abstraction technique (pattern réutilisable)
- Pas de logique gameplay

---

### ObjectPool (ancien)

**Question** : Core ou Module ?

**Réponse** : **Core** (devient PoolManager) ✅
- Utilisé par Combat (bullets), VFX (particles), AI (enemies), Audio (sounds)
- Performance-critical
- Pas de logique gameplay

---

### StaminaSystem

**Question** : Core ou Module ?

**Réponse** : **Module** (Movement/) ❌
- Logique gameplay (stamina drain pendant sprint)
- Spécifique au player movement
- Pas utilisé par autres modules (juste Movement)

**Mais** : Si 3+ modules utilisent stamina (Movement + Combat + Interaction), créer :
- Core: `IStamina` (interface)
- Module: `StaminaSystem` (implémentation)

---

### AudioManager

**Question** : Core ou Module ?

**Réponse** : **Core** (optionnel) ✅
- Cross-cutting concern (TOUS modules jouent sons)
- Singleton global
- Pas de logique gameplay (juste service technique)

---

## 🎯 Résumé Final

### Core = Boîte à outils technique
```
- Interfaces (contrats)
- Extensions (helpers)
- Events (communication)
- Pooling (performance)
- Caching (performance)
- Managers (services globaux)
```

**Caractéristiques** :
- ✅ Technique, pas gameplay
- ✅ Partagé par 3+ modules
- ✅ Abstraction, pas implémentation

---

### Modules = Features de jeu
```
- Movement (déplacement)
- Combat (santé, armes)
- Interaction (portes, items)
- Horror (sanity, flashlight)
- AI (comportements)
- etc.
```

**Caractéristiques** :
- ✅ Logique gameplay complète
- ✅ Visible par le joueur
- ✅ Standalone (peut être utilisé seul)

---

## 📝 Règle Simple (TL;DR)

**Si tu hésites** :

1. **C'est du code de gameplay visible ?** → Module
2. **C'est un outil technique réutilisable ?** → Core

**Exemple** :
- FirstPersonController (gameplay) → **Module**
- Vector3.Flat() (outil) → **Core**

**Easy !** 🚀
