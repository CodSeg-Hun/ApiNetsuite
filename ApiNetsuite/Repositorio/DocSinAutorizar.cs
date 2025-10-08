namespace ApiNetsuite.DTO.Factura
{
    public class DocSinAutorizar
    {
        public string idinternoDocumento { get; set; }

        public string numeroDocumento { get; set; }

        public string numeroAutorizacion { get; set; }

        public string fecha { get; set; }

        public string hora { get; set; }

        public string estadoAutorizacion { get; set; }

        public string pdfRetencion { get; set; }

        public string xmlRetencion { get; set; }

    }
}
