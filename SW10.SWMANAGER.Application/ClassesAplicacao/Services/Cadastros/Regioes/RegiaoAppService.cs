using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Regioes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes
{
    public class RegiaoAppService : SWMANAGERAppServiceBase, IRegiaoAppService
    {
        private readonly IRepository<Regiao, long> _regiaoRepositorio;
        private readonly IListarRegiaoExcelExporter _listarRegiaoExcelExporter;


        public RegiaoAppService(IRepository<Regiao, long> regiaoRepositorio,
            IListarRegiaoExcelExporter listarRegiaoExcelExporter)
        {
            _regiaoRepositorio = regiaoRepositorio;
            _listarRegiaoExcelExporter = listarRegiaoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Edit)]
        public async Task CriarOuEditar(CriarOuEditarRegiao input)
        {
            try
            {
                var regiao = input.MapTo<Regiao>();
                if (input.Id.Equals(0))
                {
                    await _regiaoRepositorio.InsertOrUpdateAsync(regiao);
                }
                else
                {
                    await _regiaoRepositorio.UpdateAsync(regiao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarRegiao input)
        {
            try
            {
                await _regiaoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<RegiaoDto>> Listar(ListarRegioesInput input)
        {
            var contarRegioes = 0;
            List<Regiao> regioes;
            List<RegiaoDto> regioesDtos = new List<RegiaoDto>();
            try
            {
                var query = _regiaoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarRegioes = await query
                    .CountAsync();

                regioes = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                regioesDtos = regioes
                    .MapTo<List<RegiaoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<RegiaoDto>(
                contarRegioes,
                regioesDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarRegioesInput input)
        {
            try
            {
                var query = await Listar(input);

                var regioesDtos = query.Items;

                return _listarRegiaoExcelExporter.ExportToFile(regioesDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarRegiao> Obter(long id)
        {
            try
            {
                var result = await _regiaoRepositorio.GetAsync(id);
                var regiao = result.MapTo<CriarOuEditarRegiao>();
                return regiao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
