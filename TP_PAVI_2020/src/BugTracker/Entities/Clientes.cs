using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Entities
{
    public class Clientes
    {
        public int IDCLiente { get; set; }
        public string RazonSocial { get; set; }
        public int Cuit { get; set; }
        public int Borrado { get; set; }
        public string Calle { get; set; }
        public int NumeroCalle { get; set; }
        public DateTime FechaAlta { get; set; }
        public Barrio Barrio { get; set; }



        public override string ToString()
        {
            return RazonSocial;
        }
    }
    
}
