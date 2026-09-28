# ControlCintasPLC
Control de 3 cintas transportadoras, leer el estado de entradas digitales con sensores, leer el estado de 4 entradas analogicas RTD y escribir en 4 salidas analogicas 4-20mA, usando un PLC Siemens S7-1200.
Interfaz de usuario (UI) desarrollada en C# con .NET 9 y WinForms.

Este es un proyecto de automatización industrial común, que involucra programación del PLC (usando TIA Portal) y comunicación Ethernet entre la app C# y el PLC. 
Esta es una guía para implementar proyectos basicos y escalables en complejidad, esta basado en la biblioteca S7NetPlus, que es gratuita y compatible con S7-1200.

## v2 — Planta escalable: N cintas, sensores de temperatura y variadores

La HMI y el PLC ahora se configuran por datos, sin tocar código para añadir elementos (hasta 16 de cada tipo).

### Estructura
```
PLC/                         Fuentes externas TIA Portal (importar en este orden)
  01_UDTs.udt                UDT_Cinta (8 B), UDT_Temperatura (16 B), UDT_Variador (24 B)
  02_DBs.db                  DB_General, DB_Cintas, DB_Temperaturas, DB_Variadores (NO optimizados)
  03_FBs.scl                 FB_Cinta, FB_Temperatura, FB_Variador, FB_Secuencia
  04_Planta.scl              DB_IO, FC_Entradas, FC_Salidas, FB_Planta, OB1
ControlCintasPLC/
  plantconfig.json           Cintas, temperaturas, variadores, IP, nº de DB
  Config/PlantaConfig.cs     Modelo + carga/validación del JSON
  Plc/DbLayout.cs            Mapa de offsets (espejo de los UDT)
  Plc/PlcService.cs          S7netplus async, 1 lectura por DB, acceso serializado
  Controles/*.cs             Paneles dinámicos Cinta / Temperatura / Variador
```

### Importar en TIA Portal
1. *Fuentes externas → Agregar archivo* con los 4 ficheros de `PLC/` → *Generar bloques desde fuente*.
2. Asignar números: `DB_General`=DB10, `DB_Cintas`=DB11, `DB_Temperaturas`=DB12, `DB_Variadores`=DB13 (o cambiarlos en `plantconfig.json`).
3. CPU → *Protección y seguridad → Mecanismos de conexión*: activar **Permitir acceso PUT/GET**.
4. Ajustar direcciones físicas **sólo** en `FC_Entradas` / `FC_Salidas`.

### Funciones
| Elemento | PLC | HMI |
|---|---|---|
| Cinta | Marcha/paro, permiso aguas abajo, atasco temporizado, térmico, contador de piezas, fallo enclavado + reset | Marcha/paro, reset, incluir en secuencia, simulación de sensores |
| Secuencia | Arranque N→1 y parada 1→N con retardo configurable, se aborta con emergencia | Marcha/Parada línea, retardo |
| Temperatura (RTD SM1231) | °C = raw/10, aviso/disparo con histéresis, rotura de hilo ⇒ disparo, detiene la cinta asociada | Valor, barra, estado, límites editables |
| Variador | Cableado (DO marcha/inversión/reset, AQ 4-20 mA, AI Hz/A) **o** PROFINET SINAMICS telegrama 1 (STW1/HSW, ZSW1/HIW); rampa software; esclavo de cinta | Hz reales, corriente, consigna y rampa, marcha/paro/inversión/reset |

### Añadir una cinta / sensor / variador
1. `FC_Entradas`/`FC_Salidas`: mapear sus E/S al índice `i` de `DB_IO`.
2. `plantconfig.json`: añadir la entrada en `Cintas`, `Temperaturas` o `Variadores` (`CintaAsociada` enlaza temperatura/variador con una cinta).
3. En la HMI: *Recargar config.* → *Enviar config. al PLC*.

Más de 16 elementos: subir los `Array[1..16]` en `PLC/` y `DbLayout.MaxElementos`.

### Pruebas con PLCSIM
Marcar **Simulación (PLCSIM)**: ignora térmicos, variadores responden en modo simulado y los botones *Sim E / Sim S* sustituyen a los sensores.

## Interfaz de Usuario
![Interface de Usuario](screen_1.png)

## Pantalla de TIA Portal
![TIA Portal](screen_2.png)

## Codigo fuente en Visual Studio
![Codigo fuente Visual Studio](screen_3.png)

# Configuración del PLC S7-1200
## Hardware necesario:

PLC S7-1200 con módulo Ethernet (integrado en la mayoría).
3 salidas digitales para controlar los motores de las cintas (por ejemplo, relés o módulos de salida para motores).
Entradas para sensores (ej. botones de start/stop, sensores de proximidad para detectar objetos en las cintas).
Asume que las cintas son controladas por bits en un Data Block (DB) o memorias M.

## Para cada cinta: 
Un contacto normally open (NO) ligado al bit de control, que activa la salida del motor (ej. %Q0.0 para cinta 1).
Lógica básica para una cinta transportadora: Start button (%I0.0) sella el bit On, Stop button (%I0.1) lo resetea. Sensor de fin de línea apaga automáticamente si detecta objeto.
Para 3 cintas secuenciales: La cinta 2 se activa solo si la 1 está en marcha y hay un objeto detectado, etc.
Para 3 cintas con enecendido y apagado automatico con tiempo programable entre cada encendido/apagado

## Ejemplo de lógica simple para una cinta (extiende a 3):
En el OB1 (ciclo principal), se usa lógica simple para controlar salidas.

%I0.2      Cinta1_On    %I1.1           %Q0.1

----| |--------| |------|/|-------------|/|------------( )

Si Start presionado y no Stop, enciende motor.
Si sensor detecta atasco o fin, apaga.

## Desarrollo de la UI en C# .NET 9 WinForms
La UI se ha desarrollado en Visual Studio con .NET 9 usando la biblioteca S7netplus para la comunicacion con el PLC.

## Diseño de la UI:
Botones para cada cinta: "Encender Cinta 1", "Apagar Cinta 1", etc.

Labels o LEDs (usa PictureBox con imágenes verde/rojo) para mostrar estado (On/Off).

Un TextBox para la IP del PLC y un botón "Conectar".

Un Timer para leer estados periódicamente (cada 500ms).

## Pruebas
Ejecuta PLCSIM, descarga programa, RUN.

Inicia NetToPLCSim, conecta, crea un servidor usando las IP asignadas.

Ejecuta la app C#, conecta a IP de NetToPLCSim:502 (ej. 192.168.1.11:102), el puerto 102 se asigna por defecto.

Debes ejecutar NetToPLCSim en modo administrador para que cierre s7iexe, de lo contrario no estará disponible el puerto 102.

Enciende/apaga, verifica estados en TIA Portal (DB1) y salidas simuladas.

Código para comunicación con el PLC:

Para conectar vía Ethernet (rack 0, slot 1 para S7-1200)


