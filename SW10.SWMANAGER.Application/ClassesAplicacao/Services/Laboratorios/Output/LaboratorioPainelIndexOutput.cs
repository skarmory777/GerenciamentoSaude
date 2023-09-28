using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    public class LaboratorioPainelIndexOutput: CamposPadraoCRUDDto
    {
        public string TipoAtendimento { get; set; }
        public int TipoAtendimentoId { get; set; }
        public string LeitoNome { get; set; }
        public int LeitoId { get; set; }
        public string Protocolo { get; set; }

        public string Prioridade { get; set; }
        public int StatusId { get; set; }
        public string StatusNome { get; set; }
        public long PacienteId { get; set; }
        public long? AtendimentoId { get; set; }
        public string NomePaciente { get; set; }
        public DateTime? DataAtendimento { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public long ConvenioId { get; set; }
        public string ConvenioNome { get; set; }
        public long? SolicitanteId { get; set; }
        public string Solicitante { get; set; }
        public string QtdExames { get; set; }
        
        public string CodigoColeta { get; set; }
        public long? ResultadoStatusId { get; set; }
        public string ResultadoStatusCorFonte { get; set; }
        public string ResultadoStatusCorFundo { get; set; }
        public string ResultadoStatusDescricao { get; set; }
    }

    public class BuscarPorSolicitacaoDto
    {
        public long ResultadoId { get; set; }
        public long SolicitacaoId { get; set; }
        public bool HasBaixa { get; set; }
        public List<long> ResultadoExameIds { get; set; }
        
    }
}
