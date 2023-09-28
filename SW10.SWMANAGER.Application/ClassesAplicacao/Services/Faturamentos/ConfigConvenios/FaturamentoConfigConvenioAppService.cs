using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios
{
    public class FaturamentoConfigConvenioAppService : SWMANAGERAppServiceBase, IFaturamentoConfigConvenioAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoConfigConvenio, long> _configConvenioRepository;
        private readonly IRepository<FaturamentoGrupo, long> _grupoRepository;

        public FaturamentoConfigConvenioAppService(
            IRepository<FaturamentoConfigConvenio, long> configConvenioRepository,
            IRepository<FaturamentoGrupo, long> grupoRepository
            )
        {
            _configConvenioRepository = configConvenioRepository;
            _grupoRepository = grupoRepository;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoConfigConvenioDto>> Listar(ListarFaturamentoConfigConveniosInput input)
        {
            var itemrConfigConvenios = 0;
            List<FaturamentoConfigConvenio> itens;
            List<FaturamentoConfigConvenioDto> itensDtos = new List<FaturamentoConfigConvenioDto>();
            try
            {
                var query = _configConvenioRepository.GetAll();

                itemrConfigConvenios = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens.MapTo<List<FaturamentoConfigConvenioDto>>();

                return new PagedResultDto<FaturamentoConfigConvenioDto>(itemrConfigConvenios, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoConfigConvenioDto>> ListarPorConvenio(ListarFaturamentoConfigConveniosInput input)
        {
            var itemrConfigConvenios = 0;
            List<FaturamentoConfigConvenio> itens = new List<FaturamentoConfigConvenio>();
            List<FaturamentoConfigConvenioDto> itensDtos = new List<FaturamentoConfigConvenioDto>();
            try
            {
                var queryConfig = _configConvenioRepository.GetAll();

                // var dt = queryConfig.Where(w => w.DataIncio <= DateTime.Now).Max(m => m.DataIncio);

                var query = _configConvenioRepository.GetAll().Include(m => m.Empresa).Include(m => m.Grupo)
                    .Include(m => m.Plano).Include(m => m.SubGrupo).Include(m => m.Tabela).Include(m => m.Item).Where(
                        c => c.ConvenioId == input.ConvenioId
                             && (c.EmpresaId == input.EmpresaId || queryConfig.Any(a => a.EmpresaId == null))
                             && (c.PlanoId == input.PlanoId || !queryConfig.Any(
                                     a => (a.PlanoId == input.PlanoId) && a.ConvenioId == input.ConvenioId))
                             && (c.GrupoId == input.GrupoId || !queryConfig.Any(
                                     a => (a.GrupoId == input.GrupoId) && a.ConvenioId == input.ConvenioId))
                             && (c.SubGrupoId == input.SubGrupoId || !queryConfig.Any(
                                     a => (a.SubGrupoId == input.SubGrupoId) && a.ConvenioId == input.ConvenioId))
                             && (c.ItemId == input.ItemId || !queryConfig.Any(
                                     a => (a.ItemId == input.ItemId) && a.ConvenioId == input.ConvenioId))
                             && (c.DataIncio <= DateTime.Now)).OrderBy(o => o.DataIncio);

                itensDtos = itens.Select(x => FaturamentoConfigConvenioDto.Mapear(x)).ToList();
                return new PagedResultDto<FaturamentoConfigConvenioDto>(itens.Count(), itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<PagedResultDto<FaturamentoConfigConvenioDto>> ListarPorConvenio2(ListarFaturamentoConfigConveniosInput input)
        {
            try
            {
                var queryConfig = _configConvenioRepository.GetAll();
                var query = _configConvenioRepository
                    .GetAll()
                    .Include(m => m.Empresa)
                    .Include(m => m.Grupo)
                    .Include(m => m.Plano)
                    .Include(m => m.SubGrupo)
                    .Include(m => m.Tabela)
                    .Include(m => m.Item)
                .Where(c => c.ConvenioId == input.ConvenioId)
                .WhereIf(input.PlanoEspecifico, i => i.PlanoId != null)
                .WhereIf(input.PlanoGlobal, i => i.PlanoId == null)
                .WhereIf(input.Grupo, i => i.GrupoId != null)
                .WhereIf(input.GrupoSubGrupo, i => i.SubGrupoId != null)
                .WhereIf(input.GrupoItem, i => i.ItemId != null);

                var items = (await query.OrderBy(input.Sorting).PageBy(input).ToListAsync()).Select(FaturamentoConfigConvenioDto.Mapear).ToList();
                var totalItens = await query.CountAsync();
                return new PagedResultDto<FaturamentoConfigConvenioDto>(totalItens, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarTodosGrupos(FaturamentoConfigConvenioDto input)/*long convenioId, long empresaId, DateTime dataInicio)*/
        {
            try
            {
                foreach (var grupo in _grupoRepository.GetAll())
                {
                    var configConvenio = new FaturamentoConfigConvenioDto();

                    configConvenio.EmpresaId = input.EmpresaId;
                    configConvenio.ConvenioId = input.ConvenioId;
                    configConvenio.GrupoId = grupo.Id;
                    configConvenio.DataIncio = input.DataIncio;
                    configConvenio.TabelaId = input.TabelaId;
                    configConvenio.Codigo = input.Codigo;

                    await CriarOuEditar(configConvenio);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }
        }

        public async Task CriarOuEditar(FaturamentoConfigConvenioDto input)
        {
            try
            {
                var ConfigConvenio = FaturamentoConfigConvenioDto.Mapear(input);

                if (input.Id.Equals(0))
                {
                    await _configConvenioRepository.InsertAsync(ConfigConvenio);
                }
                else
                {
                    await _configConvenioRepository.UpdateAsync(ConfigConvenio);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<int> VerificarDuplicata(FaturamentoConfigConvenioDto input)
        {
            var qtdDuplicatas = await _configConvenioRepository.GetAll().AsNoTracking().CountAsync(x =>
                x.ConvenioId == input.ConvenioId &&
                x.EmpresaId == input.EmpresaId &&
                x.GrupoId == input.GrupoId &&
                x.SubGrupoId == input.SubGrupoId &&
                x.DataIncio == input.DataIncio &&
                x.Id != input.Id);

            return qtdDuplicatas;
        }

        public async Task Excluir(FaturamentoConfigConvenioDto input)
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

        public async Task<FaturamentoConfigConvenioDto> Obter(long id)
        {
            try
            {
                var query = await _configConvenioRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();
                if(query == null)
                {
                    return new FaturamentoConfigConvenioDto();
                }
                return FaturamentoConfigConvenioDto.Mapear(query);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoConfigConveniosInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarConfigConveniosExcelExporter.ExportToFile(itens.ToList());
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

            List<FaturamentoConfigConvenioDto> faturamentoItensDto = new List<FaturamentoConfigConvenioDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }


                // Filtro usado como id de grupo
                var query = from p in _configConvenioRepository.GetAll()

                             .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Grupo.Id.ToString() == dropdownInput.filtro)
                             .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()))

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

        public void GerarLote()
        {
            var tiss = new mensagemTISS();


            tiss.cabecalho = new cabecalhoTransacao();

            var destino = new cabecalhoTransacaoDestino();

            destino.Item = "dsopfksoadfkdsf";

            tiss.cabecalho.destino = destino;


            var xmlserializer = new XmlSerializer(typeof(mensagemTISS));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, tiss);
                var asdfadsf = stringWriter.ToString();
            }



            // var xml = tiss
        }
    }
}
