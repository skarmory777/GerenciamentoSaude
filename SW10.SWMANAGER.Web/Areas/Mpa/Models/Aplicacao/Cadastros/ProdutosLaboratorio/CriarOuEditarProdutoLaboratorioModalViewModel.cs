using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosLaboratorio
{
    [AutoMap(typeof(ProdutoLaboratorioDto))]
    public class CriarOuEditarProdutoLaboratorioModalViewModel : ProdutoLaboratorioDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public FaturamentoBrasLaboratorioDto BrasLaboratorio { get; set; }

        public SelectList ProdutosLaboratorio { get; set; }

        public CriarOuEditarProdutoLaboratorioModalViewModel(ProdutoLaboratorioDto output)
        {
            output.MapTo(this);
        }
    }
}