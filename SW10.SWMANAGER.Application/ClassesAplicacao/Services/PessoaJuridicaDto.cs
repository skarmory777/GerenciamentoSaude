using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(PessoaJuridica))]
    public abstract class PessoaJuridicaDto : PessoaDto
    {
        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; }

        public string Cnpj { get; set; }

        public string InscricaoEstadual { get; set; }

        public string InscricaoMunicipal { get; set; }
    }
}
