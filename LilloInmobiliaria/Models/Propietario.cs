using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class Propietario
    {
        public int IdPropietario { get; set; }

        public String Nombre { get; set; }

        public String Apellido { get; set; }

        public String Dni { get; set; }

        public String Telefono { get; set; }

        public String Email { get; set; }

        public String ClaveProp { get; set; }

        public override string ToString()
        {
            return $"{IdPropietario} {Nombre} {Apellido} {Dni} {Telefono} {Email}";
        }



    }
}
