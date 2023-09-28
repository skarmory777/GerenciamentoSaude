using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(Favorito))]
    public class FavoritoDto : CamposPadraoCRUDDto
    {
        public long UserId { get; set; }

        public string DisplayName { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
