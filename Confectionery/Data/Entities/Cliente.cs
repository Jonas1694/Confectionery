using System.ComponentModel.DataAnnotations;

namespace Confectionery.Data.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [Display(Name = "Tipo Documento")]
        public string TipoDocumento { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [Display(Name = "Rif o Cedula")]
        [StringLength(9, MinimumLength = 7, ErrorMessage = "El campo {0} debe ser una cadena con una longitud mínima de {2} y una longitud máxima de {1}.")]
        public string Rif { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [Display(Name = "Dirección Fiscal")]
        public string DireccionFiscal { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "El campo {0} debe ser una cadena con una longitud mínima de {2} y una longitud máxima de {1}.")]
        public string Telefono { get; set; }

        public string UsuarioId { get; set; }
        public string GetRif { get => $"{TipoDocumento}-{Rif} {RazonSocial}"; }
        public DateTime FechaRegistro { get; set; }

    }
}
