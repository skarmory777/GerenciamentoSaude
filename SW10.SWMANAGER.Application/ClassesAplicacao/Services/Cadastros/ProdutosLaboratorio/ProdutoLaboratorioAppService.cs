using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class ProdutoLaboratorioAppService : SWMANAGERAppServiceBase, IProdutoLaboratorioAppService
    {
        private readonly IRepository<EstoqueLaboratorio, long> _produtoLaboratorioRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IListarProdutoLaboratorioExcelExporter _listarProdutoLaboratorioExcelExporter;
        private readonly IUltimoIdAppService _ultimoIdAppService;

        public ProdutoLaboratorioAppService(IRepository<EstoqueLaboratorio, long> produtoLaboratorioRepositorio,
            IUnitOfWorkManager unitOfWorkManager,
            IListarProdutoLaboratorioExcelExporter listarProdutoLaboratorioExcelExporter,
            IUltimoIdAppService ultimoServicoAppService)
        {
            _produtoLaboratorioRepositorio = produtoLaboratorioRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
            _listarProdutoLaboratorioExcelExporter = listarProdutoLaboratorioExcelExporter;
            _ultimoIdAppService = ultimoServicoAppService;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Edit)]
        [UnitOfWork]
        public async Task CriarOuEditar(ProdutoLaboratorioDto input)
        {
            try
            {
                var produtoLaboratorio = input.MapTo<EstoqueLaboratorio>();
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.Id.Equals(0))
                    {
                        produtoLaboratorio.Codigo = _ultimoIdAppService.ObterProximoCodigo("EstoqueLaboratorio").Result;

                        await _produtoLaboratorioRepositorio.InsertOrUpdateAsync(produtoLaboratorio);
                    }
                    else
                    {
                        await _produtoLaboratorioRepositorio.UpdateAsync(produtoLaboratorio);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Excluir(ProdutoLaboratorioDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoLaboratorioRepositorio.DeleteAsync(input.Id);

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProdutoLaboratorioDto>> Listar(ListarProdutosLaboratorioInput input)
        {
            var contarProdutosLaboratorio = 0;
            List<EstoqueLaboratorio> produtosLaboratorio;
            List<ProdutoLaboratorioDto> produtosLaboratorioDtos = new List<ProdutoLaboratorioDto>();
            try
            {
                var query = _produtoLaboratorioRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        //m.CodLaboratorio.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosLaboratorio = await query
                    .CountAsync();

                produtosLaboratorio = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosLaboratorioDtos = produtosLaboratorio
                    .MapTo<List<ProdutoLaboratorioDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoLaboratorioDto>(
                contarProdutosLaboratorio,
                produtosLaboratorioDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosLaboratorioInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosLaboratorioDtos = query.Items;

                return _listarProdutoLaboratorioExcelExporter.ExportToFile(produtosLaboratorioDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoLaboratorioDto> Obter(long id)
        {
            try
            {
                var result = await _produtoLaboratorioRepositorio.GetAsync(id);
                var produtoLaboratorio = result.MapTo<ProdutoLaboratorioDto>();
                return produtoLaboratorio;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<ProdutoLaboratorioDto>> ListarTodos()
        {
            List<ProdutoLaboratorioDto> produtosLaboratoriosDtos = new List<ProdutoLaboratorioDto>();
            try
            {
                var query = await _produtoLaboratorioRepositorio.GetAllListAsync();

                var produtosLaboratoriosDto = query.MapTo<List<ProdutoLaboratorioDto>>();

                return new ListResultDto<ProdutoLaboratorioDto> { Items = produtosLaboratoriosDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._produtoLaboratorioRepositorio).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }

}
