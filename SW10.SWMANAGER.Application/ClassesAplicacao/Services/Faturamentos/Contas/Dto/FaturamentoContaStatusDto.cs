using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto
{
    [AutoMap(typeof(FaturamentoContaStatus))]
    public class FaturamentoContaStatusDto : CamposPadraoCRUDDto
    {
        public const int Inicial = 1;
        
        public const int AuditoriaInterna = 6;
        
        public const int AuditoriaExterna = 7;

        public const int Conferido = 2;
        
        public const int Lote = 4;
        
        public const int Entregue = 3;
        
        public const int Pendente = 5;
        
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public string Cor { get; set; }


        public static FaturamentoContaStatusDto Mapear(FaturamentoContaStatus faturamentoContaStatus)
        {
            var faturamentoContaStatusDto = new FaturamentoContaStatusDto
            {
                Id = faturamentoContaStatus.Id,
                Codigo = faturamentoContaStatus.Codigo,
                Descricao = faturamentoContaStatus.Descricao,
                Cor = faturamentoContaStatus.Cor
            };

            return faturamentoContaStatusDto;
        }
        
        public static FaturamentoContaStatus Mapear(FaturamentoContaStatusDto dto)
        {
            return new FaturamentoContaStatus
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                Descricao = dto.Descricao,
                Cor = dto.Cor
            };
        }
    }
}
