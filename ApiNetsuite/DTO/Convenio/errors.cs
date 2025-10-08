using System.Collections.Generic;

namespace ApiNetsuite.DTO.Convenio
{
    public class RootError
    {
        public Errors errors { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string traceId { get; set; }
    }

    public class Errors
    {
        public List<string> error { get; set; }
    }
}
