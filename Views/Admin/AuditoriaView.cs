using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Auditorias;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Admin
{
    /// <summary>Registro de auditoría: quién hizo qué sobre qué entidad (solo lectura).</summary>
    public class AuditoriaView : UserControl, IRefrescable
    {
        private readonly AuditoriaController _controller;
        private readonly DataGridView _grilla = Ui.CrearGrilla();

        public AuditoriaView() : this(new AuditoriaController()) { }

        public AuditoriaView(AuditoriaController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            Refrescar();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            Ui.AgregarColumna(_grilla, "Fecha y hora", nameof(AuditoriaDto.Fecha), "dd/MM/yyyy HH:mm:ss");
            Ui.AgregarColumna(_grilla, "Usuario", nameof(AuditoriaDto.Usuario));
            Ui.AgregarColumna(_grilla, "Acción", nameof(AuditoriaDto.Accion));
            Ui.AgregarColumna(_grilla, "Entidad", nameof(AuditoriaDto.EntidadAfectada));
            Ui.AgregarColumna(_grilla, "Id", nameof(AuditoriaDto.EntidadId));

            Controls.Add(_grilla);
        }

        public void Refrescar()
        {
            try
            {
                _grilla.DataSource = _controller.ObtenerTodas().ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }
    }
}
