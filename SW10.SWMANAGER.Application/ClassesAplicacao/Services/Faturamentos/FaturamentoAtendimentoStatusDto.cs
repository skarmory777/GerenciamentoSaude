using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos
{
    public class FaturamentoAtendimentoStatusDto: CamposPadraoCRUDDto
    {
        public string Cor { get; set; }

        public static FaturamentoAtendimentoStatusDto Mapear(FaturamentoAtendimentoStatus entity)
        {
            var dto = MapearBase<FaturamentoAtendimentoStatusDto>(entity);
            dto.Cor = entity.Cor;
            return dto;
        }
        
        public static FaturamentoAtendimentoStatus Mapear(FaturamentoAtendimentoStatusDto dto)
        {
            var entity = MapearBase<FaturamentoAtendimentoStatus>(dto);
            entity.Cor = dto.Cor;
            return entity;
        }
    }
}