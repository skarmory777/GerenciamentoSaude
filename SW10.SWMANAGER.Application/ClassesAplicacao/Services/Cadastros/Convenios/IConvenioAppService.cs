using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios
{
    public interface IConvenioAppService : IApplicationService
    {
        Task<PagedResultDto<ConvenioDto>> Listar(ListarConveniosInput input);

        Task<PagedResultDto<ListarConveniosTabelaPrecoDto>> ListarConveniosTabelaPreco(ListarConveniosInput input);

        Task<PagedResultDto<ConvenioURLServicoDto>> ListarUrlServicos(ListarInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<ListResultDto<ConvenioDto>> ListarTodos();

        Task<DefaultReturn<ConvenioDto>> CriarOuEditar(ConvenioDto input);

        Task Excluir(ConvenioDto input);

        Task<ConvenioDto> Obter(long id);

        Task<ConvenioDto> ObterDto(long id);

        Task<FileDto> ListarParaExcel(ListarConveniosInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<ConvenioDto> ObterCNPJ(string cnpj);
        Task ExcluirPorId(long id);
        Task<ConvenioPadroesAtendimentoDto> ObterPadroes(long id);
        Task<NumeroGuiaDto> ObterNumeroGuia(long convenioId, long empresaId, long guiaId);
    }
}
