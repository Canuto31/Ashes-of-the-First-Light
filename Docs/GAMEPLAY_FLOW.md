# DIAGRAMA DE FLUJO DE JUGABILIDAD
## ASHES OF THE FIRST LIGHT

Este documento representa visualmente el flujo general del juego y la secuencia jugable del primer MVP. Debe actualizarse cuando cambien las decisiones principales del `GDD.md`.

---

## 1. Flujo general del juego

```mermaid
flowchart TD
    Start[Inicio del juego] --> Menu[Menú principal]
    Menu -->|Nueva partida| Awakening[Despertar del Guardián del Sol]
    Menu -->|Continuar| LastCheckpoint[Último checkpoint activado]

    Awakening --> IntroSector[Sector 1: Selva Corrompida]
    IntroSector --> LearnCore[Aprender movimiento, halo, combate y parry]
    LearnCore --> Patasola[Enfrentar a La Patasola]
    Patasola -->|Victoria| LifeFragment[Recuperar Fragmento de la Vida]
    Patasola -->|Derrota| Death

    LifeFragment --> RestoreJungle[Restauración parcial de la selva]
    RestoreJungle --> OpenWorld[Apertura del mundo interconectado]
    LastCheckpoint --> Explore
    OpenWorld --> Explore

    subgraph CoreLoop[Bucle principal de jugabilidad]
        Explore[Explorar sectores] --> Challenge[Combate, plataformas y acertijos]
        Challenge --> Discover[Descubrir secretos, notas, llaves y Cenizas de Luz]
        Discover --> Improve[Activar checkpoints y mejorar habilidades]
        Improve --> SelectSector[Elegir una ruta o jefe disponible]
    end

    SelectSector --> Mohan[El Mohán: Memoria]
    SelectSector --> Silbon[El Silbón: Verdad]
    SelectSector --> Llorona[La Llorona: Esperanza]
    SelectSector --> Madremonte[La Madremonte: Equilibrio]
    SelectSector --> Caiman[El Hombre Caimán: Identidad]

    Mohan --> SectorVictory[Derrotar jefe y recuperar fragmento]
    Silbon --> SectorVictory
    Llorona --> SectorVictory
    Madremonte --> SectorVictory
    Caiman --> SectorVictory

    SectorVictory --> FragmentCheck{¿Se reunieron los seis fragmentos?}
    FragmentCheck -->|No| Backtracking[Regresar, explorar y abrir nuevas rutas]
    Backtracking --> Explore
    FragmentCheck -->|Sí| FinalRegion[Desbloquear Templo Ceremonial Suspendido]
    FinalRegion --> FinalBoss[Enfrentar al jefe final]
    FinalBoss -->|Victoria| RestoreSun[Restaurar el Sol]
    RestoreSun --> Ending[Resolución narrativa y créditos]
    FinalBoss -->|Derrota| Death

    Challenge -->|Salud agotada| Death[Muerte del Guardián]
    Death --> DropEmber[Crear Ascua Caída con las Cenizas de Luz]
    DropEmber --> Respawn[Reaparecer en el último checkpoint]
    Respawn --> RecoverChoice{¿Recuperar el Ascua Caída?}
    RecoverChoice -->|Sí| RecoverEmber[Regresar al lugar de muerte]
    RecoverEmber -->|Se recupera antes de morir| RestoreCurrency[Recuperar Cenizas de Luz]
    RestoreCurrency --> Explore
    RecoverEmber -->|Se muere nuevamente| LoseCurrency[Perder las Cenizas anteriores]
    LoseCurrency --> Respawn
    RecoverChoice -->|No| Explore
```

---

## 2. Flujo del primer MVP

