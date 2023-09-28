using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos
{
    [Table("LeitoCaracteristica")]
    public class LeitoCaracteristica : CamposPadraoCRUD
    {

        [StringLength(10)]
        public string Ramal { get; set; }

        // FALTA A PROPRIEDADE 'setoresId'
    }
}
