using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade
{
    public class ProdutoTipoUnidadeAppService : SWMANAGERAppServiceBase, IProdutoTipoUnidadeAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly IRepository<ProdutoTipoUnidade, long> _produtoTipoUnidadeRepositorio;
        private readonly IRepository<UnidadeTipo, long> _produtoTipoUnidadeRepositorio;
        private readonly IListarProdutoTipoUnidadeExcelExporter _listarProdutoTipoUnidadeExcelExporter;

        //public ProdutoTipoUnidadeAppService(IRepository<ProdutoTipoUnidade, long> produtoTipoUnidadeRepositorio,
        public ProdutoTipoUnidadeAppService(IRepository<UnidadeTipo, long> produtoTipoUnidadeRepositorio,
            IUnitOfWorkManager unitOfWorkManager,
            IListarProdutoTipoUnidadeExcelExporter listarProdutoTipoUnidadeExcelExporter)
        {
            _produtoTipoUnidadeRepositorio = produtoTipoUnidadeRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
            _listarProdutoTipoUnidadeExcelExporter = listarProdutoTipoUnidadeExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Edit)]
        [UnitOfWork]
        public async Task CriarOuEditar(ProdutoTipoUnidadeDto input)
        {
            try
            {
                //var produtoTipoUnidade = input.MapTo<ProdutoTipoUnidade>();
                var produtoTipoUnidade = input.MapTo<UnidadeTipo>();
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.Id.Equals(0))
                    {
                        await _produtoTipoUnidadeRepositorio.InsertOrUpdateAsync(produtoTipoUnidade);
                    }
                    else
                    {
                        await _produtoTipoUnidadeRepositorio.UpdateAsync(produtoTipoUnidade);
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
        public async Task Excluir(ProdutoTipoUnidadeDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _produtoTipoUnidadeRepositorio.DeleteAsync(input.Id);

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

        public async Task<PagedResultDto<ProdutoTipoUnidadeDto>> Listar(ListarProdutosTiposUnidadeInput input)
        {
            var contarProdutosTiposUnidade = 0;
            //List<ProdutoTipoUnidade> produtosTiposUnidade;
            List<UnidadeTipo> produtosTiposUnidade;
            List<ProdutoTipoUnidadeDto> produtosTiposUnidadeDtos = new List<ProdutoTipoUnidadeDto>();
            try
            {
                var query = _produtoTipoUnidadeRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosTiposUnidade = await query
                    .CountAsync();

                produtosTiposUnidade = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosTiposUnidadeDtos = produtosTiposUnidade
                    .MapTo<List<ProdutoTipoUnidadeDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoTipoUnidadeDto>(
                contarProdutosTiposUnidade,
                produtosTiposUnidadeDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosTiposUnidadeInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosTiposUnidadeDtos = query.Items;

                return _listarProdutoTipoUnidadeExcelExporter.ExportToFile(produtosTiposUnidadeDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoTipoUnidadeDto> Obter(long id)
        {
            try
            {
                var result = await _produtoTipoUnidadeRepositorio.GetAsync(id);
                var produtoTipoUnidade = result.MapTo<ProdutoTipoUnidadeDto>();
                return produtoTipoUnidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
