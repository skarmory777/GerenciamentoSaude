using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(Documento))]
    public class DocumentoDto : CamposPadraoCRUDDto
    {
        public long TipoDocumentoId { get; set; }
        public TipoDocumentoDto TipoDocumento { get; set; }
        public long EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }
        public long? ForncedorId { get; set; }
        public SisFornecedorDto Fornecedor { get; set; }
        public string Numero { get; set; }
        public DateTimeOffset? DataEmissao { get; set; }
        public bool IsCredito { get; set; }
        public decimal? ValorDocumento { get; set; }
        public decimal? ValorAcrescimoDecrescimo { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string Observacao { get; set; }
        public long? PessoaId { get; set; }
        public SisPessoaDto Pessoa { get; set; }
        public List<LancamentoDto> LancamentosDto { get; set; }
        public List<DocumentoRateioDto> DocumentosRateiosDto { get; set; }
        public string LancamentosJson { get; set; }
        public string RateioJson { get; set; }
        public decimal? ValorTotalParcelas { get; set; }
        public decimal? ValorTotalRateio { get; set; }
        public decimal? ValorTotalDocumento { get; set; }
        public Guid? AnexoListaId { get; set; }
        public long? PreMovimentoId { get; set; }
    }
}
