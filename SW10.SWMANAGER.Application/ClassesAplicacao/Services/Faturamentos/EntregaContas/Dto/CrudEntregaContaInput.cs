using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto
{
    public class CrudEntregaContaInput
    {
        public long[] ContasIds { get; set; }
        public long UsuarioEntregaId { get; set; }
    }

    public class CrudEntregaLoteContaInput
    {
        public long LoteId { get; set; }
        public FaturamentoEntregaLoteDto Lote { get; set; }
        public long[] ContasIds { get; set; }
        public long ConvenioId { get; set; }
        public string NumeroProcesso { get; set; }
        public string CodigoEntrega { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsAmbulatorio { get; set; }
        public long EmpresaId { get; set; }
    }

    public class CriaLoteInput
    {
        public long LoteId { get; set; }
        public FaturamentoEntregaLoteDto Lote { get; set; }
        public long[] ContaIds { get; set; }
        public long ConvenioId { get; set; }
        public string NumeroProcesso { get; set; }
        public string CodigoEntrega { get; set; }

        public long? TipoInternacao { get; set; }

        public bool IsInternacao { get; set; }
        public bool IsAmbulatorio { get; set; }
        public long EmpresaId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
