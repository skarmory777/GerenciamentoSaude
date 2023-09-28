using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto
{
    [AutoMap(typeof(MotivoAlta))]
    public class CriarOuEditarMotivoAlta : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public long MotivoAltaTipoAltaId { get; set; }

        public static MotivoAlta Mapear(CriarOuEditarMotivoAlta dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<MotivoAlta>(dto);
            entity.MotivoAltaTipoAltaId = dto.MotivoAltaTipoAltaId;

            return entity;
        }

        public static CriarOuEditarMotivoAlta Mapear(MotivoAlta entidade)
        {
            if (entidade == null) return null;

            var dto = MapearBase<CriarOuEditarMotivoAlta>(entidade);
            dto.MotivoAltaTipoAltaId = entidade.MotivoAltaTipoAltaId;

            return dto;
        }
    }
}
