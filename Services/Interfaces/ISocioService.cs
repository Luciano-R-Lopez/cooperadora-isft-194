using CooperadoraISFT94.DTOs.Socios;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface ISocioService
    {
        /// <exception cref="Common.Exceptions.NegocioException">Datos inválidos, DNI ya registrado o responsable no válido.</exception>
        SocioDto Registrar(SocioCrearDto dto, int usuarioId);

        /// <summary>Modifica los datos del socio. No puede dejar de ser Responsable si tiene socios a cargo.</summary>
        SocioDto Editar(SocioEditarDto dto, int usuarioId);

        /// <summary>Da de baja (baja lógica) o reactiva al socio. Un responsable con socios a cargo no se puede dar de baja.</summary>
        SocioDto CambiarEstado(int socioId, bool activo, int usuarioId);

        SocioDto ObtenerPorId(int id);
        IReadOnlyList<SocioDto> Buscar(string? filtro);

        /// <summary>Socios que pueden ser elegidos como responsables de otros.</summary>
        IReadOnlyList<SocioDto> ObtenerResponsables();

        /// <summary>Socios por los que responde el socio indicado (vacío si no es responsable de nadie).</summary>
        IReadOnlyList<SocioDto> ObtenerACargo(int responsableId);
    }
}
