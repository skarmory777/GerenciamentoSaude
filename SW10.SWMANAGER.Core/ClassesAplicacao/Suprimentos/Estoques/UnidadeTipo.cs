using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_UnidadeTipo")]
    public class UnidadeTipo : CamposPadraoCRUD
    {
        /// <summary>
        /// Descrição do tipo de unidade
        /// (Referência, Gerencial, Compras, Entreda, Estoque)
        /// </summary>
    }
}
