using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposEntrada;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada
{
    public class TipoEntradaAppService : SWMANAGERAppServiceBase, ITipoEntradaAppService
    {
        private readonly IRepository<TipoEntrada, long> _tipoEntradaRepositorio;
        private readonly IListarTipoEntradaExcelExporter _listarTipoEntradaExcelExporter;


        public TipoEntradaAppService(IRepository<TipoEntrada, long> tipoEntradaRepositorio,
            IListarTipoEntradaExcelExporter listarTipoEntradaExcelExporter)
        {
            _tipoEntradaRepositorio = tipoEntradaRepositorio;
            _listarTipoEntradaExcelExporter = listarTipoEntradaExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Edit)]
        public async Task CriarOuEditar(CriarOuEditarTipoEntrada input)
        {
            try
            {
                var tipoEntrada = input.MapTo<TipoEntrada>();
                if (input.Id.Equals(0))
                {
                    await _tipoEntradaRepositorio.InsertOrUpdateAsync(tipoEntrada);
                }
                else
                {
                    await _tipoEntradaRepositorio.UpdateAsync(tipoEntrada);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarTipoEntrada input)
        {
            try
            {
                await _tipoEntradaRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoEntradaDto>> Listar(ListarTiposEntradaInput input)
        {
            var contarTiposEntrada = 0;
            List<TipoEntrada> tipoEntradas;
            List<TipoEntradaDto> tipoEntradasDtos = new List<TipoEntradaDto>();
            try
            {
                var query = _tipoEntradaRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposEntrada = await query
                    .CountAsync();

                tipoEntradas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tipoEntradasDtos = tipoEntradas
                    .MapTo<List<TipoEntradaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoEntradaDto>(
                contarTiposEntrada,
                tipoEntradasDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposEntradaInput input)
        {
            try
            {
                var query = await Listar(input);

                var tipoEntradasDtos = query.Items;

                return _listarTipoEntradaExcelExporter.ExportToFile(tipoEntradasDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarTipoEntrada> Obter(long id)
        {
            try
            {
                var result = await _tipoEntradaRepositorio.GetAsync(id);
                var tipoEntrada = result.MapTo<CriarOuEditarTipoEntrada>();
                return tipoEntrada;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<TipoEntradaDto>> ListarTodos()
        {
            List<TipoEntradaDto> tipoEntradasDtos = new List<TipoEntradaDto>();
            try
            {
                var query = await _tipoEntradaRepositorio
                    .GetAllListAsync();

                var tipoEntradasDto = query.MapTo<List<TipoEntradaDto>>();

                return new ListResultDto<TipoEntradaDto> { Items = tipoEntradasDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
