using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss
{
    [Table("SisVersaoTiss")]
    public class VersaoTiss : CamposPadraoCRUD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [Index("Sis_Idx_DataInicio")]
        public DateTime DataInicio { get; set; }
        [Index("Sis_Idx_DataFim")]
        public DateTime DataFim { get; set; }

        public ICollection<TabelaDominioVersaoTiss> TabelaDominioVersoesTiss { get; set; }
    }
}
