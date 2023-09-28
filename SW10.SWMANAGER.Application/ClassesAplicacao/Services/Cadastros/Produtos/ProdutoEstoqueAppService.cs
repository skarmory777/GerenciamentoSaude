using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public class ProdutoEstoqueAppService : SWMANAGERAppServiceBase, IProdutoEstoqueAppService
    {
        public async Task<ProdutoEstoqueDto> CriarOuEditar(ProdutoEstoqueDto input)
        {
            try
            {
                var produtoEstoque = input.MapTo<ProdutoEstoque>();//new ProdutoEstoque();

                using (var _produtoEstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEstoque, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    if (input.Id.Equals(0))
                    {
                        input.Id = await _produtoEstoqueRepositorio.Object.InsertAndGetIdAsync(produtoEstoque);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                    else
                    {
                        await _produtoEstoqueRepositorio.Object.UpdateAsync(produtoEstoque);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();

                    }
                    var query = await Obter(input.Id);
                    var newObj = query.MapTo<ProdutoEstoque>();

                    var produtoRelacaoPortariaDto = newObj.MapTo<ProdutoEstoqueDto>();
                    //produtoRelacaoPortariaDto.TipoUnidade = newObj.TipoUnidade;

                    return produtoRelacaoPortariaDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<ProdutoEstoqueDto> Obter(long id)
        {
            try
            {
                using (var _produtoEstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEstoque, long>>())
                {
                    var query = await _produtoEstoqueRepositorio.Object
                    .GetAll()
                    .Include(m => m.Estoque)
                    .FirstOrDefaultAsync(m => m.Id == id);

                    var produtoEstoque = query.MapTo<ProdutoEstoqueDto>();

                    return produtoEstoque;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ProdutoEstoqueDto>> ListarPorProduto(ListarInput input)
        {
            try
            {
                using (var _produtoEstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEstoque, long>>())
                {
                    var produtoId = Convert.ToInt64(input.Filtro);
                    var query = _produtoEstoqueRepositorio.Object
                        .GetAll()
                        .Include(m => m.Estoque)
                        .Where(w => w.ProdutoId == produtoId);

                    var contarProdutosEstoque = await query.CountAsync();

                    var produtosEstoque = await query
                         .AsNoTracking()
                         .OrderBy(input.Sorting)
                         .PageBy(input)
                         .ToListAsync();

                    var produtosEstoqueDtos = produtosEstoque
                         .MapTo<List<ProdutoEstoqueDto>>();

                    return new PagedResultDto<ProdutoEstoqueDto>(
                        contarProdutosEstoque,
                        produtosEstoqueDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(ProdutoEstoqueDto input)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                using (var _produtoEstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEstoque, long>>())
                {

                    await _produtoEstoqueRepositorio.Object.DeleteAsync(input.Id);

                    unitOfWork.Complete();
                    _unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public ListResultDto<EstoqueDto> ListarTodosNaoRelacionadosProdutos(long produtoId, long id)
        {
            try
            {
                using (var _produtoEstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEstoque, long>>())
                using (var _estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
                {

                    List<EstoqueDto> objListaDto = new List<EstoqueDto>();

                    var produtosEstoques = _produtoEstoqueRepositorio.Object.GetAll().Where(w => w.ProdutoId == produtoId);

                    var query = _estoqueRepositorio.Object
                        .GetAll()
                        .Where(w => !produtosEstoques.Any(a => a.EstoqueId == w.Id) || produtosEstoques.Any(a => a.Id == id && a.EstoqueId == w.Id));

                    //.Where(w =>  _produtoEstoqueRepositorio.GetAll()
                    //       .Any(a => a.ProdutoId == w.Id ));

                    objListaDto = EstoqueDto.Mapear(query.ToList());

                    return new ListResultDto<EstoqueDto> { Items = objListaDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
