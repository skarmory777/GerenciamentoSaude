using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosTransferenciaLeito
{
    [AutoMapFrom(typeof(CriarOuEditarMotivoTransferenciaLeito))]
    public class CriarOuEditarMotivoTransferenciaLeitoModalViewModel : CriarOuEditarMotivoTransferenciaLeito
    {
        //public MotivoTransferenciaLeitoDto MotivoTransferenciaLeito { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarMotivoTransferenciaLeitoModalViewModel(CriarOuEditarMotivoTransferenciaLeito output)
        {
            output.MapTo(this);
        }
    }
}