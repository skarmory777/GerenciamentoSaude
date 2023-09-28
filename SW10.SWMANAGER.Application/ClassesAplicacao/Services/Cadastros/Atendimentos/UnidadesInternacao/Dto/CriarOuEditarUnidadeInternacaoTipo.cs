using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto
{
    [AutoMap(typeof(UnidadeInternacaoTipo))]
    public class CriarOuEditarUnidadeInternacaoTipo : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }
        public static UnidadeInternacaoTipo Mapear(CriarOuEditarUnidadeInternacaoTipo dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<UnidadeInternacaoTipo>(dto);

            return entity;
        }

        public static CriarOuEditarUnidadeInternacaoTipo Mapear(UnidadeInternacaoTipo entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<CriarOuEditarUnidadeInternacaoTipo>(entity);

            return dto;
        }
    }


}
