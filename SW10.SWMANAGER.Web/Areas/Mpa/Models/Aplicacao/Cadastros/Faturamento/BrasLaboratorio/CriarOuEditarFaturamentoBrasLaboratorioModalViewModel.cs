using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasLaboratorios
{
    [AutoMapFrom(typeof(FaturamentoBrasLaboratorioDto))]
    public class CriarOuEditarFaturamentoBrasLaboratorioModalViewModel : FaturamentoBrasLaboratorioDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoBrasLaboratorioModalViewModel(FaturamentoBrasLaboratorioDto output)
        {
            output.MapTo(this);
        }
    }
}