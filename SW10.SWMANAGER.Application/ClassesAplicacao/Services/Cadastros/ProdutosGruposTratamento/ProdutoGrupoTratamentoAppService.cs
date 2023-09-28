using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosGruposTratamento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento
{
    public class ProdutoGrupoTratamentoAppService : SWMANAGERAppServiceBase, IProdutoGrupoTratamentoAppService
    {
        private readonly IRepository<ProdutoGrupoTratamento, long> _produtoGrupoTratamentoRepositorio;
        private readonly IListarProdutoGrupoTratamentoExcelExporter _listarProdutoGrupoTratamentoExcelExporter;


        public ProdutoGrupoTratamentoAppService(IRepository<ProdutoGrupoTratamento, long> produtoGrupoTratamentoRepositorio,
            IListarProdutoGrupoTratamentoExcelExporter listarProdutoGrupoTratamentoExcelExporter)
        {
            _produtoGrupoTratamentoRepositorio = produtoGrupoTratamentoRepositorio;
            _listarProdutoGrupoTratamentoExcelExporter = listarProdutoGrupoTratamentoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Edit)]
        public async Task CriarOuEditar(CriarOuEditarProdutoGrupoTratamento input)
        {
            try
            {
                var produtoGrupoTratamento = input.MapTo<ProdutoGrupoTratamento>();
                if (input.Id.Equals(0))
                {
                    await _produtoGrupoTratamentoRepositorio.InsertOrUpdateAsync(produtoGrupoTratamento);
                }
                else
                {
                    await _produtoGrupoTratamentoRepositorio.UpdateAsync(produtoGrupoTratamento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarProdutoGrupoTratamento input)
        {
            try
            {
                await _produtoGrupoTratamentoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProdutoGrupoTratamentoDto>> Listar(ListarProdutosGruposTratamentoInput input)
        {
            var contarProdutosGruposTratamento = 0;
            List<ProdutoGrupoTratamento> produtosGruposTratamento;
            List<ProdutoGrupoTratamentoDto> produtosGruposTratamentoDtos = new List<ProdutoGrupoTratamentoDto>();
            try
            {
                var query = _produtoGrupoTratamentoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosGruposTratamento = await query
                    .CountAsync();

                produtosGruposTratamento = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosGruposTratamentoDtos = produtosGruposTratamento
                    .MapTo<List<ProdutoGrupoTratamentoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoGrupoTratamentoDto>(
                contarProdutosGruposTratamento,
                produtosGruposTratamentoDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosGruposTratamentoInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosGruposTratamentoDtos = query.Items;

                return _listarProdutoGrupoTratamentoExcelExporter.ExportToFile(produtosGruposTratamentoDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarProdutoGrupoTratamento> Obter(long id)
        {
            try
            {
                var result = await _produtoGrupoTratamentoRepositorio.GetAsync(id);
                var produtoGrupoTratamento = result.MapTo<CriarOuEditarProdutoGrupoTratamento>();
                return produtoGrupoTratamento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
