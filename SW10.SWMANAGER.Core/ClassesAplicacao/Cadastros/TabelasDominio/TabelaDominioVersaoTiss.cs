using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio
{
    [Table("SisTabelaDominioVersaoTiss")]
    public class TabelaDominioVersaoTiss : VersaoTissRelacao
    {
        public long TabelaDominioId { get; set; }

        [ForeignKey("TabelaDominioId")]
        public TabelaDominio TabelaDominio { get; set; }

    }
}