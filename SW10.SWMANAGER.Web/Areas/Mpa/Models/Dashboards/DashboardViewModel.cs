using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Dashboards
{
    public class DashboardViewModel
    {
        public string Filtro { get; set; }

        public long? EmpresaId { get; set; }

        public SelectList Empresas { get; set; }
    }
}