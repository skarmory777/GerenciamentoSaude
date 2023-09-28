using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio
{
    public class TipoTabelaDominioAppService : SWMANAGERAppServiceBase, ITipoTabelaDominioAppService
    {
        private readonly IRepository<TipoTabelaDominio, long> _tipoTabelaDominioRepositorio;
        private readonly IListarTipoTabelaDominioExcelExporter _listarTipoTabelaDominioExcelExporter;


        public TipoTabelaDominioAppService(IRepository<TipoTabelaDominio, long> tipoTabelaDominioRepositorio,
            IListarTipoTabelaDominioExcelExporter listarTipoTabelaDominioExcelExporter)
        {
            _tipoTabelaDominioRepositorio = tipoTabelaDominioRepositorio;
            _listarTipoTabelaDominioExcelExporter = listarTipoTabelaDominioExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(CriarOuEditarTipoTabelaDominio input)
        {
            try
            {
                var tipoTabelaDominio = input.MapTo<TipoTabelaDominio>();
                if (input.Id.Equals(0))
                {
                    await _tipoTabelaDominioRepositorio.InsertOrUpdateAsync(tipoTabelaDominio);
                }
                else
                {
                    await _tipoTabelaDominioRepositorio.UpdateAsync(tipoTabelaDominio);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarTipoTabelaDominio input)
        {
            try
            {
                await _tipoTabelaDominioRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<TipoTabelaDominio>> ListarTodos()
        {
            try
            {
                var query = await _tipoTabelaDominioRepositorio
                    .GetAllListAsync();

                var tiposTabelaDominioDto = query.MapTo<List<TipoTabelaDominio>>();

                return new ListResultDto<TipoTabelaDominio> { Items = tiposTabelaDominioDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TipoTabelaDominioDto>> Listar(ListarTiposTabelaDominioInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<TipoTabelaDominio> tiposTabelaDominio;
            List<TipoTabelaDominioDto> tiposTabelaDominioDtos = new List<TipoTabelaDominioDto>();
            try
            {
                var query = _tipoTabelaDominioRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                tiposTabelaDominio = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tiposTabelaDominioDtos = tiposTabelaDominio
                    .MapTo<List<TipoTabelaDominioDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoTabelaDominioDto>(
                contarTiposTabelaDominio,
                tiposTabelaDominioDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposTabelaDominioInput input)
        {
            try
            {
                var query = await Listar(input);

                var tiposTabelaDominioDtos = query.Items;

                return _listarTipoTabelaDominioExcelExporter.ExportToFile(tiposTabelaDominioDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarTipoTabelaDominio> Obter(long id)
        {
            try
            {
                var result = await _tipoTabelaDominioRepositorio.GetAsync(id);
                var tipoTabelaDominio = result.MapTo<CriarOuEditarTipoTabelaDominio>();
                return tipoTabelaDominio;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}