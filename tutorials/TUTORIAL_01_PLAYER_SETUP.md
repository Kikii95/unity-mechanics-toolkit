# Tutorial 01 - Complete Player Setup

**Goal**: Create a fully functional horror game player with all Sprint 1 features.

**Time**: 10 minutes

---

## 🎯 What You'll Build

A complete player with:
- FPS movement (WASD + Mouse)
- Crouch (C key)
- Sprint with stamina (Shift)
- Lean (A/E keys - AZERTY)
- Flashlight with battery (F key)
- Sanity system
- Camera effects (shake, head bob, look behind)

---

## 📋 Prerequisites

- Unity 2021.3 or higher
- Unity Mechanics Toolkit imported
- Empty scene with ground plane

---

## Step 1: Create Player GameObject

1. **Create empty GameObject**
   - Hierarchy → Right-click → Create Empty
   - Name: `Player`
   - Position: `(0, 1, 0)` (1 unit above ground)

2. **Add CharacterController**
   - Select Player
   - Add Component → Character Controller
   - Settings:
     - Height: `2`
     - Radius: `0.3`
     - Center: `(0, 1, 0)`

3. **Create Camera**
   - Right-click Player → 3D Object → Camera
   - Name: `PlayerCamera`
   - Position: `(0, 0.8, 0)` (eye level)
   - Tag: `MainCamera`

---

## Step 2: Add Core Movement

1. **FirstPersonController**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Movement → First Person Controller
   - Configure:
     - Player Camera: Drag `PlayerCamera`
     - Move Speed: `5`
     - Sprint Multiplier: `1.5`
     - Jump Force: `8`
     - Mouse Sensitivity: `2`

2. **Test movement**
   - Press Play
   - Use WASD to move (ZQSD on AZERTY)
   - Use Mouse to look around
   - Space to jump

---

## Step 3: Add Stamina System

1. **StaminaSystem Component**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Core → Stamina System
   - Configure:
     - Max Stamina: `100`
     - Regen Rate: `10` (per second)
     - Regen Delay: `2` (seconds)

2. **Test stamina**
   - Press Play
   - Stamina regenerates automatically
   - Will be used by sprint/dodge/attacks

---

## Step 4: Add Advanced Movement

### 4.1 Crouch

1. **CrouchBehavior Component**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Movement → Crouch Behavior
   - Configure:
     - Crouch Key: `C`
     - Hold To Crouch: `false` (toggle mode)
     - Crouch Height: `1`
     - Standing Height: `2`
     - Crouch Speed: `2` (transition speed)
     - Camera Transform: Drag `PlayerCamera`
     - Camera Crouch Y: `0.3`
     - Camera Standing Y: `0.8`

2. **Test crouch**
   - Press Play
   - Press C to crouch/stand
   - Camera lowers smoothly
   - Movement speed reduced

### 4.2 Sprint

1. **SprintBehavior Component**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Movement → Sprint Behavior
   - Configure:
     - Sprint Key: `Left Shift`
     - Sprint Speed Multiplier: `1.5`
     - Use Stamina: `✓` (checked)
     - Stamina Drain Rate: `15` (per second)
     - Min Stamina To Sprint: `10`
     - Change FOV: `✓` (checked)
     - Player Camera: Drag `PlayerCamera`
     - Sprint FOV: `75`
     - Normal FOV: `60`

2. **Test sprint**
   - Press Play
   - Hold Shift + W to sprint forward
   - Stamina drains
   - FOV increases (speed effect)
   - Can't sprint when crouched

### 4.3 Lean

1. **LeanBehavior Component**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Movement → Lean Behavior
   - Configure:
     - Lean Left Key: `A` (AZERTY)
     - Lean Right Key: `E` (AZERTY)
     - Lean Angle: `15`
     - Lean Distance: `0.5`
     - Lean Speed: `5`
     - Camera Transform: Drag `PlayerCamera`

2. **Test lean**
   - Press Play
   - Hold A to lean left
   - Hold E to lean right
   - Camera tilts + shifts sideways

---

## Step 5: Add Flashlight (Horror)

1. **Create Spotlight**
   - Right-click PlayerCamera → Light → Spotlight
   - Name: `Flashlight`
   - Configure:
     - Type: `Spot`
     - Range: `15`
     - Spot Angle: `60`
     - Intensity: `2`
     - Color: White (slightly yellow tint optional)

2. **FlashlightSystem Component**
   - Select Flashlight
   - Add Component → Unity Mechanics Toolkit → Horror → Flashlight System
   - Configure:
     - Flashlight: Drag `Spotlight` component
     - Toggle Key: `F`
     - Max Battery: `100`
     - Drain Rate: `5` (per second)
     - Low Battery Threshold: `20`
     - Flicker When Low: `✓`
     - Flicker Intensity: `0.5`

3. **Test flashlight**
   - Press Play
   - Press F to toggle on/off
   - Battery drains when on
   - Flickers when low (<20%)

---

## Step 6: Add Sanity System (Horror)

1. **Create Post-Processing Volume** (Optional but recommended)
   - GameObject → Volume → Global Volume
   - Name: `PostProcessing`
   - Add → Post-Processing → Vignette
   - Add → Post-Processing → Chromatic Aberration

