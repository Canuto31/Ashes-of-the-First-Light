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
* **Target Platform:** PC first for development and MVP validation. Console targets (Nintendo Switch, PlayStation, and Xbox) are post-MVP goals.
* **Language:** C# (developed in Unity Engine).
* **MVP Input Device:** Keyboard and Mouse on PC. Gamepad movement and full controller mapping will be implemented after the keyboard-and-mouse version is functional and validated.

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
* **Confirmed MVP Weapon — Solar Sword:** The Guardian uses a balanced close-quarters sword powered by solar energy. It serves as the single playable weapon in the first MVP and establishes the baseline for attack speed, range, damage, parry timing, and future weapon comparisons.
* **Actions:** Sprinting, wall sliding, wall jumping, dash (without i-frames), light toggling, melee slashing, parrying, and elixir consumption.

### Player Properties
* **Stamina:** Depletes when using the dash and may also support selected combat actions. It regenerates gradually over time. Jumping, basic movement, basic attacks, and toggling the Light Halo should not consume stamina unless later testing demonstrates a clear need.
* **Light Halo:** Active toggle affecting ambient visibility and environmental collisions.

### Roll, Air Dash, and Traversal Gates

* **Ground Roll:** The initial ground-based evasive movement is a roll rather than a conventional dash. It consumes stamina and, under the current combat direction, does not grant invincibility frames.
* **Air Dash:** A separate aerial impulse propels the Guardian while airborne. It consumes stamina and does not grant invincibility frames. It may be used repeatedly during the same fall or jump whenever the Guardian still has enough stamina.
* Both the Ground Roll and the initial Air Dash are acquired during the first MVP before the fight against La Patasola, allowing the introductory sector to teach and test them.
* Their upgrades are part of generic character progression rather than Solar Fragment rewards.
* There is no separate chained-dash ability or fixed air-dash charge count. The number of consecutive aerial impulses emerges from the current stamina reserve and the cost of each use.
* Improvements may affect travel distance, stamina capacity, stamina cost, recovery time, or direction control, indirectly allowing additional aerial impulses.
* **Confirmed Stamina Rule:** Stamina does not regenerate while the Guardian is airborne or touching a wall. Regeneration resumes only after making valid contact with the ground. This prevents indefinite aerial movement through repeated wall contact.
* **Confirmed Wall Rule:** Valid wall contact refreshes the double-jump charge, but does not restore stamina or refund aerial impulses. Wall slide and wall jump do not consume stamina.
* Certain routes, gaps, hazards, and deeper portions of the world remain inaccessible until the appropriate movement improvement has been acquired.
* Several routes should remain available in parallel after the first sector, while upgraded movement gradually opens additional areas and optional backtracking paths.
* The exact upgrade tiers, acquisition sources, stamina values, and which routes they gate remain to be defined.

### Parry System

* **Confirmed MVP Direction — Precision Parry:** The player must press the parry input within a short timing window before impact. A successful parry completely negates the incoming damage, deals substantial posture damage to the attacker, and creates an opportunity to counterattack. A missed parry leaves the Guardian exposed to the full attack.
* **Optional Variant — Imperfect Guard (not confirmed):** An input outside the perfect-parry window, or holding the input beyond it, may reduce rather than negate incoming damage. This imperfect defense consumes stamina, deals no posture damage to the attacker, and does not create a counterattack opening.
* The optional guard variant must be tested against the intended difficulty and precision-focused combat identity before inclusion.

### Weapon Progression — Optional Design (Not Confirmed)

* The first MVP contains only the confirmed Solar Sword and does not require a weapon-switching system.
* In the potential full-game system, defeating a sector boss allows the Guardian to inherit a weapon associated with that boss, potentially the weapon used during the encounter.
* The player chooses between keeping the current weapon and accepting the newly offered one.
* Accepting a new weapon is a permanent commitment for that playthrough: the previous weapon cannot be recovered or re-equipped.
* Each weapon changes the Guardian's combat rhythm, range, speed, damage, and available techniques, requiring the player to adapt their playstyle.
* Because sector bosses may be confronted in different orders, weapon choices create different combat paths and encourage additional playthroughs.
* Before confirming an irreversible change, the game must clearly present the new weapon's behavior, strengths, weaknesses, and permanence. A safe preview or short training interaction should be considered to prevent accidental choices.
* The system is optional and provisional. Its production cost, balance implications, interaction with upgrades, and effect on player frustration must be evaluated after the first MVP validates the core weapon.

### Player Rewards (Power-ups and Pick-ups)
* **Elixirs:** Limited consumables that restore health in the field.
* **Map Fragments:** Purchased from merchants to reveal uncharted areas.

