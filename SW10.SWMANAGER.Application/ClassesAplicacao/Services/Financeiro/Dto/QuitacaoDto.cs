using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(Quitacao))]
    public class QuitacaoDto : CamposPadraoCRUDDto
    {
        public long? ContaCorrenteId { get; set; }
        public ContaCorrenteDto ContaCorrente { get; set; }
        public long? MeioPagamentoId { get; set; }
        public MeioPagamentoDto MeioPagamento { get; set; }
        public string Numero { get; set; }
        public long? ChequeId { get; set; }
        public DateTime? DataMovimento { get; set; }
        public DateTime? DataCompensado { get; set; }
        public DateTime? DataConsolidado { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? Acrescimo { get; set; }
        public decimal? MoraMulta { get; set; }
        public string Observacao { get; set; }
        public string LancamentosJson { get; set; }
        public long? EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }
        public ChequeDto Cheque { get; set; }
        public decimal ValorQuitacao { get; set; }
        public decimal ValorEfetivo { get; set; }
        public bool IsCredito { get; set; }
        public long? PessoaId { get; set; }
        public SisPessoaDto Pessoa { get; set; }
        public List<DocumentoRateioDto> DocumentosRateiosDto { get; set; }
        public string RateioJson { get; set; }
        public long? TipoDocumentoId { get; set; }
        public Guid? TransferenciaIdentificador { get; set; }
        public long? TipoQuitacaoId { get; set; }
    }
}
