using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class TipoCirurgiaDto : CamposPadraoCRUDDto
    {
        public static TipoCirurgiaDto Mapear(TipoCirurgia tipoCirurgia)
        {
            TipoCirurgiaDto tipoCirurgiaDto = new TipoCirurgiaDto();

            tipoCirurgiaDto.Id = tipoCirurgia.Id;
            tipoCirurgiaDto.Codigo = tipoCirurgia.Codigo;
            tipoCirurgiaDto.Descricao = tipoCirurgia.Descricao;

            return tipoCirurgiaDto;
        }
    }
}
