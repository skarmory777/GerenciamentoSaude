using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias
{
    public interface IRelacaoGuiaCampoAppService : IApplicationService
    {
        Task<CriarOuEditarRelacaoGuiaCampo> CriarOuEditar(CriarOuEditarRelacaoGuiaCampo input);

        Task Excluir(CriarOuEditarRelacaoGuiaCampo input);

        Task<CriarOuEditarRelacaoGuiaCampo> Obter(long id);

        //     RelacaoGuiaCampoDto ObterDto(long id);

        //     GuiaCampoDto ObterGuiaCampo(long id);

        //     PagedResultDto<RelacaoGuiaCampoDto> ListarParaGuia(long guiaId);

        //PagedResultDto<RelacaoGuiaCampoDto> ListarParaConjunto(long conjuntoId, long guiaId);
        PagedResultDto<GuiaCampoDto> ListarParaConjunto(long conjuntoId, long guiaId);
    }
}
