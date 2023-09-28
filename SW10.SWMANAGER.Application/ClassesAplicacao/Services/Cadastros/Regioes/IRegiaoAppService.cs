using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes
{
    public interface IRegiaoAppService : IApplicationService
    {
        //ListResultDto<RegiaoDto> GetRegioes(GetRegioesInput input);
        Task<PagedResultDto<RegiaoDto>> Listar(ListarRegioesInput input);

        Task CriarOuEditar(CriarOuEditarRegiao input);

        Task Excluir(CriarOuEditarRegiao input);

        Task<CriarOuEditarRegiao> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarRegioesInput input);
    }
}
