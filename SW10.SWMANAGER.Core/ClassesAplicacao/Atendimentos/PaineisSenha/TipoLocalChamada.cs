using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AteTipoLocalChamada")]
    public class TipoLocalChamada : CamposPadraoCRUD
    {
        public long? TipoLocalChamadaProximoId { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public List<LocalChamada> LocalChamadas { get; set; }

        [ForeignKey("TipoLocalChamadaProximoId")]
        public TipoLocalChamada TipoLocalChamadaProximo { get; set; }

        [ForeignKey("UnidadeOrganizacionalId")]
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }
    }
}
