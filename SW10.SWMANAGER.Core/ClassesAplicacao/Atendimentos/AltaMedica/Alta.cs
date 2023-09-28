using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AltaMedica
{
    [Table("AteAlta")]
    public class Alta : CamposPadraoCRUD
    {
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        public override string Codigo { get; set; }

        [ForeignKey("GrupoCID"), Column("AteGrupoCIDId")]
        public long? GrupoCIDId { get; set; }
        public GrupoCID GrupoCID { get; set; }

        [ForeignKey("MotivoAlta"), Column("AteMotivoAltaId")]
        public long? MotivoAltaId { get; set; }
        public MotivoAlta MotivoAlta { get; set; }
        
    }
}
