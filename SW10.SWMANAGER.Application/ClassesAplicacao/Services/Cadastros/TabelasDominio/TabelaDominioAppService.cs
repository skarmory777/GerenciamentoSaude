using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio
{
    using Abp.Auditing;
    using Abp.Domain.Uow;
    using SW10.SWMANAGER.Helpers;
    using System.Text;

    public class TabelaDominioAppService : SWMANAGERAppServiceBase, ITabelaDominioAppService
    {
        private readonly IRepository<TabelaDominio, long> _tabelaDominioRepository;
        private readonly IListarTabelaDominioExcelExporter _listarTabelaDominioExcelExporter;

        public TabelaDominioAppService(IRepository<TabelaDominio, long> tabelaDominioRepository,
            IListarTabelaDominioExcelExporter listarTabelaDominioExcelExporter)
        {
            _tabelaDominioRepository = tabelaDominioRepository;
            _listarTabelaDominioExcelExporter = listarTabelaDominioExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Edit)]
        public async Task CriarOuEditar(TabelaDominioDto input)
        {
            try
            {
                var tabelaDominio = TabelaDominioDto.Mapear(input); //.MapTo<TabelaDominio>();
                if (input.Id.Equals(0))
                {
                    await _tabelaDominioRepository.InsertOrUpdateAsync(tabelaDominio);
                }
                else
                {
                    await _tabelaDominioRepository.UpdateAsync(tabelaDominio);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TabelaDominioDto input)
        {
            try
            {
                await _tabelaDominioRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TabelaDominioDto>> Listar(ListarTabelasDominioInput input)
        {
            var contarTabelasDominio = 0;
            List<TabelaDominio> tabelasDominio;
            List<TabelaDominioDto> tabelasDominioDtos = new List<TabelaDominioDto>();

            try
            {
                var query = _tabelaDominioRepository
                    .GetAll()
                    .Include(m => m.GrupoTipoTabelaDominio)
                    .Include(m => m.TabelaDominioVersoesTiss)
                    .Include(m => m.TipoTabelaDominio)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    )
                    .WhereIf(!input.TipoTabelaId.Equals(0), m =>
                        m.TipoTabelaDominioId == input.TipoTabelaId
                    )
                    .WhereIf(!input.VersaoTissId.Equals(0), m =>
                        m.TabelaDominioVersoesTiss.ToList().Any(v => v.VersaoTissId == input.VersaoTissId)
                    );

                contarTabelasDominio = await query
                    .CountAsync();

                tabelasDominio = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tabelasDominioDtos = tabelasDominio
                    .MapTo<List<TabelaDominioDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TabelaDominioDto>(
                contarTabelasDominio,
                tabelasDominioDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarTabelasDominioInput input)
        {
            try
            {
                var query = await Listar(input);

                var tiposTabelaDominioDtos = query.Items;

                return _listarTabelaDominioExcelExporter.ExportToFile(tiposTabelaDominioDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        //public async Task<ListResultDto<CidadeDto>> ListarPorEstado(long? id)
        //      {
        //          try
        //          {
        //              var query = _cidadeRepository
        //                  .GetAll();

        //              var result = await query
        //                  .WhereIf(id.HasValue,
        //                      m => m.EstadoId == id
        //                  )
        //                  .ToListAsync();

        //              var cidades = new ListResultDto<CidadeDto>
        //              {
        //                  Items = result.MapTo<List<CidadeDto>>()
        //              };

        //              return cidades;
        //          }
        //          catch (Exception ex)
        //          {
        //              throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //          }

        //      }

        public async Task<TabelaDominioDto> Obter(long id)
        {
            try
            {
                var result = await _tabelaDominioRepository
                    .GetAll()
                    .Include(m => m.GrupoTipoTabelaDominio)
                    .Include(m => m.TabelaDominioVersoesTiss)
                    .Include(m => m.TipoTabelaDominio)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var tabelaDominio = TabelaDominioDto.Mapear(result); //.MapTo<CriarOuEditarTabelaDominio>();
                return tabelaDominio;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {

            return await this.CreateSelect2(this._tabelaDominioRepository)
                       .AddWhereMethod((input, dapperParamters) =>
                           {
                               var whereBuilder = new StringBuilder(
                                   Select2Helper.DefaultWhereMethod(input, dapperParamters));

                               long tabelaDominioId = 0;
                               long.TryParse(input.tabelaDominioTiss, out tabelaDominioId);

                               whereBuilder.Append(" AND TipoTabelaDominioId = @tabelaDominioId");

                               dapperParamters.Add("tabelaDominioId", tabelaDominioId);

                               return whereBuilder.ToString();
                           })
                       .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarPorTipoAtendimentoDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<TabelaDominioDto> pacientesDtos = new List<TabelaDominioDto>();
            var isInternacao = false;
            var isAmbulatorioEmergencia = false;
            bool.TryParse(dropdownInput.filtros[0], out isAmbulatorioEmergencia);
            bool.TryParse(dropdownInput.filtros[1], out isInternacao);
            try
            {
                if ((!isInternacao && !isAmbulatorioEmergencia) || (isInternacao && isAmbulatorioEmergencia))
                {
                    throw new Exception(L("InformarTipoAtendimento"));
                }
                //get com filtro
                var query = from p in _tabelaDominioRepository.GetAll()
                                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                            m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                            || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()))
                                        .WhereIf(isInternacao, m =>
                                            m.TipoTabelaDominio.Id == (long)EnumTipoTabelaDominio.TipoInternação)
                                        .WhereIf(isAmbulatorioEmergencia, m =>
                                            m.TipoTabelaDominio.Id == (long)EnumTipoTabelaDominio.TipoAtendimento)

                            orderby p.Descricao ascending
                            select new DropdownItems<long> { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
