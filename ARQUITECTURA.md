# Arquitectura de Cooperadora ISFT 194

El código sigue la misma organización que el proyecto Eventos (Calculadora de Eventos): las mismas carpetas, las
mismas interfaces, la misma forma de validar, de avisar errores y de armar las pantallas.

## Estructura del proyecto

| Carpeta | Contenido |
|---|---|
| `Domain` | Entidades y reglas de negocio (`Socio`, `Cuota`, `Caja`, `Movimiento`, `Ingreso`, `Egreso`, `Usuario`...). Todas implementan `IEntidad`. Cada entidad valida sus datos en el constructor y en los setters de sus propiedades. |
| `Repository` | Interfaz `IRepository<T>`, común a todas las entidades: `ObtenerProximoId`, `Agregar`, `Actualizar`, `ObtenerTodos`, `ObtenerExistente`, `Eliminar`. `Repositorios.cs` es el único lugar donde se elige la implementación. |
| `Repository/Memoria` | Implementación en memoria: `RepositorioBase<T>` y un repositorio por entidad. Es donde se consultan los datos, con o sin base. |
| `Repository/MySql` | Base MySQL (`Configuracion.UsarBaseDeDatos`). `CargadorMySql` lee la base al iniciar y llena los repositorios en memoria; `RepositorioMySql<T>` envuelve a cada uno y escribe cada cambio en la base antes de aplicarlo en memoria, con la clase de `Persistencias` de esa entidad (`IPersistencia<T>`). Los servicios no cambian. Detalle en `BaseDeDatos/README.md`. |
| `Service` | Casos de uso. Cada servicio tiene un constructor sin parámetros (toma los repositorios de `Repositorios`) y otro que los recibe, para los tests. Mapean entidades a DTOs (`MapearADto`). |
| `Controller` | Punto de entrada que usan las pantallas: exige el rol (`BaseController.ExigirRol`) y delega en el servicio. |
| `Dto` | Datos que viajan entre las pantallas y los servicios (`SocioDTO`, `CuotaDTO`, `MovimientoDTO`...): clases con propiedades. |
| `Forms` | Pantallas de Windows Forms hechas con el diseñador de Visual Studio (`FrmX.cs` + `FrmX.Designer.cs` + `FrmX.resx`). |
| `Forms/Utils` | Utilidades de interfaz: `Tema`, `Mensajes`, `Aviso`, `Formulario`, `Grilla`, `Busqueda`, `Navegacion`, `IVuelveDeSubpantalla`, `CampoFecha`, `ExportarCsv`, `Reportes`, `Opcion`. |
| `Common` | `Configuracion` (intentos de login, años válidos, ruta del registro de errores), `Formato` (montos y fechas), `TextoEnum`, `Validaciones`, `PasswordHasher`, `SesionActual`, `RegistroErrores`, enumeraciones (`Common/Enums`, con `[Description]`) y excepciones (`Common/Exceptions`). |
| `Datos` | `DatosIniciales` (catálogos y un usuario por rol, siempre) y `DatosDeEjemplo` (dos años y medio simulados, solo en modo Debug). |
| `Tests` | Proyecto aparte con xUnit (la app no lo compila). |

## Qué hace cada capa

| Capa | Qué hace | Qué NO hace |
|---|---|---|
| **Forms** | Arma el DTO con lo que se escribió, llama al controller y muestra el resultado. Atrapa solo `NegocioException`. | No valida reglas de negocio ni conoce entidades. |
| **Controller** | Exige el rol y delega en el servicio. | No valida datos ni contiene lógica. |
| **Service** | Busca las entidades relacionadas (`ObtenerExistente`), controla lo que depende de *otros* datos (DNI repetido, caja abierta, cuota repetida del año), guarda y devuelve DTOs. | No repite validaciones que ya hace el dominio. |
| **Domain** | Se valida sola: no se puede construir ni modificar una entidad en estado inválido. Tiene su comportamiento (`Cuota.MarcarPagada`, `Caja.Cerrar`, `Movimiento.AfectarCaja`...). | No conoce repositorios ni DTOs. |
| **Repository** | Guarda y busca. `ObtenerExistente` lanza `NoEncontradoException`. | Sin reglas de negocio. |

## Errores y avisos
- `NegocioException` es todo error que se le muestra al usuario. Sus variantes: `NoEncontradoException`,
  `AccesoDenegadoException` (rol) y `AutenticacionException` (login).
