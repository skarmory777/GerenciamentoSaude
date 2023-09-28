using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    public class Pro_ReqExameMovItem : CamposPadraoCRUD
    {
        public int IdRequisicaoMovItem { get; set; }
        public int QtdeRequisitada { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool IsEncerrada { get; set; }
        public bool IsAtendida { get; set; }
        public int IdRequisicaoMov { get; set; }
        public int IdUsuario { get; set; }
        public int IdItemRequisitado { get; set; }
        public int IdItem { get; set; }
        public int? IdAutorizacao { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        [StringLength(20)]
        public string SenhaAutorizacao { get; set; }
        [StringLength(50)]
        public string NomeAutorizacao { get; set; }
        [StringLength(100)]
        public string ObsAutorizacao { get; set; }
        public int? IdFatKit { get; set; }
        public int? IdMaterial { get; set; }
        [StringLength(1000)]
        public string ObsRequisicao { get; set; }
        public long? IDSW { get; set; }
        public long? TenantId { get; set; }

        [ForeignKey("Pro_ReqExameMov")]
        public long? Pro_ReqExameMovId { get; set; }
        public Pro_ReqExameMov Pro_ReqExameMov { get; set; }
    }
}
