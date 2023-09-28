using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps
{
    public interface ICepAppService : IApplicationService
    {
        Task<PagedResultDto<CepDto>> Listar(ListarCepsInput input);

        Task<Cep> CriarOuEditar(CriarOuEditarCep input);

        Task Excluir(CriarOuEditarCep input);

        Task<CriarOuEditarCep> Obter(long id);

        Task<CepDto> Obter(string cep);

        Task<FileDto> ListarParaExcel(ListarCepsInput input);

        Task<CepCorreios> ConsultaCep(string cep);
        //Task<ListResultDto<CepDto>> ListarAutoComplete(string input, long? estadoId);
    }
}
