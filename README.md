# ?? Rush Hour - An Endless Driving Game

## Overview
**Rush Hour** is an addictive endless driving game built with **.NET 9.0** and **.NET MAUI**. Navigate your customizable car through 5 lanes of traffic, avoid oncoming vehicles, collect coins, grab speed boosts, and beat your high score. The longer you survive, the faster the traffic becomes!

---

## ?? Game Features

### Core Gameplay
- **Endless Driving**: Navigate downward through 5 lanes continuously
- **3 Lives**: You start with 3 lives; lose them all and it's game over
- **Auto-Scrolling Traffic**: Enemy vehicles spawn from the top and move toward you
- **Real-time Score**: Points increase based on distance traveled and coins collected
- **Difficulty Scaling**: Game gets progressively harder as you travel further

### Vehicle Types
| Vehicle | Spawn Rate | Appearance | Special Feature |
|---------|-----------|-----------|-----------------|
| **Regular Cars** | 65% | Colored vehicles | Yellow headlights |
| **Police Cars** | 10% | White | Red & Blue sirens with light beams |
| **Firetrucks** | 10% | Red, 3x length | Red & Blue sirens with light beams |
| **16-Wheelers** | 5% | Brown, 4x length | Cab + trailer design with multiple axles |

### Pickups
- ?? **Coins** (90%): Collect for +50 points
- ? **Speed Boosts** (10%): Temporary +200 speed increase

### Obstacles
- ?? **Road Blockades**: Traffic cones with stone walls - avoid at all costs!

---

## ?? Settings & Customization

### Game Settings
- **Difficulty Mode**: Easy (0.7x), Medium (1.0x), Hard (1.5x) - affects vehicle spawn rate
- **Time Limit**: Set a time limit (0 = Unlimited)
- **Font Size**: Adjust UI text size (10-24px)

### Visual Customization
- **Dark Mode Toggle**: Light/Dark theme with dynamic text colors
- **Player Car Color**: Choose from 6 colors (Red, Blue, Yellow, Green, Cyan, Magenta)

---

## ?? Statistics & Tracking

### Tracked Metrics
- **High Score**: Your personal best score (persisted)
- **Total Distance**: Cumulative distance traveled
- **Total Coins**: Lifetime coins collected
- **Games Played**: Total number of games completed

All statistics are saved locally and persist between sessions.

---

## ?? How to Play

### Controls
| Action | Method |
|--------|--------|
| **Move Left** | LEFT Button |
| **Move Right** | RIGHT Button |
| **Pause/Resume** | PAUSE Button |
| **Start Game** | START Button |
| **Restart** | RESTART Button (after game over) |

### Gameplay Loop
1. Start at the center of 5 lanes
2. Use LEFT/RIGHT buttons to dodge oncoming traffic
3. Collect ?? coins for points (+50 each)
4. Grab ? speed boosts for temporary speed increase
5. Avoid ?? cars and ?? blockades
6. Survive as long as possible!

### Scoring System
- **Obstacle Avoided**: +10 points base (multiplied by difficulty)
- **Coin Collected**: +50 points base (multiplied by difficulty)
- **Difficulty Multiplier**:
  - Easy: 0.7x
  - Medium: 1.0x
  - Hard: 1.5x

---

## ??? Technical Architecture

### Technology Stack
- **Framework**: .NET MAUI (.NET 9.0)
- **UI Framework**: Native MAUI Controls
- **Graphics**: MAUI Canvas/GraphicsView
- **Persistence**: MAUI Preferences API
- **Threading**: MAUI Dispatcher/IDispatcherTimer

### Project Structure
```
Raymond_Sean_RushHour/
??? GamePage.cs                 # Main game logic & rendering
??? MainPage.xaml/cs            # Game launcher & stats
??? SettingsPage.cs             # Settings & customization
??? ScorePage.cs                # Statistics dashboard
??? Models/
?   ??? Obstacle.cs             # Enemy vehicle model
?   ??? Pickup.cs               # Coins & boosts model
?   ??? Player.cs               # Player car model
?   ??? GameState.cs            # Game state management
?   ??? AppSettings.cs          # User preferences persistence
??? Services/
?   ??? GameService.cs          # Game logic & scoring
??? Resources/
?   ??? Fonts/                  # BBHBogle-Regular.ttf
?   ??? Images/                 # SVG assets
?   ??? AppIcon/Splash/         # App branding
??? Platforms/
    ??? Android/                # Android-specific config
    ??? iOS/                    # iOS-specific config
    ??? Windows/                # Windows-specific config
```

