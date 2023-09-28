using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio
{
    public interface ITabelaDominioAppService : IApplicationService
    {
        //ListResultDto<TabelaDominioDto> GetTabelasDominio(GetTabelasDominioInput input);
        Task<PagedResultDto<TabelaDominioDto>> Listar(ListarTabelasDominioInput input);

        Task CriarOuEditar(TabelaDominioDto input);

        Task Excluir(TabelaDominioDto input);

        Task<TabelaDominioDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTabelasDominioInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarPorTipoAtendimentoDropdown(DropdownInput dropdownInput);

    }
}
