using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposPrestadores
{
    [AutoMapFrom(typeof(CriarOuEditarTipoPrestador))]
    public class CriarOuEditarTipoPrestadorModalViewModel : CriarOuEditarTipoPrestador
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarTipoPrestadorModalViewModel(CriarOuEditarTipoPrestador output)
        {
            output.MapTo(this);
        }
    }
}