### Cenizas de Luz Economy — Confirmed MVP Scope

* **Acquisition:** Cenizas de Luz are awarded for defeating standard enemies, completing encounters, discovering secrets, and collecting hidden currency deposits.
* **Map Purchases:** Currency may be spent to complete the map detail for areas the player has already visited. Purchased information may also indicate suspicious connections, incomplete routes, or the possible presence of hidden subsectors without directly revealing every secret or its solution.
* **Skill Improvements:** Currency funds a basic skill tree included in the first MVP.
* **Elixir Capacity:** Currency contributes to unlocking additional elixir slots, subject to the provisional maximum capacity of three to five.
* **MVP Economy Goal:** Prices, rewards, and loss risk should create meaningful choices between navigation information, character upgrades, and survivability.

### Basic Skill Tree — Confirmed Hybrid Structure

The first MVP uses a hybrid progression model combining exploration rewards with optional purchases. Essential movement and combat techniques are discovered through play and cannot be permanently blocked by a lack of currency.

The tree contains three branches:

* **Combat:** Solar Sword techniques, parry improvements, and posture damage.
* **Mobility:** Ground Roll, Air Dash, aerial control, and movement efficiency.
* **Survival:** Health, stamina, and elixir-related improvements.

Each branch should contain at least:

* Two upgrades earned by exploration, progression milestones, hidden sources, or checkpoints.
* One optional upgrade purchased with Cenizas de Luz.

Purchased nodes improve or specialize abilities but should not be mandatory to complete the primary route. Exact nodes, costs, dependencies, and acquisition locations remain to be designed and balanced.

#### Initial MVP Nodes

* **Combat — Exploration:** Charged heavy attack.
* **Combat — Exploration:** Special counterattack after a successful precision parry.
* **Combat — Purchase:** Moderate increase to posture damage.
* **Mobility — Exploration:** Ground Roll.
* **Mobility — Exploration:** Air Dash.
* **Mobility — Purchase:** Reduced stamina cost for the Ground Roll and Air Dash.
* **Survival — Exploration:** Increased maximum health.
* **Survival — Exploration:** Increased maximum stamina.
* **Survival — Purchase:** One additional elixir slot.

#### Future Design Fields — Intentionally Pending

The following details must be completed after the first gameplay tests provide usable balance data:

* Numerical effect of every node.
* Cenizas de Luz purchase prices.
* Prerequisites and connections between nodes.
* World, checkpoint, or source location for each exploration upgrade.
* Timing of each unlock within the first sector.
* Final icons, names, descriptions, and UI presentation.

### Health, Healing, Checkpoints, and Death

* **Health Display:** The Guardian's health bar appears in the upper-left corner of the HUD.
* **Elixir Capacity — Provisional:** The maximum capacity should fall between three and five elixirs. The initial target is three, with the possibility of unlocking additional slots up to five.
* **Elixir Acquisition:** Elixirs may be found during exploration. They do not regenerate automatically over time.
* **Checkpoint Refill:** Resting at a checkpoint restores the default allocation for every elixir slot the player has unlocked. The first MVP target is a refill of up to three elixirs.
* **Healing Action:** Drinking an elixir uses a vulnerable animation lasting approximately one to two seconds. The exact duration and whether enemy damage cancels the heal must be tested.
* **Checkpoint Recovery:** Resting restores health completely, refills the unlocked default elixir allocation, preserves carried currency, and saves progress. Recovery of any future mana-like combat resource remains dependent on that system's final design.
* **Enemy Respawn:** Resting at a checkpoint respawns standard enemies. Bosses and other unique defeated encounters remain completed unless explicitly designed otherwise.
* **Confirmed Currency Name — Cenizas de Luz (Ashes of Light):** The primary progression currency carried by the Guardian is called Cenizas de Luz.
* **Confirmed Death Recovery — Ascua Caída (Fallen Ember):** On death, all carried Cenizas de Luz are stored in a solar ember created near the place of death. Touching or briefly channeling the Ascua Caída restores the full amount.
* The Ascua Caída remains visible in both Light On and Light Off states.
* If the Guardian dies again before recovering it, the previous Ascua Caída and all currency stored within it are permanently lost. A new ember is then created containing any currency collected since the previous death.
* If death occurs in a bottomless pit, lethal hazard, or inaccessible position, the Ascua Caída appears on the most recent safe surface.

---

## 5. USER INTERFACE (UI)

This section describes the user's control layout and HUD elements:
* **HUD Layout:** Minimalist design displaying Health, Stamina, available Elixirs, and Light Halo status.
* **Map Screen:** Interactive map showing discovered sectors and player-placed pings/markers.
* **Control Mapping:** The first MVP is designed and tested with Keyboard and Mouse. Actions should be implemented through Unity input actions rather than hard-coded device checks so a complete Gamepad mapping can be added after MVP validation.

