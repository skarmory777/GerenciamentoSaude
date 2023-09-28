using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPalavrasChave;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave
{
    public class ProdutoPalavraChaveAppService : SWMANAGERAppServiceBase, IProdutoPalavraChaveAppService
    {
        private readonly IRepository<ProdutoPalavraChave, long> _produtoPalavraChaveRepositorio;
        IUnitOfWorkManager _unitOfWorkManager;
        private readonly IListarProdutoPalavraChaveExcelExporter _listarProdutoPalavraChaveExcelExporter;

        public ProdutoPalavraChaveAppService(IRepository<ProdutoPalavraChave, long> produtoPalavraChaveRepositorio,
        IUnitOfWorkManager unitOfWorkManager,
        IListarProdutoPalavraChaveExcelExporter listarProdutoPalavraChaveExcelExporter)
        {
            _produtoPalavraChaveRepositorio = produtoPalavraChaveRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
            _listarProdutoPalavraChaveExcelExporter = listarProdutoPalavraChaveExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Edit)]
        [UnitOfWork]
        public async Task CriarOuEditar(CriarOuEditarProdutoPalavraChave input)
        {
            try
            {
                var produtoPalavraChave = input.MapTo<ProdutoPalavraChave>();
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.Id.Equals(0))
                    {
                        await _produtoPalavraChaveRepositorio.InsertOrUpdateAsync(produtoPalavraChave);
                    }
                    else
                    {
                        await _produtoPalavraChaveRepositorio.UpdateAsync(produtoPalavraChave);
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
        public async Task Excluir(CriarOuEditarProdutoPalavraChave input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoPalavraChaveRepositorio.DeleteAsync(input.Id);

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

        public async Task<PagedResultDto<ProdutoPalavraChaveDto>> Listar(ListarProdutosPalavrasChaveInput input)
        {
            var contarProdutosPalavrasChave = 0;
            List<ProdutoPalavraChave> produtosPalavrasChave;
            List<ProdutoPalavraChaveDto> produtosPalavrasChaveDtos = new List<ProdutoPalavraChaveDto>();
            try
            {
                var query = _produtoPalavraChaveRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Palavra.Contains(input.Filtro) ||
                        m.Observacao.ToUpper().Contains(input.Filtro.ToUpper())
                    //m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosPalavrasChave = await query
                    .CountAsync();

                produtosPalavrasChave = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosPalavrasChaveDtos = produtosPalavrasChave
                    .MapTo<List<ProdutoPalavraChaveDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoPalavraChaveDto>(
                contarProdutosPalavrasChave,
                produtosPalavrasChaveDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosPalavrasChaveInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosPalavrasChaveDtos = query.Items;

                return _listarProdutoPalavraChaveExcelExporter.ExportToFile(produtosPalavrasChaveDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoPalavraChaveDto> Obter(long id)
        {
            try
            {
                var result = await _produtoPalavraChaveRepositorio.GetAsync(id);
                var produtoPalavraChave = result.MapTo<ProdutoPalavraChaveDto>();
                return produtoPalavraChave;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<ProdutoPalavraChaveDto>> ListarTodos()
        {
            List<ProdutoPalavraChaveDto> produtosPalavraChavesDtos = new List<ProdutoPalavraChaveDto>();
            try
            {
                var query = await _produtoPalavraChaveRepositorio
                    .GetAllListAsync();

                var produtosPalavraChavesDto = query.MapTo<List<ProdutoPalavraChaveDto>>();

                return new ListResultDto<ProdutoPalavraChaveDto> { Items = produtosPalavraChavesDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
