using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposVinculosEmpregaticios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto
{
    [AutoMap(typeof(TipoVinculoEmpregaticio))]
    public class TipoVinculoEmpregaticioDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static TipoVinculoEmpregaticioDto Mapear(TipoVinculoEmpregaticio entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<TipoVinculoEmpregaticioDto>(entity);

            dto.Descricao = entity.Descricao;
            return dto;
        }
    }
}
