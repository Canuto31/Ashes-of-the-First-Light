# Ruta de lectura de los scripts

Fecha de revisión: 2026-10-06

## Objetivo

Esta guía define un orden de lectura para reconstruir mentalmente la lógica actual de **Ashes of the First Light**. El orden no sigue únicamente las carpetas: comienza por las reglas globales, continúa con la entrada y el jugador, y después recorre los sistemas que dependen de ellos.

La ruta está pensada para responder, en este orden:

1. ¿Cuándo está permitido jugar?
2. ¿Cómo se convierte una tecla en una intención?
3. ¿Cómo ejecuta esa intención el jugador?
4. ¿Cómo interactúa el jugador con el mundo?
5. ¿Dónde se almacenan objetos, notas y tutoriales?
6. ¿Cómo presentan esos datos el HUD y los menús?
7. ¿Qué sistemas auxiliares completan el ciclo?

## Cómo estudiar cada archivo

En cada script, leer siempre en este orden:

1. **Clase, interfaz o enum:** identificar qué responsabilidad representa.
2. **Campos serializados:** revisar qué debe existir en la escena o prefab.
3. **Referencias privadas:** identificar dependencias obtenidas en ejecución.
4. **`Awake`, `OnEnable` y `Start`:** reconstruir la inicialización.
5. **`Update`, `FixedUpdate` y `LateUpdate`:** localizar el punto de entrada recurrente.
6. **Métodos públicos:** identificar qué otros componentes pueden solicitarle.
7. **Eventos y callbacks:** identificar quién publica información y quién debería reaccionar.
8. **Inspector de Unity:** abrir el GameObject o prefab correspondiente y comprobar qué referencias están asignadas.

No conviene intentar memorizar cada línea. Para cada archivo, anotar únicamente:

- qué datos recibe;
- qué decisión toma;
- qué datos modifica;
- a qué otro componente llama;
- qué condición global puede bloquearlo.

---

## Etapa 0 — Contexto funcional y mapa general

### 0.1 Documentos previos

Leer primero:

1. [`GAMEPLAY_FLOW.md`](GAMEPLAY_FLOW.md): intención jugable y bucle general.
2. [`flujo_completo_scripts.md`](flujo_completo_scripts.md): relaciones visuales entre sistemas.
3. [`CODE_REFACTOR_STATUS.md`](CODE_REFACTOR_STATUS.md): qué se reorganizó sin alterar comportamiento.
4. [`MEJORAS_PROFESIONALES_SCRIPTS.md`](MEJORAS_PROFESIONALES_SCRIPTS.md): arquitectura actual frente a arquitectura objetivo.

Estos documentos evitan confundir una funcionalidad ya implementada con una propuesta futura.

### 0.2 Asset generado del Input System

Antes de los scripts propios, abrir `Assets/InputSystem_Actions.inputactions` en Unity. Revisar el Action Map `Player`, sus bindings y las acciones `Move`, `Jump`, `Dash`, `Sprint`, `Attack`, `Interact`, `ToggleLantern` y las acciones de UI.

`Assets/InputSystem_Actions.cs` es código generado. Consultarlo sólo para confirmar nombres; no estudiarlo línea por línea ni editarlo manualmente.

**Al terminar esta etapa debes poder explicar:** qué acciones existen y qué teclas o controles las activan.

---

## Etapa 1 — Reglas globales de ejecución

### 1.1 `GameStateManager`

Archivo: `Assets/MyAssets/Scripts/Core/GameState/GameStateManager.cs`

Es el primer script de código que se debe leer porque muchos sistemas preguntan si el estado es `Playing`, `Tutorial` o `BookMenu` antes de actuar.

Prestar atención a:

- el singleton `Instance`;
- `DontDestroyOnLoad`;
- el estado inicial asignado en `Start`;
- `SetState`, `GetState` e `IsPlaying`;
- la ausencia actual de un evento de cambio de estado.

### 1.2 Gestores de pantalla

Leer después:

1. `Assets/MyAssets/Scripts/Core/Managers/UIScreenManager.cs`
2. `Assets/MyAssets/Scripts/UI/Menus/GameMenuController.cs`
3. `Assets/MyAssets/Scripts/UI/Menus/PauseMenuActions.cs`

