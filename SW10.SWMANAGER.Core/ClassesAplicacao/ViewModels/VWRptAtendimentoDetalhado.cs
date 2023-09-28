using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("VWRptAteDetalhado")]
    public class VWRptAtendimentoDetalhado : Entity<long>
    {
        public string CodigoAtendimento { get; set; }
        public string Atendimento { get; set; }
        public long? AtendimentoId { get; set; }
        public string CodPaciente { get; set; }
        public string Paciente { get; set; }
        public long? Pacienteid { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string Unidade { get; set; }
        public string Convenio { get; set; }
        public long? ConvenioId { get; set; }
        public string Medico { get; set; }
        public long? MedicoId { get; set; }
        public string Empresa { get; set; }
        public long? EmpresaId { get; set; }
        public string Origem { get; set; }
        public long? EspecialidadeId { get; set; }
        public string Especialidade { get; set; }
        public string Plano { get; set; }
        public string TipoAtendimento { get; set; }
        public string Guia { get; set; }
        public string NumeroGuia { get; set; }
        public DateTime? DataAlta { get; set; }
        public DateTime? DataAltaMedica { get; set; }
        public string Senha { get; set; }
        public DateTime? Nascimento { get; set; }
        public string IdadeAno { get; set; }
    }
}
