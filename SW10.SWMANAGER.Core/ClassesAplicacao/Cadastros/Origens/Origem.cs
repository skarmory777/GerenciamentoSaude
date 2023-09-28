using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens
{
    [Table("SisOrigem")]
    public class Origem : CamposPadraoCRUD
    {
        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        public bool IsAtivo { get; set; }

    }
}
