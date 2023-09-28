using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Desenvolvimento.DocItens
{
    public class DocItensListagemViewModel
    {
        public string Filtro { get; set; }
        public DocItemDto DocItem { get; set; }
        public DocRotuloDto DocCapitulo { get; set; }
        public DocRotuloDto DocSessao { get; set; }
        public DocRotuloDto DocAssunto { get; set; }

        public DocItensListagemViewModel()
        {
            DocItem = new DocItemDto();
            DocCapitulo = new DocRotuloDto();
            DocSessao = new DocRotuloDto();
            DocAssunto = new DocRotuloDto();
        }
    }
}