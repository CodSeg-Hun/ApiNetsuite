using ApiNetsuite.DTO.OrdenServicio;
using System.Collections.Generic;

namespace ApiNetsuite.DTO.Cliente
{
    public class RegClienteDTO
    {
        public string Id_ns_cliente { get; set; }

        public string Identificacion { get; set; }

        public string Nombre_Completo { get; set; }

        public string Primer_Nombre { get; set; }

        public string Segundo_Nombre { get; set; }

        public string Apellido_Paterno { get; set; }

        public string Apellido_Materno { get; set; }

        //public string TIPO_DOCUMENTO { get; set; }

        //public string TIPO_CLIENTE_D { get; set; }

        //public string COD_TIPO_CLIENTE { get; set; }

        //public string COD_TIPO_PERSONA { get; set; }

        public string Tipo_Cliente { get; set; }

        public string Tipo_Persona { get; set; }

        public string Id_Ejecutiva { get; set; }

        public string Id_Telematic { get; set; }

        public string Oficina { get; set; }

        public List<RegDireccionDTO> Direccion { get; set; }

        public List<RegEmailsDTO> Email { get; set; }

        public List<RegTelefonoDTO> Telefono { get; set; }
    }
}
