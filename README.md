# Empire of Sports 2.0 🎾⚽🏀

[![Unity Version](https://img.shields.io/badge/Unity-6.0-blue.svg)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-purple.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-Private-red.svg)](LICENSE)
[![Status](https://img.shields.io/badge/Status-In_Development-yellow.svg)](https://github.com/Starlight19973/empireofsports)

> Modern reimagination of the world's first multi-sport MMORPG, built with Unity 6 and AI-powered tools.

## 📋 Project Info

| Parameter | Value |
|-----------|-------|
| **Engine** | Unity 6 (6000.3.2f1) |
| **Language** | C# 12.0 |
| **IDE** | Visual Studio Code |
| **First Sport** | Tennis 🎾 |
| **Target Platform** | PC (Windows), Web (WebGL) |
| **Development Start** | December 2025 |
| **Target MVP** | 3 months |

## 🎯 About

Empire of Sports was the world's first multi-sport MMORPG, developed by French studio F4 and launched in 2009. With a budget of €30M, it featured 9 sports and ran until 2016.

**Our version** brings this concept back to life with:
- Modern Unity 6 engine
- AI-generated assets (Mixamo, Meshy.ai, Tripo3D)
- Free-to-use networking (Photon Fusion, PlayFab)
- Budget: $50K-100K (vs original $30M)
- Timeline: 3 months to MVP

## 📁 Project Structure

```
EmpireOfSports/
├── Assets/
│   ├── Scenes/              # Game scenes
│   ├── Scripts/             # All C# code
│   │   ├── Core/            # ✅ HelloWorld.cs
│   │   ├── Player/          # Player controller logic
│   │   ├── Sports/          # Sport mechanics
│   │   │   └── Tennis/      # Tennis implementation (TennisBall, CourtManager, etc.)
│   │   ├── Network/         # Photon Fusion multiplayer
│   │   ├── UI/              # UI scripts
│   │   └── Managers/        # Game managers (ScoreManager, etc.)
│   ├── Prefabs/             # Prefab objects
│   ├── Materials/           # Materials
│   ├── Textures/            # Textures
│   ├── Models/              # 3D models (from Mixamo, Meshy.ai)
│   ├── Animations/          # Animations (from Mixamo)
│   ├── Audio/               # Sounds and music
│   └── Resources/           # Runtime loaded assets
├── docs/                    # 📚 Complete documentation
│   ├── GETTING_STARTED_VSCODE.md
│   ├── EMPIRE_OF_SPORTS_QUICKSTART.md (full tennis code)
│   ├── EMPIRE_OF_SPORTS_RESEARCH.md
│   ├── EMPIRE_OF_SPORTS_SERVER_INFRASTRUCTURE.md
│   └── UNITY_SETUP_WINDOWS.md
├── claude.md                # Claude Code AI context
├── NEXT_STEPS.md            # ⭐ What to do next
└── README.md                # This file
```

## Quick Start

### First Time Setup

1. Open this project in Unity Hub
2. Wait for Unity to import packages (2-5 minutes)
3. Set VS Code as External Editor:
   - `Edit → Preferences → External Tools`
   - `External Script Editor → Visual Studio Code`

### Test the Setup

1. Open `Assets/Scenes/SampleScene.unity`
2. Create an empty GameObject: `Hierarchy → Create Empty`
3. Name it "GameManager"
4. Add `HelloWorld.cs` script to it
5. Press Play ▶️
6. Check Console (Ctrl+Shift+C) - you should see welcome messages
7. Press SPACE - should see "Input system working" message

### Next Steps

See the main documentation in `../docs/`:
- **GETTING_STARTED_VSCODE.md** - Complete VS Code + Unity guide
- **EMPIRE_OF_SPORTS_QUICKSTART.md** - Full tennis implementation code
- **../claude.md** - Context for Claude Code AI assistant

## Required Unity Packages

Install via `Window → Package Manager`:
- [ ] Input System (new input system)
- [ ] TextMeshPro (UI text)
- [ ] Cinemachine (camera control)

## Tech Stack

### Multiplayer
- **Photon Fusion** - Real-time networking (free: 100 CCU)
- **PlayFab** - Backend services (free: 100K MAU)

### AI Tools for Assets
- **Mixamo** - Free characters & animations
- **Meshy.ai** - 3D model generation (200 free credits/month)
- **Tripo3D** - Text-to-3D (150 free credits/month)

## Development Timeline

### Month 1: Prototype
- Tennis court creation
- Ball physics
- Player controller
- Basic AI opponent

### Month 2: Gameplay
- Score system
- Multiple shot types
- Court boundaries
- UI/UX

### Month 3: Multiplayer
- Photon integration
- 1v1 online matches
- Matchmaking
- Alpha testing

## Using Claude Code

From project root:
```bash
cd "C:\Empire  of sports\EmpireOfSports"
claude
```

Useful prompts:
```
> Explain Unity MonoBehaviour lifecycle
> Create a tennis ball script with realistic physics
> How to sync GameObject position with Photon Fusion?
```

## Resources

- [Unity Manual](https://docs.unity3d.com/Manual/)
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
- [Photon Fusion Docs](https://doc.photonengine.com/fusion/)
- Main project docs: `../docs/`

## 📊 Current Status

### ✅ Completed
- [x] Unity 6 project created
- [x] Folder structure organized
- [x] Git repository initialized
- [x] GitHub integration
- [x] Complete documentation written
- [x] HelloWorld.cs test script

### 🚧 In Progress
- [ ] VS Code configured as external editor
- [ ] Unity packages installed (Input System, TextMeshPro, Cinemachine)
- [ ] First tennis scene created
- [ ] Tennis court prototype

### 📅 Roadmap (3 months)

#### Month 1: Prototype
- Tennis court + ball physics
- Player controller (WASD movement)
- Basic AI opponent
- Score system

#### Month 2: Gameplay
- Multiple shot types
- Power/spin mechanics
- UI/UX polish
- Sound effects

#### Month 3: Multiplayer
- Photon Fusion integration
- 1v1 online matches
- Matchmaking
- Alpha testing (50-100 players)

## License

Private project - Not for distribution

---

Created: December 2025
Engine: Unity 6
Target: Tennis MVP in 3 months
