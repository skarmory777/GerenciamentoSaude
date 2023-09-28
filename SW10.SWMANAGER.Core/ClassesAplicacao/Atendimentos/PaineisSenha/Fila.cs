using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AteFila")]
    public class Fila : CamposPadraoCRUD
    {
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }
        public bool IsZera { get; set; }
        public DateTime? HoraZera { get; set; }
        public bool IsAtivo { get; set; }
        public bool IsDomingo { get; set; }
        public bool IsSegunda { get; set; }
        public bool IsTerca { get; set; }
        public bool IsQuarta { get; set; }
        public bool IsQuinta { get; set; }
        public bool IsSexta { get; set; }
        public bool IsSabado { get; set; }
        public string Cor { get; set; }
        public bool IsNaoImprimeSenha { get; set; }
        public long? TipoLocalChamadaInicialId { get; set; }

        [ForeignKey("TipoLocalChamadaInicialId")]
        public TipoLocalChamada TipoLocalChamadaInicial { get; set; }

        public int UltimaSenha { get; set; }
        public DateTime UltimaZera { get; set; }

        public long? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        public long QtdImpressaoSenha { get; set; } = 1;
    }
}
