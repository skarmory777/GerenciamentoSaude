using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto
{
    public class VolOutput
    {
        public int? qVol { get; set; }
        public string esp { get; set; }
        public string marca { get; set; }
        public string nVol { get; set; }
        public decimal? pesoL { get; set; }
        public decimal? pesoB { get; set; }
        public ICollection<LacresOutput> lacres { get; set; }
    }
}
