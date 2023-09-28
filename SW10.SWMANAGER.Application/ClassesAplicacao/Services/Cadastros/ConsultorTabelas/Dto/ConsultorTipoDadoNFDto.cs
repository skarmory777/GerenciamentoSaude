using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto
{
    [AutoMap(typeof(ConsultorTipoDadoNF))]
    public class ConsultorTipoDadoNFDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}