# Flujo completo de scripts

Fuente principal: `Assets/MyAssets/Scripts/todos_los_scripts.txt`. Para identificar las teclas físicas también se contrastó `Assets/InputSystem_Actions.inputactions`.

## 1. Arquitectura general

```mermaid
flowchart LR
    HW[Teclado / ratón] --> IA[InputSystem_Actions]
    IA --> PIH[PlayerInputHandler]
    PIH -->|flags de un frame| PC[PlayerController]
    PIH --> PL[PlayerLamp]
    PIH --> IT[InteractableTrigger]
    PIH --> BMM[BookMenuManager]
    PIH --> TUI[TutorialUIManager]
    PIH --> NPC[NotesPageController]
    PIH --> TPC[TutorialPageController]
    PIH --> SPC[SettingsPageController]
    PIH --> PH[PlayerHealth]
    PIH --> PS[PlayerStamina]
    PIH --> SHUD[SolarHUDController]
    PIH --> QHUD[QuickSlotHUDController]

    GSM[GameStateManager] -->|Playing habilita| PC
    GSM -->|Playing habilita| IT
    GSM -->|BookMenu habilita| BMM
    GSM -->|BookMenu habilita| NPC
    GSM -->|BookMenu habilita| TPC
    GSM -->|BookMenu habilita| SPC
    GSM -->|Tutorial habilita| TUI

    PC --> RB[Rigidbody2D]
    PC --> AN[Animator]
    PC --> PSM[PlayerStateMachine]
    PC --> PS
    PS -->|OnStaminaChanged| STUI[StaminaHUDController]
    PH -->|OnHealthChanged / OnDamageTaken| LHUI[LifeHUDController]
    PH -->|OnDeath| DEAD[Suscriptores externos; ninguno en estos scripts]

    IT --> II[IInteractable]
    II --> CP[CheckpointSource]
    II --> LEV[Lever]
    II --> ITEM[PickupItem]
    II --> NOTE[PickupNote]
    IT --> IUM[InteractionUIManager]

    CP --> CPM[CheckpointManager]
    ITEM --> INV[PlayerInventory]
    NOTE --> NM[NotesManager]
    NOTE --> BMM
    NOTE --> TUI
    LEV --> CAM[CameraDirector]
    LEV --> DOOR[DoorController]

    BMM --> NPC
    BMM --> TPC
    BMM --> IPC[InventoryPageController]
    NPC --> NM
    TPC --> TM[TutorialManager]
    IPC --> INV

    PL --> LRO[LightReactiveObject]
    LRO --> TARGETS[GameObjects objetivo activos/inactivos]
```

## 2. Entrada: de una tecla al consumidor

`PlayerInputHandler` crea y habilita el mapa `Player`. Las acciones continuas (`Move`, `Sprint`, `JumpHeld`) conservan estado; las demás levantan un booleano que se limpia en `LateUpdate`, después de que los demás `Update` hayan podido leerlo.

```mermaid
flowchart TD
    PRESS[Se pulsa una tecla] --> ACT[Input System dispara performed]
    ACT --> FLAG[PlayerInputHandler actualiza valor o flag]
    FLAG --> UPDATES[Los Update de consumidores leen el mismo dato]
    UPDATES --> LATE[PlayerInputHandler.LateUpdate]
    LATE --> RESET[Resetea los flags de un solo frame]

    HELD[Tecla mantenida] --> CONT{Tipo de acción}
    CONT -->|Move| VECTOR[MoveInput conserva Vector2 hasta canceled]
    CONT -->|Sprint| SPRINT[SprintHeld=true hasta canceled]
    CONT -->|Jump| JHELD[JumpHeld=true; JumpPressed solo el primer frame]
```

