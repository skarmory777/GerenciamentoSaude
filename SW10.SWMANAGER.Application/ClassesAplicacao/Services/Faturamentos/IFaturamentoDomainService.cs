using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Dtos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Calculos
{
    public interface IFaturamentoDomainService
    {
        Task<DefaultReturn<ValorCodigoTabela>> ValorFaturamentoItem(ValorTotalItemFaturamentoDto input);
    }
}