using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio
{
    public interface ITipoTabelaDominioAppService : IApplicationService
    {
        //ListResultDto<TipoTabelaDominioDto> GetTiposTabelaDominio(GetTiposTabelaDominioInput input);
        Task<PagedResultDto<TipoTabelaDominioDto>> Listar(ListarTiposTabelaDominioInput input);

        Task<ListResultDto<TipoTabelaDominio>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarTipoTabelaDominio input);

        Task Excluir(CriarOuEditarTipoTabelaDominio input);

        Task<CriarOuEditarTipoTabelaDominio> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposTabelaDominioInput input);
    }
}
