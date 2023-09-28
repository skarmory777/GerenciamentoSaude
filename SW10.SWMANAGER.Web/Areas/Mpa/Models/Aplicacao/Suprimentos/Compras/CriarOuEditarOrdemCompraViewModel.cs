using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Compras
{
    [AutoMap(typeof(OrdemCompraDto))]
    public class CriarOuEditarOrdemCompraViewModel : OrdemCompraDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarOrdemCompraViewModel(OrdemCompraDto output)
        {
            output.MapTo(this);
        }
    }
}