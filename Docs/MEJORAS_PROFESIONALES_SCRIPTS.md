# Hoja de ruta profesional para los scripts

Fecha de revisión: 2026-10-06

Proyecto: **Ashes of the First Light**

## Propósito

Este documento evalúa la arquitectura actual después de la reorganización de los 54 scripts de `Assets/MyAssets/Scripts`. Su objetivo no es aplicar patrones por moda, sino explicar:

- qué ya está implementado;
- qué problemas siguen presentes;
- cómo se corregirían;
- qué conocimientos requiere cada mejora;
- qué cambios afectarían escenas, prefabs o referencias del Inspector;
- en qué orden conviene implementarlos.

La limpieza reciente mejoró presentación, navegación y responsabilidad interna de métodos. No convirtió todavía el proyecto en una arquitectura modular: regiones y comentarios hacen el código comprensible, pero no reducen por sí solos el acoplamiento.

## Estado general

| Área | Estado actual | Siguiente nivel recomendado |
|---|---|---|
| Legibilidad | Implementado | Mantener comentarios orientados a intención |
| Regiones y organización | Implementado | Evitar regiones gigantes o vacías |
| Campos del Inspector | Conservados | Clasificarlos por tipo de dependencia |
| Métodos y guard clauses | Parcialmente implementado | Extraer decisiones de dominio y estados |
| Input System | Implementado parcialmente | Separar Action Maps por contexto |
| Máquina de estados | Estado almacenado, sin comportamiento polimórfico | Estados con `Enter`, `Tick`, `FixedTick`, `Exit` |
| Movimiento 2D | Funcional con `Rigidbody2D` | Separar motor, sensores y habilidades |
| Eventos | Usados en salud/stamina | Extender a inventario, estado global y coleccionables |
| Singletons | Uso alto | Composition root o referencias explícitas |
| Pruebas | Pendiente | EditMode + PlayMode + CI |
| Persistencia | Pendiente | Datos versionados por ID estable |

---

## 1. Condicionales: el objetivo no es eliminar todos los `if`

Un código profesional no evita los condicionales a cualquier precio. Un `if` pequeño que protege una precondición suele ser más claro que una jerarquía de clases.

### Mantener

Las guard clauses son apropiadas:

```csharp
if (_input == null)
    return;

if (!_input.DashPressed)
    return;
```

Expresan precondiciones y mantienen el camino principal sin indentación excesiva.

### Refactorizar

Conviene intervenir cuando:

- el mismo conjunto de condiciones aparece en varias clases;
- una cadena `if/else` selecciona comportamientos completos;
- agregar una habilidad obliga a modificar muchos métodos existentes;
- una condición mezcla input, reglas, física, animación y UI;
- los booleanos permiten combinaciones inválidas (`_isDashing`, `_isAttacking`, `_isWallSliding`, etc.).

### Herramientas apropiadas

| Problema | Solución habitual |
|---|---|
| Precondición simple | Guard clause |
| Elegir un dato | `switch` expression |
| Elegir un comportamiento extensible | Strategy o State |
| Reaccionar a un cambio | Evento C# o event channel |
| Secuencia temporal | Corrutina, Timeline o async controlado |
| Reglas configurables | ScriptableObject de configuración |
| Muchas combinaciones booleanas | Estado explícito o flags bien definidos |

No se debe reemplazar un `if` por reflexión, diccionarios o eventos si eso oculta una regla sencilla.

---

## 2. Máquina de estados del jugador

### Situación actual

`PlayerStateMachine` solo almacena un enum. `PlayerController.UpdateState()` decide el estado mediante una cadena de prioridades, mientras el comportamiento permanece repartido en el controlador.

Esto es suficiente para un prototipo, pero no es todavía una máquina de estados conductual.

### Riesgos actuales

- El estado puede quedar un frame por detrás de datos actualizados en `FixedUpdate`.
- Los booleanos pueden representar combinaciones contradictorias.
- Agregar `Stunned`, `Dead`, `Climbing`, `Parrying` o `Knockback` aumenta la cadena de condiciones.
- Entrar o salir de un estado no tiene un lugar único para ejecutar efectos.

