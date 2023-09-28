using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Cep
{
    public class CepsViewModel
    {
        public string Filtro { get; set; }

        public long CidadeId { get; set; }

        public SelectList Cidades { get; set; }

        public long EstadoId { get; set; }

        public SelectList Estados { get; set; }

        public long PaisId { get; set; }

        public SelectList Paises { get; set; }

        public SelectList TiposLogradouro { get; set; }
    }
}