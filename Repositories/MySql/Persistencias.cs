using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using MySqlConnector;

namespace CooperadoraISFT94.Repositories.MySql
{
    /// <summary>Equivalencia entre el enum Rol del código y las filas de la tabla `rol`.</summary>
    internal static class Roles
    {
        private static readonly Dictionary<Rol, int> _ids = new();
        private static readonly Dictionary<int, Rol> _roles = new();

        public static void Cargar(MySqlConnection conexion)
        {
            _ids.Clear();
            _roles.Clear();

            using var comando = new MySqlCommand("SELECT id_rol, nombre FROM rol", conexion);
            using var r = comando.ExecuteReader();
            while (r.Read())
            {
                if (!Enum.TryParse<Rol>(r.Texto("nombre"), true, out var rol))
                    continue;
                var id = r.Entero("id_rol");
                _ids[rol] = id;
                _roles[id] = rol;
            }
        }

        public static int IdDe(Rol rol) =>
            _ids.TryGetValue(rol, out var id) ? id : throw new InvalidOperationException($"La tabla rol no tiene el rol '{rol}'.");

        public static Rol RolDe(int id) =>
            _roles.TryGetValue(id, out var rol) ? rol : throw new InvalidOperationException($"El rol con id {id} no es válido.");
    }

    internal sealed class UsuarioPersistencia : IPersistencia<Usuario>
    {
        public int Insertar(Usuario u) => Sql.Insertar(
            @"INSERT INTO usuario (nombre_usuario, nombre, apellido, password_hash, activo, creado_en, id_rol, creado_por)
              VALUES (@nu, @n, @a, @p, @act, NOW(), @rol, @por)",
            ("@nu", u.NombreUsuario), ("@n", u.Nombre), ("@a", u.Apellido), ("@p", u.PasswordHash),
            ("@act", u.Activo), ("@rol", Roles.IdDe(u.Rol)), ("@por", Sql.UsuarioActual()));

        public void Actualizar(Usuario u) => Sql.Ejecutar(
            @"UPDATE usuario
              SET nombre = @n, apellido = @a, password_hash = @p, activo = @act, id_rol = @rol, actualizado_en = NOW()
              WHERE id_usuario = @id",
            ("@n", u.Nombre), ("@a", u.Apellido), ("@p", u.PasswordHash), ("@act", u.Activo),
            ("@rol", Roles.IdDe(u.Rol)), ("@id", u.Id));

        public void Eliminar(int id) => throw new NotSupportedException("Los usuarios no se eliminan: se desactivan.");
    }

    internal sealed class SocioPersistencia : IPersistencia<Socio>
    {
        private static string Tipo(Socio s) => s.EsResponsable ? "responsable" : "socio";

        private static string? Vacio(string texto) => string.IsNullOrWhiteSpace(texto) ? null : texto;

        public int Insertar(Socio s) => Sql.Insertar(
            @"INSERT INTO socio (dni, nombre, apellido, telefono, email, tipo, id_responsable, estado, fecha_alta, dado_de_alta_por)
              VALUES (@dni, @n, @a, @tel, @mail, @tipo, @resp, 'activo', CURDATE(),
                      COALESCE(@usr, (SELECT MIN(id_usuario) FROM usuario)))",
            ("@dni", s.Dni), ("@n", s.Nombre), ("@a", s.Apellido), ("@tel", Vacio(s.Telefono)),
            ("@mail", Vacio(s.Email)), ("@tipo", Tipo(s)), ("@resp", s.Responsable?.Id), ("@usr", Sql.UsuarioActual()));

        public void Actualizar(Socio s) => Sql.Ejecutar(
            @"UPDATE socio
              SET dni = @dni, nombre = @n, apellido = @a, telefono = @tel, email = @mail, tipo = @tipo, id_responsable = @resp
              WHERE id_socio = @id",
            ("@dni", s.Dni), ("@n", s.Nombre), ("@a", s.Apellido), ("@tel", Vacio(s.Telefono)),
            ("@mail", Vacio(s.Email)), ("@tipo", Tipo(s)), ("@resp", s.Responsable?.Id), ("@id", s.Id));

        public void Eliminar(int id) => throw new NotSupportedException("Los socios no se eliminan: se dan de baja.");
    }

    /// <summary>
    /// En la base la cuota tiene período (fecha) y vencimiento; en el código solo el año.
    /// Se guarda como período 1/1/año y vencimiento 31/12/año.
    /// </summary>
    internal sealed class CuotaPersistencia : IPersistencia<Cuota>
    {
        private static string Estado(Cuota c) => c.EstaPagada ? "pagada" : "pendiente";

