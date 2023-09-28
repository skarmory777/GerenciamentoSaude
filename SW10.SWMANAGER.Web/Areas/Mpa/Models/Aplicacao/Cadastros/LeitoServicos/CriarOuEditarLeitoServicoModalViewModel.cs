using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.LeitoServicos
{
    [AutoMapFrom(typeof(CriarOuEditarLeitoServico))]
    public class CriarOuEditarLeitoServicoModalViewModel : CriarOuEditarLeitoServico
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarLeitoServicoModalViewModel(CriarOuEditarLeitoServico output)
        {
            output.MapTo(this);
        }
    }
}