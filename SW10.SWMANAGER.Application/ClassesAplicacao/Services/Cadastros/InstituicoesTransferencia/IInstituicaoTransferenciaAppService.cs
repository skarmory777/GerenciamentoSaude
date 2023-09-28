using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia
{
    public interface IInstituicaoTransferenciaAppService : IApplicationService
    {
        Task<PagedResultDto<InstituicaoTransferenciaDto>> Listar(ListarInstituicoesTransferenciaInput input);

        Task CriarOuEditar(CriarOuEditarInstituicaoTransferencia input);

        Task Excluir(CriarOuEditarInstituicaoTransferencia input);

        Task<CriarOuEditarInstituicaoTransferencia> Obter(long id);

        //       Task<FileDto> ListarParaExcel(ListarInstituicoesTransferenciaInput input);
    }
}