```mermaid
flowchart LR
    WASD[WASD / flechas] --> MOVE[Move]
    MOVE --> PCMOVE[PlayerController: movimiento, giro, animación]
    MOVE --> STAM[PlayerStamina: drena al correr]

    SPACE[Espacio] --> JUMP[JumpPressed + JumpHeld]
    JUMP --> JFLOW[Salto base / coyote / doble salto / wall jump / corte de salto]

    ALT[Alt] --> DASH[DashPressed]
    DASH --> DCHECK{Stamina >= coste}
    DCHECK -->|Sí| DRAIN[PlayerStamina.DrainStamina]
    DRAIN --> VEL[Impulso Rigidbody2D + trigger Roll]

    SHIFT[Shift izquierdo] --> SPRINT[SprintHeld]
    SPRINT --> SPEED[Velocidad x sprintMultiplier]
    SPRINT --> SDRAIN[Drenaje continuo de stamina]

    MOUSE[Click izquierdo] --> ATTACK[AttackPressed]
    ATTACK --> ATRIG[Animator trigger Attack1]
    ATRIG --> AE[Evento de animación]
    AE --> PAE[PlayerAnimationEvents.EndAttack]
    PAE --> END[PlayerController.EndAttack]

    I[I] --> TL[ToggleLanternPressed]
    TL --> LAMP[PlayerLamp.ToggleLight]
    LAMP --> LIGHTOBJ[LightReactiveObject recalcula objetivos]
```

## 3. Flujo completo de interacción con E

```mermaid
flowchart TD
    ENTER[Player entra en trigger 2D] --> IT[InteractableTrigger.OnTriggerEnter2D]
    IT --> TAG{Tag == Player y existe IInteractable}
    TAG -->|Sí| STORE[Guarda PlayerInputHandler]
    STORE --> PROMPT[InteractionUIManager.Show sobre el objeto]

    E[Pulsa E] --> PIH[InteractPressed=true]
    PIH --> ACTIVE{GameState == Playing?}
    ACTIVE -->|Sí y dentro del trigger| CALL[IInteractable.Interact]
    CALL --> KIND{Implementación concreta}

    KIND --> CHECK[CheckpointSource]
    CHECK --> SAVE[CheckpointManager.SetCheckpoint]
    SAVE --> SAVEMSG[UI_Interaction: Checkpoint saved]

    KIND --> PICK[PickupItem]
    PICK --> ADD[PlayerInventory.AddItem]
    ADD --> ITEMMSG[Mensaje temporal]
    ITEMMSG --> DESTROY1[Destroy objeto]

    KIND --> PN[PickupNote]
    PN --> ADDNOTE[NotesManager.AddNote]
    ADDNOTE --> QUEUE[BookMenuManager.QueueContextPage Notes por 2 s]
    QUEUE --> FIRST{Tutorial de lectura no visto?}
    FIRST -->|Sí| SHOWT[TutorialUIManager.ShowTutorial]
    SHOWT --> GST[GameState = Tutorial]
    GST --> CLOSE[Otra E, tras un frame, cierra tutorial]
    CLOSE --> PLAY[GameState = Playing]
    PLAY --> NOTEMSG[Mensaje: TAB para leer]
    FIRST -->|No| NOTEMSG
    NOTEMSG --> DESTROY2[Destroy nota del mundo]

    KIND --> LEVER[Lever]
    LEVER --> REQ{Tiene InventoryItem requerido?}
    REQ -->|No| LOCKED[InteractionUIManager: mensaje de bloqueo]
    REQ -->|Sí| SEQ[LeverSequence]
    SEQ --> FOCUS[CameraDirector.FocusOn cámara objetivo]
    FOCUS --> WAIT[Espera 1 s]
    WAIT --> OPEN[DoorController.Open]
    OPEN --> DELAY[Espera openDelay]
    DELAY --> ANIM[Interpola posición o rotación]

    EXIT[Player sale del trigger] --> CLEAR[InteractionUIManager.Clear]
```

Nota: la misma `E` también activa `NextBookPage`; si el libro está abierto, `BookMenuManager` cambia de sección. Además `PlayerLamp.Update` usa `InteractPressed` para aumentar temporalmente el radio de luz en 1, sin comprobar el estado de juego. Por eso una interacción también aumenta la lámpara mientras no haya llegado al máximo.

## 4. Libro, notas, tutoriales e inventario

