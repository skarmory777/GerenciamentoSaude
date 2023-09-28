using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Itens
{
    public class ItensViewModel
    {
        public string Filtro { get; set; }

        public long GrupoId { get; set; }

        public SelectList Grupos { get; set; }

        public long SubGrupoId { get; set; }

        public SelectList SubGrupos { get; set; }

        public long TipoId { get; set; }

        public SelectList Tipos { get; set; }
    }
}