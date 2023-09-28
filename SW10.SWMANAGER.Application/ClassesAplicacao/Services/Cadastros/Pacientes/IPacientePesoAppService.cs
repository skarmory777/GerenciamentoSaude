using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
    using Abp.Application.Services.Dto;

    public interface IPacientePesoAppService : IApplicationService
    {
        Task CriarOuEditar(PacientePesoDto input);

        Task Excluir(PacientePesoDto input);

        Task<PacientePesoDto> Obter(long id);

        Task<PagedResultDto<PacientePesoDto>> ListarIndexAsync(long id);
    }
}
