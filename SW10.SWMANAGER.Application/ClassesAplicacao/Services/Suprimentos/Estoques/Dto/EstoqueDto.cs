using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    [AutoMap(typeof(Estoque))]
    public class EstoqueDto : CamposPadraoCRUDDto
    {
        public EstoqueDto() { }

        public EstoqueDto(Estoque estoque)
        {
            MapearBase(estoque, this);
        }

        /// <summary>
        /// Descrição Estoque
        /// </summary>
        //public string Descricao { get; set; }

        /// <summary>
        /// Lista
        /// </summary>
        //public ICollection<EstoqueGrupo> EstoquesGrupo { get; set; }

        public ICollection<MultiSelectItem> EstoquesGrupo { get; set; }

        public long[] aEstoquesGrupo { get; set; }

        public static List<EstoqueDto> Mapear(List<Estoque> estoques)
        {
            if (estoques == null)
            {
                return null;
            }

            var estoquesDto = new List<EstoqueDto>();

            foreach (var item in estoques)
            {
                estoquesDto.Add(MapearBase<EstoqueDto>(item));
            }

            return estoquesDto;
        }

        public static EstoqueDto Mapear(Estoque estoque)
        {
            if (estoque == null)
            {
                return null;
            }

            return MapearBase<EstoqueDto>(estoque);
        }

        public static Estoque Mapear(EstoqueDto estoque)
        {
            if (estoque == null)
            {
                return null;
            }

            return MapearBase<Estoque>(estoque);
        }
    }

    public class MultiSelectItem
    {
        public long id { get; set; }
        public bool checado { get; set; }
    }
}
