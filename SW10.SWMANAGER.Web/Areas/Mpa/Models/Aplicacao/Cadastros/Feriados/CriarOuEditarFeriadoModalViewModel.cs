using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Feriados
{
    [AutoMapFrom(typeof(CriarOuEditarFeriado))]
    public class CriarOuEditarFeriadoModalViewModel : CriarOuEditarFeriado
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarFeriadoModalViewModel(CriarOuEditarFeriado output)
        {
            output.MapTo(this);
        }
    }
}