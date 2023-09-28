using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class AgendamentoMaterialOPMEJson
    {
        public long? Id { get; set; }
        public string Material { get; set; }
        public decimal? QuantidadeMaterial { get; set; }
        public DateTime? DataPrevista { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public string NumeroNota { get; set; }
        public decimal? ValorNota { get; set; }
        public bool IsCobraPeloHospital { get; set; }
        public long? FornecedorId { get; set; }
        public string FornecedorDescricao { get; set; }

        public long IdGrid { get; set; }
    }
}