- Las pantallas hacen `try { ... } catch (NegocioException ex) { Mensajes.Advertencia(ex.Message); }`.
- Lo que sale bien se avisa sin interrumpir con `Aviso.Mostrar(lblAviso, "Socio guardado.")`: el texto se borra solo.
- `Mensajes.Confirmar` pregunta antes de lo que no se puede deshacer; `Formulario` detecta datos escritos sin guardar
  y pregunta antes de descartarlos (al cambiar de sección, al editar otro registro o al cerrar).
- Cualquier otra excepción es un error de programación: la atrapa `Program.cs`, se muestra con
  `Mensajes.ErrorInesperado` y queda en `%LOCALAPPDATA%\CooperadoraISFT194\errores.log`.

## Para agregar algo nuevo
1. Regla sobre los datos de una entidad → en el setter de la propiedad o en un método de la entidad.
2. Regla que necesita consultar otros datos → en el servicio.
3. Permiso por rol → en el controller.
4. Pantalla nueva → `FrmX` con el diseñador, `Tema.Aplicar(this)` en el constructor y un botón en el menú de
   `FrmPrincipal` (visible según el rol en `ConfigurarMenu`).

## Pantallas
- **`FrmPrincipal`**: menú lateral con las secciones de los roles del usuario (un usuario con varios roles ve las de
  todos). Cada sección se abre dentro de la misma ventana; `Navegacion.AbrirSubpantalla` abre otra encima (por
  ejemplo, la ficha del socio desde Socios o desde Morosos) con **Volver**, y la pantalla de origen se actualiza al
  volver (`IVuelveDeSubpantalla`).
- **Altas y ediciones** (Usuarios, Socios, Catálogos): formulario a la izquierda y grilla con buscador a la derecha.
  **Guardar** crea; **Editar** carga el registro ("Editando: ...") y aparece **Cancelar**. Los campos obligatorios
  tienen `*`.
- **Grillas**: se llenan con DTOs y `Grilla.MostrarColumnas` elige qué columnas se ven y con qué título.
  `Busqueda.Coincide` filtra mientras se escribe, sin distinguir mayúsculas ni tildes y con varias palabras.
  Cuando no hay filas se muestra una guía (`MensajeSinFilas`).
- **Tema oscuro** (`Forms/Utils/Tema.cs`): colores, letras y estilo de cada control. Como en Eventos, algunos
  estilos dependen del nombre del control: `btnGuardar`, `btnRegistrar`, `btnCobrar`... son azules; `btnAnular` es
  rojo; `lblModo`, `lblTitulo...` y `lblDgv...` son títulos; `lblAviso` es el aviso breve; `lblEstadoCaja` es la franja
  de caja abierta o cerrada; los paneles `pnlTarjeta...` son tarjetas.
- **Textos de las enumeraciones**: se definen con `[Description]` ("Cuota social", "Donación", "Modificación") y se
  muestran con `TextoEnum`.
- **Formato**: `Program.cs` fija la cultura `es-AR`; `Formato.Moneda` es "$ #,##0.00".

## Modelo de dominio

```
IEntidad ─┬─ Persona ─┬─ Usuario  (Roles, Activo)
          │           └─ Socio    (Categoria, Activo, Responsable?) ──► Socio (Responsable)
          ├─ Movimiento (abstracta: Usuario, Fecha, Monto, MedioPago, Caja, Anulado, AfectarCaja(), Anular())
          │     ├─ Ingreso (CategoriaIngreso)
          │     └─ Egreso  (MotivoEgreso, GrupoEgreso)
          ├─ Cuota (Socio, Monto, Anio, Estado) ◄── DetalleIngresoCuota (PagadaPor?) ──► Ingreso
          ├─ ValorCuota (Anio, Monto)
          ├─ Caja (MontoInicial, MontoTotal, Estado) ◄── CajaTotalMedioPago ──► MedioPago
          ├─ ItemCatalogo (Nombre, Activo) ─┬─ MedioPago
          │                                 ├─ MotivoEgreso
          │                                 └─ GrupoEgreso
          └─ Auditoria (Usuario, Accion, EntidadAfectada, EntidadId, Fecha, Detalle)
```

- **Ids:** el servicio pide el próximo id al repositorio (`ObtenerProximoId`) y se lo pasa al constructor de la entidad.
- **Edición:** el servicio construye una entidad aparte con los datos nuevos (así se validan todos) y recién después
  los pasa a la guardada; si algún dato es inválido, nada cambia.
