using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto
{
    [AutoMap(typeof(MotivoAltaTipoAlta))]
    public class CriarOuEditarMotivoAltaTipoAlta : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static MotivoAltaTipoAlta Mapear(CriarOuEditarMotivoAltaTipoAlta dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<MotivoAltaTipoAlta>(dto);
            
            return entity;
        }

        public static CriarOuEditarMotivoAltaTipoAlta Mapear(MotivoAltaTipoAlta entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<CriarOuEditarMotivoAltaTipoAlta>(entity);

            return dto;
        }
    }
}
