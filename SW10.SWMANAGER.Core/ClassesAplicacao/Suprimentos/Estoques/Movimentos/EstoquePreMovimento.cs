using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosPerdaProdutos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoquePreMovimento")]
    public class EstoquePreMovimento : CamposPadraoCRUD
    {
        public string Documento { get; set; }
        [Index("Est_Idx_Movimento")]
        public DateTimeOffset Movimento { get; set; }
        public long? EstoqueId { get; set; }
        public long? SisFornecedorId { get; set; }
        public long? FornecedorId { get; set; }
        public long? EmprestimoEmpresaId { get; set; }
        public long? EmpresaId { get; set; }
        public decimal Quantidade { get; set; }
        public long PreMovimentoEstadoId { get; set; }
        public decimal TotalProduto { get; set; }
        public decimal Frete { get; set; }
        public decimal AcrescimoDecrescimo { get; set; }
        public decimal TotalDocumento { get; set; }
        public long? CentroCustoId { get; set; }
        public bool IsEntrada { get; set; }
        public bool Contabiliza { get; set; }
        public bool Consiginado { get; set; }
        public bool AplicacaoDireta { get; set; }
        public bool EntragaProduto { get; set; }
        public string Serie { get; set; }
        public long? OrdemId { get; set; }
        public long? MotivoPerdaProdutoId { get; set; }
        public long? EstTipoMovimentoId { get; set; }
        public long? EstTipoOperacaoId { get; set; }
        public long? EstoqueEmprestimoId { get; set; }

        [ForeignKey("EstoqueEmprestimoId")]
        public EstoqueEmprestimo EstoqueEmprestimo { get; set; }

        [ForeignKey("EstoqueId")]
        public Estoque Estoque { get; set; }

        [ForeignKey("SisFornecedorId")]
        public SisFornecedor SisFornecedor { get; set; }

        [ForeignKey("FornecedorId")]
        public Fornecedor Fornecedor { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        [ForeignKey("EmprestimoEmpresaId")]
        public SisPessoa EmprestimoEmpresa { get; set; }

        [ForeignKey("PreMovimentoEstadoId")]
        public EstoquePreMovimentoEstado PreMovimentoEstado { get; set; }

        [ForeignKey("OrdemId")]
        public OrdemCompra OrdemCompra { get; set; }

        [ForeignKey("CentroCustoId")]
        public CentroCusto CentroCusto { get; set; }

        public long? CFOPId { get; set; }

        [ForeignKey("CFOPId")]
        public Cfop CFOP { get; set; }
        public decimal? ICMSPer { get; set; }

        public decimal? ValorICMS { get; set; }

        public decimal? DescontoPer { get; set; }

        public long? TipoFreteId { get; set; }

        [ForeignKey("TipoFreteId")]
        public TipoFrete TipoFrete { get; set; }

        public bool InclusoNota { get; set; }
        public decimal? FretePer { get; set; }
        public decimal? ValorFrete { get; set; }
        public long? Frete_SisFornecedorId { get; set; }

        [ForeignKey("Frete_SisFornecedorId")]
        public SisFornecedor Frete_SisForncedor { get; set; }



        public long? Frete_FornecedorId { get; set; }

        [ForeignKey("Frete_FornecedorId")]
        public Fornecedor Frete_Forncedor { get; set; }

        [Index("Est_Idx_Emissao")]
        public DateTimeOffset Emissao { get; set; }

        public long? PacienteId { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        public long? MedicoSolcitanteId { get; set; }

        [ForeignKey("MedicoSolcitanteId")]
        public Medico MedicoSolicitante { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        [ForeignKey("UnidadeOrganizacionalId")]
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        public string Observacao { get; set; }

        public long? AtendimentoId { get; set; }

        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }

        [ForeignKey("MotivoPerdaProdutoId")]
        public MotivoPerdaProduto MotivoPerdaProduto { get; set; }

        [ForeignKey("EstTipoMovimentoId")]
        public TipoMovimento EstTipoMovimento { get; set; }

        [ForeignKey("EstTipoOperacaoId")]
        public TipoOperacao EstTipoOperacao { get; set; }

        [Column("EstGrupoOperacaoId")]
        public long? GrupoOperacaoId { get; set; }

        [ForeignKey("GrupoOperacaoId")]
        public EstoqueGrupoOperacao EstoqueGrupoOperacao { get; set; }

        //[NotMapped]
        public List<EstoquePreMovimentoItem> Itens { get; set; }
        [Index("Est_Idx_HoraPrescrita")]
        public DateTime? HoraPrescrita { get; set; }

        [ForeignKey("HoraDia"), Column("SisHoraDiaId")]
        public long? HoraDiaId { get; set; }

        public HoraDia HoraDia { get; set; }

        [ForeignKey("PrescricaoMedica"), Column("SisPrescricaoMedicaId")]
        public long? PrescricaoMedicaId { get; set; }

        public PrescricaoMedica PrescricaoMedica { get; set; }

        [ForeignKey("PrescricaoItemResposta")]
        public long? PrescricaoItemRespostaId { get; set; }

        public PrescricaoItemResposta PrescricaoItemResposta { get; set; }

        public string Chave { get; set; }

        public long? InventarioId { get; set; }

        [ForeignKey("InventarioId")]
        public Inventario Inventario { get; set; }

        public long? EstoquePreMovimentoParentId { get; set; }

        [ForeignKey("EstoquePreMovimentoParentId")]
        public EstoquePreMovimento EstoquePreMovimentoParent { get; set; }

    }
}
