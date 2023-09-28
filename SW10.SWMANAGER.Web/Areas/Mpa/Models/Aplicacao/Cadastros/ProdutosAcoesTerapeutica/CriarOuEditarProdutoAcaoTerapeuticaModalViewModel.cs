using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosAcoesTerapeutica
{
    [AutoMap(typeof(ProdutoAcaoTerapeuticaDto))]
    public class CriarOuEditarProdutoAcaoTerapeuticaModalViewModel : ProdutoAcaoTerapeuticaDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList ProdutosAcoesTerapeutica { get; set; }

        public CriarOuEditarProdutoAcaoTerapeuticaModalViewModel(ProdutoAcaoTerapeuticaDto output)
        {
            output.MapTo(this);
        }
    }
}