using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Pessoas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto
{
    [AutoMap(typeof(SisTipoPessoa))]
    public class SisTipoPessoaDto : CamposPadraoCRUDDto
    {
        public bool IsReceber { get; set; }
        public bool IsPagar { get; set; }
        public bool IsAtivo { get; set; }
    }
}
