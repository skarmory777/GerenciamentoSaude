using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias
{
    public interface IGuiaAppService : IApplicationService
    {
        Task<PagedResultDto<GuiaDto>> Listar(ListarGuiasInput input);

        Task<PagedResultDto<GuiaDto>> ListarTodos();

        Task<CriarOuEditarGuia> CriarOuEditar(CriarOuEditarGuia input);

        Task<CriarOuEditarGuia> AtualizarCoordenadas(CriarOuEditarGuia guia, string camposAlterados);

        Task Excluir(CriarOuEditarGuia input);

        Task<CriarOuEditarGuia> Obter(long id);

        Task<bool> SalvarCampos(string campos, long guiaId);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
