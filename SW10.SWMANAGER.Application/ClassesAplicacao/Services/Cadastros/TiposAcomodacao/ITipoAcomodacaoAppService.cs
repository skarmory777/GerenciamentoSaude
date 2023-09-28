using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao
{
    public interface ITipoAcomodacaoAppService : IApplicationService
    {
        //ListResultDto<TipoAcomodacaoDto> GetTiposAcomodacao(GetTiposAcomodacaoInput input);
        Task<PagedResultDto<TipoAcomodacaoDto>> Listar(ListarTiposAcomodacaoInput input);

        Task CriarOuEditar(TipoAcomodacaoDto input);

        Task Excluir(TipoAcomodacaoDto input);

        Task<TipoAcomodacaoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposAcomodacaoInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<PagedResultDto<TipoAcomodacaoDto>> ListarComLeito(ListarTiposAcomodacaoInput input);

    }
}