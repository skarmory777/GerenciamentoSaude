using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio
{
    public class GrupoTipoTabelaDominioAppService : SWMANAGERAppServiceBase, IGrupoTipoTabelaDominioAppService
    {
        private readonly IRepository<GrupoTipoTabelaDominio, long> _grupoTipoTabelaDominioRepositorio;
        private readonly IListarGrupoTipoTabelaDominioExcelExporter _listarGrupoTipoTabelaDominioExcelExporter;


        public GrupoTipoTabelaDominioAppService(IRepository<GrupoTipoTabelaDominio, long> grupoTipoTabelaDominioRepositorio,
            IListarGrupoTipoTabelaDominioExcelExporter listarGrupoTipoTabelaDominioExcelExporter)
        {
            _grupoTipoTabelaDominioRepositorio = grupoTipoTabelaDominioRepositorio;
            _listarGrupoTipoTabelaDominioExcelExporter = listarGrupoTipoTabelaDominioExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Edit)]
        public async Task CriarOuEditar(CriarOuEditarGrupoTipoTabelaDominio input)
        {
            try
            {
                var grupoTipoTabelaDominio = input.MapTo<GrupoTipoTabelaDominio>();
                if (input.Id.Equals(0))
                {
                    await _grupoTipoTabelaDominioRepositorio.InsertOrUpdateAsync(grupoTipoTabelaDominio);
                }
                else
                {
                    await _grupoTipoTabelaDominioRepositorio.UpdateAsync(grupoTipoTabelaDominio);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarGrupoTipoTabelaDominio input)
        {
            try
            {
                await _grupoTipoTabelaDominioRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        //public async Task<ListResultDto<GrupoTipoTabelaDominio>> ListarFull()
        //{
        //	try
        //	{
        //		var query = await _grupoTipoTabelaDominioRepositorio
        //			.GetAllListAsync();

        //		var gruposTiposTabelaDominioDto = query.MapTo<List<GrupoTipoTabelaDominio>>();

        //		return new ListResultDto<GrupoTipoTabelaDominio> { Items = gruposTiposTabelaDominioDto };
        //	}
        //	catch(Exception ex)
        //	{
        //		throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //	}
        //}

        public async Task<PagedResultDto<GrupoTipoTabelaDominioDto>> ListarFull()
        {
            var contarGruposTipoTabelaDominio = 0;
            List<GrupoTipoTabelaDominio> gruposTipoTabelaDominio;
            List<GrupoTipoTabelaDominioDto> gruposTipoTabelaDominioDtos = new List<GrupoTipoTabelaDominioDto>();
            try
            {
                var query = _grupoTipoTabelaDominioRepositorio
                    .GetAll()
                    .Include(m => m.TipoTabelaDominio);

                contarGruposTipoTabelaDominio = await query
                    .CountAsync();

                gruposTipoTabelaDominio = await query
                    .AsNoTracking()
                    .ToListAsync();

                gruposTipoTabelaDominioDtos = gruposTipoTabelaDominio
                    .MapTo<List<GrupoTipoTabelaDominioDto>>();

                return new PagedResultDto<GrupoTipoTabelaDominioDto>(
                contarGruposTipoTabelaDominio,
                gruposTipoTabelaDominioDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<GrupoTipoTabelaDominioDto>> Listar(ListarGruposTipoTabelaDominioInput input)
        {
            var contarGruposTipoTabelaDominio = 0;
            List<GrupoTipoTabelaDominio> tiposTabelaDominio;
            List<GrupoTipoTabelaDominioDto> tiposTabelaDominioDtos = new List<GrupoTipoTabelaDominioDto>();
            try
            {
                var query = _grupoTipoTabelaDominioRepositorio
                    .GetAll()
                    .Include(m => m.TipoTabelaDominio)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarGruposTipoTabelaDominio = await query
                    .CountAsync();

                tiposTabelaDominio = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tiposTabelaDominioDtos = tiposTabelaDominio
                    .MapTo<List<GrupoTipoTabelaDominioDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<GrupoTipoTabelaDominioDto>(
                contarGruposTipoTabelaDominio,
                tiposTabelaDominioDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarGruposTipoTabelaDominioInput input)
        {
            try
            {
                var query = await Listar(input);

                var tiposTabelaDominioDtos = query.Items;

                return _listarGrupoTipoTabelaDominioExcelExporter.ExportToFile(tiposTabelaDominioDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarGrupoTipoTabelaDominio> Obter(long id)
        {
            try
            {
                var result = await _grupoTipoTabelaDominioRepositorio
                    .GetAll()
                    .Include(m => m.TipoTabelaDominio)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var grupoTipoTabelaDominio = result.MapTo<CriarOuEditarGrupoTipoTabelaDominio>();
                return grupoTipoTabelaDominio;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GrupoTipoTabelaDominioDto>> ListarPorTipo(long tipoTabelaId)
        {
            try
            {
                var query = await _grupoTipoTabelaDominioRepositorio
                    .GetAll()
                    .Include(m => m.TipoTabelaDominio)
                    .Where(m => m.TipoTabelaDominioId == tipoTabelaId)
                    .AsNoTracking()
                    .ToListAsync();
                return new ListResultDto<GrupoTipoTabelaDominioDto>
                {
                    Items = query
                    .MapTo<List<GrupoTipoTabelaDominioDto>>()
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ICollection<GrupoTipoTabelaDominio>> ListarParaTabelaDominio(long? tabelaDominioId)
        {
            try
            {
                return await _grupoTipoTabelaDominioRepositorio
                    .GetAll()
                    .Include(m => m.TipoTabelaDominio)
                    .Where(m =>
                    m.TipoTabelaDominioId == tabelaDominioId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
