using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.AgendamentoConsultaMedicoDisponibilidades
{
    [AutoMapFrom(typeof(CriarOuEditarAgendamentoConsultaMedicoDisponibilidade))]
    public class CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModalViewModel : CriarOuEditarAgendamentoConsultaMedicoDisponibilidade
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Medicos { get; set; }

        public SelectList Intervalos { get; set; }

        public SelectList HorariosInicio { get; set; }

        public SelectList HorariosFim { get; set; }

        public SelectList MedicoEspecialidades { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModalViewModel(CriarOuEditarAgendamentoConsultaMedicoDisponibilidade output)
        {
            output.MapTo(this);
        }
    }
}