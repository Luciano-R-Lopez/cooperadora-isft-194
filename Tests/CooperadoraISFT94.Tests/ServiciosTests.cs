using System.Globalization;
using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Tests
{
    public class SocioServiceTests
    {
        [Fact]
        public void No_se_repite_el_DNI_al_registrar_ni_al_editar()
        {
            Contexto c = new Contexto();
            c.Socio("30123456");
            Socio otro = c.Socio("28456789");

            Assert.Throws<NegocioException>(() => c.SociosServicio.Guardar(c.DatosSocio("30123456"), c.Operador.Id));
            Assert.Throws<NegocioException>(() => c.SociosServicio.Guardar(c.DatosSocio("30123456", otro.Id), c.Operador.Id));
        }

        [Fact]
        public void Editar_conserva_el_propio_DNI_y_audita_lo_que_cambio()
        {
            Contexto c = new Contexto();
            Socio socio = c.Socio("30123456");
            SocioDTO datos = c.SociosServicio.ObtenerPorId(socio.Id);
            datos.Nombre = "Lucía";
            datos.Telefono = "1155550101";

            c.SociosServicio.Guardar(datos, c.Operador.Id);

            Assert.Equal("Lucía", socio.Nombre);
            Auditoria auditoria = c.Auditorias.ObtenerTodos().Last();
            Assert.Equal(AccionAuditoria.Modificacion, auditoria.Accion);
            Assert.Contains("Nombre: Nombre → Lucía", auditoria.Detalle);
            Assert.DoesNotContain("DNI", auditoria.Detalle);
        }

        [Fact]
        public void Un_dato_invalido_al_editar_no_cambia_nada()
        {
            Contexto c = new Contexto();
            Socio socio = c.Socio("30123456");
            SocioDTO datos = c.SociosServicio.ObtenerPorId(socio.Id);
            datos.Nombre = "Otro";
            datos.Email = "no-es-mail";

            Assert.Throws<NegocioException>(() => c.SociosServicio.Guardar(datos, c.Operador.Id));
            Assert.Equal("Nombre", socio.Nombre);
        }

        [Fact]
        public void Un_responsable_con_socios_a_cargo_no_deja_de_serlo_ni_se_da_de_baja()
        {
            Contexto c = new Contexto();
            Socio responsable = c.Socio("28456789", Categoria.Responsable);
            c.Socio("40111222", responsable: responsable);

            Assert.Throws<NegocioException>(() =>
                c.SociosServicio.Guardar(c.DatosSocio("28456789", responsable.Id, Categoria.Socio), c.Operador.Id));
            Assert.Throws<NegocioException>(() => c.SociosServicio.CambiarEstado(responsable.Id, false, c.Operador.Id));
            Assert.True(responsable.EsResponsable);
            Assert.True(responsable.Activo);
        }

        [Fact]
        public void Un_socio_dado_de_baja_no_recibe_cuotas_nuevas_y_se_puede_reactivar()
        {
            Contexto c = new Contexto();
            Socio socio = c.Socio();
            CuotaDTO cuota = new CuotaDTO();
            cuota.SocioId = socio.Id;
            cuota.Anio = 2026;
            cuota.Monto = 5000m;

            c.SociosServicio.CambiarEstado(socio.Id, false, c.Operador.Id);
            Assert.Throws<NegocioException>(() => c.CuotasServicio.Generar(cuota, c.Operador.Id));

            c.SociosServicio.CambiarEstado(socio.Id, true, c.Operador.Id);
            c.CuotasServicio.Generar(cuota, c.Operador.Id);
            Assert.Single(c.CuotasServicio.ObtenerPorSocio(socio.Id));
        }
    }

    public class CuotaServiceTests
    {
        [Fact]
        public void Generar_para_todos_solo_genera_a_activos_sin_cuota_del_anio_y_fija_el_valor()
        {
            Contexto c = new Contexto();
            Socio conCuota = c.Socio("30123456");
            c.Cuota(conCuota, anio: 2026);
            c.Socio("28456789");
            Socio deBaja = c.Socio("40111222");
            deBaja.DarDeBaja();

            GeneracionCuotasDTO resultado = c.CuotasServicio.GenerarParaTodos(2026, 6000m, c.Operador.Id);

            Assert.Equal(1, resultado.Generadas);
            Assert.Equal(1, resultado.YaTenian);
            Assert.Empty(c.CuotasServicio.ObtenerPorSocio(deBaja.Id));
            Assert.Equal(6000m, c.CuotasServicio.ObtenerMontoSugerido(2026));
        }

        [Fact]
        public void Sin_caja_abierta_el_pago_falla_y_la_cuota_queda_pendiente()
        {
            Contexto c = new Contexto();
            Cuota cuota = c.Cuota(c.Socio());

            Assert.Throws<NegocioException>(() => c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id, null));
            Assert.False(cuota.EstaPagada);
            Assert.Empty(c.Movimientos.ObtenerTodos());
        }

        [Fact]
        public void El_pago_acredita_la_caja_marca_la_cuota_y_devuelve_el_recibo()
        {
            Contexto c = new Contexto();
            c.AbrirCaja(1000m);
            Cuota cuota = c.Cuota(c.Socio());

            ReciboPagoDTO recibo = c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id, null);

            Assert.True(cuota.EstaPagada);
            Assert.Equal(6000m, c.CajaAbierta().MontoTotal);
            Assert.Equal(5000m, recibo.Monto);
            Assert.Equal("Efectivo", recibo.MedioPago);
            Assert.Equal(string.Empty, recibo.PagadoPor);
        }

        [Fact]
        public void El_responsable_puede_pagar_la_cuota_de_su_socio_a_cargo_y_otro_socio_no()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            Socio responsable = c.Socio("28456789", Categoria.Responsable);
            Socio otroResponsable = c.Socio("27000000", Categoria.Responsable);
            Socio aCargo = c.Socio("40111222", responsable: responsable);
            Cuota cuota = c.Cuota(aCargo);

            Assert.Throws<NegocioException>(() =>
                c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id, otroResponsable.Id));
            Assert.False(cuota.EstaPagada);

            ReciboPagoDTO recibo = c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id, responsable.Id);

            Assert.True(cuota.EstaPagada);
            Assert.NotEqual(string.Empty, recibo.PagadoPor);
            Assert.Same(responsable, c.Detalles.ObtenerTodos().Single().PagadaPor);
        }
    }

    public class AnulacionTests
    {
        [Fact]
        public void Anular_el_cobro_de_una_cuota_revierte_la_caja_y_la_cuota_vuelve_a_pendiente()
        {
            Contexto c = new Contexto();
            c.AbrirCaja(1000m);
            Cuota cuota = c.Cuota(c.Socio());
            ReciboPagoDTO recibo = c.CuotasServicio.RegistrarPago(cuota.Id, c.Efectivo.Id, c.Operador.Id, null);

            MovimientoDTO anulado = c.MovimientosServicio.Anular(recibo.Numero, "Cobro duplicado", c.Contador.Id);

            Assert.True(anulado.Anulado);
            Assert.False(cuota.EstaPagada);
            Assert.Equal(1000m, c.CajaAbierta().MontoTotal);

            ResumenCajaDTO resumen = c.CajasServicio.ObtenerResumenActivo(null);
            Assert.Equal(0m, resumen.TotalIngresos);
            Assert.Equal(1, resumen.CantidadAnulados);
            Assert.Equal(1000m, resumen.MontoEsperado);
        }

        [Fact]
        public void No_se_anula_dos_veces_ni_sin_motivo()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            IngresoDTO ingreso = c.Donacion(100m);

            Assert.Throws<NegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "  ", c.Contador.Id));
            c.MovimientosServicio.Anular(ingreso.Id, "Error de carga", c.Contador.Id);
            Assert.Throws<NegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "Otra vez", c.Contador.Id));
        }

        [Fact]
        public void No_se_anulan_movimientos_de_una_caja_cerrada()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            IngresoDTO ingreso = c.Donacion(100m);
            c.CajasServicio.Cerrar(100m, c.Contador.Id);

            Assert.Throws<NegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "Tarde", c.Contador.Id));
        }

        [Fact]
        public void Anular_un_ingreso_ya_gastado_falla_por_saldo_insuficiente_sin_cambiar_nada()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            IngresoDTO ingreso = c.Donacion(100m);
            c.Egreso(80m, c.Estudiantes);

            Assert.Throws<NegocioException>(() => c.MovimientosServicio.Anular(ingreso.Id, "Error", c.Contador.Id));
            Assert.Equal(20m, c.CajaAbierta().MontoTotal);
            Assert.False(c.Movimientos.ObtenerExistente(ingreso.Id).Anulado);
        }

        [Fact]
        public void El_resumen_de_una_caja_cerrada_compara_contra_lo_contado()
        {
            Contexto c = new Contexto();
            c.AbrirCaja(100m);
            c.Donacion(50m);
            ResumenCajaDTO cerrada = c.CajasServicio.Cerrar(140m, c.Contador.Id);

            ResumenCajaDTO resumen = c.CajasServicio.ObtenerResumen(cerrada.Caja.Id);

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
            Contexto c = new Contexto();

            for (int i = 0; i < 4; i++)
            {
                Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(Contexto.Login("operador", "mal")));
            }
            AutenticacionException bloqueo = Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(Contexto.Login("operador", "mal")));
            Assert.Contains("bloqueado", bloqueo.Message);

            // Bloqueado: ni con la contraseña correcta.
            Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(Contexto.Login("operador", "operador123")));

            c.Ahora = c.Ahora.AddMinutes(Configuracion.MinutosBloqueoLogin + 1);
            Assert.Equal("operador", c.Auth.Autenticar(Contexto.Login("OPERADOR", "operador123")).NombreUsuario);
        }

        [Fact]
        public void Un_ingreso_correcto_reinicia_los_intentos()
        {
            Contexto c = new Contexto();
            for (int i = 0; i < 4; i++)
            {
                Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(Contexto.Login("operador", "mal")));
            }
            c.Auth.Autenticar(Contexto.Login("operador", "operador123"));

            for (int i = 0; i < 4; i++)
            {
                Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(Contexto.Login("operador", "mal")));
            }
            c.Auth.Autenticar(Contexto.Login("operador", "operador123"));
        }

        [Fact]
        public void Cambiar_la_propia_contrasenia_exige_la_actual_y_la_confirmacion()
        {
            Contexto c = new Contexto();
            int id = c.Operador.Id;

            Assert.Throws<NegocioException>(() => c.UsuariosServicio.CambiarPasswordPropia(id, Cambio("mal", "nueva123", "nueva123")));
            Assert.Throws<NegocioException>(() => c.UsuariosServicio.CambiarPasswordPropia(id, Cambio("operador123", "nueva123", "otra123")));

            c.UsuariosServicio.CambiarPasswordPropia(id, Cambio("operador123", "nueva123", "nueva123"));

            c.Auth.Autenticar(Contexto.Login("operador", "nueva123"));
            Assert.Throws<AutenticacionException>(() => c.Auth.Autenticar(Contexto.Login("operador", "operador123")));
        }

        [Fact]
        public void El_administrador_puede_sumarse_roles_pero_no_quitarse_el_de_administrador()
        {
            Contexto c = new Contexto();
            Usuario admin = c.AgregarUsuario("admin", "admin123", Rol.Administrador);
            UsuarioDTO datos = c.UsuariosServicio.ObtenerPorId(admin.Id);
            datos.Roles = new List<Rol> { Rol.Administrador, Rol.Contador };

            UsuarioDTO editado = c.UsuariosServicio.Guardar(datos, admin.Id);

            Assert.Equal(new[] { Rol.Administrador, Rol.Contador }, editado.Roles);
            Assert.Contains(c.Auditorias.ObtenerTodos(), a => a.Detalle == "Roles: Administrador → Administrador, Contador");

            datos.Roles = new List<Rol> { Rol.Contador };
            Assert.Throws<NegocioException>(() => c.UsuariosServicio.Guardar(datos, admin.Id));
        }

        [Fact]
        public void No_se_repite_el_nombre_de_usuario()
        {
            Contexto c = new Contexto();
            UsuarioDTO nuevo = new UsuarioDTO();
            nuevo.NombreUsuario = "OPERADOR";
            nuevo.Nombre = "Otro";
            nuevo.Apellido = "Operador";
            nuevo.Password = "clave123";
            nuevo.Roles = new List<Rol> { Rol.Operador };

            Assert.Throws<NegocioException>(() => c.UsuariosServicio.Guardar(nuevo, c.Operador.Id));
        }

        private static CambioPasswordDTO Cambio(string actual, string nueva, string confirmacion)
        {
            CambioPasswordDTO dto = new CambioPasswordDTO();
            dto.Actual = actual;
            dto.Nueva = nueva;
            dto.Confirmacion = confirmacion;
            return dto;
        }
    }

    public class CatalogoYAuditoriaTests
    {
        [Fact]
        public void No_se_repiten_nombres_y_los_desactivados_no_se_ofrecen_ni_se_usan()
        {
            Contexto c = new Contexto();
            c.AbrirCaja(1000m);

            Assert.Throws<NegocioException>(() => c.Catalogos.Guardar(Item(TipoCatalogo.MedioPago, "efectivo"), 1));
            c.Catalogos.CambiarEstado(TipoCatalogo.MedioPago, c.Transferencia.Id, false, 1);

            Assert.DoesNotContain(c.Catalogos.ObtenerItems(TipoCatalogo.MedioPago, false), m => m.Id == c.Transferencia.Id);
            Assert.Contains(c.Catalogos.ObtenerItems(TipoCatalogo.MedioPago, true), m => m.Id == c.Transferencia.Id);
            Assert.Throws<NegocioException>(() => c.Egreso(10m, c.Estudiantes, c.Transferencia));
        }

        [Fact]
        public void Renombrar_a_un_nombre_repetido_deja_el_nombre_anterior()
        {
            Contexto c = new Contexto();
            ItemCatalogoDTO renombrar = Item(TipoCatalogo.MedioPago, "Efectivo");
            renombrar.Id = c.Transferencia.Id;

            Assert.Throws<NegocioException>(() => c.Catalogos.Guardar(renombrar, 1));
            Assert.Equal("Transferencia", c.Transferencia.Nombre);
        }

        [Fact]
        public void La_auditoria_se_filtra_por_accion_y_fecha()
        {
            Contexto c = new Contexto();
            int socioId = c.SociosServicio.Guardar(c.DatosSocio("30123456"), c.Operador.Id);
            c.Ahora = c.Ahora.AddDays(2);
            c.SociosServicio.CambiarEstado(socioId, false, c.Operador.Id);

            AuditoriaFiltroDTO bajas = new AuditoriaFiltroDTO();
            bajas.Accion = AccionAuditoria.Baja;
            Assert.Single(c.Auditoria.Obtener(bajas));

            AuditoriaFiltroDTO desdeHoy = new AuditoriaFiltroDTO();
            desdeHoy.Desde = c.Ahora.Date;
            Assert.Single(c.Auditoria.Obtener(desdeHoy));

            AuditoriaFiltroDTO alReves = new AuditoriaFiltroDTO();
            alReves.Desde = c.Ahora;
            alReves.Hasta = c.Ahora.AddDays(-1);
            Assert.Throws<NegocioException>(() => c.Auditoria.Obtener(alReves));
        }

        private static ItemCatalogoDTO Item(TipoCatalogo tipo, string nombre)
        {
            ItemCatalogoDTO dto = new ItemCatalogoDTO();
            dto.Tipo = tipo;
            dto.Nombre = nombre;
            return dto;
        }
    }

    public class CobroMultipleYMorososTests
    {
        [Fact]
        public void Varias_cuotas_del_socio_se_cobran_en_un_solo_ingreso_y_recibo()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            Socio socio = c.Socio();
            Cuota vieja = c.Cuota(socio, 4500m, 2025);
            Cuota actual = c.Cuota(socio, 5000m, 2026);

            ReciboPagoDTO recibo = c.CuotasServicio.RegistrarPagos(new List<int> { actual.Id, vieja.Id }, c.Efectivo.Id, c.Operador.Id, null);

            Assert.True(vieja.EstaPagada);
            Assert.True(actual.EstaPagada);
            Assert.Single(c.Movimientos.ObtenerTodos());
            Assert.Equal(9500m, recibo.Monto);
            Assert.Equal(new[] { 2025, 2026 }, recibo.Cuotas.Select(l => l.Anio));
            Assert.Equal(9500m, c.CajaAbierta().MontoTotal);
        }

        [Fact]
        public void Sin_responsable_no_se_cobran_juntas_cuotas_de_distintos_socios_y_nada_cambia()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            Cuota una = c.Cuota(c.Socio("30123456"));
            Cuota otra = c.Cuota(c.Socio("28456789"));

            Assert.Throws<NegocioException>(() =>
                c.CuotasServicio.RegistrarPagos(new List<int> { una.Id, otra.Id }, c.Efectivo.Id, c.Operador.Id, null));
            Assert.Throws<NegocioException>(() =>
                c.CuotasServicio.RegistrarPagos(new List<int>(), c.Efectivo.Id, c.Operador.Id, null));
            Assert.False(una.EstaPagada);
            Assert.Empty(c.Movimientos.ObtenerTodos());
        }

        [Fact]
        public void El_responsable_paga_juntas_sus_cuotas_y_las_de_sus_socios_a_cargo()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            Socio responsable = c.Socio("28456789", Categoria.Responsable);
            Socio hija = c.Socio("40111222", responsable: responsable);
            Socio hijo = c.Socio("40111333", responsable: responsable);
            Cuota propia = c.Cuota(responsable);

            ReciboPagoDTO recibo = c.CuotasServicio.RegistrarPagos(
                new List<int> { propia.Id, c.Cuota(hija).Id, c.Cuota(hijo).Id }, c.Efectivo.Id, c.Operador.Id, responsable.Id);

            Assert.Equal(15000m, recibo.Monto);
            Assert.NotEqual(string.Empty, recibo.PagadoPor);
            IReadOnlyList<DetalleIngresoCuota> detalles = c.Detalles.ObtenerTodos();
            Assert.Null(detalles.Single(d => d.Cuota.Id == propia.Id).PagadaPor);
            Assert.Equal(2, detalles.Count(d => ReferenceEquals(d.PagadaPor, responsable)));
        }

        [Fact]
        public void Los_deudores_salen_uno_por_socio_con_sus_anios_y_las_familias_juntas()
        {
            Contexto c = new Contexto();
            Socio zapata = c.Socio("30000001", Categoria.Responsable, apellido: "Zapata");
            Socio alba = c.Socio("30000002", responsable: zapata, apellido: "Alba");
            Socio mena = c.Socio("30000003", apellido: "Mena");

            c.Cuota(alba, 4500m, 2025);
            c.Cuota(alba, 5000m, 2026);
            c.Cuota(zapata, 5000m, 2026);
            c.Cuota(mena, 5000m, 2026);

            DeudoresFiltroDTO todos = new DeudoresFiltroDTO();
            todos.IncluirDadosDeBaja = true;
            List<DeudorDTO> deudores = c.CuotasServicio.ObtenerDeudores(todos);

            // Mena primero; después la familia Zapata: el responsable y luego su socio a cargo (Alba).
            Assert.Equal(new[] { "Mena, Nombre", "Zapata, Nombre", "Alba, Nombre" }, deudores.Select(d => d.Socio));
            DeudorDTO deudaAlba = deudores.Single(d => d.SocioId == alba.Id);
            Assert.Equal("2025, 2026", deudaAlba.AniosQueDebe);
            Assert.Equal(9500m, deudaAlba.Total);
            Assert.Contains("Zapata", deudaAlba.Paga);

            DeudoresFiltroDTO del2025 = new DeudoresFiltroDTO();
            del2025.Anio = 2025;
            del2025.IncluirDadosDeBaja = true;
            Assert.Single(c.CuotasServicio.ObtenerDeudores(del2025));
        }

        [Fact]
        public void Los_dados_de_baja_se_pueden_ocultar()
        {
            Contexto c = new Contexto();
            Socio socio = c.Socio();
            c.Cuota(socio);
            socio.DarDeBaja();

            DeudoresFiltroDTO filtro = new DeudoresFiltroDTO();
            filtro.IncluirDadosDeBaja = true;
            Assert.Single(c.CuotasServicio.ObtenerDeudores(filtro));
            filtro.IncluirDadosDeBaja = false;
            Assert.Empty(c.CuotasServicio.ObtenerDeudores(filtro));
        }
    }

    public class GrupoEgresoTests
    {
        [Fact]
        public void Un_mismo_motivo_puede_ser_de_distintos_grupos_y_se_filtra_por_grupo()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            c.Donacion(1000m);
            c.Egreso(100m, c.Estudiantes);
            c.Egreso(200m, c.Administracion);

            MovimientoFiltroDTO filtro = new MovimientoFiltroDTO();
            filtro.CajaId = c.CajaAbierta().Id;
            filtro.GrupoEgresoId = c.Estudiantes.Id;
            MovimientoDTO egreso = Assert.Single(c.MovimientosServicio.Obtener(filtro));
            Assert.Equal(100m, egreso.Monto);
            Assert.Equal("Estudiantes", egreso.Grupo);
            Assert.Equal("Materiales", egreso.Clasificacion);

            filtro.GrupoEgresoId = null;
            filtro.Tipo = TipoMovimiento.Ingreso;
            MovimientoDTO ingreso = Assert.Single(c.MovimientosServicio.Obtener(filtro));
            Assert.Equal("-", ingreso.Grupo);
            Assert.Equal("Donación", ingreso.Clasificacion);
        }

        [Fact]
        public void No_se_puede_usar_un_grupo_desactivado()
        {
            Contexto c = new Contexto();
            c.AbrirCaja(1000m);

            c.Catalogos.CambiarEstado(TipoCatalogo.GrupoEgreso, c.Administracion.Id, false, 1);

            Assert.DoesNotContain(c.Catalogos.ObtenerItems(TipoCatalogo.GrupoEgreso, false), g => g.Id == c.Administracion.Id);
            Assert.Throws<NegocioException>(() => c.Egreso(10m, c.Administracion));
        }

        [Fact]
        public void Los_egresos_del_periodo_salen_vigentes_y_ordenados_por_grupo()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            c.Donacion(1000m);
            c.Egreso(100m, c.Estudiantes);
            c.Egreso(300m, c.Administracion);
            EgresoDTO anulado = c.Egreso(50m, c.Estudiantes);
            c.MovimientosServicio.Anular(anulado.Id, "Cargado dos veces", c.Contador.Id);

            Assert.Throws<NegocioException>(() => c.MovimientosServicio.ObtenerEgresos(c.Ahora, c.Ahora.AddDays(-1)));
            Assert.Empty(c.MovimientosServicio.ObtenerEgresos(c.Ahora.AddDays(1), c.Ahora.AddDays(2)));
            Assert.Equal(new[] { "Administración", "Estudiantes" },
                c.MovimientosServicio.ObtenerEgresos(c.Ahora, c.Ahora).Select(e => e.Grupo));
        }
    }

    public class ExportarEgresosTests
    {
        [Fact]
        public void El_csv_de_egresos_tiene_el_resumen_por_grupo_y_los_subtotales()
        {
            Contexto c = new Contexto();
            c.AbrirCaja();
            c.Donacion(1000m);
            c.Egreso(100m, c.Estudiantes);
            c.Egreso(300m, c.Administracion);
            List<MovimientoDTO> egresos = c.MovimientosServicio.ObtenerEgresos(c.Ahora, c.Ahora);

            // Mismo formato regional que fija la aplicación al arrancar.
            CultureInfo culturaAnterior = CultureInfo.CurrentCulture;
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
