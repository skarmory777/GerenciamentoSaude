using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos
{
    public class FaturarAtendimentoInputDto : ListarInput
    {
        public long? ConvenioId { get; set; }

        public long? PacienteId { get; set; }
        
        public long? TipoInternacao { get; set; }
        
        public string Periodo { get; set; }
        
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataRegistro DESC";
            }
        }
    }
    
    public class ListarContaMedicaInputDto : FaturarAtendimentoDependenciasInputDto
    {
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "FatConta.DataInicio DESC";
            }
        }
    }
    
    public class ListarFaturamentoItemInputDto : FaturarAtendimentoDependenciasInputDto
    {
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "FatItemAtendimento.Id DESC";
            }
        }
    }
    
    public class FaturarAtendimentoDependenciasInputDto : ListarInput
    {
        public long? AtendimentoId { get; set; }
    }
}