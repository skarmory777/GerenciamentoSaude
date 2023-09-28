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
    public class FaturamentoItemConfigGlobalAppService : SWMANAGERAppServiceBase, IFaturamentoItemConfigGlobalAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoItemConfigGlobal, long> _itemConfigRepository;
        private readonly IRepository<FaturamentoGrupo, long> _grupoRepository;
        private readonly IRepository<FaturamentoItem, long> _fatItemRepository;

        public FaturamentoItemConfigGlobalAppService(
            IRepository<FaturamentoItemConfigGlobal, long> itemConfigRepository
                 ,
            IRepository<FaturamentoGrupo, long> grupoRepository
            ,
            IRepository<FaturamentoItem, long> fatItemRepository
            )
        {
            _itemConfigRepository = itemConfigRepository;
            _grupoRepository = grupoRepository;
            _fatItemRepository = fatItemRepository;

        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoItemConfigGlobalDto>> Listar(ListarFaturamentoItemConfigGlobaisInput input)
        {
            var itemrItemConfigs = 0;
            List<FaturamentoItemConfigGlobal> itens;
            List<FaturamentoItemConfigGlobalDto> itensDtos = new List<FaturamentoItemConfigGlobalDto>();
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
                    .MapTo<List<FaturamentoItemConfigGlobalDto>>();

                return new PagedResultDto<FaturamentoItemConfigGlobalDto>(itemrItemConfigs, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoItemConfigGlobalDto>> ListarPorConvenio(ListarFaturamentoItemConfigGlobaisInput input)
        {
            var itemrItemConfigs = 0;
            List<FaturamentoItemConfigGlobal> itens;
            List<FaturamentoItemConfigGlobalDto> itensDtos = new List<FaturamentoItemConfigGlobalDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    //  .Include(m => m.Empresa)
                    //   .Include(m => m.Grupo)
                    //    .Include(m => m.Plano)
                    //   .Include(m => m.SubGrupo)
                    //    .Include(m => m.Tabela)
                    .Include(m => m.Item)
                    //    .Where(c => c.ConvenioId.ToString() == input.Filtro)
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
                    .MapTo<List<FaturamentoItemConfigGlobalDto>>();

                return new PagedResultDto<FaturamentoItemConfigGlobalDto>(itemrItemConfigs, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FatItemConfigGlobalGenerico>> ListarPorItem(ListarFaturamentoItemConfigGlobaisInput input)
        {
            var itemrItemConfigs = 0;
            List<FaturamentoItemConfigGlobal> itens;
            List<FaturamentoItemConfigGlobalDto> itensDtos = new List<FaturamentoItemConfigGlobalDto>();
            try
            {
                var query = _itemConfigRepository
                    .GetAll()
                    .Include(m => m.Global)
                    .Where(c => c.ItemId.ToString() == input.Filtro)
                    ;

                itemrItemConfigs = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var i in itens)
                {
                    if (i.ItemCobrarId != null)
                        i.ItemCobrar = await _fatItemRepository.GetAsync((long)i.ItemCobrarId);
                }

                itensDtos = itens
                    .MapTo<List<FaturamentoItemConfigGlobalDto>>();

                List<FatItemConfigGlobalGenerico> genericos = new List<FatItemConfigGlobalGenerico>();
                foreach (var x in itensDtos)
                {
                    var gen = new FatItemConfigGlobalGenerico();
                    gen.Id = x.Id;
                    gen.Global = new GenericoIdNome();
                    gen.ItemCobrar = new GenericoIdNome();
                    gen.Global.Id = x.GlobalId != null ? (long)x.GlobalId : 0;
                    gen.Global.Nome = x.Global?.Descricao;
                    gen.ItemCobrar.Id = x.ItemCobrarId != null ? (long)x.ItemCobrarId : 0;
                    gen.ItemCobrar.Nome = x.ItemCobrar?.Descricao;
                    genericos.Add(gen);
                }

                return new PagedResultDto<FatItemConfigGlobalGenerico>(itemrItemConfigs, genericos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarTodosGrupos(FaturamentoItemConfigGlobalDto input)/*long convenioId, long empresaId, DateTime dataInicio)*/
        {
            try
            {
                foreach (var grupo in _grupoRepository.GetAll())
                {
                    var itemConfig = new FaturamentoItemConfigGlobalDto();

                    //    itemConfig.EmpresaId = input.EmpresaId;
                    //     itemConfig.ConvenioId = input.ConvenioId;
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

        public async Task CriarOuEditar(FaturamentoItemConfigGlobalDto input)
        {
            if (input.ItemId == null || input.GlobalId == null || input.ItemCobrarId == null)
                return;

            try
            {
                var ItemConfig = input.MapTo<FaturamentoItemConfigGlobal>();

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

        public async Task Excluir(FaturamentoItemConfigGlobalDto input)
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

        public async Task<FaturamentoItemConfigGlobalDto> Obter(long id)
        {
            try
            {
                var query = await _itemConfigRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoItemConfigGlobalDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoItemConfigGlobaisInput input)
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

            List<FaturamentoItemConfigGlobalDto> faturamentoItensDto = new List<FaturamentoItemConfigGlobalDto>();
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

    public class FatItemConfigGlobalGenerico
    {
        public long Id { get; set; }
        public GenericoIdNome Global { get; set; }
        public GenericoIdNome ItemCobrar { get; set; }
    }
}
