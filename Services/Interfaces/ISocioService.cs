using CooperadoraISFT94.DTOs.Socios;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface ISocioService
    {
        /// <exception cref="Common.Exceptions.NegocioException">Datos inválidos o DNI ya registrado.</exception>
        SocioDto Registrar(SocioCrearDto dto, int usuarioId);

        SocioDto ObtenerPorId(int id);
        IReadOnlyList<SocioDto> Buscar(string? filtro);
    }
}
