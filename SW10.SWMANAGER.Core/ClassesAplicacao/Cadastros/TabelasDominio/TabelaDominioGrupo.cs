using Abp.Domain.Entities;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposTipoTabelaDominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio
{
    [Table("TabelaDominioGrupo")]
    public class TabelaDominioGrupo : Entity<long>
    {
        // ESSE CARA AINDA EXISTE?

        //[Key]
        //public long Id { get; set; }

        public long TabelaDominioId { get; set; }

        public long GrupoTipoTabelaDominioId { get; set; }

        [ForeignKey("TabelaDominioId")]
        public TabelaDominio TabelaDominio { get; set; }

        [ForeignKey("GrupoTipoTabelaDominioId")]
        public GrupoTipoTabelaDominio GrupoTipoTabelaDominio { get; set; }
    }
}
