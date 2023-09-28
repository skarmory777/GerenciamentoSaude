using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    public class LocalChamadaDto : CamposPadraoCRUDDto
    {
        public long? TipoLocalChamadaId { get; set; }

        public TipoLocalChamadaDto TipoLocalChamada { get; set; }

        public static LocalChamadaDto Mapear(LocalChamada localChamada)
        {
            LocalChamadaDto localChamadaDto = new LocalChamadaDto();

            localChamadaDto.Id = localChamada.Id;
            localChamadaDto.Codigo = localChamada.Codigo;
            localChamadaDto.Descricao = localChamada.Descricao;
            localChamadaDto.TipoLocalChamadaId = localChamada.TipoLocalChamadaId;

            if (localChamada.TipoLocalChamada != null)
            {
                localChamadaDto.TipoLocalChamada = TipoLocalChamadaDto.Mapear(localChamada.TipoLocalChamada);
            }

            return localChamadaDto;
        }

        public static LocalChamada Mapear(LocalChamadaDto localChamadaDto)
        {
            LocalChamada localChamada = new LocalChamada();

            localChamada.Id = localChamadaDto.Id;
            localChamada.Codigo = localChamadaDto.Codigo;
            localChamada.Descricao = localChamadaDto.Descricao;
            localChamada.TipoLocalChamadaId = localChamadaDto.TipoLocalChamadaId;

            if (localChamadaDto.TipoLocalChamada != null)
            {
                localChamada.TipoLocalChamada = TipoLocalChamadaDto.Mapear(localChamadaDto.TipoLocalChamada);
            }


            return localChamada;
        }

    }
}
