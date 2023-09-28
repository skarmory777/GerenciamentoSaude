using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class OrdemCompraAppService : SWMANAGERAppServiceBase, IOrdemCompraAppService
    {
        public async Task<ListResultDto<OrdemCompraDto>> ListarTodos()
        {
            var contarOrdemCompra = 0;
            List<OrdemCompraDto> OrdemCompraDtos = new List<OrdemCompraDto>();
            try
            {
                using (var ordemCompraRepository = IocManager.Instance.ResolveAsDisposable<IRepository<OrdemCompra, long>>())
                {
                    var query = ordemCompraRepository.Object
                        .GetAll().AsNoTracking()
                        .Select(s => new OrdemCompraDto { Id = s.Id, Codigo = s.Codigo, Descricao = s.Descricao });

                    contarOrdemCompra = await query
                        .CountAsync();

                    OrdemCompraDtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    //OrdemCompraDtos = OrdemCompraDto.Mapear( OrdemCompras
                    //    .MapTo<List<OrdemCompraDto>>();

                    return new ListResultDto<OrdemCompraDto> { Items = OrdemCompraDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<OrdemCompraDto> tipoMovimentoDto = new List<OrdemCompraDto>();
            try
            {
                using (var ordemCompraRepository = IocManager.Instance.ResolveAsDisposable<IRepository<OrdemCompra, long>>())
                {
                    if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                    {
                        throw new Exception("NotANumber");
                    }

                    var query = from p in ordemCompraRepository.Object.GetAll()
                                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query.AsNoTracking()
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    var result = queryResultPage.ToList();

                    //  int total = await query.CountAsync();

                    int total = result.Count();

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
