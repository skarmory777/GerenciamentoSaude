using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto
{
    [AutoMap(typeof(VersaoTiss))]
    public class CriarOuEditarVersaoTiss : CamposPadraoCRUDDto
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
