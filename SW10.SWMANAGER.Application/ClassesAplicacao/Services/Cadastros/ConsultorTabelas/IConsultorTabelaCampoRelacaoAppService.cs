using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public interface IConsultorTabelaCampoRelacaoAppService : IApplicationService
    {
        Task CriarOuEditar(ConsultorTabelaCampoRelacaoDto input);

        Task Excluir(ConsultorTabelaCampoRelacaoDto input);

        Task<ConsultorTabelaCampoRelacaoDto> Obter(long id);

        Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarCombo(long id);

        Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarTabela(long id);
    }
}
