using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Dto
{
    public class SalaCirurgicaDto : CamposPadraoCRUDDto
    {
        public string CorAgendamentoConsulta { get; set; }

        public static SalaCirurgicaDto Mapear(SalaCirurgica salaCirurgica)
        {
            SalaCirurgicaDto salaCirurgicaDto = new SalaCirurgicaDto();

            salaCirurgicaDto.Id = salaCirurgica.Id;
            salaCirurgicaDto.Codigo = salaCirurgica.Codigo;
            salaCirurgicaDto.Descricao = salaCirurgica.Descricao;
            salaCirurgicaDto.CorAgendamentoConsulta = salaCirurgica.CorAgendamento;

            return salaCirurgicaDto;
        }

        public static List<SalaCirurgicaDto> Mapear(List<SalaCirurgica> lstSalaCirurgica)
        {
            List<SalaCirurgicaDto> lstSalaCirurgicaDto = new List<SalaCirurgicaDto>();

            if (lstSalaCirurgica != null)
            {
                foreach (var item in lstSalaCirurgica)
                {
                    lstSalaCirurgicaDto.Add(Mapear(item));
                }
            }

            return lstSalaCirurgicaDto;
        }
    }
}
