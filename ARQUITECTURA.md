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
         │           └─ Socio    (Categoria)
         ├─ Movimiento (abstracta: Usuario, Fecha, Monto, MedioPago, AfectarCaja())
         │     ├─ Ingreso (CategoriaIngreso)
         │     └─ Egreso  (MotivoEgreso)
         ├─ Cuota (Socio, Monto, Anio, Estado) ◄── DetalleIngresoCuota ──► Ingreso
         ├─ Caja (MontoInicial, MontoTotal, Estado) ◄── CajaTotalMedioPago ──► MedioPago
         ├─ MedioPago, MotivoEgreso   (catálogos: son entidades, no enums)
         └─ Auditoria (Usuario, Accion, EntidadAfectada, EntidadId, Fecha)
```

- **Polimorfismo:** `Movimiento.AfectarCaja` lo implementa cada subclase (el ingreso acredita, el egreso debita).
  Nadie pregunta "¿es ingreso o egreso?" para operar la caja; `Tipo` y `Clasificacion` también son polimórficos.
- **Un solo repositorio de movimientos** (`IMovimientoRepository`): ingresos y egresos comparten id y listado.
- **`IMovimientoRegistrador`** es el único lugar que aplica un movimiento a la caja abierta, guarda y audita.
  Lo usan `IngresoService`, `EgresoService` y `CuotaService` (cobro de cuota).
- **Los movimientos no apuntan a la caja** (como en el UML): pertenecen a la caja cuyo período
  (`Caja.Abarca(fecha)`) los contiene, y su efecto queda en `Caja.MontoTotal` y `CajaTotalMedioPago`.
- **`Caja.MontoTotal`**: con la caja abierta es el monto esperado; `CerrarCaja(fecha, montoTotal)` lo reemplaza por el contado.
  El esperado de una caja cerrada se recalcula como `MontoInicial + Σ CajaTotalMedioPago`.
- **Cobrar una cuota** crea un `Ingreso(CuotaSocial)`, su `DetalleIngresoCuota` y pasa la cuota a `Pagada`.
  Los ingresos de cuota social no se pueden cargar a mano.
- **Auditoría:** cada servicio llama a `IAuditoriaService.Registrar` al dar de alta / modificar / dar de baja
  (desactivar un usuario = `Baja`). Solo la ve el administrador.
- **Módulos por rol:** `Views/Shell/ModulosPorRol.cs`. Sumar un módulo es agregar una línea ahí.

### Agregar un medio de pago o motivo de egreso
Son filas de datos (`MedioPagoRepository` / `MotivoEgresoRepository`), no código: los desplegables los leen
de `ICatalogoService`. Hoy se cargan en `DatosIniciales`; con MySQL, con un INSERT.

### Diferencias deliberadas con el UML
| UML | Código | Motivo |
|---|---|---|
| `Usuario` sin nombre de usuario | `Usuario.NombreUsuario` (único, inmutable) | Hace falta un identificador de login. |
| `Usuario.password` | `PasswordHash` (PBKDF2) | Nunca se guarda la contraseña en texto plano. |
| `AfectarCaja()` sin parámetros | `AfectarCaja(Caja, CajaTotalMedioPago)` | El UML no vincula movimiento y caja; se le pasa la caja destino en vez de usar estado global. |
| — | Clase base `Persona` | `Usuario` y `Socio` compartían nombre/apellido y su validación duplicada. |
| — | `TipoMovimiento` | Valor derivado y polimórfico, solo para filtrar/mostrar. |
