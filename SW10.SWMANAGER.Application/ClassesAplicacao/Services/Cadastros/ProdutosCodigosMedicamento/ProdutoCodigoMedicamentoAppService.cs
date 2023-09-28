using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosCodigosMedicamento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento
{
    public class ProdutoCodigoMedicamentoAppService : SWMANAGERAppServiceBase, IProdutoCodigoMedicamentoAppService
    {
        private readonly IRepository<ProdutoCodigoMedicamento, long> _produtoCodigoMedicamentoRepositorio;
        private readonly IListarProdutoCodigoMedicamentoExcelExporter _listarProdutoCodigoMedicamentoExcelExporter;


        public ProdutoCodigoMedicamentoAppService(IRepository<ProdutoCodigoMedicamento, long> produtoCodigoMedicamentoRepositorio,
            IListarProdutoCodigoMedicamentoExcelExporter listarProdutoCodigoMedicamentoExcelExporter)
        {
            _produtoCodigoMedicamentoRepositorio = produtoCodigoMedicamentoRepositorio;
            _listarProdutoCodigoMedicamentoExcelExporter = listarProdutoCodigoMedicamentoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Edit)]
        public async Task CriarOuEditar(ProdutoCodigoMedicamentoDto input)
        {
            try
            {
                var produtoCodigoMedicamento = input.MapTo<ProdutoCodigoMedicamento>();
                if (input.Id.Equals(0))
                {
                    await _produtoCodigoMedicamentoRepositorio.InsertOrUpdateAsync(produtoCodigoMedicamento);
                }
                else
                {
                    await _produtoCodigoMedicamentoRepositorio.UpdateAsync(produtoCodigoMedicamento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ProdutoCodigoMedicamentoDto input)
        {
            try
            {
                await _produtoCodigoMedicamentoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<ProdutoCodigoMedicamentoDto>> ListarTodos()
        {
            try
            {
                List<ProdutoCodigoMedicamento> listCore;
                List<ProdutoCodigoMedicamentoDto> listDtos = new List<ProdutoCodigoMedicamentoDto>();

                listCore = await _produtoCodigoMedicamentoRepositorio
                    .GetAll()
                    .ToListAsync();

                listDtos = listCore
                    .MapTo<List<ProdutoCodigoMedicamentoDto>>();

                return new ListResultDto<ProdutoCodigoMedicamentoDto> { Items = listDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ProdutoCodigoMedicamentoDto>> Listar(ListarProdutosCodigosMedicamentoInput input)
        {
            var contarProdutosCodigosMedicamento = 0;
            List<ProdutoCodigoMedicamento> produtosCodigosMedicamento;
            List<ProdutoCodigoMedicamentoDto> produtosCodigosMedicamentoDtos = new List<ProdutoCodigoMedicamentoDto>();
            try
            {
                var query = _produtoCodigoMedicamentoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarProdutosCodigosMedicamento = await query
                    .CountAsync();

                produtosCodigosMedicamento = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                produtosCodigosMedicamentoDtos = produtosCodigosMedicamento
                    .MapTo<List<ProdutoCodigoMedicamentoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoCodigoMedicamentoDto>(
                contarProdutosCodigosMedicamento,
                produtosCodigosMedicamentoDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosCodigosMedicamentoInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosCodigosMedicamentoDtos = query.Items;

                return _listarProdutoCodigoMedicamentoExcelExporter.ExportToFile(produtosCodigosMedicamentoDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoCodigoMedicamentoDto> Obter(long id)
        {
            try
            {
                var result = await _produtoCodigoMedicamentoRepositorio.GetAsync(id);
                var produtoCodigoMedicamento = result.MapTo<ProdutoCodigoMedicamentoDto>();
                return produtoCodigoMedicamento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
