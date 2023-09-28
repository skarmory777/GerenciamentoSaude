using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Profissoes
{
    [AutoMapFrom(typeof(CriarOuEditarProfissao))]
    public class CriarOuEditarProfissaoModalViewModel : CriarOuEditarProfissao
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarProfissaoModalViewModel(CriarOuEditarProfissao output)
        {
            output.MapTo(this);
        }
    }
}