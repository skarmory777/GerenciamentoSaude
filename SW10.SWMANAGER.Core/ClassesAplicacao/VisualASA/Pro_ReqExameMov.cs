using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    public class Pro_ReqExameMov : CamposPadraoCRUD
    {
        public int IdRequisicaoMov { get; set; }
        public int IdCcRequisitado { get; set; }
        public int IdMedico { get; set; }
        public DateTime DataRequisicao { get; set; }
        [StringLength(20)]
        public string NumeroDocumento { get; set; }
        public bool IsEncerrada { get; set; }
        public bool IsSemanal { get; set; }
        [StringLength(255)]
        public string Obs { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public int IdUsuario { get; set; }
        public int IdAtendimento { get; set; }
        public int IdLocalUtilizacao { get; set; }
        public int IdTerceirizado { get; set; }
        public int IdCentroCusto { get; set; }
        public bool Hidden { get; set; }
        public int IdReqExameStatus { get; set; }
        public int? IdClinica { get; set; }
        public int? TipoRequisicao { get; set; }
        public long? IDSW { get; set; }
        public long? TenantId { get; set; }

        public ICollection<Pro_ReqExameMovItem> Itens { get; set; }
    }
}
