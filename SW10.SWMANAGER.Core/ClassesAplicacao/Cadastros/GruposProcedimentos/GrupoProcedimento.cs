using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposProcedimentos
{
    [Table("GrupoProcedimento")]
    public class GrupoProcedimento : CamposPadraoCRUD
    {
        public bool IsProibido { get; set; }
    }

}
