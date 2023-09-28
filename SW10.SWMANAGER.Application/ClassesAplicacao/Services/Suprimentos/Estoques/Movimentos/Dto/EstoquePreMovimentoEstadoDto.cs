using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(EstoquePreMovimentoEstado))]
    public class EstoquePreMovimentoEstadoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
