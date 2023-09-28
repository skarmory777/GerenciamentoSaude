using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NFe.Classes;

using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais
{
    public interface INotaFiscalAppService : IApplicationService //, IDisposable
    {
        //Task<PagedResultDto<NotaFiscalDto>> Listar(ListarNotasFiscaisInput input);

        Task<PagedResultDto<VMNotaFiscal>> ListarIndex(ListarNotasFiscaisInput input);

        Task<DefaultReturn<nfeProc>> ObterNFeReceita(string chNFe, long empresaId);

        //Task CriarOuEditar(NotaFiscal input);

        //Task Excluir(CriarOuEditarNotaFiscal input);

        //Task<NotaFiscal> Obter(long id);

        //Task<NotaFiscal> Obter(string chave);

        //NotaFiscalDetalhe ObterDetalhe(long id);

        //NotaFiscalTotal ObterTotal(long id);

        //Task<NotaFiscalDto> ObterIncluding(long id);

        //Task<FileDto> ListarParaExcel(ListarNotasFiscaisInput input);

        //Task<long> MaxNsu(long empresaId);
        ////Task<string> Sincronizar(string[] lines);

        //Task ManifestacaoDestinatario (NotaFiscalManifestacaoDestinatarioDto input);
    }
}
