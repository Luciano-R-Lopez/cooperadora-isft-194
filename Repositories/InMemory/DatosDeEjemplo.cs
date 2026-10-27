using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.DTOs.Egresos;
using CooperadoraISFT94.DTOs.Ingresos;
using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Services;

namespace CooperadoraISFT94.Repositories.InMemory
{
    /// <summary>
    /// Simula unos dos años y medio de uso de la cooperadora para tener datos de prueba realistas: socios con
    /// responsables, altas y bajas, cuotas de varios años (pagadas, pendientes y pagadas por el responsable),
    /// una caja por mes con cobros, donaciones, subsidios y egresos, la caja de hoy abierta y la auditoría de todo.
    /// <para>
    /// Usa los servicios reales (con un reloj simulado), así los datos cumplen todas las reglas del negocio.
    /// Es determinístico: siempre genera lo mismo para la misma fecha de hoy. Se elimina al conectar la base de datos.
    /// </para>
    /// </summary>
    internal sealed class DatosDeEjemplo
    {
        private static readonly DateTime Inicio = new(2024, 3, 1);

        private static readonly (string Nombre, string Apellido)[] Personas =
        {
            ("Lucía", "Fernández"), ("Martín", "Gómez"), ("Sofía", "Pérez"), ("Mariana", "López"), ("Tomás", "López"),
            ("Valentina", "López"), ("Jorge", "Rodríguez"), ("Camila", "Rodríguez"), ("Graciela", "Sosa"),
            ("Matías", "Sosa"), ("Agustina", "Sosa"), ("Benjamín", "Sosa"), ("Ricardo", "Benítez"), ("Julieta", "Benítez"),
            ("Silvia", "Romero"), ("Nicolás", "Romero"), ("Claudio", "Acosta"), ("Florencia", "Acosta"),
            ("Diego", "Álvarez"), ("Paula", "Medina"), ("Gonzalo", "Herrera"), ("Carolina", "Suárez"),
            ("Federico", "Aguirre"), ("Romina", "Giménez"), ("Lautaro", "Molina"), ("Milagros", "Castro"),
            ("Facundo", "Ortiz"), ("Micaela", "Silva"), ("Emiliano", "Núñez"), ("Brenda", "Luna"),
            ("Hernán", "Juárez"), ("Natalia", "Cabrera"), ("Ezequiel", "Ríos"), ("Daiana", "Ferreyra"),
            ("Leandro", "Godoy"), ("Aldana", "Morales"), ("Maximiliano", "Domínguez"), ("Rocío", "Quiroga")
        };

        /// <summary>Índice en <see cref="Personas"/> del responsable de cada familia y de sus socios a cargo.</summary>
        private static readonly (int Responsable, int[] ACargo)[] Familias =
        {
            (1, new[] { 2 }),          // Martín Gómez responde por Sofía Pérez
            (3, new[] { 4, 5 }),       // Mariana López por Tomás y Valentina
            (6, new[] { 7 }),          // Jorge Rodríguez por Camila
            (8, new[] { 9, 10, 11 }),  // Graciela Sosa por Matías, Agustina y Benjamín
            (12, new[] { 13 }),        // Ricardo Benítez por Julieta
            (14, new[] { 15 }),        // Silvia Romero por Nicolás
            (16, new[] { 17 })         // Claudio Acosta por Florencia
        };

        /// <summary>Socios que se inscriben más tarde (no tienen las cuotas de los primeros años).</summary>
        private static readonly int[] SeInscribenDespues = { 34, 35, 36, 37 };

        /// <summary>Socios que se dan de baja en algún momento.</summary>
        private static readonly int[] SeDanDeBaja = { 20, 27, 31 };

        private readonly Random _azar = new(194);
        private readonly DateTime _hoy = DateTime.Now;
        private DateTime _ahora;

        private readonly int _operadorId;
        private readonly int _operador2Id;
        private readonly int _contadorId;
        private readonly int _adminId;

