using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    public class ProdutosViewModel
    {
        public string Filtro { get; set; }
        public long? GrupoId { get; set; }
        public long? GrupoClasseId { get; set; }
        public long? GrupoSubClasseId { get; set; }

        public SelectList FiltroPrincipais { get; set; }
        public SelectList FiltroStatus { get; set; }
        public SelectList Grupos { get; set; }
        public SelectList Classes { get; set; }
        public SelectList SubClasses { get; set; }

        public long? DCBId { get; set; }
        public DcbDto DCB { get; set; }
    }

    /// <summary>
    /// Define a opçao do filtro para produto principal
    /// Tipo: Sim / Nao/ Todos
    /// </summary>
    public class FiltroPrincipal
    {
        public long? Tipo { get; set; }
    }

}