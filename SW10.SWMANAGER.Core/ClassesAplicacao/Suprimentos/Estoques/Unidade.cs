using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_Unidade")]
    public class Unidade : CamposPadraoCRUD, IDescricao
    {
        /// <summary>
        /// Sigla para unidade (kg, ml, UNI, AMP, CX)
        /// </summary>
        public string Sigla { get; set; }

        /// <summary>
        /// Fator de multiplicação (1cx = 10un = 1000gr -> 1 comprimido possui 100g cada)
        /// </summary>
        public Decimal Fator { get; set; }

        /// <summary>
        /// Normalmente a unidade de referência é a menor unidade de dispensação do estoque/farmácia
        /// </summary>
        [NotMapped]
        public bool IsReferencia
        {
            get
            {
                return !UnidadeReferenciaId.HasValue;
            }
        }

        [ForeignKey("UnidadeReferenciaId")]
        public Unidade UnidadeReferencia { get; set; }
        public Nullable<long> UnidadeReferenciaId { get; set; }

        /// <summary>
        /// Quando for unidade de referência, esta é a lista de unidades vinculadas
        /// </summary>
        public ICollection<Unidade> Unidades { get; set; }
    }
}
