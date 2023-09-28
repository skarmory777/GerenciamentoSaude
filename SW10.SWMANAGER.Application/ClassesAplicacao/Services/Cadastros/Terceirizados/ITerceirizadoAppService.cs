using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados
{
    public interface ITerceirizadoAppService : IApplicationService
    {
        //ListResultDto<TipoAcomodacaoDto> GetTiposAcomodacao(GetTiposAcomodacaoInput input);
        Task<PagedResultDto<TerceirizadoDto>> Listar(ListarInput input);

        Task CriarOuEditar(TerceirizadoDto input);

        Task Excluir(TerceirizadoDto input);

        Task<TerceirizadoDto> Obter(long id);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownNomeCompleto(DropdownInput dropdownInput);

    }
}
