# Multiplayer Battle Royale Game

> 100-player networked battle royale with Unity, Photon PUN, shrinking zones, weapon system, and loot mechanics.

## 🚀 Overview

A complete battle royale game built in Unity with Photon PUN 2 for multiplayer networking. Features include a shrinking safe zone (6 phases), multiple weapon types with ballistic simulation, random loot spawning, a network-synced health/shield system, and matchmaking lobby.

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🎮 100 Players | Photon PUN cloud-hosted rooms |
| 🗺️ Shrinking Zone | 6-phase zone with increasing damage |
| 🔫 Weapon System | AssaultRifle, Shotgun with ballistics |
| 🎒 Loot Spawning | Random placement across the map |
| ❤️ Health + Shield | Damage reduction system |
| 📡 Network Sync | Smooth state interpolation |
| 🏆 Kill Feed | Real-time elimination notifications |
| 🎯 Matchmaking | Lobby system with room management |

## 🛠️ Tech Stack

| Component | Technology |
|-----------|-----------|
| Engine | Unity 2022 LTS |
| Networking | Photon PUN 2 |
| Language | C# |
| Server | Photon Cloud |

## ⚡ Quick Start

1. Open project in **Unity 2022.3+**
2. Import **Photon PUN 2** from Asset Store
3. Set Photon App ID in `PhotonServerSettings`
4. Open `MainScene` and press Play

## 📁 Key Scripts

| Script | Purpose |
|--------|---------|
| `PlayerController.cs` | Movement, shooting, health, network sync |
| `GameManager.cs` | Match lifecycle, player tracking |
| `ZoneController.cs` | Shrinking safe zone with 6 phases |

## 📄 License

MIT
