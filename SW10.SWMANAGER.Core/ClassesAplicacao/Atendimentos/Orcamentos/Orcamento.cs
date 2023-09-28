using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.PreAtendimentos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Orcamentos
{
    [Table("Orcamento")]
    public class Orcamento : CamposPadraoCRUD
    {
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        public long? ConvenioId { get; set; }

        public long PlanoId { get; set; }

        public long PrestadorId { get; set; }

        public long EmpresaId { get; set; }

        public long CentroCustoId { get; set; }

        public long UnidadeOrganizacionalId { get; set; }

        public long PreAtendimentoId { get; set; }

        public long PacienteId { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        [ForeignKey("PlanoId")]
        public Plano Plano { get; set; }

        //[ForeignKey("PrestadorId")]
        //public virtual Prestador Prestador { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        //[ForeignKey("CentroCustoId")]
        //public virtual CentroCusto CentroCusto { get; set; }

        //[ForeignKey("UnidadeOrganizacionalId")]
        //public virtual UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("PreAtendimentoId")]
        public PreAtendimento PreAtendimento { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }
    }
}
