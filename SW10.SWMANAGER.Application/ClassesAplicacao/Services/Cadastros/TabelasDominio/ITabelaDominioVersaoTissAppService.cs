using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio
{
    public interface ITabelaDominioVersaoTissAppService : IApplicationService
    {
        Task CriarOuEditar(TabelaDominioVersaoTissDto input);

        Task Excluir(TabelaDominioVersaoTissDto input);

        Task<TabelaDominioVersaoTissDto> Obter(long id);

    }
}
