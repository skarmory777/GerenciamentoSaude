using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Naturalidades
{
    [AutoMapFrom(typeof(CriarOuEditarNaturalidade))]
    public class CriarOuEditarNaturalidadeModalViewModel : CriarOuEditarNaturalidade
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarNaturalidadeModalViewModel(CriarOuEditarNaturalidade output)
        {
            output.MapTo(this);
        }
    }
}