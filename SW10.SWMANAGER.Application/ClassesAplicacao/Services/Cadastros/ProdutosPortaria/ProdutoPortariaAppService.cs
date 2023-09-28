using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPortaria;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria
{
    public class ProdutoPortariaAppService : SWMANAGERAppServiceBase, IProdutoPortariaAppService
    {
        private readonly IRepository<ProdutoPortaria, long> _produtoPortariaRepositorio;
        private readonly IListarProdutoPortariaExcelExporter _listarProdutoPortariaExcelExporter;

        public ProdutoPortariaAppService(IRepository<ProdutoPortaria, long> produtoPortariaRepositorio,
            IListarProdutoPortariaExcelExporter listarProdutoPortariaExcelExporter)
        {
            _produtoPortariaRepositorio = produtoPortariaRepositorio;
            _listarProdutoPortariaExcelExporter = listarProdutoPortariaExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Edit)]

        public async Task CriarOuEditar(CriarOuEditarProdutoPortaria input)
        {
            try
            {
                var produtoPortaria = input.MapTo<ProdutoPortaria>();
                if (input.Id.Equals(0))
                {
                    await _produtoPortariaRepositorio.InsertOrUpdateAsync(produtoPortaria);
                }
                else
                {
                    await _produtoPortariaRepositorio.UpdateAsync(produtoPortaria);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarProdutoPortaria input)
        {
            try
            {
                await _produtoPortariaRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProdutoPortariaDto>> Listar(ListarProdutosPortariaInput input)
        {
            var contarProdutosPortaria = 0;
            List<ProdutoPortaria> produtosPortaria;
            List<ProdutoPortariaDto> produtosPortariaDtos = new List<ProdutoPortariaDto>();
            try
            {
                var query = _produtoPortariaRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    //m.Palavra.Contains(input.Filtro) ||
                    //m.Observacao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosPortaria = await query
                    .CountAsync();

                produtosPortaria = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosPortariaDtos = produtosPortaria
                    .MapTo<List<ProdutoPortariaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoPortariaDto>(
                contarProdutosPortaria,
                produtosPortariaDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosPortariaInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosPortariaDtos = query.Items;

                return _listarProdutoPortariaExcelExporter.ExportToFile(produtosPortariaDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarProdutoPortaria> Obter(long id)
        {
            try
            {
                var result = await _produtoPortariaRepositorio.GetAsync(id);
                var produtoPortaria = result.MapTo<CriarOuEditarProdutoPortaria>();
                return produtoPortaria;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ProdutoPortariaDto>> ListarTodos()
        {
            List<ProdutoPortariaDto> produtosPortariasDtos = new List<ProdutoPortariaDto>();
            try
            {
                var query = await _produtoPortariaRepositorio
                    .GetAllListAsync();

                var produtosPortariasDto = query.MapTo<List<ProdutoPortariaDto>>();

                return new ListResultDto<ProdutoPortariaDto> { Items = produtosPortariasDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
