using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    public class DropdownLeitosViewModel
    {
        public SelectList Leitos { get; set; }

        public long LeitoId { get; set; }

        public int AbaId { get; set; }
    }
}