        private readonly SocioService _socios;
        private readonly CuotaService _cuotas;
        private readonly CajaService _cajas;
        private readonly IngresoService _ingresos;
        private readonly EgresoService _egresos;
        private readonly MovimientoService _movimientos;
        private readonly CatalogoService _catalogos;
        private readonly UsuarioService _usuarios;

        private readonly Dictionary<int, int> _idSocio = new();
        private readonly Dictionary<int, int> _responsableDe = new();

        private DatosDeEjemplo(int adminId, int operadorId, int contadorId)
        {
            _adminId = adminId;
            _operadorId = operadorId;
            _contadorId = contadorId;
            _ahora = Inicio;

            DateTime Reloj() => _ahora;
            var auditoria = new AuditoriaService(AuditoriaRepository.Instancia, UsuarioRepository.Instancia, Reloj);
            var registrador = new MovimientoRegistrador(MovimientoRepository.Instancia, CajaRepository.Instancia,
                                                        CajaTotalMedioPagoRepository.Instancia, auditoria, Reloj);

            _socios = new SocioService(SocioRepository.Instancia, auditoria);
            _cuotas = new CuotaService(CuotaRepository.Instancia, SocioRepository.Instancia, UsuarioRepository.Instancia,
                                       MedioPagoRepository.Instancia, DetalleIngresoCuotaRepository.Instancia,
                                       ValorCuotaRepository.Instancia, registrador, auditoria, Reloj);
            _cajas = new CajaService(CajaRepository.Instancia, MovimientoRepository.Instancia,
                                     CajaTotalMedioPagoRepository.Instancia, auditoria, Reloj);
            _ingresos = new IngresoService(UsuarioRepository.Instancia, MedioPagoRepository.Instancia, registrador, Reloj);
            _egresos = new EgresoService(UsuarioRepository.Instancia, MedioPagoRepository.Instancia,
                                         MotivoEgresoRepository.Instancia, GrupoEgresoRepository.Instancia, registrador, Reloj);
            _movimientos = new MovimientoService(MovimientoRepository.Instancia, DetalleIngresoCuotaRepository.Instancia,
                                                 MedioPagoRepository.Instancia, CajaRepository.Instancia,
                                                 UsuarioRepository.Instancia, CuotaRepository.Instancia, GrupoEgresoRepository.Instancia,
                                                 registrador, auditoria);
            _catalogos = new CatalogoService(MedioPagoRepository.Instancia, MotivoEgresoRepository.Instancia,
                                             GrupoEgresoRepository.Instancia, auditoria);
            _usuarios = new UsuarioService(UsuarioRepository.Instancia, auditoria);

            // Usuario con dos roles: atiende socios y cuotas, y también maneja la caja.
            _operador2Id = _usuarios.Registrar(
                new UsuarioCrearDto("pablo.paz", "Pablo", "Paz", "operador123", true, [Rol.Operador, Rol.Contador]), _adminId).Id;
        }

        /// <param name="adminId">Usuarios que ya existen (los de prueba con contraseña conocida).</param>
        public static void Cargar(int adminId, int operadorId, int contadorId) =>
            new DatosDeEjemplo(adminId, operadorId, contadorId).Simular();

        private void Simular()
        {
            InscribirSociosIniciales();

            // Una caja por mes, desde marzo de 2024 hasta el mes pasado; el mes actual, hasta ayer.
            for (var mes = Inicio; mes < new DateTime(_hoy.Year, _hoy.Month, 1); mes = mes.AddMonths(1))
                SimularMes(mes, mes.AddMonths(1).AddDays(-1));

            var inicioMesActual = new DateTime(_hoy.Year, _hoy.Month, 1);
            if (_hoy.Day > 1)
                SimularMes(inicioMesActual, _hoy.Date.AddDays(-1));

            SimularHoy();
        }

