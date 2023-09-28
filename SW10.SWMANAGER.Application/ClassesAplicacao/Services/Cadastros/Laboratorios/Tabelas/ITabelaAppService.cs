using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas
{
    public interface ITabelaAppService : IApplicationService
    {
        Task<PagedResultDto<TabelaDto>> Listar(ListarTabelasInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(TabelaDto input);

        Task Excluir(TabelaDto input);

        Task<TabelaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTabelasInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
