using Abp.Application.Services;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.VisualASA
{
    public interface IVisualAppService : IApplicationService
    {
        void MigrarVisualASA(long atendimentosId);
        void MigrarSisPessoa(long pessoaId);
        Task MigrarProRegExame(long exameId);
    }
}
