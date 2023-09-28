using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    [AutoMap(typeof(CompraRequisicaoModo))]
    public class CompraRequisicaoModoDto : CamposPadraoCRUDDto
    {
    }
}
