using CooperadoraISFT94.DTOs.Socios;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface ISocioService
    {
        /// <exception cref="Common.Exceptions.NegocioException">Datos inválidos, DNI ya registrado o responsable no válido.</exception>
        SocioDto Registrar(SocioCrearDto dto, int usuarioId);

        SocioDto ObtenerPorId(int id);
        IReadOnlyList<SocioDto> Buscar(string? filtro);

        /// <summary>Socios que pueden ser elegidos como responsables de otros.</summary>
        IReadOnlyList<SocioDto> ObtenerResponsables();

        /// <summary>Socios por los que responde el socio indicado (vacío si no es responsable de nadie).</summary>
        IReadOnlyList<SocioDto> ObtenerACargo(int responsableId);
    }
}
