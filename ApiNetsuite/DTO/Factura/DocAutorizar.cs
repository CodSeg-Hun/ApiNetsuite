using ApiNetsuite.DTO.Factura;
using System.Collections.Generic;
using System.Numerics;

namespace ApiNetsuite.DTO.Factura
{
    public class DocAutorizar
    {

        public int opcion { get; set; }

        public string idOrdenServicio { get; set; }

        public string codvehiculo { get; set; }

        public string identificacion { get; set; }

        public string formaPago { get; set; }

        public string oficina { get; set; }

        public string clase { get; set; }

        public string placa { get; set; }

        public List<DocAutorizarDetItem> items { get; set; }

    }
}
