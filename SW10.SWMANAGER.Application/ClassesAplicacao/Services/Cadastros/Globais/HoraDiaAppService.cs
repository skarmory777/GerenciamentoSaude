using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Globais.HorasDias
{
    public class HoraDiaAppService : SWMANAGERAppServiceBase, IHoraDiaAppService
    {
        private readonly IRepository<HoraDia, long> _horaDiaRepository;

        public HoraDiaAppService(IRepository<HoraDia, long> horaDiaRepository)
        {
            _horaDiaRepository = horaDiaRepository;
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                var query = from p in _horaDiaRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                          || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                                      )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao, ":00") };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
