namespace Confectionery.Data.Entities
{
    public class Cliente
    {
        public int id { get; set; }
        public string Rif { get; set; }
        public string RazonSocial { get; set; }
        public string DireccionFiscal { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
