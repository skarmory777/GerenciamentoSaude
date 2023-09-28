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
    public class ProdutoRelacaoLaboratorioAppService : SWMANAGERAppServiceBase, IProdutoRelacaoLaboratorioAppService
    {
        private readonly IRepository<ProdutoRelacaoLaboratorio, long> _produtoRelacaoLaboratorioRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProdutoRelacaoLaboratorioAppService(
            IRepository<ProdutoRelacaoLaboratorio, long> produtoRelacaoLaboratorioRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _produtoRelacaoLaboratorioRepository = produtoRelacaoLaboratorioRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task<ProdutoRelacaoLaboratorioDto> CriarOuEditar(ProdutoRelacaoLaboratorioDto input)
        {
            try
            {
                var produtoRelacaoLaboratorio = new ProdutoRelacaoLaboratorio();
                produtoRelacaoLaboratorio = input.MapTo<ProdutoRelacaoLaboratorio>();
                var produtoRelacaoLaboratorioDto = produtoRelacaoLaboratorio.MapTo<ProdutoRelacaoLaboratorioDto>();
                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await _produtoRelacaoLaboratorioRepository.InsertAndGetIdAsync(produtoRelacaoLaboratorio);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }

                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _produtoRelacaoLaboratorioRepository.UpdateAsync(produtoRelacaoLaboratorio);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                var query = await Obter(input.Id);
                var newObj = query.MapTo<ProdutoRelacaoLaboratorio>();

                produtoRelacaoLaboratorioDto = newObj.MapTo<ProdutoRelacaoLaboratorioDto>();
                //produtoRelacaoLaboratorioDto.TipoUnidade = newObj.TipoUnidade;

                return produtoRelacaoLaboratorioDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<ProdutoRelacaoLaboratorioDto> Obter(long id)
        {
            try
            {
                var query = await _produtoRelacaoLaboratorioRepository
                    .GetAll()
                    .Include(m => m.Produto)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var produto = query.MapTo<ProdutoRelacaoLaboratorioDto>();

                return produto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Editar(ProdutoRelacaoLaboratorioDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoRelacaoLaboratorioRepository.UpdateAsync(input.MapTo<ProdutoRelacaoLaboratorio>());
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
                    await _produtoRelacaoLaboratorioRepository.DeleteAsync(id);
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

        public async Task<PagedResultDto<ProdutoRelacaoLaboratorioDto>> ListarPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoRelacaoLaboratorio> Produtos;
            List<ProdutoRelacaoLaboratorioDto> ProdutosDtos = new List<ProdutoRelacaoLaboratorioDto>();
            try
            {
                var id = Convert.ToInt64(input.Filtro);
                var query = _produtoRelacaoLaboratorioRepository
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
                    .MapTo<List<ProdutoRelacaoLaboratorioDto>>();

                return new PagedResultDto<ProdutoRelacaoLaboratorioDto>(
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
