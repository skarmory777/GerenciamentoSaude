using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosCaucao
{
    [AutoMapFrom(typeof(CriarOuEditarMotivoCaucao))]
    public class CriarOuEditarMotivoCaucaoModalViewModel : CriarOuEditarMotivoCaucao
    {
        //public MotivoCaucaoDto MotivoCaucao { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarMotivoCaucaoModalViewModel(CriarOuEditarMotivoCaucao output)
        {
            output.MapTo(this);
        }
    }
}