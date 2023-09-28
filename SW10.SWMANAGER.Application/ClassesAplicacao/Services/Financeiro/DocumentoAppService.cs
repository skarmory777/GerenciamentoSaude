using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.ServicoValidacao;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class DocumentoAppService : SWMANAGERAppServiceBase, IDocumentoAppService
    {
        public async Task<ListResultDto<DocumentoDto>> Listar(ListarDocumentoInput input)
        {
            try
            {
                using (var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>())
                {
                    List<DocumentoDto> documentosDto = new List<DocumentoDto>();
                    var query = documentoRepository.Object.GetAll().AsNoTracking()
                                                          .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                     || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                           .Include(i => i.Pessoa)
                                                           .ToList();
                    //.Select(s => new DocumentoDto
                    //{
                    //    Id = s.Id,
                    //    Codigo = s.Codigo,
                    //    Descricao = s.Descricao,
                    //    DataEmissao = s.DataEmissao,
                    //    PessoaId = s.PessoaId,

                    //})


                    foreach (var item in query)
                    {
                        documentosDto.Add(item.MapTo<DocumentoDto>());
                    }

                    return new PagedResultDto<DocumentoDto>(documentosDto.Count, documentosDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public virtual async Task<DocumentoDto> Obter(long id)
        {
            try
            {
                using (var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>())
                {
                    var query = documentoRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Include(i => i.Empresa)
                    .Include(i => i.Pessoa)
                    .Include(i => i.LancamentoDocumentos)
                    .Include(i => i.LancamentoDocumentos.Select(s => s.SituacaoLancamento))
                    .Include(i => i.TipoDocumento)
                    .FirstOrDefault(m => m.Id == id);

                    var documentoDto = query.MapTo<DocumentoDto>();

                    long idGrid = 0;
                    documentoDto.LancamentosDto = new List<LancamentoDto>();
                    documentoDto.DocumentosRateiosDto = new List<DocumentoRateioDto>();

                    foreach (var lancamento in query.LancamentoDocumentos)
                    {
                        var lancamentoDto = lancamento.MapTo<LancamentoDto>();

                        lancamentoDto.SituacaoDescricao = string.Concat(lancamento.SituacaoLancamento.Codigo, " - ", lancamento.SituacaoLancamento.Descricao);
                        lancamentoDto.IdGrid = idGrid++;
                        documentoDto.LancamentosDto.Add(lancamentoDto);
                    }

                    foreach (var rateio in query.Rateios)
                    {
                        var rateioDto = rateio.MapTo<DocumentoRateioDto>();
                        documentoDto.DocumentosRateiosDto.Add(rateioDto);
                    }

                    return documentoDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public virtual DefaultReturn<DocumentoDto> CriarOuEditar(DocumentoDto input)
        {
            DefaultReturn<DocumentoDto> retornoPadrao;

            using (var documentoValidacaoService = IocManager.Instance.ResolveAsDisposable<DocumentoValidacaoService>())
            {
                retornoPadrao = documentoValidacaoService.Object.Validar(input);
                if (retornoPadrao.Errors.Any())
                {
                    return retornoPadrao;
                }
            }

            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>())
                {
                    var lancamentosDto = JsonConvert.DeserializeObject<List<LancamentoIndex>>(input.LancamentosJson, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    var rateiosDto = JsonConvert.DeserializeObject<List<DocumentoRateioIndex>>(input.RateioJson);

                    foreach (var item in lancamentosDto)
                    {
                        item.IsCredito = input.IsCredito;
                        if (item.SituacaoLancamentoId == 0)
                        {
                            item.SituacaoLancamentoId = 1;
                        }
                    }

                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        if (input.Id == 0)
                        {
                            var documento = input.MapTo<Documento>();
                            documento.LancamentoDocumentos = new List<Lancamento>();

                            foreach (var lancamentoDto in lancamentosDto)
                            {
                                documento.LancamentoDocumentos.Add(new Lancamento
                                {
                                    ValorLancamento = lancamentoDto.ValorLancamento,
                                    AnoCompetencia = lancamentoDto.AnoCompetencia,
                                    DataLancamento = lancamentoDto.DataLancamento,
                                    DataVencimento = lancamentoDto.DataVencimento,
                                    Juros = lancamentoDto.Juros,
                                    MesCompetencia = lancamentoDto.MesCompetencia,
                                    Multa = lancamentoDto.Multa,
                                    Parcela = lancamentoDto.Parcela,
                                    ValorAcrescimoDecrescimo = lancamentoDto.ValorAcrescimoDecrescimo,
                                    IsCredito = lancamentoDto.IsCredito,
                                    CodigoBarras = lancamentoDto.CodigoBarras,
                                    NossoNumero = lancamentoDto.NossoNumero,
                                    LinhaDigitavel = lancamentoDto.LinhaDigitavel,
                                    SituacaoLancamentoId = lancamentoDto.SituacaoLancamentoId
                                });
                            }

                            AtualizaListaRateio(documento, rateiosDto);
                            documentoRepository.Object.Insert(documento);
                            retornoPadrao.ReturnObject = documento.MapTo<DocumentoDto>();
                        }
                        else
                        {
                            var documento = documentoRepository.Object.GetAll()
                                                              .Where(w => w.Id == input.Id)
                                                            .Include(i => i.Empresa)
                                                            .Include(i => i.Pessoa)
                                                            .Include(i => i.LancamentoDocumentos)
                                                            .Include(i => i.TipoDocumento)
                                                            .Include(i => i.Rateios)
                                                             .FirstOrDefault();

                            if (documento != null)
                            {
                                documento.EmpresaId = input.EmpresaId;
                                documento.TipoDocumentoId = input.TipoDocumentoId;
                                documento.PessoaId = input.PessoaId;
                                documento.Numero = input.Numero;
                                documento.DataEmissao = input.DataEmissao;
                                documento.ValorDocumento = input.ValorDocumento;
                                documento.ValorAcrescimoDecrescimo = input.ValorAcrescimoDecrescimo;
                                documento.ValorDesconto = input.ValorDesconto;
                                documento.PreMovimentoId = input.PreMovimentoId;

                                AtualizaListaLancamentos(documento, lancamentosDto);
                                AtualizaListaRateio(documento, rateiosDto);


                                documentoRepository.Object.Update(documento);

                                retornoPadrao.ReturnObject = documento.MapTo<DocumentoDto>();
                            }
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
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

        public async Task Excluir(DocumentoDto input)
        {
            try
            {
                using (var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>())
                {
                    await documentoRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        virtual public async Task<ListResultDto<LancamentoIndex>> ListarLancamento(ListarDocumentoInput input)
        {
            try
            {
                if (input.EmissaoDe != null)
                {
                    input.EmissaoDe = ((DateTime)input.EmissaoDe).Date.AddMilliseconds(-1);
                }

                if (input.VencimentoDe.HasValue)
                {
                    input.VencimentoDe = input.VencimentoDe.Value.ToLocalTime();
                }

                if (input.VencimentoAte.HasValue)
                {
                    input.VencimentoAte = input.VencimentoAte.Value.ToLocalTime();
                }

                List<LancamentoIndex> lancamentosDto = new List<LancamentoIndex>();

                using (var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>())
                using (var lancamentoQuitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LancamentoQuitacao, long>>())
                {
                    string orderBy;
                    var query = lancamentoRepository.Object.GetAll().AsNoTracking()
                                                        .Where(w => (
                                                            (input.Filtro == "" || input.Filtro == null) || w.Descricao.ToString().Contains(input.Filtro))
                                                            && w.IsCredito == input.IsCredito
                                                            && (w.DocumentoId != null)

                                                            && (input.PessoaId == null || input.PessoaId == w.Documento.PessoaId)
                                                            && (input.EmissaoDe == null || input.IgnorarEmissao || (((DateTime)input.EmissaoDe).Date <= w.Documento.DataEmissao && ((DateTime)input.EmissaoAte).Date >= w.Documento.DataEmissao))
                                                            && (input.EmpresaId == null || input.EmpresaId == w.Documento.EmpresaId)
                                                            && (input.SituacaoLancamentoId == null || input.SituacaoLancamentoId == w.SituacaoLancamentoId)
                                                            && (input.Documento == null || input.Documento == string.Empty || w.Documento.Numero.Contains(input.Documento))
                                                            && (input.ContaAdministrativaId == null || w.Documento.Rateios.Any(a => a.ContaAdministrativaId == input.ContaAdministrativaId))
                                                            && (input.CentroCustoId == null || w.Documento.Rateios.Any(a => a.CentroCustoId == input.CentroCustoId))
                                                            && (input.VencimentoDe == null || input.IgnorarVencimento || (input.VencimentoDe <= w.DataVencimento && input.VencimentoAte >= w.DataVencimento))
                                                            && (input.TipoDocumentoId == null || input.TipoDocumentoId == w.Documento.TipoDocumentoId)
                                                            && (input.MeioPagamentoId == null || (w.LancamentosQuitacoes.Any(a => a.Quitacao.MeioPagamentoId == input.MeioPagamentoId)))
                                                            && (w.LancamentosQuitacoes.Count == 0 ||
                                                                w.LancamentosQuitacoes.Any(q => q.Quitacao.TipoQuitacaoId == (long)EnumTipoQuitacao.QuitacaoLancamento || q.Quitacao.TipoQuitacaoId == null)))
                                                       .Include(i => i.Documento)
                                                       .Include(i => i.Documento.Pessoa)
                                                       .Include(i => i.Documento.TipoDocumento)
                                                       .Include(i => i.SituacaoLancamento)
                                                       .Include(i => i.LancamentosQuitacoes)
                                                       .Include(i => i.LancamentosQuitacoes.Select(x => x.Quitacao));

                    orderBy = (string.IsNullOrEmpty(input.Sorting)) ? "DataLancamento" : input.Sorting;

                    var qtdLancamentos = await query.CountAsync();
                    query = query.OrderBy(x => orderBy).Skip(input.SkipCount * input.MaxResultCount).Take(input.MaxResultCount);


                    var result = query.Select(x => new LancamentoIndex()
                    {
                        Id = x.Id,
                        SituacaoDescricao = string.Concat(x.SituacaoLancamento.Codigo, " - ", x.SituacaoLancamento.Descricao),
                        CorLancamentoFundo = x.SituacaoLancamento.CorLancamentoFundo,
                        CorLancamentoLetra = x.SituacaoLancamento.CorLancamentoLetra,
                        Fornecedor = x.Documento.Pessoa.FisicaJuridica == "F" ? x.Documento.Pessoa.NomeCompleto : x.Documento.Pessoa.NomeFantasia,
                        DataVencimento = x.DataVencimento,
                        DataEmissao = x.Documento.DataEmissao,
                        Competencia = (!x.MesCompetencia.HasValue || !x.AnoCompetencia.HasValue) ? null : string.Concat(x.MesCompetencia.ToString().Length > 1 ? x.MesCompetencia.ToString()
                            : string.Concat("0", x.MesCompetencia.ToString()), "/", x.AnoCompetencia),
                        Documento = x.Documento.Numero,
                        TotalLancamento = x.ValorLancamento,
                        TipoDocumento = string.Concat(x.Documento.TipoDocumento.Codigo, " - ", x.Documento.TipoDocumento.Descricao),
                        TotalQuitacao = (x.LancamentosQuitacoes.Count > 0) ? x.LancamentosQuitacoes.Sum(s => s.ValorEfetivo) : 0,
                        EmpresaNome = x.Documento.Empresa.NomeFantasia,
                        FatEntregaLoteId = x.Documento.FatEntregaLoteId
                    }).ToList();

                    return new PagedResultDto<LancamentoIndex>(qtdLancamentos, result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public virtual async Task<DocumentoDto> ObterPorLancamento(long id)
        {
            try
            {
                using (var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>())
                using (var documentoRepositoy = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>())
                {
                    var lancamentoBusca = lancamentoRepository.Object.GetAll()
                                                      .Where(w => w.Id == id)
                                                      .FirstOrDefault();

                    if (lancamentoBusca != null)
                    {
                        var query = documentoRepositoy.Object.GetAll()
                            .Where(m => m.Id == lancamentoBusca.DocumentoId)
                            .Include(i => i.Empresa)
                            .Include(i => i.Pessoa)
                            .Include(i => i.LancamentoDocumentos)
                            .Include(i => i.LancamentoDocumentos.Select(s => s.SituacaoLancamento))
                            .Include(i => i.TipoDocumento)
                            .Include(i => i.Rateios)
                            .Include(i => i.Rateios.Select(s => s.CentroCusto))
                            .Include(i => i.Rateios.Select(s => s.ContaAdministrativa))
                            .Include(i => i.Rateios.Select(s => s.Empresa))
                            .FirstOrDefault();

                        var documentoDto = query.MapTo<DocumentoDto>();
                        long idGrid = 0;
                        documentoDto.LancamentosDto = new List<LancamentoDto>();
                        foreach (var lancamento in query.LancamentoDocumentos.OrderBy(o => o.Parcela))
                        {
                            var lancamentoDto = lancamento.MapTo<LancamentoDto>();

                            lancamentoDto.SituacaoDescricao = string.Concat(lancamento.SituacaoLancamento.Codigo, " - ", lancamento.SituacaoLancamento.Descricao);
                            lancamentoDto.CorLancamentoFundo = lancamento.SituacaoLancamento.CorLancamentoFundo;
                            lancamentoDto.CorLancamentoLetra = lancamento.SituacaoLancamento.CorLancamentoLetra;
                            lancamentoDto.IdGrid = idGrid++;
                            lancamentoDto.IsSelecionado = lancamentoDto.Id == id;
                            lancamentoDto.ValorLancamento = lancamento.ValorLancamento ?? 0;
                            lancamentoDto.ValorAcrescimoDecrescimo = lancamento.ValorAcrescimoDecrescimo ?? 0;
                            lancamentoDto.Juros = lancamento.Juros ?? 0;
                            lancamentoDto.Multa = lancamento.Multa ?? 0;
                            lancamentoDto.Total = (decimal)lancamentoDto.ValorLancamento + (decimal)lancamentoDto.ValorAcrescimoDecrescimo +
                                (decimal)lancamentoDto.Juros + (decimal)lancamentoDto.Multa;

                            documentoDto.LancamentosDto.Add(lancamentoDto);
                        }

                        documentoDto.DocumentosRateiosDto = new List<DocumentoRateioDto>();
                        foreach (var rateio in query.Rateios)
                        {
                            var rateioDto = rateio.MapTo<DocumentoRateioDto>();
                            documentoDto.DocumentosRateiosDto.Add(rateioDto);
                        }

                        documentoDto.ValorTotalDocumento = (documentoDto.ValorDocumento ?? 0) + (documentoDto.ValorAcrescimoDecrescimo ?? 0) - (documentoDto.ValorDesconto ?? 0);
                        return documentoDto;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        void AtualizaListaLancamentos(Documento documento, List<LancamentoIndex> lancamentosDto)
        {
            documento.LancamentoDocumentos.RemoveAll(r => !lancamentosDto.Any(a => a.Id == r.Id));

            //atuliza
            foreach (var lancamento in documento.LancamentoDocumentos)
            {
                var lancamentoDto = lancamentosDto.Where(w => w.Id == lancamento.Id)
                                               .First();

                //lancamento.Codigo = lancamentoDto.Codigo;
                //lancamento.Descricao = lancamentoDto.Descricao;
                lancamento.ValorLancamento = lancamentoDto.ValorLancamento;
                lancamento.AnoCompetencia = lancamentoDto.AnoCompetencia;
                lancamento.DataLancamento = lancamentoDto.DataLancamento;
                lancamento.DataVencimento = lancamentoDto.DataVencimento;
                lancamento.Juros = lancamentoDto.Juros;
                lancamento.MesCompetencia = lancamentoDto.MesCompetencia;
                lancamento.Multa = lancamentoDto.Multa;
                lancamento.Parcela = lancamentoDto.Parcela;
                lancamento.ValorAcrescimoDecrescimo = lancamentoDto.ValorAcrescimoDecrescimo;
                lancamento.ValorLancamento = lancamentoDto.ValorLancamento;
                lancamento.IsCredito = lancamentoDto.IsCredito;
                lancamento.CodigoBarras = lancamentoDto.CodigoBarras;
                lancamento.NossoNumero = lancamentoDto.NossoNumero;
                lancamento.LinhaDigitavel = lancamentoDto.LinhaDigitavel;
                lancamento.SituacaoLancamentoId = lancamentoDto.SituacaoLancamentoId;

            }

            //inclui novos
            foreach (var lancamentoDto in lancamentosDto.Where(w => (w.Id == 0 || w.Id == null)))
            {
                var lancamento = new Lancamento
                {
                    ValorLancamento = lancamentoDto.ValorLancamento,
                    AnoCompetencia = lancamentoDto.AnoCompetencia,
                    DataLancamento = lancamentoDto.DataLancamento,
                    DataVencimento = lancamentoDto.DataVencimento,
                    Juros = lancamentoDto.Juros,
                    MesCompetencia = lancamentoDto.MesCompetencia,
                    Multa = lancamentoDto.Multa,
                    Parcela = lancamentoDto.Parcela,
                    ValorAcrescimoDecrescimo = lancamentoDto.ValorAcrescimoDecrescimo
                };
                lancamento.ValorLancamento = lancamentoDto.ValorLancamento;
                lancamento.IsCredito = lancamentoDto.IsCredito;
                lancamento.CodigoBarras = lancamentoDto.CodigoBarras;
                lancamento.NossoNumero = lancamentoDto.NossoNumero;
                lancamento.LinhaDigitavel = lancamentoDto.LinhaDigitavel;
                lancamento.SituacaoLancamentoId = lancamentoDto.SituacaoLancamentoId;

                documento.LancamentoDocumentos.Add(lancamento);
            }
        }

        void AtualizaListaRateio(Documento documento, List<DocumentoRateioIndex> rateiosDto)
        {
            if (documento.Rateios == null)
            {
                documento.Rateios = new List<DocumentoRateio>();
            }
            else
            {

                documento.Rateios.RemoveAll(r => !rateiosDto.Any(a => a.Id == r.Id));

                foreach (var rateio in documento.Rateios)
                {
                    var rateioDto = rateiosDto.Where(w => w.Id == rateio.Id).First();

                    rateio.CentroCustoId = rateioDto.CentroCustoId.HasValue ? (long)rateioDto.CentroCustoId : 0;
                    rateio.ContaAdministrativaId = rateioDto.ContaAdministrativaId.HasValue ? (long)rateioDto.ContaAdministrativaId : 0;
                    rateio.EmpresaId = rateioDto.EmpresaId.HasValue ? (long)rateioDto.EmpresaId : 0;
                    rateio.Valor = rateioDto.Valor;
                    rateio.Observacao = rateioDto.Observacao;
                    rateio.IsImposto = rateioDto.IsImposto;
                }
            }

            documento.Rateios.AddRange(rateiosDto.Where(w => (w.Id == 0 || w.Id == null)).Select(rateioDto => new DocumentoRateio
            {
                CentroCustoId = rateioDto.CentroCustoId.HasValue ? (long)rateioDto.CentroCustoId : 0,
                ContaAdministrativaId = rateioDto.ContaAdministrativaId.HasValue ? (long)rateioDto.ContaAdministrativaId : 0,
                EmpresaId = rateioDto.EmpresaId.HasValue ? (long)rateioDto.EmpresaId : 0,
                Valor = rateioDto.Valor,
                Observacao = rateioDto.Observacao,
                IsImposto = rateioDto.IsImposto
            }));


        }
    }
}
