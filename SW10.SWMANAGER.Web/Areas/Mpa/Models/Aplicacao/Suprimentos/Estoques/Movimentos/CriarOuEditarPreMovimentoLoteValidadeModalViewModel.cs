using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    [AutoMap(typeof(EstoquePreMovimentoLoteValidadeDto))]
    public class CriarOuEditarEstoquePreMovimentoLoteValidadeDtoModalViewModel : EstoquePreMovimentoLoteValidadeDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Laboratorios { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarEstoquePreMovimentoLoteValidadeDtoModalViewModel(EstoquePreMovimentoLoteValidadeDto output)
        {
            output.MapTo(this);
        }
    }
}