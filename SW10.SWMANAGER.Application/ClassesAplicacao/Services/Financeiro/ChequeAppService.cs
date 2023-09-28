using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class ChequeAppService : SWMANAGERAppServiceBase, IChequeAppService
    {
        private readonly IRepository<Cheque, long> _chequeRepository;

        public ChequeAppService(IRepository<Cheque, long> chequeRepository)
        {
            _chequeRepository = chequeRepository;
        }

        public async Task<IResultDropdownList<long>> ListarChequeNaoUtilziadoPorContaCorrenteDropdown(DropdownInput dropdownInput)
        {
            long contaCorrenteId;

            long.TryParse(dropdownInput.filtro, out contaCorrenteId);

            return await ListarDropdownLambda(dropdownInput
                                                     , _chequeRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                    && m.Data == null



                                                    , p => new DropdownItems { id = p.Id, text = p.Numero.ToString() }
                                                    , o => o.Descricao
                                                    );


        }



    }
}
