using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_Grupo")]
    public class Grupo : CamposPadraoCRUD
    {
        /// <summary>
        /// Coleção de classes que pertencem ao Grupo
        /// </summary>
        public ICollection<GrupoClasse> Classes { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }
    }
}