### Confirmed MVP Keyboard and Mouse Mapping

* **A / D:** Move left and right.
* **Space:** Jump and double jump.
* **Left Shift:** Ground Roll while grounded; Air Dash while airborne.
* **Left Mouse Button:** Standard attack; hold for charged heavy attack.
* **Right Mouse Button:** Precision parry.
* **Q:** Toggle the Light Halo between Light On and Light Off.
* **E:** Interact, collect, or activate.
* **R:** Consume healing elixir.
* **F:** Activate Solar Fulgor.
* **Tab:** Open inventory and notes.
* **M:** Open map.
* **Escape:** Pause menu.
* Wall Slide activates automatically while directional input is held toward a valid wall.
* Individual bindings may change after ergonomic playtesting, but the required actions are confirmed.

### Inventory Interface — Confirmed MVP Direction

* The inventory uses a slot-based visual layout.
* Each slot displays the item's icon and its current quantity when the item can stack above one unit.
* Selecting a slot displays the item's available information and description.
* Discoverable consumable types may begin unidentified. Their complete description and effect remain hidden until the player consumes that item type for the first time.
* After the first use, the description is permanently revealed and preserved by the save system.
* Essential key items, Solar Fragments, and the standard healing elixir should communicate their function immediately rather than requiring blind consumption.
* Inventory capacity limits, stack limits, item categories, sorting behavior, and the complete MVP item list remain to be defined.

#### Confirmed MVP Item Set

* Standard healing elixir.
* Two unidentified consumable types used to validate discovery through first consumption; their identities and effects remain pending.
* Key progression items, including at least one sector key used with a mechanism.
* Fragment of Life.
* Two to four short collectible notes, potentially containing multiple pages.

### Keys and Progression Mechanisms — Confirmed MVP Direction

* Keys are identifiable key items stored in the inventory and do not use the unidentified-consumable rules.
* The player may need to explore a scenario, locate a key, and carry it to a specific lever, pedestal, lock, or equivalent mechanism.
* Interacting with the correct mechanism while carrying its required key consumes or inserts the key and permanently opens the associated door or route.
* Attempting to use the mechanism without its key provides clear visual or textual feedback identifying that a required object is missing.
* Key items are not lost on death, do not become part of the Ascua Caída, and remain collected after standard enemies respawn.
* Used mechanisms and opened progression doors persist through checkpoints, death, saving, and loading.
* The first MVP includes at least one complete key-to-mechanism-to-door sequence. Exact placement belongs to level design.

### Notes Interface — Confirmed MVP Direction

* Collected notes are accessed from the inventory or its associated journal view.
* The left side of the interface displays the list of collected notes.
* The right side displays the selected note as a readable document.
* A note may contain multiple pages, with explicit controls and visual feedback for turning between them.
* Uncollected notes do not appear as readable entries. Unread and newly collected states should be visually distinguishable.
* Collected notes and the last-read state must persist through saving and loading.
* Final typography, page animation, categories, filtering, and the number and length of MVP notes remain to be defined.

---

## 6. NARRATIVE PREMISE AND CORE LOOP

The player controls the **Guardian of the Sun**, who awakens after a malignant entity steals the Sun and divides it into six fragments. Each fragment is guarded by a sector boss. The Guardian must recover all six fragments to restore light to the world and unlock the region where the final enemy awaits.

### Core Game Loop

* Explore a sector and discover its routes.
* Overcome combat encounters, precision-platforming challenges, and light/dark puzzles.
* Activate checkpoints, acquire generic abilities, and unlock shortcuts.
* Defeat the sector boss and recover its Solar Fragment.
* Restore part of the world's ambient light, empower the Guardian's Solar Fulgor, and obtain a sector-specific ability.
* Revisit previous regions to discover optional paths, secrets, and information.

### World Structure

* The introduction follows a linear route through the first sector and ends with the first sector boss.
* Recovering the first fragment opens the wider interconnected world.
* The remaining five sector bosses may be confronted in different orders.
* Their fragment abilities may reveal secrets, shortcuts, advantages, alternative solutions, and optional subsectors. They are not exclusive requirements for reaching the other main sector bosses, preserving the freedom to confront those bosses in different orders.
* Recovering all six fragments unlocks the final region and final boss.

---

## 7. LIGHT, DARKNESS, AND SOLAR FRAGMENTS

The fragmented Sun has left the world in persistent darkness. Each recovered fragment permanently restores another stage of global illumination and produces visible changes in the world.

