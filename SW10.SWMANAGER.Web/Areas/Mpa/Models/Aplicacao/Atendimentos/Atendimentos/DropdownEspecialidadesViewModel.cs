using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    public class DropdownEspecialidadesViewModel
    {
        public SelectList Especialidades { get; set; }

        public long EspecialidadeId { get; set; }

        public int AbaId { get; set; }
    }
}