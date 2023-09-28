namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class ReceituarioMedicoRetornoMedicoMemedDto
    {
        public Data data { get; set; }
        public class Attributes
        {
            public string token { get; set; }
        }
        public class Data
        {
            public Attributes attributes { get; set; }
            public int id { get; set; }
        }
    }
}
