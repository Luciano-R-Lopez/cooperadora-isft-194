using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Genera la cuota de un año para un socio (por ejemplo, un socio que se inscribió a mitad de año).
    public partial class FrmGenerarCuota : Form
    {
        private readonly int socioId;
        private readonly CuotaController controlador;

        // Para que el diseñador de Visual Studio pueda abrir el formulario.
        public FrmGenerarCuota()
            : this(0)
        {
        }

        public FrmGenerarCuota(int socioId)
        {
            InitializeComponent();
            Tema.Aplicar(this);
            this.socioId = socioId;
            controlador = new CuotaController();
        }

        private void FrmGenerarCuota_Load(object? sender, EventArgs e)
        {
            numAnio.Value = DateTime.Today.Year;
            ProponerMonto();
        }

        private void numAnio_ValueChanged(object? sender, EventArgs e)
        {
            ProponerMonto();
        }

        // El valor fijado para el año (si no hay, queda el que estaba escrito).
        private void ProponerMonto()
        {
            try
            {
                decimal? sugerido = controlador.ObtenerMontoSugerido((int)numAnio.Value);
                if (sugerido.HasValue)
                {
                    numMonto.Value = Math.Min(sugerido.Value, numMonto.Maximum);
                }
            }
            catch (NegocioException)
            {
                // La sugerencia es una ayuda: sin ella se escribe el monto a mano.
            }
        }

        private void btnGenerar_Click(object? sender, EventArgs e)
        {
            CuotaDTO dto = new CuotaDTO();
            dto.SocioId = socioId;
            dto.Anio = (int)numAnio.Value;
            dto.Monto = numMonto.Value;

            try
            {
                controlador.Generar(dto);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
