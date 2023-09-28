using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios
{
    [Table("FatConfigConvenioGlobal")]
    public class FaturamentoConfigConvenioGlobal : CamposPadraoCRUD
    {
        [ForeignKey("Empresa"), Column("EmpresaId")]
        public long? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [ForeignKey("Convenio"), Column("ConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Plano"), Column("PlanoId")]
        public long? PlanoId { get; set; }
        public Plano Plano { get; set; }

        [ForeignKey("Grupo"), Column("FatGrupoId")]
        public long? GrupoId { get; set; }
        public FaturamentoGrupo Grupo { get; set; }

        [ForeignKey("SubGrupo"), Column("FatSubGrupoId")]
        public long? SubGrupoId { get; set; }
        public FaturamentoSubGrupo SubGrupo { get; set; }

        [ForeignKey("TabelaGlobal"), Column("FatTabelaGlobalId")]
        public long? TabelaGlobalId { get; set; }
        public FaturamentoGlobal TabelaGlobal { get; set; }

        [ForeignKey("Item"), Column("FatItemId")]
        public long? ItemId { get; set; }
        public FaturamentoItem Item { get; set; }

        [Index("Fat_Idx_DataIncio")]
        [DataType(DataType.DateTime)]
        public DateTime? DataIncio { get; set; }
        [Index("Fat_Idx_DataFim")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFim { get; set; }
    }

}


