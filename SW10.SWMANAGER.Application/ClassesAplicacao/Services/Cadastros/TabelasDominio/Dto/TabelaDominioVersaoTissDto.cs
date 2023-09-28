using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto
{
    [AutoMap(typeof(TabelaDominioVersaoTiss))]
    public class TabelaDominioVersaoTissDto : VersaoTissRelacaoDto
    {
        public long TabelaDominioId { get; set; }

        public virtual TabelaDominioDto TabelaDominio { get; set; }
    }
}
