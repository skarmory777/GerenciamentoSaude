using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas
{
    public interface IParametroAppService : IApplicationService
    {
        ParametroDto ObterPorCodigoEmpresa(long empresaId, string codigo);
        ParametroDto ObterPorCodigo(string codigo);
        Task CriarOuEditar(ParametroDto input);
    }
}