### Opción recomendada para este proyecto

Usar estados C# puros con un contrato pequeño:

```csharp
public interface IPlayerState
{
    void Enter();
    void Tick();
    void FixedTick();
    void Exit();
}
```

La máquina se ocupa exclusivamente de transiciones:

```csharp
public sealed class PlayerStateMachine
{
    public IPlayerState Current { get; private set; }

    public void ChangeState(IPlayerState next)
    {
        if (next == null || ReferenceEquals(Current, next))
            return;

        Current?.Exit();
        Current = next;
        Current.Enter();
    }

    public void Tick() => Current?.Tick();
    public void FixedTick() => Current?.FixedTick();
}
```

Estados iniciales sugeridos:

- `GroundedState`
- `AirborneState`
- `WallSlideState`
- `DashState`
- `AttackState`
- posteriormente `DeadState`, `StunnedState` y `ParryState`

### Evitar

- Un `MonoBehaviour` por estado salvo que el equipo necesite configurarlos visualmente.
- ScriptableObjects con estado runtime mutable compartido entre jugadores.
- Transiciones desde cualquier lugar del proyecto.
- Un estado diferente por cada animación; estado de gameplay y estado del Animator no son necesariamente lo mismo.

### Alternativa más simple

Si el número de estados permanece pequeño, conservar el enum y extraer una tabla de transición es válido. No hace falta adoptar State Pattern hasta que existan comportamientos de entrada, salida y actualización realmente distintos.

**Estado:** pendiente.

**Impacto:** alto; requiere pruebas de movimiento y animación.

**Conocimiento:** interfaces, composición, ciclo de Unity y pruebas PlayMode.

---

## 3. Arquitectura recomendada para movimiento 2D

No existe un controlador único “usado por toda la industria”. La elección depende de si el juego prioriza física emergente o control determinista.

### Opción A: `Rigidbody2D` dinámico

Adecuada cuando el personaje debe interactuar naturalmente con fuerzas, plataformas y colisiones.

- Leer input en `Update`.
- Guardar comandos/buffers de un frame.
- Aplicar física en `FixedUpdate`.
- Usar `AddForce` para fuerzas acumulativas e impulsos físicos.
- Escribir `linearVelocity` cuando el diseño exige una velocidad exacta y responsiva.
- No mezclar movimiento por `transform.position` con un Rigidbody dinámico.

Unity indica que `linearVelocity` representa velocidad en unidades por segundo y señala que normalmente las fuerzas pueden ser preferibles; esto no prohíbe establecer velocidad directamente para un platformer controlado.

### Opción B: motor cinemático personalizado

Adecuada para platformers que necesitan resultados muy deterministas, slopes, corner correction y control exacto sobre cada colisión.

- Casts explícitos (`BoxCast`, `CapsuleCast`, raycasts).
- Resolución manual de desplazamiento.
- Reglas propias de slopes, plataformas móviles y penetración.
- Mayor esfuerzo de ingeniería y pruebas.

### Recomendación para este proyecto

Mantener por ahora `Rigidbody2D`, pero dividir responsabilidades:

```text
PlayerController
 ├─ PlayerInputReader
 ├─ PlayerMotor2D
 ├─ PlayerGroundSensor
 ├─ PlayerJumpAbility
 ├─ PlayerDashAbility
 ├─ PlayerCombatController
 └─ PlayerAnimationDriver
```

`PlayerController` debe orquestar; `PlayerMotor2D` debe ser el único escritor normal de velocidad. Dash, knockback y wall jump deberían solicitar cambios al motor en lugar de competir escribiendo el Rigidbody desde varias clases.

### Mejoras concretas

1. Crear `PlayerGroundSensor` para suelo y paredes.
2. Crear un `PlayerMotorConfig` ScriptableObject con aceleración y velocidades.
3. Extraer dash y salto como capacidades independientes.
4. Definir prioridad de fuentes de movimiento: knockback > dash > wall jump > locomoción.
5. Mover toda escritura de Animator a `PlayerAnimationDriver`.
6. Añadir PlayMode tests para coyote time, buffer, doble salto y wall jump.

