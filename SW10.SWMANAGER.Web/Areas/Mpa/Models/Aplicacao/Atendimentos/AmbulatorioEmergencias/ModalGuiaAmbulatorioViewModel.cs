using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    public class ModalGuiaAmbulatorioViewModel : CriarOuEditarAtendimento
    {
        public string AtendimentoId { get; set; }
        public string ContaId { get; set; }
        public string TipoGuia { get; set; }
    }
}