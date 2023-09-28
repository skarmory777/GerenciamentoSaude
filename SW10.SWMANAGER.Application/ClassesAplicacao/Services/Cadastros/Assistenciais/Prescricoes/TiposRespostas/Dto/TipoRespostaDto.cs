using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto
{
    [AutoMap(typeof(TipoResposta))]
    public class TipoRespostaDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public virtual ICollection<TipoRespostaTipoRespostaConfiguracaoDto> TipoRespostaConfiguracoes { get; set; }

        public static TipoResposta Mapear(TipoRespostaDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<TipoResposta>(dto);
            entity.TipoRespostaConfiguracoes = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(dto.TipoRespostaConfiguracoes.ToList());

            return entity;
        }

        public static TipoRespostaDto Mapear(TipoResposta entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<TipoRespostaDto>(entity);
            dto.TipoRespostaConfiguracoes = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(entity.TipoRespostaConfiguracoes.ToList());

            return dto;
        }
    }
}
