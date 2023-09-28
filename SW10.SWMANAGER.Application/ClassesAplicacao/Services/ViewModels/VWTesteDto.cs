using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    [AutoMap(typeof(VWTeste))]
    public class VWTesteDto : EntityDto<long>
    {
        public long PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public long CidadeId { get; set; }
        public string NomeCidade { get; set; }
        public long EstadoId { get; set; }
        public string NomeEstado { get; set; }

        public static VWTesteDto Mapear(VWTeste vwTeste)
        {
            var vwTesteDto = new VWTesteDto();

            vwTesteDto.Id = vwTeste.Id;
            vwTesteDto.PacienteId = vwTeste.PacienteId;
            vwTesteDto.NomePaciente = vwTeste.NomePaciente;
            vwTesteDto.CidadeId = vwTeste.CidadeId;
            vwTesteDto.NomeCidade = vwTeste.NomeCidade;
            vwTesteDto.EstadoId = vwTeste.EstadoId;
            vwTesteDto.NomeEstado = vwTeste.NomeEstado;

            return vwTesteDto;
        }

        public static List<VWTesteDto> Mapear(List<VWTeste> vwTestes)
        {
            var vwTestesDto = new List<VWTesteDto>();

            foreach (var item in vwTestes)
            {
                vwTestesDto.Add(Mapear(item));
            }

            return vwTestesDto;
        }
    }
}
