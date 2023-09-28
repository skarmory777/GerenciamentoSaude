using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosCodigosMedicamento
{
    [AutoMapFrom(typeof(ProdutoCodigoMedicamentoDto))]
    public class CriarOuEditarProdutoCodigoMedicamentoModalViewModel : ProdutoCodigoMedicamentoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList ProdutosCodigoMedicamentoDto { get; set; }

        public CriarOuEditarProdutoCodigoMedicamentoModalViewModel(ProdutoCodigoMedicamentoDto output)
        {
            output.MapTo(this);
        }
    }
}