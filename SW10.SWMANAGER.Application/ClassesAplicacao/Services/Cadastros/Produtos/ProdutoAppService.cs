#region Usings
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using Dapper;
using MoreLinq;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.EntityFramework;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    /// <summary>
    /// Serviço de Produtos
    /// Classe que implementa os Serviços para operação com Produtos
    /// </summary>
    public class ProdutoAppService : SWMANAGERAppServiceBase, IProdutoAppService
    {
        #region ↓ Metodos

        #region → Basico - CRUD
        /// <summary>
        /// Cria ou Edita um produto, considerando se o valor do atributo ID possui valor ou nao
        /// </summary>
        /// <param name="input">Dto de Produto</param>
        /// <returns>Sem retorno</returns>
        [UnitOfWork]
        public async Task CriarOuEditar(ProdutoDto input)
        {
            try
            {
                input.Codigo = ObterProximoNumero(input).ToString();
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    var produto = input.MapTo<Produto>();
                    if (input.Id.Equals(0))
                    {
                        await _produtoRepositorio.Object.InsertAsync(produto);
                    }
                    else
                    {
                        var produtoEntity = _produtoRepositorio.Object.GetAll()
                                                               .Where(w => w.Id == input.Id)
                                                               .FirstOrDefault();

                        if (produtoEntity != null)
                        {
                            produtoEntity.Codigo = input.Codigo;
                            produtoEntity.CodigoBarra = input.CodigoBarra;
                            produtoEntity.CodigoTISS = input.CodigoTISS;
                            produtoEntity.ContaAdministrativaId = input.ContaAdministrativaId;
                            produtoEntity.DCBId = input.DCBId;
                            produtoEntity.Descricao = input.Descricao;
                            produtoEntity.DescricaoResumida = input.DescricaoResumida;
                            produtoEntity.EstoqueLocalizacaoId = input.EstoqueLocalizacaoId;
                            produtoEntity.EtiquetaId = input.EtiquetaId;
                            produtoEntity.FaturamentoItemId = input.FaturamentoItemId;
                            produtoEntity.GeneroId = input.GeneroId;
                            produtoEntity.GrupoClasseId = input.GrupoClasseId;
                            produtoEntity.GrupoId = input.GrupoId;
                            produtoEntity.GrupoSubClasseId = input.GrupoSubClasseId;
                            produtoEntity.IsAtivo = input.IsAtivo;
                            produtoEntity.IsBloqueioCompra = input.IsBloqueioCompra;
                            produtoEntity.IsConsignado = input.IsConsignado;
                            produtoEntity.IsCurvaABC = input.IsCurvaABC;
                            produtoEntity.IsFaturamentoItem = input.IsFaturamentoItem;
                            produtoEntity.IsKit = input.IsKit;
                            produtoEntity.IsLiberadoMovimentacao = input.IsLiberadoMovimentacao;
                            produtoEntity.IsLote = input.IsLote;
                            produtoEntity.IsMedicamento = input.IsMedicamento;
                            produtoEntity.IsMedicamentoControlado = input.IsMedicamentoControlado;
                            produtoEntity.IsOPME = input.IsOPME;
                            produtoEntity.IsPadronizado = input.IsPadronizado;
                            produtoEntity.IsPrescricaoItem = input.IsPrescricaoItem;
                            produtoEntity.IsPrincipal = input.IsPrincipal;
                            produtoEntity.IsSerie = input.IsSerie;
                            produtoEntity.IsValidade = input.IsValidade;
                            produtoEntity.ProdutoPrincipalId = input.ProdutoPrincipalId;
                            produtoEntity.ContaAdministrativaId = input.ContaAdministrativaId;
                            produtoEntity.IsItalico = input.IsItalico;
                            produtoEntity.IsNegrito = input.IsNegrito;

                            await _produtoRepositorio.Object.UpdateAsync(produtoEntity);
                        }

                    }

                    if (input.UnidadeReferencialId != null)
                    {
                        using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                        {

                            ProdutoUnidade produtoUnidade = _produtoUnidadeRepositorio.Object.GetAll().Where(w => w.ProdutoId == produto.Id
                                                                      && w.UnidadeTipoId == 1).FirstOrDefault();

                            if (produtoUnidade != null)
                            {
                                produtoUnidade.ProdutoId = produto.Id;
                                produtoUnidade.UnidadeId = (long)input.UnidadeReferencialId;
                                produtoUnidade.UnidadeTipoId = 1;
                                produtoUnidade.IsAtivo = true;
                                produtoUnidade.IsPrescricao = false;

                                await _produtoUnidadeRepositorio.Object.UpdateAsync(produtoUnidade);
                            }
                            else
                            {
                                produtoUnidade = new ProdutoUnidade();

                                produtoUnidade.ProdutoId = produto.Id;
                                produtoUnidade.UnidadeId = (long)input.UnidadeReferencialId;
                                produtoUnidade.UnidadeTipoId = 1;
                                produtoUnidade.IsAtivo = true;
                                produtoUnidade.IsPrescricao = false;

                                await _produtoUnidadeRepositorio.Object.InsertAsync(produtoUnidade);

                            }

                            if (input.UnidadeGerencialId != null)
                            {
                                ProdutoUnidade produtoUnidadeGerencial = _produtoUnidadeRepositorio.Object.GetAll().Where(w => w.ProdutoId == produto.Id
                                                                     && w.UnidadeTipoId == 2).FirstOrDefault();

                                if (produtoUnidadeGerencial != null)
                                {
                                    produtoUnidadeGerencial.ProdutoId = produto.Id;
                                    produtoUnidadeGerencial.UnidadeId = (long)input.UnidadeGerencialId;
                                    produtoUnidadeGerencial.UnidadeTipoId = 2;
                                    produtoUnidadeGerencial.IsAtivo = true;
                                    produtoUnidadeGerencial.IsPrescricao = false;

                                    await _produtoUnidadeRepositorio.Object.UpdateAsync(produtoUnidadeGerencial);
                                }
                                else
                                {
                                    produtoUnidadeGerencial = new ProdutoUnidade();

                                    produtoUnidadeGerencial.ProdutoId = produto.Id;
                                    produtoUnidadeGerencial.UnidadeId = (long)input.UnidadeGerencialId;
                                    produtoUnidadeGerencial.UnidadeTipoId = 2;
                                    produtoUnidadeGerencial.IsAtivo = true;
                                    produtoUnidadeGerencial.IsPrescricao = false;

                                    await _produtoUnidadeRepositorio.Object.InsertAsync(produtoUnidadeGerencial);
                                }
                            }
                        }
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        /// <summary>
        /// Exclui um produto
        /// </summary>
        /// <param name="input">Dto de Produto</param>
        /// <returns>Sem retorno</returns>
        [UnitOfWork]
        public async Task Excluir(ProdutoDto input)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {

                    await _produtoRepositorio.Object.DeleteAsync(input.Id);

                    unitOfWork.Complete();
                    _unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }


        [UnitOfWork]
        public async Task<DefaultReturn<ProdutoDto>> Excluir(long produtoId)
        {

            var _retornoPadrao = new DefaultReturn<ProdutoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            if (await ExisteMovimentacaoDeEstoque(produtoId))
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "EST0014" });
            }
            else
            {
                try
                {
                    using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                    using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                    {
                        await _produtoRepositorio.Object.DeleteAsync(produtoId);

                        unitOfWork.Complete();
                        _unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroExcluir"), ex);
                }
            }

            return _retornoPadrao;
        }


        /// <summary>
        /// Retorna um Dto de Produto
        /// </summary>
        /// <param name="id">Id do produto desejado</param>
        /// <returns>Dto de Produto</returns>
        public async Task<ProdutoDto> Obter(long id)
        {
            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = _produtoRepositorio.Object
                        .GetAll()
                        .Include(m => m.DCB)
                        .Include(m => m.EstoqueLocalizacao)
                        .Include(m => m.Genero)
                        .Include(m => m.Grupo)
                        .Include(m => m.ProdutoPrincipal)
                        .Include(m => m.SubClasse)
                        .Include(m => m.ContaAdministrativa)
                        .Include(m => m.FaturamentoItem)
                        .Where(m => m.Id == id);

                    var result = await query.FirstOrDefaultAsync();
                    var produto = result.MapTo<ProdutoDto>();

                    produto.possuiMovimentacaoDeEstoque = await ExisteMovimentacaoDeEstoque(id);

                    produto.possuiRequisicaoDeCompraPendente = await ExisteRequisicaoDeCompraPendenteParaOProduto(id);

                    return produto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        #endregion

        #region → Basico - Listar

        /// <summary>
        /// Retorna uma lista paginada de Dtos Produtos preparada para popular um JTable
        /// </summary>
        /// <param name="input">ListarProdutosInput</param>
        public async Task<PagedResultDto<ProdutoDto>> Listar(ListarProdutosInput input)
        {
            var contarProdutos = 0;
            List<Produto> Produtos;
            List<ProdutoDto> ProdutosDtos = new List<ProdutoDto>();

            long grupoId;
            long grupoClasseId;
            long grupoSubClasseId;
            long DCBId;
            long FiltroPrincipal;
            long FiltroStatus;

            long.TryParse(input.GrupoId, out grupoId);
            long.TryParse(input.GrupoClasseId, out grupoClasseId);
            long.TryParse(input.GrupoSubClasseId, out grupoSubClasseId);
            long.TryParse(input.DCBId, out DCBId);
            long.TryParse(input.FiltroPrincipais, out FiltroPrincipal);
            long.TryParse(input.FiltroStatus, out FiltroStatus);

            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = _produtoRepositorio.Object
                        .GetAll()
                        .Include(m => m.DCB)
                        .Include(m => m.EstoqueLocalizacao)
                        .Include(m => m.Genero)
                        .Include(m => m.Grupo)
                        .Include(m => m.ProdutoPrincipal)
                        .Include(m => m.SubClasse)
                        .Include(m => m.DCB)
                        .Include(m => m.Classe)
                        .Where(m =>
                           (m.Id.ToString().Contains(input.Filtro) ||
                           m.Descricao.Contains(input.Filtro) ||
                           m.DescricaoResumida.Contains(input.Filtro) ||
                             m.DCB.Descricao.Contains(input.Filtro) ||
                               m.Codigo.Contains(input.Filtro) ||
                             m.Grupo.Descricao.Contains(input.Filtro) ||
                             m.Classe.Descricao.Contains(input.Filtro) ||
                             m.SubClasse.Descricao.Contains(input.Filtro)
                           )

                           && (grupoId == 0 || m.GrupoId == grupoId)
                           && (grupoClasseId == 0 || m.GrupoClasseId == grupoClasseId)
                           && (grupoSubClasseId == 0 || m.GrupoSubClasseId == grupoSubClasseId)
                            && (DCBId == 0 || m.DCBId == DCBId)
                        )

                        //Opcao para Listar todos os Produtos que são Principais e Produtos que nao sao produtos principais e também nao possuem um produto principal
                        .WhereIf(FiltroPrincipal == 1, m => m.IsPrincipal == true || m.ProdutoPrincipalId == null)

                        //Mostrar apenas os que possuem produto principal(que sao filhos)
                        .WhereIf(FiltroPrincipal == 2, m => m.ProdutoPrincipalId > 0)

                        .WhereIf(FiltroStatus == 1, m => m.IsAtivo)

                        .WhereIf(FiltroStatus == 2, m => m.IsAtivo);

                    contarProdutos = await query.CountAsync();

                    Produtos = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    ProdutosDtos = Produtos.MapTo<List<ProdutoDto>>();

                    return new PagedResultDto<ProdutoDto>(contarProdutos, ProdutosDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna uma lista de Dtos de produto
        /// </summary>
        public async Task<ListResultDto<ProdutoDto>> ListarTodos()
        {
            var contarProdutos = 0;
            List<Produto> Produtos;
            List<ProdutoDto> ProdutosDtos = new List<ProdutoDto>();
            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = _produtoRepositorio.Object
                        .GetAll()
                        .Include(m => m.DCB)
                        .Include(m => m.EstoqueLocalizacao)
                        .Include(m => m.Genero)
                        .Include(m => m.Grupo)
                        .Include(m => m.ProdutoPrincipal)
                        .Include(m => m.SubClasse);

                    contarProdutos = await query
                        .CountAsync();

                    Produtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    ProdutosDtos = Produtos
                        .MapTo<List<ProdutoDto>>();

                    return new ListResultDto<ProdutoDto> { Items = ProdutosDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        #endregion Basico - Listar

        #region → Obter
        /// <summary>
        /// Retorna o próximo codigo disponivel. Ps: Não é o campo Id
        /// </summary>
        public string ObterProximoNumero(ProdutoDto input)
        {

            if (Convert.ToInt64(input.Codigo.IsNullOrEmpty() ? "0" : input.Codigo) == 0)
            {
                ///TODO: NOVO PRODUTO
                //return _ultimoIdAppService.ObterProximoCodigo("SaidaProduto").Result;
                using (var _ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                {
                    return _ultimoIdAppService.Object.ObterProximoCodigo("Produto").Result;
                }
                //var query = _produtoRepositorio.GetAll().Count() > 0 ? _produtoRepositorio.GetAll().Max(m => Convert.ToInt64(m.Codigo)) : 0; //   _produtoRepositorio.GetAll().Max(m => m.Codigo);

            }
            else
            {
                return input.Codigo;
            }
        }

        /// <summary>
        /// Cria um novo produto e retorna um ProdutoDto com seu Id
        /// </summary>
        [UnitOfWork]
        public ProdutoDto CriarGetId(ProdutoDto input)
        {
            try
            {
                input.Codigo = ObterProximoNumero(input).ToString();
                ProdutoDto produtoDto;
                var produto = input.MapTo<Produto>();
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {

                    if (input.Id.Equals(0))
                    {
                        //Inclui o produto e retorna produtoDto com o Id
                        produtoDto = new ProdutoDto { Id = AsyncHelper.RunSync(() => _produtoRepositorio.Object.InsertAndGetIdAsync(produto)) };
                        //Inclui as unidades Referencial e Gerencial
                    }
                    else
                    {
                        produtoDto = AsyncHelper.RunSync(() => _produtoRepositorio.Object.UpdateAsync(produto)).MapTo<ProdutoDto>();
                    }


                    if (input.UnidadeReferencialId != null)
                    {
                        CriarOuEditarProdutoUnidade produtoUnidade = new CriarOuEditarProdutoUnidade();
                        produtoUnidade.ProdutoId = produtoDto.Id;
                        produtoUnidade.UnidadeId = (long)input.UnidadeReferencialId;
                        produtoUnidade.UnidadeTipoId = 1;
                        produtoUnidade.IsAtivo = true;
                        produtoUnidade.IsPrescricao = false;

                        using (var _produtoUnidadeAppService = IocManager.Instance.ResolveAsDisposable<IProdutoUnidadeAppService>())
                        {
                            _produtoUnidadeAppService.Object.CriarOuEditar(produtoUnidade);

                            if (input.UnidadeGerencialId != null)
                            {
                                CriarOuEditarProdutoUnidade produtoUnidadeGerencial = new CriarOuEditarProdutoUnidade();
                                produtoUnidadeGerencial.ProdutoId = produtoDto.Id;
                                produtoUnidadeGerencial.UnidadeId = (long)input.UnidadeGerencialId;
                                produtoUnidadeGerencial.UnidadeTipoId = 2;
                                produtoUnidadeGerencial.IsAtivo = true;
                                produtoUnidadeGerencial.IsPrescricao = false;

                                _produtoUnidadeAppService.Object.CriarOuEditar(produtoUnidadeGerencial);
                            }
                        }
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                produtoDto.Codigo = input.Codigo;
                return produtoDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<ListResultDto<UnidadeDto>> ObterUnidadePorProduto(long id)
        {
            List<ProdutoUnidadeDto> listDtos = new List<ProdutoUnidadeDto>();
            List<UnidadeDto> unidades = new List<UnidadeDto>();
            try
            {
                //var result = await _produtoUnidadeRepositorio
                //    .GetAll()
                //    .Where(w => w.ProdutoId == id)
                //    .FirstOrDefaultAsync();
                using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var result = await _produtoUnidadeRepositorio.Object
                        .GetAll()
                        .Include(m => m.Produto)
                        .Include(m => m.Tipo)
                        .Include(m => m.Unidade)
                        .Where(a => a.ProdutoId == id)
                        .ToListAsync();

                    listDtos = result.MapTo<List<ProdutoUnidadeDto>>();
                    unidades = listDtos.Select(s => s.Unidade).DistinctBy(m => m.Id).ToList();
                    return new ListResultDto<UnidadeDto> { Items = unidades };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<UnidadeDto>> ObterUnidadePorProduto(long id, bool listarRefGer = true)
        {
            //ist<ProdutoUnidadeDto> listDtos = new List<ProdutoUnidadeDto>();
            List<UnidadeDto> unidades = new List<UnidadeDto>();
            try
            {
                //var result = await _produtoUnidadeRepositorio
                //    .GetAll()
                //    .Where(w => w.ProdutoId == id)
                //    .FirstOrDefaultAsync();

                using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var listDtos = (await _produtoUnidadeRepositorio.Object
                        .GetAll()
                        // .Include(m => m.Produto)
                        //.Include(m => m.Tipo)
                        .Include(m => m.Unidade)
                        .Where(a => a.ProdutoId == id)
                        .WhereIf(listarRefGer == false, m => ((m.UnidadeTipoId != 1) && (m.UnidadeTipoId != 2)))
                        .ToListAsync())

                        .Select(s => new UnidadeDto
                        {
                            Codigo = s.Unidade.Codigo,
                            Descricao = s.Unidade.Descricao,
                            Id = s.Unidade.Id
                            //IsAtivo = s.IsAtivo,
                            //IsPrescricao = s.IsPrescricao,
                            //ProdutoId = s.ProdutoId,
                            //Produto = new ProdutoDto { Id = s.Produto.Id, Codigo = s.Produto.Codigo, Descricao = s.Produto.Descricao, DescricaoResumida = s.Produto.DescricaoResumida },
                            //Tipo = new UnidadeTipoDto { Id = s.Tipo.Id, Codigo = s.Tipo.Codigo, Descricao = s.Tipo.Descricao },
                            // Unidade = new UnidadeDto { Id = s.Unidade.Id, Codigo = s.Unidade.Codigo, Descricao = s.Unidade.Descricao },
                            //UnidadeId = s.UnidadeId,
                            //UnidadeTipoId = s.UnidadeTipoId
                        });


                    // listDtos = result.MapTo<List<ProdutoUnidadeDto>>();
                    unidades = listDtos.DistinctBy(m => m.Id).ToList();
                    return new ListResultDto<UnidadeDto> { Items = unidades };
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }

        /// <summary>
        /// Retorna a Unidade Referencia ou Gerencial do produto
        /// Referencia: idTipoUnidade = 1 | Gerencial: idTipoUnidade = 2
        /// </summary>
        public async Task<UnidadeDto> ObterUnidadePorTipo(long idProduto, long idTipoUnidade)
        {
            List<ProdutoUnidadeDto> listDtos = new List<ProdutoUnidadeDto>();
            List<UnidadeDto> unidades = new List<UnidadeDto>();
            try
            {
                using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var query = _produtoUnidadeRepositorio.Object
                        .GetAll()
                        .Include(m => m.Produto)
                        .Include(m => m.Tipo)
                        .Include(m => m.Unidade)
                        .Where(a => a.ProdutoId == idProduto && a.UnidadeTipoId == idTipoUnidade);

                    var result = await query.FirstOrDefaultAsync();

                    var produtoUnidade = ProdutoUnidadeDto.Mapear(result);
                    if (produtoUnidade != null)
                    {
                        var unidade = produtoUnidade.Unidade;
                        return unidade;
                    }
                    else
                    {
                        return new UnidadeDto();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna a Unidade Refekrencia do produto - UnidadeTipoId = 1
        /// </summary>
        public async Task<UnidadeDto> ObterUnidadeReferencial(long idProduto)
        {
            List<ProdutoUnidadeDto> listDtos = new List<ProdutoUnidadeDto>();
            List<UnidadeDto> unidades = new List<UnidadeDto>();
            try
            {
                using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var query = _produtoUnidadeRepositorio.Object
                        .GetAll()
                        .Include(u => u.Unidade)
                        .Include(m => m.Produto)
                        .Include(m => m.Tipo)
                        .Where(a => a.ProdutoId == idProduto && a.UnidadeTipoId == 1);

                    var result = await query.FirstOrDefaultAsync();

                    var produtoUnidade = ProdutoUnidadeDto.Mapear(result);
                    if (produtoUnidade != null)
                    {
                        var unidade = produtoUnidade.Unidade;
                        return unidade;
                    }
                    else
                    {
                        return new UnidadeDto();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna a Unidade Gerencial do produto (UnidadeTipoId = 2)
        /// </summary>
        public async Task<UnidadeDto> ObterUnidadeGerencial(long idProduto)
        {
            List<ProdutoUnidadeDto> listDtos = new List<ProdutoUnidadeDto>();
            List<UnidadeDto> unidades = new List<UnidadeDto>();
            try
            {
                using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var query = _produtoUnidadeRepositorio.Object
                        .GetAll()
                        //.Include(m => m.Produto)
                        // .Include(m => m.Tipo)
                        .Include(m => m.Unidade)
                        .Where(a => a.ProdutoId == idProduto && a.UnidadeTipoId == 2);

                    var result = await query.FirstOrDefaultAsync();

                    var produtoUnidade = ProdutoUnidadeDto.Mapear(result);

                    if (produtoUnidade != null)
                    {
                        var unidade = produtoUnidade.Unidade;
                        return unidade;
                    }
                    else
                    {
                        return new UnidadeDto();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion

        #region → Gets
        /// <summary>
        /// Indica se há movimentação de estoque para o produto
        /// </summary>
        /// <param name="id">Id do produto desejado</param>
        /// <returns>bool</returns>
        public async Task<bool> ExisteMovimentacaoDeEstoque(long id)
        {
            var contarProdutos = 0;

            try
            {
                using (var _estoquePreMovimentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {

                    var query = _estoquePreMovimentoItemRepositorio.Object.GetAll().Where(m => m.ProdutoId == id);

                    contarProdutos = await query.CountAsync();

                    bool tem;

                    tem = contarProdutos > 0;

                    return tem;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Indica se um produto possui Requisição de Compra
        /// </summary>
        /// <param name="id">Id do produto desejado</param>
        /// <returns>bool</returns>
        public async Task<bool> ExisteRequisicaoDeCompraPendenteParaOProduto(long id)
        {
            var contarProdutos = 0;

            try
            {
                using (var _compraRequisicaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<CompraRequisicao, long>>())
                using (var _compraRequisicaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<CompraRequisicaoItem, long>>())
                {
                    var query = from cmp in _compraRequisicaoRepositorio.Object.GetAll()
                                join cmpItem in _compraRequisicaoItemRepositorio.Object.GetAll() on cmp.Id equals cmpItem.RequisicaoId into joined
                                from cmpItem in joined.DefaultIfEmpty()
                                where (cmpItem.ProdutoId == id) && (cmp.Id == cmpItem.RequisicaoId) && (!cmp.IsRequisicaoAprovada)
                                select cmp.Id;

                    //.GetAll()
                    //.Where(m => m.ProdutoId == id);

                    contarProdutos = await query
                        .CountAsync();

                    bool existe;

                    existe = contarProdutos > 0;

                    return existe;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion

        #region → Listar

        /// <summary>
        /// Retorna uma lista de produto ativos e não bloqueados para compra
        /// </summary>
        /// <returns>Lista de Dtos de produto</returns>
        public async Task<ListResultDto<ProdutoDto>> ListarTodosParaMovimento()
        {
            var contarProdutos = 0;
            List<Produto> Produtos;
            List<ProdutoDto> ProdutosDtos = new List<ProdutoDto>();
            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {

                    var query = _produtoRepositorio.Object
                        .GetAll()
                        .Include(m => m.DCB)
                        .Include(m => m.EstoqueLocalizacao)
                        .Include(m => m.Genero)
                        .Include(m => m.Grupo)
                        .Include(m => m.ProdutoPrincipal)
                        .Include(m => m.SubClasse)
                        .Where(w => !w.IsPrincipal
                                          && !w.IsBloqueioCompra
                                          && w.IsAtivo);

                    contarProdutos = await query
                        .CountAsync();

                    Produtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    ProdutosDtos = Produtos
                        .MapTo<List<ProdutoDto>>();

                    return new ListResultDto<ProdutoDto> { Items = ProdutosDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<ListResultDto<ProdutoDto>> ListarProdutosExcetoId(long ProdutoExcetoId)
        {
            List<Produto> Produtos;
            List<ProdutoDto> ProdutosDtos = new List<ProdutoDto>();
            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {

                    var query = _produtoRepositorio.Object
                               .GetAll()
                        .Include(m => m.Classe)
                        .Include(m => m.DCB)
                        .Include(m => m.EstoqueLocalizacao)
                        .Include(m => m.Genero)
                        .Include(m => m.Grupo)
                        .Include(m => m.ProdutoPrincipal)
                        .Include(m => m.SubClasse)
                                .Where(m => m.Id != ProdutoExcetoId);

                    Produtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    ProdutosDtos = Produtos
                        .MapTo<List<ProdutoDto>>();

                    return new ListResultDto<ProdutoDto> { Items = ProdutosDtos };
                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna Lista com todos os produtos definidos como Principal(Mestre)
        /// </summary>
        /// <returns>ListResultDto<GenericoIdNome> de Produto</returns>
        public async Task<ListResultDto<GenericoIdNome>> ListarProdutosMestre()
        {
            //List<ProdutoDto> produtosDtos = new List<ProdutoDto>();
            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = await _produtoRepositorio.Object
                        .GetAll()
                        //.Include(m => m.DCB)
                        //.Include(m => m.EstoqueLocalizacao)
                        //.Include(m => m.Genero)
                        //.Include(m => m.Grupo)
                        //.Include(m => m.ProdutoPrincipal)
                        //.Include(m => m.SubClasse)
                        .Where(a => a.IsPrincipal == true)
                        .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                        .ToListAsync();

                    //var produtosDto = query.MapTo<List<ProdutoDto>>();

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        /// <summary>
        /// Retorna uma lista com Nome(descricao) e Id de produtos.
        /// </summary>
        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = await _produtoRepositorio.Object
                        .GetAll()
                        .Include(m => m.DCB)
                        .Include(m => m.EstoqueLocalizacao)
                        .Include(m => m.Genero)
                        .Include(m => m.Grupo)
                        .Include(m => m.ProdutoPrincipal)
                        .Include(m => m.SubClasse)
                        .WhereIf(!input.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(input) ||
                            m.DescricaoResumida.Contains(input)
                            )
                        .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                        .ToListAsync();

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// ListResultDto de DCB
        /// </summary>
        //public async Task<ListResultDto<GenericoIdNome>> ListarDCBs()
        //{
        //    try
        //    {

        //        using (var _dcbRepositorio = this.iocResolver.Resolve<IRepository<DCB, long>>())
        //        {

        //            var query = await _dcbRepositorio
        //                .GetAll()
        //                // .Where(w=>w.Id==1)
        //                .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
        //               .ToListAsync();

        //            return new ListResultDto<GenericoIdNome> { Items = query };
        //        }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public async Task<ListResultDto<ProdutoDto>> ListarProdutosMestresExcetoId(long ProdutoExcetoId)
        {
            List<Produto> Produtos;
            List<ProdutoDto> ProdutosDtos = new List<ProdutoDto>();
            try
            {
                using (var _produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = _produtoRepositorio.Object
                               .GetAll()
                        .Include(m => m.Classe)
                        .Include(m => m.DCB)
                        .Include(m => m.EstoqueLocalizacao)
                        .Include(m => m.Genero)
                        .Include(m => m.Grupo)
                        .Include(m => m.ProdutoPrincipal)
                        .Include(m => m.SubClasse)
                               .Where(m => m.Id != ProdutoExcetoId && m.IsPrincipal);

                    Produtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    ProdutosDtos = ProdutoDto.Mapear(Produtos);

                    return new ListResultDto<ProdutoDto> { Items = ProdutosDtos };
                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<PagedResultDto<ProdutoUnidadeDto>> ListarProdutoUnidade(long produtoId)
        {
            var contarProdutosUnidades = 0;
            List<ProdutoUnidade> ProdutosUnidades;
            List<ProdutoUnidadeDto> ProdutosUnidadesDtos = new List<ProdutoUnidadeDto>();
            try
            {
                using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var query = _produtoUnidadeRepositorio.Object
                        .GetAll()
                        .Include(m => m.Produto)
                        .Include(m => m.Tipo)
                        .Include(m => m.Unidade)
                        .Where(m => m.ProdutoId == produtoId);

                    contarProdutosUnidades = await query
                        .CountAsync();

                    ProdutosUnidades = await query
                        .AsNoTracking()
                        .ToListAsync();

                    ProdutosUnidadesDtos = ProdutosUnidades
                        .MapTo<List<ProdutoUnidadeDto>>();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ProdutoUnidadeDto>(
                contarProdutosUnidades,
                ProdutosUnidadesDtos
                );
        }


        public async Task<PagedResultDto<ProdutoFornecedorDto>> ListarProdutoFornecedores(ListarInput input)
        {
            try
            {
                var _estoqueImportacaoProdutoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueImportacaoProduto, long>>();
                var _pessoaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<SisPessoa, long>>();
                List<SisPessoa> fornecedores;
                List<ProdutoFornecedorDto> dto;
                var id = Convert.ToInt64(input.Id);

                var queryCNPJNota = _estoqueImportacaoProdutoRepositorio.Object
                    .GetAll()
                    .Where(x => x.ProdutoId == id)
                    .Select(x => x.CNPJNota).Distinct();

                var fornecedoresQuery = _pessoaRepositorio.Object.GetAll().Where(x => queryCNPJNota.Contains(x.Cnpj));

                var qtdRegistros = await fornecedoresQuery.CountAsync();

                fornecedores = await fornecedoresQuery.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync();
                dto = fornecedores.Select(x => new ProdutoFornecedorDto()
                {
                    CNPJNota = x.Cnpj,
                    NomeFantasia = x.NomeFantasia,
                    RazaoSocial = x.RazaoSocial,
                }).ToList();

                return new PagedResultDto<ProdutoFornecedorDto>(qtdRegistros, dto);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<DefaultReturn<ProdutoFornecedorDto>> ExcluirProdutoFornecedor(long produtoId, string cnpj)
        {
            try
            {
                var _retornoPadrao = new DefaultReturn<ProdutoFornecedorDto>();
                _retornoPadrao.Warnings = new List<ErroDto>();
                _retornoPadrao.Errors = new List<ErroDto>();

                var _estoqueImportacaoProdutoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueImportacaoProduto, long>>();

                await _estoqueImportacaoProdutoRepositorio.Object.DeleteAsync(x => x.ProdutoId == produtoId && x.CNPJNota == cnpj);

                return _retornoPadrao;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<PagedResultDto<ProdutoUnidadeDto>> ListarProdutosUnidadesPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoUnidade> Produtos;
            List<ProdutoUnidadeDto> ProdutosDtos = new List<ProdutoUnidadeDto>();
            try
            {
                using (var _produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var id = Convert.ToInt64(input.Filtro);
                    var query = _produtoUnidadeRepositorio.Object
                        .GetAll()
                        .Include(m => m.Produto)
                        .Include(m => m.Tipo)
                        .Include(m => m.Unidade)
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
                        .MapTo<List<ProdutoUnidadeDto>>();

                    return new PagedResultDto<ProdutoUnidadeDto>(
                        contarProdutos,
                        ProdutosDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<PagedResultDto<ProdutoEmpresaDto>> ListarProdutosEmpresasPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoEmpresa> Produtos;
            List<ProdutoEmpresaDto> ProdutosDtos = new List<ProdutoEmpresaDto>();
            try
            {
                using (var _produtoEmpresaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEmpresa, long>>())
                {
                    var id = Convert.ToInt64(input.Filtro);
                    var query = _produtoEmpresaRepositorio.Object
                        .GetAll()
                        .Include(m => m.Empresa)
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
                        .MapTo<List<ProdutoEmpresaDto>>();

                    return new PagedResultDto<ProdutoEmpresaDto>(
                        contarProdutos,
                        ProdutosDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<PagedResultDto<ProdutoEstoqueDto>> ListarProdutosEstoquesPorProduto(ListarInput input)
        {
            var contarProdutos = 0;
            List<ProdutoEstoque> Produtos;
            List<ProdutoEstoqueDto> ProdutosDtos = new List<ProdutoEstoqueDto>();
            try
            {
                using (var produtoEstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEstoque, long>>())
                {
                    var id = Convert.ToInt64(input.Filtro);
                    var query = produtoEstoqueRepositorio.Object
                        .GetAll()
                        .Include(m => m.Estoque)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.ProdutoId == id);

                    contarProdutos = await query
                        .CountAsync();

                    Produtos = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    ProdutosDtos = Produtos.MapTo<List<ProdutoEstoqueDto>>();

                    return new PagedResultDto<ProdutoEstoqueDto>(contarProdutos, ProdutosDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<PagedResultDto<ProdutoSaldoMinDto>> ListarProdutoSaldo(ListarInput input)
        {
            var contarProdutos = 0;
            // List<ProdutoSaldo> Produtos;
            List<ProdutoSaldoDto> ProdutosDtos = new List<ProdutoSaldoDto>();
            try
            {
                long id = 0;
                var result = long.TryParse(input.Filtro, out id);

                using (var estoqueAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueAppService>())
                using (var produtoSaldoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {
                    var query = produtoSaldoRepositorio.Object
                        .GetAll()
                        .Include(m => m.Estoque)
                        .WhereIf(id > 0, m => m.ProdutoId == id && m.Estoque.IsDeleted == false);

                    var queryList = query
                        .GroupBy(g => g.EstoqueId)
                        .Select(s => new ProdutoSaldoDto {
                            EstoqueId = s.Key,
                            QuantidadeAtual = s.Sum(soma => soma.QuantidadeAtual),
                            QuantidadeEntradaPendente = s.Sum(soma => soma.QuantidadeEntradaPendente),
                            QuantidadeSaidaPendente = s.Sum(soma => soma.QuantidadeSaidaPendente),
                            SaldoFuturo = s.Sum(soma => soma.QuantidadeAtual + soma.QuantidadeEntradaPendente - soma.QuantidadeSaidaPendente)
                        })
                        .ToList();

                    var listaProdutoSaldoDto = new List<ProdutoSaldoMinDto>();

                    UnidadeDto unidadeGerencial = await ObterUnidadeGerencial(id);

                    foreach (var item in queryList)
                    {
                        var estoqueDto = new EstoqueDto();
                        estoqueDto = await estoqueAppService.Object.Obter(item.EstoqueId);
                        listaProdutoSaldoDto.Add(new ProdutoSaldoMinDto
                        {
                            ProdutoId = id,
                            EstoqueId = item.EstoqueId,
                            Descricao = estoqueDto.Descricao,
                            QuantidadeAtual = item.QuantidadeAtual,
                            QuantidadeEntradaPendente = item.QuantidadeEntradaPendente,
                            QuantidadeSaidaPendente = item.QuantidadeSaidaPendente,
                            SaldoFuturo = item.SaldoFuturo,
                            QuantidadeGerencialAtual = item.QuantidadeAtual > 0 ? item.QuantidadeAtual / (unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1) : 0,
                            QuantidadeGerencialEntradaPendente = item.QuantidadeEntradaPendente > 0 ? item.QuantidadeEntradaPendente / (unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1) : 0,
                            QuantidadeGerencialSaidaPendente = item.QuantidadeSaidaPendente > 0 ? item.QuantidadeSaidaPendente / (unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1) : 0,
                            SaldoGerencialFuturo = item.SaldoFuturo > 0 ? item.SaldoFuturo / (unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1) : 0
                        });
                    }

                    contarProdutos = await query
                        .CountAsync();

                    //Produtos = await query
                    //var  Produtos = await queryList
                    var Produtos = listaProdutoSaldoDto
                          //.AsNoTracking()
                          //.OrderBy(input.Sorting)
                          //.PageBy(input)
                          //.ToListAsync();
                          .ToList();

                    //ProdutosDtos = Produtos
                    //    .MapTo<List<ProdutoSaldoDto>>();

                    return new PagedResultDto<ProdutoSaldoMinDto>(
                        contarProdutos,
                        Produtos
                        //ProdutosDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWRptSaldoProdutoDto>> ListarProdutoSaldoReport(long estoqueId, long grupoId)
        {
            try
            {
                using (var _vwSaldoProdutoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<VWRptSaldoProduto, long>>())
                {
                    var query = _vwSaldoProdutoRepositorio.Object
                        .GetAll()
                        .WhereIf(estoqueId > 0, m => m.EstoqueId == estoqueId)
                        .WhereIf(grupoId > 0, m => m.GrupoId == grupoId)
                        .OrderBy(o => o.Estoque)
                        .ThenBy(o => o.Grupo)
                        .ThenBy(o => o.Produto);
                    //var query = _produtoSaldoRepositorio
                    //    .GetAll()
                    //    .Include(m => m.Produto)
                    //    .Include(m => m.Produto.Grupo)
                    //    .Where(m => m.Produto.IsDeleted == false)
                    //    .WhereIf(grupoId > 0, m =>
                    //          m.Produto.GrupoId == grupoId
                    //    )
                    //    .WhereIf(classeId > 0, m =>
                    //          m.Produto.GrupoClasseId == classeId
                    //    )
                    //    .WhereIf(classeId > 0, m =>
                    //          m.Produto.GrupoSubClasseId == subClasseId
                    //    )
                    //    .OrderBy(o => o.Produto.GrupoId)
                    //    .ThenBy(o => o.ProdutoId);

                    //var query2 = query
                    //                .GroupBy(g => new { g.Produto.GrupoId, g.ProdutoId })
                    //                .OrderBy(o => o.Key.GrupoId)
                    //                .ThenBy(o => o.Key.ProdutoId)
                    //                .Select(s => new ProdutoSaldoReportDto
                    //                {
                    //                    GrupoId = s.Key.GrupoId,
                    //                    ProdutoId = s.Key.ProdutoId,
                    //                    Descricao = s.Select(m => m.Produto).FirstOrDefault().Descricao,
                    //                    QuantidadeAtual = s.Sum(soma => soma.QuantidadeAtual),
                    //                    QuantidadeEntradaPendente = s.Sum(soma => soma.QuantidadeEntradaPendente),
                    //                    QuantidadeSaidaPendente = s.Sum(soma => soma.QuantidadeSaidaPendente),
                    //                    SaldoFuturo = s.Sum(soma => soma.QuantidadeAtual + soma.QuantidadeEntradaPendente - soma.QuantidadeSaidaPendente)
                    //                });

                    var queryList = await query.ToListAsync();

                    var listaProdutoSaldoDto = VWRptSaldoProdutoDto.Mapear(queryList).ToList();


                    //foreach (var item in queryList)
                    //{
                    //    var produtoId = item.ProdutoId.Value;
                    //    UnidadeDto unidadeGerencial = await ObterUnidadeGerencial(produtoId);
                    //    var grupo = await _produtoRepositorio
                    //        .GetAll()
                    //        .Include(i => i.Grupo)
                    //        .Where(w => w.Id == produtoId)
                    //        .Select(s => s.Grupo)
                    //        .FirstOrDefaultAsync();

                    //    listaProdutoSaldoDto.Add(new ProdutoSaldoMinReportDto
                    //    {
                    //        ProdutoId = item.ProdutoId,
                    //        GrupoId = item.GrupoId,
                    //        Descricao = grupo?.Descricao,
                    //        DescricaoProduto = item.Descricao,
                    //        QuantidadeAtual = item.QuantidadeAtual,
                    //        QuantidadeEntradaPendente = item.QuantidadeEntradaPendente,
                    //        QuantidadeSaidaPendente = item.QuantidadeSaidaPendente,
                    //        SaldoFuturo = item.SaldoFuturo,
                    //        QuantidadeGerencialAtual = item.QuantidadeAtual > 0 ? item.QuantidadeAtual / unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1 : 0,
                    //        QuantidadeGerencialEntradaPendente = item.QuantidadeEntradaPendente > 0 ? item.QuantidadeEntradaPendente / unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1 : 0,
                    //        QuantidadeGerencialSaidaPendente = item.QuantidadeGerencialSaidaPendente > 0 ? item.QuantidadeSaidaPendente / unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1 : 0,
                    //        SaldoGerencialFuturo = item.SaldoFuturo > 0 ? item.SaldoFuturo / unidadeGerencial.Fator > 0 ? unidadeGerencial.Fator : 1 : 0
                    //    });
                    //}

                    //contarProdutos = await query
                    //    .CountAsync();

                    //Produtos = await query
                    //var  Produtos = await queryList
                    // var Produtos = listaProdutoSaldoDto
                    //.AsNoTracking()
                    //.OrderBy(input.Sorting)
                    //.PageBy(input)
                    //.ToListAsync();
                    //        .ToList();

                    //ProdutosDtos = Produtos
                    //    .MapTo<List<ProdutoSaldoDto>>();

                    return new ListResultDto<VWRptSaldoProdutoDto> { Items = listaProdutoSaldoDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWRptEstoqueMovimentoResumidoDto>> ListarEstoqueMovimentoResumidoReport(DateTime startDate, DateTime endDate, long estoqueId, long grupoId, string produto, int tipoRel = 1)
        {
            try
            {
                var tenant = this.GetCurrentTenant();

                using (var dbDefault = new SWMANAGERDbContext("DEFAULT"))
                {
                    if (tenant != null)
                    {
                        using (var db = new SWMANAGERDbContext(tenant.TenancyName))
                        {
                            var dr = db.Database.SqlQuery<VWRptEstoqueMovimentoResumido>("EXEC spRptEstMovResumido @DATAINICIAL,@DATAFINAL", new SqlParameter("DATAINICIAL", startDate), new SqlParameter("DATAFINAL", endDate)).ToList();
                            db.Dispose();
                            var listDto = VWRptEstoqueMovimentoResumidoDto.Mapear(dr).ToList();
                            var result = listDto
                                .WhereIf(estoqueId > 0, m => m.EstoqueId == estoqueId)
                                .WhereIf(grupoId > 0, m => m.GrupoId == grupoId)
                                .WhereIf(!produto.IsNullOrWhiteSpace(), m => m.Produto.ToLower().Contains(produto.ToLower()) || m.CodProduto.ToLower().Contains(produto.ToLower()))
                                .ToList();
                            db.Dispose();
                            dbDefault.Dispose();
                            return new ListResultDto<VWRptEstoqueMovimentoResumidoDto> { Items = result };
                        }
                    }
                    else
                    {
                        throw new Exception(L("TenantNaoEncontrado"));
                    }
                }
                //var query = _vwEstMovResumidoRepositorio
                //    .GetAll()
                //    //.WhereIf(estoqueId > 0, m => m.EstoqueId == estoqueId)
                //    .WhereIf(grupoId > 0, m => m.GrupoId == grupoId)
                //    .WhereIf(!produto.IsNullOrWhiteSpace(), m => m.Produto.ToLower().Contains(produto.ToLower()))
                //    //.WhereIf(!lote.IsNullOrWhiteSpace(), m => m.Lote.ToLower().Contains(lote.ToLower()))
                //    .Where(m => m.Data >= startDate && m.Data <= endDate)
                //    .OrderBy(o => o.Estoque)
                //    .ThenBy(o => o.Grupo)
                //    .ThenBy(o => o.Produto);

                //var queryList = await query.ToListAsync();

                //var listaProdutoSaldoDto = VWRptEstoqueMovimentoResumidoDto.Mapear(queryList).ToList();



                //return new ListResultDto<VWRptEstoqueMovimentoResumidoDto> { Items = listaProdutoSaldoDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWRptEstoqueMovimentoDetalhadoDto>> ListarEstoqueMovimentoDetalhadoReport(DateTime startDate, DateTime endDate, long estoqueId, long grupoId, string produto, string lote, int tipoRel = 2)
        {
            try
            {
                var tenant = this.GetCurrentTenant();

                using (var dbDefault = new SWMANAGERDbContext("DEFAULT"))
                {
                    dbDefault.Dispose();
                    if (tenant != null)
                    {
                        using (var db = new SWMANAGERDbContext(tenant.TenancyName))//tenant.ConnectionStringNameSw))
                        {
                            var dr = db.Database.SqlQuery<VWRptEstoqueMovimentoDetalhadoDto>(
                                "EXEC spRptEstMovDetalhado @DATAINICIAL,@DATAFINAL",
                                new SqlParameter("DATAINICIAL", startDate),
                                new SqlParameter("DATAFINAL", endDate)
                                ).ToList();
                            db.Dispose();
                            //var listDto = VWRptEstoqueMovimentoResumidoDto.Mapear(dr).ToList();
                            var result = dr
                                .WhereIf(estoqueId > 0, m => m.EstoqueId == estoqueId)
                                .WhereIf(grupoId > 0, m => m.GrupoId == grupoId)
                                .WhereIf(!produto.IsNullOrWhiteSpace(), m => m.Produto.ToLower().Contains(produto.ToLower()) || m.CodProduto.ToLower().Contains(produto.ToLower()))
                                .ToList();
                            db.Dispose();
                            dbDefault.Dispose();
                            return new ListResultDto<VWRptEstoqueMovimentoDetalhadoDto> { Items = result };
                        }
                    }
                    else
                    {
                        throw new Exception(L("TenantNaoEncontrado"));
                    }
                }

                //var query = _vwEstMovDetalhadoRepositorio
                //    .GetAll()
                //    .WhereIf(estoqueId > 0, m => m.EstoqueId == estoqueId)
                //    .WhereIf(grupoId > 0, m => m.GrupoId == grupoId)
                //    .WhereIf(!produto.IsNullOrWhiteSpace(), m => m.Produto.ToLower().Contains(produto.ToLower()))
                //    .WhereIf(!lote.IsNullOrWhiteSpace(), m => m.Lote.ToLower().Contains(lote.ToLower()))
                //    .Where(m => m.Data >= startDate && m.Data <= endDate)
                //    .OrderBy(o => o.Estoque)
                //    .ThenBy(o => o.Grupo)
                //    .ThenBy(o => o.Produto);

                //var queryList = await query.ToListAsync();

                //var listaProdutoSaldoDto = VWRptEstoqueMovimentoDetalhadoDto.Mapear(queryList).ToList();

                //return new ListResultDto<VWRptEstoqueMovimentoDetalhadoDto> { Items = listaProdutoSaldoDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ProdutoSaldoDto>> ListarProdutoSaldoDetalhes(ListarSaldoInput input)
        {
            var contarProdutos = 0;
            List<ProdutoSaldoDto> ProdutosDtos = new List<ProdutoSaldoDto>();
            try
            {
                UnidadeDto unidadeGerencial = await ObterUnidadeGerencial((long)input.Id);
                UnidadeDto unidadeReferencial = await ObterUnidadeReferencial((long)input.Id);
                using (var _produtoSaldoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {

                    var query = _produtoSaldoRepositorio.Object
                        .GetAll()
                        .Include(m => m.LoteValidade)
                        .Include(m => m.LoteValidade.EstoqueLaboratorio)
                        .Where(m => m.ProdutoId == input.Id
                            && m.EstoqueId == input.EstoqueId
                        )
                        .Select(s => new ProdutoSaldoDto
                        {
                            EstoqueId = s.EstoqueId,
                            LoteValidadeId = s.LoteValidadeId,
                            QuantidadeAtual = s.QuantidadeAtual,

                            NomeLaboratorio = s.LoteValidade.EstoqueLaboratorio.Descricao,
                            Lote = s.LoteValidade.Lote,
                            Validade = s.LoteValidade.Validade,

                            QuantidadeEntradaPendente = s.QuantidadeEntradaPendente,
                            QuantidadeSaidaPendente = s.QuantidadeSaidaPendente,
                            SaldoFuturo = (s.QuantidadeAtual + s.QuantidadeEntradaPendente - s.QuantidadeSaidaPendente),

                            QuantidadeGerencialAtual = s.QuantidadeAtual / unidadeGerencial.Fator,
                            QuantidadeGerencialEntradaPendente = s.QuantidadeEntradaPendente / unidadeGerencial.Fator,
                            QuantidadeGerencialSaidaPendente = s.QuantidadeSaidaPendente / unidadeGerencial.Fator,
                            SaldoGerencialFuturo = ((s.QuantidadeAtual / unidadeGerencial.Fator) + (s.QuantidadeEntradaPendente / unidadeGerencial.Fator) - (s.QuantidadeSaidaPendente / unidadeGerencial.Fator)),

                            UnidadeGerencial = unidadeGerencial.Sigla,
                            UnidadeReferencia = unidadeReferencial.Sigla

                        });


                    contarProdutos = await query
                        .CountAsync();

                    //Produtos = query.ToList();

                    ProdutosDtos = query.ToList();

                    //ProdutosDtos = Produtos.MapTo<List<ProdutoSaldoDto>>();

                    return new PagedResultDto<ProdutoSaldoDto>(
                        contarProdutos,
                        ProdutosDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ProdutoSaldoMinDto>> ListarProdutoSaldoFilhos(ListarInput input)
        {
            var contarProdutos = 0;
            // List<ProdutoSaldo> Produtos;
            List<ProdutoSaldoDto> ProdutosDtos = new List<ProdutoSaldoDto>();
            try
            {
                using (var estoqueAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueAppService>())
                using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var produtoSaldoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {
                    var id = Convert.ToInt64(input.Filtro);
                    var query = from produto in produtoRepositorio.Object.GetAll()
                                join saldoProduto in produtoSaldoRepositorio.Object.GetAll()
                                on produto.Id equals saldoProduto.ProdutoId
                                where produto.ProdutoPrincipalId == id

                                select saldoProduto;


                    var queryList = query
                                    .GroupBy(g => new { g.EstoqueId, g.ProdutoId })
                                    .Select(s => new ProdutoSaldoDto
                                    {
                                        EstoqueId = s.Key.EstoqueId,
                                        ProdutoId = s.Key.ProdutoId,
                                        QuantidadeAtual = s.Sum(soma => soma.QuantidadeAtual),
                                        QuantidadeEntradaPendente = s.Sum(soma => soma.QuantidadeEntradaPendente),
                                        QuantidadeSaidaPendente = s.Sum(soma => soma.QuantidadeSaidaPendente),
                                        SaldoFuturo = s.Sum(soma => soma.QuantidadeAtual + soma.QuantidadeEntradaPendente - soma.QuantidadeSaidaPendente)
                                    })
                                    .ToList();

                    var listaProdutoSaldoDto = new List<ProdutoSaldoMinDto>();



                    foreach (var item in queryList)
                    {
                        var estoqueDto = new EstoqueDto();
                        estoqueDto = await estoqueAppService.Object.Obter(item.EstoqueId);
                        var produto = await Obter(item.ProdutoId);

                        UnidadeDto unidadeGerencial = await ObterUnidadeGerencial(item.ProdutoId);
                        UnidadeDto unidadeReferencial = await ObterUnidadeReferencial(item.ProdutoId);

                        listaProdutoSaldoDto.Add(new ProdutoSaldoMinDto
                        {
                            ProdutoId = item.ProdutoId,
                            EstoqueId = item.EstoqueId,
                            Descricao = estoqueDto.Descricao,
                            DescricaoProduto = produto.Descricao,

                            QuantidadeAtual = item.QuantidadeAtual,
                            QuantidadeEntradaPendente = item.QuantidadeEntradaPendente,
                            QuantidadeSaidaPendente = item.QuantidadeSaidaPendente,
                            SaldoFuturo = item.SaldoFuturo,

                            QuantidadeGerencialAtual = item.QuantidadeAtual / unidadeGerencial.Fator,
                            QuantidadeGerencialEntradaPendente = item.QuantidadeEntradaPendente / unidadeGerencial.Fator,
                            QuantidadeGerencialSaidaPendente = item.QuantidadeSaidaPendente / unidadeGerencial.Fator,
                            SaldoGerencialFuturo = item.SaldoFuturo / unidadeGerencial.Fator,
                            UnidadeGerencial = unidadeGerencial.Sigla,
                            UnidadeReferencia = unidadeReferencial.Sigla


                        });
                    }

                    contarProdutos = await query
                        .CountAsync();

                    //Produtos = await query
                    //var  Produtos = await queryList
                    var Produtos = listaProdutoSaldoDto
                          //.AsNoTracking()
                          //.OrderBy(input.Sorting)
                          //.PageBy(input)
                          //.ToListAsync();
                          .ToList();

                    //ProdutosDtos = Produtos
                    //    .MapTo<List<ProdutoSaldoDto>>();

                    return new PagedResultDto<ProdutoSaldoMinDto>(
                        contarProdutos,
                        Produtos
                        //ProdutosDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<PagedResultDto<ProdutoDto>> ListarProdutoMesmoPrincipal(long id)
        //public async Task<PagedResultDto<ProdutoDto>> ListarProdutoMesmoPrincipal(long id, long principalId)
        public async Task<PagedResultDto<ProdutoDto>> ListarProdutoMesmoPrincipal(ListarInput input)
        {
            var contarProdutos = 0;
            List<Produto> Produtos;
            List<ProdutoDto> ProdutosDtos = new List<ProdutoDto>();
            try
            {
                //var id = Convert.ToInt64(input.Filtro);
                var id = Convert.ToInt64(input.Id);
                var principalId = Convert.ToInt64(input.PrincipalId);
                using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = produtoRepositorio.Object
                        .GetAll()
                        .Where(m => m.Id != id)
                        .Where(m => m.ProdutoPrincipalId == principalId);

                    contarProdutos = await query
                        .CountAsync();

                    Produtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    ProdutosDtos = Produtos
                        .MapTo<List<ProdutoDto>>();

                    //return new ListResultDto<ProdutoDto> { Items = ProdutosDtos };

                    return new PagedResultDto<ProdutoDto>(
                        contarProdutos,
                        ProdutosDtos
                    );
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task<PagedResultDto<ProdutoSaldoMinDto>> ListarProdutoMesmoPrincipalComSaldo(ListarInput input)
        {
            var contarProdutos = 0;
            // List<ProdutoSaldo> Produtos;
            List<ProdutoSaldoDto> ProdutosDtos = new List<ProdutoSaldoDto>();
            try
            {
                //var query = from mov in _estoqueMovimentoReposi0tory.GetAll()
                //            join item in _estoqueMovimentoItemRepositorio.GetAll()
                //            on mov.Id equals item.MovimentoId
                //            into movjoin
                //            from joinedmov in movjoin.DefaultIfEmpty()
                //            where mov.EstoqueId == estoqueId
                //               && joinedmov.ProdutoId == produtoId
                //               && !String.IsNullOrEmpty(joinedmov.NumeroSerie)

                using (var produtoSaldoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var id = Convert.ToInt64(input.Id);
                    var principalId = Convert.ToInt64(input.PrincipalId);
                    var query = from produto in produtoRepositorio.Object.GetAll()
                                join saldoProduto in produtoSaldoRepositorio.Object.GetAll()
                                on produto.Id equals saldoProduto.ProdutoId
                                into produtojoin
                                from joined in produtojoin.DefaultIfEmpty()
                                where ((produto.ProdutoPrincipalId == principalId) && (produto.IsAtivo)) && (produto.Id != id)
                                select new //Rapidinho
                                {
                                    //ProdutoId = joined.ProdutoId == null ? 0 : joined.ProdutoId,
                                    ProdutoId = produto.Id,
                                    //EstoqueId = joined.EstoqueId,
                                    QuantidadeAtual = joined == null ? 0 : joined.QuantidadeAtual,
                                    QuantidadeEntradaPendente = joined == null ? 0 : joined.QuantidadeEntradaPendente,
                                    QuantidadeSaidaPendente = joined == null ? 0 : joined.QuantidadeSaidaPendente
                                };

                    var queryList = query
                                    //.GroupBy(g => new { g.EstoqueId, g.ProdutoId })
                                    .GroupBy(g => new { g.ProdutoId })
                                    .Select(s => new ProdutoSaldoDto
                                    {
                                        //EstoqueId = s.Key.EstoqueId,
                                        ProdutoId = s.Key.ProdutoId,
                                        QuantidadeAtual = s.Sum(soma => soma.QuantidadeAtual),
                                        QuantidadeEntradaPendente = s.Sum(soma => soma.QuantidadeEntradaPendente),
                                        QuantidadeSaidaPendente = s.Sum(soma => soma.QuantidadeSaidaPendente),
                                        SaldoFuturo = s.Sum(soma => soma.QuantidadeAtual + soma.QuantidadeEntradaPendente - soma.QuantidadeSaidaPendente)
                                    })
                                    .ToList();

                    var listaProdutoSaldoDto = new List<ProdutoSaldoMinDto>();

                    foreach (var item in queryList)
                    {
                        //var estoqueDto = new EstoqueDto();
                        //estoqueDto = await _estoqueAppService.Obter(item.EstoqueId);
                        var produto = await Obter(item.ProdutoId);

                        UnidadeDto unidadeGerencial = await ObterUnidadeGerencial(item.ProdutoId);
                        UnidadeDto unidadeReferencial = await ObterUnidadeReferencial(item.ProdutoId);

                        listaProdutoSaldoDto.Add(new ProdutoSaldoMinDto
                        {
                            Codigo = produto.Codigo,
                            ProdutoId = item.ProdutoId,
                            //EstoqueId = item.EstoqueId,
                            //Descricao = estoqueDto.Descricao,
                            EstoqueId = 0,
                            Descricao = null,
                            DescricaoProduto = produto.Descricao,

                            QuantidadeAtual = item.QuantidadeAtual,
                            QuantidadeEntradaPendente = item.QuantidadeEntradaPendente,
                            QuantidadeSaidaPendente = item.QuantidadeSaidaPendente,
                            SaldoFuturo = item.SaldoFuturo,

                            QuantidadeGerencialAtual = item.QuantidadeAtual / unidadeGerencial.Fator,
                            QuantidadeGerencialEntradaPendente = item.QuantidadeEntradaPendente / unidadeGerencial.Fator,
                            QuantidadeGerencialSaidaPendente = item.QuantidadeSaidaPendente / unidadeGerencial.Fator,
                            SaldoGerencialFuturo = item.SaldoFuturo / unidadeGerencial.Fator,
                            UnidadeReferencia = unidadeReferencial.Sigla,
                            UnidadeGerencial = unidadeGerencial.Sigla
                        });
                    }

                    contarProdutos = await query
                        .CountAsync();

                    //var  Produtos = await queryList
                    var Produtos = listaProdutoSaldoDto
                    //var Produtos = query
                    //.AsNoTracking()
                    //.OrderBy(input.Sorting)
                    //.PageBy(input)
                    //.ToListAsync();
                    .ToList();

                    //return new PagedResultDto<Rapidinho>(
                    return new PagedResultDto<ProdutoSaldoMinDto>(
                        contarProdutos,
                        Produtos
                        );
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        #endregion Listar

        #region → Dropdowns
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                return await ListarDropdownLambda(dropdownInput
                                                         , produtoRepositorio.Object
                                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                        || m.Codigo.ToString().Contains(dropdownInput.search))
                                                         && (!m.IsPrincipal
                                                         && !m.IsBloqueioCompra
                                                         && m.IsAtivo)


                                                        , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                        , o => o.Descricao
                                                        );
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoPorEstoqueIdDropdown(DropdownInput dropdownInput)
        {
            return await Select2Helper.CreateSelect2(this)
                .AddIdField("Produto.Id")
                .AddTextField("CONCAT(CONCAT(Produto.Codigo, ' - ', Produto.Descricao), ' - Qtd: ', (SELECT SUM(ProdutoSaldo.QuantidadeAtual) FROM ProdutoSaldo WHERE ProdutoId = Produto.Id AND EstoqueId = @filtro))")
                .AddFromClause(@"Est_Produto AS Produto")
                .AddOrderByClause("Produto.Descricao")
                .AddWhereMethod((input, dapperParameters) =>
                {
                    var whereBuilder = new StringBuilder();
                    dapperParameters.Add("isFalse", false);
                    dapperParameters.Add("isTrue", true);
                    whereBuilder.Append(@"
                        Produto.IsDeleted = @isFalse
                        AND Produto.IsPrincipal = @isFalse 
                        AND Produto.IsBloqueioCompra = @isFalse 
                        AND EXISTS (SELECT 1 FROM Est_EstoqueGrupo WHERE Est_EstoqueGrupo.GrupoId = Produto.GrupoId AND Est_EstoqueGrupo.IsDeleted = @isFalse AND Est_EstoqueGrupo.IsTodosItens = @isTrue  AND (Est_EstoqueGrupo.EstoqueId = @filtro OR Est_EstoqueGrupo.EstoqueId = @filtros ) )
                        AND Produto.IsAtivo = @isTrue 
                       ");
                    whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND (Produto.Descricao LIKE '%' + @search + '%' OR Produto.Codigo LIKE '%' + @search + '%')");
                    return whereBuilder.ToString();
                }).ExecuteAsync(dropdownInput);
        }

        public async Task<DropdownItems> ObterProdutoPorCodigoBarrasDropdown(string search)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                var query = produtoRepositorio.Object.GetAll()
                      .Where(p => p.CodigoBarra.ToString().Equals(search))
                      .Select(s => new DropdownItems { id = s.Id, text = string.Concat(s.Codigo.ToString(), " - ", s.Descricao) });

                if (query.Count() > 0)
                    return await query.SingleAsync();
                else
                    return new DropdownItems();
            }

        }


        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoPorGrupoEClasseDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                long grupoId;
                long classeId;

                long.TryParse(dropdownInput.filtros[0], out grupoId);
                long.TryParse(dropdownInput.filtros[1], out classeId);

                return await ListarDropdownLambda(dropdownInput
                                                         , produtoRepositorio.Object
                                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                        || m.Codigo.ToString().Contains(dropdownInput.search))
                                                         && (!m.IsPrincipal
                                                         && !m.IsBloqueioCompra
                                                         && m.IsAtivo)
                                                         && m.GrupoId == grupoId
                                                         && m.GrupoClasseId == classeId
                                                        , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                        , o => o.Descricao
                                                        );
            }
        }

        /// <summary>
        /// Listar produtos filtrados por Grupo, paginados para uso com select2
        /// </summary>
        /// <param name="dropdownInput"></param>
        /// <returns></returns>
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoPorGrupoDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                long grupoId;

                long.TryParse(dropdownInput.filtro, out grupoId);

                return await ListarDropdownLambda(dropdownInput
                                                         , produtoRepositorio.Object
                                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                        || m.Codigo.ToString().Contains(dropdownInput.search))
                                                         && (!m.IsPrincipal
                                                         && !m.IsBloqueioCompra
                                                         && m.IsAtivo)

                                                         && m.GrupoId == grupoId

                                                        , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                        , o => o.Descricao
                                                        );
            }
        }

        /// <summary>
        ///Listar para BrasPreco, paginados para uso com select2
        /// </summary>
        /// <param name="dropdownInput"></param>
        /// <returns></returns>
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownParaBrasPreco(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<ProdutoDto> faturamentoItensDto = new List<ProdutoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = from p in produtoRepositorio.Object.GetAll()
                                .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Id.ToString() == dropdownInput.filtro)
                                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.ToString().Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems<long>
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    var result = await queryResultPage.ToListAsync();

                    int total = await query.CountAsync();

                    return new ResultDropdownList<long>() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoPorEstoqueDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var estoqueGrupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
            {
                long estoqueId;

                long.TryParse(dropdownInput.filtros[0], out estoqueId);

                var estoquesGrupos = estoqueGrupoRepositorio.Object.GetAll();

                var result = await ListarDropdownLambda(dropdownInput
                                             , produtoRepositorio.Object
                                             , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                            || m.Codigo.ToString().Contains(dropdownInput.search) || m.CodigoBarra.ToString().Contains(dropdownInput.search))
                                             && (!m.IsPrincipal
                                             && !m.IsBloqueioCompra
                                             && m.IsAtivo)

                                            //&& estoquesGrupos.Any(a => a.GrupoId == m.GrupoId
                                            //                                          && a.EstoqueId == estoqueId)




                                            , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                            , o => o.Descricao
                                            );

                return result;
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarMedicamentoPorEstoqueDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var estoqueGrupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
            {
                long estoqueId;
                bool isMedicamento;

                long.TryParse(dropdownInput.filtros[0], out estoqueId);
                bool.TryParse(dropdownInput.filtros[1], out isMedicamento);

                var estoquesGrupos = estoqueGrupoRepositorio.Object.GetAll();

                var result = await ListarDropdownLambda(dropdownInput
                                             , produtoRepositorio.Object
                                             , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                            || m.Codigo.ToString().Contains(dropdownInput.search))
                                             && (!m.IsPrincipal
                                             //&& m.ProdutoPrincipalId != null
                                             && !m.IsBloqueioCompra
                                             && m.IsAtivo
                                             && m.IsMedicamento == isMedicamento)
                                             && estoquesGrupos.Any(a => a.GrupoId == m.GrupoId &&
                                                                        a.EstoqueId == estoqueId // &&
                                                                                                 // m.GrupoId == a.GrupoId
                                                                        )
                                            , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                            , o => o.Descricao
                                            );

                return result;
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoPorEstoque2Dropdown(DropdownInput dropdownInput)
        {
            using (var produtoEstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoEstoque, long>>())
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                long estoqueId;

                //long.TryParse(dropdownInput.filtros[0], out estoqueId);

                long.TryParse(dropdownInput.filtro, out estoqueId);

                var produtosEstoque = produtoEstoqueRepositorio.Object.GetAll();

                return await ListarDropdownLambda(dropdownInput
                                                  , produtoRepositorio.Object
                                                  , m => (!m.IsPrincipal && !m.IsBloqueioCompra && m.IsAtivo)
                                                          && produtosEstoque.Any(a => a.ProdutoId == m.Id && a.EstoqueId == estoqueId)
                                                  , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                  , o => o.Descricao);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoPorSaidaAtendimentoDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var estoqueMovimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            {

                long estoqueId;
                long atendimentoId;

                long.TryParse(dropdownInput.filtros[0], out estoqueId);
                long.TryParse(dropdownInput.filtros[1], out atendimentoId);


                var query = from movimento in estoqueMovimentoRepositorio.Object.GetAll()
                            join movimentoItem in estoqueMovimentoItemRepositorio.Object.GetAll()
                            on movimento.Id equals movimentoItem.MovimentoId
                            where movimento.EstoqueId == estoqueId
                               && movimento.AtendimentoId == atendimentoId
                            select movimentoItem;


                return await ListarDropdownLambda(dropdownInput
                                                         , produtoRepositorio.Object
                                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                        || m.Codigo.ToString().Contains(dropdownInput.search))
                                                         && (!m.IsPrincipal
                                                         && !m.IsBloqueioCompra
                                                         && m.IsAtivo
                                                         && query.Any(a => a.ProdutoId == m.Id))
                                                        , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                        , o => o.Descricao
                                                        );
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoPorSaidaSetorDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var estoqueMovimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            {
                long estoqueId;
                long unidadeOrganizacionalId;

                long.TryParse(dropdownInput.filtros[0], out estoqueId);
                long.TryParse(dropdownInput.filtros[1], out unidadeOrganizacionalId);


                var query = from movimento in estoqueMovimentoRepositorio.Object.GetAll()
                            join movimentoItem in estoqueMovimentoItemRepositorio.Object.GetAll()
                            on movimento.Id equals movimentoItem.MovimentoId
                            where movimento.EstoqueId == estoqueId
                               && movimento.UnidadeOrganizacionalId == unidadeOrganizacionalId
                            select movimentoItem;


                return await ListarDropdownLambda(dropdownInput, produtoRepositorio.Object
                                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                        || m.Codigo.ToString().Contains(dropdownInput.search))
                                                         && (!m.IsPrincipal
                                                         && !m.IsBloqueioCompra
                                                         && m.IsAtivo
                                                         && query.Any(a => a.ProdutoId == m.Id))
                                                        , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                        , o => o.Descricao);
            }
        }


        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoSaidaPorEstoqueIdEAtendimentoDropdown(DropdownInput dropdownInput)
        {
            using (var conn = new SqlConnection(this.GetConnection()))
            {
                var dapperParameters = new Dictionary<string, object>();

                var estoqueId = 0;
                var atendimentoId = 0;

                int.TryParse(dropdownInput.filtros[0], out estoqueId);

                int.TryParse(dropdownInput.filtros[1], out atendimentoId);

                dapperParameters.Add("isFalse", false);
                dapperParameters.Add("isTrue", true);

                dapperParameters.Add("estoqueId", estoqueId);
                dapperParameters.Add("atendimentoId", atendimentoId);
                dapperParameters.Add("search", dropdownInput.search);
                var query = $@"
                SELECT
                   DISTINCT(Est_Produto.Id) AS id,
                   CONCAT( Est_Produto.Codigo, ' - ', Est_Produto.Descricao) AS text
                FROM
                    Est_Produto
                    INNER JOIN EstoqueMovimentoItem ON EstoqueMovimentoItem.ProdutoId = Est_Produto.Id
                    INNER JOIN EstoqueMovimento ON EstoqueMovimento.Id = EstoqueMovimentoItem.MovimentoId
                WHERE
                    Est_Produto.IsDeleted = @isFalse AND EstoqueMovimentoItem.IsDeleted = @isFalse AND EstoqueMovimento.IsDeleted = @isFalse
                    AND EstoqueMovimento.IsEntrada = @isFalse AND EstoqueMovimento.AtendimentoId = @atendimentoId AND EstoqueMovimento.EstoqueId = @estoqueId
                    AND EXISTS (SELECT 1 FROM Est_EstoqueGrupo WHERE Est_EstoqueGrupo.EstoqueId  = @estoqueId  AND Est_EstoqueGrupo.IsTodosItens = @isTrue AND Est_EstoqueGrupo.GrupoId = Est_Produto.GrupoId AND Est_EstoqueGrupo.IsDeleted = @isFalse)
                    {(!dropdownInput.search.IsNullOrEmpty()? " AND Est_Produto.Descricao like '%' + @search + '%'": "")}
                ORDER BY
                    CONCAT(Est_Produto.Codigo, ' - ', Est_Produto.Descricao)";

                var itens = await conn.QueryAsync<DropdownItems<long>>(query, dapperParameters);

                return new ResultDropdownList<long> { Items = itens.ToList(), TotalCount = itens.Count() };
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoSaidaPorEstoqueIdESetorDropdown(DropdownInput dropdownInput)
        {
            using (var conn = new SqlConnection(this.GetConnection()))
            {
                var dapperParameters = new Dictionary<string, object>();

                var estoqueId = 0;
                var unidadeOrganizacionalId = 0;

                int.TryParse(dropdownInput.filtros[0], out estoqueId);

                int.TryParse(dropdownInput.filtros[1], out unidadeOrganizacionalId);

                dapperParameters.Add("isFalse", false);
                dapperParameters.Add("isTrue", true);

                dapperParameters.Add("estoqueId", estoqueId);
                dapperParameters.Add("unidadeOrganizacionalId", unidadeOrganizacionalId);
                dapperParameters.Add("search", dropdownInput.search);

                var query = $@"
                SELECT
                   DISTINCT(Est_Produto.Id) AS id,
                   CONCAT( Est_Produto.Codigo, ' - ', Est_Produto.Descricao) AS text
                FROM
                    Est_Produto
                    INNER JOIN EstoqueMovimentoItem ON EstoqueMovimentoItem.ProdutoId = Est_Produto.Id
                    INNER JOIN EstoqueMovimento ON EstoqueMovimento.Id = EstoqueMovimentoItem.MovimentoId
                WHERE
                    Est_Produto.IsDeleted = @isFalse AND EstoqueMovimentoItem.IsDeleted = @isFalse AND EstoqueMovimento.IsDeleted = @isFalse
                    AND EstoqueMovimento.IsEntrada = @isFalse AND EstoqueMovimento.UnidadeOrganizacionalId = @unidadeOrganizacionalId AND EstoqueMovimento.EstoqueId = @estoqueId
                    AND EXISTS (SELECT 1 FROM Est_EstoqueGrupo WHERE Est_EstoqueGrupo.EstoqueId  = @estoqueId AND Est_EstoqueGrupo.IsTodosItens = @isTrue  AND Est_EstoqueGrupo.GrupoId = Est_Produto.GrupoId AND Est_EstoqueGrupo.IsDeleted = @isFalse)
                    {(!dropdownInput.search.IsNullOrEmpty()? " AND Est_Produto.Descricao like '%' + @search+'%'": "")}
                ORDER BY
                    CONCAT(Est_Produto.Codigo, ' - ', Est_Produto.Descricao)";

                var itens = await conn.QueryAsync<DropdownItems<long>>(query, dapperParameters);

                return new ResultDropdownList<long> { Items = itens.ToList(), TotalCount = itens.Count() };
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await ListarProdutoPorEstoque2Dropdown(dropdownInput);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDcbDropdown(DropdownInput dropdownInput)
        {
            try
            {
                using (var dcbRepository = IocManager.Instance.ResolveAsDisposable<IRepository<DCB, long>>())
                {
                    return await Select2Helper.CreateSelect2(this, dcbRepository.Object).ExecuteAsync(dropdownInput);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarProdutoMestreDropdown(DropdownInput dropdownInput)
        {
            using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                return await ListarDropdownLambda(dropdownInput
                                                       , produtoRepositorio.Object
                                                       , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search)
                                                      || m.Codigo.ToString().Contains(dropdownInput.search))
                                                       && m.IsPrincipal
                                                      , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                      , o => o.Descricao);
            }
        }

        #endregion Dropdowns

        #endregion Metodos
    }
}