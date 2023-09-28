using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    [AutoMap(typeof(AtendimentoStatus))]
    public class AtendimentoStatusDto : CamposPadraoCRUDDto
    {
        public static long Aguardando = 1;

        public static long EmAtendimento = 2;

        public static long Pendente = 3;

        public static long Atendido = 4;

        public string CorFundo { get; set; }

        public string CorTexto { get; set; }


        public static AtendimentoStatusDto Mapear(AtendimentoStatus entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<AtendimentoStatusDto>(entity);

            dto.CorFundo = entity.CorFundo;
            dto.CorTexto = entity.CorTexto;

            return dto;
        }
    }
}