**Estado:** organización interna implementada; separación por componentes pendiente.

**Impacto:** alto; modifica el prefab Player y necesita migración controlada.

---

## 4. Input System y contextos

El proyecto ya usa el Input System moderno, que Unity recomienda para la mayoría de proyectos nuevos. El problema no es la tecnología, sino que un mismo mapa mantiene activas acciones de gameplay, UI y depuración.

### Problemas detectados

- `E` puede alimentar interacción, navegación y crecimiento de lámpara.
- `P` es observado por más de un controlador de menú.
- WASD representa movimiento y navegación.
- Debug input continúa activo fuera de gameplay.
- Flags públicos de un frame permiten varios consumidores accidentales.

### Diseño recomendado

Separar Action Maps:

- `Gameplay`
- `BookMenu`
- `Tutorial`
- `Debug`

`GameStateManager` o un `InputContextController` debe habilitar exclusivamente el mapa apropiado.

```csharp
private void ApplyInputContext(GameState state)
{
    _actions.Gameplay.Disable();
    _actions.BookMenu.Disable();
    _actions.Tutorial.Disable();

    switch (state)
    {
        case GameState.Playing:
            _actions.Gameplay.Enable();
            break;
        case GameState.BookMenu:
            _actions.BookMenu.Enable();
            break;
        case GameState.Tutorial:
            _actions.Tutorial.Enable();
            break;
    }
}
```

Para gameplay continuo, una interfaz de lectura funciona bien:

```csharp
public interface IPlayerInput
{
    Vector2 Move { get; }
    bool SprintHeld { get; }
    bool ConsumeJump();
    bool ConsumeDash();
}
```

Los comandos discretos deberían consumirse una vez o publicarse a un responsable, no ser booleanos observables por cualquier sistema.

**Estado:** Input System implementado; separación de contextos pendiente.

**Prioridad:** muy alta porque elimina conflictos reales.

---

## 5. `[SerializeField]`: cuándo usarlo y cuándo no

### Conclusión

No conviene “dejar de usar `[SerializeField]`”. Unity serializa campos, no propiedades normales, y `[SerializeField] private` es una práctica apropiada para configuración y referencias asignadas por diseñadores.

### Usar `[SerializeField] private` para

- referencias a componentes de la misma escena o prefab;
- prefabs y assets configurados por diseño;
- valores que deben ajustarse por instancia;
- curvas, layers, offsets y parámetros visuales.

```csharp
[SerializeField] private Animator _animator;
public Animator Animator => _animator;
```

### Usar `GetComponent` + `[RequireComponent]` para

Dependencias obligatorias ubicadas siempre en el mismo GameObject:

```csharp
[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerMotor2D : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}
```

Así se evita una asignación manual redundante sin ocultar una dependencia externa.

### Usar ScriptableObject para

- configuración compartida entre múltiples instancias;
- datos de diseño que deben versionarse como assets;
- catálogos de items, habilidades, enemigos o movimiento;
- event channels cuando el desacoplamiento y la edición en Inspector lo justifican.

No guardar estado mutable del jugador en un asset compartido salvo que sea una decisión explícita y se reinicie correctamente.

### Usar inicialización explícita para

Objetos creados en runtime o dependencias proporcionadas por un composition root:

```csharp
public void Initialize(IInventory inventory, IGameClock clock)
{
    _inventory = inventory;
    _clock = clock;
}
```

### Evitar

- campos `public` solo para mostrarlos en Inspector;
- `FindFirstObjectByType` como mecanismo normal de inyección;
- propiedades automáticas esperando persistencia del Inspector;
- convertir todo en ScriptableObject;
- service locator global para dependencias locales.

### Migración segura

Al convertir un campo público en privado o renombrarlo, usar `[FormerlySerializedAs]` y validar escenas/prefabs. No hacer una migración masiva sin respaldo y prueba PlayMode.

---

## 6. SOLID aplicado al proyecto

### Single Responsibility

Una clase debe tener una razón principal para cambiar.

