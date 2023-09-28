using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown
{
    public class DropdownAppService : SWMANAGERAppServiceBase
    {

        public async Task<ResultDropdownList> ListarDropdown<T>(DropdownInput dropdownInput, IRepository<T, long> repository) where T : CamposPadraoCRUD
        {

            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                var query = from p in repository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()))
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = p.Descricao };

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


        public async Task<ResultDropdownList> ListarCodigoDescricaoDropdown<T>(DropdownInput dropdownInput, IRepository<T, long> repository) where T : CamposPadraoCRUD, ICodigoDescricao
        {

            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                var query = from p in repository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                          || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                                      )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) };

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


        //public async Task<ResultDropdownList> ListarDropdownLambda<T>(DropdownInput dropdownInput, IRepository<T, long> repository
        //                        , Expression<Func<T, bool>> myWhere
        //                        , Expression<Func<T, DropdownItems>> mySelect)
        //                        where T : CamposPadraoCRUD
        //{

        //    int pageInt = int.Parse(dropdownInput.page) - 1;
        //    var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
        //    try
        //    {
        //        var query = repository.GetAll()
        //             .WhereIf(!dropdownInput.search.IsNullOrEmpty(), myWhere)
        //             .Select(mySelect);

        //        var queryResultPage = query
        //          .Skip(numberOfObjectsPerPage * pageInt)
        //          .Take(numberOfObjectsPerPage);

        //        int total = await query.CountAsync();
        //        var list = await query.ToListAsync();

        //        return new ResultDropdownList() { Items = list, TotalCount = total };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex.InnerException);
        //    }
        //}


    }
}
