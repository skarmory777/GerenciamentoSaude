using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    public class ModalPulseiraViewModel : CriarOuEditarAtendimento
    {
        public string AtendimentoId { get; set; }

        public long NumOfCopies { get; set; }
    }
}