using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades
{
    public interface ILaboratorioUnidadeAppService : IApplicationService
    {
        Task<PagedResultDto<LaboratorioUnidadeDto>> Listar(ListarInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(LaboratorioUnidadeDto input);

        Task Excluir(LaboratorioUnidadeDto input);

        Task<LaboratorioUnidadeDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
