using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [Table("DocRotulo")]
    public class DocRotulo : CamposPadraoCRUD
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public string Titulo { get; set; }
        public float Ordem { get; set; }

        [StringLength(7)]
        public string Cor { get; set; }
        public bool IsMostrarGlobal { get; set; }
        public bool IsCapitulo { get; set; }
        public bool IsSessao { get; set; }
        public bool IsAssunto { get; set; }
        public bool IsModulo { get; set; }
        public bool IsStatus { get; set; }
        public bool IsPrioridade { get; set; }
    }
}
