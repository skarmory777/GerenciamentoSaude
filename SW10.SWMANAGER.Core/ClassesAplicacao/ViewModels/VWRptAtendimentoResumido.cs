using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("vwRptAteResumido")]
    public class VWRptAtendimentoResumido : Entity<long>
    {
        public string Empresa { get; set; }
        public long? EmpresaId { get; set; }
        public string Convenio { get; set; }
        public long? ConvenioId { get; set; }
        public string Plano { get; set; }
        public string Medico { get; set; }
        public long? MedicoId { get; set; }
        public string Especialidade { get; set; }
        public int Atendimentos { get; set; }
        public int Internacoes { get; set; }
        public int InternacoesAtivas { get; set; }
        public int HomeCare { get; set; }
        public int AmbulatorioEmergencia { get; set; }
        public int PreAtendimentos { get; set; }
        public int Indefinidos { get; set; }
        public int ComAlta { get; set; }
        public int SemAlta { get; set; }
    }
}
