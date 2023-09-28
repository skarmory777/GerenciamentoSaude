using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Controladorias.NotasFiscais
{
    public class NotasFiscaisViewModel
    {
        public string Filtro { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public long EmpresaId { get; set; }

        public SelectList Empresas { get; set; }
        //public virtual SelectList Empresas { get; set; }
    }
}