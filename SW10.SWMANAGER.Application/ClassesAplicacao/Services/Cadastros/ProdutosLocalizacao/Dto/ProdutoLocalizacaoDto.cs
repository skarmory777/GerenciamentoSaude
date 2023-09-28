using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLocalizacao;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Dto
{
    [AutoMap(typeof(ProdutoLocalizacao))]
    public class ProdutoLocalizacaoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public string Sigla { get; set; }
    }
}
