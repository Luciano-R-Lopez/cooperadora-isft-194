using System.Globalization;
using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Forms.Utils;
using CooperadoraISFT94.Forms.Shell;

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
                c.CuotasServicio.Generar(new CuotaCrearDto(socio.Id, 5000m, 2026), c.Operador.Id));

            c.SociosServicio.CambiarEstado(socio.Id, true, c.Operador.Id);
            c.CuotasServicio.Generar(new CuotaCrearDto(socio.Id, 5000m, 2026), c.Operador.Id);
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
            c.Egresos.Registrar(new EgresoCrearDto(80m, c.Materiales.Id, c.Estudiantes.Id, c.Efectivo.Id), c.Contador.Id);

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
                c.Egresos.Registrar(new EgresoCrearDto(10m, c.Materiales.Id, c.Estudiantes.Id, c.Transferencia.Id), c.Contador.Id));
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

namespace CooperadoraISFT94.Tests
{
    public class CobroMultipleYMorososTests
    {
        [Fact]
        public void Varias_cuotas_del_socio_se_cobran_en_un_solo_ingreso_y_recibo()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var socio = c.Socio();
            var vieja = c.Cuota(socio, 4500m, 2025);
            var actual = c.Cuota(socio, 5000m, 2026);

            var recibo = c.CuotasServicio.RegistrarPagos(new[] { actual.Id, vieja.Id }, c.Efectivo.Id, c.Operador.Id);

            Assert.True(vieja.EstaPagada);
            Assert.True(actual.EstaPagada);
            Assert.Single(c.Movimientos.ObtenerTodos());
            Assert.Equal(9500m, recibo.Monto);
            Assert.Equal(new[] { 2025, 2026 }, recibo.Cuotas.Select(l => l.Anio));
            Assert.Equal(9500m, c.Cajas.ObtenerActiva()!.MontoTotal);
        }

        [Fact]
        public void Sin_responsable_no_se_cobran_juntas_cuotas_de_distintos_socios_y_nada_cambia()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var una = c.Cuota(c.Socio("30123456"));
            var otra = c.Cuota(c.Socio("28456789"));

