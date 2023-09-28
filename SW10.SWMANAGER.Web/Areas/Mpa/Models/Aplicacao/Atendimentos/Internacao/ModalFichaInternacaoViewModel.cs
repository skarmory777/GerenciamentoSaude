using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    public class ModalFichaInternacaoViewModel : CriarOuEditarAtendimento
    {
        public string AtendimentoId { get; set; }

        public FileContentResult FichaPdf { get; set; }
    }
}