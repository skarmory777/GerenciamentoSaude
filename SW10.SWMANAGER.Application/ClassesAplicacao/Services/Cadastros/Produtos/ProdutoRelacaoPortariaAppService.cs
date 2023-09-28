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
    public class ProdutoRelacaoPortariaAppService : SWMANAGERAppServiceBase, IProdutoRelacaoPortariaAppService
    {
        private readonly IRepository<ProdutoRelacaoPortaria, long> _produtoRelacaoPortariaRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public ProdutoRelacaoPortariaAppService(
            IRepository<ProdutoRelacaoPortaria, long> produtoRelacaoPortariaRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _produtoRelacaoPortariaRepository = produtoRelacaoPortariaRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task<ProdutoRelacaoPortariaDto> CriarOuEditar(ProdutoRelacaoPortariaDto input)
        {
            try
            {
                var produtoRelacaoPortaria = new ProdutoRelacaoPortaria();
                produtoRelacaoPortaria = input.MapTo<ProdutoRelacaoPortaria>();
                var produtoRelacaoPortariaDto = produtoRelacaoPortaria.MapTo<ProdutoRelacaoPortariaDto>();
                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await _produtoRelacaoPortariaRepository.InsertAndGetIdAsync(produtoRelacaoPortaria);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }

                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _produtoRelacaoPortariaRepository.UpdateAsync(produtoRelacaoPortaria);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                var query = await Obter(input.Id);
                var newObj = query.MapTo<ProdutoRelacaoPortaria>();

                produtoRelacaoPortariaDto = newObj.MapTo<ProdutoRelacaoPortariaDto>();
                //produtoRelacaoPortariaDto.TipoUnidade = newObj.TipoUnidade;

                return produtoRelacaoPortariaDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<ProdutoRelacaoPortariaDto> Obter(long id)
        {
            try
            {
                var query = await _produtoRelacaoPortariaRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var produto = query.MapTo<ProdutoRelacaoPortariaDto>();

                return produto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Editar(ProdutoRelacaoPortariaDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoRelacaoPortariaRepository.UpdateAsync(input.MapTo<ProdutoRelacaoPortaria>());
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
                    await _produtoRelacaoPortariaRepository.DeleteAsync(id);
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

        public async Task<PagedResultDto<ProdutoRelacaoPortariaDto>> Listar(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoRelacaoPortaria> Produtos;
            List<ProdutoRelacaoPortariaDto> ProdutosDtos = new List<ProdutoRelacaoPortariaDto>();
            try
            {
                var query = _produtoRelacaoPortariaRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Produto.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.ProdutoPortaria.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutos = await query
                    .CountAsync();

                Produtos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                ProdutosDtos = Produtos
                    .MapTo<List<ProdutoRelacaoPortariaDto>>();

                return new PagedResultDto<ProdutoRelacaoPortariaDto>(
                    contarProdutos,
                    ProdutosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ProdutoRelacaoPortariaDto>> ListarPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoRelacaoPortaria> Produtos;
            List<ProdutoRelacaoPortariaDto> ProdutosDtos = new List<ProdutoRelacaoPortariaDto>();
            try
            {
                var id = Convert.ToInt64(input.Filtro);
                var query = _produtoRelacaoPortariaRepository
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
                    .MapTo<List<ProdutoRelacaoPortariaDto>>();

                return new PagedResultDto<ProdutoRelacaoPortariaDto>(
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
