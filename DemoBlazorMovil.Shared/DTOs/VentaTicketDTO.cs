namespace DemoBlazorMovil.Shared.DTOs
{
    public class VentaTicketDTO
    {
        public string MovieNombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Sala { get; set; }
        public int Asiento { get; set; }
        public decimal Precio { get; set; }
    }
}
