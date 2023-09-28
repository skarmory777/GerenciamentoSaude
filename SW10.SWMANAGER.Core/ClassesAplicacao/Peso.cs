using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public abstract class Peso : CamposPadraoCRUD
    {
        [Index("Idx_DataPesagem")]
        public DateTime? DataPesagem { get; set; }

        public double Valor { get; set; }

        public double Altura { get; set; }

        public double PerimetroCefalico { get; set; }

        public double Imc { get { return Math.Round(Valor / (Math.Pow(Altura, 2)), 2); } }


    }
}
