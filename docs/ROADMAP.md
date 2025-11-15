# Unity Mechanics Toolkit - Complete Roadmap

**Focus**: Horror games mechanics + General reusable systems
**Inspiration**: Silent Hill, Resident Evil, Dead Space, Outlast, Amnesia, Phasmophobia, The Dark Pictures, Visage, Alan Wake, PT

---

## Priority Legend

- **P0** - Core essentials (must-have for any game)
- **P1** - Horror essentials (critical for horror games)
- **P2** - Advanced features (nice-to-have)
- **P3** - Polish & extras (for production-ready games)

---

## 📦 1. MOVEMENT SYSTEMS

### P0 - Core Movement
- [x] FirstPersonController (basic WASD + jump)
- [ ] ThirdPersonController (camera follow, character rotation)
- [ ] TopDownController (2D/3D top-down movement)
- [ ] Rigidbody-based movement (physics-driven)
- [ ] NavMesh movement (AI pathfinding compatible)

### P1 - Horror Movement
- [ ] **Crouch system** (stealth, hiding under objects)
- [ ] **Sprint with stamina drain** (exhaustion mechanic)
- [ ] **Walk/Run toggle** (noise control)
- [ ] **Lean left/right** (peek around corners)
- [ ] **Limp/Injury movement** (damaged state slow movement)
- [ ] **Backwards walking** (slower speed)
- [ ] **Ladder climbing** (vertical movement)
- [ ] **Crawling/Prone** (vents, tight spaces)

### P2 - Advanced Movement
- [ ] **Swimming/Underwater** (breath meter, slower movement)
- [ ] **Climbing system** (ledges, walls)
- [ ] **Vaulting** (jump over low obstacles)
- [ ] **Slide mechanic** (sprint + crouch)
- [ ] **Ragdoll on death** (physics-based death)
- [ ] **Push heavy objects** (puzzle mechanic)
- [ ] **Smooth step up/down** (stairs, curbs)

### P3 - Polish
- [ ] **Head bobbing** (walk/run animation)
- [ ] **Camera sway** (idle breathing, movement sway)
- [ ] **Footstep sounds** (surface detection)
- [ ] **Landing impact** (fall damage, camera shake)
- [ ] **Breath sounds** (sprint exhaustion)

---

## 🎯 2. INTERACTION SYSTEMS

### P0 - Core Interaction
- [x] InteractableObject (base class)
- [x] RaycastInteractor (center screen raycast)
- [x] DoorController (sliding/rotating)
- [ ] **Outline/Highlight system** (shader-based interactable highlight)
- [ ] **Interaction prompts UI** (dynamic "Press E" text)

### P1 - Horror Interaction
- [ ] **Locked doors** (key required, code pad)
- [ ] **Barricades** (block paths, breakable)
- [ ] **Lever/Switch** (toggle states, activate mechanisms)
- [ ] **Valve/Crank** (hold to rotate, progress bar)
- [ ] **Examine objects** (rotate, zoom, inspect)
- [ ] **Pick up items** (add to inventory)
- [ ] **Readable documents** (notes, diaries, newspapers)
- [ ] **Hiding spots** (closets, lockers, under beds)
- [ ] **Breakable windows** (escape routes)
- [ ] **Peephole** (look through doors)

### P2 - Advanced Interaction
- [ ] **Push/Pull objects** (move furniture, boxes)
- [ ] **Combine items** (inventory crafting)
- [ ] **Use item on object** (key on door, fuel on generator)
- [ ] **Two-hand interactions** (require both hands free)
- [ ] **Timed interactions** (quick-time events)
- [ ] **Multi-step interactions** (sequences, puzzles)
- [ ] **Destructible objects** (break to find items)

---

## 🎒 3. INVENTORY SYSTEMS

### P0 - Core Inventory
- [x] Item (ScriptableObject base)
- [x] InventorySystem (slot-based, stacking)
- [ ] **Inventory UI** (grid display, drag & drop)
- [ ] **Quick slots/Hotbar** (1-4 keys for quick access)

### P1 - Horror Inventory
- [ ] **Limited inventory space** (Resident Evil style)
- [ ] **Item categories** (weapons, ammo, health, keys, documents)
- [ ] **Examine mode** (3D rotate, read descriptions)
- [ ] **Ammo system** (separate ammo types, finite resources)
- [ ] **Key items** (cannot be dropped, quest critical)
- [ ] **Consumables** (health kits, batteries, stamina items)
- [ ] **Weapons in inventory** (equip/unequip)
- [ ] **Backpack/Storage upgrade** (expand capacity)

