using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios
{
    [Table("SisIntervaloGuiasConvenio")]
    public class IntervaloGuiasConvenio : CamposPadraoCRUD
    {
        public long ConvenioId { get; set; }
        public long EmpresaId { get; set; }
        public long FaturamentoGuiaId { get; set; }

        public string Inicio { get; set; }
        public string Final { get; set; }
        public int? NumeroGuiasParaAviso { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        [ForeignKey("FaturamentoGuiaId")]
        public FaturamentoGuia FaturamentoGuia { get; set; }


    }
}
