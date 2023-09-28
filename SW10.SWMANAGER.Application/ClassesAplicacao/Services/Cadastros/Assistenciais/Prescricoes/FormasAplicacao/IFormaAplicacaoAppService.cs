using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao
{
    public interface IFormaAplicacaoAppService : IApplicationService
    {
        Task<PagedResultDto<FormaAplicacaoDto>> Listar(ListarInput input);

        Task<ListResultDto<FormaAplicacaoDto>> ListarTodos();

        Task<ListResultDto<FormaAplicacaoDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput);

        Task<FormaAplicacaoDto> CriarOuEditar(FormaAplicacaoDto input);

        Task Excluir(FormaAplicacaoDto input);

        Task<FormaAplicacaoDto> Obter(long id);

        Task<IEnumerable<FormaAplicacaoDto>> ObterIds(List<long> ids);

        //Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
