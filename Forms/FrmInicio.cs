using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Pantalla de bienvenida: saludo, aviso si no hay caja abierta y un resumen según los roles del usuario.
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
            Tema.Aplicar(this);
        }

        private void FrmInicio_Load(object? sender, EventArgs e)
        {
            SesionActual sesion = SesionActual.Instancia;
            DateTime ahora = DateTime.Now;
            lblSaludo.Text = Formato.Saludo(ahora) + ", " + sesion.Usuario?.Nombre + ".";
            lblDatoFecha.Text = Formato.DiaSemana(ahora) + " " + ahora.ToString(Formato.Fecha)
                + "   |   Ingresó como " + sesion.Usuario?.RolesTexto + ".";

            bool administrador = sesion.TieneRol(Rol.Administrador);
            bool operador = sesion.TieneRol(Rol.Operador);
            bool contador = sesion.TieneRol(Rol.Contador);
            pnlTarjetaCaja.Visible = operador || contador;
            pnlTarjetaMorosos.Visible = operador;
            pnlTarjetaSocios.Visible = operador;
            pnlTarjetaUsuarios.Visible = administrador;

            try
            {
                if (operador || contador)
                {
                    MostrarCaja(contador);
                }
                if (operador)
                {
                    MostrarSociosYMorosos();
                }
                if (administrador)
                {
                    MostrarUsuarios();
                }
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void MostrarCaja(bool contador)
        {
            CajaController cajas = new CajaController();
            CajaDTO? caja = cajas.ObtenerActiva();
            lblResumenValorCaja.Text = caja == null ? "Cerrada" : "Abierta";
            lblResumenValorCaja.ForeColor = caja == null ? Tema.PeligroTexto : Tema.ExitoTexto;

            if (caja == null)
            {
                lblResumenDetalleCaja.Text = contador ? "Ábrala desde Caja > Apertura y cierre." : "El contador debe abrirla para poder cobrar.";
                lblAlerta.Text = "No hay una caja abierta: no se pueden cobrar cuotas ni registrar ingresos o egresos.";
                lblAlerta.Visible = true;
                return;
            }

            lblResumenDetalleCaja.Text = "Desde el " + caja.FechaApertura.ToString(Formato.FechaHora);
            if (contador)
            {
                ResumenCajaDTO resumen = cajas.ObtenerResumenActivo(null);
                lblResumenDetalleCaja.Text += "\nSaldo esperado: " + Formato.Monto(resumen.MontoEsperado);
            }
        }

        private void MostrarSociosYMorosos()
        {
            List<DeudorDTO> deudores = new CuotaController().ObtenerDeudores(new DeudoresFiltroDTO());
            lblResumenValorMorosos.Text = deudores.Count.ToString();
            lblResumenDetalleMorosos.Text = deudores.Sum(d => d.CantidadCuotas) + " cuotas pendientes por "
                + Formato.Monto(deudores.Sum(d => d.Total));

            List<SocioDTO> socios = new SocioController().ObtenerTodos();
            lblResumenValorSocios.Text = socios.Count(s => s.Activo).ToString();
            lblResumenDetalleSocios.Text = socios.Count(s => s.Activo && s.Categoria == Categoria.Responsable) + " responsables, "
                + socios.Count(s => !s.Activo) + " dados de baja";
        }

        private void MostrarUsuarios()
        {
            List<UsuarioDTO> usuarios = new UsuarioController().ObtenerTodos();
            lblResumenValorUsuarios.Text = usuarios.Count(u => u.Activo).ToString();
            lblResumenDetalleUsuarios.Text = usuarios.Count(u => !u.Activo) + " desactivados";
        }
    }
}
