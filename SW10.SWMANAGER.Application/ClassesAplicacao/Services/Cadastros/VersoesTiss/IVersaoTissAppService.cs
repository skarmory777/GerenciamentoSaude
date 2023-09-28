using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss
{
    public interface IVersaoTissAppService : IApplicationService
    {
        //ListResultDto<VersaoTissDto> GetVersoesTiss(GetVersoesTissInput input);
        Task<PagedResultDto<VersaoTissDto>> Listar(ListarVersoesTissInput input);

        Task<ICollection<VersaoTissDto>> ListarPorTabelaDominio(long id);

        Task<ListResultDto<VersaoTissDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarVersaoTiss input);

        Task Excluir(CriarOuEditarVersaoTiss input);

        Task<CriarOuEditarVersaoTiss> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarVersoesTissInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
