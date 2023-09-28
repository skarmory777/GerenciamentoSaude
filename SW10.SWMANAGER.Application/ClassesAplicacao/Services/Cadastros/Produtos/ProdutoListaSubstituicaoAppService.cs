using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public class ProdutoListaSubstituicaoAppService : SWMANAGERAppServiceBase, IProdutoListaSubstituicaoAppService
    {
        private readonly IRepository<ProdutoListaSubstituicao, long> _produtoListaSubstituicaoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProdutoListaSubstituicaoAppService(IRepository<ProdutoListaSubstituicao, long> produtoListaSubstituicaoRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _produtoListaSubstituicaoRepository = produtoListaSubstituicaoRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task<ProdutoListaSubstituicaoDto> CriarOuEditar(ProdutoListaSubstituicaoDto input)
        {
            try
            {
                var produtoListaSubstituicao = new ProdutoListaSubstituicao();
                produtoListaSubstituicao = input.MapTo<ProdutoListaSubstituicao>();
                var produtoListaSubstituicaoDto = produtoListaSubstituicao.MapTo<ProdutoListaSubstituicaoDto>();

                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await _produtoListaSubstituicaoRepository.InsertAndGetIdAsync(produtoListaSubstituicao);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _produtoListaSubstituicaoRepository.UpdateAsync(produtoListaSubstituicao);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }

                var query = await Obter(input.Id);
                var newObj = query.MapTo<ProdutoListaSubstituicao>();

                produtoListaSubstituicaoDto = newObj.MapTo<ProdutoListaSubstituicaoDto>();
                //produtoListaSubstituicaoDto.TipoUnidade = newObj.TipoUnidade;

                return produtoListaSubstituicaoDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<ProdutoListaSubstituicaoDto> Obter(long id)
        {
            try
            {
                var query = await _produtoListaSubstituicaoRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var produto = query.MapTo<ProdutoListaSubstituicaoDto>();

                return produto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Editar(ProdutoListaSubstituicaoDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoListaSubstituicaoRepository.UpdateAsync(input.MapTo<ProdutoListaSubstituicao>());
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
        public async Task Excluir(long id)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoListaSubstituicaoRepository.DeleteAsync(id);
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

        public Task<PagedResultDto<ProdutoListaSubstituicaoDto>> Listar(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<ProdutoListaSubstituicaoDto>> ListarPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoListaSubstituicao> Produtos;
            List<ProdutoListaSubstituicaoDto> ProdutosDtos = new List<ProdutoListaSubstituicaoDto>();
            try
            {
                var id = Convert.ToInt64(input.Filtro);
                var query = _produtoListaSubstituicaoRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.ProdutoId == id
                    );

                contarProdutos = await query
                    .CountAsync();

                Produtos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                ProdutosDtos = Produtos
                    .MapTo<List<ProdutoListaSubstituicaoDto>>();

                return new PagedResultDto<ProdutoListaSubstituicaoDto>(
                    contarProdutos,
                    ProdutosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
