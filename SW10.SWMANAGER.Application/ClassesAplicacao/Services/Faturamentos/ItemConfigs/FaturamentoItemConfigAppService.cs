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
    public class FaturamentoItemConfigAppService : SWMANAGERAppServiceBase, IFaturamentoItemConfigAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoItemConfig, long> _itemConfigRepository;
        private readonly IRepository<FaturamentoGrupo, long> _grupoRepository;

        public FaturamentoItemConfigAppService(
            IRepository<FaturamentoItemConfig, long> itemConfigRepository
                 ,
            IRepository<FaturamentoGrupo, long> grupoRepository
            )
        {
            _itemConfigRepository = itemConfigRepository;
            _grupoRepository = grupoRepository;

        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoItemConfigDto>> Listar(ListarFaturamentoItemConfigsInput input)
        {
            var itemrItemConfigs = 0;
            List<FaturamentoItemConfig> itens;
            List<FaturamentoItemConfigDto> itensDtos = new List<FaturamentoItemConfigDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    ;

                itemrItemConfigs = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoItemConfigDto>>();

                return new PagedResultDto<FaturamentoItemConfigDto>(itemrItemConfigs, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoItemConfigDto>> ListarPorConvenio(ListarFaturamentoItemConfigsInput input)
        {
            var itemrItemConfigs = 0;
            List<FaturamentoItemConfig> itens;
            List<FaturamentoItemConfigDto> itensDtos = new List<FaturamentoItemConfigDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    //  .Include(m => m.Empresa)
                    //   .Include(m => m.Grupo)
                    .Include(m => m.Plano)
                    //   .Include(m => m.SubGrupo)
                    //    .Include(m => m.Tabela)
                    .Include(m => m.Item)
                    .Where(c => c.ConvenioId.ToString() == input.Filtro)
                    //.WhereIf(input.PlanoEspecifico, i => i.PlanoId != null)
                    //.WhereIf(input.PlanoGlobal, i => i.PlanoId == null)
                    //.WhereIf(input.GrupoSubGrupo, i => i.SubGrupoId != null)
                    //.WhereIf(input.GrupoItem, i => i.ItemId != null)
                    ;

                itemrItemConfigs = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoItemConfigDto>>();

                return new PagedResultDto<FaturamentoItemConfigDto>(itemrItemConfigs, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoItemConfigDto>> ListarPorItem(ListarFaturamentoItemConfigsInput input)
        {
            var itemrItemConfigs = 0;
            List<FaturamentoItemConfigDto> itensDtos = new List<FaturamentoItemConfigDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(m => m.Plano)
                    //.Include(m => m.Item)
                    .Include(m => m.ItemCobrar)
                    .Where(c => c.ItemId.ToString() == input.Filtro)
                    //.WhereIf(input.PlanoEspecifico, i => i.PlanoId != null)
                    //.WhereIf(input.PlanoGlobal, i => i.PlanoId == null)
                    //.WhereIf(input.GrupoSubGrupo, i => i.SubGrupoId != null)
                    //.WhereIf(input.GrupoItem, i => i.ItemId != null)
                    ;

                itemrItemConfigs = await query
                    .CountAsync();

                itensDtos = (await query
                    .AsNoTracking()
                    .ToListAsync())
                    .Select(s => FaturamentoItemConfigDto.Mapear(s))
                    .ToList()
                    ;

                return new PagedResultDto<FaturamentoItemConfigDto>(itemrItemConfigs, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarTodosGrupos(FaturamentoItemConfigDto input)/*long convenioId, long empresaId, DateTime dataInicio)*/
        {
            try
            {
                foreach (var grupo in _grupoRepository.GetAll())
                {
                    var itemConfig = new FaturamentoItemConfigDto();

                    //    itemConfig.EmpresaId = input.EmpresaId;
                    itemConfig.ConvenioId = input.ConvenioId;
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

        public async Task CriarOuEditar(FaturamentoItemConfigDto input)
        {
            try
            {
                var ItemConfig = input.MapTo<FaturamentoItemConfig>();

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

        public async Task Excluir(FaturamentoItemConfigDto input)
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

        public async Task<FaturamentoItemConfigDto> Obter(long id)
        {
            try
            {
                var query = await _itemConfigRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var item = FaturamentoItemConfigDto.Mapear(query);

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoItemConfigsInput input)
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

            List<FaturamentoItemConfigDto> faturamentoItensDto = new List<FaturamentoItemConfigDto>();
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
                            orderby p.ItemCobrar.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.ItemCobrar.Descricao)
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
