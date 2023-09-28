using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto
{
    [AutoMap(typeof(UnidadeInternacao))]
    public class CriarOuEditarUnidadeInternacao : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string Localizacao { get; set; }

        public bool IsHospitalDia { get; set; }

        public bool IsAtivo { get; set; }

        public long? UnidadeInternacaoTipoId { get; set; }

        public virtual UnidadeInternacaoTipoDto UnidadeInternacaoTipo { get; set; }

        public static UnidadeInternacao Mapear(CriarOuEditarUnidadeInternacao dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<UnidadeInternacao>(dto);
            entity.Localizacao = dto.Localizacao;
            entity.IsHospitalDia = dto.IsHospitalDia;
            entity.IsAtivo = dto.IsAtivo;
            entity.UnidadeInternacaoTipoId = dto.UnidadeInternacaoTipoId;
            entity.UnidadeInternacaoTipo = UnidadeInternacaoTipoDto.Mapear(dto.UnidadeInternacaoTipo);

            return entity;
        }

        public static CriarOuEditarUnidadeInternacao Mapear(UnidadeInternacao entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<CriarOuEditarUnidadeInternacao>(entity);
            dto.Localizacao = entity.Localizacao;
            dto.IsHospitalDia = entity.IsHospitalDia;
            dto.IsAtivo = entity.IsAtivo;
            dto.UnidadeInternacaoTipoId = entity.UnidadeInternacaoTipoId;
            dto.UnidadeInternacaoTipo = UnidadeInternacaoTipoDto.Mapear(entity.UnidadeInternacaoTipo);

            return dto;
        }
    }
}
