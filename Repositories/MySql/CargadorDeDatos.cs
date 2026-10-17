using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using MySqlConnector;

namespace CooperadoraISFT94.Repositories.MySql
{
    /// <summary>
    /// Lee la base al arrancar y llena los repositorios en memoria (en el orden en que las tablas dependen
    /// unas de otras). Después activa la persistencia: desde ese momento cada alta o cambio también se escribe en MySQL.
    /// Lo que el modelo no puede representar (socios dados de baja, movimientos anulados, valores desconocidos) se omite.
    /// </summary>
    internal static class CargadorDeDatos
    {
        public static void Cargar()
        {
            using var cn = BaseDatos.Abrir();

            Roles.Cargar(cn);
            var usuarios = CargarUsuarios(cn);
            var medios = CargarMedios(cn);
            var motivos = CargarMotivos(cn);
            var socios = CargarSocios(cn);
            var cuotas = CargarCuotas(cn, socios);
            var cajas = CargarCajas(cn);
            var ingresos = CargarIngresos(cn, usuarios, medios);
            CargarEgresos(cn, usuarios, medios, motivos);
            CargarTotales(cn, cajas, medios);
            CargarDetalles(cn, ingresos, cuotas);
            CargarAuditoria(cn, usuarios);

            ActivarPersistencia();
        }

        private static Dictionary<int, Usuario> CargarUsuarios(MySqlConnection cn)
        {
            var resultado = new Dictionary<int, Usuario>();
            using var cmd = new MySqlCommand(
                @"SELECT id_usuario, nombre_usuario, nombre, apellido, password_hash, activo, id_rol
                  FROM usuario WHERE nombre_usuario IS NOT NULL ORDER BY id_usuario", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var usuario = new Usuario(r.Texto("nombre_usuario"), r.Texto("nombre"), r.Texto("apellido"),
                                          r.Texto("password_hash"), r.Si("activo"), Roles.RolDe(r.Entero("id_rol")))
                {
                    Id = r.Entero("id_usuario")
                };
                UsuarioRepository.Instancia.Cargar(usuario);
                resultado[usuario.Id] = usuario;
            }
            return resultado;
        }

        private static Dictionary<int, MedioPago> CargarMedios(MySqlConnection cn)
        {
            var resultado = new Dictionary<int, MedioPago>();
            using var cmd = new MySqlCommand(
                "SELECT id_medio_pago, nombre FROM medio_pago WHERE activo = 1 ORDER BY id_medio_pago", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var medio = new MedioPago(r.Texto("nombre")) { Id = r.Entero("id_medio_pago") };
                MedioPagoRepository.Instancia.Cargar(medio);
                resultado[medio.Id] = medio;
            }
            return resultado;
        }

        private static Dictionary<int, MotivoEgreso> CargarMotivos(MySqlConnection cn)
        {
            var resultado = new Dictionary<int, MotivoEgreso>();
            using var cmd = new MySqlCommand(
                "SELECT id_categoria_egreso, nombre FROM categoria_egreso WHERE activa = 1 ORDER BY id_categoria_egreso", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var motivo = new MotivoEgreso(r.Texto("nombre")) { Id = r.Entero("id_categoria_egreso") };
                MotivoEgresoRepository.Instancia.Cargar(motivo);
                resultado[motivo.Id] = motivo;
            }
            return resultado;
        }

        private static Dictionary<int, Socio> CargarSocios(MySqlConnection cn)
        {
            var resultado = new Dictionary<int, Socio>();
            var responsables = new List<(Socio Socio, int ResponsableId)>();

            using (var cmd = new MySqlCommand(
                @"SELECT id_socio, dni, nombre, apellido, telefono, email, tipo, id_responsable
                  FROM socio WHERE estado = 'activo' ORDER BY id_socio", cn))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    var categoria = r.Texto("tipo") == "responsable" ? Categoria.Responsable : Categoria.Socio;
                    var socio = new Socio(r.Texto("nombre"), r.Texto("apellido"), r.Texto("dni"),
                                          r.TextoONulo("telefono") ?? "", r.TextoONulo("email") ?? "", categoria)
                    {
                        Id = r.Entero("id_socio")
                    };
                    SocioRepository.Instancia.Cargar(socio);
                    resultado[socio.Id] = socio;

                    if (r.EnteroONulo("id_responsable") is int responsableId)
                        responsables.Add((socio, responsableId));
                }
            }

            // El responsable se asigna al final, cuando ya están cargados todos los socios.
            foreach (var (socio, responsableId) in responsables)
            {
                if (resultado.TryGetValue(responsableId, out var responsable))
                    socio.AsignarResponsable(responsable);
            }

            return resultado;
        }

