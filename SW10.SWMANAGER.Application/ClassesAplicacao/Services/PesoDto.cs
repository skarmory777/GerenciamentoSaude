using Abp.AutoMapper;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(Peso))]
    public abstract class PesoDto : CamposPadraoCRUDDto
    {
        public DateTime DataPesagem { get; set; }

        public double Valor { get; set; }

        public double Altura { get; set; }

        public double PerimetroCefalico { get; set; }

        public double Imc { get { return Math.Round(Valor / (Math.Pow(Altura, 2)), 2); } }

    }
}
