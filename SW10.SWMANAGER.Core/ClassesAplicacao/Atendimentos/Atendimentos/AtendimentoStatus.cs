using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos
{
    [Table("AteAtendimentoStatus")]
    public class AtendimentoStatus : CamposPadraoCRUD
    {
        /// <summary>
        /// The aguardando.
        /// </summary>
        public static long Aguardando = 1;

        public static long EmAtendimento = 2;

        public static long Pendente = 3;

        public static long Atendido = 4;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }


        public string CorFundo { get; set; }

        public string CorTexto { get; set; }


        
    }
}
