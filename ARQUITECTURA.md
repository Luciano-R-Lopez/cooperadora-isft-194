# Arquitectura de Cooperadora ISFT 94

Misma lógica que el proyecto Eventos: cada capa hace solo su parte.

| Capa | Qué hace | Qué NO hace |
|---|---|---|
| **Views** | Arma el DTO con lo que tipeó el usuario, llama al controller, muestra el resultado. Atrapa solo `NegocioException`. | No valida reglas de negocio ni conoce entidades. |
| **Controllers** | Exige el rol (`ExigirRol`) y delega al servicio. | No valida datos ni contiene lógica. |
| **Services** | Orquesta: busca entidades relacionadas (`ObtenerExistente`), controla lo que depende de *otros* datos (DNI duplicado, caja abierta, cuota duplicada del año), persiste y devuelve DTOs. | No repite validaciones que ya hace el dominio. |
| **Models (dominio)** | Se validan solos: no se puede construir ni modificar una entidad en estado inválido. Tienen su comportamiento (`Cuota.CambiarEstado`, `Caja.CerrarCaja`, `Movimiento.AfectarCaja`, `Usuario.ActualizarDatos`...). | No conocen repositorios ni DTOs. |
| **Repositories** | Guardan y buscan. `ObtenerExistente` lanza `NoEncontradoException`. | Sin reglas de negocio. |

## Errores
- `NegocioException` es la base de todo error que se le muestra al usuario:
  `ReglaNegocioException` (dato inválido / regla incumplida), `NoEncontradoException`,
  `AccesoDenegadoException` (rol), `AutenticacionException` (login).
- Cualquier otra excepción es un error de programación: la atrapa el manejador global de `Program.cs`.

## Para agregar algo nuevo
1. Regla sobre los datos de una entidad → en el modelo (setter o método).
2. Regla que necesita consultar otras entidades → en el servicio.
3. Permiso por rol → en el controller.
4. La vista solo hace `try { ... } catch (NegocioException ex) { Ui.Error(ex.Message); }`.

## Modelo de dominio (según `cooperadora_uml.drawio.xml`)

```
Entidad ─┬─ Persona ─┬─ Usuario  (Rol, Activo)
         │           └─ Socio    (Categoria, Activo, Responsable?) ──► Socio (Responsable)
         ├─ Movimiento (abstracta: Usuario, Fecha, Monto, MedioPago, Caja, Anulado, AfectarCaja(), Anular())
         │     ├─ Ingreso (CategoriaIngreso)
         │     └─ Egreso  (MotivoEgreso)
         ├─ Cuota (Socio, Monto, Anio, Estado) ◄── DetalleIngresoCuota (PagadaPor?) ──► Ingreso
         ├─ ValorCuota (Anio, Monto)
         ├─ Caja (MontoInicial, MontoTotal, Estado) ◄── CajaTotalMedioPago ──► MedioPago
         ├─ ItemCatalogo (Nombre, Activo) ─┬─ MedioPago
         │                                 └─ MotivoEgreso
         └─ Auditoria (Usuario, Accion, EntidadAfectada, EntidadId, Fecha, Detalle)
```

- **Polimorfismo:** `Movimiento.AfectarCaja` hace los controles comunes y delega en `Aplicar`, que implementa cada subclase
  (el ingreso acredita, el egreso debita). `Anular` hace lo mismo con `Revertir` (el ingreso anulado debita, el egreso acredita).
  Nadie pregunta "¿es ingreso o egreso?" para operar la caja; `Tipo` y `Clasificacion` también son polimórficos.
- **Un solo repositorio de movimientos** (`IMovimientoRepository`): ingresos y egresos comparten id y listado.
- **`IMovimientoRegistrador`** es el único lugar que aplica (o anula) un movimiento en la caja, guarda y audita.
  Lo usan `IngresoService`, `EgresoService`, `CuotaService` (cobro de cuota) y `MovimientoService` (anulación).
- **Cada movimiento guarda la caja a la que se aplicó** (`Movimiento.Caja`, la asigna `AfectarCaja`, una sola vez).
  Los listados y el resumen de una caja filtran por esa referencia, no por fechas. Su efecto queda en
  `Caja.MontoTotal` y `CajaTotalMedioPago`.
- **Anular movimientos:** solo los de la caja abierta (una caja cerrada ya se contó), con motivo obligatorio y una sola vez.
  El movimiento no se borra: queda marcado como anulado, fuera de los totales del resumen. Si era el cobro de cuotas,
  esas cuotas vuelven a `Pendiente` (`Cuota.RevertirPago`). Lo hace el Contador desde Movimientos.
- **`Caja.MontoTotal`**: con la caja abierta es el monto esperado; `CerrarCaja(fecha, montoTotal)` lo reemplaza por el contado.
  El esperado de una caja cerrada se recalcula como `MontoInicial + Σ CajaTotalMedioPago`.
- **Saldo insuficiente:** un egreso (o anular un ingreso) no puede dejar la caja en negativo (`Caja.Debitar`).
  El saldo de un medio de pago sí puede ser negativo (por ejemplo, un egreso por transferencia sin ingresos por transferencia).
