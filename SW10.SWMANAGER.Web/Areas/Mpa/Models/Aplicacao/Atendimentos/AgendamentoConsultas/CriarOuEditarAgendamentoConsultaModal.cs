using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    [AutoMapFrom(typeof(CriarOuEditarAgendamentoConsulta))]
    public class CriarOuEditarAgendamentoConsultaModal : CriarOuEditarAgendamentoConsulta
    {
        public bool IsEditMode { get { return Id > 0; } }

        public string PacienteSearch { get; set; }

        public string PlanoSearch { get; set; }

        public string ConvenioSearch { get; set; }

        public SelectList Pacientes { get; set; }

        public SelectList Convenios { get; set; }

        public SelectList Planos { get; set; }

        public CriarOuEditarAgendamentoConsultaModal(CriarOuEditarAgendamentoConsulta output)
        {
            output.MapTo(this);
        }
    }
}