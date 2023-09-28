using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Regioes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Dto
{
    [AutoMap(typeof(Regiao))]
    public class RegiaoDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

    }
}
