using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposTipoTabelaDominio
{
    public class GruposTipoTabelaDominioViewModel
    {
        public string Filtro { get; set; }

        public long TipoTabelaDominioId { get; set; }

        public SelectList TiposTabelaDominio { get; set; }
    }
}