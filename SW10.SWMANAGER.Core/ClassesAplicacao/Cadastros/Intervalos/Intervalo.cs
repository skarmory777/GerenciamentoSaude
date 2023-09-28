using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Intervalos
{
    [Table("AteIntervalo")]
    public class Intervalo : CamposPadraoCRUD
    {
        public string Nome { get; set; }
        public int IntervaloMinutos { get; set; }
        public int AtendimentosPorHora { get { return Math.Abs(60 / IntervaloMinutos); } }
    }
}
