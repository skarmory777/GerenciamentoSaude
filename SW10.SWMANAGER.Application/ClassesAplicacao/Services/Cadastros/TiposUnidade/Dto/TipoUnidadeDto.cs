using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Dto
{
    //[AutoMap(typeof(TipoUnidade))]
    [AutoMap(typeof(UnidadeTipo))]
    public class TipoUnidadeDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
