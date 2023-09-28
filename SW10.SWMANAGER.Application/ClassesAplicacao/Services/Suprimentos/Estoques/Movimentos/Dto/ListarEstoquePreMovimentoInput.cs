using Abp.Runtime.Validation;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class ListarEstoquePreMovimentoInput : ListarInput, IShouldNormalize
    {
        public string Documento { get; set; }
        public long? FornecedorId { get; set; }
        public DateTime? PeridoDe { get; set; }
        public DateTime? PeridoAte { get; set; }
        public bool EntradaConfirmada { get; set; }
        public long? TipoMovimentoId { get; set; }
        public long? TipoOperacaoId { get; set; }
        public long? MovimentoId { get; set; }

        public long? StatusMovimentoId { get; set; }

        public long[] StatusMovimentoIds { get; set; }
        public long? EstoqueId { get; set; }

        public bool? IsEntrada { get; set; }

        public override void Normalize()
        {
            if (Sorting == null)
            {
                Sorting = "Documento";
            }
        }
    }
}
