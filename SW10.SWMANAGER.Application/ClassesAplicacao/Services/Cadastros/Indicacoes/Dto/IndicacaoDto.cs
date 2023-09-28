using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Indicacoes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Dto
{
    [AutoMap(typeof(Indicacao))]
    public class IndicacaoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

    }
}
