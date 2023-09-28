using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais
{
    public interface IMaterialAppService : IApplicationService
    {
        Task<PagedResultDto<MaterialDto>> Listar(ListarMaterialsInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(MaterialDto input);

        Task Excluir(MaterialDto input);

        Task<MaterialDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMaterialsInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
