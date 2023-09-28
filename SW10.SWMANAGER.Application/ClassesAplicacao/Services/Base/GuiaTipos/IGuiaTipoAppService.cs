using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.GuiaTipos
{
    public interface IGuiaTipoAppService : IApplicationService
    {
        Task<ListResultDto<GuiaTipoDto>> ListarTodos();

        Task CriarOuEditar(GuiaTipoDto input);

        Task Excluir(GuiaTipoDto input);

        Task<GuiaTipoDto> Obter(long id);

    }
}
