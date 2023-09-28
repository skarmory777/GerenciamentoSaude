using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AltasMedicas
{
    [Table("AteAltaMedica")]
    public class AltaMedica : CamposPadraoCRUD
    {
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        [ForeignKey("Leito"), Column("AteLeitoId")]
        public long? LeitoId { get; set; }
        public Leito Leito { get; set; }
    }
}
