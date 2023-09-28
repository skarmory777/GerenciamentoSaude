using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Modulos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto
{
    [AutoMap(typeof(Modulo))]
    public class ModuloDto : CamposPadraoCRUDDto, IDescricao
    {
        public string Descricao { get; set; }

        public static ModuloDto Mapear(Modulo entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<ModuloDto>(entity);

            dto.Descricao = entity.Descricao;
            return dto;
        }
    }
}
