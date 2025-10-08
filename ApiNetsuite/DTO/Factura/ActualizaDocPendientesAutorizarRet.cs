using System.Security.Policy;

namespace ApiNetsuite.DTO.Factura
{
    public class ActualizaDocPendientesAutorizarRet
    {
        public string idinternoDocumento { get; set; }

        public string numeroDocumento { get; set; }

        public string numeroAutorizacionRetencion { get; set; }

        public string fecha { get; set; }

        public string hora { get; set; }


        public string estadoAutorizacionRetencion { get; set; }

        public string pdfRetencion { get; set; }

        public string xmlRetencion { get; set; }


    }
}