```mermaid
flowchart TD
    MVPStart[Iniciar nueva partida] --> WakeUp[Despertar en la Selva Corrompida]
    WakeUp --> BasicMovement[Aprender movimiento lateral y salto]
    BasicMovement --> HaloTutorial[Aprender cambio libre entre luz y oscuridad]
    HaloTutorial --> SolarSword[Obtener la Espada Solar]
    SolarSword --> FirstCombat[Primer combate]
    FirstCombat --> ParryTutorial[Aprender parry de precisión]
    ParryTutorial --> FirstCheckpoint[Activar primer checkpoint]

    FirstCheckpoint --> GroundRoll[Obtener roll terrestre]
    GroundRoll --> RollChallenge[Superar desafío de roll y resistencia]
    RollChallenge --> AirDash[Obtener impulso aéreo]
    AirDash --> ProgressionTutorial[Introducir Cenizas, mapa, inventario y árbol básico]

    ProgressionTutorial --> KeySearch[Explorar y encontrar llave]
    KeySearch --> Mechanism[Usar llave en palanca o mecanismo]
    Mechanism --> OpenDoor[Abrir ruta permanentemente]

    OpenDoor --> CombinedChallenge[Prueba combinada de sistemas]

    subgraph MVPSystems[Sistemas puestos a prueba]
        LightDark[Luz y oscuridad]
        Combat[Espada, daño, parry y postura]
        Movement[Salto, roll, impulso aéreo y paredes]
        Resources[Vida, resistencia, elixires y Cenizas]
        Exploration[Inventario, notas, llaves, secretos y mapa]
        Enemies[Cuatro arquetipos de enemigos]
    end

    CombinedChallenge --> LightDark
    CombinedChallenge --> Combat
    CombinedChallenge --> Movement
    CombinedChallenge --> Resources
    CombinedChallenge --> Exploration
    CombinedChallenge --> Enemies

    LightDark --> BossAccess[Acceso a La Patasola]
    Combat --> BossAccess
    Movement --> BossAccess
    Resources --> BossAccess
    Exploration --> BossAccess
    Enemies --> BossAccess

    BossAccess --> PatasolaFight[Combate contra La Patasola]
    PatasolaFight -->|Derrota| MVPDeath[Muerte y regreso al checkpoint]
    MVPDeath --> BossAccess
    PatasolaFight -->|Victoria| CollectLife[Recuperar Fragmento de la Vida]
    CollectLife --> RestoreLight[Restaurar parcialmente la luz y la selva]
    RestoreLight --> UnlockFulgor[Desbloquear primera versión del Fulgor Solar]
    UnlockFulgor --> MVPEnd[Fin del primer MVP]
```

---

## 3. Bucle de exploración de un sector

```mermaid
flowchart LR
    Enter[Entrar al sector] --> Explore[Explorar]
    Explore --> Encounter{Tipo de encuentro}
    Encounter -->|Combate| Fight[Derrotar enemigos]
    Encounter -->|Plataformas| Traverse[Usar habilidades de movimiento]
    Encounter -->|Acertijo| Toggle[Alternar luz y oscuridad]
    Encounter -->|Secreto| Reward[Obtener nota, llave, objeto o Cenizas]

    Fight --> Resources[Gestionar vida, resistencia y elixires]
    Traverse --> Discovery[Descubrir ruta o atajo]
    Toggle --> Discovery
    Reward --> Inventory[Actualizar inventario o diario]

    Resources --> Checkpoint{¿Descansar?}
    Discovery --> Checkpoint
    Inventory --> Checkpoint
    Checkpoint -->|Sí| Rest[Recuperar vida y elixires; reaparecen enemigos]
    Checkpoint -->|No| Explore
    Rest --> Explore

    Explore --> Gate[Superar llave, habilidad o requisito]
    Gate --> Boss[Acceder al jefe de sector]
    Boss --> Fragment[Recuperar fragmento y restaurar luz]
    Fragment --> Return[Regresar al mundo interconectado]
```

---

## Estado del documento

Los flujos representan las decisiones actuales del GDD. El diseño exacto de salas, encuentros, tiempos, posición de checkpoints, comportamiento detallado de La Patasola y presentación final permanecen **pendientes de definición para sesiones posteriores**.
