using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes
{
    public interface IDivisaoAppService : IApplicationService
    {
        Task<PagedResultDto<DivisaoDto>> Listar(ListarDivisoesInput input);

        Task<PagedResultDto<DivisaoDto>> ListarSubDivisoes(ListarDivisoesInput input);

        Task<PagedResultDto<DivisaoDto>> ListarDivisoesSemRelacao(ListarDivisoesInput input);

        //Task<PagedResultDto<DivisaoDto>> ListarTiposPrescricaoSemRelacao(ListarDivisoesInput input);

        Task<ListResultDto<DivisaoDto>> ListarTodos();

        Task<ListResultDto<DivisaoDto>> ListarFiltro(string filtro);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<DivisaoDto> CriarOuEditar(DivisaoDto input);

        Task<DivisaoDto> SalvarSubDivisao(DivisaoDto input);

        Task Excluir(DivisaoDto input);


        Task<IEnumerable<DivisaoDto>> ObterIds(List<long> ids);
        Task<DivisaoDto> Obter(long id);

        //Task<IEnumerable<DivisaoDto>> ObterIds(List<long> id);

        Task<FileDto> ListarParaExcel(ListarDivisoesInput input);
    }
}