        private void InscribirSociosIniciales()
        {
            _ahora = Inicio.AddHours(9);

            // Primero los responsables, para poder asignarlos.
            foreach (var (responsable, _) in Familias)
                Inscribir(responsable, Categoria.Responsable);

            for (var i = 0; i < Personas.Length; i++)
            {
                if (_idSocio.ContainsKey(i) || SeInscribenDespues.Contains(i)) continue;
                Inscribir(i, Categoria.Socio);
            }
        }

        private void Inscribir(int indice, Categoria categoria)
        {
            var (nombre, apellido) = Personas[indice];
            var familia = Familias.FirstOrDefault(f => f.ACargo.Contains(indice));
            int? responsableId = familia.ACargo is null ? null : _idSocio[familia.Responsable];

            var tieneTelefono = indice % 3 != 2;
            var tieneEmail = indice % 4 != 3;
            var email = $"{Simplificar(nombre)}.{Simplificar(apellido)}@mail.com";

            var socio = _socios.Registrar(new SocioCrearDto(
                nombre, apellido, (28_000_000 + indice * 377_123).ToString(),
                tieneTelefono ? $"11{_azar.Next(40_000_000, 70_000_000)}" : "",
                tieneEmail ? email : "", categoria, responsableId), Operador());

            _idSocio[indice] = socio.Id;
            if (responsableId is int r)
                _responsableDe[socio.Id] = r;
        }

        private static string Simplificar(string texto) =>
            new(texto.ToLowerInvariant().Normalize(System.Text.NormalizationForm.FormD)
                     .Where(c => char.IsAsciiLetter(c)).ToArray());

        private void SimularMes(DateTime desde, DateTime hasta)
        {
            // En marzo se fija el valor del año y se generan las cuotas a todos los socios activos.
            if (desde.Month == 3 && desde.Day == 1)
            {
                _ahora = desde.AddHours(8);
                var valor = desde.Year switch { 2024 => 4000m, 2025 => 4500m, _ => 5000m };
                _cuotas.GenerarParaTodos(desde.Year, valor, Operador());
            }

            EventosDelCalendario(desde);

            _ahora = desde.AddHours(9);
            AbrirCaja();

            var eventos = new List<(DateTime Cuando, Action Accion)>();
            var dias = (hasta - desde).Days + 1;
            DateTime Momento() => desde.AddDays(_azar.Next(dias)).AddHours(9 + _azar.Next(9)).AddMinutes(_azar.Next(60));

            foreach (var cuota in CuotasPendientes())
            {
                if (_azar.NextDouble() < ProbabilidadDePago(cuota, desde))
                    eventos.Add((Momento(), () => Cobrar(cuota)));
            }

            if (_azar.NextDouble() < 0.6)
                eventos.Add((Momento(), () => Ingreso(CategoriaIngreso.Donacion, Redondear(_azar.Next(5_000, 60_000), 500))));
            if (desde.Month is 4 or 8 or 11)
                eventos.Add((Momento(), () => Ingreso(CategoriaIngreso.Subsidio, 150_000m)));
            // Servicios (luz, internet...) todos los meses, y otros gastos variables.
            eventos.Add((desde.AddDays(Math.Min(9, dias - 1)).AddHours(11), () =>
                Egreso("Servicios", "Administración", Redondear(_azar.Next(18_000, 32_000), 100))));
            for (var i = 0; i < 3 + _azar.Next(3); i++)
                eventos.Add((Momento(), () => Egreso()));

            foreach (var (cuando, accion) in eventos.OrderBy(e => e.Cuando))
            {
                _ahora = cuando;
                accion();
            }

            _ahora = hasta.AddHours(19);
            CerrarCaja();
        }