### P2 - Advanced Inventory
- [ ] **Weight system** (encumbrance, movement penalty)
- [ ] **Item durability** (weapons break, flashlight degrades)
- [ ] **Combine items** (craft healing items, ammo)
- [ ] **Separate storage boxes** (safe rooms, Resident Evil style)
- [ ] **Item rarity** (common, uncommon, rare)
- [ ] **Tooltips** (hover info, stats)

---

## ⚔️ 4. COMBAT SYSTEMS

### P0 - Core Combat
- [x] Health system (damage, heal, death)
- [ ] **Damage system** (apply damage with types)
- [ ] **Hit detection** (raycast, collision-based)

### P1 - Horror Combat
- [ ] **Melee weapons** (knife, bat, axe)
  - [ ] Swing animations
  - [ ] Durability system
  - [ ] Damage zones (head, body, limbs)
- [ ] **Ranged weapons** (pistol, shotgun, rifle)
  - [ ] Ammo system
  - [ ] Recoil & spread
  - [ ] Reload mechanic
  - [ ] Aim down sights (ADS)
- [ ] **Stamina system** (drains on sprint, attack, dodge)
- [ ] **Block/Parry** (defensive mechanic)
- [ ] **Dodge/Roll** (i-frames, stamina cost)
- [ ] **Stagger/Stun** (enemy hit reaction)
- [ ] **Critical hits** (headshots, weak points)
- [ ] **Death states** (player death, respawn/reload)

### P2 - Advanced Combat
- [ ] **Weapon sway** (idle, movement, aiming)
- [ ] **Flashlight as weapon** (stun enemies, Dead Space style)
- [ ] **Throwable items** (grenades, bottles for distraction)
- [ ] **Dismemberment** (Dead Space limb cutting)
- [ ] **Execution animations** (close-range finishers)
- [ ] **Stealth kills** (backstab, silent takedown)
- [ ] **Enemy armor** (break armor, expose weak points)
- [ ] **Blood/Gore system** (decals, particle effects)

### P3 - Polish
- [ ] **Hit markers** (visual/audio feedback)
- [ ] **Damage numbers** (floating combat text)
- [ ] **Bullet shells** (eject, physics-based)
- [ ] **Muzzle flash** (weapon fire effects)
- [ ] **Impact effects** (wall hits, blood splatter)

---

## 👻 5. HORROR-SPECIFIC SYSTEMS

### P1 - Core Horror Mechanics
- [ ] **Sanity/Fear system** (Amnesia-style)
  - [ ] Sanity meter (decreases in dark, near enemies)
  - [ ] Visual effects (screen distortion, hallucinations)
  - [ ] Audio effects (whispers, heartbeat)
  - [ ] Gameplay penalties (reduced movement, inverted controls)
- [ ] **Darkness system** (vision limitation)
  - [ ] Fog/darkness zones
  - [ ] Vision range reduction
  - [ ] Brightness adjustment
- [ ] **Flashlight system** (battery drain, flicker)
  - [ ] Toggle on/off
  - [ ] Battery drain over time
  - [ ] Flicker when low battery
  - [ ] Intensity adjustment
  - [ ] Cone angle, range
- [ ] **Jump scare system** (triggered events)
  - [ ] Random vs scripted
  - [ ] Camera shake
  - [ ] Audio sting
  - [ ] Disable controls briefly
- [ ] **Chase sequences** (scripted pursuits)
  - [ ] Auto-run events
  - [ ] Path waypoints
  - [ ] Obstacle spawning

### P2 - Advanced Horror
- [ ] **Breathing/Heartbeat audio** (dynamic intensity)
- [ ] **Vignette effect** (danger, low health)
- [ ] **Screen shake** (impacts, explosions, footsteps)
- [ ] **Chromatic aberration** (fear, damage)
- [ ] **Film grain/Noise** (VHS horror aesthetic)
- [ ] **Hallucinations** (fake enemies, visual glitches)
- [ ] **Radio static** (Silent Hill proximity detection)
- [ ] **Possession mechanic** (lose control temporarily)
- [ ] **Oxygen/Breath meter** (underwater, gas mask)
- [ ] **Temperature system** (freezing, hypothermia)

### P3 - Polish
- [ ] **Dynamic music system** (calm, tension, chase, combat)
- [ ] **Environmental storytelling** (blood trails, corpses, notes)
- [ ] **Scripted events** (door slams, lights flicker)
- [ ] **Mannequin system** (move when not looking, Weeping Angels)
- [ ] **Mirror scares** (reflections, distortions)

