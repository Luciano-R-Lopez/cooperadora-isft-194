# Cooperadora ISFT 194

Aplicación de escritorio (WinForms, .NET 10) para la cooperadora del ISFT 194: socios, cuotas, ingresos,
egresos, caja, usuarios y auditoría. Por ahora los datos viven en memoria (se pierden al cerrar); la idea
es conectarla más adelante a MySQL a través de PHP.

La organización del código (capas, reglas y decisiones de diseño) está explicada en [ARQUITECTURA.md](ARQUITECTURA.md).

## Cómo correrla

Requiere el SDK de .NET 10 en Windows.

```bash
dotnet run --project CooperadoraISFT94.csproj
```

Usuarios de prueba (se cargan en `Datos/DatosIniciales.cs`):

| Usuario | Contraseña | Rol |
|---|---|---|
| `admin` | `admin123` | Administrador: usuarios, catálogos y auditoría |
| `operador` | `operador123` | Operador: socios, cuotas e ingresos |
| `contador` | `contador123` | Contador: caja, egresos y movimientos |
| `pablo.paz` | `operador123` | Operador y Contador (usuario con dos roles; solo con los datos de ejemplo) |

### Datos de ejemplo

Al iniciar en modo **Debug**, `Datos/DatosDeEjemplo.cs` simula el uso de la cooperadora desde marzo de 2024 hasta hoy,
usando los servicios reales (así todo cumple las reglas y queda en la auditoría):

- **38 socios**: 7 responsables con 1 a 3 socios a cargo, 4 que se inscriben a mitad de 2025 y 3 dados de baja.
- **Cuotas 2024, 2025 y 2026** ($ 4.000, $ 4.500 y $ 5.000), muchas pagadas (varias por el responsable) y algunas
  pendientes para el listado de morosos.
- **Una caja por mes** con cobros, donaciones, subsidios (abril, agosto y noviembre), servicios y otros gastos
  (repartidos entre los tres grupos de egreso);
  algunos cierres con diferencia. La caja de hoy queda abierta con movimientos y una donación anulada.
- Cambios de catálogos (se agregan "Mercado Pago" y "Limpieza", se desactiva "Cheque"), un usuario desactivado
  y la corrección de datos de un socio.

Es determinístico: siempre genera lo mismo (para la misma fecha de hoy). En modo Release solo se cargan los catálogos y
los tres usuarios de prueba.

Tests:

```bash
dotnet test CooperadoraISFT94.slnx
```

## Organización como el proyecto Eventos

El código se reorganizó para seguir la misma lógica que el proyecto Eventos (detalle en [ARQUITECTURA.md](ARQUITECTURA.md)):

- **Carpetas y capas iguales:** `Domain`, `Repository` (con `Memoria`), `Service`, `Controller`, `Dto`, `Forms`
  (con `Utils`), `Common` (con `Enums` y `Exceptions`) y `Datos`.
- **Interfaces:** `IEntidad` (todas las entidades), `IRepository<T>` (todos los repositorios, con `ObtenerProximoId`)
  e `IVuelveDeSubpantalla` (pantallas que se actualizan al volver de otra). `Repository/Repositorios.cs` es el único
  lugar donde se eligen los repositorios: para conectar una base de datos se cambian esas líneas.
- **Propiedades que se validan solas:** cada entidad valida sus datos en los setters y en el constructor, con
  mensajes claros ("El DNI debe tener 7 u 8 números, sin puntos.").
- **Servicios y controladores** como en Eventos: el servicio recibe los repositorios por constructor y mapea a DTOs;
  el controlador exige el rol y delega. Los DTO son clases con propiedades (`SocioDTO`, `CuotaDTO`...).
- **Pantallas hechas con el diseñador de Visual Studio** (`FrmSocios`, `FrmCuotas`, `FrmCaja`...): se pueden abrir y
  editar visualmente.
- **Menú lateral** en lugar de pestañas: cada usuario ve las secciones de sus roles; la ficha del socio se abre
  dentro de la misma ventana con **Volver**. Nueva pantalla de **Inicio** con un resumen según el rol (caja,
  morosos, socios, usuarios) y un aviso si la caja está cerrada.
- **Avisos de error como en Eventos:** los errores se muestran con `Mensajes.Advertencia`; lo que sale bien, con un
  aviso breve en la pantalla ("Socio guardado.") que se borra solo. Si quedan datos sin guardar, se pregunta antes
  de descartarlos.
- **Buscador** en cada grilla: filtra mientras se escribe, sin distinguir mayúsculas ni tildes y con varias palabras.
- **Textos con tildes** en las enumeraciones ("Cuota social", "Donación", "Modificación").
- **Cobro en familia:** desde la ficha de un responsable se cobran juntas sus cuotas y las de sus socios a cargo,
  eligiendo cuáles.

## Cambios de la rama `agus`

### Correcciones de la revisión del código
- **Cada movimiento guarda la caja a la que se aplicó.** Antes se ubicaban por fecha, lo que fallaba si una caja se
  cerraba y otra se abría en el mismo segundo, y el resumen calculaba totales con dos fuentes distintas.
- **Saldo insuficiente:** un egreso ya no puede dejar la caja en negativo.
- **Socio Responsable:** un responsable con socios a cargo no puede dejar de serlo ni darse de baja
  (sus socios quedarían a cargo de alguien que ya no es responsable).
