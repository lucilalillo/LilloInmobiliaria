using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class Inquilino
    {
        [Key]
        [Display(Name ="Codigo")]
        public int IdInquilino { get; set; }

        [Required]
        public String Nombre { get; set; }

        [Required]
        public String Apellido { get; set; }

        [Required]
        public String Dni { get; set; }

        [Required]
        public String Telefono { get; set; }

        [Required, EmailAddress]
        public String Email { get; set; }

        [Required, DataType(DataType.Password)]
        public String ClaveInq { get; set; }

        public override string ToString()
        {
            return $"{IdInquilino} {Nombre} {Apellido} {Dni} {Telefono} {Email}";
        }
    }
}