            Assert.Throws<ReglaNegocioException>(() =>
                c.CuotasServicio.RegistrarPagos(new[] { una.Id, otra.Id }, c.Efectivo.Id, c.Operador.Id));
            Assert.Throws<ReglaNegocioException>(() =>
                c.CuotasServicio.RegistrarPagos(Array.Empty<int>(), c.Efectivo.Id, c.Operador.Id));
            Assert.False(una.EstaPagada);
            Assert.Empty(c.Movimientos.ObtenerTodos());
        }

        [Fact]
        public void El_responsable_paga_juntas_sus_cuotas_y_las_de_sus_socios_a_cargo()
        {
            var c = new Contexto();
            c.AbrirCaja();
            var responsable = c.Socio("28456789", Categoria.Responsable);
            var hija = c.Socio("40111222", responsable: responsable);
            var hijo = c.Socio("40111333", responsable: responsable);
            var propia = c.Cuota(responsable);

            var recibo = c.CuotasServicio.RegistrarPagos(
                new[] { propia.Id, c.Cuota(hija).Id, c.Cuota(hijo).Id }, c.Efectivo.Id, c.Operador.Id, responsable.Id);

            Assert.Equal(15000m, recibo.Monto);
            Assert.NotNull(recibo.PagadoPor);
            var detalles = c.Detalles.ObtenerTodos();
            Assert.Null(detalles.Single(d => d.Cuota.Id == propia.Id).PagadaPor);
            Assert.Equal(2, detalles.Count(d => ReferenceEquals(d.PagadaPor, responsable)));
        }

        [Fact]
        public void Los_deudores_salen_uno_por_socio_con_sus_anios_y_las_familias_juntas()
        {
            var c = new Contexto();
            var zapata = c.Socio("30000001");
            zapata.ActualizarDatos("Ana", "Zapata", zapata.Dni, "", "", Categoria.Responsable, null, false);
            var aCargo = c.Socio("30000002", responsable: zapata);
            aCargo.ActualizarDatos("Bruno", "Alba", aCargo.Dni, "", "", Categoria.Socio, zapata, false);
            var medio = c.Socio("30000003");
            medio.ActualizarDatos("Carla", "Mena", medio.Dni, "", "", Categoria.Socio, null, false);

            c.Cuota(aCargo, 4500m, 2025);
            c.Cuota(aCargo, 5000m, 2026);
            c.Cuota(zapata, 5000m, 2026);
            c.Cuota(medio, 5000m, 2026);

            var deudores = c.CuotasServicio.ObtenerDeudores(new DeudoresFiltroDto(null, null, true));

            // Mena primero; después la familia Zapata: el responsable y luego su socio a cargo (Alba).
            Assert.Equal(new[] { "Mena", "Zapata", "Alba" }, deudores.Select(d => d.Apellido));
            var alba = deudores.Single(d => d.Apellido == "Alba");
            Assert.Equal(new[] { 2025, 2026 }, alba.Anios);
            Assert.Equal(9500m, alba.Total);
            Assert.Contains("Zapata", alba.Paga);

            Assert.Single(c.CuotasServicio.ObtenerDeudores(new DeudoresFiltroDto(2025, null, true)));
            Assert.Equal(2, c.CuotasServicio.ObtenerDeudores(new DeudoresFiltroDto(null, "zapata", true)).Count);
        }

        [Fact]
        public void Los_dados_de_baja_se_pueden_ocultar()
        {
            var c = new Contexto();
            var socio = c.Socio();
            c.Cuota(socio);
            socio.DarDeBaja(tieneSociosACargo: false);

            Assert.Single(c.CuotasServicio.ObtenerDeudores(new DeudoresFiltroDto(null, null, true)));
            Assert.Empty(c.CuotasServicio.ObtenerDeudores(new DeudoresFiltroDto(null, null, false)));
        }
    }

    public class GrupoEgresoTests
    {
        [Fact]
        public void Un_mismo_motivo_puede_ser_de_distintos_grupos_y_se_filtra_por_grupo()
        {
            var c = new Contexto();
            c.AbrirCaja();
            c.Ingresos.Registrar(new IngresoCrearDto(1000m, CategoriaIngreso.Donacion, c.Efectivo.Id), c.Operador.Id);
            c.Egresos.Registrar(new EgresoCrearDto(100m, c.Materiales.Id, c.Estudiantes.Id, c.Efectivo.Id), c.Contador.Id);
            c.Egresos.Registrar(new EgresoCrearDto(200m, c.Materiales.Id, c.Administracion.Id, c.Efectivo.Id), c.Contador.Id);

            var caja = c.Cajas.ObtenerActiva()!.Id;
            var deEstudiantes = c.MovimientosServicio.Obtener(new MovimientoFiltroDto(caja, null, null, null, c.Estudiantes.Id));

            var egreso = Assert.Single(deEstudiantes);
            Assert.Equal(100m, egreso.Monto);
            Assert.Equal("Estudiantes", egreso.Grupo);
            Assert.Equal("Materiales", egreso.Clasificacion);
            Assert.Contains(c.MovimientosServicio.Obtener(new MovimientoFiltroDto(caja, TipoMovimiento.Ingreso, null, null)),
                            m => m.Grupo == "-");
        }

        [Fact]
        public void No_se_puede_usar_un_grupo_desactivado_y_el_administrador_lo_mantiene_como_los_demas_catalogos()
        {
            var c = new Contexto();
            c.AbrirCaja(1000m);

            var otros = c.Catalogos.Agregar(TipoCatalogo.GrupoEgreso, "Otros", 1);
            Assert.Throws<ReglaNegocioException>(() => c.Catalogos.Agregar(TipoCatalogo.GrupoEgreso, "estudiantes", 1));
            c.Catalogos.CambiarEstado(TipoCatalogo.GrupoEgreso, otros.Id, false, 1);

            Assert.DoesNotContain(c.Catalogos.ObtenerGruposEgreso(), g => g.Id == otros.Id);
            Assert.Contains(c.Catalogos.ObtenerGruposEgreso(incluirInactivos: true), g => g.Id == otros.Id);
            Assert.Throws<ReglaNegocioException>(() =>
                c.Egresos.Registrar(new EgresoCrearDto(10m, c.Materiales.Id, otros.Id, c.Efectivo.Id), c.Contador.Id));
        }
    }

    public class MultirolTests
    {
        [Fact]
        public void El_administrador_puede_sumarse_roles_pero_no_quitarse_el_de_administrador()
        {
            var c = new Contexto();
            var admin = c.Usuarios.Agregar(new Usuario("admin", "Ana", "Admin", PasswordHasher.Hashear("admin123"), true, Rol.Administrador));

            var editado = c.UsuariosServicio.Editar(
                new UsuarioEditarDto(admin.Id, "Ana", "Admin", null, true, [Rol.Administrador, Rol.Contador]), admin.Id);

            Assert.Equal(new[] { Rol.Administrador, Rol.Contador }, editado.Roles);
            Assert.Contains(c.Auditorias.ObtenerTodos(), a => a.Detalle == "Roles: Administrador → Administrador, Contador");
            Assert.Throws<ReglaNegocioException>(() => c.UsuariosServicio.Editar(
                new UsuarioEditarDto(admin.Id, "Ana", "Admin", null, true, [Rol.Contador]), admin.Id));
        }

        [Fact]
        public void Un_usuario_con_varios_roles_ve_los_modulos_de_todos_sin_repetir()
        {
            var titulos = ModulosPorRol.De([Rol.Contador, Rol.Operador, Rol.Contador]).Select(m => m.Titulo);

            Assert.Equal(new[] { "Socios y cuotas", "Cuotas y morosos", "Ingresos", "Caja y movimientos" }, titulos);
        }
    }

    public class ExportarEgresosTests
    {
        [Fact]
        public void Exporta_los_egresos_vigentes_del_periodo_organizados_por_grupo()
        {
            var c = new Contexto();
            c.AbrirCaja();
            c.Ingresos.Registrar(new IngresoCrearDto(1000m, CategoriaIngreso.Donacion, c.Efectivo.Id), c.Operador.Id);
            c.Egresos.Registrar(new EgresoCrearDto(100m, c.Materiales.Id, c.Estudiantes.Id, c.Efectivo.Id), c.Contador.Id);
            c.Egresos.Registrar(new EgresoCrearDto(300m, c.Materiales.Id, c.Administracion.Id, c.Efectivo.Id), c.Contador.Id);
            var anulado = c.Egresos.Registrar(new EgresoCrearDto(50m, c.Materiales.Id, c.Estudiantes.Id, c.Efectivo.Id), c.Contador.Id);
            c.MovimientosServicio.Anular(anulado.Id, "Cargado dos veces", c.Contador.Id);

            Assert.Throws<ReglaNegocioException>(() => c.MovimientosServicio.ObtenerEgresos(c.Ahora, c.Ahora.AddDays(-1)));
            Assert.Empty(c.MovimientosServicio.ObtenerEgresos(c.Ahora.AddDays(1), c.Ahora.AddDays(2)));

            var egresos = c.MovimientosServicio.ObtenerEgresos(c.Ahora, c.Ahora);
            Assert.Equal(new[] { "Administración", "Estudiantes" }, egresos.Select(e => e.Grupo));

            // Mismo formato regional que fuerza la aplicación al arrancar.
            var culturaAnterior = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo("es-AR");
            string csv;
            try
            {
                csv = ExportarCsv.ArmarEgresosPorGrupo(egresos, c.Ahora, c.Ahora);
            }
            finally
            {
                CultureInfo.CurrentCulture = culturaAnterior;
            }

            Assert.Contains("Administración;1;$ 300,00;75,0 %", csv);
            Assert.Contains("Estudiantes;1;$ 100,00;25,0 %", csv);
            Assert.Contains("Total;2;$ 400,00;100,0 %", csv);
            Assert.Contains("Subtotal Estudiantes;;;$ 100,00;", csv);
        }
    }
}
