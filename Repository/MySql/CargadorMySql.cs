using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;
using MySqlConnector;

namespace CooperadoraISFT94.Repository.MySql
{
    // Al iniciar, lee la base y arma las entidades en los repositorios en memoria, con los mismos ids.
    // Las entidades se arman con sus constructores y métodos, así pasan por las mismas validaciones que en uso.
    // Las cajas no se copian: se vuelven a abrir, aplicar sus movimientos en orden y cerrar, así sus montos
    // y los totales por medio de pago salen de los movimientos.
    internal static class CargadorMySql
    {
        private const string MensajeSinBase =
            "No se pudo conectar con la base de datos.\n\n"
            + "1. Prenda MySQL en el panel de XAMPP.\n"
            + "2. Revise que exista la base `cooperadora` (se crea importando BaseDeDatos/cooperadora.sql).\n\n"
            + "Detalle: ";

        public static void Cargar()
        {
            try
            {
                using (MySqlConnection conexion = BaseDatos.AbrirConexion())
                {
                    CargarUsuarios(conexion);
                    CargarCatalogos(conexion);
                    CargarSocios(conexion);
                    CargarCuotas(conexion);
                    CargarCajasYMovimientos(conexion);
                    CargarDetallesDePago(conexion);
                    CargarAuditoria(conexion);
                }
            }
            catch (MySqlException ex)
            {
                throw new NegocioException(MensajeSinBase + ex.Message);
            }
        }

        private static void CargarUsuarios(MySqlConnection conexion)
        {
            Dictionary<int, List<Rol>> roles = new Dictionary<int, List<Rol>>();
            foreach ((int usuarioId, Rol rol) in Leer(conexion,
                "SELECT ur.id_usuario, r.nombre FROM usuario_rol ur JOIN rol r ON r.id_rol = ur.id_rol",
                r => (r.GetInt32("id_usuario"), Enum.Parse<Rol>(r.GetString("nombre"), true))))
            {
                if (!roles.ContainsKey(usuarioId))
                {
                    roles[usuarioId] = new List<Rol>();
                }
                roles[usuarioId].Add(rol);
            }

            foreach (Usuario usuario in Leer(conexion,
                "SELECT id_usuario, nombre_usuario, nombre, apellido, password_hash, activo FROM usuario ORDER BY id_usuario",
                r => new Usuario(r.GetInt32("id_usuario"), r.GetString("nombre_usuario"), r.GetString("nombre"),
                    r.GetString("apellido"), r.GetString("password_hash"), r.GetBoolean("activo"),
                    roles.GetValueOrDefault(r.GetInt32("id_usuario")) ?? new List<Rol>())))
            {
                UsuarioRepository.ObtenerInstancia().Agregar(usuario);
            }
        }

        private static void CargarCatalogos(MySqlConnection conexion)
        {
            foreach (MedioPago medio in Leer(conexion, "SELECT id_medio_pago, nombre, activo FROM medio_pago ORDER BY id_medio_pago",
                r => Activar(new MedioPago(r.GetInt32("id_medio_pago"), r.GetString("nombre")), r.GetBoolean("activo"))))
            {
                MedioPagoRepository.ObtenerInstancia().Agregar(medio);
            }
            foreach (MotivoEgreso motivo in Leer(conexion,
                "SELECT id_categoria_egreso, nombre, activa FROM categoria_egreso ORDER BY id_categoria_egreso",
                r => Activar(new MotivoEgreso(r.GetInt32("id_categoria_egreso"), r.GetString("nombre")), r.GetBoolean("activa"))))
            {
                MotivoEgresoRepository.ObtenerInstancia().Agregar(motivo);
            }
            foreach (GrupoEgreso grupo in Leer(conexion,
                "SELECT id_grupo_egreso, nombre, activo FROM grupo_egreso ORDER BY id_grupo_egreso",
                r => Activar(new GrupoEgreso(r.GetInt32("id_grupo_egreso"), r.GetString("nombre")), r.GetBoolean("activo"))))
            {
                GrupoEgresoRepository.ObtenerInstancia().Agregar(grupo);
            }
        }

