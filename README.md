# Empire of Sports 2.0 - Tennis Prototype

Modern reimagination of the world's first multi-sport MMORPG, built with Unity and AI-powered tools.

## Project Info

- **Engine:** Unity 6 (6000.3.2f1)
- **Language:** C#
- **IDE:** Visual Studio Code
- **First Sport:** Tennis
- **Target Platform:** PC (Windows), Web (WebGL)

## Project Structure

```
EmpireOfSports/
├── Assets/
│   ├── Scenes/              # Game scenes
│   ├── Scripts/             # All C# code
│   │   ├── Core/            # Core systems
│   │   ├── Player/          # Player logic
│   │   ├── Sports/          # Sport mechanics
│   │   │   └── Tennis/      # Tennis implementation
│   │   ├── Network/         # Multiplayer
│   │   ├── UI/              # UI scripts
│   │   └── Managers/        # Game managers
│   ├── Prefabs/             # Prefab objects
│   ├── Materials/           # Materials
│   ├── Textures/            # Textures
│   ├── Models/              # 3D models
│   ├── Animations/          # Animation files
│   ├── Audio/               # Sounds and music
│   └── Resources/           # Runtime loaded assets
└── [Other Unity folders]
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

## Current Status

- [x] Unity project created
- [x] Folder structure set up
- [x] .gitignore configured
- [x] HelloWorld.cs ready for testing
- [ ] VS Code configured as editor
- [ ] Input System installed
- [ ] First scene created
- [ ] Tennis court prototype started

## License

Private project - Not for distribution

---

Created: December 2025
Engine: Unity 6
Target: Tennis MVP in 3 months
