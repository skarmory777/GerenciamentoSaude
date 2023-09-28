using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Authorization;
using SW10.SWMANAGER.Authorization;
using System.Data.Entity;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposDocumento;
using Abp.UI;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento
{
    public class TipoDocumentoAppService : SWMANAGERAppServiceBase, ITipoDocumentoAppService
    {
        private readonly IRepository<TipoDocumento, long> _tipoDocumentoRepositorio;
        private readonly IListarTipoDocumentoExcelExporter _listarTipoDocumentoExcelExporter;


        public TipoDocumentoAppService(IRepository<TipoDocumento, long> tipoDocumentoRepositorio,
            IListarTipoDocumentoExcelExporter listarTipoDocumentoExcelExporter)
        {
            _tipoDocumentoRepositorio = tipoDocumentoRepositorio;
            _listarTipoDocumentoExcelExporter = listarTipoDocumentoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Edit)]
        public async Task CriarOuEditar(CriarOuEditarTipoDocumento input)
        {
            try
            {
                var tipoDocumento = input.MapTo<TipoDocumento>();

                if (input.Id.Equals(0))
                {
                    await _tipoDocumentoRepositorio.InsertOrUpdateAsync(tipoDocumento);
                }
                else
                {
                    await _tipoDocumentoRepositorio.UpdateAsync(tipoDocumento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }

        }

        public async Task Excluir(CriarOuEditarTipoDocumento input)
        {
            try
            {
                await _tipoDocumentoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"));
            }

        }

        public async Task<PagedResultDto<TipoDocumentoDto>> Listar(ListarTiposDocumentoInput input)
        {
            var contarTiposDocumento = 0;
            List<TipoDocumento> tipoDocumentos;
            List<TipoDocumentoDto> tipoDocumentosDtos = new List<TipoDocumentoDto>();
            try
            {
                var query = _tipoDocumentoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposDocumento = await query
                    .CountAsync();

                tipoDocumentos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tipoDocumentosDtos = tipoDocumentos
                    .MapTo<List<TipoDocumentoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
            return new PagedResultDto<TipoDocumentoDto>(
                contarTiposDocumento,
                tipoDocumentosDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposDocumentoInput input)
        {
            try
            {
                var query = await Listar(input);

                var tipoDocumentosDtos = query.Items;

                return _listarTipoDocumentoExcelExporter.ExportToFile(tipoDocumentosDtos.ToList());
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarTipoDocumento> Obter(long id)
        {
            try
            {
                var result = await _tipoDocumentoRepositorio.GetAsync(id);
                var tipoDocumento = result.MapTo<CriarOuEditarTipoDocumento>();
                return tipoDocumento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }

        public async Task<ListResultDto<TipoDocumentoDto>> ListarTodos()
        {
            //List<TipoDocumentoDto> tipoDocumentosDtos = new List<TipoDocumentoDto>();
            try
            {
                var query = await _tipoDocumentoRepositorio
                    .GetAllListAsync();

                var tipoDocumentosDto = query.MapTo<List<TipoDocumentoDto>>();

                return new ListResultDto<TipoDocumentoDto> { Items = tipoDocumentosDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task<ListResultDto<TipoDocumentoDto>> ListarPorEntradaSaida(bool isEntrada)
        {
            //List<TipoDocumentoDto> tipoDocumentosDtos = new List<TipoDocumentoDto>();
            try
            {
                var query = _tipoDocumentoRepositorio
                    .GetAll()
                    .Where(w => w.IsEntrada == isEntrada).ToList();

                var tipoDocumentosDto = query.MapTo<List<TipoDocumentoDto>>();

                return new ListResultDto<TipoDocumentoDto> { Items = tipoDocumentosDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }
    }
}
