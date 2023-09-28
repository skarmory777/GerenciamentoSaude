using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Unidades
{
    [AutoMap(typeof(UnidadeDto))]
    public class CriarOuEditarUnidadeModalViewModel : UnidadeDto
    {
        //public UnidadeDto Unidade { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarUnidadeModalViewModel(UnidadeDto output)
        {
            output.MapTo(this);
        }
    }
}