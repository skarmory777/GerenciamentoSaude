using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(LeitoStatus))]
    public class CriarOuEditarLeitoStatus : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string Cor { get; set; }

        public bool IsBloqueioAtendimento { get; set; }

        public static LeitoStatus Mapear(CriarOuEditarLeitoStatus dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<LeitoStatus>(dto);
            entity.Cor = dto.Cor;
            entity.IsBloqueioAtendimento = dto.IsBloqueioAtendimento;

            return entity;
        }

        public static CriarOuEditarLeitoStatus Mapear(LeitoStatus entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<CriarOuEditarLeitoStatus>(entity);
            dto.Cor = entity.Cor;
            dto.IsBloqueioAtendimento = entity.IsBloqueioAtendimento;

            return dto;
        }
    }
}
