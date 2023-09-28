using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos
{
    public class FaturarAtendimentoContaMedicaIndexDto: CamposPadraoCRUDDto
    {
        public long? StatusId { get; set; }
        public string FatContaStatus { get; set; }
        public string FatContaStatusCor { get; set; }


        public long? AtendimentoId { get; set; }
            
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        
        public long? DiasFaturados { get; set; }
        
        public string Convenio { get; set; }
        public string Plano { get; set; }

        public DateTime? DataConferencia { get; set; }
        public long? UsuarioConferenciaId { get; set; }
        public string UsuarioConferencia { get; set; }

        public float ValorTotal { get; set; }
        
        public string Observacao { get; set; }
        
        public string MotivoPendencia { get; set; }
    }


    public class FaturarItemAtendimentoIndexDto : CamposPadraoCRUDDto
    {
        public DateTime? Data { get; set; }
        public double? Quantidade { get; set; }
        
        public string Entidade { get; set; }
        public long? EntidadeId { get; set; }
        
        public long? FatItemId { get; set; }
        
        public string Tipo { get; set; }
        
        public string MedicoNomeCompleto { get; set; }
    }

    public class FaturarItemAtendimentoAssPrescricaoItemResposta
    {
        public long AssPrescricaoItemRespostaId { get; set; }
        public long? FatItemId { get; set; }
        public string AssPrescricaoItemDescricao { get; set; }
        
        public string FatItemDescricao { get; set; }
    }
}