using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades
{
    public interface ICidadeAppService : IApplicationService
    {
        Task<PagedResultDto<CidadeDto>> Listar(ListarCidadesInput input);

        // Task<ListResultDto<GenericoIdNome>> ListarAutoComplete (string input,long? estadoId);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? estadoId);

        Task CriarOuEditar(CidadeDto input);

        Task Excluir(CidadeDto input);

        Task<CidadeDto> Obter(long id);

        Task<CidadeDto> ObterComEstado(string nome, long estadoId);

        Task<FileDto> ListarParaExcel(ListarCidadesInput input);

        Task<ListResultDto<CidadeDto>> ListarPorEstado(long? id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
