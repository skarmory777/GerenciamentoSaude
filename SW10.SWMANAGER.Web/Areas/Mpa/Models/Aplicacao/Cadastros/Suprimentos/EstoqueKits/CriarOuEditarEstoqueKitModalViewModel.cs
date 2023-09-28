using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Suprimentos.EstoqueKits
{
    [AutoMapFrom(typeof(EstoqueKitDto))]
    public class CriarOuEditarEstoqueKitModalViewModel : EstoqueKitDto
    {
        public UserEditDto UpdateUser { get; set; }
        public ProdutoDto Item { get; set; }
        public string GrupoId { get; set; }
        public string GrupoClasseId { get; set; }
        public int Quantidade { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarEstoqueKitModalViewModel(EstoqueKitDto output)
        {
            output.MapTo(this);
        }
    }
}