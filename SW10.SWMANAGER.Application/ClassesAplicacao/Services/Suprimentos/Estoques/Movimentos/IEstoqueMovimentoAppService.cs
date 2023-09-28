using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoqueMovimentoAppService : IApplicationService
    {
        Task<DefaultReturn<object>> GerarMovimentoEntrada(long preMovimentoId);
        Task<DefaultReturn<Object>> GerarMovimentoTransferencia(long transferenciaId);
        Task<ListResultDto<MovimentoIndexDto>> ListarBaixaValePendente(ListarEstoquePreMovimentoInput input);
        Task<EstoqueMovimentoDto> Obter(long id);
        Task<ListResultDto<MovimentoIndexDto>> listarMovimentosValeSelecionados(ListarEstoquePreMovimentoInput input);
        DefaultReturn<EstoqueMovimentoDto> CriarOuEditar(EstoqueMovimentoDto input, string valesIds);
        Task<ListResultDto<MovimentoItemDto>> ListarBaixaConsignadosPendente(ListarEstoquePreMovimentoInput input);
        Task<ListResultDto<MovimentoItemDto>> ListarMovimentosItemConsinagdoSelecionados(ListarEstoquePreMovimentoInput input);
        Task<MovimentoItemDto> ObterItem(long id);
        DefaultReturn<EstoqueMovimentoDto> CriarOuEditarBaixaConsignado(EstoqueMovimentoDto input);
        DefaultReturn<EstoqueMovimentoDto> CriarOuEditarBaixaItemConsignado(MovimentoItemDto input);
        Task<DefaultReturn<MovimentoItemDto>> CriarOuEditarBaixaItem(MovimentoItemDto input);
        Task<ListResultDto<MovimentoIndexDto>> ListarVales(ListarEstoquePreMovimentoInput input);
        Task<ListResultDto<MovimentoIndexDto>> ListarNota(ListarEstoquePreMovimentoInput input);
        Task<ListResultDto<MovimentoItemDto>> ListarItensConsignados(ListarEstoquePreMovimentoInput input);
        Task<ListResultDto<MovimentoItemDto>> ListarItensValesSelecionados(ListarEstoquePreMovimentoInput input);
        //DefaultReturn<EstoqueMovimentoDto> ConfirmarSolicitacao(EstoquePreMovimentoDto preMovimento);
    }
}
