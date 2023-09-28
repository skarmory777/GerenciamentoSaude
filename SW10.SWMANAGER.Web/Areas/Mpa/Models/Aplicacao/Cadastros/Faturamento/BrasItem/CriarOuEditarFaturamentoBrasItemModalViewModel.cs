using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasItens
{
    [AutoMapFrom(typeof(FaturamentoBrasItemDto))]
    public class CriarOuEditarFaturamentoBrasItemModalViewModel : FaturamentoBrasItemDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoBrasItemModalViewModel(FaturamentoBrasItemDto output)
        {
            output.MapTo(this);
        }
    }
}