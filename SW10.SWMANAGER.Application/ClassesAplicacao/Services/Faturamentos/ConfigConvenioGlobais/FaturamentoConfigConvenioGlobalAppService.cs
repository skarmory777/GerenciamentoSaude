using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenioGlobais
{
    public class FaturamentoConfigConvenioGlobalAppService : SWMANAGERAppServiceBase, IFaturamentoConfigConvenioGlobalAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoConfigConvenioGlobal, long> _configConvenioRepository;
        private readonly IRepository<FaturamentoGrupo, long> _grupoRepository;

        public FaturamentoConfigConvenioGlobalAppService(
            IRepository<FaturamentoConfigConvenioGlobal, long> configConvenioRepository
                 ,
            IRepository<FaturamentoGrupo, long> grupoRepository
            )
        {
            _configConvenioRepository = configConvenioRepository;
            _grupoRepository = grupoRepository;

        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoConfigConvenioGlobalDto>> Listar(ListarFaturamentoConfigConvenioGlobaisInput input)
        {
            var itemrConfigConvenioGlobais = 0;
            List<FaturamentoConfigConvenioGlobal> itens;
            List<FaturamentoConfigConvenioGlobalDto> itensDtos = new List<FaturamentoConfigConvenioGlobalDto>();
            try
            {
                var query = _configConvenioRepository
                    .GetAll()
                    ;

                itemrConfigConvenioGlobais = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoConfigConvenioGlobalDto>>();

                return new PagedResultDto<FaturamentoConfigConvenioGlobalDto>(itemrConfigConvenioGlobais, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoConfigConvenioGlobalDto>> ListarPorConvenio(ListarFaturamentoConfigConvenioGlobaisInput input)
        {
            var itemrConfigConvenioGlobais = 0;
            List<FaturamentoConfigConvenioGlobal> itens;
            List<FaturamentoConfigConvenioGlobalDto> itensDtos = new List<FaturamentoConfigConvenioGlobalDto>();
            try
            {
                var query = _configConvenioRepository
                    .GetAll()
                    .Include(m => m.Empresa)
                    .Include(m => m.Grupo)
                    .Include(m => m.Plano)
                    .Include(m => m.SubGrupo)
                    .Include(m => m.TabelaGlobal)
                    .Include(m => m.Item)
                    .Where(c => c.ConvenioId.ToString() == input.Filtro)
                    .WhereIf(input.PlanoEspecifico, i => i.PlanoId != null)
                    .WhereIf(input.PlanoGlobal, i => i.PlanoId == null)
                    .WhereIf(input.GrupoSubGrupo, i => i.SubGrupoId != null)
                    .WhereIf(input.GrupoItem, i => i.ItemId != null)
                    ;

                itemrConfigConvenioGlobais = await query.CountAsync();

                itens = await query.AsNoTracking().ToListAsync();

                itensDtos = FaturamentoConfigConvenioGlobalDto.Mapear(itens);

                return new PagedResultDto<FaturamentoConfigConvenioGlobalDto>(itemrConfigConvenioGlobais, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarTodosGrupos(FaturamentoConfigConvenioGlobalDto input)/*long convenioId, long empresaId, DateTime dataInicio)*/
        {
            try
            {
                foreach (var grupo in _grupoRepository.GetAll())
                {
                    var configConvenio = new FaturamentoConfigConvenioGlobalDto();

                    configConvenio.EmpresaId = input.EmpresaId;
                    configConvenio.ConvenioId = input.ConvenioId;
                    configConvenio.GrupoId = grupo.Id;
                    configConvenio.DataIncio = input.DataIncio;
                    configConvenio.TabelaGlobalId = input.TabelaGlobalId;

                    await CriarOuEditar(configConvenio);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }
        }

        public async Task CriarOuEditar(FaturamentoConfigConvenioGlobalDto input)
        {
            try
            {
                var ConfigConvenioGlobal = input.MapTo<FaturamentoConfigConvenioGlobal>();
                if (input.Id.Equals(0))
                {
                    await _configConvenioRepository.InsertAsync(ConfigConvenioGlobal);
                }
                else
                {
                    await _configConvenioRepository.UpdateAsync(ConfigConvenioGlobal);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoConfigConvenioGlobalDto input)
        {
            try
            {
                await _configConvenioRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoConfigConvenioGlobalDto> Obter(long id)
        {
            try
            {
                var query = await _configConvenioRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoConfigConvenioGlobalDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoConfigConvenioGlobaisInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarConfigConvenioGlobaisExcelExporter.ExportToFile(itens.ToList());
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

            List<FaturamentoConfigConvenioGlobalDto> faturamentoItensDto = new List<FaturamentoConfigConvenioGlobalDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }


                // Filtro usado como id de grupo
                var query = from p in _configConvenioRepository.GetAll()
                             .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Grupo.Id.ToString() == dropdownInput.filtro)
                             .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.DataIncio ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.DataIncio)
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