        /// <summary>Altas, bajas y cambios puntuales en fechas fijas.</summary>
        private void EventosDelCalendario(DateTime mes)
        {
            if (mes == new DateTime(2024, 9, 1))
            {
                _ahora = mes.AddDays(2).AddHours(10);
                _catalogos.Agregar(TipoCatalogo.MedioPago, "Mercado Pago", _adminId);
                _catalogos.Agregar(TipoCatalogo.MotivoEgreso, "Limpieza", _adminId);
            }

            if (mes == new DateTime(2025, 6, 1))
            {
                // Nuevos socios a mitad de año: se les genera la cuota del año en curso.
                _ahora = mes.AddDays(4).AddHours(11);
                foreach (var indice in SeInscribenDespues)
                {
                    Inscribir(indice, Categoria.Socio);
                    _cuotas.Generar(new CuotaCrearDto(_idSocio[indice], 4500m, 2025), Operador());
                }
            }

            if (mes == new DateTime(2025, 8, 1))
            {
                _ahora = mes.AddDays(6).AddHours(10);
                foreach (var indice in SeDanDeBaja)
                    _socios.CambiarEstado(_idSocio[indice], false, Operador());

                // Un usuario que dejó la cooperadora.
                var ex = _usuarios.Registrar(
                    new UsuarioCrearDto("ex.tesorero", "Raúl", "Vera", "contador123", true, [Rol.Contador]), _adminId);
                _usuarios.CambiarEstado(ex.Id, false, _adminId);
            }

            if (mes == new DateTime(2025, 10, 1))
            {
                _ahora = mes.AddDays(1).AddHours(12);
                var cheque = _catalogos.ObtenerMediosPago().First(m => m.Nombre == "Cheque");
                _catalogos.CambiarEstado(TipoCatalogo.MedioPago, cheque.Id, false, _adminId);
            }

            if (mes == new DateTime(2026, 2, 1))
            {
                // Corrección de datos de un socio (queda en la auditoría).
                _ahora = mes.AddDays(9).AddHours(16);
                var socio = _socios.ObtenerPorId(_idSocio[0]);
                _socios.Editar(new SocioEditarDto(socio.Id, socio.Nombre, socio.Apellido, socio.Dni, "1144448888",
                                                  "lucia.fernandez@correo.com.ar", socio.Categoria, socio.ResponsableId), Operador());
            }
        }

        /// <summary>
        /// La cuota del año se paga más en los primeros meses; las de años anteriores, de a poco.
        /// Algunas quedan sin pagar (morosos).
        /// </summary>
        private static double ProbabilidadDePago(Cuota cuota, DateTime mes)
        {
            if (!cuota.Socio.Activo) return 0.02;
            if (cuota.Anio < mes.Year) return 0.07;
            return mes.Month switch { 3 => 0.30, 4 => 0.25, _ => 0.10 };
        }

        private IEnumerable<Cuota> CuotasPendientes() =>
            CuotaRepository.Instancia.ObtenerPendientes(null).ToList();

        private void Cobrar(Cuota cuota)
        {
            if (cuota.EstaPagada) return;

            // A los socios a cargo, casi siempre les paga su responsable.
            int? pagador = _responsableDe.TryGetValue(cuota.Socio.Id, out var r) && _azar.NextDouble() < 0.7 ? r : null;
            _cuotas.RegistrarPago(cuota.Id, MedioCobro(), Operador(), pagador);
        }

        private void Ingreso(CategoriaIngreso categoria, decimal monto)
        {
            var medio = categoria == CategoriaIngreso.Subsidio ? Medio("Transferencia") : MedioCobro();
            _ingresos.Registrar(new IngresoCrearDto(monto, categoria, medio), Operador());
        }

