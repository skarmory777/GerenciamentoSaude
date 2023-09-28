using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaConvenioCodigo;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TabelaConvenioCodigos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TabelaConvenioCodigos
{
    public class TabelaConvenioCodigoAppService : SWMANAGERAppServiceBase, ITabelaConvenioCodigoAppService
    {
        private readonly IRepository<TabelaConvenioCodigo, long> _tabelaConvenioCodigoRepository;

        public TabelaConvenioCodigoAppService(
            IRepository<TabelaConvenioCodigo, long> tabelaConvenioCodigoRepository
            )
        {
            _tabelaConvenioCodigoRepository = tabelaConvenioCodigoRepository;
        }

        public async Task CriarOuEditar(TabelaConvenioCodigoDto input)
        {
            try
            {
                var tabelaConvenioCodigo = TabelaConvenioCodigoDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    await _tabelaConvenioCodigoRepository.InsertAsync(tabelaConvenioCodigo);
                }
                else
                {
                    var ori = _tabelaConvenioCodigoRepository.GetAll()
                                                    .Where(w => w.Id == input.Id)
                                                    .FirstOrDefault();

                    if (ori != null)
                    {
                        ori.Codigo = input.Codigo;
                        ori.ConvenioId = input.ConvenioId;
                        ori.CreationTime = input.CreationTime;
                        ori.CreatorUserId = input.CreatorUserId;
                        ori.Id = input.Id;
                        ori.LastModificationTime = input.LastModificationTime;
                        ori.LastModifierUserId = input.LastModifierUserId;
                        ori.Descricao = input.Descricao;
                        ori.IsFromTuss = input.IsFromTuss;
                        ori.TabelaPrecoItemId = input.TabelaPrecoItemId;



                        await _tabelaConvenioCodigoRepository.UpdateAsync(ori);
                    }


                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TabelaConvenioCodigoDto input)
        {
            try
            {
                await _tabelaConvenioCodigoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TabelaConvenioCodigoDto>> Listar(ListarInput input)
        {
            var contarTabelaConvenioCodigos = 0;
            List<TabelaConvenioCodigo> tabelaConvenioCodigos;
            List<TabelaConvenioCodigoDto> tabelaConvenioCodigosDtos = new List<TabelaConvenioCodigoDto>();
            try
            {
                var query = _tabelaConvenioCodigoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Convenio.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Convenio.RazaoSocial.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Convenio.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.TabelaPrecoItem.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.TabelaPrecoItem.Codigo.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTabelaConvenioCodigos = await query
                    .CountAsync();

                tabelaConvenioCodigos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tabelaConvenioCodigosDtos = TabelaConvenioCodigoDto.Mapear(tabelaConvenioCodigos).ToList();

                return new PagedResultDto<TabelaConvenioCodigoDto>(
                    contarTabelaConvenioCodigos,
                    tabelaConvenioCodigosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<TabelaConvenioCodigoDto> Obter(long id)
        {
            try
            {
                var query = await _tabelaConvenioCodigoRepository
                    .GetAll()
                    .Include(i => i.Convenio)
                    .Include(i => i.TabelaPrecoItem)
                    .Where(w => w.Id == id)
                    .FirstOrDefaultAsync();

                var tabelaConvenioCodigo = TabelaConvenioCodigoDto.Mapear(query);

                return tabelaConvenioCodigo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<TabelaConvenioCodigoDto> pacientesDtos = new List<TabelaConvenioCodigoDto>();
            try
            {
                //get com filtro
                var query = from p in _tabelaConvenioCodigoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Convenio.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Convenio.NomeFantasia.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Convenio.RazaoSocial.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.TabelaPrecoItem.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.TabelaPrecoItem.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Codigo ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
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
