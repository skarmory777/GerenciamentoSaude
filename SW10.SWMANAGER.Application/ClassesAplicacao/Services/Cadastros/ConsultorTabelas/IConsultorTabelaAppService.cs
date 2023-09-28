using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public interface IConsultorTabelaAppService : IApplicationService
    {
        Task<PagedResultDto<ConsultorTabelaDto>> Listar(ListarConsultorTabelasInput input);

        Task<PagedResultDto<ConsultorTabelaDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarConsultorTabela input);

        Task Excluir(CriarOuEditarConsultorTabela input);

        Task<CriarOuEditarConsultorTabela> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarConsultorTabelasInput input);
    }
}
