namespace SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto
{
    using System;

    public class IndexDisparoDeMensagemViewModel : CamposPadraoCRUDDto
    {
        public string NomeCompleto { get; set; }

        public DateTime? Nascimento { get; set; }

        public string Idade { get; set; }

        public string Telefone1 { get; set; }

        public string Telefone2 { get; set; }

        public string Telefone3 { get; set; }

        public string Telefone4 { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Email4 { get; set; }

        public string Pais { get; set; }

        public string Estado { get; set; }

        public string Cidade { get; set; }

        public string Bairro { get; set; }

        public string Atendimento { get; set; }

        public string DataAtendimento { get; set; }

        public string DataAltaAtendimento { get; set; }

        public string UnidadeAtendimento { get; set; }

        public string StatusAtendimento { get; set; }
    }
}
