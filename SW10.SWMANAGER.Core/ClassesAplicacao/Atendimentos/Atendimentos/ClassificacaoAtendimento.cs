using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    [Table("AteClassificacaoAtendimento")]
    public class ClassificacaoAtendimento : CamposPadraoCRUD
    {
        public string Cor { get; set; }
        public int Prioridade { get; set; }
        public int PrazoAtendimento { get; set; }
        public bool Ativo { get; set; }
    }
}
