using Abp.Application.Services.Dto;
//using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class TipoOperacaoAppService : SWMANAGERAppServiceBase, ITipoOperacaoAppService
    {

        //private readonly IRepository<TipoOperacao, long> _tipoOperacaoRepository;


        //public TipoOperacaoAppService(IRepository<TipoOperacao, long> tipoOperacaoRepository)
        //{
        //    _tipoOperacaoRepository = tipoOperacaoRepository;
        //}



        public async Task<PagedResultDto<TipoOperacaoDto>> Listar()
        {
            using (var tipoOperacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoOperacao, long>>())
            {
                var query = tipoOperacaoRepository.Object.GetAll().AsNoTracking();

                var contarTipoOperacao = await query.CountAsync();


                var tipoOperacaos = (await query.AsNoTracking().ToListAsync().ConfigureAwait(false)).Select(x => TipoOperacaoDto.Mapear(x)).ToList();

                return new PagedResultDto<TipoOperacaoDto>(contarTipoOperacao, tipoOperacaos);
            }

        }
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<TipoOperacaoDto> faturamentoItensDto = new List<TipoOperacaoDto>();
            try
            {
                using (var tipoOperacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoOperacao, long>>())
                {
                    if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                    {
                        throw new Exception("NotANumber");
                    }


                    var query = from p in tipoOperacaoRepository.Object.GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                )
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query
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
