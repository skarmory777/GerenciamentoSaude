using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.CodigoCredenciado
{
    [Table("FatCodigoCredenciado")]
    public class CodigoCredenciado : CamposPadraoCRUD
    {
        public bool IsAmbulatorioEmergencia { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsFuturoEspecialidade { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Empresa"), Column("SisEmpresaId")]
        public long? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