        private static Dictionary<int, Cuota> CargarCuotas(MySqlConnection cn, Dictionary<int, Socio> socios)
        {
            var resultado = new Dictionary<int, Cuota>();
            using var cmd = new MySqlCommand(
                "SELECT id_cuota, id_socio, periodo, importe, estado FROM cuota ORDER BY id_cuota", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (!socios.TryGetValue(r.Entero("id_socio"), out var socio))
                    continue;

                var cuota = new Cuota(socio, r.Numero("importe"), r.Fecha("periodo").Year) { Id = r.Entero("id_cuota") };
                if (r.Texto("estado") == "pagada")
                    cuota.CambiarEstado(EstadoCuota.Pagada);

                CuotaRepository.Instancia.Cargar(cuota);
                resultado[cuota.Id] = cuota;
            }
            return resultado;
        }

        private static Dictionary<int, Caja> CargarCajas(MySqlConnection cn)
        {
            var resultado = new Dictionary<int, Caja>();
            using var cmd = new MySqlCommand(
                @"SELECT id_caja, fecha_apertura, fecha_cierre, efectivo_inicial, efectivo_esperado, efectivo_declarado, estado
                  FROM caja ORDER BY id_caja", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var inicial = r.Numero("efectivo_inicial");
                var apertura = r.Fecha("fecha_apertura");
                var esperado = r.NumeroONulo("efectivo_esperado") ?? inicial;

                var caja = new Caja(inicial, apertura) { Id = r.Entero("id_caja") };

                if (r.Texto("estado") == "cerrada")
                {
                    caja.CerrarCaja(r.FechaONula("fecha_cierre") ?? apertura, r.NumeroONulo("efectivo_declarado") ?? esperado);
                }
                else
                {
                    // La caja abierta arranca en su monto inicial: se le suma o resta lo que ya acumuló.
                    var diferencia = esperado - inicial;
                    if (diferencia > 0) caja.Acreditar(diferencia);
                    else if (diferencia < 0) caja.Debitar(-diferencia);
                }

                CajaRepository.Instancia.Cargar(caja);
                resultado[caja.Id] = caja;
            }
            return resultado;
        }

        private static Dictionary<int, Ingreso> CargarIngresos(MySqlConnection cn,
            Dictionary<int, Usuario> usuarios, Dictionary<int, MedioPago> medios)
        {
            var resultado = new Dictionary<int, Ingreso>();
            using var cmd = new MySqlCommand(
                @"SELECT i.id_ingreso, i.fecha_hora, i.importe, i.registrado_por, i.id_medio_pago, c.nombre AS categoria
                  FROM ingreso i
                  JOIN categoria_ingreso c ON c.id_categoria_ingreso = i.id_categoria_ingreso
                  WHERE i.estado = 'registrado'
                  ORDER BY i.fecha_hora, i.id_ingreso", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (!usuarios.TryGetValue(r.Entero("registrado_por"), out var usuario)) continue;
                if (!medios.TryGetValue(r.Entero("id_medio_pago"), out var medio)) continue;
                if (!Enum.TryParse<CategoriaIngreso>(r.Texto("categoria"), true, out var categoria)) continue;

                var ingreso = new Ingreso(usuario, r.Fecha("fecha_hora"), r.Numero("importe"), medio, categoria)
                {
                    Id = r.Entero("id_ingreso")
                };
                MovimientoRepository.Instancia.Cargar(ingreso);
                resultado[ingreso.Id] = ingreso;
            }
            return resultado;
        }

