using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public interface IConsultorTabelaCampoAppService : IApplicationService
    {
        Task CriarOuEditar(CriarOuEditarConsultorTabelaCampo input);

        Task Excluir(CriarOuEditarConsultorTabelaCampo input);

        Task<CriarOuEditarConsultorTabelaCampo> Obter(long id);

        Task<PagedResultDto<ConsultorTabelaCampoDto>> Listar(ListarConsultorTabelaCamposInput input);

        Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarTodos();

        Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarConsultorTabelaCampos(ListarConsultorTabelaCamposInput input);

        Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarPorConsultorTabela(long id);

        Task<PagedResultDto<ConsultorTabelaCampoDto>> ComboCampos(long id);

        Task RemoverRelacaoTabelaCampo(CriarOuEditarConsultorTabelaCampo input);
    }
}
