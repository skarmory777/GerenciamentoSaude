using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao
{
    public interface IUnidadeInternacaoAppService : IApplicationService
    {
        Task<PagedResultDto<UnidadeInternacaoDto>> Listar(ListarUnidadesInternacaoInput input);

        Task CriarOuEditar(CriarOuEditarUnidadeInternacao input);

        Task Excluir(CriarOuEditarUnidadeInternacao input);

        Task<CriarOuEditarUnidadeInternacao> Obter(long id);

        //	Task<FileDto> ListarParaExcel (ListarUnidadesInternacaoInput input);
    }
}
