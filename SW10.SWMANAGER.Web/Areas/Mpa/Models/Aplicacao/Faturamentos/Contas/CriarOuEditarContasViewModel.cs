using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Contas
{
    [AutoMapFrom(typeof(FaturamentoContaDto))]
    public class CriarOuEditarContaViewModel : FaturamentoContaDto
    {
        public UserEditDto UpdateUser { get; set; }

        //   public SelectList Estados { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarContaViewModel(FaturamentoContaDto output)
        {
            output.MapTo(this);
        }
    }
}