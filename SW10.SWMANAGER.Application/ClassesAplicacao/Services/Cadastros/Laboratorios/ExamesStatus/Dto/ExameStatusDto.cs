using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto
{
    public class ExameStatusDto : CamposPadraoCRUDDto
    {
        public const long Inicial = 1;
        public const long Pendente = 2;
        public const long Digitado = 3;
        public const long Conferido = 4;
        public const long EmColeta = 5;
        public const long Coletado = 6;
        public const long Interfaceado = 7;
        public const long EnviadoEquipamento = 8;
        
        public string Cor { get; set; }

        public static ExameStatusDto Mapear(ExameStatus exameStatus)
        {
            var exameStatusDto = new ExameStatusDto
            {
                Id = exameStatus.Id,
                Codigo = exameStatus.Codigo,
                Descricao = exameStatus.Descricao,
                Cor = exameStatus.Cor
            };

            return exameStatusDto;
        }
        
        public static IEnumerable<ExameStatusDto> Mapear(IEnumerable<ExameStatus> exameStatusList)
        {
            foreach (var exameStatus in exameStatusList)
            {
                yield return Mapear(exameStatus);
            }
        }

        public static ExameStatus Mapear(ExameStatusDto exameStatusDto)
        {
            var exameStatus = new ExameStatus
            {
                Id = exameStatusDto.Id,
                Codigo = exameStatusDto.Codigo,
                Descricao = exameStatusDto.Descricao,
                Cor = exameStatusDto.Cor
            };

            return exameStatus;
        }
    }
}
