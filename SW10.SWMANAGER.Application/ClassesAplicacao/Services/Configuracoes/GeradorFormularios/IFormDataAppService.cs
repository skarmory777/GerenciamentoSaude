using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public interface IFormDataAppService : IApplicationService
    {
        Task<PagedResultDto<FormData>> Listar(ListarInput input);

        Task<ListResultDto<FormData>> ListarTodos();

        Task CriarOuEditar(FormData input);

        Task Excluir(long id);

        void Excluir(IEnumerable<long> ids);

        Task<FormData> Obter(long id);

        Task<List<FormData>> ListarNoLazy(long id);

    }
}
