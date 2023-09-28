using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosGruposTratamento
{
    [AutoMapFrom(typeof(CriarOuEditarProdutoGrupoTratamento))]
    public class CriarOuEditarProdutoGrupoTratamentoModalViewModel : CriarOuEditarProdutoGrupoTratamento
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList ProdutosGruposTratamento { get; set; }

        public CriarOuEditarProdutoGrupoTratamentoModalViewModel(CriarOuEditarProdutoGrupoTratamento output)
        {
            output.MapTo(this);
        }
    }
}