        public int Insertar(Cuota c) => Sql.Insertar(
            @"INSERT INTO cuota (id_socio, periodo, importe, fecha_vencimiento, estado)
              VALUES (@s, @per, @imp, @ven, @est)",
            ("@s", c.Socio.Id), ("@per", new DateTime(c.Anio, 1, 1)), ("@imp", c.Monto),
            ("@ven", new DateTime(c.Anio, 12, 31)), ("@est", Estado(c)));

        public void Actualizar(Cuota c) => Sql.Ejecutar(
            "UPDATE cuota SET estado = @est WHERE id_cuota = @id",
            ("@est", Estado(c)), ("@id", c.Id));

        public void Eliminar(int id) => throw new NotSupportedException("Las cuotas no se eliminan.");
    }

    /// <summary>
    /// Caja abierta: efectivo_esperado guarda el monto total que va acumulando.
    /// Al cerrar: efectivo_declarado guarda el monto contado y se calcula la diferencia.
    /// </summary>
    internal sealed class CajaPersistencia : IPersistencia<Caja>
    {
        public int Insertar(Caja c) => Sql.Insertar(
            @"INSERT INTO caja (fecha_apertura, efectivo_inicial, efectivo_esperado, estado, abierta_por)
              VALUES (@fa, @ini, @ini, 'abierta', COALESCE(@usr, (SELECT MIN(id_usuario) FROM usuario)))",
            ("@fa", c.FechaApertura), ("@ini", c.MontoInicial), ("@usr", Sql.UsuarioActual()));

        public void Actualizar(Caja c)
        {
            if (c.EstaAbierta)
            {
                Sql.Ejecutar(
                    "UPDATE caja SET efectivo_esperado = @m WHERE id_caja = @id",
                    ("@m", c.MontoTotal), ("@id", c.Id));
                return;
            }

            Sql.Ejecutar(
                @"UPDATE caja
                  SET fecha_cierre = @fc, estado = 'cerrada', efectivo_declarado = @m,
                      diferencia_efectivo = @m - efectivo_esperado,
                      cerrada_por = COALESCE(@usr, (SELECT MIN(id_usuario) FROM usuario))
                  WHERE id_caja = @id",
                ("@fc", c.FechaCierre), ("@m", c.MontoTotal), ("@usr", Sql.UsuarioActual()), ("@id", c.Id));
        }

        public void Eliminar(int id) => throw new NotSupportedException("Las cajas no se eliminan.");
    }

    /// <summary>
    /// La tabla no tiene id propio (su clave es caja + medio de pago) y guarda ingresos y egresos por separado.
    /// Los dos totales se recalculan desde las tablas ingreso y egreso, así siempre coinciden con los movimientos.
    /// </summary>
    internal sealed class CajaTotalMedioPagoPersistencia : IPersistencia<CajaTotalMedioPago>
    {
        private const string Upsert =
            @"INSERT INTO caja_total_medio_pago (id_caja, id_medio_pago, total_ingresos, total_egresos)
              VALUES (@c, @m,
                      (SELECT COALESCE(SUM(importe), 0) FROM ingreso
                        WHERE id_caja = @c AND id_medio_pago = @m AND estado = 'registrado'),
                      (SELECT COALESCE(SUM(importe), 0) FROM egreso
                        WHERE id_caja = @c AND id_medio_pago = @m AND estado = 'registrado'))
              ON DUPLICATE KEY UPDATE total_ingresos = VALUES(total_ingresos), total_egresos = VALUES(total_egresos)";

        public int Insertar(CajaTotalMedioPago t)
        {
            Guardar(t);
            return 0;
        }

        public void Actualizar(CajaTotalMedioPago t) => Guardar(t);

        public void Eliminar(int id) => throw new NotSupportedException("Los totales por medio de pago no se eliminan.");

        private static void Guardar(CajaTotalMedioPago t) =>
            Sql.Ejecutar(Upsert, ("@c", t.Caja.Id), ("@m", t.MedioPago.Id));
    }

