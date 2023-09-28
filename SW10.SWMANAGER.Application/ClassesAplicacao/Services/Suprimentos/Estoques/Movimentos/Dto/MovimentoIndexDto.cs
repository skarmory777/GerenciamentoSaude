using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class MovimentoIndexDto: CamposPadraoCRUDDto
    {
        public string Empresa { get; set; }

        public string NomePaciente { get; set; }

        public DateTimeOffset DataEmissaoSaida { get; set; }
        public string Documento { get; set; }
        public string Fornecedor { get; set; }
        public decimal Valor { get; set; }
        public string Usuario { get; set; }
        public long? UsuarioId { get; set; }
        public long PreMovimentoEstadoId { get; set; }
        public string Estoque { get; set; }
        public string TipoMovimento { get; set; }
        public bool IsEntrada { get; set; }
        public long? TipoMovimentoId { get; set; }
        public string TipoDocumento { get; set; }
        public long? TipoDocumentoId { get; set; }
        public long? TipoOperacaoId { get; set; }
        public string TipoOperacao { get; set; }
        public decimal? ValorDocumento { get; set; }
        public long? FornecedorId { get; set; }
        public DateTime? HoraPrescrita { get; set; }

    }
}
