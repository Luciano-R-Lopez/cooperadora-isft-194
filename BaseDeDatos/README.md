# Base de datos

`cooperadora.sql` crea la base MySQL/MariaDB de la cooperadora con sus datos iniciales. Es el script de la
rama `feature/mysql-agus` adaptado a la aplicación actual.

La aplicación la usa cuando `Configuracion.UsarBaseDeDatos` es `true` (así viene). Con `false` todo
queda en memoria y, en modo Debug, se cargan datos de ejemplo: sirve para probar sin XAMPP.

## Cómo funciona la conexión

- Al iniciar, `Repository/MySql/CargadorMySql` lee toda la base y arma las entidades en los repositorios
  en memoria, con los mismos ids. Las arma con sus constructores, así pasan por las mismas validaciones.
  Las cajas se vuelven a abrir, aplicar sus movimientos en orden y cerrar, así sus montos salen de los
  movimientos.
- Cada alta o cambio pasa por `RepositorioMySql`: primero se escribe en la base (con las clases de
  `Repository/MySql/Persistencias`) y, si salió bien, en memoria. Los servicios y pantallas no cambian.
- Si MySQL está apagado al abrir la aplicación, avisa y no abre. Si se apaga mientras se usa, el cambio
  que se estaba guardando muestra un aviso.
- **La aplicación tiene que estar abierta en una sola computadora a la vez**: lo que guarda una no lo ve
  la otra hasta reiniciarla, y las dos usarían los mismos ids.
- La conexión es `Server=localhost;Port=3306;Database=cooperadora;User ID=root;Password=;` (el root de
  XAMPP). Para otra contraseña o puerto, definir la variable de entorno `COOPERADORA_CONEXION` con la
  cadena completa.

## Cómo importarla

1. En XAMPP, iniciar MySQL.
2. En phpMyAdmin, crear una base vacía (por ejemplo `cooperadora`, cotejamiento `utf8mb4_general_ci`).
3. Elegir la base → pestaña **Importar** → elegir `cooperadora.sql` → **Continuar**.

Quedan los usuarios `admin` / `admin123`, `operador` / `operador123` y `contador` / `contador123`, y
los mismos catálogos que carga `Datos/DatosIniciales.cs`.

## Qué cambió respecto de `feature/mysql-agus`

| Cambio | Por qué |
| --- | --- |
| Tabla nueva `usuario_rol`; se quitó `usuario.id_rol` | Un usuario puede tener varios roles. |
| Tabla nueva `grupo_egreso` y columna `egreso.id_grupo_egreso` | Cada egreso es de Estudiantes, Administración u Otros, aparte del motivo. |
| `usuario.nombre_usuario` obligatorio | Es con lo que se inicia sesión. |
| `auditoria.accion` es `enum('alta','baja','modificacion')` | Son los valores de `AccionAuditoria`. |
| Largos: nombres 50, usuario 30, DNI 8, teléfono 20, motivo de anulación 200, entidad auditada 50 | Son los que valida el dominio; así la base no acepta lo que la aplicación rechaza. |
| Medios de pago: Efectivo, Transferencia, Tarjeta, Cheque | Los de `DatosIniciales`. |
| Sin socios ni cuotas de ejemplo | Los datos de ejemplo los carga `Datos/DatosDeEjemplo.cs` (solo en Debug). |
| Script limpio (cada tabla con sus claves) en lugar del volcado de phpMyAdmin + `ALTER` | Más fácil de leer y de mantener. |

## Equivalencias con el código

| Clase (`Domain`) | Tabla | Notas |
| --- | --- | --- |
| `Usuario` | `usuario` + `usuario_rol` + `rol` | `Roles` son las filas de `usuario_rol`; `Rol.Administrador` ↔ `'administrador'`. |
| `Socio` | `socio` | `Categoria` ↔ `tipo`; `Activo` ↔ `estado = 'activo'` (la baja guarda `'baja'`, fecha y usuario). `Responsable` ↔ `id_responsable`. |
| `ValorCuota` | `valor_cuota` | |
| `Cuota` | `cuota` | `Anio` ↔ `periodo` = 1/1/año y `fecha_vencimiento` = 31/12/año. `Monto` ↔ `importe`. |
| `MedioPago` | `medio_pago` | |
| `MotivoEgreso` | `categoria_egreso` | `Activo` ↔ `activa`. |
| `GrupoEgreso` | `grupo_egreso` | |
| `CategoriaIngreso` (enum) | `categoria_ingreso` | Se busca por `nombre` (`'CuotaSocial'`, `'Donacion'`, `'Subsidio'`). |
| `Caja` | `caja` | `MontoInicial` ↔ `efectivo_inicial`. `MontoTotal` ↔ `efectivo_esperado` mientras está abierta y `efectivo_declarado` al cerrarla. `abierta_por`/`cerrada_por`: el usuario de la sesión. |
| `CajaTotalMedioPago` | `caja_total_medio_pago` | Sin id propio (clave caja + medio). `Monto` = `total_ingresos - total_egresos`. |
| `Ingreso` | `ingreso` | `Anulado` ↔ `estado = 'anulado'`; `concepto` = `Clasificacion`. |
| `Egreso` | `egreso` | `Motivo` ↔ `id_categoria_egreso`; `Grupo` ↔ `id_grupo_egreso`. Ids desde 1.000.001 para no repetirse con los ingresos. |
| `DetalleIngresoCuota` | `detalle_ingreso_cuota` | `Monto` ↔ `monto_aplicado`; `PagadaPor` ↔ `pagada_por`. |
| `Auditoria` | `auditoria` | `EntidadAfectada` ↔ `entidad`; `EntidadId` ↔ `id_registro`. |

`clave_global` viene del diseño original y la aplicación todavía no la usa.
