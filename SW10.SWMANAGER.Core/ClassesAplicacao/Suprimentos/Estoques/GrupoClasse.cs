using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_GrupoClasse")]
    public class GrupoClasse : CamposPadraoCRUD
    {
        /// <summary>
        /// Grupo
        /// </summary>
        public long GrupoId { get; set; }
        [ForeignKey("GrupoId")]
        public Grupo Grupo { get; set; }

        /// <summary>
        /// Coleção de sub-classe que pertencem a classe
        /// </summary>
        public ICollection<GrupoSubClasse> SubClasses { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }
    }
}
