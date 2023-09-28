using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class AgendamentoConsultasViewModel
    {
        public string Filtro { get; set; }

        public long MedicoId { get; set; }

        public long MedicoEspecialidadeId { get; set; }

        public long ConvenioId { get; set; }

        public long PacienteId { get; set; }

        //public SelectList Medicos { get; set; }

        public SelectList Especialidades { get; set; }

        //public SelectList Intervalos { get; set; }

        //public SelectList Pacientes { get; set; }

        //public SelectList Convenios { get; set; }

        public bool IsConsulta { get; set; }
        public bool IsCirurgia { get; set; }
        public bool IsExame { get; set; }

        public long? EmpresaId { get; set; }

        public EmpresaDto Empresa { get; set; }
    }
}