The Guardian may freely and instantly toggle their halo between **Light On** and **Light Off**. This action consumes no resource and may be performed during exploration, platforming, or combat.

Environmental elements are classified as:

* **Solar:** Exist or function only while the light is on.
* **Shadow:** Exist or function only while the light is off.
* **Neutral:** Remain available in both states.

Platforms, ladders, passages, doors, levers, symbols, and hidden information may depend on the current state. Platforms and paths change dynamically when the halo is toggled; if a supporting platform disappears, the player falls. Actions performed through mechanisms such as levers may create persistent changes. Enemies that are visible or vulnerable only in one state remain an experimental mechanic to be validated during prototyping.

### Progression Layers

* **Generic abilities:** Basic attack, parry, double jump, ground dash, air dash, wall slide, and wall jump are acquired through normal progression, checkpoints, special sources, or a skill tree rather than through Solar Fragments.
* **Fragment abilities:** Each of the six fragments may grant one distinct thematic ability.
* **World restoration:** Every fragment increases global light and changes the state or appearance of affected regions.
* **Solar Fulgor:** Recovering the first fragment unlocks a temporary empowered state. Later fragments strengthen or expand it. Its definitive behavior has not yet been selected.

### Solar Fulgor — Optional Designs (Not Confirmed)

**Optional Candidate A — Solar Fury**

* The Guardian becomes temporarily invulnerable to enemy damage.
* Attacks become considerably faster, heavier, and more aggressive, with increased damage and posture-breaking power.
* The mode represents a brief release of overwhelming solar energy and should feel immediately powerful and accessible.
* Whether lethal environmental hazards bypass its invulnerability remains to be defined.

**Optional Candidate B — Dawn Ascendance**

* The Guardian does not gain complete invulnerability; instead, the surrounding world slows while the Guardian retains normal speed.
* Solar and Shadow environmental elements coexist temporarily, creating new opportunities during platforming and combat.
* Attacks generate solar echoes, movement abilities recover more quickly, and successful parries extend the transformation's remaining duration.
* Taking damage significantly reduces the remaining duration, rewarding precise and aggressive play.

**Shared Provisional Rules**

* Activation is player-controlled once the Solar Fulgor meter meets its activation requirement.
* The first Solar Fragment unlocks the system. Subsequent fragments may improve its duration, power, meter efficiency, or moveset.
* Both candidates require a distinct solar aura, strong audiovisual feedback, and an intensified attack presentation.
* Charge sources, activation cost, duration, numerical bonuses, frequency of use, and boss-specific restrictions remain to be determined through prototyping.
* Both designs are optional and provisional. Neither is part of the confirmed design until playtesting determines which better supports the game's combat identity.

---

## 8. FIRST MINIMUM VIABLE PRODUCT (MVP)

The first MVP covers the complete playable route from the Guardian's awakening to defeating the first sector boss and recovering the first Solar Fragment. It should validate movement, combat, light/dark switching, checkpoints, environmental interactions, a basic enemy set, a complete sector, a boss encounter, fragment recovery, global-light restoration, and the first version of Solar Fulgor.

### Confirmed MVP Boundary

* **Playable World:** Only Sector 1, the Corrupted Jungle, including its linear route and conclusion.
* **Boss Content:** La Patasola is the only sector boss included and fought in the first MVP.
* **Enemy Content:** The four approved Sector 1 enemy archetypes form the target roster.
* **Weapon Content:** The Solar Sword is the only usable weapon. The optional irreversible boss-weapon system is excluded.
* **Progression Content:** Only the basic three-branch skill tree, initial movement abilities, initial health/elixir progression, and first-fragment reward flow are included.
* **World Conclusion:** The MVP ends after the first fragment restores part of the jungle and reveals or opens the transition toward the wider world.
* **Explicitly Outside MVP:** Sectors 2–6, their bosses and enemies, the final region, the final boss, the complete weapon roster, and full-game balance/content production.
* Detailed design fields remain documented for every later sector, but they are placeholders and do not authorize production work during the first MVP.

### Confirmed MVP Macro Flow

This sequence defines progression and teaching goals without prescribing final room geometry. Exact spaces, routes, distances, enemy placement, secrets, shortcuts, and checkpoint locations belong to the later level-design phase.

1. The Guardian awakens and the player learns basic lateral movement and jumping.
2. The game introduces freely toggling the Light Halo between Light On and Light Off.
3. The Guardian obtains the Solar Sword.
4. The player completes the first combat encounter and learns the precision parry.
5. The player activates the first checkpoint and learns its recovery and respawn rules.
6. The Guardian obtains the Ground Roll.
7. A mixed traversal and combat challenge teaches the Ground Roll's stamina cost.
8. The Guardian obtains the Air Dash.
9. The game introduces Cenizas de Luz, map information purchases, and the basic skill tree.
10. A combined challenge tests movement, combat, parry, light/dark switching, and the four approved enemy archetypes.
11. The Guardian confronts La Patasola, the only boss in the first MVP.
12. The Guardian recovers the Fragment of Life, partially restores the jungle, unlocks the first Solar Fulgor state, and reaches the end of the MVP.

