using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosAlta
{
    public class MotivosAltaViewModel
    {
        public SelectList TiposAlta { get; set; }

        public string Filtro { get; set; }
    }
}