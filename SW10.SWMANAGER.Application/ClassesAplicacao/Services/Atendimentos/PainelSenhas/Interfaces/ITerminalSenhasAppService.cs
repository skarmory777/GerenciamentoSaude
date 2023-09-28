namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces
{
    using Abp.Application.Services;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITerminalSenhasAppService : IApplicationService
    {
        Task<SenhaIndex> GerarSenha(long filaId);

        Task<SenhaIndex> GerarSenhaEImprimir(long filaId, string printerName);

        List<FilaTerminalIndex> ListarFilasDisponiveis();

        List<FilaTerminalIndex> ListarFilasDisponiveis(long? tipoLocalChamadaId);
        Task ChamarSenha(long tipoLocalChamada, long localChamadaId, long senhaMovimentacaoId);
    }
}
