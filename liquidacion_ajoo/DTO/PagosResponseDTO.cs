using liquidacion_ajoo.Models;

namespace liquidacion_ajoo.DTO
{
    public class PagosResponseDTO
    {
        public int ClienteId { get; set; }
        public string nombre { get; set; }
        public double TotalPagado { get; set; }
        public Pagos_ajoo[] pagos { get; set; }

        
    }
}
