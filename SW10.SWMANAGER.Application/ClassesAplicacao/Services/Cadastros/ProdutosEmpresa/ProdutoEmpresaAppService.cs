using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa
{
    public class ProdutoEmpresaAppService : SWMANAGERAppServiceBase, IProdutoEmpresaAppService
    {
        private readonly IRepository<ProdutoEmpresa, long> _produtoEmpresaRepositorio;
        private readonly IListarProdutoEmpresaExcelExporter _listarProdutoEmpresaExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public ProdutoEmpresaAppService(IRepository<ProdutoEmpresa, long> produtoEmpresaRepositorio,
            IUnitOfWorkManager unitOfWorkManager,
            IListarProdutoEmpresaExcelExporter listarProdutoEmpresaExcelExporter)
        {
            _produtoEmpresaRepositorio = produtoEmpresaRepositorio;
            _listarProdutoEmpresaExcelExporter = listarProdutoEmpresaExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 
        /// </summary>
        //[AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Empresa_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Empresa_Edit)]
        [UnitOfWork]
        public async Task CriarOuEditar(ProdutoEmpresaDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var produtoEmpresa = input.MapTo<ProdutoEmpresa>();
                    if (input.Id.Equals(0))
                    {
                        await _produtoEmpresaRepositorio.InsertAsync(produtoEmpresa);
                    }
                    else
                    {
                        await _produtoEmpresaRepositorio.UpdateAsync(produtoEmpresa);
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

        /// <summary>
        /// 
        /// </summary>
        [UnitOfWork]
        public async Task Excluir(ProdutoEmpresaDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoEmpresaRepositorio.DeleteAsync(input.Id);

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

        public async Task<PagedResultDto<ProdutoEmpresaDto>> Listar(ListarProdutosEmpresaInput input)
        {
            var contarProdutosEmpresa = 0;
            List<ProdutoEmpresaDto> produtosEmpresaDtos = new List<ProdutoEmpresaDto>();
            try
            {
                var query = _produtoEmpresaRepositorio
                    .GetAll();
                //.WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                //m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                //);
                ///TODO: NOVO PRODUTO
                //var query = new List<ProdutoEmpresa>();

                contarProdutosEmpresa = await query
                    .CountAsync();

                //produtosEmpresa = await query
                //    .AsNoTracking()
                //    .OrderBy(input.Sorting)
                //    .PageBy(input)
                //    .ToListAsync();

                contarProdutosEmpresa = query
    .Count();

                //produtosEmpresa = query
                //    .OrderBy(input.Sorting)
                //    .ToListAsync();

                //produtosEmpresaDtos = produtosEmpresa
                //    .MapTo<List<ProdutoEmpresaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoEmpresaDto>(
                contarProdutosEmpresa,
                produtosEmpresaDtos
                );
        }

        //public async Task<ListResultDto<ProdutoEmpresaDto>> ListarTodos()
        //{
        //    List<ProdutoEmpresa> produtoEmpresas;
        //    List<ProdutoEmpresaDto> produtosEmpresasDtos = new List<ProdutoEmpresaDto>();
        //    try
        //    {
        //        produtoEmpresas = await _produtoEmpresaRepositorio
        //            .GetAll()
        //            .ToListAsync();

        //        produtosEmpresasDtos = produtoEmpresas
        //            .MapTo<List<ProdutoEmpresaDto>>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //    return new ListResultDto<SexoDto> { Items = produtosEmpresasDtos };
        //}

        public async Task<FileDto> ListarParaExcel(ListarProdutosEmpresaInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosEmpresaDtos = query.Items;

                return _listarProdutoEmpresaExcelExporter.ExportToFile(produtosEmpresaDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoEmpresaDto> Obter(long id)
        {
            try
            {
                var result = await _produtoEmpresaRepositorio.GetAsync(id);
                var produtoEmpresa = result.MapTo<ProdutoEmpresaDto>();
                return produtoEmpresa;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
