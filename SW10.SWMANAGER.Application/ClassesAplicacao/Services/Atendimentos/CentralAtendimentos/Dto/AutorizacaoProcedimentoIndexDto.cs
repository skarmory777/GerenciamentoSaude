using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto
{
    public class AutorizacaoProcedimentoIndexDto
    {
        public long Id { get; set; }

        public string CodigoAutorizacao { get; set; }
        public bool IsOrtese { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public string Paciente { get; set; }
        public string Convenio { get; set; }
        public string Medico { get; set; }
        public string FaturamentoItem { get; set; }
        public string Especialidade { get; set; }
        public string GrupoItem { get; set; }
        public long? MedicoId { get; set; }
        public long? FaturamentoItemId { get; set; }
        public long ItemId { get; set; }
        public string Status { get; set; }
        public string NumeroGuia { get; set; }
        public int? QuantidadeSolicitada { get; set; }
        public int? QuantidadeAutorizada { get; set; }
    }
}
