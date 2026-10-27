using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class CuotaService
    {
        private readonly IRepository<Cuota> repositorio;
        private readonly IRepository<Socio> socioRepositorio;
        private readonly IRepository<Usuario> usuarioRepositorio;
        private readonly IRepository<MedioPago> medioRepositorio;
        private readonly IRepository<DetalleIngresoCuota> detalleRepositorio;
        private readonly IRepository<ValorCuota> valorRepositorio;
        private readonly MovimientoRegistrador registrador;
        private readonly AuditoriaService auditoria;

        public CuotaService()
            : this(Repositorios.Cuotas, Repositorios.Socios, Repositorios.Usuarios, Repositorios.MediosPago,
                Repositorios.DetallesIngresoCuota, Repositorios.ValoresCuota, new MovimientoRegistrador(), new AuditoriaService())
        {
        }

        public CuotaService(IRepository<Cuota> repositorio, IRepository<Socio> socioRepositorio,
            IRepository<Usuario> usuarioRepositorio, IRepository<MedioPago> medioRepositorio,
            IRepository<DetalleIngresoCuota> detalleRepositorio, IRepository<ValorCuota> valorRepositorio,
            MovimientoRegistrador registrador, AuditoriaService auditoria)
        {
            this.repositorio = repositorio;
            this.socioRepositorio = socioRepositorio;
            this.usuarioRepositorio = usuarioRepositorio;
            this.medioRepositorio = medioRepositorio;
            this.detalleRepositorio = detalleRepositorio;
            this.valorRepositorio = valorRepositorio;
            this.registrador = registrador;
            this.auditoria = auditoria;
        }

        // De la más reciente a la más antigua.
        public List<CuotaDTO> ObtenerPorSocio(int socioId)
        {
            Socio socio = socioRepositorio.ObtenerExistente(socioId);
            return CuotasDe(socio).OrderByDescending(c => c.Anio).Select(MapearADto).ToList();
        }

        // Genera la cuota de un año para un socio (dto.SocioId, dto.Anio y dto.Monto).
        public CuotaDTO Generar(CuotaDTO dto, int usuarioId)
        {
            Socio socio = socioRepositorio.ObtenerExistente(dto.SocioId);
            socio.ExigirActivo();
            Cuota cuota = new Cuota(repositorio.ObtenerProximoId(), socio, dto.Monto, dto.Anio);
            if (TieneCuota(socio, cuota.Anio))
            {
                throw new NegocioException("El socio ya tiene una cuota para el año " + cuota.Anio + ".");
            }

            repositorio.Agregar(cuota);
            auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Cuota), cuota.Id,
                "Cuota " + cuota.Anio + " de " + socio.ObtenerDescripcion() + " por " + Formato.Monto(cuota.Monto));
            return MapearADto(cuota);
        }

        // Fija el valor del año y genera la cuota a cada socio activo que todavía no la tenga.
        public GeneracionCuotasDTO GenerarParaTodos(int anio, decimal monto, int usuarioId)
        {
            FijarValorCuota(anio, monto, usuarioId);

            GeneracionCuotasDTO resultado = new GeneracionCuotasDTO();
            resultado.Anio = anio;
            resultado.Monto = monto;
            foreach (Socio socio in socioRepositorio.ObtenerTodos().Where(s => s.Activo))
            {
                if (TieneCuota(socio, anio))
                {
                    resultado.YaTenian++;
                    continue;
                }
                Cuota cuota = new Cuota(repositorio.ObtenerProximoId(), socio, monto, anio);
                repositorio.Agregar(cuota);
                auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Cuota), cuota.Id,
                    "Cuota " + anio + " de " + socio.ObtenerDescripcion() + " por " + Formato.Monto(monto) + " (generación del año)");
                resultado.Generadas++;
            }
            return resultado;
        }

        // El valor fijado para el año o, si no se fijó, el de la última cuota generada de ese año.
        public decimal? ObtenerMontoSugerido(int anio)
        {
            ValorCuota? valor = BuscarValor(anio);
            if (valor != null)
            {
                return valor.Monto;
            }
            Cuota? ultima = repositorio.ObtenerTodos().Where(c => c.Anio == anio).OrderByDescending(c => c.Id).FirstOrDefault();
            return ultima?.Monto;
        }

        public void FijarValorCuota(int anio, decimal monto, int usuarioId)
        {
            ValorCuota? valor = BuscarValor(anio);
            if (valor == null)
            {
                valor = new ValorCuota(valorRepositorio.ObtenerProximoId(), anio, monto);
                valorRepositorio.Agregar(valor);
                auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(ValorCuota), valor.Id,
                    "Valor de la cuota " + anio + ": " + Formato.Monto(valor.Monto));
                return;
            }

            decimal anterior = valor.Monto;
            valor.Monto = monto;
            if (anterior == valor.Monto)
            {
                return;
            }
            valorRepositorio.Actualizar(valor);
            auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(ValorCuota), valor.Id,
                new DescripcionCambios().Campo("Valor de la cuota " + anio, anterior, valor.Monto).ToString());
        }

        // pagadorId: el responsable del socio si es quien paga; null si paga el propio socio.
        public ReciboPagoDTO RegistrarPago(int cuotaId, int medioPagoId, int usuarioId, int? pagadorId)
        {
            return RegistrarPagos(new List<int> { cuotaId }, medioPagoId, usuarioId, pagadorId);
        }

        // Cobra varias cuotas juntas en un solo ingreso y un solo recibo. No hay recargo por pagar tarde.
        public ReciboPagoDTO RegistrarPagos(List<int> cuotaIds, int medioPagoId, int usuarioId, int? pagadorId)
        {
            if (cuotaIds == null || cuotaIds.Count == 0)
            {
                throw new NegocioException("Elija al menos una cuota para cobrar.");
            }

            List<Cuota> cuotas = cuotaIds.Distinct().Select(repositorio.ObtenerExistente).OrderBy(c => c.Anio).ToList();
            foreach (Cuota cuota in cuotas)
            {
                cuota.ValidarPagable();
            }
            Usuario usuario = usuarioRepositorio.ObtenerExistente(usuarioId);
            MedioPago medio = medioRepositorio.ObtenerExistente(medioPagoId);
            medio.ExigirActivo();
            Socio? pagador = pagadorId.HasValue ? socioRepositorio.ObtenerExistente(pagadorId.Value) : null;

            // Sin un responsable que pague, un recibo es de un solo socio.
            if (pagador == null && cuotas.Select(c => c.Socio).Distinct().Count() > 1)
            {
                throw new NegocioException("Solo se pueden cobrar juntas cuotas del mismo socio (o las que paga un responsable).");
            }

            // Todo lo que puede fallar por los datos se arma antes de tocar la caja. El registro del ingreso
            // (que exige caja abierta) va último: si falla, las cuotas quedan como estaban.
            Ingreso ingreso = new Ingreso(registrador.ObtenerProximoId(), usuario, registrador.Ahora(), cuotas.Sum(c => c.Monto),
                medio, CategoriaIngreso.CuotaSocial);
            List<DetalleIngresoCuota> detalles = new List<DetalleIngresoCuota>();
            foreach (Cuota cuota in cuotas)
            {
                // El responsable puede pagar sus propias cuotas junto con las de sus socios a cargo.
                Socio? pagadaPor = ReferenceEquals(cuota.Socio, pagador) ? null : pagador;
                detalles.Add(new DetalleIngresoCuota(detalleRepositorio.ObtenerProximoId(), ingreso, cuota, cuota.Monto, pagadaPor));
            }

            registrador.Registrar(ingreso);
            foreach (DetalleIngresoCuota detalle in detalles)
            {
                detalleRepositorio.Agregar(detalle);
                detalle.Cuota.MarcarPagada();
                repositorio.Actualizar(detalle.Cuota);

                string quienPago = detalle.PagadaPor != null
                    ? " (pagó su responsable " + detalle.PagadaPor.ObtenerDescripcion() + ")"
                    : string.Empty;
                auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Cuota), detalle.Cuota.Id,
                    "Estado: Pendiente → Pagada" + quienPago);
            }

            ReciboPagoDTO recibo = new ReciboPagoDTO();
            recibo.Numero = ingreso.Id;
            recibo.Fecha = ingreso.Fecha;
            recibo.MedioPago = medio.Nombre;
            recibo.Usuario = usuario.NombreCompleto;
            recibo.PagadoPor = detalles.Any(d => d.PagadaPor != null) ? pagador!.ObtenerDescripcion() : string.Empty;
            foreach (Cuota cuota in cuotas)
            {
                LineaReciboDTO linea = new LineaReciboDTO();
                linea.CuotaId = cuota.Id;
                linea.Socio = cuota.Socio.ObtenerDescripcion();
                linea.Anio = cuota.Anio;
                linea.Monto = cuota.Monto;
                recibo.Cuotas.Add(linea);
            }
            return recibo;
        }

        // Un socio por fila con sus cuotas pendientes. Las familias quedan juntas: primero se ordena por
        // quien paga (el responsable o el propio socio) y después por socio.
        public List<DeudorDTO> ObtenerDeudores(DeudoresFiltroDTO filtro)
        {
            if (filtro.Anio.HasValue)
            {
                Validaciones.ExigirAnio(filtro.Anio.Value);
            }

            IEnumerable<IGrouping<Socio, Cuota>> grupos = repositorio.ObtenerTodos()
                .Where(c => !c.EstaPagada && (!filtro.Anio.HasValue || c.Anio == filtro.Anio.Value))
                .Where(c => filtro.IncluirDadosDeBaja || c.Socio.Activo)
                .GroupBy(c => c.Socio);

            return grupos.OrderBy(g => (g.Key.Responsable ?? g.Key).Apellido)
                         .ThenBy(g => (g.Key.Responsable ?? g.Key).Nombre)
                         .ThenBy(g => g.Key.Responsable == null ? 0 : 1)
                         .ThenBy(g => g.Key.Apellido)
                         .ThenBy(g => g.Key.Nombre)
                         .Select(g => MapearDeudor(g.Key, g.ToList()))
                         .ToList();
        }

        private IEnumerable<Cuota> CuotasDe(Socio socio)
        {
            return repositorio.ObtenerTodos().Where(c => ReferenceEquals(c.Socio, socio));
        }

        private bool TieneCuota(Socio socio, int anio)
        {
            return CuotasDe(socio).Any(c => c.Anio == anio);
        }

        private ValorCuota? BuscarValor(int anio)
        {
            return valorRepositorio.ObtenerTodos().FirstOrDefault(v => v.Anio == anio);
        }

        private static DeudorDTO MapearDeudor(Socio socio, List<Cuota> pendientes)
        {
            DeudorDTO dto = new DeudorDTO();
            dto.SocioId = socio.Id;
            dto.Socio = socio.Apellido + ", " + socio.Nombre;
            dto.Dni = socio.Dni;
            dto.Paga = socio.Responsable == null ? "El socio" : "Su responsable: " + socio.Responsable.ObtenerDescripcion();
            dto.AniosQueDebe = string.Join(", ", pendientes.Select(c => c.Anio).OrderBy(a => a));
            dto.CantidadCuotas = pendientes.Count;
            dto.Total = pendientes.Sum(c => c.Monto);
            dto.Telefono = socio.Telefono;
            dto.Email = socio.Email;
            dto.EmailDeContacto = socio.Responsable != null ? socio.Responsable.Email : socio.Email;
            dto.ResponsableId = socio.Responsable?.Id;
            dto.Responsable = socio.Responsable?.ObtenerDescripcion() ?? string.Empty;
            dto.SocioActivo = socio.Activo;
            return dto;
        }

        private static CuotaDTO MapearADto(Cuota cuota)
        {
            CuotaDTO dto = new CuotaDTO();
            dto.Id = cuota.Id;
            dto.SocioId = cuota.Socio.Id;
            dto.Socio = cuota.Socio.ObtenerDescripcion();
            dto.Anio = cuota.Anio;
            dto.Monto = cuota.Monto;
            dto.Estado = cuota.Estado;
            return dto;
        }
    }
}
