using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs
{
    public class FaturamentoGlobalAppService : SWMANAGERAppServiceBase, IFaturamentoGlobalAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoGlobal, long> _itemConfigRepository;
        private readonly IRepository<FaturamentoGrupo, long> _grupoRepository;

        public FaturamentoGlobalAppService(
            IRepository<FaturamentoGlobal, long> itemConfigRepository
                 ,
            IRepository<FaturamentoGrupo, long> grupoRepository
            )
        {
            _itemConfigRepository = itemConfigRepository;
            _grupoRepository = grupoRepository;

        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoGlobalDto>> Listar(ListarFaturamentoGlobaisInput input)
        {
            var globais = 0;
            List<FaturamentoGlobal> itens;
            List<FaturamentoGlobalDto> itensDtos = new List<FaturamentoGlobalDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    ;

                globais = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoGlobalDto>>();

                return new PagedResultDto<FaturamentoGlobalDto>(globais, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoGlobalDto>> ListarPorConvenio(ListarFaturamentoGlobaisInput input)
        {
            var globais = 0;
            List<FaturamentoGlobal> itens;
            List<FaturamentoGlobalDto> itensDtos = new List<FaturamentoGlobalDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    //  .Include(m => m.Empresa)
                    //   .Include(m => m.Grupo)
                    //         .Include(m => m.Plano)
                    //   .Include(m => m.SubGrupo)
                    //    .Include(m => m.Tabela)
                    //      .Include(m => m.Item)
                    //  .Where(c => c.ConvenioId.ToString() == input.Filtro)
                    //.WhereIf(input.PlanoEspecifico, i => i.PlanoId != null)
                    //.WhereIf(input.PlanoGlobal, i => i.PlanoId == null)
                    //.WhereIf(input.GrupoSubGrupo, i => i.SubGrupoId != null)
                    //.WhereIf(input.GrupoItem, i => i.ItemId != null)
                    ;

                globais = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoGlobalDto>>();

                return new PagedResultDto<FaturamentoGlobalDto>(globais, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoGlobalDto>> ListarPorItem(ListarFaturamentoGlobaisInput input)
        {
            var globais = 0;
            List<FaturamentoGlobal> itens;
            List<FaturamentoGlobalDto> itensDtos = new List<FaturamentoGlobalDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    //    .Include(m => m.Convenio)
                    //    .Include(m => m.Plano)
                    //    .Include(m => m.Item)
                    //    .Include(m => m.ItemCobrar)
                    //     .Where(c => c.ItemId.ToString() == input.Filtro)
                    //.WhereIf(input.PlanoEspecifico, i => i.PlanoId != null)
                    //.WhereIf(input.PlanoGlobal, i => i.PlanoId == null)
                    //.WhereIf(input.GrupoSubGrupo, i => i.SubGrupoId != null)
                    //.WhereIf(input.GrupoItem, i => i.ItemId != null)
                    ;

                globais = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoGlobalDto>>();

                return new PagedResultDto<FaturamentoGlobalDto>(globais, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarTodosGrupos(FaturamentoGlobalDto input)/*long convenioId, long empresaId, DateTime dataInicio)*/
        {
            try
            {
                foreach (var grupo in _grupoRepository.GetAll())
                {
                    var itemConfig = new FaturamentoGlobalDto();

                    //    itemConfig.EmpresaId = input.EmpresaId;
                    //       itemConfig.ConvenioId = input.ConvenioId;
                    //    itemConfig.GrupoId = grupo.Id;
                    //    itemConfig.DataIncio = input.DataIncio;
                    //     itemConfig.TabelaId = input.TabelaId;

                    await CriarOuEditar(itemConfig);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }
        }

        public async Task CriarOuEditar(FaturamentoGlobalDto input)
        {
            try
            {
                var ItemConfig = input.MapTo<FaturamentoGlobal>();

                if (input.Id.Equals(0))
                {
                    await _itemConfigRepository.InsertAsync(ItemConfig);
                }
                else
                {
                    await _itemConfigRepository.UpdateAsync(ItemConfig);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoGlobalDto input)
        {
            try
            {
                await _itemConfigRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoGlobalDto> Obter(long id)
        {
            try
            {
                var query = await _itemConfigRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoGlobalDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoGlobaisInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarItemConfigsExcelExporter.ExportToFile(itens.ToList());
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroExportar"));
            //}
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoGlobalDto> faturamentoItensDto = new List<FaturamentoGlobalDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }


                // Filtro usado como id de grupo
                var query = from p in _itemConfigRepository.GetAll()
                             //  .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Grupo.Id.ToString() == dropdownInput.filtro)
                             .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
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

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
