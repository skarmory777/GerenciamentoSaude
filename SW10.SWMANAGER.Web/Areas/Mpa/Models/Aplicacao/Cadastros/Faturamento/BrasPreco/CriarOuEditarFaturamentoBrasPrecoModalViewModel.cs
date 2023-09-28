using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasPrecos
{
    [AutoMapFrom(typeof(FaturamentoBrasPrecoDto))]
    public class CriarOuEditarFaturamentoBrasPrecoModalViewModel : FaturamentoBrasPrecoDto
    {
        public UserEditDto UpdateUser { get; set; }

        //      public ProdutoDto Produto { get; set; }

        public long ProdutoId { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoBrasPrecoModalViewModel(FaturamentoBrasPrecoDto output)
        {
            output.MapTo(this);
        }
    }
}