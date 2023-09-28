using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstMovimentoBaixaAppService : IApplicationService
    {
        bool PossuiVales(long preMovimentoId);
        bool PossuiNota(long preMovimentoId);
        Task<decimal> QuantidadeBaixaItemPendente(long movimentoItemId);
        Task<EstMovimentoBaixaItemDto> ObterItemBaixa(long baixaItemid);
        bool PossuiItemConsignados(long preMovimentoId);
        Task<decimal> Editar(MovimentoItemDto input);
    }
}
