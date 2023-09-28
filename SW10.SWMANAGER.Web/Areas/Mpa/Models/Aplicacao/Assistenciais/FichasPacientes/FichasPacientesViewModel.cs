using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.FichasPacientes
{
    public class FichasPacientesViewModel
    {
        public string Filtro { get; set; }

        public bool IsEditMode { get { return this.CriarOuEditarPacienteModalViewModel.Id > 0; } }
        public CriarOuEditarPacienteModalViewModel CriarOuEditarPacienteModalViewModel { get; set; }
    }
}