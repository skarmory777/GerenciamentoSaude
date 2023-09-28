using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto
{
    [AutoMap(typeof(Cfop))]
    public class CriarOuEditarCfop : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
        public long Numero { get; set; }
        public bool Tipo { get; set; }
        public DateTime Vigencia { get; set; }
    }
}
