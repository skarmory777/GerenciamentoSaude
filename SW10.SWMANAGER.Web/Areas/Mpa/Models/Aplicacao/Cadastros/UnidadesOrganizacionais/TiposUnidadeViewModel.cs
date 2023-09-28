using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.UnidadesOrganizacionais
{
    public class TipoUnidadeOrganizacionalViewModel
    {
        public string Filtro { get; set; }

        public string PropriedadeSelecionada { get; set; }

        public SelectList TiposUnidadeOrganizacional { get; set; }
    }
}