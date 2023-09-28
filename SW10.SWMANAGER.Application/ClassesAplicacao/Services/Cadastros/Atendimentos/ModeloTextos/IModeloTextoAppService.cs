using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos
{
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;

    public interface IModeloTextoAppService : IApplicationService
    {
        Task<TextoModeloDto> Obter(long id);
        Task<PagedResultDto<TextoModeloDto>> Listar(ListarModeloTextoInput input);
        Task<DefaultReturn<TextoModeloDto>> CriarOuEditar(TextoModeloDto input);
        Task<bool> Excluir(long id);
        Task<bool> IsGuide(long EmpresaId, long GuideId);

        Task<TextoModelo> ObterPorTipoAsync(long tipoModeloId);
    }
}
