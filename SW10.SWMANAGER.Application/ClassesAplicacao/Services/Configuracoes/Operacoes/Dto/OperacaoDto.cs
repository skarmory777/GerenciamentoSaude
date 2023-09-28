using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto
{
    [AutoMap(typeof(Operacao))]
    public class OperacaoDto : CamposPadraoCRUDDto
    {
        public bool IsFormulario { get; set; }

        public bool IsEspecialidade { get; set; }

        public string Name { get; set; }

        public long? ModuloId { get; set; }

        public ModuloDto Modulo { get; set; }

        public long? PaginaId { get; set; }

        public static OperacaoDto Mapear(Operacao entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<OperacaoDto>(entity);

            dto.Name = entity.Name;
            dto.IsEspecialidade = entity.IsEspecialidade;
            dto.IsFormulario = entity.IsFormulario;
            dto.ModuloId = entity.ModuloId;
            dto.Modulo = ModuloDto.Mapear(entity.Modulo);

            return dto;
        }
    }
}
