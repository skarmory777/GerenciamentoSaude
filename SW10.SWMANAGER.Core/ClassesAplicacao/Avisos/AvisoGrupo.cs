using System.ComponentModel.DataAnnotations.Schema;
using SW10.SWMANAGER.Authorization.Roles;

namespace SW10.SWMANAGER.ClassesAplicacao.Avisos
{
    [Table("SisAvisoGrupos")]
    public class AvisoGrupo: CamposPadraoCRUD
    {
        public long AvisoId { get; set; }
        public Aviso Aviso { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}