    /// <summary>
    /// Ingresos y egresos son tablas distintas en la base (cada una con su propio id).
    /// La caja es la que está abierta en este momento: el registrador solo guarda movimientos en esa caja.
    /// </summary>
    internal sealed class MovimientoPersistencia : IPersistencia<Movimiento>
    {
        public int Insertar(Movimiento m)
        {
            var caja = CajaRepository.Instancia.ObtenerActiva()
                ?? throw new ReglaNegocioException(Caja.MensajeSinCajaAbierta);

            switch (m)
            {
                case Ingreso i:
                    return Sql.Insertar(
                        @"INSERT INTO ingreso (fecha_hora, importe, concepto, estado, id_medio_pago, id_categoria_ingreso, id_caja, registrado_por)
                          VALUES (@f, @imp, @con, 'registrado', @mp,
                                  (SELECT id_categoria_ingreso FROM categoria_ingreso WHERE nombre = @cat), @caja, @usr)",
                        ("@f", i.Fecha), ("@imp", i.Monto), ("@con", i.Clasificacion), ("@mp", i.MedioPago.Id),
                        ("@cat", i.CategoriaIngreso.ToString()), ("@caja", caja.Id), ("@usr", i.Usuario.Id));

                case Egreso e:
                    return Sql.Insertar(
                        @"INSERT INTO egreso (fecha_hora, importe, concepto, estado, id_medio_pago, id_categoria_egreso, id_caja, registrado_por)
                          VALUES (@f, @imp, @con, 'registrado', @mp, @cat, @caja, @usr)",
                        ("@f", e.Fecha), ("@imp", e.Monto), ("@con", e.Clasificacion), ("@mp", e.MedioPago.Id),
                        ("@cat", e.Motivo.Id), ("@caja", caja.Id), ("@usr", e.Usuario.Id));

                default:
                    throw new NotSupportedException("Tipo de movimiento desconocido.");
            }
        }

        public void Actualizar(Movimiento m) => throw new NotSupportedException("Los movimientos no se modifican.");

        public void Eliminar(int id) => throw new NotSupportedException("Los movimientos no se eliminan.");
    }

    internal sealed class DetalleIngresoCuotaPersistencia : IPersistencia<DetalleIngresoCuota>
    {
        public int Insertar(DetalleIngresoCuota d) => Sql.Insertar(
            "INSERT INTO detalle_ingreso_cuota (id_ingreso, id_cuota, monto_aplicado) VALUES (@i, @c, @m)",
            ("@i", d.Ingreso.Id), ("@c", d.Cuota.Id), ("@m", d.Monto));

        public void Actualizar(DetalleIngresoCuota d) => throw new NotSupportedException("Los detalles no se modifican.");

        public void Eliminar(int id) => throw new NotSupportedException("Los detalles no se eliminan.");
    }

    internal sealed class MedioPagoPersistencia : IPersistencia<MedioPago>
    {
        public int Insertar(MedioPago m) => Sql.Insertar(
            "INSERT INTO medio_pago (nombre, activo) VALUES (@n, 1)", ("@n", m.Nombre));

        public void Actualizar(MedioPago m) => Sql.Ejecutar(
            "UPDATE medio_pago SET nombre = @n WHERE id_medio_pago = @id", ("@n", m.Nombre), ("@id", m.Id));

        public void Eliminar(int id) => throw new NotSupportedException("Los medios de pago no se eliminan.");
    }

    /// <summary>El motivo de egreso del código es la tabla `categoria_egreso`.</summary>
    internal sealed class MotivoEgresoPersistencia : IPersistencia<MotivoEgreso>
    {
        public int Insertar(MotivoEgreso m) => Sql.Insertar(
            @"INSERT INTO categoria_egreso (nombre, activa, creada_por)
              VALUES (@n, 1, COALESCE(@usr, (SELECT MIN(id_usuario) FROM usuario)))",
            ("@n", m.Nombre), ("@usr", Sql.UsuarioActual()));

        public void Actualizar(MotivoEgreso m) => Sql.Ejecutar(
            "UPDATE categoria_egreso SET nombre = @n WHERE id_categoria_egreso = @id", ("@n", m.Nombre), ("@id", m.Id));

        public void Eliminar(int id) => throw new NotSupportedException("Los motivos de egreso no se eliminan.");
    }

    internal sealed class AuditoriaPersistencia : IPersistencia<Auditoria>
    {
        public int Insertar(Auditoria a) => Sql.Insertar(
            @"INSERT INTO auditoria (id_usuario, entidad, id_registro, accion, fecha_hora)
              VALUES (@u, @e, @r, @a, @f)",
            ("@u", a.Usuario.Id), ("@e", a.EntidadAfectada), ("@r", a.EntidadId),
            ("@a", a.Accion.ToString()), ("@f", a.Fecha));

        public void Actualizar(Auditoria a) => throw new NotSupportedException("La auditoría no se modifica.");

        public void Eliminar(int id) => throw new NotSupportedException("La auditoría no se elimina.");
    }
}