```mermaid
flowchart TD
    P[P] --> TOGGLE[ToggleMenuPressed]
    TOGGLE --> CONSUME[BookMenuManager.ConsumeToggleMenu]
    CONSUME --> STATE{Estado actual}
    STATE -->|Playing| OPEN[OpenBookAtPage Notes]
    STATE -->|BookMenu| CLOSE[CloseBook]
    OPEN --> BM[GameState = BookMenu]
    BM --> ROOT[Activa bookRoot]
    ROOT --> REFRESH[ShowPage y refresca controlador]
    CLOSE --> GP[GameState = Playing]
    CLOSE --> HIDE[Desactiva bookRoot]

    TAB[TAB, hasta 2 s tras recoger nota] --> OPENITEM[OpenItemPressed]
    OPENITEM --> PENDING{Hay página contextual pendiente?}
    PENDING -->|Sí| NOTEOPEN[Abre libro en Notes]
    NOTEOPEN --> LAST[NotesPageController.FocusLastCollectedNote]

    EKEY[E] --> NEXTBOOK[Siguiente sección]
    QKEY[Q] --> PREVBOOK[Sección anterior]
    NEXTBOOK --> CYCLE[Notes -> Tutorials -> Inventory -> Settings -> Notes]
    PREVBOOK --> CYCLE

    W[W] --> UP[Selecciona opción anterior]
    S[S] --> DOWN[Selecciona opción siguiente]
    UP --> PAGE{Página activa}
    DOWN --> PAGE
    PAGE -->|Notes| NSELECT[Selecciona NoteData y muestra página 1]
    PAGE -->|Tutorials| TSELECT[Selecciona TutorialData]
    PAGE -->|Settings| SSELECT[Selecciona acción]

    A[A] --> PREVCONTENT[Página anterior dentro de la nota]
    D[D] --> NEXTCONTENT[Página siguiente dentro de la nota]

    ENTER[Enter] --> CONFIRM[SettingsPageController.Confirm]
    CONFIRM --> OPT{Opción}
    OPT -->|0| RESUME[Cierra libro]
    OPT -->|1| RETURN[CheckpointManager.ReturnToCheckpoint + cierra libro]
    OPT -->|2| EXIT[Actualmente solo Debug.Log]
```

`InventoryPageController` actualmente solo crea y selecciona las categorías visuales; no obtiene ni dibuja todavía los `InventoryEntry` de `PlayerInventory`.

## 5. Estados globales y bloqueo de sistemas

```mermaid
stateDiagram-v2
    [*] --> Playing: GameStateManager.Start
    Playing --> BookMenu: P / abrir nota contextual
    BookMenu --> Playing: P / Resume / ReturnToCheckpoint
    Playing --> Tutorial: PickupNote -> TutorialUIManager
    Tutorial --> Playing: E después del retardo de 1 frame

    state Playing {
        [*] --> Gameplay
        Gameplay: PlayerController procesa movimiento/combate
        Gameplay: InteractableTrigger permite Interact
        Gameplay: prompts de interacción visibles
    }

    state BookMenu {
        [*] --> Notes
        Notes --> Tutorials: E
        Tutorials --> Inventory: E
        Inventory --> Settings: E
        Settings --> Notes: E
        Notes --> Settings: Q
    }
```

`PlayerController.FixedUpdate` pone la velocidad a cero cuando no está en `Playing`. Sin embargo, `PlayerLamp`, `PlayerHealth`, `PlayerStamina`, `SolarHUDController` y `QuickSlotHUDController` no consultan `GameStateManager`, por lo que siguen procesando sus entradas o actualizaciones también durante libro/tutorial.

## 6. Vida, stamina y HUD

```mermaid
flowchart LR
    DOWN[ Flecha abajo ] --> DMG[PlayerHealth.TakeDamage 10]
    DMG --> HC[OnHealthChanged]
    DMG --> DT[OnDamageTaken]
    HC --> LIFE[LifeHUDController actualiza barra suavemente]
    DT --> FLASH[Flash de daño y desvanecimiento]
    DMG --> ZERO{Vida == 0}
    ZERO -->|Sí| DEATH[OnDeath; sin consumidor local]

    UP[ Flecha arriba ] --> HEAL[PlayerHealth.Heal 10]
    HEAL --> HC

    RUN[Move != 0 + Shift + no exhausto] --> DRAIN[DrainStamina por segundo]
    DASH[Dash válido] --> COST[DrainStamina coste fijo]
    DRAIN --> SC[OnStaminaChanged]
    COST --> SC
    IDLE[No corriendo] --> REGEN[RegenerateStamina]
    REGEN --> SC
    SC --> STBAR[StaminaHUDController suaviza barra]
    DRAIN --> EMPTY{Llega a 0}
    EMPTY --> EXH[Exhausted: no sprint]
    REGEN --> THRESH{Recupera 20%}
    THRESH --> READY[Permite sprint]
```

