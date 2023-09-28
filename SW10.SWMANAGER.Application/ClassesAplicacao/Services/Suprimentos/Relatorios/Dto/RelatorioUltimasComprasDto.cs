using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto
{
    public class RelatorioUltimasComprasDto
    {
        public long? EmpresaId { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        

        public long? Rank { get; set; }

        public long? EstoqueId { get; set; }

        public long? GrupoId { get; set; }

        public long? ProdutoId { get; set; }

        public long? LaboratorioId { get; set; }

        public long? FornecedorId { get; set; }

        public string ProdutoDescricao { get; set; }

        public long CasasDecimais { get; set; }

        public double? VariacaoInicial { get; set; }

        public double? VariacaoFinal { get; set; }

    }
}
