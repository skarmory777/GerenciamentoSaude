using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto
{
    [AutoMap(typeof(Frequencia))]
    public class FrequenciaDto : CamposPadraoCRUDDto
    {
        public long Intervalo { get; set; }
        public string Horarios { get; set; }
        public string HoraInicialMedicacao { get; set; }
        public bool IsSos { get; set; }


        public static FrequenciaDto Mapear(Frequencia input)
        {
            if (input == null)
            {
                return null;
            }
            var result = MapearBase<FrequenciaDto>(input);
            result.HoraInicialMedicacao = input.HoraInicialMedicacao;
            result.Horarios = input.Horarios;
            result.Intervalo = input.Intervalo;
            result.IsSos = input.IsSos;
            return result;
        }

        public static Frequencia Mapear(FrequenciaDto input)
        {
            if (input == null)
            {
                return null;
            }
            var result = MapearBase<Frequencia>(input);
            result.HoraInicialMedicacao = input.HoraInicialMedicacao;
            result.Horarios = input.Horarios;
            result.Intervalo = input.Intervalo;
            result.IsSos = input.IsSos;
            return result;
        }

        public static IEnumerable<FrequenciaDto> Mapear(List<Frequencia> input)
        {
            foreach (var item in input)
            {
                yield return Mapear(item);
            }
        }

        public static IEnumerable<Frequencia> Mapear(List<FrequenciaDto> input)
        {
            foreach (var item in input)
            {
                yield return Mapear(item);
            }
        }

    }
}
