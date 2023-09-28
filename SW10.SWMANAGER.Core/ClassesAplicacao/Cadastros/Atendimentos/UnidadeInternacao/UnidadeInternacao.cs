using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao
{
    [Table("UnidadeInternacao")]
    public class UnidadeInternacao : CamposPadraoCRUD
    {
        [StringLength(255)]
        public string Localizacao { get; set; }

        public bool IsHospitalDia { get; set; }

        public bool IsAtivo { get; set; }

        // Falta a propriedade 'SETORES GERAIS ID'

        public long? UnidadeInternacaoTipoId { get; set; }

        [ForeignKey("UnidadeInternacaoTipoId")]
        public UnidadeInternacaoTipo UnidadeInternacaoTipo { get; set; }
    }
}