---

## 🤖 6. AI SYSTEMS

### P0 - Core AI
- [ ] **State Machine** (Idle, Patrol, Chase, Attack, Search)
- [ ] **Pathfinding** (NavMesh integration)
- [ ] **Target detection** (player in range)

### P1 - Horror AI
- [ ] **Patrol behavior** (waypoints, random walk)
- [ ] **Chase behavior** (follow player, last known position)
- [ ] **Search behavior** (lost player, investigate area)
- [ ] **Vision cone** (field of view detection)
  - [ ] Angle & range
  - [ ] Line of sight checks
  - [ ] Occlusion detection
- [ ] **Hearing system** (detect noise from player)
  - [ ] Footsteps (walk/run volume)
  - [ ] Gunshots, item drops
  - [ ] Noise radius
- [ ] **Attack patterns** (melee lunge, ranged shoot)
- [ ] **Retreat behavior** (low health, flee)
- [ ] **Alert system** (call other enemies)

### P2 - Advanced AI
- [ ] **Group behavior** (pack hunting, flanking)
- [ ] **Dynamic difficulty** (adapt to player skill)
- [ ] **Teleportation** (Slenderman-style repositioning)
- [ ] **Stalking behavior** (follow at distance, Alien Isolation)
- [ ] **Vent/Crawl space AI** (use alternate paths)
- [ ] **Door interactions** (open, break down doors)
- [ ] **Investigative AI** (check hiding spots)
- [ ] **Randomized spawn points** (unpredictability)

### P3 - Polish
- [ ] **Animation blending** (smooth transitions)
- [ ] **Footstep sounds** (AI movement audio)
- [ ] **Breathing/Growl sounds** (proximity audio)
- [ ] **Taunt system** (voice lines, psychological horror)

---

## 🔊 7. AUDIO SYSTEMS

### P0 - Core Audio
- [ ] **Audio Manager** (singleton, pools)
- [ ] **2D/3D sound** (spatial audio)
- [ ] **Music system** (background tracks)

### P1 - Horror Audio
- [ ] **Ambient soundscapes** (wind, creaks, distant noises)
- [ ] **Footstep system** (material detection: wood, metal, concrete)
- [ ] **Heartbeat audio** (dynamic intensity based on fear/danger)
- [ ] **Breathing sounds** (sprint exhaustion, fear)
- [ ] **Door sounds** (open, close, creak, lock)
- [ ] **Weapon sounds** (shoot, reload, draw, holster)
- [ ] **Enemy sounds** (growls, footsteps, attacks)
- [ ] **Dynamic music** (layers, transitions)
- [ ] **Silence system** (sudden quiet for tension)
- [ ] **Directional audio cues** (help player locate threats)

### P2 - Advanced Audio
- [ ] **Audio occlusion** (muffled through walls)
- [ ] **Reverb zones** (room acoustics)
- [ ] **Low-pass filter** (underwater, muffled hearing)
- [ ] **Audio snapshots** (mix states: normal, combat, stealth)
- [ ] **Radio/Walkie-talkie** (distorted voice comms)
- [ ] **Phonograph/Music box** (diegetic horror music)

---

## 📹 8. CAMERA SYSTEMS

### P0 - Core Camera
- [ ] **First-person camera** (mouse look)
- [ ] **Third-person camera** (orbit, follow)

### P1 - Horror Camera
- [ ] **Camera shake** (impacts, explosions, monster footsteps)
  - [ ] Intensity, duration, frequency
  - [ ] Positional & rotational shake
- [ ] **Look behind** (press key to 180° turn, Resident Evil)
- [ ] **Head bobbing** (walk/run rhythm)
- [ ] **Breathing sway** (idle camera movement)
- [ ] **Zoom/Aim** (ADS, binoculars)
- [ ] **Camera constraints** (limit look angles)
- [ ] **Fixed camera angles** (classic Resident Evil style)

### P2 - Advanced Camera
- [ ] **Cinemachine integration** (smooth transitions, cinematics)
- [ ] **Dynamic FOV** (sprint increases FOV)
- [ ] **Drunk/Dizzy effect** (spin, blur)
- [ ] **Security camera view** (CCTV, grainy, static)
- [ ] **VHS camera filter** (found footage horror)

---

## 💡 9. LIGHTING SYSTEMS

