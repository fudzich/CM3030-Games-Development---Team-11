# 🥷 Ninja Survivors: Co-op Action Survival

> A cooperative, wave-based horde survival game developed by Team 11 for the CM3030 Games Development module. 

![Language](https://img.shields.io/badge/Language-C%23-239120) ![Engine](https://img.shields.io/badge/Engine-Unity-black) ![Category](https://img.shields.io/badge/Project-Academic%20%2F%20Team-purple)

## 💡 Overview & Team Collaboration

Developed as a joint project for the BSc Computer Science program at the University of London, *Ninja Survivors* is a fast-paced co-op survival game. Building this project required our three-person team to maintain strict version control coordination, manage a shared asset pipeline, and divide complex gameplay mechanics into modular scripts to prevent merge conflicts.

*(Note on Repository Size: This repository serves as our complete, uncompiled project archive. It intentionally includes all raw game assets, including high-resolution sprites, audio files, animations, and UI elements, to demonstrate our team's end-to-end asset integration and rendering pipeline).*

## ⚙️ Core Technical Implementation

### 1. Cooperative Systems & State Management
*   **Shared Game State:** Synchronized multiple player states, hitboxes, and independent health systems to support seamless co-op mechanics without input lag.
*   **Collision & Physics:** Utilized rigid-body physics and custom collision matrices to ensure accurate weapon strikes and enemy pathfinding across all active player instances.

### 2. Horde Mechanics & Optimization
*   **Dynamic Spawning:** Engineered a wave-based spawning algorithm that scales enemy count and difficulty linearly over time.
*   **Object Pooling:** To prevent memory leaks and frame-rate drops when rendering hundreds of enemies and projectiles simultaneously, memory allocation is handled via strict Object Pooling rather than continuous instantiation and destruction.

### 3. Asset & UI Pipeline
*   Integrated raw textures, animations, and audio clips into a cohesive user interface, managing the bridge between backend C# logic and frontend canvas updates (health bars, timers, score tracking) for multiple players.

