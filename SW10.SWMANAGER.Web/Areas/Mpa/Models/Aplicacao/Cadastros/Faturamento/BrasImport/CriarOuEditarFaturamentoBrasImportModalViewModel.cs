using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasImports
{
    [AutoMapFrom(typeof(FaturamentoBrasImportDto))]
    public class CriarOuEditarFaturamentoBrasImportModalViewModel : FaturamentoBrasImportDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoBrasImportModalViewModel(FaturamentoBrasImportDto output)
        {
            output.MapTo(this);
        }
    }
}