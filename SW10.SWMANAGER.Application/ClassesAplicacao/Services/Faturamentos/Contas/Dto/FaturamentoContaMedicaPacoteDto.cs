using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto
{
    public class FaturamentoContaMedicaPacoteDto: CamposPadraoCRUDDto
    {
        public long? FaturamentoContaMedicaId { get; set; }
        public DateTime? DataInicio { get; set; }
        
        public DateTime? DataFinal { get; set; }

        public long? FaturamentoItemId { get; set; }
        public string FaturamentoItemDescricao { get; set; }
        
        public string FaturamentoItemCodAmb { get; set; }
        
        public string FaturamentoItemCodCbhpm { get; set; }
        
        public string FaturamentoItemCodigo { get; set; }
        
        public string FaturamentoItemCodTuss { get; set; }
        
        public string FaturamentoItemDescricaoTuss { get; set; }
        
        public float Qtde { get; set; }
        public long TotalItensNoPacote { get; set; }
        
    }
}