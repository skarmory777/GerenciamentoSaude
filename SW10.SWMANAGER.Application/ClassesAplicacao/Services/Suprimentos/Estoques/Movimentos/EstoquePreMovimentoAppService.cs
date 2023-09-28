using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using Castle.Core.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Validacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Movimentos.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helper;
using SW10.SWMANAGER.Helpers;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstoquePreMovimentoAppService : SWMANAGERAppServiceBase, IEstoquePreMovimentoAppService
    {
        [UnitOfWork]
        public async Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarSaida(EstoquePreMovimentoDto input)
        {
            input.IsEntrada = false;
            input.Movimento = input.Emissao;
            input.EstTipoOperacaoId = 3;
            return await this.CriarOuEditar(input);
        }

        public async Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditar(EstoquePreMovimentoDto input)
        {
            var retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>();

            try
            {
                input.CentroCustoId = input.CentroCustoId == 0 ? null : input.CentroCustoId;

                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                {
                    var preMovimentosItens = await estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                            .Include(x => x.EstoquePreMovimentosLoteValidades)
                            .Include(x => x.EstoquePreMovimentosLoteValidades.Select(z => z.LoteValidade))
                            .Include(x => x.Produto)
                            .Where(w => w.PreMovimentoId == input.Id).ToListAsync().ConfigureAwait(false);

                    var _preMovimentoItens = EstoquePreMovimentoItemDto.MapPreMovimentoItem(preMovimentosItens);


                    //TIPO MOVIMENTO INVENTÁRIO 
                    if (input.EstTipoMovimentoId != (long)EnumTipoMovimento.Inventario_Entrada)
                    {
                        retornoPadrao.Errors = await preMovimentoValidacaoDomainService.Object.Validar(input, _preMovimentoItens);

                        var erroESt0003 = retornoPadrao.Errors.Where(w => w.CodigoErro == "EST0003").ToList();
                        if (erroESt0003.Any())
                        {
                            retornoPadrao.Warnings = new List<ErroDto>
                            {
                                ErroDto.Criar("","Existem itens não disponiveis no estoque.  A validação será realizada na confirmação da saída.")
                            };

                            foreach (var item in erroESt0003)
                            {
                                retornoPadrao.Errors.Remove(item);
                            }
                        }
                    }
                    else if (retornoPadrao.Errors == null)
                    {
                        retornoPadrao.Errors = new List<ErroDto>();
                    }


                    if (retornoPadrao.Errors.Any())
                    {
                        return retornoPadrao;
                    }

                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        input.PossuiLoteValidade = preMovimentoValidacaoDomainService.Object.ExisteLoteValidadePendente(input);

                        input.PreMovimentoEstadoId = input.PossuiLoteValidade ? (long)EnumPreMovimentoEstado.NaoAutorizado : (long)EnumPreMovimentoEstado.Autorizado;

                        var preMovimento = EstoquePreMovimentoDto.MapPreMovimento(input); //.MapTo<EstoquePreMovimento>();

                        preMovimento.GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos;

                        if (input.Id.Equals(0))
                        {
                            input.Id = await estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimento);
                        }
                        else
                        {
                            var edit = await estoquePreMovimentoRepository.Object.GetAll().FirstOrDefaultAsync(x => x.Id == input.Id);
                            if (edit != null)
                            {
                                edit.OrdemId = input.OrdemId;
                                edit.Quantidade = input.Quantidade;
                                edit.Serie = input.Serie;
                                //edit.TipoDocumentoId = input.TipoDocumentoId;
                                edit.TipoFreteId = input.TipoFreteId;
                                edit.TotalDocumento = input.TotalDocumento;
                                edit.TotalProduto = input.TotalProduto;
                                edit.ValorFrete = input.ValorFrete;
                                edit.ValorICMS = input.ValorICMS;
                                edit.EmpresaId = input.EmpresaId;
                                edit.SisFornecedorId = input.FornecedorId;
                                edit.Documento = input.Documento;
                                edit.Serie = input.Serie;
                                edit.Movimento = input.Movimento;
                                edit.EstoqueId = input.EstoqueId;
                                edit.CentroCustoId = input.CentroCustoId;
                                edit.OrdemId = input.OrdemId;
                                edit.Emissao = input.Emissao;
                                edit.CFOPId = input.CFOPId;
                                edit.ICMSPer = input.ICMSPer;
                                edit.DescontoPer = input.DescontoPer;
                                edit.AcrescimoDecrescimo = input.AcrescimoDecrescimo;
                                edit.TipoFreteId = input.TipoFreteId;
                                edit.Frete = input.Frete;
                                edit.FretePer = input.FretePer;
                                edit.ValorFrete = input.ValorFrete;
                                edit.Frete_SisFornecedorId = input.Frete_FornecedorId;

                                edit.MedicoSolcitanteId = input.MedicoSolcitanteId;
                                edit.PacienteId = input.PacienteId;
                                edit.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                                edit.AtendimentoId = input.AtendimentoId;
                                edit.Observacao = input.Observacao;

                                edit.Consiginado = input.Consiginado;
                                edit.Contabiliza = input.Contabiliza;
                                edit.AplicacaoDireta = input.AplicacaoDireta;

                                edit.PreMovimentoEstadoId = input.PreMovimentoEstadoId;
                                edit.EstTipoMovimentoId = input.EstTipoMovimentoId;

                                edit.TotalProduto = input.TotalProduto;
                                edit.DescontoPer = input.DescontoPer;

                                await estoquePreMovimentoRepository.Object.UpdateAsync(edit);
                            }
                        }

                        input.PossuiLoteValidade = preMovimentoValidacaoDomainService.Object.ExisteLoteValidadePendente(input);
                        input.PermiteConfirmacaoEntrada = !input.PossuiLoteValidade;

                        var enumTiposIgnore = new List<long>
                        {
                            (long) EnumTipoMovimento.Inventario_Entrada,
                            (long) EnumTipoMovimento.Emprestimo_Entrada,
                            (long) EnumTipoMovimento.Emprestimo_Saida
                        };

                        if (input.IsEntrada && input.EstTipoMovimentoId.HasValue && !enumTiposIgnore.Contains(input.EstTipoMovimentoId.Value))
                        {
                            var retornoContasPagar = await GerarContasPagar(input).ConfigureAwait(false);
                            retornoPadrao.Errors.AddRange(retornoContasPagar.Errors);
                        }

                        if (retornoPadrao.Errors.Count == 0)
                        {
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                        } // unitOfWorkManager.Object.Current.SaveChanges();
                    }

                    retornoPadrao.ReturnObject = input;
                }
            }
            catch (Exception ex)
            {
                if (ex.TargetSite.ToString() != "Void Dispose()")
                {
                    ErrorHandler(retornoPadrao, ex);
                }
            }
            return retornoPadrao;
        }

        public async Task<bool> PermiteConfirmarEntrada(EstoquePreMovimentoDto preMovimento)
        {
            using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
            {
                return !preMovimentoValidacaoDomainService.Object.ExisteLoteValidadePendente(preMovimento);
            }
        }

        public async Task Excluir(EstoquePreMovimentoDto input)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    await estoquePreMovimentoRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task Excluir(long preMovimentoId)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    await estoquePreMovimentoRepository.Object.DeleteAsync(preMovimentoId).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<MovimentoIndexDto>> Listar(ListarEstoquePreMovimentoInput input)
        {
            var contarEstoquePreMovimentos = 0;
            List<MovimentoIndexDto> EstoquePreMovimentos;
            List<MovimentoIndexDto> EstoquePreMovimentoDtos = new List<MovimentoIndexDto>();
            try
            {
                input.PeridoDe = ((DateTime)input.PeridoDe).Date;
                input.PeridoAte = ((DateTime)input.PeridoAte).Date.AddDays(1).AddSeconds(-1);

                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                {
                    var query = estoquePreMovimentoRepository.Object.GetAll().AsNoTracking()
                        .Where(
                            m =>
                                ((input.Filtro == "" || input.Filtro == null)
                                 || m.Documento.ToString().Contains(input.Filtro)
                                 || m.Empresa.NomeFantasia.Contains(input.Filtro)
                                 || m.Emissao.ToString().Contains(input.Filtro)
                                 || m.SisFornecedor.SisPessoa.NomeCompleto.Contains(input.Filtro)
                                 || m.SisFornecedor.SisPessoa.NomeFantasia.Contains(input.Filtro)
                                 || m.TotalDocumento.ToString().Contains(input.Filtro)
                                 || m.EstTipoMovimento.Descricao.Contains(input.Filtro))
                                && (input.FornecedorId == null || m.SisFornecedor.Id == input.FornecedorId)
                                && (input.PeridoDe == null || m.Emissao >= input.PeridoDe)
                                && (input.PeridoAte == null || m.Emissao <= input.PeridoAte)
                                && (input.TipoMovimentoId == null || m.EstTipoMovimento.Id == input.TipoMovimentoId)
                                && m.IsEntrada == input.IsEntrada
                                && (m.GrupoOperacaoId == null ||
                                    m.GrupoOperacaoId == (long)EnumGrupoOperacao.Movimentos)
                                && (m.EstTipoOperacaoId == 1 || m.EstTipoOperacaoId == 3)).Select(
                            s => new MovimentoIndexDto
                            {
                                Id = s.Id,
                                Fornecedor =
                                    (s.SisFornecedor != null)
                                        ? s.SisFornecedor.SisPessoa.FisicaJuridica == "F"
                                            ? s.SisFornecedor.SisPessoa.NomeCompleto
                                            : s.SisFornecedor.SisPessoa.NomeFantasia
                                        : string.Empty,
                                DataEmissaoSaida = s.Emissao,
                                Documento = s.Documento,
                                Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : string.Empty,
                                Valor = s.TotalDocumento,
                                UsuarioId = s.CreatorUserId,
                                PreMovimentoEstadoId = s.PreMovimentoEstadoId,
                                Estoque = (s.Estoque != null) ? s.Estoque.Descricao : string.Empty,
                                TipoMovimento =
                                    (s.EstTipoMovimento != null) ? s.EstTipoMovimento.Descricao : string.Empty
                            });

                    contarEstoquePreMovimentos = await query.CountAsync().ConfigureAwait(false);

                    EstoquePreMovimentos = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                        .ConfigureAwait(false);

                    var idsUsuarios = EstoquePreMovimentos.Select(s => s.UsuarioId);

                    var usuarios = usuarioRepository.Object.GetAll().Where(w => idsUsuarios.Any(a => a == w.Id))
                        .ToList();

                    foreach (var preMovimento in EstoquePreMovimentos)
                    {
                        if (preMovimento.UsuarioId != null)
                        {
                            var usuario = usuarios.FirstOrDefault(w => w.Id == preMovimento.UsuarioId);
                            if (usuario != null)
                            {
                                preMovimento.Usuario = usuario.FullName;
                            }
                        }
                    }

                    return new PagedResultDto<MovimentoIndexDto>(contarEstoquePreMovimentos, EstoquePreMovimentos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<EstoquePreMovimentoDto>> ListarTodos()
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var query = estoquePreMovimentoRepository.Object.GetAll().AsNoTracking();
                    return new ListResultDto<EstoquePreMovimentoDto> { Items = EstoquePreMovimentoDto.MapPreMovimento(await query.ToListAsync().ConfigureAwait(false)) };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var query = await estoquePreMovimentoRepository.Object.GetAll().AsNoTracking()
                        .WhereIf(!input.IsNullOrEmpty(), m => m.Documento.ToString().Contains(input))
                        .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Documento.ToString() })
                        .ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<EstoquePreMovimentoDto> ObterParaConfirmarSolicitacao(long id)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                {
                    var result = await estoquePreMovimentoRepository.Object
                        .GetAllIncluding(
                            x => x.Atendimento,
                            x => x.Atendimento.Paciente,
                            x => x.Atendimento.Paciente.SisPessoa,
                            x => x.Paciente,
                            x => x.Paciente.SisPessoa,
                            x => x.MedicoSolicitante,
                            x => x.MedicoSolicitante.SisPessoa,
                            x => x.Itens)
                        .SingleOrDefaultAsync(x => x.Id == id);

                    var preMovimentoDto = EstoquePreMovimentoDto.MapPreMovimento(result);

                    preMovimentoDto.PossuiLoteValidade = preMovimentoValidacaoDomainService.Object.ExisteLoteValidadePendente(preMovimentoDto);

                    return preMovimentoDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<EstoquePreMovimentoDto> Obter(long id)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                {
                    var query = estoquePreMovimentoRepository.Object.GetAll().AsNoTracking()
                        .Include(i => i.EstTipoMovimento)
                        .Include(i => i.SisFornecedor).Include(i => i.CentroCusto).Include(i => i.EstTipoMovimento)
                        .Include(i => i.Atendimento).Include(i => i.MedicoSolicitante)
                        .Include(i => i.MotivoPerdaProduto)
                        .Include(i => i.MedicoSolicitante.SisPessoa).Include(i => i.Atendimento.Paciente)
                        .Include(i => i.Atendimento.Paciente.SisPessoa).Include(i => i.SisFornecedor.SisPessoa)
                        .Include(i => i.Frete_SisForncedor).Include(i => i.Frete_SisForncedor.SisPessoa)
                        .Include(i => i.Empresa)
                        .Include(i => i.EstoqueEmprestimo).Include(x => x.EstoqueEmprestimo.SisPessoa)
                        .Include(i => i.Estoque).Include(i => i.TipoFrete).Include(i => i.CFOP)
                        .Include(i => i.UnidadeOrganizacional).Where(m => m.Id == id);

                    var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                    if (result == null)
                    {
                        return null;
                    }

                    var preMovimento = EstoquePreMovimentoDto.MapPreMovimento(result);

                    preMovimento.PossuiLoteValidade = preMovimentoValidacaoDomainService.Object.ExisteLoteValidadePendente(preMovimento);

                    return preMovimento;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<TipoMovimentoDto> ObterTipoMovimentoEntrada(long id)
        {
            try
            {
                using (var tpoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
                {
                    var query = tpoMovimentoRepository.Object.GetAll().AsNoTracking().Where(x => x.Id.Equals(id))
                        .Select(
                            s => new TipoMovimentoDto
                            {
                                Id = s.Id,
                                Descricao = s.Descricao,
                                IsOrdemCompra = s.IsOrdemCompra,
                                IsPessoa = s.IsPessoa,
                                IsOrdemCompraObrigatoria = s.IsOrdemCompraObrigatoria,
                                IsFiscal = s.IsFiscal,
                                IsFrete = s.IsFrete,
                                IsFinanceiro = s.IsFinanceiro
                            }).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<MovimentoItemDto>> ListarItensSaida(ListarEstoquePreMovimentoInput input)
        {
            input.Sorting = " Id desc";
            var itens = await this.ListarItens(input).ConfigureAwait(false);

            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var produtoLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueLaboratorio, long>>())
            {
                foreach (var item in itens.Items)
                {
                    var preMovimentoItemLoteValidade = await estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                        .AsNoTracking().Include(i => i.LoteValidade)
                        .FirstOrDefaultAsync(w => w.EstoquePreMovimentoItemId == item.Id).ConfigureAwait(false);

                    if (preMovimentoItemLoteValidade == null || preMovimentoItemLoteValidade.LoteValidade == null)
                    {
                        continue;
                    }

                    item.Lote = preMovimentoItemLoteValidade.LoteValidade.Lote;
                    item.Validade = preMovimentoItemLoteValidade.LoteValidade.Validade;

                    if (preMovimentoItemLoteValidade.LoteValidade.EstLaboratorioId != null)
                    {
                        item.Laboratorio = (await produtoLaboratorioRepository.Object
                            .GetAll()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x =>
                                x.Id == (long)preMovimentoItemLoteValidade.LoteValidade.EstLaboratorioId)
                            .ConfigureAwait(false))?.Descricao;
                    }
                }
                return itens;
            }
        }

        public async Task<PagedResultDto<MovimentoItemDto>> ListarItens(ListarEstoquePreMovimentoInput input)
        {
            var contarPreMovimentos = 0;
            List<MovimentoItemDto> preMovimentoItens = null;
            try
            {
                if (string.IsNullOrEmpty(input.Filtro) || input.Filtro == "0")
                {
                    return new PagedResultDto<MovimentoItemDto>();
                }

                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    var id = Convert.ToInt64(input.Filtro);

                    var query = estoquePreMovimentoItemRepository.Object.GetAll()
                        .AsNoTracking()
                        .Include(x => x.EstoquePreMovimentosLoteValidades.Select(z => z.LoteValidade))
                        .Where(m => m.PreMovimentoId == id)
                        //.Where(m => m.Id == id)
                        .Select(
                            s => new MovimentoItemDto
                            {
                                Id = s.Id,
                                Produto = s.Produto.Descricao,
                                Quantidade = s.Quantidade / s.ProdutoUnidade.Fator,
                                IsValidade = s.Produto.IsValidade,
                                ProdutoId = s.ProdutoId,
                                Unidade = s.ProdutoUnidade.Descricao,
                                ProdutoUnidadeId = s.ProdutoUnidadeId,
                                CustoUnitario = s.CustoUnitario,
                                CustoTotal = s.CustoUnitario * (s.Quantidade / s.ProdutoUnidade.Fator),
                                IsLote = s.Produto.IsLote,
                                NumeroSerie = s.NumeroSerie,
                                ValorIPI = s.ValorIPI,
                                ValorICMS = s.ValorICMS,
                                EstoqueKitItemId = s.EstoqueKitItemId,
                                LoteValidades = s.EstoquePreMovimentosLoteValidades.Select(x =>
                                    new LoteValidadesGrid
                                    {
                                        Id = x.Id,
                                        LoteValidadeId = x.LoteValidadeId,
                                        Lote = x.LoteValidade.Lote,
                                        Quantidade = x.Quantidade,
                                        Validade = x.LoteValidade.Validade
                                    }).ToList()
                            });
                    contarPreMovimentos = await query.CountAsync().ConfigureAwait(false);
                    input.Sorting = " Id desc ";

                    preMovimentoItens = await query.OrderBy(input.Sorting).ToListAsync().ConfigureAwait(false);
                }

                return new PagedResultDto<MovimentoItemDto>(contarPreMovimentos, preMovimentoItens);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false, IsDisabled = false)]
        [HttpPost]
        public async Task<PagedResultDto<EstoquePreMovimentoItemSolicitacaoDto>> ListarItensJson(ListarItensJsonInput input)
        {
            if (input == null || input.Data.IsNullOrEmpty())
            {
                return null;
            }

            try
            {
                var contarPreMovimentos = 0;
                long idGrid = 1;
                var preMovimentoItens = JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(input.Data);

                using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var unidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                using (var estoqueSolicitacaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                {
                    if (!preMovimentoItens.Any())
                    {
                        return new PagedResultDto<EstoquePreMovimentoItemSolicitacaoDto>(contarPreMovimentos, preMovimentoItens);
                    }

                    var idsProdutos = preMovimentoItens.Select(s => s.ProdutoId);
                    var idsProdutosUnidades = preMovimentoItens.Select(s => s.ProdutoUnidadeId);

                    var produtos = await produtoRepository.Object.GetAll().AsNoTracking().Where(w => idsProdutos.Any(a => a == w.Id)).ToListAsync();

                    var unidades = await unidadeRepository.Object.GetAll().AsNoTracking().Where(w => idsProdutosUnidades.Any(a => a == w.Id)).ToListAsync();

                    var estoqueSolicitacaoItemId = preMovimentoItens.First().Id;

                    var preMovimentoId = await estoqueSolicitacaoItemRepository.Object.GetAll()
                        .AsNoTracking().Where(x => x.Id == estoqueSolicitacaoItemId).Select(x => x.SolicitacaoId)
                        .FirstOrDefaultAsync();

                    var preMovimentoEstoqueId = await estoquePreMovimentoRepository.Object.GetAll()
                        .AsNoTracking().Where(x => x.Id == preMovimentoId).Select(x => x.EstoqueId)
                        .FirstOrDefaultAsync();

                    foreach (var item in preMovimentoItens)
                    {
                        item.IdGrid = idGrid++;
                        if (item.ProdutoId != 0)
                        {
                            var produto = produtos.FirstOrDefault(w => w.Id == item.ProdutoId); //_produtoRepository.Get(item.ProdutoId);
                            if (produto != null)
                            {
                                item.Produto = produto.Descricao;
                                item.CodigoProduto = produto.Codigo;
                                item.IsLote = produto.IsLote;
                            }

                            if (produto.IsLote)
                            {
                                var loteSugerido =
                                    (await estoqueLoteValidadeAppService.Object.ListarProdutoDropdownPorLaboratorio(
                                        new DropdownInput
                                        {
                                            filtros = new[] { item.ProdutoId.ToString(), (preMovimentoEstoqueId ?? 0).ToString() },
                                            totalPorPagina = 10000.ToString(),
                                            page = 1.ToString(),
                                        })).Items.FirstOrDefault();
                                if (loteSugerido != null)
                                {
                                    item.LoteSugeridoId = loteSugerido.id;
                                    item.LoteSugeridoName = loteSugerido.text;
                                }
                            }
                        }

                        if (item.ProdutoUnidadeId != null)
                        {
                            var unidade = unidades.FirstOrDefault(x => x.Id == item.ProdutoUnidadeId.Value);
                            if (unidade != null)
                            {
                                item.ProdutoUnidade = unidade.Descricao;
                                item.Quantidade /= unidade.Fator;
                            }
                        }

                        if (item.QuantidadeSolicitada == 0 || !item.QuantidadeAtendida.HasValue)
                        {
                            continue;
                        }

                        item.Quantidade = item.QuantidadeSolicitada - item.QuantidadeAtendida.Value;
                        item.EstadoSolicitacaoItemId = EstoquePreMovimentoItemSolicitacaoDto.CalcularEstadoSolicitacaoItem(item.QuantidadeSolicitada, item.QuantidadeAtendida);
                        item.EstoqueKitItemId = item.EstoqueKitItemId;
                    }

                    return new PagedResultDto<EstoquePreMovimentoItemSolicitacaoDto>(contarPreMovimentos, preMovimentoItens);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public class ListarItensJsonInput
        {
            public string Data { get; set; }
        }

        public EstoquePreMovimentoDto CriarGetIdSaida(EstoquePreMovimentoDto input)
        {
            input.IsEntrada = false;
            input.Movimento = input.Emissao;
            input.EstTipoOperacaoId = 3;

            if (!string.IsNullOrEmpty(input.Documento))
            {
                return this.CriarGetId(input);
            }

            using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
            {
                input.Documento = ultimoIdAppService.Object.ObterProximoCodigo("SaidaProduto").Result;
            }

            return this.CriarGetId(input);
        }

        public EstoquePreMovimentoDto CriarGetIdEntrada(EstoquePreMovimentoDto input)
        {
            input.IsEntrada = true;
            input.EstTipoOperacaoId = 1;
            return this.CriarGetId(input);
        }

        private EstoquePreMovimentoDto CriarGetId(EstoquePreMovimentoDto input)
        {
            try
            {
                if (input.PacienteId == 0)
                {
                    input.PacienteId = null;
                }

                input.CentroCustoId = input.CentroCustoId == 0 ? null : input.CentroCustoId;

                EstoquePreMovimentoDto estoquePreMovimentoDto;
                var preMovimento = EstoquePreMovimentoDto.MapPreMovimento(input);

                preMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.NaoAutorizado;
                preMovimento.GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos;
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    if (input.Id.Equals(0))
                    {
                        estoquePreMovimentoDto = new EstoquePreMovimentoDto
                        {
                            Id = AsyncHelper.RunSync(() => estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimento))
                        };
                    }
                    else
                    {
                        estoquePreMovimentoDto = EstoquePreMovimentoDto.MapPreMovimento(AsyncHelper.RunSync(() => estoquePreMovimentoRepository.Object.UpdateAsync(preMovimento))); //.MapTo<EstoquePreMovimentoDto>();
                    }

                    estoquePreMovimentoDto.PossuiLoteValidade = estoquePreMovimentoItemRepository.Object
                        .GetAll().Any(w => w.PreMovimentoId == preMovimento.Id && w.Produto.IsValidade);

                    estoquePreMovimentoDto.Documento = input.Documento;
                    return estoquePreMovimentoDto;
                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEstoquePreMovimentoInput input)
        {
            try
            {
                var result = await this.Listar(input).ConfigureAwait(false);
                var preMovimentos = result.Items;
                using (var _listarPreMovimentosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarPreMovimentosExcelExporter>())
                {
                    return _listarPreMovimentosExcelExporter.Object.ExportToFile(preMovimentos.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        [UnitOfWork]
        public async Task<DefaultReturn<EstoqueTransferenciaProdutoDto>> TransferirProduto(EstoqueTransferenciaProdutoDto transferenciaProdutoDto)
        {
            var retornoPadrao = new DefaultReturn<EstoqueTransferenciaProdutoDto>
            {
                Errors = new List<ErroDto>(),
                Warnings = new List<ErroDto>()
            };
            try
            {
                using (var estoqueTransferenciaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var documento = await ultimoIdAppService.Object.ObterProximoCodigo("TransferenciaProduto").ConfigureAwait(false);

                    var preMovimentoSaida = new EstoquePreMovimento
                    {
                        EstoqueId = transferenciaProdutoDto.EstoqueSaidaId,
                        Movimento = transferenciaProdutoDto.Movimento
                    };
                    preMovimentoSaida.Emissao = preMovimentoSaida.Movimento;
                    preMovimentoSaida.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Autorizado;
                    preMovimentoSaida.EstTipoOperacaoId = 2;
                    preMovimentoSaida.IsEntrada = false;
                    preMovimentoSaida.Id = transferenciaProdutoDto.PreMovimentoSaidaId;
                    preMovimentoSaida.Documento = documento;

                    preMovimentoSaida.GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos;

                    var preMovimentoEntrada = new EstoquePreMovimento
                    {
                        EstoqueId = transferenciaProdutoDto.EstoqueEntradaId,
                        Movimento = transferenciaProdutoDto.Movimento,
                        Emissao = preMovimentoSaida.Movimento,
                        PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Autorizado,
                        EstTipoOperacaoId = 2,
                        IsEntrada = true,
                        Id = transferenciaProdutoDto.PreMovimentoEntradaId,
                        Documento = documento,
                        GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos
                    };

                    retornoPadrao.Errors = await preMovimentoValidacaoDomainService.Object.ValidarSaidaEstoque(preMovimentoSaida);

                    var erroESt0003 = new List<ErroDto>();
                    erroESt0003 = retornoPadrao.Errors.Where(w => w.CodigoErro == "EST0003").ToList();
                    if (erroESt0003.Any())
                    {
                        retornoPadrao.Warnings.Add(new ErroDto
                        {
                            Descricao =
                                "Existem itens não disponiveis no estoque.  A validação será realizada na confirmação da saída."
                        });

                        foreach (var item in erroESt0003)
                        {
                            retornoPadrao.Errors.Remove(item);
                        }
                    }

                    if (!retornoPadrao.Errors.Any())
                    {
                        if (transferenciaProdutoDto.Id.Equals(0))
                        {
                            var transferenciaProduto = new EstoqueTransferenciaProduto
                            {
                                PreMovimentoEntrada = preMovimentoEntrada,
                                PreMovimentoSaida = preMovimentoSaida
                            };

                            transferenciaProdutoDto.Id = await estoqueTransferenciaProdutoRepository.Object.InsertAndGetIdAsync(transferenciaProduto).ConfigureAwait(false);
                        }
                        else
                        {
                            var transferenciaProduto = await estoqueTransferenciaProdutoRepository.Object.GetAsync(transferenciaProdutoDto.Id);
                            if (transferenciaProduto != null)
                            {
                                // transferenciaProduto.PreMovimentoEntradaId = preMovimentoEntrada.Id;
                                transferenciaProduto.PreMovimentoEntrada = await estoquePreMovimentoRepository.Object.GetAsync(transferenciaProduto.PreMovimentoEntradaId);
                                transferenciaProduto.PreMovimentoEntrada.EstoqueId = preMovimentoEntrada.EstoqueId;

                                //transferenciaProduto.PreMovimentoSaidaId = preMovimentoSaida.Id;
                                transferenciaProduto.PreMovimentoSaida =
                                    estoquePreMovimentoRepository.Object.Get(transferenciaProduto.PreMovimentoSaidaId);
                                transferenciaProduto.PreMovimentoSaida.EstoqueId = preMovimentoSaida.EstoqueId;

                                await estoqueTransferenciaProdutoRepository.Object.UpdateAsync(transferenciaProduto)
                                    .ConfigureAwait(false);
                            }
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                    transferenciaProdutoDto.Documento = documento;
                    retornoPadrao.ReturnObject = transferenciaProdutoDto;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return retornoPadrao;
        }

        public async Task<PagedResultDto<EstoqueTransferenciaProdutoDto>> ListarTransferencia(ListarEstoquePreMovimentoInput input)
        {
            using (var estoqueTransferenciaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            {
                var query = estoqueTransferenciaProdutoRepository.Object.GetAll()
                    .Where(
                        w =>
                            (w.PreMovimentoSaida.Movimento >= input.PeridoDe
                             && w.PreMovimentoSaida.Movimento <= input.PeridoAte)
                            && (String.IsNullOrEmpty(input.Filtro)
                                || (w.PreMovimentoSaida.Estoque.Descricao.Contains(input.Filtro))
                                || w.PreMovimentoEntrada.Estoque.Descricao.Contains(input.Filtro)
                                || w.PreMovimentoEntrada.Documento.Contains(input.Filtro))).Select(
                        s => new EstoqueTransferenciaProdutoDto
                        {
                            Id = s.Id,
                            Documento = s.PreMovimentoSaida.Documento,
                            EstoqueEntrada = s.PreMovimentoEntrada.Estoque.Descricao,
                            EstoqueSaida = s.PreMovimentoSaida.Estoque.Descricao,
                            PreMovimentoEntradaId = s.PreMovimentoEntradaId,
                            PreMovimentoSaidaId = s.PreMovimentoSaidaId,
                            Movimento = s.PreMovimentoEntrada.Movimento,
                            CreatorUserId = s.CreatorUserId,
                            PreMovimentoEstadoId = s.PreMovimentoEntrada.PreMovimentoEstadoId
                        });


                var contarTransferencias = await query.CountAsync().ConfigureAwait(false);
                var transferencias = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                    .ConfigureAwait(false);


                var idsUsuarios = transferencias.Select(s => s.CreatorUserId);

                var usuarios = usuarioRepository.Object.GetAll().Where(w => idsUsuarios.Any(a => a == w.Id)).ToList();

                foreach (var item in transferencias)
                {
                    if (item.CreatorUserId == null)
                    {
                        continue;
                    }

                    var usuario = usuarios.FirstOrDefault(w => w.Id == item.CreatorUserId); //_usuarioRepository.Get((long)item.CreatorUserId);
                    if (usuario != null)
                    {
                        item.Usuario = usuario.FullName;
                    }
                }

                return new PagedResultDto<EstoqueTransferenciaProdutoDto>(contarTransferencias, transferencias);
            }
        }

        public async Task<EstoqueTransferenciaProdutoDto> ObterTransferencia(long id)
        {
            using (var estoqueTransferenciaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
            {
                var estoqueTransferenciaProdutoDto = new EstoqueTransferenciaProdutoDto();
                var query = estoqueTransferenciaProdutoRepository.Object.GetAll().AsNoTracking()
                    .Include(i => i.PreMovimentoEntrada)
                    .Include(i => i.PreMovimentoSaida).Where(m => m.Id == id);

                var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                if (result == null)
                {
                    return null;
                }
                estoqueTransferenciaProdutoDto.Id = result.Id;
                estoqueTransferenciaProdutoDto.PreMovimentoEntradaId = result.PreMovimentoEntradaId;
                estoqueTransferenciaProdutoDto.PreMovimentoEntrada = EstoquePreMovimentoDto.MapPreMovimento(result.PreMovimentoEntrada);

                estoqueTransferenciaProdutoDto.PreMovimentoSaidaId = result.PreMovimentoSaidaId;
                estoqueTransferenciaProdutoDto.PreMovimentoSaida = EstoquePreMovimentoDto.MapPreMovimento(result.PreMovimentoSaida);
                estoqueTransferenciaProdutoDto.Documento = result.PreMovimentoSaida.Documento;

                return estoqueTransferenciaProdutoDto;

            }
        }

        public async Task<EstoqueTransferenciaProdutoDto> ObterTransferenciaPorEntradaId(long id)
        {
            using (var estoqueTransferenciaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
            {
                var estoqueTransferenciaProdutoDto = new EstoqueTransferenciaProdutoDto();
                var query = estoqueTransferenciaProdutoRepository.Object.GetAll().AsNoTracking()
                    .Include(i => i.PreMovimentoEntrada.Estoque)
                    .Include(i => i.PreMovimentoSaida.Estoque).Where(m => m.PreMovimentoEntradaId == id);

                var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                if (result == null) return null;
                estoqueTransferenciaProdutoDto.Id = result.Id;
                estoqueTransferenciaProdutoDto.PreMovimentoEntradaId = result.PreMovimentoEntradaId;
                estoqueTransferenciaProdutoDto.PreMovimentoEntrada = EstoquePreMovimentoDto.MapPreMovimento(result.PreMovimentoEntrada);

                estoqueTransferenciaProdutoDto.PreMovimentoSaidaId = result.PreMovimentoSaidaId;
                estoqueTransferenciaProdutoDto.PreMovimentoSaida = EstoquePreMovimentoDto.MapPreMovimento(result.PreMovimentoSaida);
                estoqueTransferenciaProdutoDto.Documento = result.PreMovimentoSaida.Documento;

                return estoqueTransferenciaProdutoDto;

            }
        }

        public async Task<PagedResultDto<MovimentoItemDto>> ListarItensTranferencia(ListarEstoquePreMovimentoInput input)
        {
            using (var estoqueTransferenciaProdutoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProdutoItem, long>>())
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var produtoLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueLaboratorio, long>>())
            {
                var id = long.Parse(input.Filtro);
                var itensTransferencias = await estoqueTransferenciaProdutoItemRepository.Object.GetAll().AsNoTracking()
                    .Include(i => i.PreMovimentoSaidaItem).Include(i => i.PreMovimentoSaidaItem.Produto)
                    .Include(i => i.PreMovimentoSaidaItem.ProdutoUnidade)
                    .Where(w => w.EstoqueTransferenciaProdutoID == id).ToListAsync();

                // var itens = await ListarItens(input);

                var itens = new List<MovimentoItemDto>();

                foreach (var itemTransferencia in itensTransferencias)
                {
                    var item = new MovimentoItemDto
                    {
                        TransferenciaItemId = itemTransferencia.Id
                    };

                    var preMovimentoItem = itemTransferencia.PreMovimentoSaidaItem;

                    item.Id = itemTransferencia.Id;
                    item.ProdutoId = itemTransferencia.PreMovimentoSaidaItem.ProdutoId;
                    item.Produto = itemTransferencia.PreMovimentoSaidaItem.Produto.Descricao;
                    item.Quantidade = itemTransferencia.PreMovimentoSaidaItem.Quantidade / itemTransferencia.PreMovimentoSaidaItem.ProdutoUnidade.Fator;
                    item.Unidade = itemTransferencia.PreMovimentoSaidaItem.ProdutoUnidade.Descricao;


                    var preMovimentoItemLoteValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                        .AsNoTracking().Include(i => i.LoteValidade)
                        .FirstOrDefault(w => w.EstoquePreMovimentoItemId == preMovimentoItem.Id);

                    if (preMovimentoItemLoteValidade != null && preMovimentoItemLoteValidade.LoteValidade != null)
                    {
                        item.Lote = preMovimentoItemLoteValidade.LoteValidade.Lote;
                        item.Validade = preMovimentoItemLoteValidade.LoteValidade.Validade;

                        if (preMovimentoItemLoteValidade.LoteValidade.EstLaboratorioId != null)
                        {
                            item.Laboratorio = produtoLaboratorioRepository.Object.Get((long)preMovimentoItemLoteValidade.LoteValidade.EstLaboratorioId).Descricao;
                        }
                    }

                    itens.Add(item);
                }

                return new PagedResultDto<MovimentoItemDto>(itens.Count, itens);
            }
        }

        public async Task ExcluirTransferencia(long transferenciaId)
        {
            try
            {
                using (var estoqueTransferenciaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
                using (var estoqueTransferenciaProdutoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProdutoItem, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var transferencia = await estoqueTransferenciaProdutoRepository.Object.GetAsync(transferenciaId);
                    if (transferencia != null)
                    {
                        var transferenciasItens = estoqueTransferenciaProdutoItemRepository.Object.GetAll()
                            .Where(w => w.EstoqueTransferenciaProdutoID == transferenciaId);

                        foreach (var itemTransferencia in transferenciasItens)
                        {
                            await estoqueTransferenciaProdutoItemRepository.Object.DeleteAsync(itemTransferencia)
                                .ConfigureAwait(false);
                        }

                        var preMovimentoSaida =
                            await estoquePreMovimentoRepository.Object.GetAsync(transferencia.PreMovimentoSaidaId);
                        await this.Excluir(EstoquePreMovimentoDto.MapPreMovimento(preMovimentoSaida)).ConfigureAwait(false);

                        var preMovimentoEntrada =
                            await estoquePreMovimentoRepository.Object.GetAsync(transferencia.PreMovimentoEntradaId);
                        await this.Excluir(EstoquePreMovimentoDto.MapPreMovimento(preMovimentoEntrada)).ConfigureAwait(false);

                        await estoqueTransferenciaProdutoRepository.Object.DeleteAsync(transferencia)
                            .ConfigureAwait(false);
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<MovimentoIndexDto>> ListarMovimentosPendente(
            ListarEstoquePreMovimentoInput input)
        {
            var contarEstoquePreMovimentos = 0;
            List<MovimentoIndexDto> EstoquePreMovimentos;
            List<MovimentoIndexDto> EstoquePreMovimentoDtos = new List<MovimentoIndexDto>();
            try
            {
                DateTime PeridoDe = ((DateTime)input.PeridoDe).Date;
                DateTime PeridoAte = ((DateTime)input.PeridoAte).Date.AddDays(1).AddSeconds(-1);
                var estadoPreMovimentoIds = new List<long>
                {
                    (long) EnumPreMovimentoEstado.TotalmenteAtendido, (long) EnumPreMovimentoEstado.TotalmenteSuspenso,
                    (long) EnumPreMovimentoEstado.Conferencia
                };
                using (var _estoquePreMovimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var _usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                {
                    var query = _estoquePreMovimentoRepository.Object.GetAll().AsNoTracking().Where(
                            m => ((input.Filtro == "" || input.Filtro == null)
                                  || m.Documento.ToString().Contains(input.Filtro)
                                  || m.Emissao.ToString().Contains(input.Filtro)
                                  || m.EstTipoOperacao.Descricao.Contains(input.Filtro)
                                  || m.TotalDocumento.ToString().Contains(input.Filtro))
                                 //  && (input.TipoMovimentoId == null || m.TipoMovimentoId == input.TipoMovimentoId)
                                 && ((input.PeridoDe == null || m.Emissao >= PeridoDe)
                                     && (input.PeridoAte == null || m.Emissao <= PeridoAte))
                                 && (input.TipoOperacaoId == null || m.EstTipoOperacaoId == input.TipoOperacaoId)
                                 && !estadoPreMovimentoIds.Contains(m.PreMovimentoEstadoId)
                                 && (m.EstTipoOperacaoId != 2 || (m.EstTipoOperacaoId == 2 && m.IsEntrada)))
                        .Select(
                            s => new MovimentoIndexDto
                            {
                                Id = s.Id,
                                DataEmissaoSaida = s.Emissao,
                                Documento = s.Documento,
                                Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : string.Empty,
                                UsuarioId = s.CreatorUserId,
                                PreMovimentoEstadoId = s.PreMovimentoEstadoId,
                                IsEntrada = s.IsEntrada,
                                TipoMovimento = s.EstTipoOperacao.Descricao,
                                TipoOperacaoId = s.EstTipoOperacaoId,
                                ValorDocumento = s.TotalDocumento
                            });

                    contarEstoquePreMovimentos = await query.CountAsync().ConfigureAwait(false);

                    EstoquePreMovimentos = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync()
                        .ConfigureAwait(false);


                    var idsUsuarios = EstoquePreMovimentos.Select(s => s.UsuarioId);

                    var usuarios = await _usuarioRepository.Object.GetAll().AsNoTracking()
                        .Where(w => idsUsuarios.Any(a => a == w.Id)).ToListAsync();


                    foreach (var preMovimento in EstoquePreMovimentos)
                    {
                        if (preMovimento.UsuarioId == null)
                        {
                            continue;
                        }

                        var usuario = usuarios.FirstOrDefault(w => w.Id == preMovimento.UsuarioId);
                        if (usuario != null)
                        {
                            preMovimento.Usuario = usuario.FullName;
                        }
                    }

                    return new PagedResultDto<MovimentoIndexDto>(contarEstoquePreMovimentos, EstoquePreMovimentos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<EstoquePreMovimentoDto> CriarGetIdDevolucao(EstoquePreMovimentoDto input)
        {
            var retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>();

            input.IsEntrada = true;
            input.Movimento = input.Emissao;
            input.EstTipoOperacaoId = 4;
            input.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Autorizado;

            if (string.IsNullOrEmpty(input.Documento))
            {
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                {
                    input.Documento = ultimoIdAppService.Object.ObterProximoCodigo("DevolucaoProduto").Result;
                }
            }

            retornoPadrao.ReturnObject = CriarGetId(input);

            return retornoPadrao;
        }

        public async Task<PagedResultDto<MovimentoIndexDto>> ListarDevolucoes(ListarEstoquePreMovimentoInput input)
        {
            var contarEstoquePreMovimentos = 0;
            List<MovimentoIndexDto> EstoquePreMovimentos;
            List<MovimentoIndexDto> EstoquePreMovimentoDtos = new List<MovimentoIndexDto>();
            try
            {
                input.PeridoDe = ((DateTime)input.PeridoDe).Date;
                input.PeridoAte = ((DateTime)input.PeridoAte).Date.AddDays(1).AddSeconds(-1);

                using (var estoquePreMovimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                {
                    var query = estoquePreMovimentoRepository.Object.GetAll().AsNoTracking()
                        .Where(
                            m => ((input.Filtro == "" || input.Filtro == null)
                                  || m.Documento.ToString().Contains(input.Filtro)
                                  || m.Empresa.NomeFantasia.Contains(input.Filtro)
                                  || m.Paciente.NomeCompleto.Contains(input.Filtro)
                                  || m.Emissao.ToString().Contains(input.Filtro)
                                  || m.SisFornecedor.Descricao.Contains(input.Filtro)
                                  || m.TotalDocumento.ToString().Contains(input.Filtro)
                                  || m.EstTipoMovimento.Descricao.Contains(input.Filtro))
                                 && (input.FornecedorId == null || m.SisFornecedor.Id == input.FornecedorId)
                                 && (input.PeridoDe == null || m.Emissao >= input.PeridoDe)
                                 && (input.PeridoAte == null || m.Emissao <= input.PeridoAte)
                                 && (input.TipoMovimentoId == null || m.EstTipoMovimento.Id == input.TipoMovimentoId)
                                 && m.EstTipoOperacaoId == (long)EnumTipoOperacao.Devolucao)
                        .Select(s => new MovimentoIndexDto
                        {
                            Id = s.Id,
                            Fornecedor = (s.SisFornecedor != null) ? s.SisFornecedor.Descricao : String.Empty,
                            DataEmissaoSaida = s.Emissao,
                            Documento = s.Documento,
                            Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : String.Empty,
                            Valor = s.TotalDocumento,
                            UsuarioId = s.CreatorUserId,
                            PreMovimentoEstadoId = s.PreMovimentoEstadoId,
                            Estoque = (s.Estoque != null) ? s.Estoque.Descricao : String.Empty,
                            TipoMovimento = (s.EstTipoMovimento != null) ? s.EstTipoMovimento.Descricao : String.Empty
                        });

                    contarEstoquePreMovimentos = await query.CountAsync().ConfigureAwait(false);

                    EstoquePreMovimentos =
                        await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var idsUsuarios = EstoquePreMovimentos.Select(s => s.UsuarioId);
                    var usuarios = usuarioRepository.Object.GetAll().Where(w => idsUsuarios.Any(a => a == w.Id))
                        .ToList();

                    foreach (var preMovimento in EstoquePreMovimentos)
                    {
                        if (preMovimento.UsuarioId != null)
                        {
                            var usuario = usuarios.FirstOrDefault(w => w.Id == preMovimento.UsuarioId);
                            if (usuario != null)
                            {
                                preMovimento.Usuario = usuario.FullName;
                            }
                        }
                    }

                    return new PagedResultDto<MovimentoIndexDto>(contarEstoquePreMovimentos, EstoquePreMovimentos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<EstoquePreMovimentoDto> CriarOuEditarDevolucoes(EstoquePreMovimentoDto input)
        {
            var retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>();
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var query = estoquePreMovimentoItemRepository.Object.GetAll().Where(m => m.PreMovimentoId == input.Id);
                    if (!retornoPadrao.Errors.Any())
                    {
                        var preMovimento = EstoquePreMovimentoDto.MapPreMovimento(input);
                        if (input.Id.Equals(0))
                        {
                            using (var unitOfWork = unitOfWorkManager.Object.Begin())
                            {
                                input.Id = AsyncHelper.RunSync(() => estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimento));

                                unitOfWork.Complete();
                                unitOfWorkManager.Object.Current.SaveChanges();
                                unitOfWork.Dispose();
                            }
                        }
                        else
                        {
                            using (var unitOfWork = unitOfWorkManager.Object.Begin())
                            {
                                var edit = estoquePreMovimentoRepository.Object.Get(input.Id);
                                if (edit != null)
                                {
                                    edit.OrdemId = input.OrdemId;
                                    edit.Quantidade = input.Quantidade;
                                    edit.Serie = input.Serie;
                                    edit.EstTipoMovimentoId = input.TipoDocumentoId;
                                    edit.ValorICMS = input.ValorICMS;
                                    edit.EmpresaId = input.EmpresaId;
                                    edit.SisFornecedorId = input.FornecedorId;
                                    edit.Documento = input.Documento;
                                    edit.Serie = input.Serie;
                                    edit.EstoqueId = input.EstoqueId;

                                    edit.MedicoSolcitanteId = input.MedicoSolcitanteId;
                                    edit.PacienteId = input.PacienteId;
                                    edit.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                                    edit.AtendimentoId = input.AtendimentoId;
                                    edit.Observacao = input.Observacao;

                                    edit.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Autorizado;
                                    edit.EstTipoMovimentoId = input.EstTipoMovimentoId;

                                    edit.Codigo = input.Codigo;

                                    AsyncHelper.RunSync(() => estoquePreMovimentoRepository.Object.UpdateAsync(edit));

                                    unitOfWork.Complete();
                                    unitOfWorkManager.Object.Current.SaveChanges();
                                    unitOfWork.Dispose();
                                }
                            }
                        }

                        retornoPadrao.ReturnObject = input;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return retornoPadrao;
        }

        #region Solicitações

        public async Task<PagedResultDto<MovimentoIndexDto>> ListarSolicitacoes(ListarEstoquePreMovimentoInput input)
        {
            try
            {
                input.PeridoDe = ((DateTime)input.PeridoDe).Date;
                input.PeridoAte = ((DateTime)input.PeridoAte).Date.AddDays(1).AddSeconds(-1);
                using (var estoquePreMovimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                {
                    var emprestimoStatus = new List<long>
                    {
                        (long) EnumTipoMovimento.Emprestimo_Entrada,
                        (long) EnumTipoMovimento.Emprestimo_Saida
                    };

                    var query = estoquePreMovimentoRepository.Object.GetAll()
                        .Where(m => (input.Filtro == "" || input.Filtro == null
                                                        || m.Documento.ToString().Contains(input.Filtro)
                                                        || m.Empresa.NomeFantasia.Contains(input.Filtro)
                                                        || m.Emissao.ToString().Contains(input.Filtro)
                                                        || m.Estoque.Descricao.Contains(input.Filtro)
                                                        || m.TotalDocumento.ToString().Contains(input.Filtro)
                                                        || m.EstTipoMovimento.Descricao.Contains(input.Filtro))
                                    && (input.EstoqueId == null || m.EstoqueId == input.EstoqueId)
                                    && (input.PeridoDe == null || m.Emissao >= input.PeridoDe)
                                    && (input.PeridoAte == null || m.Emissao <= input.PeridoAte)
                                    && (input.StatusMovimentoId == null ||
                                        m.PreMovimentoEstadoId == input.StatusMovimentoId)
                                    && (input.TipoMovimentoId == null || m.EstTipoMovimento.Id == input.TipoMovimentoId)
                                    && m.GrupoOperacaoId == (long)EnumGrupoOperacao.Solicitacao
                                    && (m.EstTipoMovimentoId.HasValue &&
                                        !emprestimoStatus.Contains(m.EstTipoMovimentoId.Value)))
                        .Select(s => new MovimentoIndexDto
                        {
                            Id = s.Id,
                            NomePaciente = (s.PacienteId != null) ? s.Paciente.SisPessoa.NomeCompleto : string.Empty,
                            Fornecedor = (s.SisFornecedor != null) ? s.SisFornecedor.Descricao : string.Empty,
                            DataEmissaoSaida = s.Emissao,
                            Documento = s.Documento,
                            Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : string.Empty,
                            IsEntrada = s.IsEntrada,
                            Valor = s.TotalDocumento,
                            UsuarioId = s.CreatorUserId,
                            PreMovimentoEstadoId = s.PreMovimentoEstadoId,
                            Estoque = (s.Estoque != null) ? s.Estoque.Descricao : string.Empty,
                            TipoMovimento = (s.EstTipoMovimento != null) ? s.EstTipoMovimento.Descricao : string.Empty,
                            TipoOperacao = (s.EstTipoOperacao != null) ? s.EstTipoOperacao.Descricao : string.Empty,
                            HoraPrescrita = s.HoraPrescrita
                        });

                    var contarEstoquePreMovimentos = await query.CountAsync().ConfigureAwait(false);

                    var EstoquePreMovimentos = await query.AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync()
                        .ConfigureAwait(false);


                    var idsUsuarios = EstoquePreMovimentos.Select(s => s.UsuarioId);
                    var usuarios = usuarioRepository.Object.GetAll().Where(w => idsUsuarios.Any(a => a == w.Id))
                        .ToList();

                    foreach (var preMovimento in EstoquePreMovimentos)
                    {
                        if (preMovimento.UsuarioId != null)
                        {
                            var usuario = usuarios.FirstOrDefault(w => w.Id == preMovimento.UsuarioId);
                            if (usuario != null)
                            {
                                preMovimento.Usuario = usuario.FullName;
                            }
                        }
                    }

                    return new PagedResultDto<MovimentoIndexDto>(contarEstoquePreMovimentos, EstoquePreMovimentos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<EstoquePreMovimentoDto> CriarOuEditarSolicitacao(EstoquePreMovimentoDto input)
        {
            var retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>();
            try
            {
                var listTipoContains = new List<long> { (long)EnumTipoMovimento.Emprestimo_Entrada, (long)EnumTipoMovimento.Emprestimo_Saida };
                if (string.IsNullOrEmpty(input.Itens) && !listTipoContains.Contains(input.EstTipoMovimentoId.Value))
                {
                    var itens = JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(input.Itens);
                    if (itens.Count == 0)
                    {
                        retornoPadrao.Errors.Add(ErroDto.Criar("", $"Não é possível {(input.Id.Equals(0) ? "criar" : "editar")} uma solicitação sem itens."));
                    }
                }
            }
            catch (Exception)
            {
                retornoPadrao.Errors.Add(ErroDto.Criar("", $"Não é possível {(input.Id.Equals(0) ? "criar" : "editar")} uma solicitação sem itens."));
            }

            if (retornoPadrao.Errors.Any())
            {
                return retornoPadrao;
            }

            try
            {
                using (var atendimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var estoquePreMovimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var estoqueSolcitacaoItemRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var itens = new List<EstoquePreMovimentoItemSolicitacaoDto>();
                    if (!string.IsNullOrEmpty(input.Itens))
                    {
                        itens = JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(input.Itens);
                    }

                    if (input.Id.Equals(0))
                    {
                        input.CentroCustoId = input.CentroCustoId != 0 ? input.CentroCustoId : null;
                        var preMovimento = EstoquePreMovimentoDto.MapPreMovimento(input);
                        preMovimento.IsEntrada = input.EstTipoOperacaoId != null &&
                                                 input.EstTipoOperacaoId.HasValue &&
                                                 (input.EstTipoOperacaoId == (long)EnumTipoOperacao.Entrada ||
                                                  input.EstTipoOperacaoId == (long)EnumTipoOperacao.Devolucao);
                        preMovimento.Movimento = DateTime.Now;
                        preMovimento.GrupoOperacaoId = (long)EnumGrupoOperacao.Solicitacao;
                        preMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Pendente;
                        preMovimento.MotivoPerdaProdutoId = input.MotivoPerdaProdutoId;


                        if (preMovimento.AtendimentoId != null && preMovimento.AtendimentoId != 0)
                        {
                            var atendimento = atendimentoRepository.Object.Get((long)preMovimento.AtendimentoId);
                            if (atendimento != null)
                            {
                                preMovimento.MedicoSolcitanteId = atendimento.MedicoId;
                                preMovimento.PacienteId = atendimento.PacienteId;
                            }
                        }

                        preMovimento.Documento = ultimoIdAppService.Object.ObterProximoCodigo("Solicitacao")
                            .GetAwaiter().GetResult();

                        input.Id = estoquePreMovimentoRepository.Object.InsertAndGetId(preMovimento);

                        if (!input.Itens.IsNullOrEmpty() && !itens.IsNullOrEmpty())
                        {
                            foreach (var item in itens)
                            {
                                var preMovimentoItem = new EstoqueSolicitacaoItem
                                {
                                    SolicitacaoId = input.Id,
                                    ProdutoId = item.ProdutoId,
                                    ProdutoUnidadeId = (long)item.ProdutoUnidadeId,
                                    Quantidade = item.Quantidade,
                                    EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.Pendente,
                                    EstoqueKitItemId = item.EstoqueKitItemId
                                };
                                estoqueSolcitacaoItemRepository.Object.InsertAndGetId(preMovimentoItem);
                            }
                        }

                        input.Documento = preMovimento.Documento;
                    }
                    else
                    {
                        var edit = estoquePreMovimentoRepository.Object.Get(input.Id);


                        //this.Logger.Warn($"##Solicitação -- Inicio Edição Item  -- {edit.Id}");
                        //this.Logger.Warn($"Itens Input: {input.Itens}");
                        if (edit != null)
                        {
                            edit.EstTipoMovimentoId = input.EstTipoMovimentoId;
                            edit.EmpresaId = input.EmpresaId;
                            edit.SisFornecedorId = input.FornecedorId;
                            edit.Documento = input.Documento;
                            edit.EstoqueId = input.EstoqueId;
                            edit.Emissao = input.Emissao;
                            edit.Observacao = input.Observacao;
                            edit.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                            edit.AtendimentoId = input.AtendimentoId;
                            edit.MedicoSolcitanteId = input.MedicoSolcitanteId;
                            edit.PacienteId = input.PacienteId;
                            edit.MotivoPerdaProdutoId = input.MotivoPerdaProdutoId;

                            edit.Id = estoquePreMovimentoRepository.Object.InsertOrUpdateAndGetId(edit);

                            if (!input.Itens.IsNullOrEmpty())
                            {
                                var ids = itens.Where(x => x.Id != 0).Select(x => x.Id).ToList();
                                var dbItems = estoqueSolcitacaoItemRepository.Object.GetAll()
                                    .Where(x => ids.Contains(x.Id)).ToList();
                                foreach (var item in itens)
                                {
                                    EstoqueSolicitacaoItem preMovimentoItem;

                                    if (item.Id != default(long))
                                    {
                                        preMovimentoItem = dbItems.FirstOrDefault(x => x.Id == item.Id);
                                        if (preMovimentoItem != null)
                                        {
                                            preMovimentoItem.ProdutoId = item.ProdutoId;
                                            preMovimentoItem.ProdutoUnidadeId = (long)item.ProdutoUnidadeId;
                                            preMovimentoItem.Quantidade = item.Quantidade;
                                            preMovimentoItem.EstadoSolicitacaoItemId =
                                                (long)EnumPreMovimentoEstado.Pendente;
                                            preMovimentoItem.EstoqueKitItemId = item.EstoqueKitItemId;
                                        }
                                    }
                                    else
                                    {
                                        preMovimentoItem = new EstoqueSolicitacaoItem
                                        {
                                            //NumeroSerie = item.NumeroSerie,
                                            SolicitacaoId = edit.Id,
                                            ProdutoId = item.ProdutoId,
                                            ProdutoUnidadeId = (long)item.ProdutoUnidadeId,
                                            Quantidade = item.Quantidade,
                                            EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.Pendente,
                                            Id = item.Id,
                                            EstoqueKitItemId = item.EstoqueKitItemId
                                    };
                                    }

                                    if (preMovimentoItem != null)
                                    {
                                        //this.Logger.Warn($"Item GridItemId:{item.IdGrid} : {JsonConvert.SerializeObject(preMovimentoItem)}");
                                        item.Id = estoqueSolcitacaoItemRepository.Object.InsertOrUpdateAndGetId(
                                            preMovimentoItem);
                                    }
                                }

                                var itensMantidos = itens.Where(w => w.Id != 0).Select(x => x.Id).ToList();

                                var itensAtuais = estoqueSolcitacaoItemRepository.Object.GetAll().AsNoTracking()
                                    .Where(w => w.SolicitacaoId == edit.Id).Select(x => x.Id).ToList();

                                foreach (var item in itensAtuais.Where(w => !itensMantidos.Contains(w)))
                                {
                                    estoqueSolcitacaoItemRepository.Object.Delete(item);
                                }
                            }

                            //this.Logger.Warn($"Items Finais: {JsonConvert.SerializeObject(estoqueSolcitacaoItemRepository.Object.GetAll().AsNoTracking().Where(w => w.SolicitacaoId == edit.Id).ToList())}");
                        }

                        //this.Logger.Warn($"##Solicitação -- Final Edição Item  -- {edit.Id}");
                    }

                    unitOfWork.Complete();
                    unitOfWork.Dispose();

                    retornoPadrao.ReturnObject = RetornaPreMovimentoDto(input);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(retornoPadrao, ex);
            }

            return retornoPadrao;
        }

        private static EstoquePreMovimentoDto RetornaPreMovimentoDto(EstoquePreMovimentoDto input)
        {
            using (var estoqueSolcitacaoItemRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
            using (var estoquePreMovimentoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                var preMovimentoSalvo = estoquePreMovimentoRepository.Object.GetAll().AsNoTracking().FirstOrDefault(x => x.Id == input.Id);
                input.EstTipoMovimentoId = preMovimentoSalvo.EstTipoMovimentoId;
                input.EmpresaId = preMovimentoSalvo.EmpresaId;
                input.FornecedorId = preMovimentoSalvo.FornecedorId;
                input.Documento = preMovimentoSalvo.Documento;
                input.EstoqueId = preMovimentoSalvo.EstoqueId;
                input.Emissao = preMovimentoSalvo.Emissao;
                input.Observacao = preMovimentoSalvo.Observacao;
                input.UnidadeOrganizacionalId = preMovimentoSalvo.UnidadeOrganizacionalId;
                input.AtendimentoId = preMovimentoSalvo.AtendimentoId;
                input.MedicoSolcitanteId = preMovimentoSalvo.MedicoSolcitanteId;
                input.PacienteId = preMovimentoSalvo.PacienteId;
                input.MotivoPerdaProdutoId = preMovimentoSalvo.MotivoPerdaProdutoId;
                input.Itens = JsonConvert.SerializeObject(estoqueSolcitacaoItemRepository.Object.GetAll().AsNoTracking().Where(x => x.SolicitacaoId == input.Id).ToList());
                return input;
            }
        }

        public async Task<PagedResultDto<MovimentoIndexDto>> ListarSolicitacaoesPendente(ListarEstoquePreMovimentoInput input)
        {
            const string defaultField = "EstoquePreMovimento.Id";
            const string selectClause = @"
                    EstoquePreMovimento.Id AS Id,
                    EstoquePreMovimento.Emissao AS DataEmissaoSaida,
                    EstoquePreMovimento.Documento AS Documento,
                    EstoquePreMovimento.CreatorUserId AS UsuarioId,
                    EstoquePreMovimento.PreMovimentoEstadoId AS PreMovimentoEstadoId,
                    EstoquePreMovimento.IsEntrada AS IsEntrada,
                    EstoquePreMovimento.EstTipoOperacaoId AS TipoOperacaoId,
                    EstoquePreMovimento.TotalDocumento AS ValorDocumento,
                    EstoquePreMovimento.HoraPrescrita AS HoraPrescrita,
                    TipoMovimento.Descricao AS TipoMovimento,
                    TipoOperacao.Descricao AS TipoOperacao,
                    Pessoa.NomeCompleto As NomePaciente,
                    Empresa.NomeFantasia AS Empresa,
                    Estoque.Descricao AS Estoque,
                    CONCAT(Coalesce(AbpUsers.Name,''), ' ', Coalesce(AbpUsers.Surname,'')) AS Usuario
                ";
            const string fromClause = @"EstoquePreMovimento
                    LEFT JOIN Est_Estoque Estoque ON Estoque.Id = EstoquePreMovimento.EstoqueId AND Estoque.IsDeleted = @IsDeleted
                    LEFT JOIN EstTipoMovimento TipoMovimento ON TipoMovimento.Id = EstoquePreMovimento.EstTipoMovimentoId AND TipoMovimento.IsDeleted = @IsDeleted
                    LEFT JOIN EstTipoOperacao TipoOperacao ON TipoOperacao.Id = EstoquePreMovimento.EstTipoOperacaoId AND TipoOperacao.IsDeleted = @IsDeleted
                    LEFT JOIN SisPaciente Paciente ON Paciente.Id = EstoquePreMovimento.PacienteId AND Paciente.IsDeleted = @IsDeleted
                    LEFT JOIN SisPessoa Pessoa ON Pessoa.Id = Paciente.SisPessoaId AND Pessoa.IsDeleted = @IsDeleted
                    LEFT JOIN SisEmpresa Empresa ON Empresa.Id = EstoquePreMovimento.EmpresaId AND Empresa.IsDeleted = @IsDeleted
                    LEFT JOIN AbpUsers AS AbpUsers ON AbpUsers.Id = EstoquePreMovimento.CreatorUserId AND AbpUsers.IsDeleted = @IsDeleted";
            const string whereClause = "EstoquePreMovimento.IsDeleted = @IsDeleted AND EstGrupoOperacaoId = @GrupoOperacaoId";
            return await this.CreateDataTable<MovimentoIndexDto, ListarEstoquePreMovimentoInput>()
                .AddDefaultField(defaultField)
                .AddSelectClause(selectClause)
                .AddFromClause(fromClause)
                .AddWhereClause(whereClause)
                .AddDefaultErrorMessage(L("ErroPesquisar"))
                .AddWhereMethod(ListarSolicitacaoesPendenteWhereMethod)
                .ExecuteAsync(input);
        }

        private static string ListarSolicitacaoesPendenteWhereMethod(ListarEstoquePreMovimentoInput input, Dictionary<string, object> dapperParameters)
        {
            var queryBuilder = new StringBuilder();
            var emprestimoStatus = new List<long> { (long)EnumTipoMovimento.Emprestimo_Entrada, (long)EnumTipoMovimento.Emprestimo_Saida };
            dapperParameters.Add("IsDeleted", false);
            dapperParameters.Add("GrupoOperacaoId", (long)EnumGrupoOperacao.Solicitacao);
            dapperParameters["PeridoDe"] = ((DateTime)input.PeridoDe).Date.AddSeconds(-1);
            dapperParameters["PeridoAte"] = ((DateTime)input.PeridoAte).Date.AddDays(1).AddSeconds(-1);
            dapperParameters["EmprestimoStatus"] = emprestimoStatus;


            queryBuilder.WhereIf(!input.Filtro.IsNullOrEmpty(),
                "AND ( Documento LIKE '%' + @filtro + '%' OR  Emissao LIKE '%' + @filtro + '%' OR  EstTipoOperacao LIKE '%' + @filtro + '%' OR TotalDocumento LIKE '%' +@filtro + '%' OR Estoque LIKE '%' + @filtro + '%') ");
            queryBuilder.Append(" AND EstTipoMovimentoId IS NOT NULL AND  EstTipoMovimentoId NOT IN @EmprestimoStatus");
            queryBuilder.WhereIf(input.PeridoDe.HasValue, " AND Movimento >= @PeridoDe");
            queryBuilder.WhereIf(input.PeridoAte.HasValue, " AND Movimento <= @PeridoAte");
            queryBuilder.WhereIf(input.TipoMovimentoId.HasValue, " AND EstTipoMovimentoId = @TipoMovimentoId");
            queryBuilder.WhereIf(input.TipoOperacaoId.HasValue, " AND EstTipoOperacaoId = @TipoOperacaoId");
            queryBuilder.WhereIf(!input.Documento.IsNullOrEmpty(), " AND Documento = @Documento");
            queryBuilder.WhereIf(input.EstoqueId.HasValue, " AND EstoquePreMovimento.EstoqueId = @EstoqueId");
            queryBuilder.WhereIf(!input.StatusMovimentoIds.IsNullOrEmpty(), " AND  PreMovimentoEstadoId IN @StatusMovimentoIds");

            return queryBuilder.ToString();
        }


        public async Task<DefaultReturn<EstoquePreMovimentoDto>> AtenderSolicitacao(EstoquePreMovimentoDto preMovimento)
        {
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            using (var estoqueSolicitacaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
            using (var estoqueMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueMovimentoAppService>())
            using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
            using (var tipoOperacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoOperacao, long>>())
            using (var movimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IMovimentoValidacaoDomainService>())
            {
                var itemsErrors = new List<AtenderSolicitacaoErrorInstance>();
                EstoquePreMovimento preMovimentoSolicitacao = null;
                var dependencies = new AtenderSolicitacaoDependencies
                {
                    MovimentoItemRepository = estoqueMovimentoItemRepository.Object,
                    PreMovimentoItemRepository = estoquePreMovimentoItemRepository.Object,
                    ProdutoSaldoDomainService = produtoSaldoDomainService.Object,
                    UnidadeAppService = unidadeAppService.Object,
                    PreMovimentoLoteValidadeRepository = estoquePreMovimentoLoteValidadeRepository.Object,
                };
                try
                {
                    DefaultReturn<EstoquePreMovimentoDto> retornoPadrao;
                    var statusAtendimento = new List<long>
                {
                    (long) EnumPreMovimentoEstado.TotalmenteAtendido,
                    (long) EnumPreMovimentoEstado.TotalmenteSuspenso
                };
                    var itens = JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(preMovimento.Itens);
                    var itemsParaAtualizar = itens.Where(x => !statusAtendimento.Contains(x.EstadoSolicitacaoItemId)
                    && x.QuantidadeAtendida != null && x.QuantidadeAtendida != 0);

                    if(itemsParaAtualizar.IsNullOrEmpty())
                    {
                        retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>
                        {
                            Errors = new List<ErroDto>()
                        };
                        retornoPadrao.Errors.Add(ErroDto.Criar("", "Não existem items para atender"));
                        return retornoPadrao;
                    }

                    //Validação
                    retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>(movimentoValidacaoDomainService.Object.ValidarConfirmacaoSolicitacao(preMovimento));

                    if (retornoPadrao.Errors.Count != 0 || !itemsParaAtualizar.Any())
                    {
                        return retornoPadrao;
                    }

                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        //Persistência

                        preMovimentoSolicitacao = new EstoquePreMovimento
                        {
                            // Id = preMovimento.Id,
                            Documento = preMovimento.Documento,
                            Emissao = preMovimento.Emissao,
                            Movimento = preMovimento.Emissao,
                            EmpresaId = preMovimento.EmpresaId,
                            EstoqueId = preMovimento.EstoqueId,
                            MedicoSolcitanteId = preMovimento.MedicoSolcitanteId,
                            Observacao = preMovimento.Observacao,
                            PacienteId = preMovimento.PacienteId,
                            UnidadeOrganizacionalId = preMovimento.UnidadeOrganizacionalId,
                            AtendimentoId = preMovimento.AtendimentoId,
                            PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Autorizado,
                            IsEntrada = preMovimento.IsEntrada,
                            EstTipoOperacaoId = preMovimento.EstTipoOperacaoId,
                            EstTipoMovimentoId = preMovimento.EstTipoMovimentoId,
                            GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos,
                            EstoquePreMovimentoParentId = preMovimento.Id
                        };

                        var tipoOperacao = await tipoOperacaoRepository.Object.GetAsync(preMovimentoSolicitacao.EstTipoOperacaoId.Value);
                        preMovimentoSolicitacao.Documento = await ultimoIdAppService.Object.ObterProximoCodigo($"{tipoOperacao.Descricao.Capitalize()}Produto");
                        await estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimentoSolicitacao);

                        var solicitacaoAtuais = new List<EstoqueSolicitacaoItem>();
                        foreach (var item in itemsParaAtualizar)
                        {
                            var itemError = new AtenderSolicitacaoErrorInstance();
                            var somaQuantidade = 0m;

                            if (!string.IsNullOrEmpty(item.NumerosSerieJson))
                                somaQuantidade = await InserePorNumeroDeSerie(item, preMovimentoSolicitacao, somaQuantidade, dependencies);
                            else if (item.IsLote)
                                somaQuantidade = await InsereComLoteValidade(item, preMovimentoSolicitacao, somaQuantidade, dependencies);
                            else
                                somaQuantidade = await InsereSemLoteValidade(item, preMovimentoSolicitacao, somaQuantidade, dependencies);

                            var solicitacaoItem = await estoqueSolicitacaoItemRepository.Object.GetAsync(item.Id);

                            itemError.Id = item.Id;
                            itemError.EstadoAnteriorSolicitacaoItemId = solicitacaoItem.EstadoSolicitacaoItemId;
                            itemError.QuantidadeAtendida = solicitacaoItem.QuantidadeAtendida;

                            solicitacaoItem.QuantidadeAtendida = somaQuantidade;

                            if (solicitacaoItem.Quantidade == solicitacaoItem.QuantidadeAtendida)
                            {
                                item.EstadoSolicitacaoItemId = item.EstadoSolicitacaoItemId = solicitacaoItem.EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.TotalmenteAtendido;
                            }
                            else
                            {
                                item.EstadoSolicitacaoItemId = solicitacaoItem.EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.ParcialmenteAtendido;
                            }

                            await estoqueSolicitacaoItemRepository.Object.UpdateAsync(solicitacaoItem);

                            itemsErrors.Add(itemError);
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        var resultMovimentoEntrada = await estoqueMovimentoAppService.Object.GerarMovimentoEntrada(preMovimentoSolicitacao.Id).ConfigureAwait(false);
                        if (resultMovimentoEntrada.Errors.Any())
                        {
                            retornoPadrao.Errors.AddRange(resultMovimentoEntrada.Errors);

                            foreach (var itemError in itemsErrors)
                            {
                                var solicitacaoItem = await estoqueSolicitacaoItemRepository.Object.GetAsync(itemError.Id);
                                solicitacaoItem.EstadoSolicitacaoItemId = itemError.EstadoAnteriorSolicitacaoItemId;
                                solicitacaoItem.QuantidadeAtendida = itemError.QuantidadeAtendida;
                                await estoqueSolicitacaoItemRepository.Object.UpdateAsync(solicitacaoItem);
                                await ExcluiPorNumeroDeSerie(preMovimentoSolicitacao, dependencies);
                                await ExcluiComLoteValidade(preMovimentoSolicitacao, dependencies);
                                await ExcluiSemLoteValidade(preMovimentoSolicitacao, dependencies);

                            }

                            // excluir preMovimento
                            await estoquePreMovimentoRepository.Object.DeleteAsync(preMovimentoSolicitacao);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                            return retornoPadrao;
                        }
                        else
                        {
                            var estoquePreMovimento = await estoquePreMovimentoRepository.Object.GetAsync(preMovimento.Id);
                            if (estoquePreMovimento != null)
                            {
                                estoquePreMovimento.PreMovimentoEstadoId = itens.Count(x => statusAtendimento.Contains(x.EstadoSolicitacaoItemId)) == itens.Count
                                        ? (long)EnumPreMovimentoEstado.TotalmenteAtendido
                                        : (long)EnumPreMovimentoEstado.ParcialmenteAtendido;

                                await estoquePreMovimentoRepository.Object.UpdateAsync(estoquePreMovimento);
                            }

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                            retornoPadrao.ReturnObject = EstoquePreMovimentoDto.MapPreMovimento(preMovimentoSolicitacao);
                            return retornoPadrao;
                        }
                    }
                }
                catch (Exception e)
                {
                    foreach (var itemError in itemsErrors)
                    {
                        var solicitacaoItem = await estoqueSolicitacaoItemRepository.Object.GetAsync(itemError.Id);
                        solicitacaoItem.EstadoSolicitacaoItemId = itemError.EstadoAnteriorSolicitacaoItemId;
                        solicitacaoItem.QuantidadeAtendida = itemError.QuantidadeAtendida;
                        await estoqueSolicitacaoItemRepository.Object.UpdateAsync(solicitacaoItem);
                        if (preMovimentoSolicitacao != null)
                        {
                            await ExcluiPorNumeroDeSerie(preMovimentoSolicitacao, dependencies);
                            await ExcluiComLoteValidade(preMovimentoSolicitacao, dependencies);
                            await ExcluiSemLoteValidade(preMovimentoSolicitacao, dependencies);
                        }
                    }
                    // excluir preMovimento
                    if (preMovimentoSolicitacao != null)
                    {
                        await estoquePreMovimentoRepository.Object.DeleteAsync(preMovimentoSolicitacao);
                    }
                    this.Logger.Error("ERRO AO ATENDER SOLICITACAO", e);
                    return new DefaultReturn<EstoquePreMovimentoDto> { Errors = new List<ErroDto> { ErroDto.Criar("", "Erro ao atender a solicitação favor entrar em contato com o suporte.") } };
                }
            }
        }

        public class AtenderSolicitacaoErrorInstance
        {
            public long Id { get; set; }
            public long EstadoAnteriorSolicitacaoItemId { get; set; }
            public decimal QuantidadeAtendida { get; set; }
        }

        

        public async Task<DefaultReturn<DocumentoDto>> ExcluirSolicitacoesPrescritasNaoAtendidas(long prescricaoMedicaId)
        {
            var retornoPadrao = new DefaultReturn<DocumentoDto>();
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var preMovimentosPendentes = estoquePreMovimentoRepository.Object.GetAll().Where(
                        w => w.PrescricaoMedicaId == prescricaoMedicaId
                             && w.PreMovimentoEstadoId == (long)EnumPreMovimentoEstado.Pendente).ToList();


                    var preMovimentosTotais = estoquePreMovimentoRepository.Object.GetAll()
                        .Where(w => w.PrescricaoMedicaId == prescricaoMedicaId).ToList();

                    foreach (var preMovimento in preMovimentosPendentes)
                    {
                        preMovimento.PreMovimentoEstadoId = (preMovimentosPendentes.Count == preMovimentosTotais.Count)
                            ? (long)EnumPreMovimentoEstado.TotalmenteSuspenso
                            : (long)EnumPreMovimentoEstado.ParcialmentoSuspenso;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(retornoPadrao, ex);
            }

            return retornoPadrao;
        }

        public async Task<DefaultReturn<DocumentoDto>> ExcluirSolicitacoesPrescritasNaoAtendidasPorItemResposta(
            long prescricaoItemRespostaId)
        {
            var retornoPadrao = new DefaultReturn<DocumentoDto>();

            try
            {
                using (var estoquePreMovimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var preMovimentosPendentes = estoquePreMovimentoRepository.Object.GetAll().Where(
                        w => w.PrescricaoItemRespostaId == prescricaoItemRespostaId
                             && w.PreMovimentoEstadoId == (long)EnumPreMovimentoEstado.Pendente).ToList();


                    var preMovimentosTotais = estoquePreMovimentoRepository.Object.GetAll()
                        .Where(w => w.PrescricaoItemRespostaId == prescricaoItemRespostaId).ToList();

                    foreach (var preMovimento in preMovimentosPendentes)
                    {
                        preMovimento.PreMovimentoEstadoId = (preMovimentosPendentes.Count == preMovimentosTotais.Count)
                            ? (long)EnumPreMovimentoEstado.TotalmenteSuspenso
                            : (long)EnumPreMovimentoEstado.ParcialmentoSuspenso;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(retornoPadrao, ex);
            }

            return retornoPadrao;
        }


        public async Task<DefaultReturn<EstoquePreMovimento>> ReAtivarSolicitacoDePrescricaoMedica(
            long prescricaoMedicaId)
        {
            var retornoPadrao = new DefaultReturn<EstoquePreMovimento>();

            try
            {
                using (var estoquePreMovimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var preMovimentosPendentes = estoquePreMovimentoRepository.Object.GetAll().Where(
                            w => w.PrescricaoMedicaId == prescricaoMedicaId
                                 && (w.PreMovimentoEstadoId == (long)EnumPreMovimentoEstado.ParcialmentoSuspenso
                                     || w.PreMovimentoEstadoId == (long)EnumPreMovimentoEstado.TotalmenteSuspenso))
                        .ToList();


                    retornoPadrao.ReturnObject = preMovimentosPendentes.FirstOrDefault();

                    foreach (var preMovimento in preMovimentosPendentes)
                    {
                        preMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Pendente;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(retornoPadrao, ex);
            }

            return retornoPadrao;
        }

        public async Task<DefaultReturn<EstoquePreMovimento>> ReAtivarSolicitacoDePrescricaoItemResposta(
            long prescricaoItemRespostaId)
        {
            var retornoPadrao = new DefaultReturn<EstoquePreMovimento>();
            try
            {
                using (var estoquePreMovimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var preMovimentosPendentes = estoquePreMovimentoRepository.Object.GetAll().Where(
                            w => w.PrescricaoItemRespostaId == prescricaoItemRespostaId
                                 && (w.PreMovimentoEstadoId == (long)EnumPreMovimentoEstado.ParcialmentoSuspenso
                                     || w.PreMovimentoEstadoId == (long)EnumPreMovimentoEstado.TotalmenteSuspenso))
                        .ToList();

                    retornoPadrao.ReturnObject = preMovimentosPendentes.FirstOrDefault();

                    foreach (var preMovimento in preMovimentosPendentes)
                    {
                        preMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Pendente;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(retornoPadrao, ex);
            }

            return retornoPadrao;
        }


        public async Task<IResultDropdownList<long>> ListarDropdownPreMovimentoEstado(DropdownInput dropdownInput)
        {
            using (var estoquePreMovimentoEstadoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoEstado, long>>())
            {
                return await this.CreateSelect2(estoquePreMovimentoEstadoRepository.Object).ExecuteAsync(dropdownInput)
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<MovimentoIndexDto>> BuscarPorPrescricaoMedica(string prescricaoMedicaId)
        {
            using (var preMovimentoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                long id;
                if (!long.TryParse(prescricaoMedicaId, out id))
                {
                    return null;
                }

                return await preMovimentoRepository.Object.GetAll()
                    .Where(x => x.PrescricaoMedicaId == id)
                    .Select(
                        s => new MovimentoIndexDto
                        {
                            Id = s.Id,
                            Fornecedor = (s.SisFornecedor != null) ? s.SisFornecedor.SisPessoa.FisicaJuridica == "F" ? s.SisFornecedor.SisPessoa.NomeCompleto : s.SisFornecedor.SisPessoa.NomeFantasia : string.Empty,
                            DataEmissaoSaida = s.Emissao,
                            Documento = s.Documento,
                            Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : string.Empty,
                            Valor = s.TotalDocumento,
                            UsuarioId = s.CreatorUserId,
                            PreMovimentoEstadoId = s.PreMovimentoEstadoId,
                            Estoque = (s.Estoque != null) ? s.Estoque.Descricao : string.Empty,
                            TipoMovimento = (s.EstTipoMovimento != null) ? s.EstTipoMovimento.Descricao : string.Empty
                        })
                    .ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task<MovimentoIndexDto> BuscarPorSolicitacao(string solicitacaoId)
        {
            using (var preMovimentoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                long id;
                if (!long.TryParse(solicitacaoId, out id))
                {
                    return null;
                }

                return await preMovimentoRepository.Object.GetAll()
                    .Where(x => x.Id == id)
                    .Select(
                        s => new MovimentoIndexDto
                        {
                            Id = s.Id,
                            Fornecedor = (s.SisFornecedor != null) ? s.SisFornecedor.SisPessoa.FisicaJuridica == "F" ? s.SisFornecedor.SisPessoa.NomeCompleto : s.SisFornecedor.SisPessoa.NomeFantasia : string.Empty,
                            DataEmissaoSaida = s.Emissao,
                            Documento = s.Documento,
                            Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : string.Empty,
                            Valor = s.TotalDocumento,
                            UsuarioId = s.CreatorUserId,
                            PreMovimentoEstadoId = s.PreMovimentoEstadoId,
                            Estoque = (s.Estoque != null) ? s.Estoque.Descricao : string.Empty,
                            TipoMovimento = (s.EstTipoMovimento != null) ? s.EstTipoMovimento.Descricao : string.Empty
                        }).FirstOrDefaultAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #region Emprestimos

        public async Task<PagedResultDto<MovimentoIndexDto>> ListarSolicitacoesEmprestimos(ListarEstoquePreMovimentoInput input)
        {
            try
            {
                var labelEmprestimoSaida = "Empréstimo Concedido";
                var labelEmprestimoEntrada = "Empréstimo Recebido";
                input.PeridoDe = ((DateTime)input.PeridoDe).Date;
                input.PeridoAte = ((DateTime)input.PeridoAte).Date.AddDays(1).AddSeconds(-1);
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                {
                    var query = estoquePreMovimentoRepository.Object.GetAll()
                        .Where(m => m.GrupoOperacaoId == (long)EnumGrupoOperacao.Solicitacao
                                    && (input.PeridoDe == null || m.Emissao >= input.PeridoDe)
                                    && (input.PeridoAte == null || m.Emissao <= input.PeridoAte)
                                    && (
                                            m.EstTipoMovimentoId == (long)EnumTipoMovimento.Emprestimo_Entrada ||
                                            m.EstTipoMovimentoId == (long)EnumTipoMovimento.Emprestimo_Saida
                                       )
                                    && (input.TipoOperacaoId == null || m.EstTipoOperacaoId == input.TipoOperacaoId)
                              )
                        .WhereIf(input.Filtro != "" && input.Filtro != null, m =>
                            m.Documento.ToString().Contains(input.Filtro)
                            || m.Empresa.NomeFantasia.Contains(input.Filtro)
                            || m.Emissao.ToString().Contains(input.Filtro)
                            || m.Estoque.Descricao.Contains(input.Filtro)
                            || m.TotalDocumento.ToString().Contains(input.Filtro)
                            || m.EstTipoMovimento.Descricao.Contains(input.Filtro))
                        .WhereIf(input.EstoqueId != null, m => m.EstoqueId == input.EstoqueId)
                        .WhereIf(input.StatusMovimentoId != null,
                            m => m.PreMovimentoEstadoId == input.StatusMovimentoId)
                        .Select(s => new MovimentoIndexDto
                        {
                            Id = s.Id,
                            NomePaciente = (s.PacienteId != null) ? s.Paciente.SisPessoa.NomeCompleto : string.Empty,
                            Fornecedor = (s.SisFornecedor != null) ? s.SisFornecedor.Descricao : string.Empty,
                            DataEmissaoSaida = s.Emissao,
                            Documento = s.Documento,
                            Empresa = s.EstoqueEmprestimo.SisPessoa.NomeFantasia,
                            IsEntrada = s.IsEntrada,
                            Valor = s.TotalDocumento,
                            UsuarioId = s.CreatorUserId,
                            PreMovimentoEstadoId = s.PreMovimentoEstadoId,
                            Estoque = (s.Estoque != null) ? s.Estoque.Descricao : string.Empty,
                            TipoMovimento = (s.EstTipoMovimento.Id == (long)EnumTipoMovimento.Emprestimo_Entrada) ?
                                labelEmprestimoEntrada : labelEmprestimoSaida,
                            TipoOperacao = (s.EstTipoOperacao != null) ? s.EstTipoOperacao.Descricao : string.Empty,
                            HoraPrescrita = s.HoraPrescrita
                        });

                    var contarEstoquePreMovimentos = await query.CountAsync().ConfigureAwait(false);

                    var EstoquePreMovimentos = await query.AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    var idsUsuarios = EstoquePreMovimentos.Select(s => s.UsuarioId);
                    var usuarios = usuarioRepository.Object.GetAll()
                        .Where(w => idsUsuarios.Any(a => a == w.Id))
                        .ToList();

                    foreach (var preMovimento in EstoquePreMovimentos)
                    {
                        if (preMovimento.UsuarioId != null)
                        {
                            var usuario = usuarios.FirstOrDefault(w => w.Id == preMovimento.UsuarioId);
                            if (usuario != null)
                            {
                                preMovimento.Usuario = usuario.FullName;
                            }
                        }
                    }

                    return new PagedResultDto<MovimentoIndexDto>(contarEstoquePreMovimentos, EstoquePreMovimentos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarEmprestimoEntrada(
            EstoquePreMovimentoDto input)
        {
            input.IsEntrada = true;
            input.Movimento = input.Emissao;
            input.EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Entrada;
            input.EstTipoOperacaoId = (long)EnumTipoOperacao.Entrada;
            return this.CriarOuEditarSolicitacao(input);
        }

        public async Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarEmprestimoSaida(
            EstoquePreMovimentoDto input)
        {
            input.IsEntrada = false;
            input.Movimento = input.Emissao;
            input.EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Saida;
            input.EstTipoOperacaoId = (long)EnumTipoOperacao.Saida;
            return this.CriarOuEditarSolicitacao(input);
        }

        #endregion

        #region Metodos auxiliares

        private static async Task<decimal> InserePorNumeroDeSerie(EstoquePreMovimentoItemSolicitacaoDto item, EstoquePreMovimento preMovimentoSolicitacao, decimal somaQuantidade, AtenderSolicitacaoDependencies dependencies)
        {
            if (string.IsNullOrEmpty(item.NumerosSerieJson))
            {
                return somaQuantidade;
            }

            var numerosSerie = JsonConvert.DeserializeObject<List<NumeroSerieGridDto>>(item.NumerosSerieJson);

            foreach (var numeroSerie in numerosSerie)
            {
                var movimentoItem = new EstoquePreMovimentoItem
                {
                    PreMovimentoId = preMovimentoSolicitacao.Id,
                    ProdutoId = item.ProdutoId,
                    ProdutoUnidadeId = item.ProdutoUnidadeId,
                    Quantidade = 1,
                    NumeroSerie = numeroSerie.NumeroSerie
                };

                movimentoItem.Id = await dependencies.PreMovimentoItemRepository.InsertAndGetIdAsync(movimentoItem);

                dependencies.ProdutoSaldoDomainService.AtualizarSaldoPreMovimentoItemPreMovimento(preMovimentoSolicitacao, movimentoItem);
            }

            somaQuantidade = numerosSerie.Count;
            return somaQuantidade;
        }

        private static async Task ExcluiPorNumeroDeSerie(EstoquePreMovimento preMovimentoSolicitacao, AtenderSolicitacaoDependencies dependencies)
        {
            var movimentoItems = await dependencies.PreMovimentoItemRepository.GetAll().Where(x => x.PreMovimentoId == preMovimentoSolicitacao.Id).ToListAsync();

            foreach(var movimentoItem in movimentoItems)
            {
                await dependencies.PreMovimentoItemRepository.DeleteAsync(movimentoItem);
                dependencies.ProdutoSaldoDomainService.AtualizarSaldoPreMovimentoItemPreMovimento(preMovimentoSolicitacao, movimentoItem);
            }
        }


        private static async Task<decimal> InsereComLoteValidade(EstoquePreMovimentoItemSolicitacaoDto item, EstoquePreMovimento preMovimentoSolicitacao, decimal somaQuantidade, AtenderSolicitacaoDependencies dependencies)
        {
            if (!string.IsNullOrEmpty(item.NumerosSerieJson))
            {
                return somaQuantidade;
            }

            somaQuantidade = dependencies.MovimentoItemRepository.GetAll()
                .Where(w => w.EstoquePreMovimentoItemId == item.Id)
                .Select(s => s.Quantidade).ToList().Sum();

            var movimentoItem = new EstoquePreMovimentoItem
            {
                PreMovimentoId = preMovimentoSolicitacao.Id,
                ProdutoId = item.ProdutoId,
                ProdutoUnidadeId = item.ProdutoUnidadeId,
                Quantidade = dependencies.UnidadeAppService.ObterQuantidadeReferencia((long)item.ProdutoUnidadeId, (decimal)item.QuantidadeAtendida)
            };
            movimentoItem.Id = await dependencies.PreMovimentoItemRepository.InsertAndGetIdAsync(movimentoItem);

            var lotesValidades = JsonConvert.DeserializeObject<List<LoteValidadeGridDto>>(item.LotesValidadesJson, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            if (lotesValidades != null)
            {
                foreach (var itemLoteValidade in lotesValidades.Where(x => x.Id.Equals(0)))
                {
                    var movimentoLoteValidade = new EstoquePreMovimentoLoteValidade
                    {
                        EstoquePreMovimentoItemId = movimentoItem.Id,
                        LoteValidadeId = itemLoteValidade.LoteValidadeId,
                        Quantidade = dependencies.UnidadeAppService.ObterQuantidadeReferencia((long)item.ProdutoUnidadeId, (decimal)itemLoteValidade.Quantidade)
                    };

                    movimentoLoteValidade.Id = await dependencies.PreMovimentoLoteValidadeRepository.InsertAndGetIdAsync(movimentoLoteValidade);
                    dependencies.ProdutoSaldoDomainService.AtualizarSaldoPreMovimentoItemLoteValidade(EstoqueLoteValidadeAppService.MapLoteValidade(movimentoLoteValidade));
                }
                somaQuantidade = lotesValidades.Sum(x => x.Quantidade.Value);
            }

            return somaQuantidade;
        }


        private static async Task ExcluiComLoteValidade(EstoquePreMovimento preMovimentoSolicitacao, AtenderSolicitacaoDependencies dependencies)
        {
            
            var movimentoItems = await dependencies.PreMovimentoItemRepository.GetAll().Where(x => x.PreMovimentoId == preMovimentoSolicitacao.Id).ToListAsync();

            foreach (var movimentoItem in movimentoItems)
            {
                await dependencies.PreMovimentoItemRepository.DeleteAsync(movimentoItem);
                var movimentoLoteValidades = await dependencies.PreMovimentoLoteValidadeRepository.GetAll().Where(x => x.EstoquePreMovimentoItemId == movimentoItem.Id).ToListAsync();

                foreach(var movimentoLoteValidade in movimentoLoteValidades)
                {
                    await dependencies.PreMovimentoLoteValidadeRepository.DeleteAsync(movimentoLoteValidade);
                    movimentoLoteValidade.Quantidade *= -1;
                    dependencies.ProdutoSaldoDomainService.AtualizarSaldoPreMovimentoItemLoteValidade(EstoqueLoteValidadeAppService.MapLoteValidade(movimentoLoteValidade));
                }
            }
        }

        private static async Task<decimal> InsereSemLoteValidade(EstoquePreMovimentoItemSolicitacaoDto item, EstoquePreMovimento preMovimentoSolicitacao, decimal somaQuantidade, AtenderSolicitacaoDependencies dependencies)
        {
            if (item.IsLote) return somaQuantidade;

            var qtdMovimento = item.QuantidadeAtendida.Value - (item.QuantidadeSolicitada - item.Quantidade);

            var preMovimentoItem = new EstoquePreMovimentoItem
            {
                PreMovimentoId = preMovimentoSolicitacao.Id,
                ProdutoId = item.ProdutoId,
                ProdutoUnidadeId = item.ProdutoUnidadeId,
                Quantidade = dependencies.UnidadeAppService.ObterQuantidadeReferencia((long)item.ProdutoUnidadeId, qtdMovimento)
            };


            await dependencies.PreMovimentoItemRepository.InsertAndGetIdAsync(preMovimentoItem);

            return item.QuantidadeAtendida.Value;
        }

        private static async Task ExcluiSemLoteValidade(EstoquePreMovimento preMovimentoSolicitacao, AtenderSolicitacaoDependencies dependencies)
        {
            var movimentoItems = await dependencies.PreMovimentoItemRepository.GetAll().Where(x => x.PreMovimentoId == preMovimentoSolicitacao.Id).ToListAsync();

            foreach (var movimentoItem in movimentoItems)
            {
                await dependencies.PreMovimentoItemRepository.DeleteAsync(movimentoItem);
                dependencies.ProdutoSaldoDomainService.AtualizarSaldoPreMovimentoItemPreMovimento(preMovimentoSolicitacao, movimentoItem);
            }
        }

        private class AtenderSolicitacaoDependencies
        {
            public IRepository<EstoqueMovimentoItem, long> MovimentoItemRepository { get; set; }
            public IRepository<EstoquePreMovimentoItem, long> PreMovimentoItemRepository { get; set; }

            public IProdutoSaldoDomainService ProdutoSaldoDomainService { get; set; }

            public IUnidadeAppService UnidadeAppService { get; set; }

            public IRepository<EstoquePreMovimentoLoteValidade, long> PreMovimentoLoteValidadeRepository { get; set; }
        }

        private static async Task<DefaultReturn<DocumentoDto>> GerarContasPagar(EstoquePreMovimentoDto preMovimento)
        {
            var documento = new DocumentoDto
            {
                DataEmissao = preMovimento.Emissao,
                EmpresaId = (long)preMovimento.EmpresaId,
                ForncedorId = preMovimento.FornecedorId,
                Numero = preMovimento.Documento,
                TipoDocumentoId = 1, //TODO TIPO DOCUMENTO 
                ValorDocumento = preMovimento.TotalDocumento,
                ValorTotalParcelas = preMovimento.TotalDocumento,
                LancamentosJson = preMovimento.LancamentosJson,
                PreMovimentoId = preMovimento.Id
            };

            using (var contasPagarAppService = IocManager.Instance.ResolveAsDisposable<IContasPagarAppService>())
            using (var fornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
            {
                documento.RateioJson = GerarRateio(preMovimento);

                var fornecedor = await fornecedorRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(w => w.Id == preMovimento.FornecedorId);

                if (fornecedor != null)
                {
                    documento.PessoaId = fornecedor.SisPessoaId;
                    var documentoExiste = contasPagarAppService.Object.ObterPorPessoaNumero((long)documento.PessoaId, documento.Numero);
                    if (documentoExiste != null)
                    {
                        documento.Id = documentoExiste.Id;
                    }
                }

                var retornoContasPagar = contasPagarAppService.Object.CriarOuEditar(documento);

                return retornoContasPagar;
            }
        }

        private static string GerarRateio(EstoquePreMovimentoDto preMovimento)
        {
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            {
                var itens = estoquePreMovimentoItemRepository.Object.GetAll()
                    .Include(x => x.Produto)
                    .Include(x => x.ProdutoUnidade)
                    .AsNoTracking().Where(w => w.PreMovimentoId == preMovimento.Id)
                    .ToList();
                if (!itens.Any())
                {
                    return null;
                }

                var contasAdministrativas = itens.GroupBy(g => g.Produto.ContaAdministrativaId).Select(
                    s => new
                    {
                        ContaId = s.First().Produto.ContaAdministrativaId,
                        Soma = s.Sum(soma =>
                            (soma.CustoUnitario * (soma.Quantidade / soma.ProdutoUnidade.Fator) + soma.ValorIPI +
                             PreMovimentoValidacaoService.CalculaValorICMS(preMovimento, soma.ValorICMS)))
                    }).Distinct().ToList();

                var rateios = contasAdministrativas.Select(item => new DocumentoRateioIndex
                {
                    CentroCustoId = preMovimento.CentroCustoId,
                    EmpresaId = preMovimento.EmpresaId,
                    ContaAdministrativaId = item.ContaId,
                    Valor = item.Soma + (preMovimento.ValorAcrescimo - preMovimento.ValorDesconto + (preMovimento.ValorFrete ?? 0)) / contasAdministrativas.Count
                }).ToList();

                return JsonConvert.SerializeObject(rateios);

            }
        }

        private static string ObterTipoMovimentacao(MovimentoIndexDto preMovimento)
        {
            using (var estoqueTransferenciaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
            {
                var transferencia = estoqueTransferenciaProdutoRepository.Object.GetAll().AsNoTracking().FirstOrDefault(
                    w => (w.PreMovimentoEntradaId == preMovimento.Id || w.PreMovimentoSaidaId == preMovimento.Id));

                if (transferencia != null)
                {
                    preMovimento.Id = transferencia.Id;
                    return "Transferência";
                }
                else
                {
                    return preMovimento.IsEntrada ? "Entrada" : "Saída";
                }
            }
        }

        #endregion

        #region Relatorio

        public RelatorioEntradaModelDto ObterDadosRelatorioEntrada(long preMovimentoId)
        {
            RelatorioEntradaModelDto relatorioEntradaModelDto = new RelatorioEntradaModelDto();
            relatorioEntradaModelDto.Itens = new List<RelatorioEntradaItemModalDto>();

            decimal totalItens = 0;
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            using (var estoquePreMovimentoItemRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance
                .ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            {
                var loginInformations = Task.Run(() => sessionAppService.Object.GetCurrentLoginInformations()).Result;


                var itens = estoquePreMovimentoItemRepository.Object.GetAll()
                    .Where(w => w.PreMovimentoId == preMovimentoId)
                    .Include(i => i.Produto).Include(i => i.ProdutoUnidade).Include(i => i.EstoquePreMovimento.Empresa)
                    .Include(i => i.EstoquePreMovimento.SisFornecedor)
                    .Include(i => i.EstoquePreMovimento.SisFornecedor.SisPessoa)
                    .Include(i => i.EstoquePreMovimento.Estoque).Include(i => i.EstoquePreMovimento.EstTipoMovimento)
                    .Include(i => i.EstoquePreMovimento).Include(i => i.EstoquePreMovimento.Paciente)
                    .Include(i => i.EstoquePreMovimento.Paciente.SisPessoa)
                    .Include(i => i.EstoquePreMovimento.UnidadeOrganizacional).ToList();

                relatorioEntradaModelDto.IsEntrada = itens[0].EstoquePreMovimento.IsEntrada;
                if (relatorioEntradaModelDto.IsEntrada)
                {
                    relatorioEntradaModelDto.Titulo = string.Concat("Entrada de Produtos");
                }
                else
                {
                    relatorioEntradaModelDto.Titulo = string.Concat("Saída de Produtos");
                }

                relatorioEntradaModelDto.NomeUsuario = string.Concat(
                    loginInformations.User.Name,
                    " ",
                    loginInformations.User.Surname);
                relatorioEntradaModelDto.DataHora = Convert.ToString(DateTime.Now);

                if (itens.Count > 0)
                {
                    relatorioEntradaModelDto.NomeHospital =
                        itens[0].EstoquePreMovimento != null
                            ? itens[0].EstoquePreMovimento.Empresa.NomeFantasia.ToString()
                            : string.Empty;

                    relatorioEntradaModelDto.Fornecedor = itens[0].EstoquePreMovimento.SisFornecedor != null
                        ? itens[0].EstoquePreMovimento.SisFornecedor.SisPessoa
                            .FisicaJuridica == "F"
                            ? itens[0].EstoquePreMovimento.SisFornecedor.SisPessoa
                                .NomeCompleto
                            : itens[0].EstoquePreMovimento.SisFornecedor
                                .SisPessoa.NomeFantasia
                        : string.Empty;
                    relatorioEntradaModelDto.Documento = itens[0].EstoquePreMovimento.Documento;
                    relatorioEntradaModelDto.TipoEntrada = itens[0].EstoquePreMovimento.EstTipoMovimento.Descricao;

                    relatorioEntradaModelDto.ValorFrete = string.Format(
                        "{0:C}",
                        itens[0].EstoquePreMovimento.ValorFrete);
                    relatorioEntradaModelDto.ValorTotal = string.Format(
                        "{0:C}",
                        itens[0].EstoquePreMovimento.TotalDocumento);

                    if (itens[0].EstoquePreMovimento.SisFornecedor != null)
                    {
                        if (itens[0].EstoquePreMovimento.SisFornecedor.SisPessoa.FisicaJuridica == "F")
                        {
                            relatorioEntradaModelDto.CNPJFornecedor =
                                itens[0].EstoquePreMovimento.SisFornecedor.SisPessoa.Cpf;
                        }
                        else
                        {
                            relatorioEntradaModelDto.CNPJFornecedor =
                                itens[0].EstoquePreMovimento.SisFornecedor.SisPessoa.Cnpj;
                        }
                    }

                    relatorioEntradaModelDto.Estoque = itens[0].EstoquePreMovimento.Estoque.Descricao;
                    relatorioEntradaModelDto.DataEntrada = string.Format(
                        "{0:dd/MM/yyyy}",
                        itens[0].EstoquePreMovimento.Movimento);


                    if (itens[0].EstoquePreMovimento.CreatorUserId != null
                        && itens[0].EstoquePreMovimento.CreatorUserId != 0)
                    {
                        var usuario = usuarioRepository.Object.Get((long)itens[0].EstoquePreMovimento.CreatorUserId);
                        if (usuario != null)
                        {
                            relatorioEntradaModelDto.UsuarioEntrada = string.Concat(usuario.Name, " ", usuario.Surname);
                        }
                    }

                    if (itens[0].EstoquePreMovimento.Paciente != null)
                    {
                        relatorioEntradaModelDto.Paciente = itens[0].EstoquePreMovimento.Paciente.NomeCompleto;
                    }

                    if (itens[0].EstoquePreMovimento.UnidadeOrganizacional != null)
                    {
                        relatorioEntradaModelDto.Setor = itens[0].EstoquePreMovimento.UnidadeOrganizacional.Descricao;
                    }
                }

                foreach (var item in itens)
                {
                    totalItens += (item.CustoUnitario * (item.Quantidade / item.ProdutoUnidade.Fator));

                    if (item.Produto.IsLote || item.Produto.IsValidade)
                    {
                        var lotesValidades = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                            .Where(w => w.EstoquePreMovimentoItemId == item.Id).Include(i => i.LoteValidade).ToList();

                        foreach (var loteValidade in lotesValidades)
                        {
                            relatorioEntradaModelDto.Itens.Add(
                                new RelatorioEntradaItemModalDto
                                {
                                    CodigoProduto = item.Produto.Codigo,
                                    DescricaoProduto = item.Produto.Descricao,
                                    Lote = loteValidade.LoteValidade.Lote,
                                    Validade = String.Format("{0:dd/MM/yyyy}", loteValidade.LoteValidade.Validade),
                                    Quantidade =
                                        string.Format(
                                            "{0:0.0000}",
                                            (loteValidade.Quantidade / item.ProdutoUnidade.Fator)),
                                    Sigla = item.ProdutoUnidade.Sigla,
                                    ValorUnitario = item.CustoUnitario.ToString(),
                                    ValorIPI =
                                        ((item.CustoUnitario * (item.Quantidade / item.ProdutoUnidade.Fator) / 100)
                                         * item.PerIPI).ToString(),
                                    ValorTotal =
                                        (item.CustoUnitario * (item.Quantidade / item.ProdutoUnidade.Fator))
                                        .ToString()
                                });
                        }
                    }
                    else
                    {
                        relatorioEntradaModelDto.Itens.Add(
                            new RelatorioEntradaItemModalDto
                            {
                                CodigoProduto = item.Produto.Codigo,
                                DescricaoProduto = item.Produto.Descricao,
                                Quantidade =
                                    string.Format("{0:0.00}", (item.Quantidade / item.ProdutoUnidade.Fator)),
                                Sigla = item.ProdutoUnidade.Sigla,
                                ValorUnitario = item.CustoUnitario.ToString(),
                                ValorIPI =
                                    ((item.CustoUnitario * (item.Quantidade / item.ProdutoUnidade.Fator) / 100)
                                     * item.PerIPI).ToString(),
                                ValorTotal = (item.CustoUnitario * (item.Quantidade / item.ProdutoUnidade.Fator))
                                    .ToString()
                            });
                    }
                }

                relatorioEntradaModelDto.TotalItens = string.Format("{0:C}", totalItens);


                return relatorioEntradaModelDto;
            }
        }


        public RelatorioSolicitacaoSaidaModelDto ObterDadosRelatorioSolicitacao(long solicitacaoId)
        {
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            using (var estoqueSolcitacaiItemRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
            {
                var relatorioSolicitacaoSaidaModelDto = new RelatorioSolicitacaoSaidaModelDto
                { Itens = new List<RelatorioSolicitacaoSaidaItemModelDto>() };

                var loginInformations = Task.Run(() => sessionAppService.Object.GetCurrentLoginInformations()).Result;

                var itens = estoqueSolcitacaiItemRepository.Object.GetAll().Where(w => w.SolicitacaoId == solicitacaoId)
                    .Include(i => i.Produto).Include(i => i.ProdutoUnidade).Include(i => i.Solicitacao.Empresa)
                    .Include(i => i.Solicitacao.Estoque).Include(i => i.Solicitacao)
                    .Include(i => i.Solicitacao.Paciente).Include(i => i.Solicitacao.Paciente.SisPessoa)
                    .Include(i => i.Solicitacao.UnidadeOrganizacional).Include(i => i.Solicitacao.MedicoSolicitante)
                    .Include(i => i.Solicitacao.MedicoSolicitante.SisPessoa).ToList();

                relatorioSolicitacaoSaidaModelDto.Titulo = "Solicitação de Saída";

                relatorioSolicitacaoSaidaModelDto.NomeUsuario = string.Concat(
                    loginInformations.User.Name,
                    " ",
                    loginInformations.User.Surname);
                relatorioSolicitacaoSaidaModelDto.DataHora = Convert.ToString(DateTime.Now);
                relatorioSolicitacaoSaidaModelDto.PreMovimentoId = solicitacaoId;

                if (itens.Any())
                {
                    relatorioSolicitacaoSaidaModelDto.Documento = itens[0].Solicitacao.Documento;
                    relatorioSolicitacaoSaidaModelDto.Estoque = itens[0].Solicitacao.Estoque.Descricao;
                    relatorioSolicitacaoSaidaModelDto.DataHoraSolicitacao = string.Format(
                        "{0:dd/MM/yyyy}",
                        itens[0].Solicitacao.Emissao);

                    relatorioSolicitacaoSaidaModelDto.Paciente =
                        (itens[0].Solicitacao.Paciente != null) ? itens[0].Solicitacao.Paciente.NomeCompleto : null;
                    relatorioSolicitacaoSaidaModelDto.Medico =
                        (itens[0].Solicitacao.MedicoSolicitante != null)
                            ? itens[0].Solicitacao.MedicoSolicitante.NomeCompleto
                            : null;
                    relatorioSolicitacaoSaidaModelDto.Setor =
                        (itens[0].Solicitacao.UnidadeOrganizacional != null)
                            ? itens[0].Solicitacao.UnidadeOrganizacional.Descricao
                            : null;

                    if (itens[0].Solicitacao.CreatorUserId != null && itens[0].Solicitacao.CreatorUserId != 0)
                    {
                        var usuario = usuarioRepository.Object.Get((long)itens[0].Solicitacao.CreatorUserId);
                        if (usuario != null)
                        {
                            relatorioSolicitacaoSaidaModelDto.UsuarioSolicitacao = string.Concat(
                                usuario.Name,
                                " ",
                                usuario.Surname);
                        }
                    }
                }

                foreach (var item in itens)
                {
                    relatorioSolicitacaoSaidaModelDto.Itens.Add(
                        new RelatorioSolicitacaoSaidaItemModelDto
                        {
                            CodigoProduto = item.Produto.Codigo,
                            DescricaoProduto = item.Produto.Descricao,
                            Quantidade = item.Quantidade.ToString(),
                            Sigla = item.ProdutoUnidade.Sigla
                        });
                }


                return relatorioSolicitacaoSaidaModelDto;
            }
        }

        public bool ChaveNFeUtilizada(string chave, long? movimentoId)
        {
            using (var estoquePreMovimentoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                if (movimentoId.HasValue && movimentoId.Value != 0)
                {
                    return estoquePreMovimentoRepository.Object.GetAll().AsNoTracking()
                        .Any(w => w.Chave == chave && w.Id != movimentoId.Value);
                }
                else
                {
                    return estoquePreMovimentoRepository.Object.GetAll().AsNoTracking().Any(w => w.Chave == chave);
                }
            }
        }

        public byte[] RetornaArquivoSolicitacaoBaixa(long preMovimentoId)
        {
            return this.CreateJasperReport("SolicitacaoSaidaBaixa")
                .SetMethod(Method.POST)
                .AddParameter("preMovimentoId", preMovimentoId.ToString())
                .AddParameter("UsuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }

        public byte[] RetornaArquivoSolicitacao(long preMovimentoId)
        {
            return this.CreateJasperReport("SolicitacaoSaida")
                .SetMethod(Method.POST)
                .AddParameter("preMovimentoId", preMovimentoId.ToString())
                .AddParameter("UsuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }

        #endregion
    }
}