### Game Loop (30ms Tick)
```
1. Update game state
2. Spawn vehicles (probabilistic)
3. Move all game objects
4. Check collisions
5. Update UI
6. Render canvas
```

---

## ?? Platform Support

### Supported Platforms
- ? **Windows** (net9.0-windows10.0.19041.0)
- ? **Android** (net9.0-android, API 21+)
- ? **iOS** (net9.0-ios, v15.0+)
- ? **macOS** (net9.0-maccatalyst)

### Tested Environments
- Windows 10/11 with .NET 9.0 SDK
- Android Emulator (API 31+)
- Physical Android devices

---

## ?? Getting Started

### Prerequisites
- .NET 9.0 SDK installed
- Visual Studio 2022 or VS Code with C# extension
- Android NDK/SDK (for Android builds)

### Build Instructions

#### Windows
```bash
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

#### Android
```bash
dotnet build -f net9.0-android
# Deploy to emulator or device
```

### Run the Game
1. Press START on the main menu
2. Select difficulty
3. Press PLAY
4. Use LEFT/RIGHT to navigate
5. Survive as long as possible!

---

## ?? Visual Design

### Color Scheme
- **Background**: Grey asphalt (#808080)
- **UI Accent**: Lilac Purple (#B19CD9)
- **Text**: White (light mode), Black (dark mode)
- **Lanes**: White dashed lines
- **Player Car**: Customizable (6 colors)

### Asset Details
- **Font**: BBHBogle-Regular.ttf (custom game font)
- **Graphics**: All rendered via canvas API (no external image dependencies)
- **Icons**: SVG-based resources

---

## ?? Game Mechanics Deep Dive

### Vehicle Spawning
- Spawn rate decreases on higher difficulty
- Easy: ~43ms between spawns
- Medium: ~30ms between spawns
- Hard: ~20ms between spawns

### Speed Progression
```
Base Speed = 5.5 units/frame
Progressive Speed = Base × (1 + Distance/1000) × Difficulty Multiplier
Max Speed: Infinite (increases with distance)
```

### Collision Detection
- Hitbox-based collision with vehicle position
- 100px lead distance for detection
- Instant life loss on collision

### Boost Mechanics
- Activates on pickup
- Adds +200 speed temporarily
- Duration: ~200ms of boost effect

---

## ?? Data Persistence

### Saved Data
```
AppSettings (per-device, encrypted)
??? DarkMode (bool)
??? FontSize (int)
??? DifficultyMode (string)
??? TimeLimit (int)
??? BoxColor (hex string)
??? HighScore (int)

Device Preferences
??? GamesPlayed (int)
??? TotalDistance (int)
??? TotalCoins (int)
??? HighScore (int)
```

---

## ?? Known Issues & Limitations

- Police car lights use same position (stacked) - visual effect intended
- No sound effects (requires platform-specific audio implementation)
- No multiplayer support
- No leaderboards
- Time limit feature works but not heavily featured

---

## ?? Future Enhancements

Potential features for future versions:
- ?? Sound effects for collisions, pickups, engine
- ?? Background music
- ?? Global leaderboards
- ??? Achievement system
- ?? Unlockable vehicle skins
- ?? Power-ups (shield, slow-motion)
- ?? Night mode graphics
- ?? Custom car image uploads
- ?? Replay system

---

## ?? License

This project is created for educational purposes as part of a college game development assignment.

---

## ????? Developer

**Sean Raymond**  
Rush Hour © 2024  
Built with ?? using .NET MAUI and .NET 9.0

---

## ?? Repository

GitHub: [https://github.com/SeanRaymond03/Raymond_Sean_RushHour](https://github.com/SeanRaymond03/Raymond_Sean_RushHour)

---

## ?? Support

For issues, bugs, or feature requests, please visit the GitHub repository and create an issue.

---

**Enjoy the Rush Hour experience! ????**
