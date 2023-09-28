using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade
{
    using Castle.Core.Internal;

    public class ProdutoUnidadeAppService : SWMANAGERAppServiceBase, IProdutoUnidadeAppService
    {
        private readonly IRepository<ProdutoUnidade, long> _produtoUnidadeRepositorio;
        private readonly IListarProdutoUnidadeExcelExporter _listarProdutoUnidadeExcelExporter;
        private readonly IRepository<Unidade, long> _unidadeRepositorio;

        public ProdutoUnidadeAppService(IRepository<ProdutoUnidade, long> produtoUnidadeRepositorio,
            IListarProdutoUnidadeExcelExporter listarProdutoUnidadeExcelExporter,
            IRepository<Unidade, long> unidadeRepositorio)
        {
            _produtoUnidadeRepositorio = produtoUnidadeRepositorio;
            _listarProdutoUnidadeExcelExporter = listarProdutoUnidadeExcelExporter;
            _unidadeRepositorio = unidadeRepositorio;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Edit)]
        public async Task CriarOuEditar(CriarOuEditarProdutoUnidade input)
        {
            try
            {
                var produtoUnidade = input.MapTo<ProdutoUnidade>();
                if (input.Id.Equals(0))
                {
                    await _produtoUnidadeRepositorio.InsertOrUpdateAsync(produtoUnidade);
                }
                else
                {
                    //var _produtoUnidade = await _produtoUnidadeRepositorio.GetAsync(input.Id);
                    //if (_produtoUnidade != null)
                    //{
                    //    _produtoUnidade.IsAtivo = input.IsAtivo;
                    //    _produtoUnidade.IsPrescricao = input.IsPrescricao;
                    //    _produtoUnidade.ProdutoId = input.ProdutoId;
                    //    _produtoUnidade.UnidadeId = input.UnidadeId;
                    //    _produtoUnidade.UnidadeTipoId = input.UnidadeTipoId;

                    await _produtoUnidadeRepositorio.UpdateAsync(produtoUnidade);
                    // }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarProdutoUnidade input)
        {
            try
            {
                await _produtoUnidadeRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProdutoUnidadeDto>> Listar(ListarProdutosUnidadeInput input)
        {
            var contarProdutosUnidade = 0;
            List<ProdutoUnidadeDto> produtosUnidadeDtos = new List<ProdutoUnidadeDto>();
            try
            {
                var query = _produtoUnidadeRepositorio
                    .GetAll();
                //.WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                //m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                //);
                ///TODO: NOVO PRODUTO
                //var query = new List<ProdutoUnidade>();

                contarProdutosUnidade = await query
                    .CountAsync();

                //produtosUnidade = await query
                //    .AsNoTracking()
                //    .OrderBy(input.Sorting)
                //    .PageBy(input)
                //    .ToListAsync();

                contarProdutosUnidade = query
    .Count();

                //produtosUnidade = query
                //    .OrderBy(input.Sorting)
                //    .ToListAsync();

                //produtosUnidadeDtos = produtosUnidade
                //    .MapTo<List<ProdutoUnidadeDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoUnidadeDto>(
                contarProdutosUnidade,
                produtosUnidadeDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosUnidadeInput input)
        {
            try
            {
                var query = await Listar(input);

                var produtosUnidadeDtos = query.Items;

                return _listarProdutoUnidadeExcelExporter.ExportToFile(produtosUnidadeDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ProdutoUnidadeDto> Obter(long id)
        {
            try
            {
                var result = await _produtoUnidadeRepositorio.GetAll()
                                                             .Include(i=> i.Unidade)
                                                             .Include(i=> i.Tipo)
                                                             .Where(w=> w.Id ==id)
                                                             .FirstOrDefaultAsync();
                var produtoUnidade = result.MapTo<ProdutoUnidadeDto>();
                return produtoUnidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }



        public async Task<IResultDropdownList<long>> ListarUnidadePorProdutoDropdown(DropdownInput dropdownInput)
        {
            try
            {
                long produtoId;
                //long.TryParse(dropdownInput.filtro, out produtoId);

                if (dropdownInput.filtros.IsNullOrEmpty())
                {
                    return new ResultDropdownList();
                }

                long.TryParse(dropdownInput.filtros[0], out produtoId);

                var result = await ListarDropdownLambda(dropdownInput
                                                         , _produtoUnidadeRepositorio
                                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                        || m.Codigo.ToString().Contains(dropdownInput.search))
                                                         && (m.ProdutoId == produtoId)
                                                        , p => new DropdownItems { id = p.UnidadeId, text = string.Concat(p.Unidade.Sigla.ToString(), " - ", p.Unidade.Descricao) }
                                                        , o => o.Descricao
                                                        );
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<IResultDropdownList<long>> ListarUnidadePorProdutoPrescricaoDropdown(DropdownInput dropdownInput)
        {
            try
            {
                long produtoId;
                //long.TryParse(dropdownInput.filtro, out produtoId);

                if (dropdownInput.filtros.IsNullOrEmpty())
                {
                    return new ResultDropdownList();
                }

                long.TryParse(dropdownInput.filtros[0], out produtoId);

                var result = await ListarDropdownLambda(dropdownInput
                                                         , _produtoUnidadeRepositorio
                                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                        || m.Codigo.ToString().Contains(dropdownInput.search))
                                                         && (m.ProdutoId == produtoId && m.IsPrescricao)


                                                        , p => new DropdownItems { id = p.UnidadeId, text = string.Concat(p.Unidade.Sigla.ToString(), " - ", p.Unidade.Descricao) }
                                                        , o => o.Descricao
                                                        );
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        /// <summary>
        /// Retorna lista paginada de Unidades do tipo 'Compras' vinculados a um Produto para carga em select2
        /// </summary>
        /// <param name="dropdownInput"></param>
        /// <returns></returns>
        public async Task<IResultDropdownList<long>> ListarUnidadeComprasPorProdutoDropdown(DropdownInput dropdownInput)
        {
            long produtoId;
            long.TryParse(dropdownInput.filtros[0], out produtoId);

            return await ListarDropdownLambda(dropdownInput
                                              , _produtoUnidadeRepositorio
                                              , m => (string.IsNullOrEmpty(dropdownInput.search)
                                                || m.Descricao.Contains(dropdownInput.search)
                                                || m.Codigo.ToString().Contains(dropdownInput.search))
                                              && (m.ProdutoId == produtoId)
                                              && (m.UnidadeTipoId == 3) // UnidadeTipo 3 = Compras

                                             , p => new DropdownItems<long> { id = p.Unidade.Id, text = string.Concat(p.Unidade.Sigla.ToString(), " - ", p.Unidade.Descricao) }
                                             , o => o.Descricao
                                             );
        }

        public async Task<IResultDropdownList<long>> ListarUnidadePorReferenciaProdutoDropdown(DropdownInput dropdownInput)
        {
            long produtoId;

            var filtroProdutoId = dropdownInput.filtros[0];

            long.TryParse(filtroProdutoId, out produtoId);


            var query = _produtoUnidadeRepositorio.GetAll()
                                                  .Where(w => w.ProdutoId == produtoId
                                                           && w.UnidadeTipoId == 1);




            return await ListarDropdownLambda(dropdownInput
                                                     , _unidadeRepositorio
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                    || m.Codigo.ToString().Contains(dropdownInput.search))

                                                    && (query.Any(a => a.UnidadeId == m.UnidadeReferenciaId || a.UnidadeId == m.Id))


                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Sigla.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );

        }

        public async Task<IResultDropdownList<long>> ListarUnidadeConsumoProdutoDropdown(DropdownInput dropdownInput)
        {


            if(!dropdownInput.id.IsNullOrEmpty())
            {
                long id;
                long.TryParse(dropdownInput.id, out id);
                dropdownInput.page = "1";
                dropdownInput.totalPorPagina = "10";
                return await ListarDropdownLambda(dropdownInput, _unidadeRepositorio,
                                                        m => m.Id == id
                                                        , p => new DropdownItems { id = p.Id, text = string.Concat(p.Sigla.ToString(), " - ", p.Descricao) }
                                                        , o => o.Descricao
                                                        );
            }

            long produtoId;
            if (dropdownInput.filtros.IsNullOrEmpty())
            {
                return null;
            }

            var filtroProdutoId = dropdownInput.filtros[0];

            long.TryParse(filtroProdutoId, out produtoId);


            var query = _produtoUnidadeRepositorio.GetAll().Where(w => w.ProdutoId == produtoId && w.IsPrescricao).AsNoTracking();

            return await ListarDropdownLambda(dropdownInput, _unidadeRepositorio, 
                                                    m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                    || m.Codigo.ToString().Contains(dropdownInput.search))
                                                    && (query.Any(a => a.UnidadeId == m.Id))
                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Sigla.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );

        }
    }
}
