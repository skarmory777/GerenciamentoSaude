using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.TiposAtendimento;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Exporting;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento
{
    public class TipoAtendimentoAppService : SWMANAGERAppServiceBase, ITipoAtendimentoAppService
    {
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Edit)]
        public async Task CriarOuEditar(TipoAtendimentoDto input)
        {
            using (var _tipoAtendimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtendimento, long>>())
            {
                try
                {
                    var tipoAtendimento = TipoAtendimentoDto.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _tipoAtendimentoRepositorio.Object.InsertOrUpdateAsync(tipoAtendimento).ConfigureAwait(false);
                    }
                    else
                    {
                        await _tipoAtendimentoRepositorio.Object.UpdateAsync(tipoAtendimento).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroSalvar"), ex);
                }
            }

        }

        public async Task Excluir(TipoAtendimentoDto input)
        {
            try
            {
                using (var _tipoAtendimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtendimento, long>>())                
                    await _tipoAtendimentoRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoAtendimentoDto>> Listar(ListarTiposAtendimentoInput input)
        {
            using (var _tipoAtendimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtendimento, long>>())
            {
                var contarTiposAtendimento = 0;
                List<TipoAtendimento> tiposAtendimento;
                List<TipoAtendimentoDto> tiposAtendimentoDtos = new List<TipoAtendimentoDto>();
                try
                {
                    var query = _tipoAtendimentoRepositorio.Object
                        .GetAll()
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Codigo.Contains(input.Filtro) ||
                            m.Descricao.Contains(input.Filtro)
                        );

                    contarTiposAtendimento = await query
                                                 .CountAsync().ConfigureAwait(false);

                    tiposAtendimento = await query
                                           .AsNoTracking()
                                           .OrderBy(input.Sorting)
                                           .PageBy(input)
                                           .ToListAsync().ConfigureAwait(false);

                    tiposAtendimentoDtos = TipoAtendimentoDto.Mapear(tiposAtendimento);

                    return new PagedResultDto<TipoAtendimentoDto>(
                        contarTiposAtendimento,
                        tiposAtendimentoDtos
                        );
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposAtendimentoInput input)
        {
            using (var _listarTipoAtendimentoExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarTipoAtendimentoExcelExporter>())
            {
                try
                {
                    var query = await Listar(input).ConfigureAwait(false);

                    var tiposAtendimentoDtos = query.Items;

                    return _listarTipoAtendimentoExcelExporter.Object.ExportToFile(tiposAtendimentoDtos.ToList());
                }
                catch (Exception)
                {
                    throw new UserFriendlyException(L("ErroExportar"));
                }
            }

        }

        public async Task<TipoAtendimentoDto> Obter(long id)
        {
            using (var _tipoAtendimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtendimento, long>>())
            {
                try
                {
                    var result = await _tipoAtendimentoRepositorio.Object.GetAsync(id).ConfigureAwait(false);
                    var tipoAtendimento = TipoAtendimentoDto.Mapear(result);

                    return tipoAtendimento;
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var _tipoAtendimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtendimento, long>>())            
                return await this.CreateSelect2(_tipoAtendimentoRepositorio.Object)
                       .AddIdField("AteAtendimentoTipo.Id")
                       .AddTextField("CONCAT(AteAtendimentoTipo.Codigo, ' - ', AteAtendimentoTipo.Descricao)")
                       .AddFromClause("AteAtendimentoTipo INNER JOIN SisTabelaDominio ON AteAtendimentoTipo.SisTabelaItemTissId = SisTabelaDominio.Id")
                       .AddWhereMethod((input, dapperParamters) =>
                           {
                               var whereBuilder = new StringBuilder();

                               whereBuilder.Append(
                                   " AND AteAtendimentoTipo.IsDeleted = @deleted AND SisTabelaDominio.IsDeleted = @deleted ");

                               whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND (AteAtendimentoTipo.Descricao LIKE '%' + @search + '%' OR AteAtendimentoTipo.Codigo LIKE '%' + @search + '%')");

                               dapperParamters.Add("deleted", false);

                               var isInternacao = false;
                               var isAmbulatorioEmergencia = false;

                               if (input.filtros != null && input.filtros.Count() >= 2)
                               {
                                   bool.TryParse(dropdownInput.filtros[0], out isAmbulatorioEmergencia);
                                   bool.TryParse(dropdownInput.filtros[1], out isInternacao);
                               }

                               whereBuilder.WhereIf(
                                   isAmbulatorioEmergencia,
                                   " AND SisTabelaDominio.TipoTabelaDominioId = @tipoAtendimento");

                               whereBuilder.WhereIf(isInternacao, " AND SisTabelaDominio.TipoTabelaDominioId = @tipoInternacao");

                               dapperParamters.Add("tipoAtendimento", (long)EnumTipoTabelaDominio.TipoAtendimento);

                               dapperParamters.Add("tipoInternacao", (long)EnumTipoTabelaDominio.TipoInternação);
                               return whereBuilder.ToString();
                           }).AddOrderByClause("AteAtendimentoTipo.Descricao")
                       .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
