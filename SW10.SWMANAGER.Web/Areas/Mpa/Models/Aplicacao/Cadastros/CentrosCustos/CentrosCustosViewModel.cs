using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.CentrosCustos
{
    public class CentrosCustosViewModel
    {
        public string Filtro { get; set; }

        public long UnidadeOrganizacionalId { get; set; }

        public SelectList UnidadesOrganizacionais { get; set; }
    }
}