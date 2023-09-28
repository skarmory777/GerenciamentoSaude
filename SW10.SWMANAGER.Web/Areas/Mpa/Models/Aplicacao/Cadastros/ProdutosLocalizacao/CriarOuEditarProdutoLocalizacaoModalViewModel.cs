using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosLocalizacao
{
    [AutoMapFrom(typeof(ProdutoLocalizacaoDto))]
    public class CriarOuEditarProdutoLocalizacaoModalViewModel : ProdutoLocalizacaoDto
    {
        //public TipoAtendimentoDto TipoAtendimento { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarProdutoLocalizacaoModalViewModel(ProdutoLocalizacaoDto output)
        {
            output.MapTo(this);
        }
    }
}