Controles de depuración adicionales: `'` restaura 0.5 de energía solar, `;` consume 0.5, `.` desbloquea un quick slot y `,` bloquea uno.

## 7. Cámara, puerta y objetos reactivos a luz

```mermaid
flowchart TD
    LEV[LeverSequence] --> FOCUS[CameraDirector.FocusOn]
    FOCUS --> STOP{¿Había una cinemática?}
    STOP -->|Sí| RESTORE[Detiene corutina y restaura cámara del player]
    STOP --> TARGET[Cámara objetivo priority=20]
    TARGET --> PLAYER[Player camera priority=10]
    PLAYER --> DUR[Espera cameraDuration]
    DUR --> RESTORE2[Objetivo=10; player=20]

    LAMP[PlayerLamp] --> ON{Luz encendida?}
    ON --> RADIUS[Posición + radio actual]
    RADIUS --> LRO[LightReactiveObject.Update]
    LRO --> DIST{Algún target dentro del radio?}
    DIST --> MODE{activeInLight}
    MODE --> SET[Activa o desactiva todos los targets]
```

## 8. Relaciones de datos

```mermaid
classDiagram
    class IInteractable {
      <<interface>>
      +Interact()
      +GetInteractionText() string
    }
    IInteractable <|.. CheckpointSource
    IInteractable <|.. Lever
    IInteractable <|.. PickupItem
    IInteractable <|.. PickupNote

    class InventoryItem {
      itemId
      itemName
      icon
      category
      description
    }
    class InventoryEntry {
      Item
      Quantity
      Identified
    }
    PlayerInventory "1" o-- "many" InventoryEntry
    InventoryEntry --> InventoryItem
    Lever --> InventoryItem : requisito
    PickupItem --> InventoryItem

    class NoteData {
      noteTitle
      pages[]
    }
    NotesManager "1" o-- "many" NoteData
    PickupNote --> NoteData
    NotesPageController --> NotesManager

    class TutorialData {
      tutorialId
      title
      popupMessage
      description
    }
    TutorialManager "1" o-- "many" TutorialData
    PickupNote --> TutorialData
    TutorialPageController --> TutorialManager
```

## 9. Hallazgos importantes del flujo actual

- `CameraTrigger.TryActivate()` nunca es llamado: el bloque automático está comentado y, cuando requiere entrada, `Update()` llama al `IInteractable` padre, no a `TryActivate()`. Por sí solo, este script no inicia su enfoque de cámara.
- Hay dos controladores para abrir/cerrar menú (`BookMenuManager` y `GameMenuController`) leyendo el mismo `ToggleMenuPressed`. `BookMenuManager` lo consume, pero el resultado depende del orden de `Update`; si `GameMenuController` corre primero puede cambiar el estado antes que el libro y dejar la UI desincronizada.
- `E` tiene tres consumidores: interacción, página siguiente del libro y aumento temporal del radio de lámpara. El estado limita algunos consumidores, pero no `PlayerLamp`.
- `W/S/A/D` sirven simultáneamente para movimiento y navegación del libro. El movimiento físico queda frenado fuera de `Playing`, pero el vector de entrada sigue activo.
- `TutorialCodexManager` y `TutorialManager` mantienen listas de tutoriales separadas. En este conjunto de scripts, las pantallas usan `TutorialManager`; `TutorialCodexManager` no tiene consumidores.
- `NotesUIManager` ofrece un segundo flujo para mostrar notas, pero no hay ninguna llamada a `OpenNote()` dentro de estos scripts. El flujo activo de notas parece ser `BookMenuManager` + `NotesPageController`.
- La muerte solo emite `PlayerHealth.OnDeath`; ningún script del archivo está suscrito, así que no hay respawn, pantalla de muerte ni cambio de estado implementado aquí.
- `CameraDirector.SetCamera()` tampoco tiene llamadas dentro de este conjunto.
- Las referencias `[SerializeField]`, jerarquías padre/hijo de GameObjects y eventos conectados desde el Inspector no pueden inferirse completamente desde el `.txt`; el diagrama refleja dependencias de código y búsquedas de componentes. Para mapear la jerarquía exacta habría que analizar escenas y prefabs.
