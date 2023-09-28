using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(SisMoeda))]
    public class SisMoedaDto : CamposPadraoCRUDDto
    {
        // public string Codigo { get; set; }
        //
        // public string Descricao { get; set; }

        // 1 - fixa
        // 2 - variavel por convenio
        public int Tipo { get; set; }

        public bool IsCobraCoch { get; set; }

        public static SisMoedaDto Mapear(SisMoeda entity)
        {
            var dto = MapearBase<SisMoedaDto>(entity);
            dto.Tipo = entity.Tipo;
            dto.IsCobraCoch = entity.IsCobraCoch;
            return dto;
        }
        
        
        public static SisMoeda Mapear(SisMoedaDto dto)
        {
            var entity = MapearBase<SisMoeda>(dto);
            entity.Tipo = dto.Tipo;
            entity.IsCobraCoch = dto.IsCobraCoch;
            return entity;
        }
        
        
    }
}
