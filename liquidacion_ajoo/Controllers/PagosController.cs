using liquidacion_ajoo.DTO;
using liquidacion_ajoo.Models;
using liquidacion_ajoo.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace liquidacion_ajoo.Controllers
{
    [Route("api/pagos")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        public PagosServices _pagosServices;
        public PagosController(PagosServices pagosServices) { 
            _pagosServices = pagosServices;
        }

        
        // GET api/pagos/5
        [HttpGet("{id}")]
        public Task<ActionResult<PagosResponseDTO>> GetPagosCliente(int id)
        {
            Task<ActionResult<PagosResponseDTO>> actionResult = _pagosServices.GetPagosCliente(id);
            return actionResult;
        }

        // POST api/pagos
        [HttpPost]
        public Task<ActionResult> Post([FromBody] Pagos_ajoo body)
        {
            Task<ActionResult> actionResult =  _pagosServices.RegistrarPago(body);
            return actionResult;

        }

        // GET api/pagos/
        [HttpGet]
        public Task<ActionResult<List<LiquidacionDTO>>> GetLiquidacion()
        {
            Task<ActionResult<List<LiquidacionDTO>>> actionResult = _pagosServices.GetLiquidacion();
            return actionResult;
        }



    }
}
