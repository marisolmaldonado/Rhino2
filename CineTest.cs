using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace JustMockTest
{
    [TestClass]
    public class CineTest
    {
        //Dado ---        Given --   Arrange -- Dado que existen butacas libres
        //Cuando--      When --   Act --         Cuando compro Tickets
        //Entonces--    Then --    Assert --    Entonces las butacas se reservan
        [TestMethod]
        public void SiExistenButacasLibresComproEntradasEntoncesReservoButacas()
        {
            string pelicula = "La Vida Es Bella";
            int cantidadEntrada = 2;

            var cine = Mock.Create<Cine>();
            cine.Arrange(cine1 => cine1.ButacasLibres(pelicula)).Returns(20);
            cine.Arrange(cine1 => cine1.Descargar(pelicula, cantidadEntrada)).Returns(new List<string> { "E1", "E2" });

            Persona persona = new Persona();
            persona.CompraEntradas(cine, cantidadEntrada, pelicula);

            Assert.IsTrue(persona.ObtuvoEntradas);
            cine.Assert(cine1 => cine1.ButacasLibres(pelicula));
            cine.Assert(cine1 => cine1.Descargar(pelicula, cantidadEntrada));
        }

        [TestMethod]
        public void DadoQueNoHayButacasCuandoComproEntradasEntoncesNoReservoButacas()
        {
            string pelicula = "La Vida Es Bella";
            int cantidadEntrada = 2;

            var cine = Mock.Create<Cine>();
            cine.Arrange(cineN => cineN.ButacasLibres(pelicula)).Returns(0);

            Persona persona = new Persona();
            persona.CompraEntradas(cine, cantidadEntrada, pelicula);

            Assert.IsFalse(persona.ObtuvoEntradas);
            cine.Assert(cineN => cineN.ButacasLibres(pelicula));
        }

        [TestMethod]
        public void DadoQueNoHayTodasLasEntradasQueQuieroCuandoComproConsigoLasQueEstanDisponibles()
        {
            string pelicula = "La Vida Es Bella";
            int cantidadEntrada = 6;
            int entradaDisponible = 5;

            var cine = Mock.Create<Cine>();
            cine.Arrange(cineD => cineD.ButacasLibres(pelicula)).Returns(entradaDisponible);
            cine.Arrange(cineD => cineD.Descargar(pelicula, entradaDisponible)).Returns(new List<string> { "E1", "E2", "E3", "E4", "E5" });

            Persona persona = new Persona();
            persona.CompraEntradas(cine, cantidadEntrada, pelicula);

            Assert.IsTrue(persona.ObtuvoEntradas);
            Assert.AreEqual(entradaDisponible, persona.Entradas.Count);
            cine.Assert(cineD => cineD.ButacasLibres(pelicula));
            cine.Assert(cineD => cineD.Descargar(pelicula, entradaDisponible));
        }

        [TestMethod]
        public void DadoQueNoHayTodasLasEntradasQueQuieroCuandoComproConsigoSoloUna()
        {
            string pelicula = "La Vida Es Bella";
            int cantidadEntrada = 7;
            int entradasDisponible = 5;
            int entradasEsperadas = 1;

            var cine = Mock.Create<Cine>();
            cine.Arrange(cineF => cineF.ButacasLibres(pelicula)).Returns(entradasDisponible);
            cine.Arrange(cineF => cineF.Descargar(pelicula, entradasEsperadas)).Returns(new List<string> { "E1" });

            var modeloCompra = Mock.Create<IModeloCompra>();
            modeloCompra.Arrange(m => m.Comprar(cine, cantidadEntrada, pelicula)).Returns(new List<string> { "E1" });

            Persona persona = new Persona();
            persona.CompraEntradas(cine, cantidadEntrada, pelicula, modeloCompra);

            Assert.IsTrue(persona.ObtuvoEntradas);
            Assert.AreEqual(entradasEsperadas, persona.Entradas.Count);
            modeloCompra.Assert(m => m.Comprar(cine, cantidadEntrada, pelicula));
            cine.Assert(cineF => cineF.ButacasLibres(pelicula));
            cine.Assert(cineF => cineF.Descargar(pelicula, entradasEsperadas));
        }
    }
}
