using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Dto
{
    [AutoMap(typeof(Kit))]
    public class KitDto : CamposPadraoCRUDDto
    {
        public int TipoLayout { get; set; }
        public string DiretorioOrdem { get; set; }
        public string DiretorioResultado { get; set; }
        //public Informacao Informacao { get; set; }
    }
}
