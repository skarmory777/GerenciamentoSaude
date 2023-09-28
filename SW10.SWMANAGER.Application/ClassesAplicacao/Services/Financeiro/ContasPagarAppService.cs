using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using RestSharp;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class ContasPagarAppService : DocumentoAppService, IContasPagarAppService
    {
        public override async Task<DocumentoDto> Obter(long id)
        {
            var documentoDto = await base.Obter(id);

            using (var fornecedoreRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Fornecedor, long>>())
            {
                var fornecedor = fornecedoreRepository.Object.GetAll().AsNoTracking()
                                                   .Include(i => i.SisPessoa)
                                                   .FirstOrDefault(w => w.SisPessoaId == documentoDto.PessoaId);

                if (fornecedor != null)
                {
                    documentoDto.ForncedorId = fornecedor.Id;

                    documentoDto.Pessoa.Descricao = fornecedor.SisPessoa.FisicaJuridica == "F" ? fornecedor.SisPessoa.NomeCompleto : fornecedor.SisPessoa.NomeFantasia;

                    documentoDto.Fornecedor = fornecedor.MapTo<SisFornecedorDto>();
                }


                return documentoDto;
            }
        }

        public override DefaultReturn<DocumentoDto> CriarOuEditar(DocumentoDto input)
        {
            input.IsCredito = false;


            return base.CriarOuEditar(input);
        }

        public override Task<ListResultDto<LancamentoIndex>> ListarLancamento(ListarDocumentoInput input)
        {
            input.IsCredito = false;
            return base.ListarLancamento(input);
        }

        public DocumentoDto ObterPorPessoaNumero(long pessoaId, string numero)
        {
            try
            {
                using (var documentoRepositoy = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>())
                {
                    var query = documentoRepositoy.Object
                    .GetAll()
                    .AsNoTracking()
                    .Include(i => i.Empresa)
                    .Include(i => i.Pessoa)
                    .Include(i => i.LancamentoDocumentos)
                    .Include(i => i.LancamentoDocumentos.Select(s => s.SituacaoLancamento))
                    .Include(i => i.TipoDocumento)
                    .FirstOrDefault(m => m.PessoaId == pessoaId && m.Numero == numero);
                    if (query != null)
                    {
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

                        return documentoDto;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public override Task<DocumentoDto> ObterPorLancamento(long id)
        {
            return base.ObterPorLancamento(id);
        }

        public async Task<ListResultDto<VWRptContaPagarDetalhadoDto>> ListarContaPagarDetalhadoReport(VWRptContaPagarInput input)
        {
            try
            {
                using (var contaPagarRptRepository = IocManager.Instance.ResolveAsDisposable<IRepository<VWRptContaPagarDetalhado, long>>())
                {
                    var query = contaPagarRptRepository.Object
                    .GetAll().AsNoTracking()
                    .WhereIf(input.MeioPagamentoId > 0, m => m.TipoCobrancaId == input.MeioPagamentoId)
                    .WhereIf(input.EmpresaId.HasValue && input.EmpresaId.Value > 0, m => m.EmpresaId == input.EmpresaId)
                    .WhereIf(input.PessoaId.HasValue && input.PessoaId > 0, m => m.PessoaId == input.PessoaId)
                    .WhereIf(input.Situacao > 0, m => m.SituacaoLancamentoId == input.Situacao)
                    .WhereIf(input.TipoDocumentoId > 0, m => m.TipoDocumentoId == input.TipoDocumentoId)
                    //.WhereIf(input.SituacaoNotaFiscal > 0, m => m. == input.Situacao)
                    //.WhereIf(input.TipoPessoaId > 0, m => m. == input.Situacao)
                    .WhereIf(input.TipoData == 1, m => m.Vencimento >= input.StartDate && m.Vencimento <= input.EndDate)
                    .WhereIf(input.TipoData == 2, m => m.Emissao >= input.StartDate && m.Emissao <= input.EndDate)
                    .Where(w => w.IsCredito == input.IsCredito);

                    var queryList = query.ToList();

                    var listaProdutoSaldoDto = VWRptContaPagarDetalhadoDto.Mapear(queryList).ToList();

                    return new ListResultDto<VWRptContaPagarDetalhadoDto> { Items = listaProdutoSaldoDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long?> ObterFornecedorId(long pessoaId)
        {
            var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>();
            var fornecedor = await sisFornecedorRepository.Object
                             .GetAll()
                             .Include(x => x.SisPessoa)
                             .SingleOrDefaultAsync(x => x.SisPessoaId == pessoaId)
                             .ConfigureAwait(false);

            if (fornecedor != null)
                return fornecedor.Id;


            return null;
        }
        public byte[] GerarRelatorio(RelatorioContasApagarDto input)
        {
            return this.CreateJasperReport("Financeiro/ContasPagar")
                .SetMethod(Method.POST)
                .AddParameter("dataInicio", input.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFim", input.DataFim.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("isCredito", (input.IsCredito ? 1 : 0).ToString())
                .AddParameter("pessoaId", input.PessoaId.HasValue ? input.PessoaId.ToString() : "0")
                .AddParameter("empresaId", input.EmpresaId.HasValue ? input.EmpresaId.ToString() : "0")
                .AddParameter("situacaoLancamentoId", input.SituacaoLancamentoId.HasValue ? input.SituacaoLancamentoId.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }

        public byte[] GerarRelatorioQuitacao(RelatorioContasApagarDto input)
        {
            return this.CreateJasperReport("Financeiro/ContasPagarQuitacao")
                .SetMethod(Method.POST)
                .AddParameter("dataInicio", input.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFim", input.DataFim.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("isCredito", (input.IsCredito ? 1 : 0).ToString())
                .AddParameter("pessoaId", input.PessoaId.HasValue ? input.PessoaId.ToString() : "0")
                .AddParameter("empresaId", input.EmpresaId.HasValue ? input.EmpresaId.ToString() : "0")
                .AddParameter("situacaoLancamentoId", input.SituacaoLancamentoId.HasValue ? input.SituacaoLancamentoId.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }

        public byte[] GerarRelatorioGroupNome(RelatorioContasApagarDto input)
        {
            return this.CreateJasperReport("Financeiro/ContasPagarGroupNome")
                .SetMethod(Method.POST)
                .AddParameter("dataInicio", input.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFim", input.DataFim.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("isCredito", (input.IsCredito ? 1 : 0).ToString())
                .AddParameter("pessoaId", input.PessoaId.HasValue ? input.PessoaId.ToString() : "0")
                .AddParameter("empresaId", input.EmpresaId.HasValue ? input.EmpresaId.ToString() : "0")
                .AddParameter("situacaoLancamentoId", input.SituacaoLancamentoId.HasValue ? input.SituacaoLancamentoId.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }

        public async Task<DefaultReturn<DocumentoDto>> Excluir(long id)
        {
            var retornoPadrao = new DefaultReturn<DocumentoDto>();

            try
            {
                using (var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>())
                {
                    var lancamento = await lancamentoRepository.Object.GetAll().AsNoTracking()
                        .Include(i => i.LancamentosQuitacoes).SingleOrDefaultAsync(x => x.Id == id);

                    if (lancamento.LancamentosQuitacoes.Any())
                    {
                        retornoPadrao.Errors.Add(ErroDto.Criar("", "Não é possível excluir conta com quitações."));
                        return retornoPadrao;
                    }

                    await lancamentoRepository.Object.DeleteAsync(lancamento.Id);
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
    }
}
