namespace ApiNetsuite.DTO.Cliente
{
    public class RegDireccionDTO
    {

        public string Identificacion { get; set; }

        public string Cedula_Ruc { get; set; }

        public string Direccion { get; set; }

        public string Direccion1 { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string IdClienteNs { get; set; }

        public string NombreCliente { get; set; }

        public string IdPais { get; set; }

        public string Pais { get; set; }

        public string IdProvincia { get; set; }

        public string CodProvincia { get; set; }

        public string Provincia { get; set; }
         
        public string IdCanton { get; set; }

        public string CodCanton { get; set; }

        public string Canton { get; set; }

        public string IdParroquia { get; set; }

        public string CodParroquia { get; set; }

        public string Parroquia { get; set; }

        public int IdZona { get; set; } = 0;

        public string Zona { get; set; }

        public string IdTipoDireccion { get; set; }

        public string TipoDireccion { get; set; }

        public string IdDireccionNs { get; set; }

        public string IdDireccionDetNs { get; set; }

        public string DireccionPredeterminadaEnvio { get; set; }

        public string DireccionPredeterminadaFactura { get; set; }


    }
}
