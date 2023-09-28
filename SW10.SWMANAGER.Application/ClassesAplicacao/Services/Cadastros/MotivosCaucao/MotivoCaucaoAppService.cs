using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCaucao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao
{
    public class MotivoCaucaoAppService : SWMANAGERAppServiceBase, IMotivoCaucaoAppService
    {
        private readonly IRepository<MotivoCaucao, long> _MotivoCaucaoRepositorio;
        private readonly IListarMotivosCaucaoExcelExporter _listarMotivosCaucaoExcelExporter;


        public MotivoCaucaoAppService(IRepository<MotivoCaucao, long> MotivoCaucaoRepositorio,
            IListarMotivosCaucaoExcelExporter listarMotivosCaucaoExcelExporter)
        {
            _MotivoCaucaoRepositorio = MotivoCaucaoRepositorio;
            _listarMotivosCaucaoExcelExporter = listarMotivosCaucaoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Edit)]
        public async Task CriarOuEditar(CriarOuEditarMotivoCaucao input)
        {
            try
            {
                var MotivoCaucao = input.MapTo<MotivoCaucao>();
                if (input.Id.Equals(0))
                {
                    await _MotivoCaucaoRepositorio.InsertOrUpdateAsync(MotivoCaucao);
                }
                else
                {
                    await _MotivoCaucaoRepositorio.UpdateAsync(MotivoCaucao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarMotivoCaucao input)
        {
            try
            {
                await _MotivoCaucaoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<MotivoCaucaoDto>> Listar(ListarMotivosCaucaoInput input)
        {
            var contarMotivosCaucao = 0;
            List<MotivoCaucao> MotivosCaucao;
            List<MotivoCaucaoDto> MotivosCaucaoDtos = new List<MotivoCaucaoDto>();
            try
            {
                var query = _MotivoCaucaoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper()
                        )
                    );

                contarMotivosCaucao = await query
                    .CountAsync();

                MotivosCaucao = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                MotivosCaucaoDtos = MotivosCaucao
                    .MapTo<List<MotivoCaucaoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<MotivoCaucaoDto>(
                contarMotivosCaucao,
                MotivosCaucaoDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarMotivosCaucaoInput input)
        {
            try
            {
                var query = await Listar(input);

                var MotivosCaucaoDtos = query.Items;

                return _listarMotivosCaucaoExcelExporter.ExportToFile(MotivosCaucaoDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarMotivoCaucao> Obter(long id)
        {
            try
            {
                var result = await _MotivoCaucaoRepositorio.GetAsync(id);
                var MotivoCaucao = result.MapTo<CriarOuEditarMotivoCaucao>();
                return MotivoCaucao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
