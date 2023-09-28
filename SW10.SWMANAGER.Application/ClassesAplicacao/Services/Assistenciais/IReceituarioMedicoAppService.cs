namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
    using System.Threading.Tasks;

    public interface IReceituarioMedicoAppService : IApplicationService
    {
        Task<ListResultDto<ReceituarioMedicoDto>> ListarTodos(ListarReceituarioMedicoInput input);
        Task<ReceituarioMedicoDto> GerarNovoReceituarioMedico(long atendimentoId);
        Task SalvarDadosDaReceitaMemed(RetornoReceitaMemedInput input);
        Task<string> ObterLinkMemedPDFPrescricao(long receituarioId, long atendimentoId);
        Task<ReceituarioMedicoRecuperaLinkPrescricaoMemedDto.Attributes> ObterLinkMemedReceitaDigitalPacientePrescricao(long receituarioId, long atendimentoId);
    }
}
