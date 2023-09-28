using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.LeitosStatus
{
    [AutoMapFrom(typeof(CriarOuEditarLeitoStatus))]
    public class CriarOuEditarLeitoStatusModalViewModel : CriarOuEditarLeitoStatus
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarLeitoStatusModalViewModel(CriarOuEditarLeitoStatus output)
        {
            output.MapTo(this);
        }
    }
}