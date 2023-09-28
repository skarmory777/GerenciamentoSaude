using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposParticipacoes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto
{
    [AutoMap(typeof(TipoParticipacao))]
    public class TipoParticipacaoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static TipoParticipacaoDto Mapear(TipoParticipacao entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<TipoParticipacaoDto>(entity);

            dto.Descricao = entity.Descricao;
            return dto;
        }
    }
}
