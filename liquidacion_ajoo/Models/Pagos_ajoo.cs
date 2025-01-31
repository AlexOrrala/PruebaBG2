
namespace liquidacion_ajoo.Models
{
    public class Pagos_ajoo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
