using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.ModelosPrescricoes
{
    [Table("AssModeloPrescricaoMedica")]
    public class ModeloPrescricao : CamposPadraoCRUD
    {
       public long PrescricaoMedicaId { get; set; }

        [ForeignKey("PrescricaoMedicaId")]
        public PrescricaoMedica PrescricaoMedica { get; set; }

        public long AtendimentoId { get; set; }

        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }


    }
}
