using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public interface IFormRespostaAppService : IApplicationService
    {
        Task<PagedResultDto<FormResposta>> Listar(ListarFormRespostaInput input);

        Task<ListResultDto<FormResposta>> ListarTodos();

        //Task CriarOuEditar(FormRespostaDto input);
        Task<long> CriarOuEditar(FormConfigDto formConfig, long idDadosResposta, string nomeClasse, string registroClasseId);

        Task Excluir(FormResposta input);

        Task<FormRespostaDto> Obter(long id);

        Task<FormResposta> ObterNoLazy(long id);

        Task<FormRespostaDto> ObterUltimoLancamentoPorFormConfig(long formConfigId, long formRespostaId);

        Task<List<FormRespostaDto>> ObterUltimoLancamentosPorFormConfig(long formConfigId, long formRespostaId,
            long atendimentoId);
    }
}
