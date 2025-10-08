using System.ComponentModel.DataAnnotations;

namespace ApiNetsuite.DTO.Cliente
{
    public class ClienteDTO
    {
        [Required]
        public string id_cliente { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido, por favor verificar")]
        public string primer_nombre { get; set; }

        public string segundo_nombre { get; set; }

        public string apellido_paterno { get; set; }

        public string apellido_materno { get; set; }

        public string direccion { get; set; }

        public string telefono_convencional { get; set; }

        public string telefono_celular { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido, por favor verificar")]
        public string email { get; set; }

        public string id_usuario { get; set; }

        public string numeroorden { get; set; }

        [Required]
        public string plataforma { get; set; }

        // public string ami_monitoreo { get; set; }

        public string email_ami { get; set; }

        public string ami_tipodocumento { get; set; }

        //public string ami_propietario { get; set; }

        //public string ami_financiera { get; set; }

        //public string ami_asociado { get; set; }

        public string amicliente { get; set; }

        public string ami_company { get; set; }

        public string ami_support { get; set; }

        public string ami_telefono { get; set; }

        public string ami_celular { get; set; }

        public string ejecutiva { get; set; }

        public string sucursal { get; set; }

    }
}
