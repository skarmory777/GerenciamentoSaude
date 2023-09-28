using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    [AutoMap(typeof(ProdutoRelacaoLaboratorioDto))]
    public class CriarOuEditarProdutoRelacaoLaboratorioModalViewModel : ProdutoRelacaoLaboratorioDto

    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Laboratorios { get; set; }

        public CriarOuEditarProdutoRelacaoLaboratorioModalViewModel(ProdutoRelacaoLaboratorioDto output)
        {
            output.MapTo(this);
        }
    }
}