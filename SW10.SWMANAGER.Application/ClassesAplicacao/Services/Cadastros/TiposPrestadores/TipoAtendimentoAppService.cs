using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposPrestadores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores
{
    public class TipoPrestadorAppService : SWMANAGERAppServiceBase, ITipoPrestadorAppService
    {
        private readonly IRepository<TipoPrestador, long> _tipoAtendimentoRepositorio;
        private readonly IListarTipoPrestadorExcelExporter _listarTipoPrestadorExcelExporter;


        public TipoPrestadorAppService(IRepository<TipoPrestador, long> tipoAtendimentoRepositorio,
            IListarTipoPrestadorExcelExporter listarTipoPrestadorExcelExporter)
        {
            _tipoAtendimentoRepositorio = tipoAtendimentoRepositorio;
            _listarTipoPrestadorExcelExporter = listarTipoPrestadorExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposPrestadores_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposPrestadores_Edit)]
        public async Task CriarOuEditar(CriarOuEditarTipoPrestador input)
        {
            try
            {
                var tipoAtendimento = input.MapTo<TipoPrestador>();
                if (input.Id.Equals(0))
                {
                    await _tipoAtendimentoRepositorio.InsertOrUpdateAsync(tipoAtendimento);
                }
                else
                {
                    await _tipoAtendimentoRepositorio.UpdateAsync(tipoAtendimento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarTipoPrestador input)
        {
            try
            {
                await _tipoAtendimentoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoPrestadorDto>> Listar(ListarTiposPrestadoresInput input)
        {
            var contarTiposPrestadores = 0;
            List<TipoPrestador> tiposPrestadores;
            List<TipoPrestadorDto> tiposPrestadoresDtos = new List<TipoPrestadorDto>();
            try
            {
                var query = _tipoAtendimentoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposPrestadores = await query
                    .CountAsync();

                tiposPrestadores = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tiposPrestadoresDtos = tiposPrestadores
                    .MapTo<List<TipoPrestadorDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoPrestadorDto>(
                contarTiposPrestadores,
                tiposPrestadoresDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposPrestadoresInput input)
        {
            try
            {
                var query = await Listar(input);

                var tiposPrestadoresDtos = query.Items;

                return _listarTipoPrestadorExcelExporter.ExportToFile(tiposPrestadoresDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarTipoPrestador> Obter(long id)
        {
            try
            {
                var result = await _tipoAtendimentoRepositorio.GetAsync(id);
                var tipoAtendimento = result.MapTo<CriarOuEditarTipoPrestador>();
                return tipoAtendimento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
