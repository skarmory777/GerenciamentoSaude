using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes
{
    [Table("AssTipoPrescricao")]
    public class TipoPrescricao : CamposPadraoCRUD, IDescricao
    {
        [StringLength(60)]
        public override string Descricao { get; set; }
    }
}
