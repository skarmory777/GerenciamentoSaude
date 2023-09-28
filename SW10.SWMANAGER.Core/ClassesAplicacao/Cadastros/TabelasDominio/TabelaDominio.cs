using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio
{
    [Table("SisTabelaDominio")]
    public class TabelaDominio : CamposPadraoCRUD
    {
        public long? TipoTabelaDominioId { get; set; }

        //     public long? VersaoTissId { get; set; }

        public long? GrupoTipoTabelaDominioId { get; set; }

        [ForeignKey("TipoTabelaDominioId")]
        public TipoTabelaDominio TipoTabelaDominio { get; set; }

        [ForeignKey("GrupoTipoTabelaDominioId")]
        public GrupoTipoTabelaDominio GrupoTipoTabelaDominio { get; set; }

        public ICollection<TabelaDominioVersaoTiss> TabelaDominioVersoesTiss { get; set; }
    }
}
