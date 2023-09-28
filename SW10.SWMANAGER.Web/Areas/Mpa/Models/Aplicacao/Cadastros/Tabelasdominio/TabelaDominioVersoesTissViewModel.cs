using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TabelasDominio
{
    public class TabelaDominioVersoesTissViewModel
    {
        public ICollection<TabelaDominioVersaoTissDto> TabelaDominioVersoesTiss { get; set; }

        public ICollection<VersaoTissDto> VersoesTiss { get; set; }
    }
}