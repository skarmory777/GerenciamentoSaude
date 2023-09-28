using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias
{
    [Table("SisSubTipoOcorrencias")]
    public class SubTipoOcorrencia : CamposPadraoCRUD
    {
        public long TipoOcorrenciaId { get; set; }
        
        public TipoOcorrencia TipoOcorrencia { get; set; }
        
        public const long ContaMedicaItem = 1;
        public const long ContaMedicaKit = 2;
        public const long ContaMedicaPacote = 3;
    }
}