Aquí se ve cómo un cambio de estado habilita o bloquea gameplay y cómo las pantallas reaccionan al menú.

**Pregunta de control:** si el juego entra en `BookMenu`, ¿qué scripts dejan de procesar gameplay y cuáles empiezan a procesar navegación?

---

## Etapa 2 — De una tecla a una intención

### 2.1 `PlayerInputHandler`

Archivo: `Assets/MyAssets/Scripts/Player/Input/PlayerInputHandler.cs`

Debe leerse antes de `PlayerController`. Es el adaptador entre el Input System generado y todos los consumidores del proyecto.

Seguir este recorrido:

1. `Awake` crea `InputSystem_Actions`.
2. `RegisterInputCallbacks` conecta las acciones.
3. Las acciones continuas conservan valores (`MoveInput`, `SprintHeld`, `JumpHeld`).
4. Las acciones puntuales levantan flags (`JumpPressed`, `DashPressed`, etc.).
5. Los consumidores leen esos flags durante `Update`.
6. `LateUpdate` ejecuta `ResetFrameInput` y limpia los pulsos.
7. `ConsumeToggleMenu` muestra el patrón de consumo explícito para evitar dos lectores.

Aspecto crítico: varios scripts leen el mismo handler. Comprobar qué flags sólo se consultan y cuáles se consumen.

**Pregunta de control:** ¿por qué `JumpHeld` no se limpia cada frame, pero `JumpPressed` sí?

---

## Etapa 3 — Núcleo del jugador

Leer estos archivos en el orden indicado.

### 3.1 `PlayerStateMachine`

Archivo: `Assets/MyAssets/Scripts/Player/StateMachine/PlayerStateMachine.cs`

Revisar primero el enum `PlayerState` y después `ChangeState`. Tener presente que actualmente almacena una etiqueta de estado; todavía no ejecuta objetos de estado con `Enter`, `Tick` y `Exit`.

### 3.2 `PlayerStamina`

Archivo: `Assets/MyAssets/Scripts/Player/Components/PlayerStamina.cs`

Revisar:

- cómo encuentra `PlayerInputHandler`;
- cómo determina `CanSprint`;
- consumo y regeneración;
- evento `OnStaminaChanged`;
- teclas de depuración, si continúan habilitadas.

Se lee antes del controller porque sprint y dash dependen de la resistencia.

### 3.3 `PlayerController`

Archivo: `Assets/MyAssets/Scripts/Player/Controllers/PlayerController.cs`

Ahora sí recorrer el controlador. No empezar por todos sus métodos privados; seguir el flujo de arriba hacia abajo:

1. `Awake`: dependencias requeridas.
2. `Update` → `ProcessGameplayFrame`.
3. `RefreshEnvironmentState`: suelo, paredes y estado derivado.
4. `UpdateAbilityTimers`: buffers y bloqueos temporales.
5. `ProcessActionInput`: prioridad de salto, dash y ataque.
6. `FixedUpdate` → `ProcessPhysicsFrame`.
7. `HandleMovement`, gravedad y wall slide.
8. Métodos específicos de salto, wall jump, dash y ataque.
9. Actualización de Animator y orientación visual.

Mientras se lee, dibujar dos carriles:

```text
Update      = decisiones, input, timers y estado
FixedUpdate = cambios físicos sobre Rigidbody2D
```

Aspectos que conviene rastrear:

- orden de prioridad de las habilidades;
- quién escribe `Rigidbody2D.linearVelocity`;
- qué flags bloquean movimiento (`_isDashing`, `_isWallJumping`, `_isAttacking`);
- cómo funcionan coyote time y jump buffer;
- cuándo se actualiza `PlayerStateMachine`;
- cómo `GameStateManager.IsPlaying()` suspende el controller.

### 3.4 `PlayerAnimationEvents`

Archivo: `Assets/MyAssets/Scripts/Player/Animations/PlayerAnimationEvents.cs`

Leerlo inmediatamente después para entender el flujo inverso: el controller activa una animación y un evento del clip vuelve a llamar al controller para liberar el bloqueo del ataque.

### 3.5 `PlayerLamp`

Archivo: `Assets/MyAssets/Scripts/Player/Controllers/PlayerLamp.cs`

