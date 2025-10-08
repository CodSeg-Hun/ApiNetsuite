using System.Collections.Generic;

namespace ApiNetsuite.DTO.Factura
{
    public class DocPendienteRespCli
    {

        public string status { get; set; }

        public string count { get; set; }

        public List<DetDocPendienteRespCli> results { get; set; }

    }
}