- **Polimorfismo:** `Movimiento.AfectarCaja` hace los controles comunes y delega en `Aplicar`, que implementa cada
  subclase (el ingreso acredita, el egreso debita). `Anular` hace lo mismo con `Revertir`.
- **`MovimientoRegistrador`** es el único lugar que aplica (o anula) un movimiento en la caja, guarda y audita.
  Lo usan `IngresoService`, `EgresoService`, `CuotaService` (cobro de cuotas) y `MovimientoService` (anulación).
- **Cada movimiento guarda la caja a la que se aplicó** (`Movimiento.Caja`). Los listados y el resumen de una caja
  filtran por esa referencia, no por fechas.
- **Anular movimientos:** solo los de la caja abierta, con motivo obligatorio y una sola vez. El movimiento queda
  marcado como anulado, fuera de los totales. Si era el cobro de cuotas, esas cuotas vuelven a `Pendiente`.
- **`Caja.MontoTotal`**: con la caja abierta es el monto esperado; `Cerrar(fecha, montoContado)` lo reemplaza por el
  contado. Un egreso (o anular un ingreso) no puede dejar la caja en negativo.
- **Cobro de cuotas:** un solo `Ingreso(CuotaSocial)` por el total con un `DetalleIngresoCuota` por cuota. Sin
  responsable, las cuotas son de un solo socio; un responsable puede pagar juntas las suyas y las de sus socios a
  cargo, eligiendo cuáles (queda en `DetalleIngresoCuota.PagadaPor`). **No hay recargo** por pagar tarde.
- **Morosos:** `ObtenerDeudores` agrupa las cuotas pendientes por socio y ordena por quien paga, así cada familia
  queda junta.
- **Valor de la cuota:** `ValorCuota` guarda el valor de cada año; "Generar cuotas para todos" se la crea a cada
  socio activo que no la tenga.
- **Socios:** baja lógica (`Activo`). Un responsable con socios a cargo no puede dejar de serlo ni darse de baja hasta
  reasignarlos. Un `Responsable` no puede tener responsable.
- **Catálogos:** medios de pago, motivos y grupos de egreso heredan de `ItemCatalogo`; los inactivos no se ofrecen
  ni se aceptan en movimientos nuevos.
- **Grupo de egreso:** a quién corresponde el gasto (Estudiantes, Administración, Otros), elegido aparte del motivo.
- **Auditoría:** cada servicio llama a `AuditoriaService.Registrar` con un detalle de qué cambió (`DescripcionCambios`).
- **Login:** 5 intentos fallidos seguidos bloquean ese usuario por 5 minutos (`Configuracion`).
- **Multirol:** un usuario tiene uno o más roles. Un administrador no puede quitarse el rol de Administrador.
- **Permisos:** Operador = socios, cuotas, morosos, donaciones y subsidios; Contador = caja, egresos y movimientos;
  Administrador = usuarios, catálogos y auditoría.

## Tests
`Tests/CooperadoraISFT94.Tests` (xUnit) prueba el dominio y los servicios: `dotnet test CooperadoraISFT94.slnx`.
`Contexto` arma servicios con repositorios propios (los constructores de los repositorios en memoria son `internal`,
visibles para los tests) y un reloj fijo.

## Diferencias deliberadas con el UML
| UML | Código | Motivo |
|---|---|---|
| `Usuario` sin nombre de usuario | `Usuario.NombreUsuario` (único, no se cambia) | Hace falta un identificador para iniciar sesión. |
| `Usuario.password` | `PasswordHash` (PBKDF2) | Nunca se guarda la contraseña en texto plano. |
| `AfectarCaja()` sin parámetros | `AfectarCaja(Caja, CajaTotalMedioPago)` | Se le pasa la caja destino en vez de usar estado global. |
| Movimiento sin caja | `Movimiento.Caja` | Ubicar los movimientos de una caja por fechas es frágil. |
| — | `Socio.Responsable` | Relación opcional entre socios pedida después del UML. |
| — | Clase base `Persona` | `Usuario` y `Socio` comparten nombre, apellido y su validación. |
| — | `ValorCuota`, `ItemCatalogo`, anulación de movimientos | Valor de cuota por año, catálogos administrables y corrección de errores de carga sin borrar datos. |
