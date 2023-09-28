using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados
{
    public interface IAtestadoAppService : IApplicationService
    {
        Task<PagedResultDto<AtestadoDto>> Listar(ListarAtestadosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(AtestadoDto input);

        Task Excluir(AtestadoDto input);

        Task<AtestadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarAtestadosInput input);
    }
}
