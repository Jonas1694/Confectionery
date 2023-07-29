using System.ComponentModel.DataAnnotations;

namespace Confectionery.Data.Entities
{
	public class Facturacion
	{
		public int FacturacionId { get; set; }
		public int ClienteId { get; set; }
		public Cliente Clientes { get; set; }
		[Display(Name = "N Documento")]
		public double NFactura { get; set; }
		public decimal SubTotal { get; set; }
		public decimal TotalIva { get; set; }
		public decimal Iva { get; set; }
		public decimal Total { get; set; }
		public string UsuarioId { get; set; }
		public User User { get; set; }
		public int StatusDocumentoId { get; set; }
		[Display(Name = "Status")]
		public DateTime FechaRegistro { get; set; }
	}
}
