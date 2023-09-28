using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Feriados;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Dto
{
    [AutoMap(typeof(Feriado))]
    public class FeriadoDto : CamposPadraoCRUDDto
    {
        public DateTime DiaMesAno { get; set; }
        public string Descricao { get; set; }
    }
}
