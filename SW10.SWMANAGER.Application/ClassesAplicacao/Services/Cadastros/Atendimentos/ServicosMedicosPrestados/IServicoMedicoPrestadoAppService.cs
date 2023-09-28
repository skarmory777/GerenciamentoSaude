using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados
{
    public interface IServicoMedicoPrestadoAppService : IApplicationService
    {
        Task<PagedResultDto<ServicoMedicoPrestadoDto>> Listar(ListarServicosMedicosPrestadosInput input);

        Task<ListResultDto<ServicoMedicoPrestadoDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarServicoMedicoPrestado input);

        Task Excluir(CriarOuEditarServicoMedicoPrestado input);

        Task<CriarOuEditarServicoMedicoPrestado> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
