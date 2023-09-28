using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.PreAtendimentos
{
    public class PreAtendimentosViewModel
    {
        public SelectList Sexos { get; set; }

        public string Filtro { get; set; }
    }
}