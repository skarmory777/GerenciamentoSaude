using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Intervalos;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto
{
    [AutoMap(typeof(Intervalo))]
    public class CriarOuEditarIntervalo : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
        public int IntervaloMinutos { get; set; }
        public int AtendimentosPorHora { get { return Math.Abs(60 / IntervaloMinutos); } }
    }
}
