using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Auditorias;
using CooperadoraISFT94.DTOs.Auth;
using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.DTOs.Egresos;
using CooperadoraISFT94.DTOs.Ingresos;
using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Tests
{
    public class SocioServiceTests
    {
        [Fact]
        public void No_se_repite_el_DNI_al_registrar_ni_al_editar()
        {
            var c = new Contexto();
            c.Socio("30123456");
            var otro = c.Socio("28456789");

            Assert.Throws<ReglaNegocioException>(() => c.SociosServicio.Registrar(
                new SocioCrearDto("A", "B", "30123456", "", "", Categoria.Socio, null), c.Operador.Id));
            Assert.Throws<ReglaNegocioException>(() => c.SociosServicio.Editar(
                new SocioEditarDto(otro.Id, "A", "B", "30123456", "", "", Categoria.Socio, null), c.Operador.Id));
        }

        [Fact]
        public void Editar_conserva_el_propio_DNI_y_audita_lo_que_cambio()
        {
            var c = new Contexto();
            var socio = c.Socio("30123456");

            var editado = c.SociosServicio.Editar(
                new SocioEditarDto(socio.Id, "Lucía", "Fernández", "30123456", "1155550101", "", Categoria.Socio, null), c.Operador.Id);

            Assert.Equal("Lucía", editado.Nombre);
            var auditoria = c.Auditorias.ObtenerTodos().Last();
            Assert.Equal(AccionAuditoria.Modificacion, auditoria.Accion);
            Assert.Contains("Nombre: Nombre → Lucía", auditoria.Detalle);
            Assert.DoesNotContain("DNI", auditoria.Detalle);
        }

        [Fact]
        public void Un_responsable_con_socios_a_cargo_no_deja_de_serlo_ni_se_da_de_baja()
        {
            var c = new Contexto();
            var responsable = c.Socio("28456789", Categoria.Responsable);
            c.Socio("40111222", responsable: responsable);

            Assert.Throws<ReglaNegocioException>(() => c.SociosServicio.Editar(
                new SocioEditarDto(responsable.Id, "A", "B", "28456789", "", "", Categoria.Socio, null), c.Operador.Id));
            Assert.Throws<ReglaNegocioException>(() => c.SociosServicio.CambiarEstado(responsable.Id, false, c.Operador.Id));
            Assert.True(responsable.EsResponsable);
            Assert.True(responsable.Activo);
        }

        [Fact]
        public void Un_socio_dado_de_baja_no_recibe_cuotas_nuevas_y_se_puede_reactivar()
        {
            var c = new Contexto();
            var socio = c.Socio();

            c.SociosServicio.CambiarEstado(socio.Id, false, c.Operador.Id);

            Assert.Throws<ReglaNegocioException>(() =>
                c.CuotasServicio.Generar(new DTOs.Cuotas.CuotaCrearDto(socio.Id, 5000m, 2026), c.Operador.Id));

            c.SociosServicio.CambiarEstado(socio.Id, true, c.Operador.Id);
            c.CuotasServicio.Generar(new DTOs.Cuotas.CuotaCrearDto(socio.Id, 5000m, 2026), c.Operador.Id);
            Assert.Single(c.Cuotas.ObtenerPorSocio(socio));
        }
    }

    public class CuotaServiceTests
    {
        [Fact]
        public void Generar_para_todos_solo_genera_a_activos_sin_cuota_del_anio_y_fija_el_valor()
        {
            var c = new Contexto();
            var conCuota = c.Socio("30123456");
            c.Cuota(conCuota, anio: 2026);
            c.Socio("28456789");
            var deBaja = c.Socio("40111222");
            deBaja.DarDeBaja(tieneSociosACargo: false);

            var resultado = c.CuotasServicio.GenerarParaTodos(2026, 6000m, c.Operador.Id);

            Assert.Equal(1, resultado.Generadas);
            Assert.Equal(1, resultado.YaTenian);
            Assert.Empty(c.Cuotas.ObtenerPorSocio(deBaja));
            Assert.Equal(6000m, c.CuotasServicio.ObtenerMontoSugerido(2026));
        }

        [Fact]
        public void Sin_caja_abierta_el_pago_falla_y_la_cuota_queda_pendiente()
        {
            var c = new Contexto();
            var cuota = c.Cuota(c.Socio());

            Assert.Throws<ReglaNegocioException>(() => c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id));
            Assert.False(cuota.EstaPagada);
            Assert.Empty(c.Movimientos.ObtenerTodos());
        }

        [Fact]
        public void El_pago_acredita_la_caja_marca_la_cuota_y_devuelve_el_recibo()
        {
            var c = new Contexto();
            c.AbrirCaja(1000m);
            var cuota = c.Cuota(c.Socio());

            var recibo = c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id);

            Assert.True(cuota.EstaPagada);
            Assert.Equal(6000m, c.Cajas.ObtenerActiva()!.MontoTotal);
            Assert.Equal(5000m, recibo.Monto);
            Assert.Equal("Efectivo", recibo.MedioPago);
            Assert.Null(recibo.PagadoPor);
        }

        [Fact]
        public void El_responsable_puede_pagar_la_cuota_de_su_socio_a_cargo_y_otro_socio_no()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var responsable = c.Socio("28456789", Categoria.Responsable);
            var otroResponsable = c.Socio("27000000", Categoria.Responsable);
            var aCargo = c.Socio("40111222", responsable: responsable);
            var cuota = c.Cuota(aCargo);

            Assert.Throws<ReglaNegocioException>(() =>
                c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id, otroResponsable.Id));
            Assert.False(cuota.EstaPagada);

            var recibo = c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id, responsable.Id);

            Assert.True(cuota.EstaPagada);
            Assert.NotNull(recibo.PagadoPor);
            Assert.Same(responsable, c.Detalles.ObtenerTodos().Single().PagadaPor);
        }

        [Fact]
        public void Pendientes_lista_solo_las_cuotas_sin_pagar_del_anio_pedido()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var socio = c.Socio();
            c.Cuota(socio, anio: 2025);
            var pagada = c.Cuota(socio, anio: 2026);
            c.CuotasServicio.RegistrarPago(pagada.Id, c.Efectivo.Id, c.Operador.Id);

            Assert.Single(c.CuotasServicio.ObtenerPendientes(null));
            Assert.Single(c.CuotasServicio.ObtenerPendientes(2025));
            Assert.Empty(c.CuotasServicio.ObtenerPendientes(2026));
        }
    }

    public class AnulacionTests
    {
        [Fact]
        public void Anular_el_cobro_de_una_cuota_revierte_la_caja_y_la_cuota_vuelve_a_pendiente()
        {
            var c = new Contexto();
            c.AbrirCaja(1000m);
            var cuota = c.Cuota(c.Socio());
            var recibo = c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id);

            var anulado = c.MovimientosServicio.Anular(recibo.Numero, "Cobro duplicado", c.Contador.Id);

            Assert.True(anulado.Anulado);
            Assert.False(cuota.EstaPagada);
            Assert.Equal(1000m, c.Cajas.ObtenerActiva()!.MontoTotal);

            var resumen = c.CajasServicio.ObtenerResumenActivo(null);
            Assert.Equal(0m, resumen.TotalIngresos);
            Assert.Equal(1, resumen.CantidadAnulados);
            Assert.Equal(1000m, resumen.MontoEsperado);
        }

        [Fact]
        public void No_se_anula_dos_veces_ni_sin_motivo()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var ingreso = c.Ingresos.Registrar(new IngresoCrearDto(100m, CategoriaIngreso.Donacion, c.Efectivo.Id), c.Operador.Id);

            Assert.Throws<ReglaNegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "  ", c.Contador.Id));
            c.MovimientosServicio.Anular(ingreso.Id, "Error de carga", c.Contador.Id);
            Assert.Throws<ReglaNegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "Otra vez", c.Contador.Id));
        }

        [Fact]
        public void No_se_anulan_movimientos_de_una_caja_cerrada()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var ingreso = c.Ingresos.Registrar(new IngresoCrearDto(100m, CategoriaIngreso.Donacion, c.Efectivo.Id), c.Operador.Id);
            c.CajasServicio.Cerrar(new CerrarCajaDto(100m), c.Contador.Id);

            Assert.Throws<ReglaNegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "Tarde", c.Contador.Id));
        }

        [Fact]
        public void Anular_un_ingreso_ya_gastado_falla_por_saldo_insuficiente_sin_cambiar_nada()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var ingreso = c.Ingresos.Registrar(new IngresoCrearDto(100m, CategoriaIngreso.Donacion, c.Efectivo.Id), c.Operador.Id);
            c.Egresos.Registrar(new EgresoCrearDto(80m, c.Materiales.Id, c.Efectivo.Id), c.Contador.Id);

            Assert.Throws<ReglaNegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "Error", c.Contador.Id));
            Assert.Equal(20m, c.Cajas.ObtenerActiva()!.MontoTotal);
            Assert.False(c.Movimientos.ObtenerExistente(ingreso.Id).Anulado);
        }

        [Fact]
        public void El_resumen_de_una_caja_cerrada_compara_contra_lo_contado()
        {
            var c = new Contexto();
            c.AbrirCaja(100m);
            c.Ingresos.Registrar(new IngresoCrearDto(50m, CategoriaIngreso.Donacion, c.Efectivo.Id), c.Operador.Id);
            var cerrada = c.CajasServicio.Cerrar(new CerrarCajaDto(140m), c.Contador.Id);

            var resumen = c.CajasServicio.ObtenerResumen(cerrada.Caja.Id);

            Assert.Equal(150m, resumen.MontoEsperado);
            Assert.Equal(140m, resumen.MontoDeclarado);
            Assert.Equal(-10m, resumen.Diferencia);
        }
    }

    public class AuthYUsuarioTests
    {
        [Fact]
        public void Cinco_intentos_fallidos_bloquean_el_ingreso_hasta_que_vence_el_bloqueo()
        {
            var c = new Contexto();

            for (var i = 0; i < 4; i++)
                Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(new LoginDto("operador", "mal")));
            var bloqueo = Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(new LoginDto("operador", "mal")));
            Assert.Contains("bloqueado", bloqueo.Message);

            // Bloqueado: ni con la contraseña correcta.
            Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(new LoginDto("operador", "operador123")));

            c.Ahora = c.Ahora.AddMinutes(6);
            Assert.Equal("operador", c.Auth.Autenticar(new LoginDto("OPERADOR", "operador123")).NombreUsuario);
        }

        [Fact]
        public void Un_ingreso_correcto_reinicia_los_intentos()
        {
            var c = new Contexto();
            for (var i = 0; i < 4; i++)
                Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(new LoginDto("operador", "mal")));

            c.Auth.Autenticar(new LoginDto("operador", "operador123"));

            for (var i = 0; i < 4; i++)
                Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(new LoginDto("operador", "mal")));
            c.Auth.Autenticar(new LoginDto("operador", "operador123"));
        }

        [Fact]
        public void Cambiar_la_propia_contrasenia_exige_la_actual_y_la_confirmacion()
        {
            var c = new Contexto();
            var id = c.Operador.Id;

            Assert.Throws<ReglaNegocioException>(() =>
                c.UsuariosServicio.CambiarPasswordPropia(id, new CambioPasswordDto("mal", "nueva123", "nueva123")));
            Assert.Throws<ReglaNegocioException>(() =>
                c.UsuariosServicio.CambiarPasswordPropia(id, new CambioPasswordDto("operador123", "nueva123", "otra123")));

            c.UsuariosServicio.CambiarPasswordPropia(id, new CambioPasswordDto("operador123", "nueva123", "nueva123"));

            c.Auth.Autenticar(new LoginDto("operador", "nueva123"));
            Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(new LoginDto("operador", "operador123")));
        }
    }

    public class CatalogoYAuditoriaTests
    {
        [Fact]
        public void No_se_repiten_nombres_y_los_desactivados_no_se_ofrecen_ni_se_usan()
        {
            var c = new Contexto();
            c.AbrirCaja(1000m);

            Assert.Throws<ReglaNegocioException>(() => c.Catalogos.Agregar(TipoCatalogo.MedioPago, "efectivo", 1));
            c.Catalogos.CambiarEstado(TipoCatalogo.MedioPago, c.Transferencia.Id, false, 1);

            Assert.DoesNotContain(c.Catalogos.ObtenerMediosPago(), m => m.Id == c.Transferencia.Id);
            Assert.Contains(c.Catalogos.ObtenerMediosPago(incluirInactivos: true), m => m.Id == c.Transferencia.Id);
            Assert.Throws<ReglaNegocioException>(() =>
                c.Egresos.Registrar(new EgresoCrearDto(10m, c.Materiales.Id, c.Transferencia.Id), c.Contador.Id));
        }

        [Fact]
        public void Renombrar_a_un_nombre_repetido_deja_el_nombre_anterior()
        {
            var c = new Contexto();

            Assert.Throws<ReglaNegocioException>(() =>
                c.Catalogos.Renombrar(TipoCatalogo.MedioPago, c.Transferencia.Id, "Efectivo", 1));
            Assert.Equal("Transferencia", c.Transferencia.Nombre);
        }

        [Fact]
        public void La_auditoria_se_filtra_por_accion_fecha_y_texto()
        {
            var c = new Contexto();
            var socio = c.SociosServicio.Registrar(new SocioCrearDto("Ana", "Paz", "30123456", "", "", Categoria.Socio, null), c.Operador.Id);
            c.Ahora = c.Ahora.AddDays(2);
            c.SociosServicio.CambiarEstado(socio.Id, false, c.Operador.Id);

            Assert.Single(c.Auditoria.Obtener(new AuditoriaFiltroDto(null, null, AccionAuditoria.Baja, null)));
            Assert.Single(c.Auditoria.Obtener(new AuditoriaFiltroDto(c.Ahora.Date, null, null, null)));
            Assert.Single(c.Auditoria.Obtener(new AuditoriaFiltroDto(null, null, null, "paz")));
            Assert.Throws<ReglaNegocioException>(() =>
                c.Auditoria.Obtener(new AuditoriaFiltroDto(c.Ahora, c.Ahora.AddDays(-1), null, null)));
        }
    }
}
