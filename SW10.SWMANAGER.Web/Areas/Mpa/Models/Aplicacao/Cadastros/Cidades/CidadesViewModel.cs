using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Cidades
{
    public class CidadesViewModel
    {
        public string Filtro { get; set; }

        public long EstadoId { get; set; }

        public SelectList Estados { get; set; }
    }
}