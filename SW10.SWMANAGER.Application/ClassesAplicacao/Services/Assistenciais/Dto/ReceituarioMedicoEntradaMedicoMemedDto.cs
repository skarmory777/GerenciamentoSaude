namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class ReceituarioMedicoEntradaMedicoMemedDto
    {
        public Data data { get; set; }
        public class Attributes
        {
            public string external_id { get; set; }
            public string nome { get; set; }
            public string sobrenome { get; set; }
            public string data_nascimento { get; set; }
            public string cpf { get; set; }
            public string uf { get; set; }
            public string sexo { get; set; }
            public string crm { get; set; }
        }
        public class Data
        {
            public Attributes attributes { get; set; }
            public string type { get; set; }
        }
    }
}
