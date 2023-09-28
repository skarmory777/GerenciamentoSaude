using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiros.TipoDocumentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface
{
    public interface ITipoDocumentoAppService : IApplicationService
    {
        Task<TipoDocumentoDto> ObterPelaDescricao(string descricao);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<PagedResultDto<TipoDocumentoDto>> Listar(TipoDocumentoInput input);
        Task<TipoDocumentoDto> Obter(long id);
        Task<DefaultReturn<TipoDocumentoDto>> CriarOuEditar(TipoDocumentoDto input);
        Task<DefaultReturn<TipoDocumentoDto>> Excluir(TipoDocumentoDto input);        
    }
}
