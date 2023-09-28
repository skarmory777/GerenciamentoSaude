using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Indicacoes
{
    [AutoMapFrom(typeof(CriarOuEditarIndicacao))]
    public class CriarOuEditarIndicacaoModalViewModel : CriarOuEditarIndicacao
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarIndicacaoModalViewModel(CriarOuEditarIndicacao output)
        {
            output.MapTo(this);
        }
    }
}