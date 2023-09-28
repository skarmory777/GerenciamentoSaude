using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(Turno))]
    public class TurnoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }


        public static Turno Mapear(TurnoDto turnoDto)
        {
            Turno turno = new Turno();

            turno.Id = turnoDto.Id;
            turno.Codigo = turnoDto.Codigo;
            turno.Descricao = turnoDto.Descricao;

            return turno;
        }

        public static TurnoDto Mapear(Turno turno)
        {
            TurnoDto turnoDto = new TurnoDto();

            turnoDto.Id = turno.Id;
            turnoDto.Codigo = turno.Codigo;
            turnoDto.Descricao = turno.Descricao;

            return turnoDto;
        }


    }
}
