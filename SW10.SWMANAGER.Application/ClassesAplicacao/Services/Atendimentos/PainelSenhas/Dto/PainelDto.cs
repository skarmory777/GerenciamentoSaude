using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    public class PainelDto : CamposPadraoCRUDDto
    {
        public List<PainelTipoLocalChamadaDto> PaineisTipoLocaisChamadasDto { get; set; }

        public string TipoLocalChamadas { get; set; }


        public static PainelDto Mapear(Painel painel)
        {
            PainelDto painelDto = new PainelDto();

            painelDto.Id = painel.Id;
            painelDto.Codigo = painel.Codigo;
            painelDto.Descricao = painel.Descricao;

            if (painel.PaineisTipoLocaisChamadas != null)
            {
                painelDto.PaineisTipoLocaisChamadasDto = new List<PainelTipoLocalChamadaDto>();

                foreach (var item in painel.PaineisTipoLocaisChamadas)
                {
                    painelDto.PaineisTipoLocaisChamadasDto.Add(PainelTipoLocalChamadaDto.Mapear(item));
                }
            }

            return painelDto;
        }

        public static Painel Mapear(PainelDto painelDto)
        {
            Painel painel = new Painel();

            painel.Id = painelDto.Id;
            painel.Codigo = painelDto.Codigo;
            painel.Descricao = painelDto.Descricao;

            if (painelDto.PaineisTipoLocaisChamadasDto != null)
            {
                painel.PaineisTipoLocaisChamadas = new List<PainelTipoLocalChamada>();

                foreach (var item in painelDto.PaineisTipoLocaisChamadasDto)
                {
                    painel.PaineisTipoLocaisChamadas.Add(PainelTipoLocalChamadaDto.Mapear(item));
                }
            }

            return painel;
        }
    }
}