### P1 - Horror Lighting
- [ ] **Flashlight** (cone light, battery drain, flicker)
- [ ] **Lantern/Candle** (radius light, wind effects)
- [ ] **Lighter/Match** (temporary light, duration)
- [ ] **Dynamic shadows** (real-time shadow casting)
- [ ] **Flicker lights** (broken lights, intermittent)
- [ ] **Light switch** (toggle room lights)
- [ ] **Emergency lights** (red, flashing)
- [ ] **Glow sticks** (throwable light source)

### P2 - Advanced Lighting
- [ ] **Global Illumination** (baked lighting)
- [ ] **Light shafts** (god rays, volumetric)
- [ ] **Bioluminescence** (glowing organisms, paths)
- [ ] **Fire/Torch** (dynamic light, warmth)
- [ ] **Lightning flashes** (outdoor storms)

---

## 🌫️ 10. VISUAL EFFECTS (POST-PROCESSING)

### P1 - Horror VFX
- [ ] **Vignette** (danger indicator, low health)
- [ ] **Chromatic aberration** (fear, damage)
- [ ] **Motion blur** (fast movement, disorientation)
- [ ] **Depth of field** (focus on objects, blur background)
- [ ] **Film grain** (VHS, found footage aesthetic)
- [ ] **Color grading** (desaturate, cold/warm tones)
- [ ] **Screen blood** (damage indicator)
- [ ] **Screen cracks** (severe damage)
- [ ] **Blur/Distortion** (sanity loss, poison)

### P2 - Advanced VFX
- [ ] **Bloom** (bright lights, supernatural glow)
- [ ] **Lens distortion** (barrel, fisheye)
- [ ] **Screen space reflections** (wet surfaces, mirrors)
- [ ] **Fog/Mist** (volumetric fog)
- [ ] **Rain/Snow particles** (weather effects)
- [ ] **Dust motes** (ambient particles)
- [ ] **Screen glitch** (digital horror, possession)

---

## 🧩 11. PUZZLE SYSTEMS

### P1 - Core Puzzles
- [ ] **Keypad/Code entry** (numeric, alphanumeric)
- [ ] **Lock picking** (mini-game)
- [ ] **Valve puzzle** (rotate to unlock)
- [ ] **Lever sequence** (specific order)
- [ ] **Symbol matching** (pattern recognition)
- [ ] **Sliding tile puzzle** (image reconstruction)
- [ ] **Fuse box** (restore power)
- [ ] **Circuit puzzle** (connect paths)
- [ ] **Pressure plate** (weight-based triggers)
- [ ] **Light beam redirection** (mirrors, lasers)

### P2 - Advanced Puzzles
- [ ] **Inventory combination** (mix items to progress)
- [ ] **Riddle system** (text-based clues)
- [ ] **Timed puzzles** (countdown pressure)
- [ ] **Multi-room puzzles** (coordinated actions)
- [ ] **Sound-based puzzles** (musical sequence, pitch)
- [ ] **Memory puzzles** (recall sequences)

---

## 📖 12. NARRATIVE SYSTEMS

### P1 - Core Narrative
- [ ] **Dialogue system** (NPC conversations)
  - [ ] Choice trees
  - [ ] Branching paths
  - [ ] Typed text effect
- [ ] **Readable documents** (notes, diaries, emails)
  - [ ] UI display
  - [ ] Collectible tracking
  - [ ] Lore database
- [ ] **Subtitles** (dialogue, audio logs)
- [ ] **Objective system** (quest tracking, waypoints)
- [ ] **Journal/Log** (track story progress)

### P2 - Advanced Narrative
- [ ] **Cutscenes** (timeline-based, skippable)
- [ ] **Environmental storytelling** (visual clues)
- [ ] **Audio logs** (voice recordings, cassette tapes)
- [ ] **Flashbacks** (triggered memories)
- [ ] **Multiple endings** (choice-based outcomes)
- [ ] **Character relationships** (affinity system)

---

## 💾 13. SAVE/LOAD SYSTEMS

### P1 - Core Save
- [ ] **Save game system** (JSON/Binary serialization)
- [ ] **Auto-save** (checkpoints, area transitions)
- [ ] **Manual save** (save points, typewriters RE-style)
- [ ] **Load game** (scene restoration, inventory state)
- [ ] **Multiple save slots** (separate playthroughs)

### P2 - Advanced Save
- [ ] **Cloud saves** (Steam, Epic integration)
- [ ] **Save encryption** (prevent tampering)
- [ ] **Permadeath mode** (hardcore, delete save on death)
- [ ] **Save statistics** (playtime, deaths, completion %)

---

## 🎨 14. UI SYSTEMS

