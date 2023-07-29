using System.ComponentModel.DataAnnotations;

namespace Confectionery.Data.Entities
{
	public class Productos
	{
		public int ProductosId { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		[Display(Name = "Producto")]
		public string Descripcion { get; set; }

		[Required(ErrorMessage = "Este campo es obligatorio!")]
		[Display(Name = "Descripción Detallada del Producto")]
		public string DetalleDescripcion { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		[Display(Name = "Cantidad del producto")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
		public int Stock { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		[Display(Name = "Stock Minimo")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
		public int StockMin { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		[Display(Name = "Stock Maximo")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
		public int StockMax { get; set; }

		public decimal Precio { get; set; }

		[Required(ErrorMessage = "Debe seleccionar el Proveedor del Producto!")]
		[Display(Name = "Proveedor")]
		public int ProveedorId { get; set; }

		public string GetDescripcion { get => $"{Descripcion}"; }
		public DateTime FechaRegistro { get; set; }

	}
}
