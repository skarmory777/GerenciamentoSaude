using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos
{
    public interface IModeloLaudoAppService : IApplicationService
    {
        Task<PagedResultDto<ModeloLaudoDto>> Listar(ListarInput input);

        //	Task<ListResultDto<GenericoIdNome>> ListarAutoComplete (string input);

        Task<ListResultDto<ModeloLaudoDto>> ListarTodos();

        Task CriarOuEditar(ModeloLaudoDto input);

        Task Excluir(ModeloLaudoDto input);

        Task<ModeloLaudoDto> Obter(long id);

        //   Task<FileDto> ListarParaExcel(ListarInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarDropdownPorExame(DropdownInput dropdownInput);
    }
}
