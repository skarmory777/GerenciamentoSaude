using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    public class PainelTipoLocalChamadaDto : CamposPadraoCRUDDto
    {
        public long? TipoLocalChamadaId { get; set; }
        public long? PainelId { get; set; }

        public TipoLocalChamadaDto TipoLocalChamada { get; set; }
        public PainelDto Painel { get; set; }


        public static PainelTipoLocalChamadaDto Mapear(PainelTipoLocalChamada painelTipoLocalChamada)
        {
            PainelTipoLocalChamadaDto painelTipoLocalChamadaDto = new PainelTipoLocalChamadaDto();

            painelTipoLocalChamadaDto.Id = painelTipoLocalChamada.Id;
            painelTipoLocalChamadaDto.Codigo = painelTipoLocalChamada.Codigo;
            painelTipoLocalChamadaDto.Descricao = painelTipoLocalChamada.Descricao;
            painelTipoLocalChamadaDto.TipoLocalChamadaId = painelTipoLocalChamada.TipoLocalChamadaId;
            painelTipoLocalChamadaDto.PainelId = painelTipoLocalChamada.PainelId;


            //if(painelTipoLocalChamada.Painel!=null)
            //{
            //    painelTipoLocalChamadaDto.Painel = PainelDto.Mapear(painelTipoLocalChamada.Painel);
            //}

            if (painelTipoLocalChamada.TipoLocalChamada != null)
            {
                painelTipoLocalChamadaDto.TipoLocalChamada = TipoLocalChamadaDto.Mapear(painelTipoLocalChamada.TipoLocalChamada);
            }


            return painelTipoLocalChamadaDto;
        }

        public static PainelTipoLocalChamada Mapear(PainelTipoLocalChamadaDto painelTipoLocalChamadaDto)
        {
            PainelTipoLocalChamada painelTipoLocalChamada = new PainelTipoLocalChamada();

            painelTipoLocalChamada.Id = painelTipoLocalChamadaDto.Id;
            painelTipoLocalChamada.Codigo = painelTipoLocalChamadaDto.Codigo;
            painelTipoLocalChamada.Descricao = painelTipoLocalChamadaDto.Descricao;
            painelTipoLocalChamada.TipoLocalChamadaId = painelTipoLocalChamadaDto.TipoLocalChamadaId;
            painelTipoLocalChamada.PainelId = painelTipoLocalChamadaDto.PainelId;


            if (painelTipoLocalChamadaDto.Painel != null)
            {
                painelTipoLocalChamada.Painel = PainelDto.Mapear(painelTipoLocalChamadaDto.Painel);
            }

            if (painelTipoLocalChamadaDto.TipoLocalChamada != null)
            {
                painelTipoLocalChamada.TipoLocalChamada = TipoLocalChamadaDto.Mapear(painelTipoLocalChamadaDto.TipoLocalChamada);
            }

            return painelTipoLocalChamada;
        }


    }
}
