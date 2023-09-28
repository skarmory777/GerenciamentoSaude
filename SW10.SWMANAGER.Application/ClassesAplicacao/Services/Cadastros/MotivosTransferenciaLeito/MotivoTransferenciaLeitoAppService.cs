using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosTransferenciaLeito;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito
{
    public class MotivoTransferenciaLeitoAppService : SWMANAGERAppServiceBase, IMotivoTransferenciaLeitoAppService
    {
        private readonly IRepository<MotivoTransferenciaLeito, long> _MotivoTransferenciaLeitoRepositorio;
        private readonly IListarMotivosTransferenciaLeitoExcelExporter _listarMotivosTransferenciaLeitoExcelExporter;


        public MotivoTransferenciaLeitoAppService(IRepository<MotivoTransferenciaLeito, long> MotivoTransferenciaLeitoRepositorio,
            IListarMotivosTransferenciaLeitoExcelExporter listarMotivosTransferenciaLeitoExcelExporter)
        {
            _MotivoTransferenciaLeitoRepositorio = MotivoTransferenciaLeitoRepositorio;
            _listarMotivosTransferenciaLeitoExcelExporter = listarMotivosTransferenciaLeitoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Edit)]
        public async Task CriarOuEditar(CriarOuEditarMotivoTransferenciaLeito input)
        {
            try
            {
                var MotivoTransferenciaLeito = input.MapTo<MotivoTransferenciaLeito>();
                if (input.Id.Equals(0))
                {
                    await _MotivoTransferenciaLeitoRepositorio.InsertOrUpdateAsync(MotivoTransferenciaLeito);
                }
                else
                {
                    await _MotivoTransferenciaLeitoRepositorio.UpdateAsync(MotivoTransferenciaLeito);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarMotivoTransferenciaLeito input)
        {
            try
            {
                await _MotivoTransferenciaLeitoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<MotivoTransferenciaLeitoDto>> Listar(ListarMotivosTransferenciaLeitoInput input)
        {
            var contarMotivosTransferenciaLeito = 0;
            List<MotivoTransferenciaLeito> MotivosTransferenciaLeito;
            List<MotivoTransferenciaLeitoDto> MotivosTransferenciaLeitoDtos = new List<MotivoTransferenciaLeitoDto>();
            try
            {
                var query = _MotivoTransferenciaLeitoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper()
                        )
                    );

                contarMotivosTransferenciaLeito = await query
                    .CountAsync();

                MotivosTransferenciaLeito = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                MotivosTransferenciaLeitoDtos = MotivosTransferenciaLeito
                    .MapTo<List<MotivoTransferenciaLeitoDto>>();

                return new PagedResultDto<MotivoTransferenciaLeitoDto>(
                    contarMotivosTransferenciaLeito,
                    MotivosTransferenciaLeitoDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarMotivosTransferenciaLeitoInput input)
        {
            try
            {
                var query = await Listar(input);

                var MotivosTransferenciaLeitoDtos = query.Items;

                return _listarMotivosTransferenciaLeitoExcelExporter.ExportToFile(MotivosTransferenciaLeitoDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarMotivoTransferenciaLeito> Obter(long id)
        {
            try
            {
                var result = await _MotivoTransferenciaLeitoRepositorio.GetAsync(id);
                var MotivoTransferenciaLeito = result.MapTo<CriarOuEditarMotivoTransferenciaLeito>();
                return MotivoTransferenciaLeito;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