Revisar cómo `ToggleLanternPressed` cambia el estado de la lámpara y qué objetos visuales activa. Este script conecta input, presentación y objetos reactivos del entorno.

### 3.6 `PlayerHealth`

Archivo: `Assets/MyAssets/Scripts/Player/Components/PlayerHealth.cs`

Revisar daño, curación, muerte y los eventos `OnHealthChanged`, `OnDamageTaken` y `OnDeath`. Después identificar quién escucha esos eventos.

**Al terminar esta etapa debes poder narrar:** qué ocurre desde que se pulsa salto hasta que cambia la velocidad vertical, y desde que se pulsa ataque hasta que la animación libera el ataque.

---

## Etapa 4 — Interacción con el mundo

### 4.1 Contrato base

1. `Assets/MyAssets/Scripts/Core/Interfaces/IInteractable.cs`
2. `Assets/MyAssets/Scripts/Interaction/InteractableTrigger.cs`

Primero entender el contrato; después, el detector. Seguir el flujo `OnTriggerEnter2D` → mostrar indicación → `Update` comprueba `InteractPressed` → ejecutar `Interact()` → `OnTriggerExit2D` limpia la UI.

### 4.2 Presentación de interacción

1. `Assets/MyAssets/Scripts/UI/Interaction/InteractionUIManager.cs`
2. `Assets/MyAssets/Scripts/UI/Interaction/UI_Interaction.cs`

Comparar responsabilidades: indicación contextual anclada al objeto frente a mensajes de texto generales o temporizados.

### 4.3 Implementaciones concretas

Leer en este orden:

1. `Assets/MyAssets/Scripts/Environment/Checkpoints/CheckpointSource.cs`
2. `Assets/MyAssets/Scripts/Items/Pickups/PickupItem.cs`
3. `Assets/MyAssets/Scripts/Items/Pickups/PickupNote.cs`
4. `Assets/MyAssets/Scripts/Environment/Levers/Lever.cs`
5. `Assets/MyAssets/Scripts/Environment/Doors/DoorController.cs`

Cada uno implementa o participa en una consecuencia distinta de `Interact`: guardar posición, añadir datos, abrir una lectura, validar un requisito o accionar una puerta.

**Pregunta de control:** ¿qué diferencia existe entre detectar una interacción, validarla y presentar su resultado?

---

## Etapa 5 — Inventario y flujo de objetos

### 5.1 Modelo de datos

1. `Assets/MyAssets/Scripts/Inventory/InventoryCategory.cs`
2. `Assets/MyAssets/Scripts/Items/Data/InventoryItem.cs`
3. `Assets/MyAssets/Scripts/Inventory/InventoryEntry.cs`

Entender primero la categoría, después el ScriptableObject que define un objeto y finalmente la entrada que combina definición y cantidad.

### 5.2 Estado en ejecución

Archivo: `Assets/MyAssets/Scripts/Player/Inventory/PlayerInventory.cs`

Revisar inicialización singleton, adición, consulta, agrupación por categoría y búsqueda. Relacionarlo con `PickupItem` y `Lever`.

### 5.3 Presentación en el libro

1. `Assets/MyAssets/Scripts/UI/Utilities/UISelectableOption.cs`
2. `Assets/MyAssets/Scripts/Items/Inventory/InventoryCategoryOptionUI.cs`
3. `Assets/MyAssets/Scripts/Items/Inventory/InventoryItemOptionUI.cs`
4. `Assets/MyAssets/Scripts/UI/BookMenu/Inventory/InventoryPageController.cs`

Seguir datos desde `PlayerInventory` hasta la creación de opciones, selección de categoría y detalle del objeto.

**Traza recomendada:** `InteractPressed` → `PickupItem.Interact` → `PlayerInventory.AddItem` → abrir libro → `InventoryPageController` → opción visual.

---

## Etapa 6 — Notas y lectura contextual

### 6.1 Modelo y almacenamiento

1. `Assets/MyAssets/Scripts/Items/Data/NoteData.cs`
2. `Assets/MyAssets/Scripts/Items/Notes/NotesManager.cs`

### 6.2 Elementos de presentación

1. `Assets/MyAssets/Scripts/Items/Notes/NoteOptionUI.cs`
2. `Assets/MyAssets/Scripts/Items/Notes/NotesPageController.cs`
3. `Assets/MyAssets/Scripts/Items/Notes/NotesUIManager.cs`

