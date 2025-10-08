using System.Numerics;

namespace ApiNetsuite.DTO.Factura
{
    public class DocAutorizarResp
    {

        
        public string status { get; set; } = "";

        public string message { get; set; } = "";

        public string idFactura { get; set; } = "";

        public string idDocumento { get; set; } = "";

        public string idCliente { get; set; } = "";

        public string cliente { get; set; } = "";

    }
}
