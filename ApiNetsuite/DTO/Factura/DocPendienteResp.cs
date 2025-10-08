using System.Collections.Generic;

namespace ApiNetsuite.DTO.Factura
{
    public class DocPendienteResp
    {

        public string status { get; set; }

        public string count { get; set; }

        public List<DetDocPendienteResp> results { get; set; }

    }
}
