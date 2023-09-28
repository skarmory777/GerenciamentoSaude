using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ConsultorTabelas
{
    public class ConsultorTabelasViewModel
    {
        public string Filtro { get; set; }

        public long CampoId { get; set; }

        public SelectList Campos { get; set; }
    }
}