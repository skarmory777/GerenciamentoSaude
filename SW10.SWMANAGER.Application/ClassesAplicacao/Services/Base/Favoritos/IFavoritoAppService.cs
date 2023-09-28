using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Favoritos
{
    public interface IFavoritoAppService : IApplicationService
    {
        Task<ListResultDto<FavoritoDto>> Listar(long userId);

        Task CriarOuEditar(FavoritoDto input);

        Task Excluir(FavoritoDto input);

        Task<FavoritoDto> Obter(long id);

    }
}