        private static void CargarEgresos(MySqlConnection cn, Dictionary<int, Usuario> usuarios,
            Dictionary<int, MedioPago> medios, Dictionary<int, MotivoEgreso> motivos)
        {
            using var cmd = new MySqlCommand(
                @"SELECT id_egreso, fecha_hora, importe, registrado_por, id_medio_pago, id_categoria_egreso
                  FROM egreso WHERE estado = 'registrado' ORDER BY fecha_hora, id_egreso", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (!usuarios.TryGetValue(r.Entero("registrado_por"), out var usuario)) continue;
                if (!medios.TryGetValue(r.Entero("id_medio_pago"), out var medio)) continue;
                if (!motivos.TryGetValue(r.Entero("id_categoria_egreso"), out var motivo)) continue;

                var egreso = new Egreso(usuario, r.Fecha("fecha_hora"), r.Numero("importe"), medio, motivo)
                {
                    Id = r.Entero("id_egreso")
                };
                MovimientoRepository.Instancia.Cargar(egreso);
            }
        }

        private static void CargarTotales(MySqlConnection cn, Dictionary<int, Caja> cajas, Dictionary<int, MedioPago> medios)
        {
            using var cmd = new MySqlCommand(
                "SELECT id_caja, id_medio_pago, total_ingresos, total_egresos FROM caja_total_medio_pago", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (!cajas.TryGetValue(r.Entero("id_caja"), out var caja)) continue;
                if (!medios.TryGetValue(r.Entero("id_medio_pago"), out var medio)) continue;

                var total = new CajaTotalMedioPago(caja, medio);
                var neto = r.Numero("total_ingresos") - r.Numero("total_egresos");
                if (neto > 0) total.Acreditar(neto);
                else if (neto < 0) total.Debitar(-neto);

                // Sin id propio en la base: el repositorio le asigna uno solo en memoria.
                CajaTotalMedioPagoRepository.Instancia.Cargar(total);
            }
        }

        private static void CargarDetalles(MySqlConnection cn, Dictionary<int, Ingreso> ingresos, Dictionary<int, Cuota> cuotas)
        {
            using var cmd = new MySqlCommand(
                "SELECT id_detalle_ingreso_cuota, id_ingreso, id_cuota, monto_aplicado FROM detalle_ingreso_cuota ORDER BY id_detalle_ingreso_cuota", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (!ingresos.TryGetValue(r.Entero("id_ingreso"), out var ingreso)) continue;
                if (!cuotas.TryGetValue(r.Entero("id_cuota"), out var cuota)) continue;

                var detalle = new DetalleIngresoCuota(ingreso, cuota, r.Numero("monto_aplicado"))
                {
                    Id = r.Entero("id_detalle_ingreso_cuota")
                };
                DetalleIngresoCuotaRepository.Instancia.Cargar(detalle);
            }
        }

        private static void CargarAuditoria(MySqlConnection cn, Dictionary<int, Usuario> usuarios)
        {
            using var cmd = new MySqlCommand(
                "SELECT id_auditoria, id_usuario, entidad, id_registro, accion, fecha_hora FROM auditoria ORDER BY id_auditoria", cn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (!usuarios.TryGetValue(r.Entero("id_usuario"), out var usuario)) continue;
                if (!Enum.TryParse<AccionAuditoria>(r.Texto("accion"), true, out var accion)) continue;

                var auditoria = new Auditoria(usuario, accion, r.Texto("entidad"), r.Entero("id_registro"), r.Fecha("fecha_hora"))
                {
                    Id = r.Entero("id_auditoria")
                };
                AuditoriaRepository.Instancia.Cargar(auditoria);
            }
        }

        private static void ActivarPersistencia()
        {
            UsuarioRepository.Instancia.Persistencia = new UsuarioPersistencia();
            SocioRepository.Instancia.Persistencia = new SocioPersistencia();
            CuotaRepository.Instancia.Persistencia = new CuotaPersistencia();
            CajaRepository.Instancia.Persistencia = new CajaPersistencia();
            CajaTotalMedioPagoRepository.Instancia.Persistencia = new CajaTotalMedioPagoPersistencia();
            MovimientoRepository.Instancia.Persistencia = new MovimientoPersistencia();
            DetalleIngresoCuotaRepository.Instancia.Persistencia = new DetalleIngresoCuotaPersistencia();
            MedioPagoRepository.Instancia.Persistencia = new MedioPagoPersistencia();
            MotivoEgresoRepository.Instancia.Persistencia = new MotivoEgresoPersistencia();
            AuditoriaRepository.Instancia.Persistencia = new AuditoriaPersistencia();
        }
    }
}