2. **SanitySystem Component**
   - Select Player
   - Add Component → Unity Mechanics Toolkit → Horror → Sanity System
   - Configure:
     - Max Sanity: `100`
     - Drain In Darkness: `✓`
     - Darkness Drain Rate: `2`
     - Can Recover: `✓`
     - Recovery Rate: `1`
     - Recovery Delay: `3`
     - Enable Visual Effects: `✓`
     - Post Process Volume: Drag `PostProcessing`

3. **Test sanity**
   - Press Play
   - Turn off flashlight (F)
   - Sanity drains in darkness
   - Visual effects increase (vignette, aberration)
   - Turn on flashlight to recover

---

## Step 7: Add Camera Effects

### 7.1 Camera Shake

1. **CameraShake Component**
   - Select PlayerCamera
   - Add Component → Unity Mechanics Toolkit → Camera → Camera Shake
   - Configure:
     - Default Intensity: `0.3`
     - Default Duration: `0.5`
     - Shake Frequency: `25`
     - Shake Position: `✓`
     - Shake Rotation: `✓`

2. **Test** (trigger via script or event later)

### 7.2 Head Bobbing

1. **HeadBobbing Component**
   - Select PlayerCamera
   - Add Component → Unity Mechanics Toolkit → Camera → Head Bobbing
   - Configure:
     - Enable Bobbing: `✓`
     - Walk Bobbing Speed: `14`
     - Walk Bobbing Amount: `0.05`
     - Sprint Bobbing Speed: `18`
     - Sprint Bobbing Amount: `0.1`
     - Enable Horizontal Bob: `✓`

2. **Test head bob**
   - Press Play
   - Walk around (WASD)
   - Camera bobs rhythmically
   - Sprint for faster bobbing

### 7.3 Look Behind

1. **LookBehind Component**
   - Select PlayerCamera
   - Add Component → Unity Mechanics Toolkit → Camera → Look Behind
   - Configure:
     - Look Behind Key: `V`
     - Hold To Look: `true`
     - Rotation Speed: `10`

2. **Test look behind**
   - Press Play
   - Hold V to look behind
   - Camera rotates 180°
   - Release to return to normal

---

## Step 8: Add Audio (Optional)

### 8.1 Footsteps

1. **Create AudioSource**
   - Select Player
   - Add Component → Audio Source
   - Uncheck `Play On Awake`

2. **FootstepSystem Component**
   - Add Component → Unity Mechanics Toolkit → Audio → Footstep System
   - Configure:
     - Surface Sounds: Add entries for different surfaces
       - Default: Add footstep AudioClips
       - Wood: Add wood footstep clips
       - Metal: Add metal footstep clips
     - Walk Step Interval: `0.5`
     - Run Step Interval: `0.3`
     - Walk Volume: `0.3`
     - Run Volume: `0.6`

3. **Tag ground surfaces**
   - Select ground plane
   - Tag: `Default` (or `Wood`, `Metal`, etc.)

### 8.2 Heartbeat (Optional)

1. **Create AudioSource for heartbeat**
   - Select Player
   - Add Component → Audio Source
   - Uncheck `Play On Awake`

2. **HeartbeatAudio Component**
   - Add Component → Unity Mechanics Toolkit → Audio → Heartbeat Audio
   - Configure:
     - Heartbeat Clip: Drag heartbeat sound
     - React To Sanity: `✓`
     - Low Sanity Threshold: `30`
     - React To Enemies: `✓`
     - Enemy Detection Radius: `10`

---

## ✅ Final Hierarchy

```
Player (CharacterController)
├── FirstPersonController
├── StaminaSystem
├── CrouchBehavior
├── SprintBehavior
├── LeanBehavior
├── SanitySystem
├── FootstepSystem (AudioSource)
└── PlayerCamera
    ├── Spotlight (Flashlight)
    │   └── FlashlightSystem
    ├── CameraShake
    ├── HeadBobbing
    └── LookBehind
```

---

## 🎮 Controls Summary

| Action | Key (AZERTY) | Key (QWERTY) |
|--------|--------------|--------------|
| Move Forward | Z | W |
| Move Backward | S | S |
| Strafe Left | Q | A |
| Strafe Right | D | D |
| Look | Mouse | Mouse |
| Jump | Space | Space |
| Crouch | C | C |
| Sprint | Left Shift | Left Shift |
| Lean Left | A | Q |
| Lean Right | E | E |
| Flashlight | F | F |
| Look Behind | V | V |

---

## 🧪 Testing Checklist

- [ ] Movement works (WASD/ZQSD + Mouse)
- [ ] Jump works
- [ ] Crouch lowers camera and reduces speed
- [ ] Sprint increases speed and drains stamina
- [ ] Lean tilts camera left/right
- [ ] Flashlight toggles on/off
- [ ] Battery drains over time
- [ ] Sanity drains in darkness
- [ ] Visual effects appear at low sanity
- [ ] Head bobbing rhythmic when walking
- [ ] Look behind rotates camera 180°
- [ ] Footsteps play based on surface

---

## 🚀 Next Steps

- **Tutorial 02**: Enemy AI Setup
- **Tutorial 03**: Interaction Systems
- **Tutorial 04**: Combat Systems
- **Tutorial 05**: Complete Horror Scene

---

**Congratulations!** You have a fully functional horror game player 🎉
