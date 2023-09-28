using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    [AutoMap(typeof(EstoqueLocalizacao))]
    public class EstoqueLocalizacaoDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Código Localização Estoque
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Descrição Localização Estoque
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Estoque
        /// </summary>
        public long EstoqueId { get; set; }
        public virtual EstoqueDto Estoque { get; set; }
    }
}
