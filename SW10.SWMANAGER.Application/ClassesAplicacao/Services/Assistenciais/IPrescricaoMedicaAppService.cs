using System;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.PrescricaoMedicaAppService;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface IPrescricaoMedicaAppService : IApplicationService
    {
        Task<PagedResultDto<PrescricaoMedicaIndexDto>> Listar(PrescricaoMedicaListarInput input);

        Task<ListResultDto<PrescricaoMedicaDto>> ListarFiltro(string filtro);

        Task<PagedResultDto<PrescricaoItemRespostaDto>> ListarRespostas(ListarPrescricaoMedicaInput input);

        Task<ListResultDto<PrescricaoItemRespostaDto>> ListarRespostasPorPrescricao(long id);

        Task<IEnumerable<PrescricaoItemRespostaViewModel>> ListarRespostasPorPrescricaoCompleta(long id);

        Task<PagedResultDto<PrescricaoItemRespostaDto>> ListarRespostasJson(List<PrescricaoItemRespostaDto> list, long divisaoId);

        Task<List<PrescricaoItemRespostaDto>> ListarPrescricaoCompleta(List<PrescricaoItemRespostaDto> list);

        Task<PagedResultDto<PrescricaoMedicaDto>> ListarPorPaciente(ListarInput input);

        Task<PrescricaoMedicaDto> CriarOuEditar(PrescricaoMedicaDto input,  bool atualizaOuCriaArquivo = true);

        Task Excluir(long id);

        Task ExcluirItemResposta(long prescricaoItemRespostaId);

        Task<PrescricaoMedicaDto> Obter(long id);

        Task<ListResultDto<PrescricaoMedicaDto>> ListarTodos();

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<RetornoCopiarPrescricaoMedica> ListarRespostasParaCopiar(long id);

        Task<DefaultReturn<PrescricaoMedicaDto>> Copiar(long id, long atendimentoId,  DateTime? dataAgrupamento = null, long modeloPrescricaoId = 0, bool dataFutura = false);

        byte[] RetornaArquivoPrescricaoMedica(long prescricaoId, bool imprimirResumido = false,
            DateTime? dataAgrupamento = null, int tentantId = 0);

        Task Suspender(long id, long atendimentoId);

        Task SuspenderItemResposta(long prescricaoItemRespostaId, DateTime dataAgrupamento);

        Task<PrescricaoMedicaDto> Liberar(long id, long atendimentoId);

        Task LiberarItemResposta(long prescricaoItemRespostaId);

        Task AprovarItemResposta(long prescricaoItemRespostaId);

        Task Aprovar(long id, long atendimentoId);

        Task ReAtivar(long id);

        Task ReAtivarItemResposta(long prescricaoItemRespostaId);

        Task<DefaultReturn<RetornoPrescricao>> IncluirItemPrescricaoModelo(IncluirItemPrescricaoModeloDto input);

        Task<RetornoCheckarMedico> ChecarMedicoPrescricao(long prescricaoId);

        Task<bool> AlterarMedicoPrescricao(long prescricaoId);

        bool ValidarPrescricaoFutura(long atendimentoId);

        Task AtualizaArquivoPrescricaoMedica(long prescricaoId);

        Task<RetornoValidacaoDuplicidadeNaPrescricao> ValidaDuplicidadeItemNaPrescricao(long prescricaoMedicaId, long prescricaoItemId);
    }
}
