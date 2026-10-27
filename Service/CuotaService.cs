using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Repository;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Service
{
    public sealed class CuotaService : ICuotaService
    {
        private static readonly Lazy<CuotaService> _instancia = new(() =>
            new CuotaService(CuotaRepository.Instancia, SocioRepository.Instancia, UsuarioRepository.Instancia,
                             MedioPagoRepository.Instancia, DetalleIngresoCuotaRepository.Instancia,
                             ValorCuotaRepository.Instancia, MovimientoRegistrador.Instancia, AuditoriaService.Instancia));
        public static CuotaService Instancia => _instancia.Value;

        private readonly ICuotaRepository _cuotas;
        private readonly ISocioRepository _socios;
        private readonly IUsuarioRepository _usuarios;
        private readonly IMedioPagoRepository _medios;
        private readonly IDetalleIngresoCuotaRepository _detalles;
        private readonly IValorCuotaRepository _valores;
        private readonly IMovimientoRegistrador _registrador;
        private readonly IAuditoriaService _auditoria;
        private readonly Func<DateTime> _reloj;

        internal CuotaService(ICuotaRepository cuotas, ISocioRepository socios, IUsuarioRepository usuarios,
                              IMedioPagoRepository medios, IDetalleIngresoCuotaRepository detalles,
                              IValorCuotaRepository valores, IMovimientoRegistrador registrador,
                              IAuditoriaService auditoria, Func<DateTime>? reloj = null)
        {
            _cuotas = cuotas;
            _socios = socios;
            _usuarios = usuarios;
            _medios = medios;
            _detalles = detalles;
            _valores = valores;
            _registrador = registrador;
            _auditoria = auditoria;
            _reloj = reloj ?? (() => DateTime.Now);
        }

        public IReadOnlyList<CuotaDto> ObtenerPorSocio(int socioId)
        {
            var socio = _socios.ObtenerExistente(socioId);
            return _cuotas.ObtenerPorSocio(socio).Select(CuotaMapper.ADto).ToList();
        }

        public CuotaDto Generar(CuotaCrearDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var socio = _socios.ObtenerExistente(dto.SocioId);
            socio.ExigirActivo();

            // La Cuota valida monto y año; acá solo se controla lo que depende de las otras cuotas del socio.
            var cuota = new Cuota(socio, dto.Monto, dto.Anio);

            if (_cuotas.ExisteParaAnio(socio, cuota.Anio))
                throw new ReglaNegocioException("El socio ya tiene una cuota para ese año.");

            _cuotas.Agregar(cuota);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Cuota), cuota.Id,
                                 $"Cuota {cuota.Anio} de {socio.ObtenerDescripcion()} por {cuota.Monto:C2}");

            return CuotaMapper.ADto(cuota);
        }

        public GeneracionCuotasDto GenerarParaTodos(int anio, decimal monto, int usuarioId)
        {
            // Fijar el valor valida año y monto antes de generar nada.
            FijarValorCuota(anio, monto, usuarioId);

            var generadas = 0;
            var yaTenian = 0;

            foreach (var socio in _socios.ObtenerTodos().Where(s => s.Activo))
            {
                if (_cuotas.ExisteParaAnio(socio, anio))
                {
                    yaTenian++;
                    continue;
                }

                var cuota = _cuotas.Agregar(new Cuota(socio, monto, anio));
                _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Cuota), cuota.Id,
                                     $"Cuota {anio} de {socio.ObtenerDescripcion()} por {monto:C2} (generación del año)");
                generadas++;
            }

            return new GeneracionCuotasDto(anio, monto, generadas, yaTenian);
        }

        public decimal? ObtenerMontoSugerido(int anio) =>
            _valores.ObtenerPorAnio(anio)?.Monto ?? _cuotas.ObtenerUltimaDelAnio(anio)?.Monto;

        public void FijarValorCuota(int anio, decimal monto, int usuarioId)
        {
            var valor = _valores.ObtenerPorAnio(anio);

            if (valor is null)
            {
                valor = _valores.Agregar(new ValorCuota(anio, monto));
                _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(ValorCuota), valor.Id,
                                     $"Valor de la cuota {anio}: {valor.Monto:C2}");
                return;
            }

            var anterior = valor.Monto;
            valor.CambiarMonto(monto);
            if (anterior == valor.Monto)
                return;

            _valores.Actualizar(valor);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(ValorCuota), valor.Id,
                                 new DescripcionCambios().Campo($"Valor de la cuota {anio}", anterior, valor.Monto).ToString());
        }

        public ReciboPagoDto RegistrarPago(int cuotaId, int medioPagoId, int usuarioId, int? pagadorId = null) =>
            RegistrarPagos(new[] { cuotaId }, medioPagoId, usuarioId, pagadorId);

        public ReciboPagoDto RegistrarPagos(IReadOnlyCollection<int> cuotaIds, int medioPagoId, int usuarioId, int? pagadorId = null)
        {
            ArgumentNullException.ThrowIfNull(cuotaIds);
            if (cuotaIds.Count == 0)
                throw new ReglaNegocioException("Elegí al menos una cuota para cobrar.");

            var cuotas = cuotaIds.Distinct().Select(_cuotas.ObtenerExistente).OrderBy(c => c.Anio).ToList();
            foreach (var cuota in cuotas)
                cuota.ValidarPagable();

            var usuario = _usuarios.ObtenerExistente(usuarioId);
            var medio = _medios.ObtenerExistente(medioPagoId);
            medio.ExigirActivo();
            var pagador = pagadorId is int id ? _socios.ObtenerExistente(id) : null;

            // Sin responsable que pague, un recibo es de un solo socio.
            if (pagador is null && cuotas.Select(c => c.Socio).Distinct().Count() > 1)
                throw new ReglaNegocioException("Solo se pueden cobrar juntas cuotas del mismo socio (o las que paga un responsable).");

            // Todo lo que puede fallar por los datos se construye antes de tocar la caja; el registro del ingreso
            // (que exige caja abierta) va último, así si falla las cuotas quedan como estaban.
            var ingreso = new Ingreso(usuario, _reloj(), cuotas.Sum(c => c.Monto), medio, CategoriaIngreso.CuotaSocial);

            // El responsable puede pagar sus propias cuotas junto con las de sus socios a cargo.
            var detalles = cuotas
                .Select(c => new DetalleIngresoCuota(ingreso, c, c.Monto, ReferenceEquals(c.Socio, pagador) ? null : pagador))
                .ToList();

            _registrador.Registrar(ingreso);

            foreach (var detalle in detalles)
            {
                _detalles.Agregar(detalle);

                var cuota = detalle.Cuota;
                cuota.CambiarEstado(EstadoCuota.Pagada);
                _cuotas.Actualizar(cuota);

                var quienPago = detalle.PagadaPor is Socio responsable
                    ? $" (pagó su responsable {responsable.ObtenerDescripcion()})"
                    : string.Empty;
                _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Cuota), cuota.Id,
                                     $"Estado: Pendiente → Pagada{quienPago}");
            }

            var lineas = cuotas
                .Select(c => new LineaReciboDto(c.Id, c.Socio.ObtenerDescripcion(), c.Anio, c.Monto))
                .ToList();
            var pagoResponsable = detalles.Any(d => d.PagadaPor is not null);

            return new ReciboPagoDto(ingreso.Id, ingreso.Fecha, lineas, medio.Nombre,
                                     pagoResponsable ? pagador!.ObtenerDescripcion() : null, usuario.NombreCompleto);
        }

        public IReadOnlyList<CuotaPendienteDto> ObtenerPendientes(int? anio)
        {
            if (anio is int a && !Common.Validaciones.EsAnioValido(a))
                throw new ReglaNegocioException("El año no es válido.");

            return _cuotas.ObtenerPendientes(anio)
                          .Select(c => new CuotaPendienteDto(
                              c.Id, c.Socio.Id, c.Socio.Apellido, c.Socio.Nombre, c.Socio.Dni, c.Socio.Telefono,
                              c.Socio.Email, c.Socio.Responsable?.ObtenerDescripcion() ?? "-", c.Socio.Activo,
                              c.Anio, c.Monto))
                          .ToList();
        }

        public IReadOnlyList<DeudorDto> ObtenerDeudores(DeudoresFiltroDto filtro)
        {
            ArgumentNullException.ThrowIfNull(filtro);
            if (filtro.Anio is int a && !Common.Validaciones.EsAnioValido(a))
                throw new ReglaNegocioException("El año no es válido.");

            var texto = filtro.Texto?.Trim() ?? string.Empty;

            return _cuotas.ObtenerPendientes(filtro.Anio)
                .Where(c => filtro.IncluirDadosDeBaja || c.Socio.Activo)
                .GroupBy(c => c.Socio)
                .Where(g => texto.Length == 0
                            || g.Key.Dni.Contains(texto, StringComparison.OrdinalIgnoreCase)
                            || g.Key.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase)
                            || $"{g.Key.Apellido} {g.Key.Nombre}".Contains(texto, StringComparison.OrdinalIgnoreCase)
                            || (g.Key.Responsable?.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false))
                // Las familias juntas: primero se ordena por quien paga y después por socio.
                .OrderBy(g => (g.Key.Responsable ?? g.Key).Apellido)
                .ThenBy(g => (g.Key.Responsable ?? g.Key).Nombre)
                .ThenBy(g => g.Key.Responsable is null ? 0 : 1)
                .ThenBy(g => g.Key.Apellido)
                .ThenBy(g => g.Key.Nombre)
                .Select(g => new DeudorDto(
                    g.Key.Id, g.Key.Apellido, g.Key.Nombre, g.Key.Dni, g.Key.Telefono, g.Key.Email,
                    g.Key.Responsable?.Id, g.Key.Responsable?.ObtenerDescripcion(), g.Key.Activo,
                    g.Select(c => c.Anio).OrderBy(x => x).ToList(), g.Sum(c => c.Monto)))
                .ToList();
        }
    }
}
