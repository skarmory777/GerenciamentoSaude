using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    [Table("CmpOrdemCompra")]
    public class CmpOrdemCompra : CamposPadraoCRUD
    {
        #region ↓ Propriedades
        public int PrazoPagamento { get; set; }
        [Index("Cmp_Idx_DataOrdemCompra")]
        [DataType(DataType.DateTime)]
        public DateTime DataOrdemCompra { get; set; }
        [Index("Cmp_Idx_DataPrevistaEntrega")]
        [DataType(DataType.DateTime)]
        public DateTime DataPrevistaEntrega { get; set; }
        [Index("Cmp_Idx_DataFinalEntrega")]
        [DataType(DataType.DateTime)]
        public DateTime DataFinalEntrega { get; set; }

        public decimal ValorFrete { get; set; }

        public string EnderecoEntrega { get; set; }

        public decimal ValorDesconto { get; set; }
        #region → Chaves Estrangeiras
        [ForeignKey("Empresa"), Column("CmpEmpresaId")]
        public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("OrdemCompraStatus"), Column("CmpOrdemCompraStatusId")]
        public long OrdemCompraStatusId { get; set; }
        public OrdemCompraStatus OrdemCompraStatus { get; set; }
        #endregion Chaves Estrangeiras

        #endregion
    }
}
