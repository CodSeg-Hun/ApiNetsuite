namespace ApiNetsuite.DTO.Cliente
{
    public class ClienteRespDTO
    {
        public string id_cliente { get; set; }

        public string nombres { get; set; }

        public string apellidos { get; set; }

        public string email { get; set; }

        public string plataforma { get; set; }

        public int status { get; set; }

        public string mensaje { get; set; }
    }
}
