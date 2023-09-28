using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto
{
    [AutoMap(typeof(VersaoTiss))]
    public class VersaoTissDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }
        //public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        //public virtual ICollection<TabelaDominioVersaoTissDto> TabelaDominioVersoesTiss { get; set; }
    }
}