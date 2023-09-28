using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    public class ProdutosUnidadeTipoViewModel
    {
        public long ProdutoId { get; set; }

        public ICollection<ProdutoUnidadeDto> ProdutosUnidadeTipo { get; set; }
    }
}