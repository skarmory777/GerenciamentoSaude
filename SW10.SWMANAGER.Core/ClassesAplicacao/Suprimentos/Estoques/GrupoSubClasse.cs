using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_GrupoSubClasse")]
    public class GrupoSubClasse : CamposPadraoCRUD
    {
        /// <summary>
        /// Classe
        /// </summary>
        public long GrupoClasseId { get; set; }
        [ForeignKey("GrupoClasseId")]
        public GrupoClasse Classe { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }
    }
}
