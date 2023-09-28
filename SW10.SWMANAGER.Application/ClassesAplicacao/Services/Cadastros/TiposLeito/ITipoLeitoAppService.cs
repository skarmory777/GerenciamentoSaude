using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito
{
    public interface ITipoLeitoAppService : IApplicationService
    {
        //ListResultDto<TipoLeitoDto> GetTiposLeito(GetTiposLeitoInput input);
        Task<PagedResultDto<TipoLeitoDto>> Listar(ListarTiposLeitoInput input);

        Task<ListResultDto<TipoLeitoDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarTipoLeito input);

        Task Excluir(CriarOuEditarTipoLeito input);

        Task<CriarOuEditarTipoLeito> Obter(long id);

        //   Task<FileDto> ListarParaExcel(ListarTiposLeitoInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
