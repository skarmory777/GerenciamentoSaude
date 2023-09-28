using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros.ControleBancario
{
    public class ControleBancarioViewModel : QuitacaoDto
    {

        public string Filtro { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
    }
}