### P0 - Core UI
- [ ] **Main menu** (start, load, settings, quit)
- [ ] **Pause menu** (resume, save, load, exit)
- [ ] **Settings menu** (graphics, audio, controls)

### P1 - Horror UI
- [ ] **Health bar/indicator** (visual, minimal HUD)
- [ ] **Stamina bar** (sprint, dodge, attack)
- [ ] **Sanity meter** (mental state indicator)
- [ ] **Ammo counter** (bullet count, magazine display)
- [ ] **Inventory UI** (grid, drag & drop)
- [ ] **Interaction prompts** (context-sensitive)
- [ ] **Objective tracker** (current goals)
- [ ] **Damage indicators** (directional hit markers)
- [ ] **Reticle/Crosshair** (aiming, customizable)

### P2 - Advanced UI
- [ ] **Map system** (mini-map, full screen map)
- [ ] **Compass** (directional navigation)
- [ ] **Item pickup notifications** (toast messages)
- [ ] **Tutorial tooltips** (first-time hints)
- [ ] **Death screen** (retry, load, quit)
- [ ] **Credits** (end game credits roll)

---

## 🎮 15. GAMEPLAY MECHANICS

### P1 - Core Mechanics
- [ ] **Quick Time Events (QTE)** (button mashing, timing)
- [ ] **Checkpoint system** (respawn points)
- [ ] **Achievements/Trophies** (unlock tracking)
- [ ] **Difficulty settings** (easy, normal, hard, nightmare)
- [ ] **New Game+** (carry over items, harder enemies)

### P2 - Advanced Mechanics
- [ ] **Stealth system** (visibility, noise detection)
  - [ ] Crouch reduces visibility
  - [ ] Walk reduces noise
  - [ ] Hide in shadows
- [ ] **Day/Night cycle** (time-based events)
- [ ] **Weather system** (rain, fog, thunder)
- [ ] **Physics interactions** (ragdoll, breakable objects)
- [ ] **Destructible environment** (break walls, furniture)
- [ ] **Traps** (bear traps, tripwires, explosive)

### P3 - Polish
- [ ] **Photo mode** (free camera, filters)
- [ ] **Easter eggs** (hidden secrets, references)
- [ ] **Alternate costumes** (unlockable skins)
- [ ] **Cheat codes** (God mode, infinite ammo)

---

## 🛠️ 16. DEVELOPER TOOLS

### P2 - Debug Tools
- [ ] **Debug console** (command input, cheats)
- [ ] **FPS counter** (performance monitoring)
- [ ] **Scene reload** (quick iteration)
- [ ] **God mode** (invincibility, infinite resources)
- [ ] **Teleport system** (warp to locations)
- [ ] **Spawn enemies** (test combat)
- [ ] **Toggle AI** (disable enemies)
- [ ] **Gizmos** (visualize triggers, colliders, AI vision)

---

## 📊 IMPLEMENTATION PRIORITY

### Sprint 1 (P0 + P1 Core)
1. Movement: Crouch, Sprint+Stamina, Lean
2. Interaction: Highlight, Locked doors, Hiding spots
3. Inventory: UI, Quick slots, Examine mode
4. Combat: Melee weapon, Stamina, Block
5. Horror: Flashlight, Sanity system, Darkness
6. AI: State machine, Vision cone, Patrol/Chase
7. Audio: Footsteps, Heartbeat, Ambient
8. Camera: Shake, Look behind, Head bob

### Sprint 2 (P1 Advanced)
9. Combat: Ranged weapons, Ammo, Reload
10. Horror: Jump scares, Chase sequences
11. AI: Hearing, Search behavior, Attack patterns
12. Puzzles: Keypad, Lever, Valve
13. Narrative: Documents, Dialogue, Objectives
14. UI: Health, Stamina, Inventory grid
15. Save/Load: Manual save, Auto-save

### Sprint 3 (P2 Polish)
16. Movement: Climb, Vault, Swim
17. Combat: Throwables, Stealth kills
18. Horror: Hallucinations, Radio static
19. Lighting: Flicker, Light switches
20. VFX: Post-processing effects
21. Advanced AI: Group behavior, Stalking
22. Advanced puzzles, narrative branches

### Sprint 4 (P3 Production)
23. Full polish pass (all P3 items)
24. Example scenes for each module
25. Documentation & tutorials
26. Unit tests
27. Performance optimization

---

**TOTAL FEATURES**: ~200+ mechanics to implement
**Estimated Development**: 3-6 months (full-time)
**Current Progress**: 7/200 (3.5%) ✅

**Focus**: Horror essentials first, then expand to general mechanics.
