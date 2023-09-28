using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class EstoqueTransferenciaProdutoDto : CamposPadraoCRUDDto
    {
        public long PreMovimentoSaidaId { get; set; }
        public long PreMovimentoEntradaId { get; set; }
        public long EstoqueSaidaId { get; set; }
        public long EstoqueEntradaId { get; set; }
        public DateTimeOffset Movimento { get; set; }
        public long EmpresaId { get; set; }
        public string EstoqueEntrada { get; set; }
        public string EstoqueSaida { get; set; }
        public string Empresa { get; set; }
        public string Usuario { get; set; }
        public string Documento { get; set; }
        public long PreMovimentoEstadoId { get; set; }

        public EstoquePreMovimentoDto PreMovimentoEntrada { get; set; }
        public EstoquePreMovimentoDto PreMovimentoSaida { get; set; }
    }
}
