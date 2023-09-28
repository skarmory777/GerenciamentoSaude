using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GrausInstrucoes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Dto
{
    [AutoMap(typeof(GrauInstrucao))]
    public class CriarOuEditarGrauInstrucao : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

    }
}