### Playtime and Solo-Developer Scope

* **Initial Full-Game Target:** The complete primary playthrough, covering six sector bosses and the final region, should take no more than approximately 60 minutes in the initial production scope.
* **First MVP Target:** The introductory Corrupted Jungle route and La Patasola encounter should target approximately 10–15 minutes for a first-time player.
* **Sector Budget:** Later sectors should be designed as compact gameplay modules. Their individual duration will be assigned during level design so the combined critical path remains within the one-hour limit.
* **Expansion Policy:** Additional rooms, optional challenges, secrets, narrative content, and longer playtime may be added only after the complete initial experience is functional and the solo-development workload remains sustainable.
* **Scope Priority:** Reusable systems and distinct, concentrated encounters take priority over large maps or repeated filler content.

### MVP Definition of Done

The first MVP is considered functionally complete when a player can begin a new game, reach and defeat La Patasola, recover the Fragment of Life, and reach the ending state of the build without developer intervention.

The following systems must function together throughout that playable flow:

* **Exploration:** Lateral movement, jumping, Ground Roll, Air Dash, wall interaction, double jump, room transitions, and basic discovery flow.
* **Light and Darkness:** Free halo toggling and functional Solar, Shadow, and Neutral environmental interactions.
* **Combat:** Solar Sword attacks, precision parry, enemy damage, posture interactions, player damage, and defeat of the four MVP enemy archetypes and La Patasola.
* **Resources:** Correct stamina expenditure and recovery, health loss and restoration, elixir consumption, and Cenizas de Luz acquisition and spending.
* **Inventory:** A minimal interface that displays collected key items, available consumables, and the recovered Solar Fragment.
* **Notes:** A minimal journal that stores and displays discovered narrative or tutorial entries.
* **Progression:** Basic three-branch skill tree, exploration upgrades, purchased upgrades, and fragment collection.
* **Checkpoints:** Activation, full health recovery, default elixir refill, saving progress, and respawning standard enemies after resting.
* **Death and Recovery:** Player death, checkpoint respawn, creation of the Ascua Caída, currency recovery, and permanent loss after a second death.
* **Map:** Recording or displaying explored space and purchasing additional information for previously visited areas.
* **User Interface:** Functional health, stamina, elixir, currency, halo-state, inventory, notes, map, and interaction feedback.
* **Persistence:** Required checkpoint, inventory, note, currency, upgrade, fragment, and boss-defeat state remains correct after closing and reopening the game.
* **MVP Completion:** Recovering the Fragment of Life restores part of the jungle, unlocks the first Solar Fulgor state, and presents a clear end-of-MVP state.

Final art, complete narrative presentation, full accessibility options, content for later sectors, and production-level audiovisual polish are not required for functional MVP completion unless separately prioritized.

### Sector 1 — Corrupted Jungle

* **Role:** Linear introductory sector and the main content of the first MVP.
* **Theme:** A pre-Columbian-inspired jungle consumed by darkness and corruption.
* **Enemies:** Creatures inspired by regional mythology and folklore.
* **Sector Boss:** La Patasola.
* **Solar Fragment:** Fragment of Life.
* **Restoration Effect:** Defeating La Patasola begins restoring the jungle's light, vegetation, and vitality, and opens access to the wider world.
* **Optional Candidate A — Regrowth (not confirmed):** Causes roots, vines, and plants to grow temporarily. It may create platforms or traversal points during exploration and restrain enemies during combat.
* **Optional Candidate B — Solar Sap (not confirmed):** Successful attacks and parries accumulate vital energy that may be consumed to restore a limited amount of health or extend Solar Fulgor. Outside combat, it may purify small areas of corrupted vegetation.
* **Decision Status:** Both abilities are optional, provisional alternatives and are not part of the confirmed design. Their effect on healing balance, exploration, and combat must be validated through prototyping.
* **Detailed Design Pending:** Narrative role, cause of corruption, sector route, environmental rules, La Patasola's motivation, boss arena, encounter phases, attacks, parry opportunities, light/dark interactions, restoration sequence, and final rewards.

#### Confirmed MVP Enemy Roster

