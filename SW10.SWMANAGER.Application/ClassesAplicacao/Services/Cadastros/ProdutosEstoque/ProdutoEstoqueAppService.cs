using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Authorization;
using SW10.SWMANAGER.Authorization;
using System.Data.Entity;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque
{
    public class ProdutoEstoqueAppService : SWMANAGERAppServiceBase, IProdutoEstoqueAppService
    {
        private readonly IRepository<ProdutoEstoque, long> _produtoEstoqueRepositorio;
        //private readonly IListarProdutoEstoqueExcelExporter _listarProdutoEstoqueExcelExporter;


        public ProdutoEstoqueAppService(IRepository<ProdutoEstoque, long> produtoEstoqueRepositorio
            //,
            //IListarProdutoEstoqueExcelExporter listarProdutoEstoqueExcelExporter)
        {
            _produtoEstoqueRepositorio = produtoEstoqueRepositorio;
          //  _listarProdutoEstoqueExcelExporter = listarProdutoEstoqueExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Edit)]
        public async Task CriarOuEditar(ProdutoEstoqueDto input)
        {
            try
            {
                var produtoEstoque = input.MapTo<ProdutoEstoque>();
                if (input.Id.Equals(0))
                {
                    await _produtoEstoqueRepositorio.InsertOrUpdateAsync(produtoEstoque);
                }
                else
                {
                    await _produtoEstoqueRepositorio.UpdateAsync(produtoEstoque);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }

        }

        public async Task Excluir(ProdutoEstoqueDto input)
        {
            try
            {
                await _produtoEstoqueRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"));
            }

        }

        public async Task<PagedResultDto<ProdutoEstoqueDto>> Listar(ListarProdutosEstoqueInput input)
        {
            var contarProdutosEstoque = 0;
            List<ProdutoEstoque> produtosEstoque;
            List<ProdutoEstoqueDto> produtosEstoqueDtos = new List<ProdutoEstoqueDto>();
            try
            {
                var query = _produtoEstoqueRepositorio
                    .GetAll();
                    //.WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    //m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    //);
                    ///TODO: NOVO PRODUTO
                //var query = new List<ProdutoEstoque>();

                contarProdutosEstoque = await query
                    .CountAsync();

                //produtosEstoque = await query
                //    .AsNoTracking()
                //    .OrderBy(input.Sorting)
                //    .PageBy(input)
                //    .ToListAsync();

                contarProdutosEstoque = query
    .Count();

                //produtosEstoque = query
                //    .OrderBy(input.Sorting)
                //    .ToListAsync();

                //produtosEstoqueDtos = produtosEstoque
                //    .MapTo<List<ProdutoEstoqueDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
            return new PagedResultDto<ProdutoEstoqueDto>(
                contarProdutosEstoque,
                produtosEstoqueDtos
                );
        }

        //public async Task<ListResultDto<ProdutoEstoqueDto>> ListarTodos()
        //{
        //    List<ProdutoEstoque> produtoEstoques;
        //    List<ProdutoEstoqueDto> produtosEstoquesDtos = new List<ProdutoEstoqueDto>();
        //    try
        //    {
        //        produtoEstoques = await _produtoEstoqueRepositorio
        //            .GetAll()
        //            .ToListAsync();

        //        produtosEstoquesDtos = produtoEstoques
        //            .MapTo<List<ProdutoEstoqueDto>>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"));
        //    }
        //    return new ListResultDto<SexoDto> { Items = produtosEstoquesDtos };
        //}


        //public async Task<FileDto> ListarParaExcel(ListarProdutosEstoqueInput input)
        //{
        //    try
        //    {
        //        var query = await Listar(input);

        //        var produtosEstoqueDtos = query.Items;

        //        return _listarProdutoEstoqueExcelExporter.ExportToFile(produtosEstoqueDtos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }

        //}

        public async Task<ProdutoEstoqueDto> Obter(long id)
        {
            try
            {
                var result = await _produtoEstoqueRepositorio.GetAsync(id);
                var produtoEstoque = result.MapTo<ProdutoEstoqueDto>();
                return produtoEstoque;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

    }
}
