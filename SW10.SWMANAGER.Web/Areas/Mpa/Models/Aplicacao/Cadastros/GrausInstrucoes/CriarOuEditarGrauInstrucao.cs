using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GrausInstrucoes
{
    [AutoMapFrom(typeof(CriarOuEditarGrauInstrucao))]
    public class CriarOuEditarGrauInstrucaoModalViewModel : CriarOuEditarGrauInstrucao
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarGrauInstrucaoModalViewModel(CriarOuEditarGrauInstrucao output)
        {
            output.MapTo(this);
        }
    }
}