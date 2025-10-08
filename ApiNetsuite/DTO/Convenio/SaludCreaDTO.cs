namespace ApiNetsuite.DTO.Convenio
{
    public class SaludCreaDTO
    {

        public string identification { get; set; } = "";

        public string lastname { get; set; } = "";

        public string firstname { get; set; } = "";

        public string birthdate { get; set; } = "";


        public string address { get; set; } = "";

        public string email { get; set; } = "";

        public string amount { get; set; } = "";

        public string certificate_number { get; set; } = "";


        public string gender { get; set; } = "";

        public string city { get; set; } = "";

        public string civil_status { get; set; } = "";

        public string identification_type { get; set; } = "";


        public string telephone { get; set; } = "";

        public string movil { get; set; } = "";

        public string payment_type { get; set; } = "";

        public string product_code { get; set; } = "";

        public string plan_code { get; set; } = "";


        public string sale_date { get; set; } = "";

        public AgentDTO agent { get; set; }



    }

    public class AgentDTO
    {
        public string identification { get; set; }

        public string profile { get; set; }

        public string level1_zonification { get; set; }

        public string level2_zonification { get; set; }

        public string level3_zonification { get; set; }

    }
}
