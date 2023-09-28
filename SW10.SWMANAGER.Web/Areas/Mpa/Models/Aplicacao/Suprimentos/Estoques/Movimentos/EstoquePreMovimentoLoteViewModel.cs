using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    [AutoMap(typeof(EstoquePreMovimentoLoteValidadeDto))]
    public class EstoquePreMovimentoLoteViewModel : EstoquePreMovimentoLoteValidadeDto
    {
        public EstoquePreMovimentoLoteViewModel(EstoquePreMovimentoLoteValidadeDto output)
        {
            output.MapTo(this);
        }

    }
}