* **Corrupted Rootwalker:** Basic close-range enemy with slow, clearly telegraphed attacks. Introduces standard attacks, evasion, and precision parries.
* **Spore Spitter:** Stationary corrupted plant that fires poisonous projectiles and temporarily closes to defend itself after attacking. Encourages use of the Ground Roll, Air Dash, and platform positioning.
* **Shadow Stalker:** Fast creature whose silhouette remains visible in the light but only becomes fully material and vulnerable in darkness. Validates enemy interactions with the Light Halo system.
* **Vine Guardian:** Armored elite enemy that resists ordinary attacks and requires heavy attacks or precision parries to break its posture. Prepares the player for the La Patasola encounter.

Enemy health, damage, posture, attack timing, encounter composition, visual design, and final localized names remain intentionally pending until combat prototyping.

### Sector 2 — Flooded Temples and Ruins

* **Theme:** Ancient temples and ruins partially consumed by water.
* **Sector Boss:** El Mohán.
* **Solar Fragment:** Fragment of Memory.
* **Optional Candidate A — Echo of the Past (not confirmed):** Temporarily reveals and reconstructs the former state of ruined bridges, structures, or mechanisms. In combat, it may create an echo that repeats the Guardian's most recent attack at reduced power.
* **Optional Candidate B — Memory Anchor (not confirmed):** Records the Guardian's current position and allows them to return to it within a short time window. It can support timed puzzles, recover missed jumps, and misdirect enemies, but does not reverse damage or restore health.
* **Decision Status:** Both abilities are optional, provisional alternatives and are not part of the confirmed design. The final ability, or a possible combination of their concepts, must be validated through prototyping.
* **Detailed Design Pending:** Narrative role, sector route, environmental rules, enemy roster, boss motivation, arena, encounter phases, attacks, parry opportunities, light/dark interactions, restoration sequence, and weapon reward.

### Sector 3 — Mountains of Mist and Wind

* **Theme:** High mountains dominated by dense fog and powerful winds.
* **Sector Boss:** El Silbón.
* **Solar Fragment:** Fragment of Truth.
* **Optional Candidate A — Seal of Truth (not confirmed):** Temporarily marks an object or enemy and fixes it in its authentic form. A marked platform may remain materialized when the halo state changes. A marked enemy cannot hide, transform, or create false duplicates and may receive increased posture damage.
* **Optional Candidate B — Revealing Breath (not confirmed):** Releases a solar gust that temporarily clears fog, reveals concealed objects, and activates wind mechanisms. In combat, it may push enemies, redirect projectiles, and remove camouflage or false duplicates.
* **Decision Status:** Both abilities are optional, provisional alternatives and are not part of the confirmed design. Their interaction with level geometry, enemy states, and light/dark switching must be validated through prototyping.
* **Detailed Design Pending:** Narrative role, sector route, environmental rules, enemy roster, boss motivation, arena, encounter phases, attacks, parry opportunities, light/dark interactions, restoration sequence, and weapon reward.

### Sector 4 — Village of Eternal Rain

* **Theme:** An abandoned or afflicted settlement trapped beneath unending rain.
* **Sector Boss:** La Llorona.
* **Solar Fragment:** Fragment of Hope.
* **Optional Candidate A — Beacon of Hope (not confirmed):** Places a temporary source of light that creates a safe area, reduces harmful effects of the rain, and reveals nearby spirits or paths. In combat, it may weaken spectral creatures and improve stamina recovery within its radius.
* **Optional Candidate B — Second Dawn (not confirmed):** When sufficient energy is available, prevents death and returns the Guardian to the most recent safe position with a limited amount of health, releasing a burst of light. It may only activate once per rest.
* **Decision Status:** Both abilities are optional, provisional alternatives and are not part of the confirmed design. Their impact on difficulty, checkpoints, and resource balance must be validated through prototyping.
* **Detailed Design Pending:** Narrative role, sector route, environmental rules, enemy roster, boss motivation, arena, encounter phases, attacks, parry opportunities, light/dark interactions, restoration sequence, and weapon reward.

### Sector 5 — Shifting Ancestral Forest

* **Theme:** An ancient living forest whose paths and environment continually change.
* **Sector Boss:** La Madremonte.
* **Solar Fragment:** Fragment of Balance.
* **Optional Candidate A — Convergence (not confirmed):** Temporarily causes Solar and Shadow elements to coexist, allowing the Guardian to combine platforms and environmental objects from both states and affect enemies aligned with either one. Its short duration must prevent it from bypassing entire puzzles.
* **Optional Candidate B — Return to Balance (not confirmed):** Perfect parries store part of the incoming force. The Guardian may release it as a shockwave that pushes enemies, damages posture, and moves roots, rocks, or counterweight mechanisms.
* **Decision Status:** Both abilities are optional, provisional alternatives and are not part of the confirmed design. Their duration, stored-force limits, and effect on puzzle difficulty must be validated through prototyping.
* **Detailed Design Pending:** Narrative role, sector route, environmental rules, enemy roster, boss motivation, arena, encounter phases, attacks, parry opportunities, light/dark interactions, restoration sequence, and weapon reward.