        /// <summary>Gasto de la caja; sin monto, uno al azar acotado a lo que hay. Nunca deja la caja en negativo.</summary>
        private void Egreso(string? motivoNombre = null, string? grupoNombre = null, decimal? monto = null)
        {
            var disponible = CajaRepository.Instancia.ObtenerActiva()!.MontoTotal;
            if (disponible < 10_000m) return;

            var importe = Math.Min(monto ?? Redondear(_azar.Next(3_000, (int)Math.Min(60_000m, disponible * 0.35m)), 100),
                                   Redondear((int)(disponible * 0.9m), 100));
            var motivos = _catalogos.ObtenerMotivosEgreso();
            var motivo = motivoNombre is null
                ? motivos[_azar.Next(motivos.Count)]
                : motivos.First(m => m.Nombre == motivoNombre);
            // Un mismo motivo (por ejemplo, mantenimiento) puede ser un gasto de los estudiantes o de la administración.
            var nombreGrupo = grupoNombre ?? _azar.NextDouble() switch { < 0.45 => "Estudiantes", < 0.85 => "Administración", _ => "Otros" };
            var grupo = _catalogos.ObtenerGruposEgreso().First(g => g.Nombre == nombreGrupo);
            var medio = _azar.NextDouble() < 0.7 ? Medio("Efectivo") : Medio("Transferencia");
            _egresos.Registrar(new EgresoCrearDto(importe, motivo.Id, grupo.Id, medio), _contadorId);
        }

        private void AbrirCaja()
        {
            // El efectivo contado al cerrar la caja anterior es el saldo inicial de la siguiente.
            var anterior = CajaRepository.Instancia.ObtenerTodos().LastOrDefault();
            _cajas.Abrir(new AbrirCajaDto(anterior?.MontoTotal ?? 20_000m), _contadorId);
        }

        private void CerrarCaja()
        {
            var esperado = _cajas.ObtenerResumenActivo(null).MontoEsperado;

            // De vez en cuando el conteo no coincide con lo esperado.
            var diferencia = _azar.Next(6) switch { 0 => -500m, 1 => 200m, _ => 0m };
            _cajas.Cerrar(new CerrarCajaDto(Math.Max(0, esperado + diferencia)), _contadorId);
        }

        /// <summary>La caja de hoy queda abierta, con algunos movimientos y uno anulado.</summary>
        private void SimularHoy()
        {
            var apertura = _hoy.AddHours(-3);
            _ahora = apertura;
            AbrirCaja();

            var pendientes = CuotasPendientes().Where(c => c.Socio.Activo).Take(3).ToList();
            var paso = TimeSpan.FromMinutes(25);

            foreach (var cuota in pendientes)
            {
                _ahora += paso;
                Cobrar(cuota);
            }

            _ahora += paso;
            Ingreso(CategoriaIngreso.Donacion, 15_000m);

            _ahora += paso;
            var duplicada = _ingresos.Registrar(new IngresoCrearDto(15_000m, CategoriaIngreso.Donacion, Medio("Efectivo")), Operador());

            _ahora += TimeSpan.FromMinutes(5);
            _movimientos.Anular(duplicada.Id, "Donación cargada dos veces", _contadorId);

            _ahora += paso;
            Egreso();
        }

        /// <summary>Medio de cobro de los activos: más efectivo y transferencias que el resto.</summary>
        private int MedioCobro()
        {
            var medios = _catalogos.ObtenerMediosPago();
            var tirada = _azar.NextDouble();
            var nombre = tirada switch
            {
                < 0.45 => "Efectivo",
                < 0.75 => "Transferencia",
                < 0.90 => "Mercado Pago",
                < 0.96 => "Tarjeta",
                _ => "Cheque"
            };
            return (medios.FirstOrDefault(m => m.Nombre == nombre) ?? medios.First(m => m.Nombre == "Efectivo")).Id;
        }

        private int Medio(string nombre) => _catalogos.ObtenerMediosPago().First(m => m.Nombre == nombre).Id;

        /// <summary>Los operadores se reparten la atención.</summary>
        private int Operador() => _azar.NextDouble() < 0.7 ? _operadorId : _operador2Id;

        private static decimal Redondear(int monto, int multiplo) => monto / multiplo * multiplo;
    }
}
