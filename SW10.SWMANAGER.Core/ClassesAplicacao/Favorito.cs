using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    [Table("SisFavorito")]
    public class Favorito : CamposPadraoCRUD
    {
        public long UserId { get; set; }

        public string DisplayName { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
