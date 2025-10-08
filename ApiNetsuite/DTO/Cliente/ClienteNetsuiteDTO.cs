using System.Collections.Generic;

namespace ApiNetsuite.DTO.Cliente
{
    public class ClienteNetsuiteDTO
    {
        public string accion { get; set; }

        public string id { get; set; }

        public string customform { get; set; }

        public string entityid { get; set; }

        public string isperson { get; set; }

        public string companyname { get; set; }

        public string primernombre { get; set; }

        public string segundonombre { get; set; }

        public string primerapellido { get; set; }

        public string segundoapellido { get; set; }

        public string concesionario { get; set; }

        public string entitystatus { get; set; }

        public string salesrep { get; set; }

        public string ejerenova { get; set; }

        public string category { get; set; }

        public string subsidiary { get; set; }

        public string oficinacliente { get; set; }

        public string document_type { get; set; }

        public string vatregnumber { get; set; }

        public string tipo_persona { get; set; }

        public string pais_entidad { get; set; }

        public List<GenDireccionNetsuite> addressbook { get; set; }

        public List<GenMailNetsuite> correo { get; set; }

        public List<GenTelefonoNetsuite> telefono { get; set; }

    }
}
