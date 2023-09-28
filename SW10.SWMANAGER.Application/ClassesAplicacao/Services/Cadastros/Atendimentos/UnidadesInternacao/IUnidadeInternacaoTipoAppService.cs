using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao
{
    public interface IUnidadeInternacaoTipoAppService : IApplicationService
    {
        Task<PagedResultDto<UnidadeInternacaoTipoDto>> Listar(ListarUnidadesInternacaoInput input);

        Task CriarOuEditar(CriarOuEditarUnidadeInternacaoTipo input);

        Task Excluir(CriarOuEditarUnidadeInternacaoTipo input);

        Task<CriarOuEditarUnidadeInternacaoTipo> Obter(long id);
    }
}
