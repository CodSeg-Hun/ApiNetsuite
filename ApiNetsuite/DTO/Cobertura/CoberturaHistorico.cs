namespace ApiNetsuite.DTO.Cobertura
{
    public class CoberturaHistorico
    {
        public string bienId { get; set; } = "";

        public string ordenId { get; set; } = "";

        public string numeroGeneral { get; set; } = "";

        public string origen { get; set; } = "";

        public string docOrigenId { get; set; } = "";

        public string productoId { get; set; } = "";

        public string producto { get; set; } = "";

        public string fechaInicio { get; set; } = "";

        public string fechaFin { get; set; } = "";

        public string plazo { get; set; } = "";

        public string tipoPlazo { get; set; } = "";

        public string estadoId { get; set; } = "";

        public string fechaInicioAnt { get; set; } = "19000101";

        public string fechaFinAnt { get; set; } = "19000101";

        public string estadoInstalacion { get; set; } = "";

        public string ejecutivaId { get; set; } = "";

        public string usuarioId { get; set; } = "";


    }
}
