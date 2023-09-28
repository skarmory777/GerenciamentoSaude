using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposAcomodacao
{
    [AutoMap(typeof(TipoAcomodacaoDto))]
    public class CriarOuEditarTipoAcomodacaoModalViewModel : TipoAcomodacaoDto
    {
        //public TipoAcomodacaoDto TipoAcomodacao { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarTipoAcomodacaoModalViewModel(TipoAcomodacaoDto output)
        {
            output.MapTo(this);
        }
    }
}