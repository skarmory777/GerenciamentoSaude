using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    [AutoMap(typeof(CompraRequisicaoTipo))]
    public class CompraRequisicaoTipoDto : CamposPadraoCRUDDto
    {
    }
}