### Sector 6 — Swamps and Submerged Ruins

* **Theme:** Hostile wetlands containing the remains of partially submerged structures.
* **Sector Boss:** El Hombre Caimán.
* **Solar Fragment:** Fragment of Identity.
* **Optional Candidate A — Caiman Skin (not confirmed):** Grants a temporary amphibious and resistant form that can swim underwater, cross strong currents, resist poison, and perform heavy attacks or charges. If selected, it also unlocks previously inaccessible aquatic spaces and optional subsectors across the world, encouraging backtracking without blocking the main routes to other sector bosses.
* **Optional Candidate B — Reflected Essence (not confirmed):** Temporarily copies one predetermined trait from a targeted enemy family, such as poison resistance or a ranged attack. The number of compatible traits must remain limited to control production and balance scope.
* **Decision Status:** Both abilities are optional, provisional alternatives and are not part of the confirmed design. Caiman Skin is the clearer production choice; Reflected Essence is thematically broader but substantially more complex to implement and balance.
* **Detailed Design Pending:** Narrative role, sector route, environmental rules, enemy roster, boss motivation, arena, encounter phases, attacks, parry opportunities, light/dark interactions, restoration sequence, and weapon reward.

### Final Region — To Be Defined

* **Access:** Unlocked after recovering all six Solar Fragments.
* **Purpose:** Final traversal challenges and confrontation with the entity that fragmented the Sun.
* **Provisional Theme:** A monumental ceremonial temple suspended above the world, associated with the worship of the Sun.
* **Name and inhabitants:** To be defined.
* **Detailed Design Pending:** Final route, environmental rules, enemy roster, arena, encounter phases, attacks, light/dark interactions, use of the six fragment abilities, ending sequence, and narrative resolution.

#### Final Boss Alternatives — Open Design Decision

The final boss concept is not definitive. The following alternatives must remain marked as subject to change until narrative development and visual exploration determine the strongest direction.

**Option A — The Corrupted Guardian (current primary concept)**

An original guardian who rejected the population's worship of the Sun and the belief that all responsibility for the world's fate should rest upon it. Believing that this dependence deprived the people of their autonomy, the guardian stole and fragmented the Sun in an attempt to free them. The prolonged influence of darkness, their methods, or the power they seized ultimately corrupted them. This motivation should present the antagonist as ideologically driven rather than purely evil.

**Option B — The Herald of the Eclipse (folklore-inspired alternative)**

An original lunar guardian inspired by the Muisca duality between Sué, the Sun, and Chía, the Moon. The Herald believes that excessive devotion to the Sun broke the ancient balance: daylight came to represent absolute authority, while night, dreams, memory, and the Moon were feared or forgotten. During a forbidden ceremony, the guardian invoked the power of an eclipse and fragmented the Sun, intending to force the world to remember both halves of the cycle. The eclipse's power eventually consumed the guardian, and the prolonged night became a corruption they could no longer control.

This version preserves a strong connection to regional cosmology without directly portraying a traditional deity as evil. Its final encounter would take place in the suspended ceremonial temple, which alternates between solar and lunar states. The boss and arena could react to the Guardian's light toggle and require mastery of the six recovered fragment powers.

**Provisional Name:** The Herald of the Eclipse. The final name, cultural references, visual design, and narrative details require further research and cultural review before production.

---

## 9. MVP OPEN DECISIONS

The following information is still required before the first MVP can be considered fully defined. Numerical tuning may remain open until playtesting, but each system needs a clear functional rule.

### Required Before MVP Definition Is Closed

#### Solar Sword Moveset

**Status: PENDING DEFINITION — To be addressed in a future design session.**

The standard combo, charged attack, aerial attack, downward attack, attack-cancellation rules, and post-parry counterattack must be defined.

#### Damage and Environmental Hazards

**Status: PENDING DEFINITION — To be addressed in a future design session.**

Fall behavior, bottomless pits, poison, environmental damage, temporary invulnerability after taking damage, and knockback must be defined.

#### Healing Rules

**Status: PENDING DEFINITION — To be addressed in a future design session.**

Initial elixir capacity, healing duration, amount restored, interruption behavior, and consumption timing must be defined.

#### Remaining Stamina Rules

**Status: PENDING DEFINITION — To be addressed in a future design session.**

Every stamina-consuming action and the general regeneration delay must be confirmed. Exact numerical costs may wait for testing.