        // Primero todos activos, después los responsables y recién al final las bajas: un responsable
        // que hoy está dado de baja estaba activo cuando se lo asignó.
        private static void CargarSocios(MySqlConnection conexion)
        {
            SocioRepository socios = SocioRepository.ObtenerInstancia();
            List<(Socio Socio, int? ResponsableId, bool Activo)> filas = Leer(conexion,
                @"SELECT id_socio, dni, nombre, apellido, telefono, email, tipo, id_responsable, estado
                  FROM socio ORDER BY id_socio",
                r => (new Socio(r.GetInt32("id_socio"), r.GetString("nombre"), r.GetString("apellido"), r.GetString("dni"),
                        TextoOpcional(r, "telefono") ?? string.Empty, TextoOpcional(r, "email") ?? string.Empty,
                        Enum.Parse<Categoria>(r.GetString("tipo"), true)),
                      EnteroOpcional(r, "id_responsable"), r.GetString("estado") == "activo"));

            foreach ((Socio socio, int? _, bool _) in filas)
            {
                socios.Agregar(socio);
            }
            foreach ((Socio socio, int? responsableId, bool _) in filas)
            {
                if (responsableId != null)
                {
                    socio.AsignarResponsable(socios.ObtenerExistente(responsableId.Value));
                }
            }
            foreach ((Socio socio, int? _, bool activo) in filas)
            {
                if (!activo)
                {
                    socio.DarDeBaja();
                }
            }
        }

        private static void CargarCuotas(MySqlConnection conexion)
        {
            foreach (ValorCuota valor in Leer(conexion, "SELECT id_valor_cuota, anio, monto FROM valor_cuota ORDER BY id_valor_cuota",
                r => new ValorCuota(r.GetInt32("id_valor_cuota"), r.GetInt32("anio"), r.GetDecimal("monto"))))
            {
                ValorCuotaRepository.ObtenerInstancia().Agregar(valor);
            }

            SocioRepository socios = SocioRepository.ObtenerInstancia();
            foreach (Cuota cuota in Leer(conexion,
                "SELECT id_cuota, id_socio, periodo, importe, estado FROM cuota ORDER BY id_cuota",
                r =>
                {
                    Cuota leida = new Cuota(r.GetInt32("id_cuota"), socios.ObtenerExistente(r.GetInt32("id_socio")),
                        r.GetDecimal("importe"), r.GetDateTime("periodo").Year);
                    if (r.GetString("estado") == "pagada")
                    {
                        leida.MarcarPagada();
                    }
                    return leida;
                }))
            {
                CuotaRepository.ObtenerInstancia().Agregar(cuota);
            }
        }

        private static void CargarCajasYMovimientos(MySqlConnection conexion)
        {
            List<MovimientoGuardado> movimientos = LeerMovimientos(conexion);
            CajaTotalMedioPagoRepository totales = CajaTotalMedioPagoRepository.ObtenerInstancia();

            foreach ((Caja caja, DateTime? cierre, decimal? contado) in Leer(conexion,
                "SELECT id_caja, fecha_apertura, efectivo_inicial, fecha_cierre, efectivo_declarado FROM caja ORDER BY id_caja",
                r => (new Caja(r.GetInt32("id_caja"), r.GetDecimal("efectivo_inicial"), r.GetDateTime("fecha_apertura")),
                      FechaOpcional(r, "fecha_cierre"), DecimalOpcional(r, "efectivo_declarado"))))
            {
                Dictionary<int, CajaTotalMedioPago> totalesDeLaCaja = new Dictionary<int, CajaTotalMedioPago>();
                foreach (EventoDeCaja evento in EventosEnOrden(movimientos.Where(m => m.CajaId == caja.Id)))
                {
                    Movimiento movimiento = evento.Guardado.Movimiento;
                    CajaTotalMedioPago? total = totalesDeLaCaja.GetValueOrDefault(movimiento.MedioPago.Id);
                    if (total == null)
                    {
                        total = new CajaTotalMedioPago(totales.ObtenerProximoId(), caja, movimiento.MedioPago);
                        totalesDeLaCaja[movimiento.MedioPago.Id] = total;
                    }

                    if (evento.EsAnulacion)
                    {
                        movimiento.Anular(evento.Guardado.AnuladoPor!, evento.Fecha, evento.Guardado.MotivoAnulacion!, total);
                    }
                    else
                    {
                        movimiento.AfectarCaja(caja, total);
                    }
                }

                if (cierre != null)
                {
                    caja.Cerrar(cierre.Value, contado ?? caja.MontoTotal);
                }
                CajaRepository.ObtenerInstancia().Agregar(caja);
                foreach (CajaTotalMedioPago total in totalesDeLaCaja.Values)
                {
                    totales.Agregar(total);
                }
            }

            foreach (MovimientoGuardado guardado in movimientos.OrderBy(m => m.Movimiento.Id))
            {
                MovimientoRepository.ObtenerInstancia().Agregar(guardado.Movimiento);
            }
        }

