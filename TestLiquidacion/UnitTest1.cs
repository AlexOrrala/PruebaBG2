using liquidacion_ajoo;
using liquidacion_ajoo.Controllers;
using liquidacion_ajoo.DTO;
using liquidacion_ajoo.Models;
using liquidacion_ajoo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestLiquidacion
{
    public class UnitTest1
    {
        private readonly ApplicationDbcontext _context;
        private readonly PagosController _pagosController;

        public UnitTest1()
        {
            // Crear opciones para un contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbcontext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbcontext(options);


            var clientesServies = new ClientesServices(_context);

            // Crear el servicio con el contexto en memoria
            var pagosServices = new PagosServices(_context, clientesServies); // Usamos el contexto real

            // Crear el controlador
            _pagosController = new PagosController(pagosServices);
        }

        [Theory]
        [InlineData(-100,"USD","2025-01-31",1)]
        [InlineData(100, "USD", "2025-02-01", 1)]
        public async Task RegistrarPago(decimal monto,string moneda,string fecha,int cliente)
        {
            // Arrange
            var pago = new Pagos_ajoo
            {
                Monto = monto, // Monto negativo
                Moneda = moneda,
                FechaPago = DateTime.Parse(fecha),
                ClienteId = cliente
            };

            // Act
            ActionResult result = await _pagosController.Post(pago);

            // Assert
            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode); // Verifica que el status code sea 500 para error
        }

        [Fact]
        public async Task ObtenerPagosPorCliente()
        {
            var clientesServies = new ClientesServices(_context);

            await clientesServies.AddCliente(new Clientes_ajoo
            {
                Name = "Prueba",
                Correo = "prueba.com"
            });
            var PagoEsperado = new Pagos_ajoo { Id = 1, ClienteId = 1, FechaPago = DateTime.Now, Moneda = "USD", Monto = 200 };

            await _pagosController.Post(PagoEsperado);

            // Act
            ActionResult<PagosResponseDTO> result = await _pagosController.GetPagosCliente(1);

            // Assert
           
            //var pagoEsperado = new PagosResponseDTO{ ClienteId=1,nombre="Prueba", pagos=[Pagos_ajoo{ Id = 1, ClienteId = 1, FechaPago = DateTime.Now, Moneda = "USD", Monto = 200 }], TotalPagado=200};
            


            
        }

    }
}
