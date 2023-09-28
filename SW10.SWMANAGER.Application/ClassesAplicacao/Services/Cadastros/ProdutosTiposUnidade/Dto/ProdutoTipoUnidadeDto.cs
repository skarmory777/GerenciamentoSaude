using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Dto
{
    [AutoMap(typeof(UnidadeTipo))]
    //[AutoMap(typeof(ProdutoTipoUnidade))]
    public class ProdutoTipoUnidadeDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