### 6.3 Volver a `PickupNote`

Releer `PickupNote.cs` al final de esta etapa. Ahora será posible entender la cadena completa: recoge la definición, la registra, encola la página contextual, puede mostrar tutorial y cambia el estado global para abrir lectura.

Aspectos clave:

- diferencia entre poseer una nota y mostrarla;
- última nota recolectada;
- navegación entre páginas;
- transición `Playing` ↔ `BookMenu`;
- posible tutorial de primera lectura.

---

## Etapa 7 — Tutoriales

Leer en este orden:

1. `Assets/MyAssets/Scripts/Items/Tutorials/TutorialData.cs`
2. `Assets/MyAssets/Scripts/Core/Managers/TutorialManager.cs`
3. `Assets/MyAssets/Scripts/Core/Managers/TutorialCodexManager.cs`
4. `Assets/MyAssets/Scripts/UI/Tutorials/TutorialUIManager.cs`
5. `Assets/MyAssets/Scripts/Items/Tutorials/TutorialOptionUI.cs`
6. `Assets/MyAssets/Scripts/Items/Tutorials/TutorialPageController.cs`

Separar mentalmente tres conceptos:

- definición del tutorial;
- tutorial desbloqueado/visto;
- presentación inmediata o consulta posterior en el códice.

Seguir con atención cómo `TutorialUIManager` cambia `GameStateManager` a `Tutorial` y cómo lo devuelve a `Playing`.

---

## Etapa 8 — Libro, páginas y configuración

Leer después de inventario, notas y tutoriales, porque el libro los integra:

1. `Assets/MyAssets/Scripts/UI/BookMenu/BookMenuManager.cs`
2. `Assets/MyAssets/Scripts/UI/BookMenu/SettingsPageController.cs`
3. Releer `GameMenuController.cs` y `UIScreenManager.cs`.

Revisar:

- apertura y cierre;
- `BookPage` y página contextual pendiente;
- navegación horizontal entre secciones;
- llamadas que compiten por `ToggleMenuPressed`;
- retorno al checkpoint desde Settings;
- restauración del estado `Playing`.

**Traza recomendada:** `ToggleMenuPressed` → consumidor de la entrada → `GameStateManager.SetState(BookMenu)` → activar raíz del libro → seleccionar página → cerrar → `Playing`.

---

## Etapa 9 — HUD y recursos del jugador

### 9.1 Base común

1. `Assets/MyAssets/Scripts/UI/Core/BaseHUDModule.cs`
2. `Assets/MyAssets/Scripts/UI/Core/UIManager.cs`

### 9.2 Vida y resistencia

1. `Assets/MyAssets/Scripts/UI/HUD/LifeHUD/LifeHUDController.cs`
2. `Assets/MyAssets/Scripts/UI/HUD/StaminaHUD/StaminaHUDController.cs`

Relacionarlos con los eventos de `PlayerHealth` y `PlayerStamina`. Distinguir actualización por evento de interpolación visual realizada en `Update`.

### 9.3 Energía solar y slots

1. `Assets/MyAssets/Scripts/UI/HUD/SolarHUD/SolarHUDController.cs`
2. `Assets/MyAssets/Scripts/UI/HUD/SlotsHUD/QuickSlotState.cs`
3. `Assets/MyAssets/Scripts/UI/HUD/SlotsHUD/QuickSlotUI.cs`
4. `Assets/MyAssets/Scripts/UI/HUD/SlotsHUD/QuickSlotHUDController.cs`

Revisar qué partes son gameplay real y cuáles son controles de prueba o depuración.

---

## Etapa 10 — Checkpoints y recuperación

Leer juntos:

1. `Assets/MyAssets/Scripts/Core/Managers/CheckpointManager.cs`
2. Releer `Assets/MyAssets/Scripts/Environment/Checkpoints/CheckpointSource.cs`.
3. Releer `Assets/MyAssets/Scripts/UI/Menus/PauseMenuActions.cs`.
4. Releer la acción correspondiente en `SettingsPageController.cs`.

Seguir la diferencia entre guardar un `Transform`/posición y trasladar al jugador. Confirmar qué ocurre con `Rigidbody2D` al reaparecer.

---

## Etapa 11 — Cámara y objetos reactivos

