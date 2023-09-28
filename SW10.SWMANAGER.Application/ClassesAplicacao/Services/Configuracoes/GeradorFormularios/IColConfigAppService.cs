using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

    public interface IColConfigAppService : IApplicationService
    {
        Task<PagedResultDto<ColConfigDto>> Listar(ListarInput input);

        Task<ListResultDto<ColConfigDto>> ListarTodos();
        
        
        Task<ListResultDto<ColConfigDto>> ListarTodos(IEnumerable<long> colunaId);

        Task CriarOuEditar(ColConfig input);

        Task Excluir(ColConfig input);

        Task<ColConfigDto> Obter(long id);

    }
}
