using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(DocumentoRateio))]
    public class DocumentoRateioDto : CamposPadraoCRUDDto
    {
        public long? DocumentoId { get; set; }
        public long CentroCustoId { get; set; }
        public long ContaAdministrativaId { get; set; }
        public long EmpresaId { get; set; }
        public decimal? Valor { get; set; }
        public bool IsCredito { get; set; }
        public string Observacao { get; set; }
        public bool IsImposto { get; set; }

        public CentroCustoDto CentroCusto { get; set; }
        public ContaAdministrativaDto ContaAdministrativa { get; set; }
        public EmpresaDto Empresa { get; set; }
        public DocumentoDto Documento { get; set; }
    }
}
