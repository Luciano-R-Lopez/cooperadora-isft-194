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

Usuarios de prueba (se cargan en `Repositories/InMemory/DatosIniciales.cs`):

| Usuario | Contraseña | Rol |
|---|---|---|
| `admin` | `admin123` | Administrador: usuarios, catálogos y auditoría |
| `operador` | `operador123` | Operador: socios, cuotas e ingresos |
| `contador` | `contador123` | Contador: caja, egresos y movimientos |

Tests:

```bash
dotnet test CooperadoraISFT94.slnx
```

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
- **Listado de morosos** (cuotas pendientes, por año o todas) con el total adeudado.
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
- **Catálogos administrables:** agregar, renombrar y desactivar medios de pago y motivos de egreso.
  Los desactivados no se ofrecen para movimientos nuevos; los ya registrados no cambian.
- **Auditoría con detalle** de qué cambió (por ejemplo "Rol: Operador → Contador") y **filtros** por fecha,
  acción y texto.

### Seguridad y soporte
- **Mi contraseña:** cualquier usuario puede cambiar la suya (pide la actual y la nueva dos veces).
- **Bloqueo del login** por 5 minutos después de 5 intentos fallidos.
- **Registro de errores** inesperados en `%LOCALAPPDATA%\CooperadoraISFT94\errores.log`.

### Reportes y exportación
- Recibo de cuota y resumen de caja con la vista previa de impresión de Windows
  (para PDF, elegir la impresora "Microsoft Print to PDF").
- **Exportar a CSV** (se abre con Excel) los listados de socios, morosos, movimientos y auditoría.

### Interfaz
- **Tema oscuro y sobrio** en todas las pantallas: pestañas, desplegables, grillas, barras de desplazamiento,
  campos numéricos, avisos, calendario y barra de título (Windows 10/11).
- **Pantalla completa:** el login y la ventana principal abren maximizados; los formularios quedan centrados.
- **Montos en pesos** ("$ 5.000,00") sin importar la configuración regional de Windows
  (con "español (Latinoamérica)" se mostraba "XDR").
- Campo de fecha propio con calendario desplegable, y avisos propios en lugar de los de Windows.

### Tests
- Proyecto `Tests/CooperadoraISFT94.Tests` (xUnit) con **54 tests**: reglas del dominio (caja, movimientos,
  socios, cuotas, contraseñas) y servicios (DNI repetido, generación de cuotas, pagos, pago del responsable,
  anulaciones, resumen de caja, bloqueo de login, catálogos y filtros de auditoría).
- Los servicios reciben sus repositorios y un reloj por constructor, así cada test usa datos propios y una hora fija.

## Pendiente

- Persistencia en MySQL (a través de PHP). Hoy todo se guarda en memoria.
