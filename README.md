# Stack Ball 🏀

A hyper-casual mobile game built with Unity, where you control a ball smashing through rotating platforms to reach the finish line.

## 🎮 Gameplay

- **Tap & Hold** — Smash the ball down through enemy platforms
- **Release** — Bounce off platforms and avoid falling
- **Invincibility Mode** — Keep smashing to charge the invincibility meter. When full, you can destroy any platform (including black "plane" obstacles)
- **Reach the Finish** — Break through all platforms to complete the level

## 📂 Project Structure

| File | Description |
|------|-------------|
| `Ball.cs` | Core player logic — movement, collision, state machine (Prepare → Playing → Died → Finish), invincibility system |
| `GameUI.cs` | UI management — home screen, in-game HUD, level progress bar, win/game-over panels |
| `LevelSpawner.cs` | Procedural level generation — spawns platforms with rotation offsets, random model selection |
| `StackController.cs` | Controls a single stack of platform parts; shatters on collision |
| `StackPartController.cs` | Individual part physics — applies force and torque when shattered |
| `ScoreManager.cs` | Score tracking, high score persistence with `PlayerPrefs` |
| `SoundManager.cs` | Singleton audio manager — toggle sound on/off |
| `CameraFollow.cs` | Camera follows the ball downward; locks to win position at the finish |
| `Rotator.cs` | Spins the entire level group, creating the rotating visual effect |
| `Ignore.cs` | Empty marker component; attached to UI elements to ignore raycast in `IgnoreUI()` |

## ⚙️ How It Works

### Level Generation
`LevelSpawner` procedurally builds the level in `Start()`:
1. Picks a random color scheme
2. Selects a random model set (5 variations, 4 models each)
3. Instantiates platforms (`(level + addOn) * 2` pieces) downward with rotation offsets
4. Places the finish object at the bottom of the level

### Ball State Machine
```
Prepare → Playing → Died (game over)
                  → Finish (level complete)
```
- **Prepare** — Home screen, waiting for tap
- **Playing** — Player controls the ball
- **Died** — Hit a black plane without invincibility → score resets, game-over UI shows
- **Finish** — Reached the bottom → win UI shows, tap to go to next level

### Invincibility System
- Smashing charges a meter (`_currentTime`)
- At 30% charge the invincibility icon appears
- At 100% charge the ball becomes invincible (red indicator)
- While invincible, any platform (including black planes) is destroyed on contact

### Collision Logic
- **Not smashing** — Ball bounces off platforms
- **Smashing + not invincible** — Destroys enemy platforms; black planes kill the ball
- **Smashing + invincible** — Destroys everything (enemies + planes)

## 🛠️ Built With

- **Unity** (URP)
- **C#**

## 🧩 Dependencies

- Unity Input System
- Unity Universal Render Pipeline (URP)