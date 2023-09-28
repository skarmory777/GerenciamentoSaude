using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposTipoTabelaDominio
{
    [Table("SisGrupoTipoTabelaDominio")]
    public class GrupoTipoTabelaDominio : CamposPadraoCRUD
    {
        public long? TipoTabelaDominioId { get; set; }

        [ForeignKey("TipoTabelaDominioId")]
        public TipoTabelaDominio TipoTabelaDominio { get; set; }

    }
}