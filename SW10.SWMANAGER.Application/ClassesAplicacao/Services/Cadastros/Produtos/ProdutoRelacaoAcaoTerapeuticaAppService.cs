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
    public class ProdutoRelacaoAcaoTerapeuticaAppService : SWMANAGERAppServiceBase, IProdutoRelacaoAcaoTerapeuticaAppService
    {
        private readonly IRepository<ProdutoRelacaoAcaoTerapeutica, long> _produtoRelacaoAcaoTerapeuticaRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProdutoRelacaoAcaoTerapeuticaAppService(
            IRepository<ProdutoRelacaoAcaoTerapeutica, long> produtoRelacaoAcaoTerapeuticaRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _produtoRelacaoAcaoTerapeuticaRepository = produtoRelacaoAcaoTerapeuticaRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        //public async Task<ProdutoRelacaoAcaoTerapeuticaDto> CriarOuEditar(ProdutoRelacaoAcaoTerapeuticaDto input, long id)
        public async Task<ProdutoRelacaoAcaoTerapeuticaDto> CriarOuEditar(ProdutoRelacaoAcaoTerapeuticaDto input)
        {
            try
            {
                var produtoRelacaoAcaoTerapeutica = new ProdutoRelacaoAcaoTerapeutica();
                produtoRelacaoAcaoTerapeutica = input.MapTo<ProdutoRelacaoAcaoTerapeutica>();
                var produtoRelacaoAcaoTerapeuticaDto = produtoRelacaoAcaoTerapeutica.MapTo<ProdutoRelacaoAcaoTerapeuticaDto>();

                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await _produtoRelacaoAcaoTerapeuticaRepository.InsertAndGetIdAsync(produtoRelacaoAcaoTerapeutica);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }

                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _produtoRelacaoAcaoTerapeuticaRepository.UpdateAsync(produtoRelacaoAcaoTerapeutica);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }

                var query = await Obter(input.Id);
                var newObj = query.MapTo<ProdutoRelacaoAcaoTerapeutica>();

                produtoRelacaoAcaoTerapeuticaDto = newObj.MapTo<ProdutoRelacaoAcaoTerapeuticaDto>();
                //produtoRelacaoLaboratorioDto.TipoUnidade = newObj.TipoUnidade;

                return produtoRelacaoAcaoTerapeuticaDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<ProdutoRelacaoAcaoTerapeuticaDto> Obter(long id)
        {
            try
            {
                var query = await _produtoRelacaoAcaoTerapeuticaRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var produto = query.MapTo<ProdutoRelacaoAcaoTerapeuticaDto>();

                return produto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Editar(ProdutoRelacaoAcaoTerapeuticaDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoRelacaoAcaoTerapeuticaRepository.UpdateAsync(input.MapTo<ProdutoRelacaoAcaoTerapeutica>());
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
                    await _produtoRelacaoAcaoTerapeuticaRepository.DeleteAsync(id);
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

        public async Task<PagedResultDto<ProdutoRelacaoAcaoTerapeuticaDto>> ListarPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoRelacaoAcaoTerapeutica> Produtos;
            List<ProdutoRelacaoAcaoTerapeuticaDto> ProdutosDtos = new List<ProdutoRelacaoAcaoTerapeuticaDto>();
            try
            {
                var id = Convert.ToInt64(input.Filtro);
                var query = _produtoRelacaoAcaoTerapeuticaRepository
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
                    .MapTo<List<ProdutoRelacaoAcaoTerapeuticaDto>>();

                return new PagedResultDto<ProdutoRelacaoAcaoTerapeuticaDto>(
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
