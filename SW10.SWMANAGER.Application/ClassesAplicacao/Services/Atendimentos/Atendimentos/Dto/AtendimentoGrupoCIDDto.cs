using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class AtendimentoGrupoCIDDto : CamposPadraoCRUDDto
    {
        public long AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }
        public long GrupoCIDId { get; set; }
        public GrupoCIDDto GrupoCID { get; set; }

        public static AtendimentoGrupoCIDDto Mapear(AtendimentoGrupoCID atendimentoGrupoCID)
        {
            AtendimentoGrupoCIDDto atendimentoGrupoCIDDto = new AtendimentoGrupoCIDDto();

            atendimentoGrupoCIDDto.Id = atendimentoGrupoCID.Id;
            atendimentoGrupoCIDDto.Codigo = atendimentoGrupoCID.Codigo;
            atendimentoGrupoCIDDto.Descricao = atendimentoGrupoCID.Descricao;
            atendimentoGrupoCIDDto.AtendimentoId = atendimentoGrupoCID.AtendimentoId;
            atendimentoGrupoCIDDto.GrupoCIDId = atendimentoGrupoCID.GrupoCIDId;

            if (atendimentoGrupoCID.GrupoCID != null)
            {
                atendimentoGrupoCIDDto.GrupoCID = new GrupoCIDDto { Id = atendimentoGrupoCID.GrupoCID.Id, Codigo = atendimentoGrupoCID.GrupoCID.Codigo, Descricao = atendimentoGrupoCID.GrupoCID.Descricao };
            }

            return atendimentoGrupoCIDDto;
        }
    }
}
