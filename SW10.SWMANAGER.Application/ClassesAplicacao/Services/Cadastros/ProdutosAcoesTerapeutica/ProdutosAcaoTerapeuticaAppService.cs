using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosAcoesTerapeutica;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica
{
    public class ProdutoAcaoTerapeuticaAppService : SWMANAGERAppServiceBase, IProdutoAcaoTerapeuticaAppService
    {
        private readonly IRepository<ProdutoAcaoTerapeutica, long> _produtoAcaoTerapeuticaRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IListarProdutoAcaoTerapeuticaExcelExporter _listarProdutoAcaoTerapeuticaExcelExporter;

        public ProdutoAcaoTerapeuticaAppService(IRepository<ProdutoAcaoTerapeutica, long> produtoAcaoTerapeuticaRepositorio,
            IUnitOfWorkManager unitOfWorkManager,
            IListarProdutoAcaoTerapeuticaExcelExporter listarProdutoAcaoTerapeuticaExcelExporter)
        {
            _produtoAcaoTerapeuticaRepositorio = produtoAcaoTerapeuticaRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
            _listarProdutoAcaoTerapeuticaExcelExporter = listarProdutoAcaoTerapeuticaExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Edit)]
        [UnitOfWork]
        public async Task CriarOuEditar(ProdutoAcaoTerapeuticaDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var produtoAcaoTerapeutica = input.MapTo<ProdutoAcaoTerapeutica>();
                    if (input.Id.Equals(0))
                    {
                        await _produtoAcaoTerapeuticaRepositorio.InsertOrUpdateAsync(produtoAcaoTerapeutica);
                    }
                    else
                    {
                        await _produtoAcaoTerapeuticaRepositorio.UpdateAsync(produtoAcaoTerapeutica);
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
        public async Task Excluir(ProdutoAcaoTerapeuticaDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoAcaoTerapeuticaRepositorio.DeleteAsync(input.Id);

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

        public async Task<PagedResultDto<ProdutoAcaoTerapeuticaDto>> Listar(ListarProdutosAcoesTerapeuticaInput input)
        {
            var contarProdutosAcoesTerapeutica = 0;
            List<ProdutoAcaoTerapeutica> produtosAcoesTerapeutica;
            List<ProdutoAcaoTerapeuticaDto> produtosAcoesTerapeuticaDtos = new List<ProdutoAcaoTerapeuticaDto>();
            try
            {
                var query = _produtoAcaoTerapeuticaRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                contarProdutosAcoesTerapeutica = await query
                    .CountAsync();

                produtosAcoesTerapeutica = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosAcoesTerapeuticaDtos = produtosAcoesTerapeutica
                    .MapTo<List<ProdutoAcaoTerapeuticaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoAcaoTerapeuticaDto>(
                contarProdutosAcoesTerapeutica,
                produtosAcoesTerapeuticaDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosAcoesTerapeuticaInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosAcoesTerapeuticaDtos = query.Items;

                return _listarProdutoAcaoTerapeuticaExcelExporter.ExportToFile(produtosAcoesTerapeuticaDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoAcaoTerapeuticaDto> Obter(long id)
        {
            try
            {
                var result = await _produtoAcaoTerapeuticaRepositorio.GetAsync(id);
                var produtoAcaoTerapeutica = result.MapTo<ProdutoAcaoTerapeuticaDto>();
                return produtoAcaoTerapeutica;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<ProdutoAcaoTerapeuticaDto>> ListarTodos()
        {
            List<ProdutoAcaoTerapeuticaDto> produtosAcaoTerapeuticasDtos = new List<ProdutoAcaoTerapeuticaDto>();
            try
            {
                var query = await _produtoAcaoTerapeuticaRepositorio
                    .GetAllListAsync();

                var produtosAcaoTerapeuticasDto = query.MapTo<List<ProdutoAcaoTerapeuticaDto>>();

                return new ListResultDto<ProdutoAcaoTerapeuticaDto> { Items = produtosAcaoTerapeuticasDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