#### Solar Fulgor in the MVP

**Status: PENDING DEFINITION — To be addressed in a future design session.**

One candidate must be selected, its meter-generation rules must be defined, and the MVP must establish whether the player tests it after recovering the Fragment of Life or only sees it unlock at the ending.

#### Inventory Limits

**Status: PENDING DEFINITION — To be addressed in a future design session.**

Slot count, stack limits, item categories, and full-inventory behavior must be defined.

#### MVP Consumable Effects

**Status: PENDING DEFINITION — To be addressed in a future design session.**

The identities and effects of the two unidentified consumable types must be defined.

#### Map Flow

**Status: PENDING DEFINITION — To be addressed in a future design session.**

The rules for recording explored space, purchasing map information, and revealing visited or suspicious areas must be defined.

#### Skill Tree Access

**Status: PENDING DEFINITION — To be addressed in a future design session.**

The access location, purchase flow, confirmation behavior, and any refund rules must be defined.

#### Save Structure

**Status: PENDING DEFINITION — To be addressed in a future design session.**

Autosave triggers, save-slot count, New Game/Continue behavior, and handling of missing or invalid save data must be defined.

#### Pause and Menu Behavior

**Status: PENDING DEFINITION — To be addressed in a future design session.**

The game must define whether gameplay pauses while the inventory, notes, map, skill tree, and settings screens are open.

#### MVP End State

**Status: PENDING DEFINITION — To be addressed in a future design session.**

The final screen, playable moment, or transition shown after recovering the Fragment of Life must be defined.

#### Basic Technical Target

**Status: PENDING DEFINITION — To be addressed in a future design session.**

Target resolution, supported aspect ratios, frame-rate target, and minimum acceptable performance on the development PC must be defined.

### Requires Later Design Work but Is Already Reserved

* Corrupted Jungle map blockout, room layout, shortcuts, checkpoints, secrets, and encounter placement.
* La Patasola's arena, phases, attacks, parry windows, difficulty, and restoration sequence.
* Exact statistics, prices, drop rates, timings, and progression locations.
* Final tutorial wording, note text, item descriptions, and narrative dialogue.

---

## 10. LEVEL DESIGN — PENDING DEFINITION

**Status: PENDING DEFINITION — To be addressed in a future design session.**

This section is intentionally reserved for the spatial design of the first MVP and later sectors. It will document maps, rooms, critical paths, optional paths, ability gates, light/dark puzzles, shortcuts, checkpoints, keys, mechanisms, secrets, enemy encounters, boss arenas, and expected traversal time.

No final level layout has been approved.

---

## 11. ART AND VISUAL DIRECTION — PENDING DEFINITION

**Status: PENDING DEFINITION — To be addressed in a future design session.**

This section is intentionally reserved for the final 2D or 2.5D approach, character proportions, environment style, color scripts, Solar/Shadow visual language, corruption language, lighting rules, UI art, visual references, asset specifications, and production constraints suitable for a solo developer.

No final graphic style has been approved.

---

## 12. ANIMATION DIRECTION — PENDING DEFINITION

**Status: PENDING DEFINITION — To be addressed in a future design session.**

This section is intentionally reserved for animation technique, frame or sampling targets, state lists, combat readability, anticipation and recovery timing, hit reactions, movement transitions, enemy animation requirements, boss animation requirements, and the scope of cinematics.

No final animation style or production pipeline has been approved.

---

## 13. AUDIO AND MUSIC — PENDING DEFINITION

**Status: PENDING DEFINITION — To be addressed in a future design session.**

This section is intentionally reserved for musical direction, sector themes, adaptive Light/Dark music behavior, combat layers, boss music, ambience, sound-effect language, UI feedback, voice requirements, mixing priorities, and implementation scope.

No final music or audio direction has been approved.

---

## 14. NARRATIVE CONTENT AND WRITING — PENDING DEFINITION

**Status: PENDING DEFINITION — To be addressed in a future design session.**

This section is intentionally reserved for the opening text or scene, character voice, tutorial wording, collectible notes, item descriptions, boss introductions, fragment-recovery text, final dialogue, ending structure, and cultural review.

The premise and sector concepts are established, but final scripts have not been written or approved.

---

## 15. ACCESSIBILITY, LOCALIZATION, AND SETTINGS — PENDING DEFINITION

**Status: PENDING DEFINITION — To be addressed in a future design session.**

This section is intentionally reserved for input rebinding, text size, contrast options, brightness calibration, screen shake controls, flashing-light controls, subtitles, language support, difficulty assists, audio sliders, display settings, and gamepad support.

Only PC Keyboard and Mouse support is required for the first functional MVP; the final accessibility and localization scope has not been approved.