- `PlayerController`: orquestación.
- `PlayerMotor2D`: física de locomoción.
- `PlayerAnimationDriver`: Animator.
- `NotesRepository`: colección de notas.
- `NotesPagePresenter`: selección y contenido visible.

Una clase pequeña no es automáticamente SRP; importa el motivo de cambio.

### Open/Closed

Estados y habilidades deberían poder agregarse sin editar una cadena central enorme. Usar interfaces cuando existan varias implementaciones reales, no para cada clase.

### Liskov Substitution

Una implementación de estado o interacción debe respetar el contrato sin introducir precondiciones inesperadas. Evitar herencias de MonoBehaviour profundas.

### Interface Segregation

Preferir contratos específicos:

```csharp
public interface IReadOnlyStamina
{
    float Current { get; }
    bool CanSpend(float amount);
}

public interface IStaminaConsumer
{
    bool TrySpend(float amount);
}
```

El HUD necesita lectura; el dash necesita consumo. No ambos necesitan toda la clase concreta.

### Dependency Inversion

Las reglas de gameplay deberían depender de contratos, mientras un composition root conecta implementaciones Unity.

No es necesario usar un framework de dependency injection. Referencias serializadas, factories pequeñas e inicialización explícita pueden ser suficientes.

---

## 7. Eventos frente a `Update`

Usar `Update` para:

- lectura continua relevante;
- temporizadores activos;
- interpolación visual;
- movimiento por frame.

Usar eventos para:

- cambio de vida o stamina;
- item agregado/eliminado;
- nota recogida;
- cambio de `GameState`;
- tutorial desbloqueado;
- checkpoint actualizado.

El proyecto ya usa eventos correctamente en salud y stamina. El siguiente paso es publicar eventos desde `GameStateManager`, `PlayerInventory`, `NotesManager` y `TutorialManager` para evitar polling y refresh manual.

Opciones:

- eventos C#: rápidos, tipados y adecuados cuando productor y consumidor viven juntos;
- UnityEvent: útil para wiring de diseñador, menos visible en búsqueda de código;
- ScriptableObject event channels: desacoplan escenas y son amigables para Inspector, pero requieren disciplina para rastrear el flujo.

---

## 8. Singletons y composition root

Los singletons actuales facilitan el prototipo, pero ocultan dependencias y orden de inicialización.

### Mantener temporalmente

- servicios verdaderamente globales;
- sistemas únicos y estables durante la transición.

### Migrar primero

- UI que puede recibir referencias directas;
- managers encontrados con `FindFirstObjectByType`;
- dependencias locales del Player;
- servicios que no sobreviven cambios de escena.

Crear un `SceneContext` o `GameBootstrapper` que conecte referencias explícitamente. Este objeto es el composition root: conoce implementaciones concretas para que las reglas de dominio no tengan que conocerlas.

---

## 9. Inventario, notas y tutoriales

### Pendientes detectados

- `TutorialManager` y `TutorialCodexManager` duplican colecciones.
- `NotesUIManager` y `NotesPageController` representan dos lectores.
- `InventoryPageController` todavía no presenta los items de `PlayerInventory`.
- Managers devuelven listas mutables.

### Diseño recomendado

- Un servicio por dominio.
- Exponer `IReadOnlyList<T>`.
- Eventos `ItemAdded`, `ItemRemoved`, `NoteCollected`, `TutorialUnlocked`.
- UI como vista/presenter, sin modificar repositorios directamente.
- Persistir IDs y cantidades, no referencias completas a objetos runtime.
- Mantener ScriptableObjects como catálogo de datos estáticos.

---

## 10. Animación

Actualmente `PlayerController` escribe directamente parámetros del Animator.

Crear `PlayerAnimationDriver` con una API semántica:

```csharp
public void SetGrounded(bool grounded);
public void SetHorizontalSpeed(float speed);
public void PlayJump();
public void PlayDash();
public void PlayAttack();
```

Ventajas:

- nombres/hashes del Animator quedan en una sola clase;
- tests de movimiento no necesitan Animator;
- cambiar Animator Controller no modifica reglas de gameplay;
- animation events pueden comunicarse con combate mediante una interfaz estrecha.

