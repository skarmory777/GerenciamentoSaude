using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TabelasDominio
{
    public class MontarComboTabelaDominioGruposViewModel
    {
        public long GrupoTipoTabelaDominioId { get; set; }
        public SelectList GruposTipoTabelaDominio { get; set; }
    }
}