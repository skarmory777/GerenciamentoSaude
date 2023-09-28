using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao
{
    public class IndexViewModel
    {
        public string Filtro { get; set; }

        public SelectList Empresas { get; set; }
    }
}