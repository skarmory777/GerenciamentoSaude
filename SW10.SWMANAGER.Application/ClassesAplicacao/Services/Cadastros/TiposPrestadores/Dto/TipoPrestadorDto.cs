using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposPrestadores;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto
{
    [AutoMap(typeof(TipoPrestador))]
    public class TipoPrestadorDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        //ACERTAR REFERENCIA
        //public long? ConselhoId { get; set; }
        //[ForeignKey("ConselhoId")]
        //public virtual Conselho Consenho { get; set; }

        public bool IsDescricao { get; set; }

        public bool IsEvolucaoEnfermagem { get; set; }

        public bool IsSolicitaIntervencao { get; set; }

        public bool IsTecnicoExame { get; set; }

        public bool IsAssinaLaudo { get; set; }

        public bool IsNumeroConselhoObrigatorio { get; set; }

        public static TipoPrestadorDto Mapear(TipoPrestador entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<TipoPrestadorDto>(entity);

            dto.Descricao = entity.Descricao;
            dto.IsAssinaLaudo = entity.IsAssinaLaudo;
            dto.IsDescricao = entity.IsDescricao;
            dto.IsEvolucaoEnfermagem = entity.IsEvolucaoEnfermagem;
            dto.IsSolicitaIntervencao = entity.IsSolicitaIntervencao;
            dto.IsTecnicoExame = entity.IsTecnicoExame;
            dto.IsNumeroConselhoObrigatorio = entity.IsNumeroConselhoObrigatorio;
            
            return dto;
        }
    }
}
