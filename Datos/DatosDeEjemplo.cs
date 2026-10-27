using System.Text;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Datos
{
    // Simula unos dos años y medio de uso de la cooperadora para probar todas las pantallas: socios con responsables,
    // altas y bajas, cuotas de varios años (pagadas, pendientes y pagadas por el responsable), una caja por mes con
    // cobros, donaciones, subsidios y egresos, la caja de hoy abierta y la auditoría de todo.
    // Usa los servicios reales con un reloj simulado, así los datos cumplen todas las reglas del negocio.
    // Siempre genera lo mismo para la misma fecha de hoy. Se carga solo en modo Debug (ver Program).
    internal class DatosDeEjemplo
    {
        private static readonly DateTime Inicio = new DateTime(2024, 3, 1);

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

        // Índice en Personas del responsable de cada familia y de sus socios a cargo.
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

        // Socios que se inscriben más tarde (no tienen las cuotas de los primeros años) y socios que se dan de baja.
        private static readonly int[] SeInscribenDespues = { 34, 35, 36, 37 };
        private static readonly int[] SeDanDeBaja = { 20, 27, 31 };

        private readonly Random azar = new Random(194);
        private readonly DateTime hoy = DateTime.Now;
        private DateTime ahora;

        private readonly int adminId;
        private readonly int operadorId;
        private readonly int contadorId;
        private readonly int operador2Id;

        private readonly SocioService socios;
        private readonly CuotaService cuotas;
        private readonly CajaService cajas;
        private readonly IngresoService ingresos;
        private readonly EgresoService egresos;
        private readonly MovimientoService movimientos;
        private readonly CatalogoService catalogos;
        private readonly UsuarioService usuarios;

        private readonly Dictionary<int, int> idSocio = new Dictionary<int, int>();
        private readonly Dictionary<int, int> responsableDe = new Dictionary<int, int>();

        private DatosDeEjemplo()
        {
            ahora = Inicio;
            Func<DateTime> reloj = () => ahora;

            AuditoriaService auditoria = new AuditoriaService(Repositorios.Auditorias, Repositorios.Usuarios, reloj);
            MovimientoRegistrador registrador = new MovimientoRegistrador(Repositorios.Movimientos, Repositorios.Cajas,
                Repositorios.TotalesMedioPago, auditoria, reloj);

            socios = new SocioService(Repositorios.Socios, auditoria);
            cuotas = new CuotaService(Repositorios.Cuotas, Repositorios.Socios, Repositorios.Usuarios, Repositorios.MediosPago,
                Repositorios.DetallesIngresoCuota, Repositorios.ValoresCuota, registrador, auditoria);
            cajas = new CajaService(Repositorios.Cajas, Repositorios.Movimientos, Repositorios.TotalesMedioPago, auditoria, reloj);
            ingresos = new IngresoService(Repositorios.Usuarios, Repositorios.MediosPago, registrador);
            egresos = new EgresoService(Repositorios.Usuarios, Repositorios.MediosPago, Repositorios.MotivosEgreso,
                Repositorios.GruposEgreso, registrador);
            movimientos = new MovimientoService(Repositorios.Movimientos, Repositorios.DetallesIngresoCuota, Repositorios.Cajas,
                Repositorios.Usuarios, Repositorios.Cuotas, registrador, auditoria);
            catalogos = new CatalogoService(Repositorios.MediosPago, Repositorios.MotivosEgreso, Repositorios.GruposEgreso, auditoria);
            usuarios = new UsuarioService(Repositorios.Usuarios, auditoria);

            adminId = IdUsuario("admin");
            operadorId = IdUsuario("operador");
            contadorId = IdUsuario("contador");

            // Usuario con dos roles: atiende socios y cuotas, y también maneja la caja.
            operador2Id = usuarios.Guardar(NuevoUsuario("pablo.paz", "Pablo", "Paz", "operador123", Rol.Operador, Rol.Contador), adminId).Id;
        }

        // Necesita los catálogos y usuarios de DatosIniciales. Si ya hay socios, no hace nada.
        public static void Cargar()
        {
            if (Repositorios.Socios.ObtenerTodos().Count > 0)
            {
                return;
            }
            new DatosDeEjemplo().Simular();
        }

        private void Simular()
        {
            InscribirSociosIniciales();

            // Una caja por mes desde marzo de 2024 hasta el mes pasado; el mes actual, hasta ayer.
            DateTime inicioMesActual = new DateTime(hoy.Year, hoy.Month, 1);
            for (DateTime mes = Inicio; mes < inicioMesActual; mes = mes.AddMonths(1))
            {
                SimularMes(mes, mes.AddMonths(1).AddDays(-1));
            }
            if (hoy.Day > 1)
            {
                SimularMes(inicioMesActual, hoy.Date.AddDays(-1));
            }

            SimularHoy();
        }

        private void InscribirSociosIniciales()
        {
            ahora = Inicio.AddHours(9);

            // Primero los responsables, para poder asignarlos.
            foreach ((int Responsable, int[] ACargo) familia in Familias)
            {
                Inscribir(familia.Responsable, Categoria.Responsable);
            }
            for (int i = 0; i < Personas.Length; i++)
            {
                if (!idSocio.ContainsKey(i) && !SeInscribenDespues.Contains(i))
                {
                    Inscribir(i, Categoria.Socio);
                }
            }
        }

        private void Inscribir(int indice, Categoria categoria)
        {
            (string nombre, string apellido) = Personas[indice];
            int? responsableId = null;
            foreach ((int Responsable, int[] ACargo) familia in Familias)
            {
                if (familia.ACargo.Contains(indice))
                {
                    responsableId = idSocio[familia.Responsable];
                }
            }

            SocioDTO dto = new SocioDTO();
            dto.Nombre = nombre;
            dto.Apellido = apellido;
            dto.Dni = (28000000 + indice * 377123).ToString();
            dto.Telefono = indice % 3 != 2 ? "11" + azar.Next(40000000, 70000000) : string.Empty;
            dto.Email = indice % 4 != 3 ? Simplificar(nombre) + "." + Simplificar(apellido) + "@mail.com" : string.Empty;
            dto.Categoria = categoria;
            dto.ResponsableId = responsableId;

            int id = socios.Guardar(dto, Operador());
            idSocio[indice] = id;
            if (responsableId.HasValue)
            {
                responsableDe[id] = responsableId.Value;
            }
        }

        private void SimularMes(DateTime desde, DateTime hasta)
        {
            // En marzo se fija el valor del año y se generan las cuotas a todos los socios activos.
            if (desde.Month == 3 && desde.Day == 1)
            {
                ahora = desde.AddHours(8);
                decimal valor = desde.Year == 2024 ? 4000m : desde.Year == 2025 ? 4500m : 5000m;
                cuotas.GenerarParaTodos(desde.Year, valor, Operador());
            }

            EventosDelCalendario(desde);

            ahora = desde.AddHours(9);
            AbrirCaja();

            List<(DateTime Cuando, Action Accion)> eventos = new List<(DateTime Cuando, Action Accion)>();
            int dias = (hasta - desde).Days + 1;
            Func<DateTime> momento = () => desde.AddDays(azar.Next(dias)).AddHours(9 + azar.Next(9)).AddMinutes(azar.Next(60));

            foreach (Cuota cuota in CuotasPendientes())
            {
                if (azar.NextDouble() < ProbabilidadDePago(cuota, desde))
                {
                    eventos.Add((momento(), () => Cobrar(cuota)));
                }
            }
            if (azar.NextDouble() < 0.6)
            {
                decimal donacion = Redondear(azar.Next(5000, 60000), 500);
                eventos.Add((momento(), () => Ingreso(CategoriaIngreso.Donacion, donacion)));
            }
            if (desde.Month == 4 || desde.Month == 8 || desde.Month == 11)
            {
                eventos.Add((momento(), () => Ingreso(CategoriaIngreso.Subsidio, 150000m)));
            }

            // Servicios (luz, internet...) todos los meses, y otros gastos variables.
            decimal servicios = Redondear(azar.Next(18000, 32000), 100);
            eventos.Add((desde.AddDays(Math.Min(9, dias - 1)).AddHours(11), () => Egreso("Servicios", "Administración", servicios)));
            int gastos = 3 + azar.Next(3);
            for (int i = 0; i < gastos; i++)
            {
                eventos.Add((momento(), () => Egreso(null, null, null)));
            }

            foreach ((DateTime Cuando, Action Accion) evento in eventos.OrderBy(e => e.Cuando))
            {
                ahora = evento.Cuando;
                evento.Accion();
            }

            ahora = hasta.AddHours(19);
            CerrarCaja();
        }

        // Altas, bajas y cambios puntuales en fechas fijas.
        private void EventosDelCalendario(DateTime mes)
        {
            if (mes == new DateTime(2024, 9, 1))
            {
                ahora = mes.AddDays(2).AddHours(10);
                catalogos.Guardar(NuevoItem(TipoCatalogo.MedioPago, "Mercado Pago"), adminId);
                catalogos.Guardar(NuevoItem(TipoCatalogo.MotivoEgreso, "Limpieza"), adminId);
            }

            if (mes == new DateTime(2025, 6, 1))
            {
                // Nuevos socios a mitad de año: se les genera la cuota del año en curso.
                ahora = mes.AddDays(4).AddHours(11);
                foreach (int indice in SeInscribenDespues)
                {
                    Inscribir(indice, Categoria.Socio);
                    CuotaDTO cuota = new CuotaDTO();
                    cuota.SocioId = idSocio[indice];
                    cuota.Anio = 2025;
                    cuota.Monto = 4500m;
                    cuotas.Generar(cuota, Operador());
                }
            }

            if (mes == new DateTime(2025, 8, 1))
            {
                ahora = mes.AddDays(6).AddHours(10);
                foreach (int indice in SeDanDeBaja)
                {
                    socios.CambiarEstado(idSocio[indice], false, Operador());
                }

                // Un usuario que dejó la cooperadora.
                UsuarioDTO ex = usuarios.Guardar(NuevoUsuario("ex.tesorero", "Raúl", "Vera", "contador123", Rol.Contador), adminId);
                usuarios.CambiarEstado(ex.Id, false, adminId);
            }

            if (mes == new DateTime(2025, 10, 1))
            {
                ahora = mes.AddDays(1).AddHours(12);
                ItemCatalogoDTO cheque = catalogos.ObtenerItems(TipoCatalogo.MedioPago, false).First(m => m.Nombre == "Cheque");
                catalogos.CambiarEstado(TipoCatalogo.MedioPago, cheque.Id, false, adminId);
            }

            if (mes == new DateTime(2026, 2, 1))
            {
                // Corrección de datos de un socio (queda en la auditoría).
                ahora = mes.AddDays(9).AddHours(16);
                SocioDTO socio = socios.ObtenerPorId(idSocio[0]);
                socio.Telefono = "1144448888";
                socio.Email = "lucia.fernandez@correo.com.ar";
                socios.Guardar(socio, Operador());
            }
        }

        // La cuota del año se paga más en los primeros meses; las de años anteriores, de a poco.
        // Algunas quedan sin pagar (morosos).
        private static double ProbabilidadDePago(Cuota cuota, DateTime mes)
        {
            if (!cuota.Socio.Activo)
            {
                return 0.02;
            }
            if (cuota.Anio < mes.Year)
            {
                return 0.07;
            }
            return mes.Month == 3 ? 0.30 : mes.Month == 4 ? 0.25 : 0.10;
        }

        private static List<Cuota> CuotasPendientes()
        {
            return Repositorios.Cuotas.ObtenerTodos()
                .Where(c => !c.EstaPagada)
                .OrderBy(c => c.Anio)
                .ThenBy(c => c.Socio.Apellido)
                .ThenBy(c => c.Socio.Nombre)
                .ToList();
        }

        private void Cobrar(Cuota cuota)
        {
            if (cuota.EstaPagada)
            {
                return;
            }
            // A los socios a cargo, casi siempre les paga su responsable.
            int? pagador = null;
            if (responsableDe.TryGetValue(cuota.Socio.Id, out int responsable) && azar.NextDouble() < 0.7)
            {
                pagador = responsable;
            }
            cuotas.RegistrarPago(cuota.Id, MedioCobro(), Operador(), pagador);
        }

        private IngresoDTO Ingreso(CategoriaIngreso categoria, decimal monto)
        {
            IngresoDTO dto = new IngresoDTO();
            dto.Monto = monto;
            dto.CategoriaIngreso = categoria;
            dto.MedioPagoId = categoria == CategoriaIngreso.Subsidio ? Medio("Transferencia") : MedioCobro();
            return ingresos.Registrar(dto, Operador());
        }

        // Gasto de la caja; sin monto, uno al azar acotado a lo que hay. Nunca deja la caja en negativo.
        private void Egreso(string? motivoNombre, string? grupoNombre, decimal? monto)
        {
            decimal disponible = Repositorios.Cajas.ObtenerTodos().First(c => c.EstaAbierta).MontoTotal;
            if (disponible < 10000m)
            {
                return;
            }

            decimal importe = monto ?? Redondear(azar.Next(3000, (int)Math.Min(60000m, disponible * 0.35m)), 100);
            importe = Math.Min(importe, Redondear((int)(disponible * 0.9m), 100));

            List<ItemCatalogoDTO> motivos = catalogos.ObtenerItems(TipoCatalogo.MotivoEgreso, false);
            ItemCatalogoDTO motivo = motivoNombre == null ? motivos[azar.Next(motivos.Count)] : motivos.First(m => m.Nombre == motivoNombre);

            // Un mismo motivo (por ejemplo, mantenimiento) puede ser un gasto de los estudiantes o de la administración.
            double tirada = azar.NextDouble();
            string nombreGrupo = grupoNombre ?? (tirada < 0.45 ? "Estudiantes" : tirada < 0.85 ? "Administración" : "Otros");
            ItemCatalogoDTO grupo = catalogos.ObtenerItems(TipoCatalogo.GrupoEgreso, false).First(g => g.Nombre == nombreGrupo);

            EgresoDTO dto = new EgresoDTO();
            dto.Monto = importe;
            dto.MotivoEgresoId = motivo.Id;
            dto.GrupoEgresoId = grupo.Id;
            dto.MedioPagoId = azar.NextDouble() < 0.7 ? Medio("Efectivo") : Medio("Transferencia");
            egresos.Registrar(dto, contadorId);
        }

        private void AbrirCaja()
        {
            // El efectivo contado al cerrar la caja anterior es el saldo inicial de la siguiente.
            Caja? anterior = Repositorios.Cajas.ObtenerTodos().LastOrDefault();
            cajas.Abrir(anterior?.MontoTotal ?? 20000m, contadorId);
        }

        private void CerrarCaja()
        {
            decimal esperado = cajas.ObtenerResumenActivo(null).MontoEsperado;

            // De vez en cuando el conteo no coincide con lo esperado.
            int tirada = azar.Next(6);
            decimal diferencia = tirada == 0 ? -500m : tirada == 1 ? 200m : 0m;
            cajas.Cerrar(Math.Max(0, esperado + diferencia), contadorId);
        }

        // La caja de hoy queda abierta, con algunos movimientos y uno anulado.
        private void SimularHoy()
        {
            ahora = hoy.AddHours(-3);
            AbrirCaja();

            TimeSpan paso = TimeSpan.FromMinutes(25);
            foreach (Cuota cuota in CuotasPendientes().Where(c => c.Socio.Activo).Take(3).ToList())
            {
                ahora += paso;
                Cobrar(cuota);
            }

            ahora += paso;
            Ingreso(CategoriaIngreso.Donacion, 15000m);

            ahora += paso;
            IngresoDTO duplicada = Ingreso(CategoriaIngreso.Donacion, 15000m);

            ahora += TimeSpan.FromMinutes(5);
            movimientos.Anular(duplicada.Id, "Donación cargada dos veces", contadorId);

            ahora += paso;
            Egreso(null, null, null);
        }

        // Medio de cobro: más efectivo y transferencias que el resto.
        private int MedioCobro()
        {
            List<ItemCatalogoDTO> medios = catalogos.ObtenerItems(TipoCatalogo.MedioPago, false);
            double tirada = azar.NextDouble();
            string nombre = tirada < 0.45 ? "Efectivo"
                : tirada < 0.75 ? "Transferencia"
                : tirada < 0.90 ? "Mercado Pago"
                : tirada < 0.96 ? "Tarjeta"
                : "Cheque";
            ItemCatalogoDTO? medio = medios.FirstOrDefault(m => m.Nombre == nombre);
            return (medio ?? medios.First(m => m.Nombre == "Efectivo")).Id;
        }

        private int Medio(string nombre)
        {
            return catalogos.ObtenerItems(TipoCatalogo.MedioPago, false).First(m => m.Nombre == nombre).Id;
        }

        // Los dos operadores se reparten la atención.
        private int Operador()
        {
            return azar.NextDouble() < 0.7 ? operadorId : operador2Id;
        }

        private static int IdUsuario(string nombreUsuario)
        {
            return Repositorios.Usuarios.ObtenerTodos().First(u => u.NombreUsuario == nombreUsuario).Id;
        }

        private static UsuarioDTO NuevoUsuario(string nombreUsuario, string nombre, string apellido, string password, params Rol[] roles)
        {
            UsuarioDTO dto = new UsuarioDTO();
            dto.NombreUsuario = nombreUsuario;
            dto.Nombre = nombre;
            dto.Apellido = apellido;
            dto.Password = password;
            dto.Activo = true;
            dto.Roles = roles.ToList();
            return dto;
        }

        private static ItemCatalogoDTO NuevoItem(TipoCatalogo tipo, string nombre)
        {
            ItemCatalogoDTO dto = new ItemCatalogoDTO();
            dto.Tipo = tipo;
            dto.Nombre = nombre;
            return dto;
        }

        private static string Simplificar(string texto)
        {
            string descompuesto = texto.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            return new string(descompuesto.Where(char.IsAsciiLetter).ToArray());
        }

        private static decimal Redondear(int monto, int multiplo)
        {
            return monto / multiplo * multiplo;
        }
    }
}
