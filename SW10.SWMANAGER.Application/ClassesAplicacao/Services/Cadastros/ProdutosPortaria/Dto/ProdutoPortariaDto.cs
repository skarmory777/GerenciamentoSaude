using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPortaria;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto
{
    [AutoMap(typeof(ProdutoPortaria))]
    public class ProdutoPortariaDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

    }
}