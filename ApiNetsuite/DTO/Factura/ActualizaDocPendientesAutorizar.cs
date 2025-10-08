namespace ApiNetsuite.DTO.Factura
{
    public class ActualizaDocPendientesAutorizar
    {
        public string idinternoDocumento { get; set; }

        public string numeroDocumento { get; set; }

        public string numeroAutorizacion { get; set; }

        public string fecha { get; set; }

        public string hora { get; set; }
      

        public string pdfPrincipal { get; set; }

        public string xmlPrincipal { get; set; }


    }
}
