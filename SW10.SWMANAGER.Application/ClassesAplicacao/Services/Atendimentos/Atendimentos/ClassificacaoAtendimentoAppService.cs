using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    public class ClassificacaoAtendimentoAppService : SWMANAGERAppServiceBase, IClassificacaoAtendimentoAppService
    {
        public readonly IRepository<ClassificacaoAtendimento, long> _classificacaoAtendimentoRepository;

        public ClassificacaoAtendimentoAppService(IRepository<ClassificacaoAtendimento, long> classificacaoAtendimentoRepository)
        {
            _classificacaoAtendimentoRepository = classificacaoAtendimentoRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            // return await base.ListarCodigoDescricaoDropdown(dropdownInput, _classificacaoAtendimentoRepository);

            return await this.ListarDropdownLambda(
                       dropdownInput,
                       this._classificacaoAtendimentoRepository,
                       m => (string.IsNullOrEmpty(dropdownInput.search)
                             || m.Descricao.Contains(dropdownInput.search)
                             || m.Codigo.ToString().Contains(dropdownInput.search)) && m.Ativo,
                       p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) },
                       o => o.Prioridade.ToString()).ConfigureAwait(false);
        }
    }
}
