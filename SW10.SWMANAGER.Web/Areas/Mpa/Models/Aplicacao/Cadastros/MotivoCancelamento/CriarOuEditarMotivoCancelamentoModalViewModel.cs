using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosCancelamento
{
    [AutoMapFrom(typeof(CriarOuEditarMotivoCancelamento))]
    public class CriarOuEditarMotivoCancelamentoModalViewModel : CriarOuEditarMotivoCancelamento
    {
        //public MotivoCancelamentoDto MotivoCancelamento { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarMotivoCancelamentoModalViewModel(CriarOuEditarMotivoCancelamento output)
        {
            output.MapTo(this);
        }
    }
}