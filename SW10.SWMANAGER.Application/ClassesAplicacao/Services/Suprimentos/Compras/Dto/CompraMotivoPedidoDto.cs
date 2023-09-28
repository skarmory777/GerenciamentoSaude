using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    /// <summary>
    /// Motivo Pedido: Reposição de Estoque, Aumento de Consumo, Setor, Paciente
    /// </summary>
    [AutoMap(typeof(CompraMotivoPedido))]
    public class CompraMotivoPedidoDto : CamposPadraoCRUDDto
    {
    }
}
