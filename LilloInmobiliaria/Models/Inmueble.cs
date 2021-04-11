using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        
        public Propietario Prop { get; set; }

        public String Direccion { get; set; }

        public int CantAmbientes { get; set; }

        public String Uso { get; set; }

        public String Tipo { get; set; }

        public double Precio { get; set; }

        public override string ToString()
        {
            return $"{Prop.Nombre} {Prop.Apellido} {Direccion} {CantAmbientes} {Uso} {Tipo} {Precio}";
        }
    }
}