using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class ReceituarioMedicoRecuperaLinkPrescricaoMemedDto
    {
        public List<Datum> data { get; set; }
        public Links links { get; set; }
        public Meta meta { get; set; }
        public class Attributes
        {
            public string link { get; set; }
            public int signed { get; set; }
            public int digits { get; set; }
        }
        public class Datum
        {
            public string type { get; set; }
            public Attributes attributes { get; set; }
        }
        public class Links
        {
            public string self { get; set; }
        }
        public class Meta
        {
            public int total { get; set; }
        }
    }
}
