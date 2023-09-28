using Abp.Application.Services;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos
{
    public interface IModeloTextoGuiaAppService : IApplicationService
    {
        TextoModeloDto Obter(long? guiaId, long? emprasId);
    }
}
