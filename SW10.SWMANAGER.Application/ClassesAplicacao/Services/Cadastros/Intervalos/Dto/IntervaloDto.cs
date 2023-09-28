using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Intervalos;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto
{
    [AutoMap(typeof(Intervalo))]
    public class IntervaloDto : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
        public int IntervaloMinutos { get; set; }
        public int AtendimentosPorHora { get { return Math.Abs(60 / IntervaloMinutos); } }


        public static IntervaloDto Mapear(Intervalo intervalo)
        {
            IntervaloDto intervaloDto = new IntervaloDto();

            intervaloDto.Id = intervalo.Id;
            intervaloDto.Codigo = intervalo.Codigo;
            intervaloDto.Descricao = intervalo.Descricao;
            intervaloDto.Nome = intervalo.Nome;
            intervaloDto.IntervaloMinutos = intervalo.IntervaloMinutos;
            return intervaloDto;
        }

        public static Intervalo Mapear(IntervaloDto dto)
        {
            var intervalo = new Intervalo();

            if (dto == null) return null;

            intervalo.Id = dto.Id;
            intervalo.Codigo = dto.Codigo;
            intervalo.Descricao = dto.Descricao;
            intervalo.Nome = dto.Nome;
            intervalo.IntervaloMinutos = dto.IntervaloMinutos;

            return intervalo;
        }

    }
}
