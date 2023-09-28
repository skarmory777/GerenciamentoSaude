using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(LeitoCaracteristica))]
    public class CriarOuEditarLeitoCaracteristica : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string Ramal { get; set; }

        public static LeitoCaracteristica Mapear(CriarOuEditarLeitoCaracteristica dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<LeitoCaracteristica>(dto);
            entity.Ramal = dto.Ramal;

            return entity;
        }

        public static CriarOuEditarLeitoCaracteristica Mapear(LeitoCaracteristica entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<CriarOuEditarLeitoCaracteristica>(entity);
            dto.Ramal = entity.Ramal;

            return dto;
        }
    }
}