using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposUnidade;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Dto
{
    [AutoMap(typeof(TipoUnidade))]
    public class CriarOuEditarTipoUnidade : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
    }
}
