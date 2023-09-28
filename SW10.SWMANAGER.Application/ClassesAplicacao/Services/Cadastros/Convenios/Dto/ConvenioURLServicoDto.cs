using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto
{
    public class ConvenioURLServicoDto : CamposPadraoCRUDDto
    {
        public string Url { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long? VersaoTissId { get; set; }
        public VersaoTissDto VersaoTiss { get; set; }

        public int? IdGrid { get; set; }
    }
}
