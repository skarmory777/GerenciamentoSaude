using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto
{
    [AutoMap(typeof(Unidade))]
    public class UnidadeDto : CamposPadraoCRUDDto
    {

        /// </summary>
        public string Sigla { get; set; }
        /// <summary>
        /// Fator de multiplicação (1cx = 10un = 1000gr -> 1 comprimido possui 100g cada)
        /// </summary>
        public Decimal Fator { get; set; }

        /// <summary>
        /// Normalmente a unidade de referência é a menor unidade de dispensação do estoque/farmácia
        /// </summary>

        public bool IsReferencia
        {
            get
            {
                return !UnidadeReferenciaId.HasValue;
            }
        }

        //public virtual Unidade UnidadeReferencia { get; set; }

        ///// <summary>
        ///// Quando for unidade de referência, esta é a lista de unidades vinculadas
        ///// </summary>
        //public virtual ICollection<Unidade> Unidades { get; set; }
        public Nullable<long> UnidadeReferenciaId { get; set; }


        public static UnidadeDto Mapear(Unidade unidade)
        {
            if (unidade == null)
            {
                return null;
            }
            
            var unidadeDto = MapearBase<UnidadeDto>(unidade);
            unidadeDto.Sigla = unidade.Sigla;
            unidadeDto.Fator = unidade.Fator;
            unidadeDto.UnidadeReferenciaId = unidade.UnidadeReferenciaId;

            return unidadeDto;

        }


        public static Unidade Mapear(UnidadeDto unidadeDto)
        {
            if (unidadeDto == null)
            {
                return null;
            }

            var unidade = MapearBase<Unidade>(unidadeDto);
            unidade.Sigla = unidadeDto.Sigla;
            unidade.Fator = unidadeDto.Fator;
            unidade.UnidadeReferenciaId = unidadeDto.UnidadeReferenciaId;

            return unidade;

        }
    }
}
