using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLocalizacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao
{
    public class ProdutoLocalizacaoAppService : SWMANAGERAppServiceBase, IProdutoLocalizacaoAppService
    {
        private readonly IRepository<ProdutoLocalizacao, long> _produtoLocalizacaoRepositorio;
        private readonly IListarProdutoLocalizacaoExcelExporter _listarProdutoLocalizacaoExcelExporter;


        public ProdutoLocalizacaoAppService(IRepository<ProdutoLocalizacao, long> produtoLocalizacaoRepositorio,
            IListarProdutoLocalizacaoExcelExporter listarProdutoLocalizacaoExcelExporter)
        {
            _produtoLocalizacaoRepositorio = produtoLocalizacaoRepositorio;
            _listarProdutoLocalizacaoExcelExporter = listarProdutoLocalizacaoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Edit)]
        public async Task CriarOuEditar(ProdutoLocalizacaoDto input)
        {
            try
            {
                var produtoLocalizacao = input.MapTo<ProdutoLocalizacao>();
                if (input.Id.Equals(0))
                {
                    await _produtoLocalizacaoRepositorio.InsertOrUpdateAsync(produtoLocalizacao);
                }
                else
                {
                    await _produtoLocalizacaoRepositorio.UpdateAsync(produtoLocalizacao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ProdutoLocalizacaoDto input)
        {
            try
            {
                await _produtoLocalizacaoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProdutoLocalizacaoDto>> Listar(ListarProdutosLocalizacaoInput input)
        {
            var contarProdutosLocalizacao = 0;
            List<ProdutoLocalizacao> produtosLocalizacao;
            List<ProdutoLocalizacaoDto> produtosLocalizacaoDtos = new List<ProdutoLocalizacaoDto>();
            try
            {
                var query = _produtoLocalizacaoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        //m.CodLaboratorio.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosLocalizacao = await query
                    .CountAsync();

                produtosLocalizacao = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosLocalizacaoDtos = produtosLocalizacao
                    .MapTo<List<ProdutoLocalizacaoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoLocalizacaoDto>(
                contarProdutosLocalizacao,
                produtosLocalizacaoDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosLocalizacaoInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosLocalizacaoDtos = query.Items;

                return _listarProdutoLocalizacaoExcelExporter.ExportToFile(produtosLocalizacaoDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoLocalizacaoDto> Obter(long id)
        {
            try
            {
                var result = await _produtoLocalizacaoRepositorio.GetAsync(id);
                var produtoLocalizacao = result.MapTo<ProdutoLocalizacaoDto>();
                return produtoLocalizacao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<ProdutoLocalizacaoDto>> ListarTodos()
        {
            try
            {
                List<ProdutoLocalizacaoDto> objListaDto = new List<ProdutoLocalizacaoDto>();

                var query = await _produtoLocalizacaoRepositorio
                    .GetAllListAsync();

                objListaDto = query.MapTo<List<ProdutoLocalizacaoDto>>();

                return new ListResultDto<ProdutoLocalizacaoDto> { Items = objListaDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