- **Registrar pago** sin medios de pago cargados ya no rompe la aplicación.
- **Login:** un hash de contraseña guardado con formato inválido se trata como contraseña incorrecta, no como error.
- **Sesión:** si el usuario logueado edita sus datos, la barra superior se actualiza al instante.
- El dominio quedó encapsulado: los datos de socios y usuarios solo cambian por métodos que validan todo junto.
- `IngresoView` se movió a `Views/Operador` (la usa el Operador) y `CajaEstadoLabel` a `Views/Common`.

### Socios
- **Editar socios** (todos los datos, incluida la categoría y el responsable) con auditoría de qué cambió.
- **Dar de baja y reactivar** (baja lógica): se conservan sus cuotas y pagos, no recibe cuotas nuevas y no puede
  ser elegido como responsable.
- Columna de estado en el listado; los dados de baja se ven atenuados.

### Cuotas
- **Valor de la cuota por año**, que se propone al generar cuotas (el operador lo puede cambiar).
- **Generar la cuota del año para todos los socios activos** de una vez (saltea a los que ya la tienen).
- **Lista de morosos:** un socio por fila con los años que debe, el total y quién paga (él o su responsable),
  con las familias juntas. Se filtra por año, nombre o DNI y se pueden ocultar los dados de baja. Desde la lista se
  **cobran una o varias cuotas en un solo pago** (un ingreso y un recibo con el detalle), se abre la **ficha** del
  socio y se **copian los emails** (el del responsable si es quien paga) para mandar un recordatorio.
- **El responsable paga la cuota de un socio a cargo:** desde su ficha elige el socio (si tiene más de uno),
  la cuota pendiente y el medio de pago. También se puede marcar "Paga su responsable" al cobrar desde la ficha
  del socio. Queda registrado quién pagó (en el movimiento, el recibo y la auditoría).
- **Sin recargo** por pagar fuera de término: la cuota se cobra siempre por su monto.
- **Recibo** imprimible al registrar cada pago.

### Caja y movimientos
- **Anular movimientos** de la caja abierta, con motivo obligatorio: se revierte su efecto en la caja y queda a la
  vista marcado como anulado. Si era el cobro de una cuota, la cuota vuelve a pendiente.
- **Resumen de caja imprimible**, también de cajas ya cerradas (con lo contado y la diferencia).
- El resumen no cuenta los movimientos anulados.

### Administración
- **Catálogos administrables:** agregar, renombrar y desactivar medios de pago, motivos de egreso
  y grupos de egreso.
  Los desactivados no se ofrecen para movimientos nuevos; los ya registrados no cambian.
- **Grupo de egreso** (Estudiantes, Administración, Otros, editables por el administrador): cada egreso se carga
  con su grupo, independiente del motivo. Caja y movimientos tiene columna y filtro por grupo, y el resumen impreso
  de caja muestra el total de egresos de cada grupo.
- **Usuarios con varios roles:** en Administración se tildan uno o más roles (por ejemplo, Operador y Contador)
  y el usuario ve las pestañas de todos. Un administrador no puede quitarse su propio rol de Administrador.
- **Auditoría con detalle** de qué cambió (por ejemplo "Roles: Operador → Operador, Contador") y **filtros** por fecha,
  acción y texto.

### Seguridad y soporte
- **Mi contraseña:** cualquier usuario puede cambiar la suya (pide la actual y la nueva dos veces).
- **Bloqueo del login** por 5 minutos después de 5 intentos fallidos.
- **Registro de errores** inesperados en `%LOCALAPPDATA%\CooperadoraISFT194\errores.log`.

### Reportes y exportación
- Recibo de cuota y resumen de caja con la vista previa de impresión de Windows
  (para PDF, elegir la impresora "Microsoft Print to PDF").
- **Exportar a CSV** (se abre con Excel) los listados de socios, morosos, movimientos y auditoría.
- **Exportar egresos por grupo** (Caja y movimientos): se elige un período y se genera un CSV con el total de cada
  grupo (Estudiantes, Administración, Otros) y el detalle de sus gastos con subtotales. No incluye los anulados.

### Interfaz
- **Tema oscuro y sobrio** en todas las pantallas: menú, desplegables, grillas, barras de desplazamiento,
  campos numéricos, avisos, calendario y barra de título (Windows 10/11).
- **Pantalla completa:** el login y la ventana principal abren maximizados; los formularios quedan centrados.
- **Montos en pesos** ("$ 5.000,00") sin importar la configuración regional de Windows
  (con "español (Latinoamérica)" se mostraba "XDR").
- Campo de fecha propio con calendario desplegable, y avisos propios en lugar de los de Windows.

### Tests
- Proyecto `Tests/CooperadoraISFT94.Tests` (xUnit) con **67 tests**: reglas del dominio (caja, movimientos,
  socios, cuotas, contraseñas) y servicios (DNI repetido, generación de cuotas, pagos, pago del responsable,
  anulaciones, resumen de caja, bloqueo de login, catálogos, grupos de egreso, exportación de egresos, usuarios multirol
  y filtros de auditoría).
- Los servicios reciben sus repositorios y un reloj por constructor, así cada test usa datos propios y una hora fija.

## Pendiente

- Persistencia en MySQL (a través de PHP). Hoy todo se guarda en memoria.
