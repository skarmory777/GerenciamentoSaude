using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposCentroCusto
{
    public class GruposCentroCustoViewModel
    {
        public string Filtro { get; set; }

        public long TipoGrupoCentroCustoId { get; set; }

        public SelectList TipoGrupoCentroCustos { get; set; }
    }
}
