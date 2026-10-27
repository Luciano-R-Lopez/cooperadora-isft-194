using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Apertura y cierre de la caja. Con la caja abierta muestra el resumen; al escribir el dinero contado se ve
    // la diferencia antes de cerrar.
    public partial class FrmCaja : Form
    {
        private readonly CajaController controlador;
        private bool cajaAbierta;
        private bool saldoEscrito;
        private bool actualizando;

        public FrmCaja()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            controlador = new CajaController();
        }

        private void FrmCaja_Load(object? sender, EventArgs e)
        {
            Refrescar();
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void Refrescar()
        {
            try
            {
                cajaAbierta = controlador.ObtenerActiva() != null;
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }

            saldoEscrito = false;
            Tema.MostrarEstadoCaja(lblEstadoCaja, cajaAbierta);
            lblModo.Text = cajaAbierta ? "Cerrar caja" : "Abrir caja";
            lblSaldo.Text = cajaAbierta ? "Dinero contado al cerrar: *" : "Saldo inicial (efectivo con el que se abre): *";
            btnAccionCaja.Text = cajaAbierta ? "Cerrar caja" : "Abrir caja";
            actualizando = true;
            numSaldo.Value = 0;
            actualizando = false;
            ActualizarResumen();
        }

        private void numSaldo_ValueChanged(object? sender, EventArgs e)
        {
            if (actualizando)
            {
                return;
            }
            saldoEscrito = true;
            ActualizarResumen();
        }

        private void ActualizarResumen()
        {
            if (!cajaAbierta)
            {
                MostrarResumen(null);
                return;
            }
            try
            {
                MostrarResumen(controlador.ObtenerResumenActivo(saldoEscrito ? numSaldo.Value : null));
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void MostrarResumen(ResumenCajaDTO? resumen)
        {
            lblResumenDetalleInicial.Text = resumen == null ? "-" : Formato.Monto(resumen.MontoInicial);
            lblResumenDetalleIngresos.Text = resumen == null ? "-" : Formato.Monto(resumen.TotalIngresos);
            lblResumenDetalleEgresos.Text = resumen == null ? "-" : Formato.Monto(resumen.TotalEgresos);
            lblResumenDetalleEsperado.Text = resumen == null ? "-" : Formato.Monto(resumen.MontoEsperado);

            decimal? diferencia = resumen?.Diferencia;
            lblResumenDetalleDiferencia.Text = diferencia.HasValue ? Formato.Monto(diferencia.Value) : "-";
            lblResumenDetalleDiferencia.ForeColor = diferencia.HasValue && diferencia.Value != 0 ? Tema.PeligroTexto : Tema.Texto;

            dgvMedios.MensajeSinFilas(resumen == null ? "No hay una caja abierta." : "Todavía no hay movimientos en la caja.");
            dgvMedios.DataSource = null;
            dgvMedios.DataSource = resumen?.TotalesPorMedio ?? new List<TotalMedioPagoDTO>();
            dgvMedios.MostrarColumnas(("MedioPago", "Medio de pago"), ("Monto", "Saldo"));
            dgvMedios.FormatoMoneda("Monto");
            dgvMedios.QuitarSeleccion();
        }

        private void btnAccionCaja_Click(object? sender, EventArgs e)
        {
            if (cajaAbierta)
            {
                CerrarCaja();
            }
            else
            {
                AbrirCaja();
            }
        }

        private void AbrirCaja()
        {
            try
            {
                controlador.Abrir(numSaldo.Value);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }
            Refrescar();
            Aviso.Mostrar(lblAviso, "Caja abierta.");
        }

        private void CerrarCaja()
        {
            if (!Mensajes.Confirmar("¿Cerrar la caja con " + Formato.Monto(numSaldo.Value) + " contados?"))
            {
                return;
            }

            ResumenCajaDTO resumen;
            try
            {
                resumen = controlador.Cerrar(numSaldo.Value);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }

            Refrescar();
            Mensajes.Informacion("Caja cerrada.\n\nSaldo esperado: " + Formato.Monto(resumen.MontoEsperado)
                + "\nDinero contado: " + Formato.Monto(resumen.MontoDeclarado ?? 0)
                + "\nDiferencia: " + Formato.Monto(resumen.Diferencia ?? 0));
        }
    }
}
