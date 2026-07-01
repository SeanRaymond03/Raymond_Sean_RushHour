#  Rush Hour - An Endless Driving Game

## Overview
**Rush Hour** is an driving game built with **.NET 9.0** and **.NET MAUI**. Navigate your customizable car through 5 lanes of traffic, avoid oncoming vehicles, collect coins, grab speed boosts, and beat your high score. The longer you survive, the faster the traffic becomes!



##  Game Features

### Core Gameplay
- **Endless Driving**: Navigate downward through 5 lanes continuously
- **3 Lives**: You start with 3 lives; lose them all and it's game over
- **Auto-Scrolling Traffic**: Enemy vehicles spawn from the top and move toward you
- **Real-time Score**: Points increase based on distance traveled and coins collected
- **Difficulty Scaling**: Game gets progressively harder as you travel further

<img width="525" height="736" alt="RushHourGamePlay" src="https://github.com/user-attachments/assets/4b9ba0f7-58d7-4715-be83-88be40e7e5f2" />
<img width="525" height="730" alt="RushHourSettings" src="https://github.com/user-attachments/assets/6d580d49-8ba1-455b-9b3e-b51f7ef8925b" />
<img width="524" height="752" alt="RushHourLeaderboard" src="https://github.com/user-attachments/assets/b6fb21d4-d2bf-4aa6-8fa7-d2a10b98e4b0" />
<img width="524" height="734" alt="RushHourGameOver" src="https://github.com/user-attachments/assets/4e929335-d5c8-422f-b678-1b279b30d5a7" />


##  Statistics & Tracking

### Tracked Metrics
- **High Score**: Your personal best score (persisted)
- **Total Distance**: Cumulative distance traveled
- **Total Coins**: Lifetime coins collected
- **Games Played**: Total number of games completed

All statistics are saved locally and persist between sessions.

---

##  How to Play

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
3. Collect  coins for points (+50 each)
4. Grab speed boosts for temporary speed increase
5. Avoid cars and blockades
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


##  Platform Support

### Supported Platforms
-  **Windows** (net9.0-windows10.0.19041.0)
-  **Android** (net9.0-android, API 21+)
-  **iOS** (net9.0-ios, v15.0+)
-  **macOS** (net9.0-maccatalyst)

### Tested Environments
- Windows 10/11 with .NET 9.0 SDK
- Android Emulator (API 31+)
- Physical Android devices


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


##  Known Issues & Limitations

- Police car lights use same position (stacked) - visual effect intended
- No sound effects (requires platform-specific audio implementation)
- No multiplayer support
- No leaderboards
- Time limit feature works but not heavily featured


**Sean Raymond**  
Rush Hour © 2024  
Built using .NET MAUI and .NET 9.0
