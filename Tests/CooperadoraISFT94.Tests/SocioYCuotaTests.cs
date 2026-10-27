using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Tests
{
    public class SocioYCuotaTests
    {
        [Fact]
        public void Socio_puede_tener_un_responsable()
        {
            var responsable = Crear.Socio(Categoria.Responsable, "28456789");
            var socio = Crear.Socio();

            socio.AsignarResponsable(responsable);

            Assert.Same(responsable, socio.Responsable);
        }

        [Fact]
        public void El_responsable_debe_tener_categoria_Responsable()
        {
            var otro = Crear.Socio(Categoria.Socio, "28456789");

            Assert.Throws<NegocioException>(() => Crear.Socio().AsignarResponsable(otro));
        }

        [Fact]
        public void Un_Responsable_no_puede_tener_responsable()
        {
            var responsable = Crear.Socio(Categoria.Responsable);
            var otroResponsable = Crear.Socio(Categoria.Responsable, "28456789");

            Assert.Throws<NegocioException>(() => responsable.AsignarResponsable(otroResponsable));
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789")]
        [InlineData("30.123.456")]
        public void Dni_invalido_se_rechaza(string dni)
        {
            Assert.Throws<NegocioException>(() => Crear.Socio(dni: dni));
        }

        [Fact]
        public void Email_invalido_se_rechaza_y_vacio_se_acepta()
        {
            Assert.Throws<NegocioException>(() => new Socio(Crear.Id(), "A", "B", "30123456", "", "no-es-mail", Categoria.Socio));
            Assert.Equal(string.Empty, new Socio(Crear.Id(), "A", "B", "30123456", "", "  ", Categoria.Socio).Email);
        }

        [Fact]
        public void Cuota_nace_pendiente_y_pasa_a_pagada_una_sola_vez()
        {
            var cuota = new Cuota(Crear.Id(), Crear.Socio(), 5000m, 2026);
            Assert.Equal(EstadoCuota.Pendiente, cuota.Estado);

            cuota.MarcarPagada();

            Assert.True(cuota.EstaPagada);
            Assert.Throws<NegocioException>(() => cuota.ValidarPagable());
            Assert.Throws<NegocioException>(() => cuota.MarcarPagada());
        }

        [Theory]
        [InlineData(1999)]
        [InlineData(2101)]
        public void Cuota_con_anio_invalido_se_rechaza(int anio)
        {
            Assert.Throws<NegocioException>(() => new Cuota(Crear.Id(), Crear.Socio(), 5000m, anio));
        }

        [Fact]
        public void Detalle_de_cuota_exige_ingreso_de_cuota_social_por_el_monto_exacto()
        {
            var cuota = new Cuota(Crear.Id(), Crear.Socio(), 5000m, 2026);
            var medio = Crear.Efectivo();

            var donacion = Crear.Ingreso(5000m, medio, CategoriaIngreso.Donacion);
            Assert.Throws<NegocioException>(() => new DetalleIngresoCuota(Crear.Id(), donacion, cuota, 5000m, null));

            var cuotaSocial = Crear.Ingreso(5000m, medio, CategoriaIngreso.CuotaSocial);
            Assert.Throws<NegocioException>(() => new DetalleIngresoCuota(Crear.Id(), cuotaSocial, cuota, 4000m, null));

            var detalle = new DetalleIngresoCuota(Crear.Id(), cuotaSocial, cuota, 5000m, null);
            Assert.Same(cuota, detalle.Cuota);
        }
    }
}
