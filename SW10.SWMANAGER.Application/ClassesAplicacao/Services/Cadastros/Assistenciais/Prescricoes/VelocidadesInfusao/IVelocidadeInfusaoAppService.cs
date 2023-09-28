using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao
{
    public interface IVelocidadeInfusaoAppService : IApplicationService
    {
        Task<PagedResultDto<VelocidadeInfusaoDto>> Listar(ListarInput input);

        Task<ListResultDto<VelocidadeInfusaoDto>> ListarTodos();

        Task<ListResultDto<VelocidadeInfusaoDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput);

        Task<VelocidadeInfusaoDto> CriarOuEditar(VelocidadeInfusaoDto input);

        Task Excluir(VelocidadeInfusaoDto input);

        Task<VelocidadeInfusaoDto> Obter(long id);
    }
}
