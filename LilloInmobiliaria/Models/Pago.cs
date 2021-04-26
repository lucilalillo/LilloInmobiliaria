using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class Pago
    {
        [Key]
        [DisplayName("Codigo")]
        public int IdPago { get; set; }

        [DisplayName("Numero de pago")]
        public int NroPago { get; set; }

        [DisplayName("Fecha de pago"), DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        public decimal Importe { get; set; }

        public int ContratoId { get; set; }

        public Contrato Contrato { get; set; }
    }
}
