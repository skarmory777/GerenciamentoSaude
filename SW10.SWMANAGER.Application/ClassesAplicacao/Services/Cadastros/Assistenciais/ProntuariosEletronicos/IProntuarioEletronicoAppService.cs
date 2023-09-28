using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos
{
    public interface IProntuarioEletronicoAppService : IApplicationService
    {
        Task<PagedResultDto<ProntuarioEletronicoIndexDto>> Listar(ListarInput input);

        Task<ListResultDto<ProntuarioEletronicoDto>> ListarTodos();

        Task<ProntuarioEletronicoDto> CriarOuEditar(ProntuarioEletronicoDto input);

        Task Excluir(long id, string justificativa);

        Task Reativar(long id, string justificativa);

        Task<PagedResultDto<ProntuarioEletronicoIndexDto>> ListarInativos(ListarInput input);

        Task<ProntuarioEletronicoDto> Obter(long id);

        //Task<FileDto> ListarParaExcel(ListarInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task AtualizarFormId(long id, long respostaId);


        Task AlterarLeito(long id, long? leitoId);

        Task<ProntuarioEletronicoDto> ObterUltimoProntuarioPorAtendimentoEFormulario(
            long atendimentoId,
            long formConfigId,
            long formRespostaId);

    }
}
