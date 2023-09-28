using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    /// <summary>
    /// Classe CompraRequisicaoDto.
    /// Representa um Dto de uma Requisicao de Compra. Mapeia a Classe CompraRequisicao que define a referente entidade no BD
    /// </summary>
    [AutoMap(typeof(CompraRequisicao))]
    public class CompraRequisicaoDto : CamposPadraoCRUDDto
    {

        #region ↓ Propriedades

        public string RequisicoesItensJson { get; set; }

        public bool IsUrgente { get; set; }

        public bool IsAlteraAposGravacao { get; set; }

        public bool IsEncerrada { get; set; }

        public DateTime DataRequisicao { get; set; }

        public DateTime DataLimiteEntrega { get; set; }

        public DateTime? DataInicioCotacao { get; set; }

        public DateTime? DataFinalCotacao { get; set; }

        public string Observacao { get; set; }

        #region → Chaves Estrangeiras
        public long EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }

        public long? EstoqueId { get; set; }
        public EstoqueDto Estoque { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public long MotivoPedidoId { get; set; }
        public CompraMotivoPedidoDto MotivoPedido { get; set; }

        public long TipoRequisicaoId { get; set; }
        public CompraRequisicaoTipo TipoRequisicao { get; set; }

        public long ModoRequisicaoId { get; set; }
        public CompraRequisicaoModo ModoRequisicao { get; set; }

        public long CotacaoStatusId { get; set; }
        public CompraCotacaoStatus CotacaoStatus { get; set; }

        public long AprovacaoStatusId { get; set; }
        public CompraAprovacaoStatus AprovacaoStatus { get; set; }

        public long? FinFormaPagamentoId { get; set; }
        public FormaPagamentoDto FinFormaPagamento { get; set; }

        public DateTime? DataHoraVencimento { get; set; }
        #endregion

        #endregion Propriedades

    }
}
