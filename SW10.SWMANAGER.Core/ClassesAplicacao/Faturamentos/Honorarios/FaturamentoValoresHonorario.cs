using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Honorarios
{
    public class FaturamentoValoresHonorario: CamposPadraoCRUD
    {

        public float PercentualMedico { get; set; }

        public float PercentualAuxiliar1 { get; set; }

        public float PercentualAuxiliar2 { get; set; }

        public float PercentualAuxiliar3 { get; set; }

        public float PercentualInstrumentador { get; set; }

        public float PercentualAnestesista { get; set; }
    }
}
