using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    [Table("AssPrescricaoStatus")]
    public class PrescricaoStatus : CamposPadraoCRUD, IDescricao
    {
        public static int Inicial = 1;
        public static int Liberada = 2;
        public static int LiberadaComAcrescimo = 7;
        public static int Aprazada = 3;
        public static int Cancelada = 4;
        public static int Suspensa = 5;
        public static int Aprovada = 6;
        public static int AprovadaComAcrescimo = 8;

        public string Cor { get; set; }
    }
}
