using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(TipoMeioPagamento))]
    public class TipoMeioPagamentoDto : CamposPadraoCRUDDto
    {
    }
}