### 11.1 Cámara

1. `Assets/MyAssets/Scripts/Camera/CameraDirector.cs`
2. `Assets/MyAssets/Scripts/Camera/CameraTrigger.cs`

El director posee la operación de enfoque; el trigger decide cuándo solicitarla. Revisar también la relación de `CameraTrigger` con `IInteractable`, `PlayerInputHandler` y la UI contextual.

### 11.2 Entorno reactivo

1. `Assets/MyAssets/Scripts/Environment/ReactiveObjets/LightReactiveObject.cs`
2. Releer `Assets/MyAssets/Scripts/Player/Controllers/PlayerLamp.cs`.

### 11.3 Utilidad visual

Archivo: `Assets/MyAssets/Scripts/Core/Utilities/Billboard.cs`

Leerlo al final porque es una utilidad aislada de presentación y no ayuda a comprender primero el dominio principal.

---

## Etapa 12 — Recorrido completo por acciones

Después de leer todos los bloques, reconstruir estas trazas sin mirar la guía:

### Pulsar salto

```text
Input Action Jump
→ PlayerInputHandler.JumpPressed / JumpHeld
→ PlayerController.Update
→ ProcessActionInput
→ HandleJump
→ validaciones de suelo, coyote time, buffer, salto extra o pared
→ cambio de Rigidbody2D.linearVelocity
→ Animator
→ PlayerInputHandler.LateUpdate limpia JumpPressed
```

### Pulsar interacción frente a una palanca

```text
Collider entra en InteractableTrigger
→ obtiene PlayerInputHandler y muestra InteractionUIManager
→ InteractPressed
→ valida GameStateManager.IsPlaying
→ Lever.Interact
→ consulta PlayerInventory si requiere objeto
→ acciona DoorController
→ CameraDirector enfoca el resultado
→ limpia la indicación
```

### Recoger y leer una nota

```text
InteractableTrigger
→ PickupNote.Interact
→ NotesManager.AddNote
→ BookMenuManager.QueueContextPage
→ tutorial opcional
→ GameStateManager cambia de estado
→ NotesPageController obtiene las notas
→ NoteOptionUI presenta selección
```

### Abrir el libro

```text
ToggleMenu Input Action
→ PlayerInputHandler.ToggleMenuPressed
→ consumidor del menú
→ GameStateManager: Playing a BookMenu
→ BookMenuManager activa la página
→ controladores de página leen navegación
→ cerrar libro
→ GameStateManager: BookMenu a Playing
```

### Recibir daño

```text
fuente de daño
→ PlayerHealth
→ modifica salud
→ OnDamageTaken / OnHealthChanged
→ LifeHUDController actualiza objetivo visual
→ salud cero
→ OnDeath
```

Si una de estas cadenas no puede explicarse con claridad, volver al primer script de la cadena que introduce una dependencia desconocida.

---

## Etapa 13 — Revisión dentro de Unity

La lectura termina en el Editor, no en el código. Para los componentes principales:

1. Abrir la escena jugable.
2. Seleccionar el GameObject que contiene el script.
3. Comparar cada campo del Inspector con su declaración.
4. Abrir los prefabs instanciados por controladores de páginas.
5. Revisar los ScriptableObjects de inventario, notas y tutoriales.
6. Revisar eventos de animación en los clips de ataque.
7. Activar **Enter Play Mode** y observar una traza a la vez.

Orden práctico de pruebas:

1. movimiento y salto;
2. sprint, dash y stamina;
3. ataque y evento de animación;
4. lámpara y objeto reactivo;
5. interacción básica;
6. pickup de objeto e inventario;
7. pickup de nota y lectura;
8. tutorial;
9. libro y navegación;
10. checkpoint y retorno;
11. palanca, puerta y cámara.

## Criterio de finalización

La revisión puede considerarse completa cuando sea posible responder, para cualquier acción del jugador:

- qué Input Action la origina;
- qué flag expone `PlayerInputHandler`;
- qué componente consume el flag;
- qué estado global la permite o bloquea;
- qué modelo o manager cambia;
- qué UI o animación presenta el resultado;
- qué referencia del Inspector conecta los componentes.

En ese punto ya no sólo se habrá leído el código: se habrá reconstruido el flujo operativo actual del videojuego.
