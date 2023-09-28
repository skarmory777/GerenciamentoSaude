using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto
{
    [AutoMap(typeof(Cfop))]
    public class CfopDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
        public long Numero { get; set; }
        public bool Tipo { get; set; }
        public DateTime Vigencia { get; set; }

        public static CfopDto Mapear(Cfop cfop)
        {
            var cfopDto = new CfopDto();

            cfopDto.Id = cfop.Id;
            cfopDto.Codigo = cfop.Codigo;
            cfopDto.Descricao = cfop.Descricao;
            cfopDto.Numero = cfop.Numero;
            cfopDto.Tipo = cfop.Tipo;
            cfopDto.Vigencia = cfop.Vigencia;

            return cfopDto;
        }
    }
}
