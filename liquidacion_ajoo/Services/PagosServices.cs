using liquidacion_ajoo.DTO;
using liquidacion_ajoo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liquidacion_ajoo.Services
{
    public class PagosServices
    {
        private readonly ApplicationDbcontext _context;

        private readonly ClientesServices _clientesServices;
        public PagosServices(ApplicationDbcontext context, ClientesServices clientesServices) {
            _context = context;
            _clientesServices = clientesServices;
        }

        internal async Task<ActionResult<PagosResponseDTO>> GetPagosCliente(int id)
        {
            Clientes_ajoo cliente = await _clientesServices.validarId(id);

            Task<Pagos_ajoo[]> pagosDTO = GetPagosClientId(id);

            decimal totalPagado = TotalPagado(pagosDTO.Result);

            if (pagosDTO.Result != null)
            {
                PagosResponseDTO pagosResponse = new PagosResponseDTO()
                {
                    ClienteId = id,
                    TotalPagado = (double)totalPagado,
                    nombre = cliente.Name,
                    pagos = pagosDTO.Result
                };
                return pagosResponse;
            }
            return null;
        }

        private decimal TotalPagado(Pagos_ajoo[] pagos)
        {
            decimal total = 0;
            foreach (Pagos_ajoo pago in pagos)
            {
                total += pago.Monto;
            }
            return total;
        }

        internal async Task<Pagos_ajoo[]> GetPagosClientId(int id)
        {
            Pagos_ajoo[] pagos = await _context.pagos_ajoos.Where(p => p.ClienteId == id).ToArrayAsync();

            return pagos;
        }



        public async Task<ActionResult> RegistrarPago(Pagos_ajoo body)
        {
            try
            {

                
                await validacionPagos(body);

                await _context.AddAsync(body);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Pago registrado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ObjectResult("Ocurrió un error al registrar el pago: " + ex)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }

        private async Task validacionPagos(Pagos_ajoo body)
        {
            if (body.Monto<0)
            {
                throw new Exception("El monto debe ser mayor a 0");
            }
            if (body.FechaPago > DateTime.Now)
            {
                throw new Exception("La fecha debe pago no puede ser futura");
            }

            if (!(body.Moneda == "USD" || body.Moneda == "EUR" || body.Moneda == "GBP"))
            {
                throw new Exception("""La moneda solo puede ser "USD", "EUR", "GBP".""");
            }

            await _clientesServices.validarId(body.ClienteId);



        }

        internal async Task<ActionResult<List<LiquidacionDTO>>> GetLiquidacion()
        {
            string[] monedas = { "USD", "EUR", "GBP" };

            List<LiquidacionDTO> liquidacion = new List<LiquidacionDTO>();

            foreach (string moneda in monedas)
            {
                decimal totalPagado = await _context.pagos_ajoos.Where(p => p.Moneda == moneda).SumAsync(p => p.Monto);

                if (totalPagado >0)
                {
                LiquidacionDTO liquidacionDTO = new LiquidacionDTO() {
                    Moneda = moneda,
                    total = totalPagado,
                };

                liquidacion.Add(liquidacionDTO);

                }

            }
            return liquidacion;

        }


    }
}
