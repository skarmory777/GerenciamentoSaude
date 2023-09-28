using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCancelamento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento
{
    public class MotivoCancelamentoAppService : SWMANAGERAppServiceBase, IMotivoCancelamentoAppService
    {
        private readonly IRepository<MotivoCancelamento, long> _MotivoCancelamentoRepositorio;
        private readonly IListarMotivosCancelamentoExcelExporter _listarMotivosCancelamentoExcelExporter;
        IUnitOfWorkManager _unitOfWorkManager;

        public MotivoCancelamentoAppService(IRepository<MotivoCancelamento, long> MotivoCancelamentoRepositorio,
            IListarMotivosCancelamentoExcelExporter listarMotivosCancelamentoExcelExporter,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _MotivoCancelamentoRepositorio = MotivoCancelamentoRepositorio;
            _listarMotivosCancelamentoExcelExporter = listarMotivosCancelamentoExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
        }


        [UnitOfWork]
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Edit)]
        public async Task CriarOuEditar(CriarOuEditarMotivoCancelamento input)
        {
            try
            {
                var MotivoCancelamento = input.MapTo<MotivoCancelamento>();
                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _MotivoCancelamentoRepositorio.InsertOrUpdateAsync(MotivoCancelamento);

                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _MotivoCancelamentoRepositorio.UpdateAsync(MotivoCancelamento);

                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }


        [UnitOfWork]
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Delete)]
        public async Task Excluir(CriarOuEditarMotivoCancelamento input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _MotivoCancelamentoRepositorio.DeleteAsync(input.Id);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }


        public async Task<PagedResultDto<MotivoCancelamentoDto>> Listar(ListarMotivosCancelamentoInput input)
        {
            var contarMotivosCancelamento = 0;
            List<MotivoCancelamento> MotivosCancelamento;
            List<MotivoCancelamentoDto> MotivosCancelamentoDtos = new List<MotivoCancelamentoDto>();
            try
            {
                var query = _MotivoCancelamentoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarMotivosCancelamento = await query
                    .CountAsync();

                MotivosCancelamento = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                MotivosCancelamentoDtos = MotivosCancelamento
                    .MapTo<List<MotivoCancelamentoDto>>();

                return new PagedResultDto<MotivoCancelamentoDto>(
                    contarMotivosCancelamento,
                    MotivosCancelamentoDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<FileDto> ListarParaExcel(ListarMotivosCancelamentoInput input)
        {
            try
            {
                var query = await Listar(input);

                var MotivosCancelamentoDtos = query.Items;

                return _listarMotivosCancelamentoExcelExporter.ExportToFile(MotivosCancelamentoDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }


        public async Task<CriarOuEditarMotivoCancelamento> Obter(long id)
        {
            try
            {
                var result = await _MotivoCancelamentoRepositorio.GetAsync(id);
                var MotivoCancelamento = result.MapTo<CriarOuEditarMotivoCancelamento>();
                return MotivoCancelamento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}