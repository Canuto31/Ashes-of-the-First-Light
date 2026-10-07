# Estado del refactor de código

Fecha de última validación: 2026-10-06

La evaluación técnica y la hoja de ruta actualizadas se encuentran en
[`MEJORAS_PROFESIONALES_SCRIPTS.md`](MEJORAS_PROFESIONALES_SCRIPTS.md).

## Alcance y reglas aplicadas

- Se revisaron los 54 scripts propios ubicados en `Assets/MyAssets/Scripts`.
- Se conservaron los nombres y tipos de todos los campos serializados y públicos que Unity muestra en el Inspector.
- Se conservaron nombres de clases, archivos, métodos públicos, parámetros del Animator y archivos `.meta`.
- No se modificaron escenas, prefabs, ScriptableObjects ni configuraciones de Input Actions.
- Se excluyeron el código generado (`Assets/InputSystem_Actions.cs`) y los paquetes de terceros.

## Clases refactorizadas

### Jugador

- `PlayerController`
- `PlayerInputHandler`
- `PlayerHealth`
- `PlayerStamina`
- `PlayerLamp`
- `PlayerInventory`
- `PlayerStateMachine`
- `PlayerAnimationEvents`

### Estado y gestores centrales

- `GameStateManager`
- `CheckpointManager`
- `TutorialManager`
- `TutorialCodexManager`
- `UIScreenManager`
- `Billboard`

### Interacción y entorno

- `InteractableTrigger`
- `CheckpointSource`
- `Lever`
- `DoorController`
- `LightReactiveObject`
- `CameraDirector`
- `CameraTrigger`

### Inventario, notas y tutoriales

- `PickupItem`
- `PickupNote`
- `NotesManager`
- `NotesPageController`
- `NotesUIManager`
- `NoteOptionUI`
- `TutorialPageController`
- `TutorialOptionUI`
- `InventoryPageController`
- `InventoryCategoryOptionUI`
- `InventoryItemOptionUI`

### Menús y HUD

- `BookMenuManager`
- `SettingsPageController`
- `GameMenuController`
- `PauseMenuActions`
- `TutorialUIManager`
- `UIManager`
- `BaseHUDModule`
- `InteractionUIManager`
- `UI_Interaction`
- `LifeHUDController`
- `StaminaHUDController`
- `SolarHUDController`
- `QuickSlotHUDController`
- `QuickSlotUI`
- `UISelectableOption`

## Clases revisadas sin cambios necesarios

Estas clases son definiciones pequeñas de datos, contratos o enumeraciones y ya estaban claras:

- `IInteractable`
- `InventoryCategory`
- `InventoryEntry`
- `InventoryItem`
- `NoteData`
- `TutorialData`
- `QuickSlotState`

## Mejoras relevantes

- Se evitó registrar varias veces los callbacks del Input System al reactivar al jugador.
- Se inicializó correctamente la colección de `TutorialCodexManager`.
- Se sustituyeron cadenas repetidas del Animator por hashes conservando los mismos parámetros.
- Se simplificaron condicionales, navegación circular, control de estados y corrutinas.
- Se agregaron validaciones defensivas para referencias opcionales, datos vacíos y cantidades inválidas.
- Se corrigió `UISelectableOption`: el estado seleccionado ahora cambia `fontSize` y no intenta usar el tamaño como valor de transparencia.
- Se centralizó el cierre del libro mediante `BookMenuManager.CloseBook()` en vez de `SendMessage`.
- Se retiraron imports, comentarios corruptos y código redundante.

## Validación realizada

- Comparación automatizada contra Git: las firmas de los campos serializados y públicos permanecen iguales.
- `git diff --check`: sin errores de espacios o parches inválidos.
- Compilación de `Ashes of the First Light.sln`: 0 errores y 0 advertencias.
- Solo se modificaron scripts propios y este documento; no se alteraron referencias de escenas o prefabs.

## Pendiente manual

No quedan clases propias pendientes de revisión en esta tanda. Falta abrir Unity, esperar la recompilación y ejecutar una prueba rápida en Play Mode de movimiento, dash, interacción, checkpoints, libro, notas, tutoriales, HUD, palanca/puerta y cambio de luz. El proyecto no contiene pruebas automatizadas para esos flujos.
