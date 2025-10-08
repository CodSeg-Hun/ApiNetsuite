using System.ComponentModel.DataAnnotations;

namespace ApiNetsuite.DTO.Bien
{
    public class BienDTO
    {
        [Required]
        public string plataforma { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido, por favor verificar")]
        public string cliente { get; set; }

        public string username { get; set; }

        public string amicliente { get; set; }

        [Required]
        public string codvehiculo { get; set; }

        public string vehiculoid { get; set; }

        public string amivehiculo { get; set; }

        public string descripcionvehiculo { get; set; }

        public string tipovehiculo { get; set; }

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

        public string idconcesionario { get; set; }

        public string concesionariodesc { get; set; }

        public string concesionariodire { get; set; }

        public string idfinanciera { get; set; }

        public string financieradesc { get; set; }

        public string financieradire { get; set; }

        public string idaseguradora { get; set; }

        public string aseguradoradesc { get; set; }

        public string aseguradoradire { get; set; }

        public string numeroorden { get; set; }

        public string idusuario { get; set; }


        public string ejecutiva { get; set; }

        public string estadocartera { get; set; }

        public string sucursal { get; set; }




    }
}
