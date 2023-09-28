using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias
{
    [Table("SisTipoOcorrencias")]
    public class TipoOcorrencia : CamposPadraoCRUD
    {
        public const long ContaMedica = 1;
        public const long PrescricaoMedica = 2;
        public const long ResultadoExame = 3;
    }
}