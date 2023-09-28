using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade
{
    public class TipoUnidadeAppService : SWMANAGERAppServiceBase, ITipoUnidadeAppService
    {
        private readonly IRepository<UnidadeTipo, long> _tipoUnidadeRepositorio;
        private readonly IListarTipoUnidadeExcelExporter _listarTipoUnidadeExcelExporter;


        public TipoUnidadeAppService(IRepository<UnidadeTipo, long> tipoUnidadeRepositorio,
            IListarTipoUnidadeExcelExporter listarTipoUnidadeExcelExporter)
        {
            _tipoUnidadeRepositorio = tipoUnidadeRepositorio;
            _listarTipoUnidadeExcelExporter = listarTipoUnidadeExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Edit)]
        public async Task CriarOuEditar(CriarOuEditarTipoUnidade input)
        {
            try
            {
                var tipoUnidade = input.MapTo<UnidadeTipo>();
                if (input.Id.Equals(0))
                {
                    await _tipoUnidadeRepositorio.InsertOrUpdateAsync(tipoUnidade);
                }
                else
                {
                    await _tipoUnidadeRepositorio.UpdateAsync(tipoUnidade);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarTipoUnidade input)
        {
            try
            {
                await _tipoUnidadeRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoUnidadeDto>> Listar(ListarTiposUnidadeInput input)
        {
            var contarTiposUnidade = 0;
            List<UnidadeTipo> tiposUnidade;
            List<TipoUnidadeDto> tiposUnidadeDtos = new List<TipoUnidadeDto>();
            try
            {
                var query = _tipoUnidadeRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                contarTiposUnidade = await query
                    .CountAsync();

                tiposUnidade = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tiposUnidadeDtos = tiposUnidade
                    .MapTo<List<TipoUnidadeDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoUnidadeDto>(
                contarTiposUnidade,
                tiposUnidadeDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposUnidadeInput input)
        {
            try
            {
                var query = await Listar(input);

                var tiposUnidadeDtos = query.Items;

                return _listarTipoUnidadeExcelExporter.ExportToFile(tiposUnidadeDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarTipoUnidade> Obter(long id)
        {
            try
            {
                var result = await _tipoUnidadeRepositorio.GetAsync(id);
                var tipoUnidade = result.MapTo<CriarOuEditarTipoUnidade>();
                return tipoUnidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<TipoUnidadeDto>> ListarTodos()
        {
            List<TipoUnidadeDto> tiposUnidadeDtos = new List<TipoUnidadeDto>();
            try
            {
                var query = await _tipoUnidadeRepositorio
                    .GetAllListAsync();

                var tiposUnidadeDto = query.MapTo<List<TipoUnidadeDto>>();

                return new ListResultDto<TipoUnidadeDto> { Items = tiposUnidadeDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<TipoUnidadeDto>> ListarSemReferencialGerencial()
        {
            List<TipoUnidadeDto> tiposUnidadeDtos = new List<TipoUnidadeDto>();
            try
            {
                var query = await _tipoUnidadeRepositorio
                    .GetAllListAsync(u => u.Id != 1 && u.Id != 2);

                var tiposUnidadeDto = query.MapTo<List<TipoUnidadeDto>>();

                return new ListResultDto<TipoUnidadeDto> { Items = tiposUnidadeDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        Task ITipoUnidadeAppService.CriarOuEditar(CriarOuEditarTipoUnidade input)
        {
            throw new NotImplementedException();
        }

        Task ITipoUnidadeAppService.Excluir(CriarOuEditarTipoUnidade input)
        {
            throw new NotImplementedException();
        }

        Task<CriarOuEditarTipoUnidade> ITipoUnidadeAppService.Obter(long id)
        {
            throw new NotImplementedException();
        }

        Task<FileDto> ITipoUnidadeAppService.ListarParaExcel(ListarTiposUnidadeInput input)
        {
            throw new NotImplementedException();
        }
    }
}
