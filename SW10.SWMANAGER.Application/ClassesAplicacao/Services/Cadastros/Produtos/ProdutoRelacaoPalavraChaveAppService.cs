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
    public class ProdutoRelacaoPalavraChaveAppService : SWMANAGERAppServiceBase, IProdutoRelacaoPalavraChaveAppService
    {
        private readonly IRepository<ProdutoRelacaoPalavraChave, long> _produtoRelacaoPalavraChaveRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProdutoRelacaoPalavraChaveAppService(
            IRepository<ProdutoRelacaoPalavraChave, long> produtoRelacaoPalavraChaveRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _produtoRelacaoPalavraChaveRepository = produtoRelacaoPalavraChaveRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        //public async Task<ProdutoRelacaoPalavraChaveDto> CriarOuEditar(ProdutoRelacaoPalavraChaveDto input, long id)
        public async Task<ProdutoRelacaoPalavraChaveDto> CriarOuEditar(ProdutoRelacaoPalavraChaveDto input)
        {
            try
            {
                var produtoRelacaoPalavraChave = new ProdutoRelacaoPalavraChave();
                produtoRelacaoPalavraChave = input.MapTo<ProdutoRelacaoPalavraChave>();
                var produtoRelacaoPalavraChaveDto = produtoRelacaoPalavraChave.MapTo<ProdutoRelacaoPalavraChaveDto>();
                if (input.Id.Equals(0))
                {

                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await _produtoRelacaoPalavraChaveRepository.InsertAndGetIdAsync(produtoRelacaoPalavraChave);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }

                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _produtoRelacaoPalavraChaveRepository.UpdateAsync(produtoRelacaoPalavraChave);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }

                var query = await Obter(input.Id);
                var newObj = query.MapTo<ProdutoRelacaoPalavraChave>();

                produtoRelacaoPalavraChaveDto = newObj.MapTo<ProdutoRelacaoPalavraChaveDto>();
                //produtoRelacaoPalavraChaveDto.TipoUnidade = newObj.TipoUnidade;

                return produtoRelacaoPalavraChaveDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<ProdutoRelacaoPalavraChaveDto> Obter(long id)
        {
            try
            {
                var query = await _produtoRelacaoPalavraChaveRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var produto = query.MapTo<ProdutoRelacaoPalavraChaveDto>();

                return produto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Editar(ProdutoRelacaoPalavraChaveDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoRelacaoPalavraChaveRepository.UpdateAsync(input.MapTo<ProdutoRelacaoPalavraChave>());
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
                    await _produtoRelacaoPalavraChaveRepository.DeleteAsync(id);
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

        public async Task<PagedResultDto<ProdutoRelacaoPalavraChaveDto>> ListarPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoRelacaoPalavraChave> Produtos;
            List<ProdutoRelacaoPalavraChaveDto> ProdutosDtos = new List<ProdutoRelacaoPalavraChaveDto>();
            try
            {
                var id = Convert.ToInt64(input.Filtro);
                var query = _produtoRelacaoPalavraChaveRepository
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
                    .MapTo<List<ProdutoRelacaoPalavraChaveDto>>();

                return new PagedResultDto<ProdutoRelacaoPalavraChaveDto>(
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
