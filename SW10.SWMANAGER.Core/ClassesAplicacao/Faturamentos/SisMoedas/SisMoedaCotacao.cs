using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.SisMoedas
{
    [Table("SisMoedaCotacao")]
    public class SisMoedaCotacao : CamposPadraoCRUD
    {
        [ForeignKey("SisMoedaId")]
        public SisMoeda SisMoeda { get; set; }
        public long? SisMoedaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
        public long? EmpresaId { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }
        public long? ConvenioId { get; set; }

        [ForeignKey("PlanoId")]
        public Plano Plano { get; set; }
        public long? PlanoId { get; set; }

        [ForeignKey("GrupoId")]
        public FaturamentoGrupo Grupo { get; set; }
        public long? GrupoId { get; set; }

        [ForeignKey("SubGrupoId")]
        public FaturamentoSubGrupo SubGrupo { get; set; }
        public long? SubGrupoId { get; set; }

        [Index("Fat_Idx_DataInicio")]
        [DataType(DataType.DateTime)]
        public DateTime DataInicio { get; set; }

        [Index("Fat_Idx_DataFinal")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFinal { get; set; }

        public float Valor { get; set; }

        public bool IsTodosConvenio { get; set; }
        public bool IsTodosPlano { get; set; }
        public bool IsTodosItem { get; set; }
    }
}

