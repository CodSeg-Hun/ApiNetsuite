using System.ComponentModel.DataAnnotations;

namespace ApiNetsuite.DTO.Cliente
{
    public class PropietarioDTO
    {
        [Required]
        public string plataforma { get; set; }

        public string codvehiculo { get; set; }

        public string tipo { get; set; }

        public string placa { get; set; }

        public string idmarca { get; set; }

        public string marca { get; set; }

        public string idmodelo { get; set; }

        public string modelo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido, por favor verificar")]
        public string chasis { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido, por favor verificar")]
        public string motor { get; set; }

        public string color { get; set; }

        public string anio { get; set; }

        public string numeroorden { get; set; }

        public string idusuario { get; set; }

        public string numero_documento { get; set; }


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
    }
}
