using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosSubstancia;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia
{
    public class ProdutoSubstanciaAppService : SWMANAGERAppServiceBase, IProdutoSubstanciaAppService
    {
        private readonly IRepository<ProdutoSubstancia, long> _produtoSubstanciaRepositorio;
        private readonly IListarProdutoSubstanciaExcelExporter _listarProdutoSubstanciaExcelExporter;

        public ProdutoSubstanciaAppService(IRepository<ProdutoSubstancia, long> produtoSubstanciaRepositorio,
            IListarProdutoSubstanciaExcelExporter listarProdutoSubstanciaExcelExporter)
        {
            _produtoSubstanciaRepositorio = produtoSubstanciaRepositorio;
            _listarProdutoSubstanciaExcelExporter = listarProdutoSubstanciaExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Edit)]

        public async Task CriarOuEditar(CriarOuEditarProdutoSubstancia input)
        {
            try
            {
                var produtoSubstancia = input.MapTo<ProdutoSubstancia>();
                if (input.Id.Equals(0))
                {
                    await _produtoSubstanciaRepositorio.InsertOrUpdateAsync(produtoSubstancia);
                }
                else
                {
                    await _produtoSubstanciaRepositorio.UpdateAsync(produtoSubstancia);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarProdutoSubstancia input)
        {
            try
            {
                await _produtoSubstanciaRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProdutoSubstanciaDto>> Listar(ListarProdutosSubstanciaInput input)
        {
            var contarProdutosSubstancia = 0;
            List<ProdutoSubstancia> produtosSubstancia;
            List<ProdutoSubstanciaDto> produtosSubstanciaDtos = new List<ProdutoSubstanciaDto>();
            try
            {
                var query = _produtoSubstanciaRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    //m.Palavra.Contains(input.Filtro) ||
                    //m.Observacao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosSubstancia = await query
                    .CountAsync();

                produtosSubstancia = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosSubstanciaDtos = produtosSubstancia
                    .MapTo<List<ProdutoSubstanciaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoSubstanciaDto>(
                contarProdutosSubstancia,
                produtosSubstanciaDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosSubstanciaInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosSubstanciaDtos = query.Items;

                return _listarProdutoSubstanciaExcelExporter.ExportToFile(produtosSubstanciaDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarProdutoSubstancia> Obter(long id)
        {
            try
            {
                var result = await _produtoSubstanciaRepositorio.GetAsync(id);
                var produtoSubstancia = result.MapTo<CriarOuEditarProdutoSubstancia>();
                return produtoSubstancia;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