---

## 11. Configuración y datos

Separar tres categorías:

1. **Configuración compartida:** ScriptableObjects, por ejemplo `PlayerMovementConfig`.
2. **Referencias de escena:** `[SerializeField] private`.
3. **Estado runtime:** campos privados no serializados.

Ejemplo de configuración:

```csharp
[CreateAssetMenu(menuName = "Game/Player/Movement Config")]
public sealed class PlayerMovementConfig : ScriptableObject
{
    [Min(0f)] public float maxSpeed = 6f;
    [Min(0f)] public float acceleration = 20f;
    [Min(0f)] public float deceleration = 25f;
}
```

En una siguiente iteración conviene usar propiedades de solo lectura o campos privados serializados, pero migrando datos con cuidado.

---

## 12. Pruebas profesionales

### EditMode

- transiciones de `PlayerStateMachine`;
- inventario: agregar, remover, categorías e identificación;
- stamina: agotamiento y umbral de recuperación;
- notas/tutoriales sin duplicados;
- reglas puras de habilidades.

### PlayMode

- coyote time y jump buffer;
- doble salto y wall jump;
- dash y coste de stamina;
- bloqueo de input en libro/tutorial;
- pickup de nota y página contextual;
- checkpoint y retorno;
- palanca, cámara y puerta;
- suscripción/desuscripción del HUD.

### Integración continua

- compilar en cada pull request;
- ejecutar EditMode y PlayMode tests;
- rechazar warnings nuevos;
- ejecutar `git diff --check`;
- mantener Assembly Definitions separadas para Runtime, UI y Tests.

---

## 13. Orden recomendado de implementación

### Fase 1 — Riesgos actuales

- [ ] Separar Action Maps por contexto.
- [ ] Eliminar el doble controlador del menú.
- [ ] Eliminar el uso de `InteractPressed` para aumentar la lámpara.
- [ ] Conectar `PlayerHealth.OnDeath` a un flujo real.
- [ ] Corregir o retirar la ruta inactiva de `CameraTrigger.TryActivate()`.
- [ ] Agregar tests de regresión del Player.

### Fase 2 — Player modular

- [ ] Extraer `PlayerGroundSensor`.
- [ ] Extraer `PlayerMotor2D`.
- [ ] Extraer `PlayerAnimationDriver`.
- [ ] Extraer salto y dash como capacidades.
- [ ] Adoptar estados conductuales cuando existan suficientes estados.

### Fase 3 — Dominios y UI

- [ ] Consolidar managers duplicados.
- [ ] Implementar eventos de inventario/notas/tutoriales.
- [ ] Completar la página de inventario.
- [ ] Separar vistas y presenters.

### Fase 4 — Infraestructura

- [ ] Crear composition root por escena.
- [ ] Reducir singletons y búsquedas globales.
- [ ] Añadir Assembly Definitions.
- [ ] Diseñar guardado versionado.
- [ ] Configurar CI y pruebas automáticas.

---

## Referencias oficiales consultadas

- [Unity Manual — Input](https://docs.unity3d.com/Manual/Input.html)
- [Unity 6 — Serialization rules](https://docs.unity3d.com/6000.0/Documentation/Manual/script-serialization-rules.html)
- [Unity 6 — Rigidbody2D.linearVelocity](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody2D-linearVelocity.html)
- [Unity — ScriptableObject](https://docs.unity.com/en-us/engine/6000.7/script-reference/unityengine/scriptableobject)
- [Unity — Architect game code with ScriptableObjects](https://unity.com/how-to/architect-game-code-scriptable-objects)
- [Unity — ScriptableObject event channels](https://unity.com/how-to/scriptableobjects-event-channels-game-code)

## Nota final

“Código de estudio profesional” no significa maximizar patrones, interfaces o archivos. Significa que las responsabilidades son claras, las dependencias son visibles, el comportamiento está probado, el flujo puede rastrearse y el equipo puede cambiar una funcionalidad sin romper otras. Esta hoja de ruta prioriza esos resultados sobre la complejidad accidental.
