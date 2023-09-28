using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinDocumentoRateio")]
    public class DocumentoRateio : CamposPadraoCRUD
    {
        public long? DocumentoId { get; set; }
        public long CentroCustoId { get; set; }
        public long ContaAdministrativaId { get; set; }
        public long EmpresaId { get; set; }
        public decimal? Valor { get; set; }
        public bool IsCredito { get; set; }
        public string Observacao { get; set; }
        public bool IsImposto { get; set; }

        [ForeignKey("CentroCustoId")]
        public CentroCusto CentroCusto { get; set; }

        [ForeignKey("ContaAdministrativaId")]
        public ContaAdministrativa ContaAdministrativa { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        [ForeignKey("DocumentoId")]
        public Documento Documento { get; set; }
    }
}
