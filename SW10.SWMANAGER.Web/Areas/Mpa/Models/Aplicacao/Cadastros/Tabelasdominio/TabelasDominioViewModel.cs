using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TabelasDominio
{
    public class TabelasDominioViewModel
    {
        public string Filtro { get; set; }

        public long TipoTabelaId { get; set; }

        public long VersaoTissId { get; set; }

        public SelectList TiposTabela { get; set; }

        public SelectList VersoesTiss { get; set; }
    }
}