# GAME DESIGN DOCUMENT (GDD)
## ASHES OF THE FIRST LIGHT

| Attribute | Details |
| :--- | :--- |
| **Game Name** | *Ashes of the First Light* |
| **Genre** | Metroidvania |
| **Game Elements** | Non-linear exploration of an interconnected world, technical real-time 2D combat featuring parries, agile mobility skills (dash, wall jump, and light-toggle mechanics) for high-precision platforming, and environmental puzzle-solving alongside challenging boss fights. |
| **Player** | Single player |
| **Platform / Devices** | Consoles and PC |

---

## 1. TECHNICAL SPECS

* **Technical Form:** 2D / 2.5D Side-Scroller (2D Sprites or 3D models restricted to a 2D plane, featuring multi-layered parallax backgrounds and dynamic lighting).
* **View:** Side-Scrolling perspective with smooth orthographic camera tracking locked to the main action plane.
* **Platform:** PC and Consoles (Nintendo Switch, PlayStation, Xbox).
* **Language:** C# (developed in Unity Engine).
* **Device:** Gamepad / Controller (recommended primary input) and Keyboard + Mouse for PC.

---

## 2. GAME PLAY

Players step into the shoes of a Guardian of Light venturing into the depths of a dense pre-Columbian jungle dotted with majestic medieval ruins. The gameplay blends the agility of a precision platformer with the tension of direct, tactical combat. Movement through the world is smooth and fluid: the Guardian sprints, slides down ancient stone walls, and wall-jumps across chasms and environmental hazards.

The core of exploration and puzzle-solving lies in manipulating the character’s halo of light. Toggling this light alters the very reality of the environment: platforms hidden in shadow materialize, secret passages are revealed, and certain obstacles only take physical shape when the world is plunged into darkness. To navigate this vast, interconnected world, players discover regions organically, charting map fragments using points of interest (pings) and visual markers purchased from merchants found in newly explored areas.

Combat is strictly close-quarters, relentless, and unforgiving. While basic enemies respawn across sectors to challenge the player’s progress, boss fights demand flawless reading and execution. Since the dash lacks invincibility frames (i-frames), the primary defense relies on precise parries: when a boss unleashes a devastating volley of close-range or long-range attacks, the Guardian must block at the exact moment. Executing a successful parry breaks the enemy's posture, creating a brief window of vulnerability to land a lethal counterattack. Stamina regenerates gradually during combat, while health must be carefully managed using limited-dose elixirs or by resting at sacred save altars.

### Game Play Outline

This outline will vary depending on the type of game:
* **Opening the game application:** Main title screen and loading at the last saved altar.
* **Game options:** Controls mapping, sound levels, and contrast/brightness calibration (essential for the light/shadow toggle).
* **Story synopsis:** The journey of the Guardian of Light through the overgrown pre-Columbian jungle and medieval ruins.
* **Modes:** Main Story / Adventure Mode.
* **Game elements & Game levels:** Non-linear exploration, wall-sliding/jumping, light-manipulation puzzles, and parry-focused combat.
* **Player's controls:** Movement, Jump, Wall Slide, Dash, Light Toggle (On/Off), Melee Attack, Parry, and Elixir Consumption.
* **Winning & Losing conditions:**
  * **Winning:** Defeating regional bosses and restoring the primal light in the core of the ruins.
  * **Losing:** Running out of health points; respawning at the last visited save altar.
* **End:** Final credits and story resolution.
* **Why is all this fun?:** The satisfaction of mastering tight parry mechanics combined with the discovery of hidden paths by manipulating light and shadow.

### Key Features
* Dynamic Light/Darkness mechanics that alter level geometry and reveal hidden paths.
* High-precision 2D platforming with wall-sliding and wall-jumping.
* High-stakes melee combat centered around precise timing and parry mechanics.
* Organic map discovery paired with purchasable sector maps and custom map pings.

---

## 3. DESIGN DOCUMENT

This document describes how GameObjects behave, how they're controlled, and their properties. This is often referred to as the "mechanics" of the game. This documentation is primarily concerned with the game itself. This part of the document is meant to be modular, meaning you could have several different Game Design Documents attached to the Concept Document.

### Design Guidelines
This section outlines creative restrictions and general objectives:
* Combat must reward precision and timing over button-mashing.
* Light manipulation should feel like an active puzzle-solving tool rather than just a visual gimmick.

### Game Design Definitions
Establishes core loop definitions: win/loss conditions, level transition rules, and core mechanical priorities.

### Game Flowchart
The game flowchart provides a visual representation of how different game elements interact:
* **Menu** -> **Story Synopsis** -> **Game Play Loop** -> **Player Control** -> **Game Over (Win/Loss)**

---

## 4. PLAYER DEFINITION

Use this section for quick descriptions that define the player character:

### Player Definitions
* **Health:** Represented by vitality bars; restored via limited elixirs or by resting at sacred save altars.
* **Weapons:** Melee weapon for close-quarters combat.
* **Actions:** Sprinting, wall sliding, wall jumping, dash (without i-frames), light toggling, melee slashing, parrying, and elixir consumption.

### Player Properties
* **Stamina:** Depletes with actions and regenerates gradually over time.
* **Light Halo:** Active toggle affecting ambient visibility and environmental collisions.

### Player Rewards (Power-ups and Pick-ups)
* **Elixirs:** Limited consumables that restore health in the field.
* **Map Fragments:** Purchased from merchants to reveal uncharted areas.

---

## 5. USER INTERFACE (UI)

This section describes the user's control layout and HUD elements:
* **HUD Layout:** Minimalist design displaying Health, Stamina, available Elixirs, and Light Halo status.
* **Map Screen:** Interactive map showing discovered sectors and player-placed pings/markers.
* **Control Mapping:** Optimized for Gamepad (Triggers for Dash/Parry, Face buttons for Jump/Attack/Light Toggle).