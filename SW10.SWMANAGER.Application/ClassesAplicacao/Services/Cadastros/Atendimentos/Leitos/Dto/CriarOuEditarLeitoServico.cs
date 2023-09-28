using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(LeitoServico))]
    public class CriarOuEditarLeitoServico : CamposPadraoCRUDDto
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        [StringLength(10)]
        public string Ramal { get; set; }

        public static LeitoServico Mapear(CriarOuEditarLeitoServico dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<LeitoServico>(dto);
            entity.Ramal = dto.Ramal;

            return entity;
        }

        public static CriarOuEditarLeitoServico Mapear(LeitoServico entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<CriarOuEditarLeitoServico>(entity);
            dto.Ramal = entity.Ramal;

            return dto;
        }
    }
}