        private static List<MovimientoGuardado> LeerMovimientos(MySqlConnection conexion)
        {
            UsuarioRepository usuarios = UsuarioRepository.ObtenerInstancia();
            MedioPagoRepository medios = MedioPagoRepository.ObtenerInstancia();

            List<MovimientoGuardado> ingresos = Leer(conexion,
                @"SELECT i.id_ingreso AS id, i.registrado_por, i.fecha_hora, i.importe, i.id_medio_pago, c.nombre AS categoria,
                         i.id_caja, i.estado, i.fecha_anulacion, i.anulado_por, i.motivo_anulacion
                  FROM ingreso i JOIN categoria_ingreso c ON c.id_categoria_ingreso = i.id_categoria_ingreso",
                r => LeerAnulacion(r, new Ingreso(r.GetInt32("id"), usuarios.ObtenerExistente(r.GetInt32("registrado_por")),
                    r.GetDateTime("fecha_hora"), r.GetDecimal("importe"), medios.ObtenerExistente(r.GetInt32("id_medio_pago")),
                    Enum.Parse<CategoriaIngreso>(r.GetString("categoria"), true))));

            List<MovimientoGuardado> egresos = Leer(conexion,
                @"SELECT id_egreso AS id, registrado_por, fecha_hora, importe, id_medio_pago, id_categoria_egreso, id_grupo_egreso,
                         id_caja, estado, fecha_anulacion, anulado_por, motivo_anulacion
                  FROM egreso",
                r => LeerAnulacion(r, new Egreso(r.GetInt32("id"), usuarios.ObtenerExistente(r.GetInt32("registrado_por")),
                    r.GetDateTime("fecha_hora"), r.GetDecimal("importe"), medios.ObtenerExistente(r.GetInt32("id_medio_pago")),
                    MotivoEgresoRepository.ObtenerInstancia().ObtenerExistente(r.GetInt32("id_categoria_egreso")),
                    GrupoEgresoRepository.ObtenerInstancia().ObtenerExistente(r.GetInt32("id_grupo_egreso")))));

            return ingresos.Concat(egresos).ToList();
        }

        private static MovimientoGuardado LeerAnulacion(MySqlDataReader r, Movimiento movimiento)
        {
            MovimientoGuardado guardado = new MovimientoGuardado(movimiento, r.GetInt32("id_caja"));
            if (r.GetString("estado") == "anulado")
            {
                int? anuladoPor = EnteroOpcional(r, "anulado_por");
                guardado.FechaAnulacion = FechaOpcional(r, "fecha_anulacion") ?? movimiento.Fecha;
                guardado.AnuladoPor = anuladoPor != null
                    ? UsuarioRepository.ObtenerInstancia().ObtenerExistente(anuladoPor.Value)
                    : movimiento.Usuario;
                guardado.MotivoAnulacion = TextoOpcional(r, "motivo_anulacion") ?? "(sin motivo)";
            }
            return guardado;
        }

        // Registros y anulaciones en el orden en que pasaron. A igual hora, primero el que tiene menor id
        // (los ids se dan en orden) y la anulación de un movimiento siempre después de su registro.
        private static IEnumerable<EventoDeCaja> EventosEnOrden(IEnumerable<MovimientoGuardado> movimientos)
        {
            List<EventoDeCaja> eventos = new List<EventoDeCaja>();
            foreach (MovimientoGuardado guardado in movimientos)
            {
                eventos.Add(new EventoDeCaja(guardado, guardado.Movimiento.Fecha, false));
                if (guardado.FechaAnulacion != null)
                {
                    eventos.Add(new EventoDeCaja(guardado, guardado.FechaAnulacion.Value, true));
                }
            }
            return eventos.OrderBy(e => e.Fecha).ThenBy(e => e.EsAnulacion).ThenBy(e => e.Guardado.Movimiento.Id);
        }

