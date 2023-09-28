using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class TransferenciaDto
    {
        public long OrigemEmpresaId { get; set; }
        public long DestinoEmpresaId { get; set; }
        public long MeioPagamentoId { get; set; }
        public DateTime OrigemDataMovimento { get; set; }
        public DateTime DestinoDataMovimento { get; set; }
        public long OrigemContaCorrenteId { get; set; }
        public long DestinoContaCorrenteId { get; set; }
        public string OrigemNumero { get; set; }
        public string DestinoNumero { get; set; }
        public decimal Valor { get; set; }
        public string OrigemObservacao { get; set; }
        public string DestinoObservacao { get; set; }
    }
}