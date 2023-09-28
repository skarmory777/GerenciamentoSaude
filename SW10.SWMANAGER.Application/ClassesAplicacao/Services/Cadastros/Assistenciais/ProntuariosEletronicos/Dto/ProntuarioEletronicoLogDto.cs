using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.PrescricoesLogs;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto
{
    [AutoMap(typeof(ProntuarioLog))]
    public class ProntuarioEletronicoLogDto : CamposPadraoCRUDDto
    {
        public long? ProntuarioId { get; set; }

        public ProntuarioEletronicoDto Prontuario { get; set; }

        public long? UserId { get; set; }

        //public User User { get; set; }

        public string Anterior { get; set; }

        public string Lancamento { get; set; }

        public static ProntuarioEletronicoLogDto Mapear(ProntuarioLog entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<ProntuarioEletronicoLogDto>(entity);

            dto.ProntuarioId = entity.ProntuarioId;
            dto.Prontuario = ProntuarioEletronicoDto.Mapear(entity.Prontuario);

            dto.UserId = entity.UserId;

            dto.Anterior = entity.Anterior;
            dto.Lancamento = entity.Lancamento;
            return dto;
        }
    }
}