- **Cobrar una cuota** crea un `Ingreso(CuotaSocial)`, su `DetalleIngresoCuota` y pasa la cuota a `Pagada`; devuelve
  los datos del recibo (el número es el id del ingreso). Los ingresos de cuota social no se pueden cargar a mano.
  **No hay recargo** por pagar fuera de término: la cuota se cobra siempre por su monto.
- **El responsable paga por un socio a cargo:** desde su ficha elige el socio (si tiene más de uno), la cuota pendiente
  y el medio. También se puede marcar al cobrar desde la ficha del socio. Queda en `DetalleIngresoCuota.PagadaPor`,
  que solo acepta al responsable de ese socio.
- **Valor de la cuota:** `ValorCuota` guarda el valor de cada año. Se propone al generar cuotas (si no hay, el de la
  última cuota del año) y el operador lo puede cambiar. "Generar cuotas para todos" se la crea a cada socio activo
  que no la tenga.
- **Socios:** se editan (validando todo junto) y se dan de baja con baja lógica (`Activo`): conservan cuotas y pagos,
  no reciben cuotas nuevas y no pueden ser responsables. Un responsable con socios a cargo no puede dejar de serlo
  ni darse de baja hasta reasignarlos. Un `Responsable` no puede tener responsable.
- **Catálogos:** medios de pago y motivos de egreso heredan de `ItemCatalogo`. El administrador los agrega, renombra
  (sin repetir nombres) y desactiva; los inactivos no se ofrecen ni se aceptan en movimientos nuevos.
- **Auditoría:** cada servicio llama a `IAuditoriaService.Registrar` al dar de alta / modificar / dar de baja, con un
  detalle de qué cambió (`DescripcionCambios`: "Rol: Operador → Contador"). Solo la ve el administrador, con filtros.
- **Login:** 5 intentos fallidos seguidos bloquean ese nombre de usuario por 5 minutos (también si no existe, para no
  revelar qué usuarios hay). Cada usuario puede cambiar su propia contraseña ("Mi contraseña").
- **Reportes:** `Views/Common/Reportes.cs` imprime el recibo de cuota y el resumen de caja (vista previa de Windows;
  "Microsoft Print to PDF" para PDF). `ExportarCsv` exporta cualquier grilla tal como se ve (separador `;` para Excel).
- **Errores inesperados:** se muestran y quedan guardados en `%LOCALAPPDATA%\CooperadoraISFT94\errores.log`
  (`RegistroErrores`).
- **Tema visual (oscuro):** `Views/Common/Tema.cs` tiene la paleta y estila los controles; cada ventana llama a `Tema.Aplicar(this)`.
  Los colores van siempre por `Tema` (nada de `Color.X` sueltos). Las pestañas usan `Pestanias` (el `TabControl` nativo
  no se puede oscurecer). El login y la ventana principal abren maximizados; los formularios de tamaño fijo van
  centrados con `Ui.Centrado`. Los desplegables se llenan con `Ui.Llenar` (no con `DataSource`).
- **Formato regional:** `Program.cs` fija la cultura `es-AR`, así los montos se ven "$ 5.000,00" sin importar
  la configuración de Windows.
- **Módulos por rol:** `Views/Shell/ModulosPorRol.cs`. Sumar un módulo es agregar una línea ahí.
- **Permisos:** Operador = socios, cuotas (valor, generación, cobro, morosos) e ingresos (donación/subsidio);
  Contador = caja, egresos, consulta y anulación de movimientos; Administrador = usuarios, catálogos y auditoría.

### Tests
`Tests/CooperadoraISFT94.Tests` (xUnit) prueba las reglas del dominio y los servicios. Se corren con
`dotnet test CooperadoraISFT94.slnx`. Los servicios y repositorios tienen constructores `internal` (visibles para los
tests) que reciben sus dependencias y un reloj (`Func<DateTime>`), así cada test arma los suyos con datos propios
(`Contexto`). El `.csproj` de la app excluye la carpeta `Tests/` para no compilarla adentro.

### Agregar un medio de pago o motivo de egreso
Desde la pestaña "Catálogos" del administrador. Los datos de ejemplo se cargan en `DatosIniciales`.

### Diferencias deliberadas con el UML
| UML | Código | Motivo |
|---|---|---|
| `Usuario` sin nombre de usuario | `Usuario.NombreUsuario` (único, inmutable) | Hace falta un identificador de login. |
| `Usuario.password` | `PasswordHash` (PBKDF2) | Nunca se guarda la contraseña en texto plano. |
| `AfectarCaja()` sin parámetros | `AfectarCaja(Caja, CajaTotalMedioPago)` | Se le pasa la caja destino en vez de usar estado global. |
| Movimiento sin caja | `Movimiento.Caja` | Ubicar los movimientos de una caja por fechas es frágil (cierre y apertura en el mismo segundo, precisión de la base). |
| — | `Socio.Responsable` | Relación opcional entre socios pedida después del UML. |
| — | Clase base `Persona` | `Usuario` y `Socio` compartían nombre/apellido y su validación duplicada. |
| — | `TipoMovimiento` | Valor derivado y polimórfico, solo para filtrar/mostrar. |
| — | `ValorCuota`, `ItemCatalogo`, anulación de movimientos | Agregados al pedir valor de cuota por año, catálogos administrables y corrección de errores de carga sin borrar datos. |
