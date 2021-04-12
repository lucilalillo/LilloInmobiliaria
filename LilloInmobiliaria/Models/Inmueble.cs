using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class Inmueble
    {
        [Display(Name ="Codigo")]
        public int IdInmueble { get; set; }
        
        public Propietario Prop { get; set; }

        public int PropietarioId { get; set; }
        [ForeignKey(nameof(PropietarioId))]

        [Required]
        public String Direccion { get; set; }

        [Required]
        public int CantAmbientes { get; set; }

        [Required]
        public String Uso { get; set; }

        [Required]
        public String Tipo { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public override string ToString()
        {
            return $"{Prop.Nombre} {Prop.Apellido} {Direccion} {CantAmbientes} {Uso} {Tipo} {Precio}";
        }
    }
}