        private static void CargarDetallesDePago(MySqlConnection conexion)
        {
            SocioRepository socios = SocioRepository.ObtenerInstancia();
            foreach (DetalleIngresoCuota detalle in Leer(conexion,
                @"SELECT id_detalle_ingreso_cuota, id_ingreso, id_cuota, monto_aplicado, pagada_por
                  FROM detalle_ingreso_cuota ORDER BY id_detalle_ingreso_cuota",
                r =>
                {
                    int? pagadaPor = EnteroOpcional(r, "pagada_por");
                    return DetalleIngresoCuota.Restaurar(r.GetInt32("id_detalle_ingreso_cuota"),
                        (Ingreso)MovimientoRepository.ObtenerInstancia().ObtenerExistente(r.GetInt32("id_ingreso")),
                        CuotaRepository.ObtenerInstancia().ObtenerExistente(r.GetInt32("id_cuota")),
                        r.GetDecimal("monto_aplicado"),
                        pagadaPor != null ? socios.ObtenerExistente(pagadaPor.Value) : null);
                }))
            {
                DetalleIngresoCuotaRepository.ObtenerInstancia().Agregar(detalle);
            }
        }

        private static void CargarAuditoria(MySqlConnection conexion)
        {
            foreach (Auditoria registro in Leer(conexion,
                @"SELECT id_auditoria, id_usuario, accion, entidad, id_registro, fecha_hora, detalle
                  FROM auditoria ORDER BY id_auditoria",
                r => new Auditoria(r.GetInt32("id_auditoria"), UsuarioRepository.ObtenerInstancia().ObtenerExistente(r.GetInt32("id_usuario")),
                    Enum.Parse<AccionAuditoria>(r.GetString("accion"), true), r.GetString("entidad"), r.GetInt32("id_registro"),
                    r.GetDateTime("fecha_hora"), TextoOpcional(r, "detalle"))))
            {
                AuditoriaRepository.ObtenerInstancia().Agregar(registro);
            }
        }

        private static List<T> Leer<T>(MySqlConnection conexion, string sql, Func<MySqlDataReader, T> armar)
        {
            List<T> filas = new List<T>();
            using (MySqlCommand comando = new MySqlCommand(sql, conexion))
            using (MySqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    filas.Add(armar(lector));
                }
            }
            return filas;
        }

        private static T Activar<T>(T item, bool activo) where T : ItemCatalogo
        {
            item.Activo = activo;
            return item;
        }

        private static string? TextoOpcional(MySqlDataReader r, string columna)
        {
            return r.IsDBNull(r.GetOrdinal(columna)) ? null : r.GetString(columna);
        }

        private static int? EnteroOpcional(MySqlDataReader r, string columna)
        {
            return r.IsDBNull(r.GetOrdinal(columna)) ? null : r.GetInt32(columna);
        }

        private static decimal? DecimalOpcional(MySqlDataReader r, string columna)
        {
            return r.IsDBNull(r.GetOrdinal(columna)) ? null : r.GetDecimal(columna);
        }

        private static DateTime? FechaOpcional(MySqlDataReader r, string columna)
        {
            return r.IsDBNull(r.GetOrdinal(columna)) ? null : r.GetDateTime(columna);
        }

        // Un movimiento leído, con la caja a la que pertenece y, si se anuló, los datos de la anulación.
        private sealed class MovimientoGuardado
        {
            public MovimientoGuardado(Movimiento movimiento, int cajaId)
            {
                Movimiento = movimiento;
                CajaId = cajaId;
            }

            public Movimiento Movimiento { get; }

            public int CajaId { get; }

            public DateTime? FechaAnulacion { get; set; }

            public Usuario? AnuladoPor { get; set; }

            public string? MotivoAnulacion { get; set; }
        }

        private sealed class EventoDeCaja
        {
            public EventoDeCaja(MovimientoGuardado guardado, DateTime fecha, bool esAnulacion)
            {
                Guardado = guardado;
                Fecha = fecha;
                EsAnulacion = esAnulacion;
            }

            public MovimientoGuardado Guardado { get; }

            public DateTime Fecha { get; }

            public bool EsAnulacion { get; }
        }
    }
}
