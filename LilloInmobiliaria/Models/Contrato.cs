using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        [DisplayName("Codigo")]
        public int IdContrato { get; set; }

        public Inmueble Inmueble { get; set; }

        [Required, DisplayName("Dueño")]
        public int InmuebleId { get; set; }

        public Inquilino Inquilino { get; set; }

        [Required, DisplayName("Inquilino")]
        public int InquilinoId { get; set; }

        [Required, DisplayName("Fecha Inicio Contrato")]
        public DateTime FecInicio { get; set; }

        [Required, DisplayName("Fecha Fin contrato")]
        public DateTime FecFin { get; set; }

        public decimal Monto { get; set; }

        public bool Estado { get; set; }
    }
}
