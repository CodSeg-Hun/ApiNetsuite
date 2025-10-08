using Newtonsoft.Json;

namespace ApiNetsuite.DTO.Digitalizacion
{
    public class DigitalizacionResp
    {
        [JsonProperty(PropertyName = "CODIGO_ESCANEADO")]
        public string Codigo { get; set; } = "";

        public string mensaje { get; set; } = "";

    }
}
