
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos
{
    public class FaturamentoGrupoAppService : SWMANAGERAppServiceBase, IFaturamentoGrupoAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoGrupo, long> _grupoRepository;
        private readonly IRepository<FaturamentoGrupoConvenio, long> _grupoConvenioRepository;

        public FaturamentoGrupoAppService(
            IRepository<FaturamentoGrupo, long> grupoRepository,
            IRepository<FaturamentoGrupoConvenio, long> grupoConvenioRepository
            )
        {
            _grupoRepository = grupoRepository;
            _grupoConvenioRepository = grupoConvenioRepository;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoGrupoDto>> Listar(ListarFaturamentoGruposInput input)
        {
            var itemrGrupos = 0;
            List<FaturamentoGrupo> itens;
            List<FaturamentoGrupoDto> itensDtos = new List<FaturamentoGrupoDto>();
            try
            {
                var query = _grupoRepository
                    .GetAll()
                    .Include(m => m.TipoGrupo)
                    //.WhereIf(!input.EstadoId.Equals(0), m =>
                    //    m.EstadoId == input.EstadoId
                    //)
                    ;

                itemrGrupos = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoGrupoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoGrupoDto>(
                itemrGrupos,
                itensDtos
                );
        }

        public async Task<ResultDropdownList> ListarPorTipo(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoGrupoDto> faturamentoItensDto = new List<FaturamentoGrupoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _grupoRepository.GetAll()
                            .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.TipoGrupoId.ToString() == dropdownInput.filtro)
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

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long> CriarOuEditar(FaturamentoGrupoDto input)
        {
            try
            {
                var Grupo = input.MapTo<FaturamentoGrupo>();
                if (input.Id.Equals(0))
                {
                    return await _grupoRepository.InsertAndGetIdAsync(Grupo);
                }
                else
                {
                    return await _grupoRepository.InsertOrUpdateAndGetIdAsync(Grupo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoGrupoDto input)
        {
            try
            {
                await _grupoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoGrupoDto> Obter(long id)
        {
            try
            {
                var query = await _grupoRepository
                      .GetAll()
                      .Include(m => m.TipoGrupo)
                      .Where(m => m.Id == id)
                      .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoGrupoDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoGrupoDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _grupoRepository
                    .GetAll()
                    //.Include(m => m.Estado)
                    //.Where(m =>
                    //    m.Nome.ToUpper().Equals(nome.ToUpper()) &&
                    //    m.EstadoId.Equals(estadoId)
                    //)
                    ;

                var result = await query.FirstOrDefaultAsync();

                var item = result
                    .MapTo<FaturamentoGrupoDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoGruposInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarGruposExcelExporter.ExportToFile(itens.ToList());
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroExportar"));
            //}
        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var FaturamentoGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoGrupo, long>>())
            {
                return await this.CreateSelect2(FaturamentoGrupoRepository.Object)
                            .AddTextField("CONCAT(Codigo, ' - ', Descricao)")
                            .EnableDistinct()
                           .AddWhereMethod(
                               (input, dapperParameters) =>
                               {
                                   dapperParameters.Add("deleted", false);
                                   var whereBuilder = new StringBuilder();
                                   whereBuilder.Append("IsDeleted = @deleted");

                                   whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND (Descricao LIKE '%' + @search + '%' OR Descricao LIKE '%' + @search + '%')");
                                   return whereBuilder.ToString();
                               }).AddOrderByClause("CONCAT(Codigo, ' - ', Descricao)").ExecuteAsync(dropdownInput)
                           .ConfigureAwait(false);
            }
        }

        // FATURAMENTO GRUPO CONVENIO
        public async Task<PagedResultDto<FaturamentoGrupoConvenioDto>> ListarConfigPorGrupo(ListarFaturamentoGruposConveniosInput input)
        {
            var itemrGrupos = 0;
            List<FaturamentoGrupoConvenio> itens;
            List<FaturamentoGrupoConvenioDto> itensDtos = new List<FaturamentoGrupoConvenioDto>();
            try
            {
                var query = _grupoConvenioRepository
                    .GetAll()
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(m => m.Grupo)
                    .WhereIf(!input.Filtro.Equals(0), m =>
                        m.GrupoId.ToString() == input.Filtro
                    )
                    ;

                itemrGrupos = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens.MapTo<List<FaturamentoGrupoConvenioDto>>();

                return new PagedResultDto<FaturamentoGrupoConvenioDto>(
                    itemrGrupos,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long> CriarOuEditarGrupoConvenio(FaturamentoGrupoConvenioDto input)
        {
            try
            {
                var Grupo = input.MapTo<FaturamentoGrupoConvenio>();
                if (input.Id.Equals(0))
                {
                    return await _grupoConvenioRepository.InsertAndGetIdAsync(Grupo);
                }
                else
                {
                    return await _grupoConvenioRepository.InsertOrUpdateAndGetIdAsync(Grupo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<FaturamentoGrupoConvenioDto> ObterGrupoConvenio(long id)
        {
            try
            {
                var query = await _grupoConvenioRepository
                      .GetAll()
                      // .Include(m => m.TipoGrupo)
                      .Where(m => m.Id == id)
                      .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoGrupoConvenioDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }


    }
}

