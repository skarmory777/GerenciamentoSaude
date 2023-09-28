using Abp.Application.Navigation;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos
{
    public class AtendimentosViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }

        public string Filtro { get; set; }
    }
}