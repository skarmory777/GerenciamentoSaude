using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposTabelaDominio
{
    [AutoMapFrom(typeof(CriarOuEditarTipoTabelaDominio))]
    public class CriarOuEditarTipoTabelaDominioModalViewModel : CriarOuEditarTipoTabelaDominio
    {
        //public TipoTabelaDominioDto TipoTabelaDominio { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarTipoTabelaDominioModalViewModel(CriarOuEditarTipoTabelaDominio output)
        {
            output.MapTo(this);
        }
    }
}