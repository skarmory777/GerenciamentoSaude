using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IMotivoPerdaProdutoAppService : IApplicationService
    {
        Task<ListResultDto<MotivoPerdaProdutoDto>> ListarTodos();

        Task<IResultDropdownList<long>> ListarDropDown(DropdownInput input);

    }
}
