using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cbos.Dto
{
    [AutoMap(typeof(Cbo))]
    public class CboDto : CamposPadraoCRUDDto
    {
        public static CboDto Mapear(Cbo cbo)
        {
            if (cbo == null)
            {
                return null;
            }

            CboDto cboDto = MapearBase<CboDto>(cbo);

            cboDto.Id = cbo.Id;
            cboDto.Codigo = cbo.Codigo;
            cboDto.Descricao = cbo.Descricao;

            return cboDto;
        }

        public static Cbo Mapear(CboDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<Cbo>(dto);

            entity.Id = dto.Id;
            entity.Codigo = dto.Codigo;
            entity.Descricao = dto.Descricao;

            return entity;
        }
    }
}
