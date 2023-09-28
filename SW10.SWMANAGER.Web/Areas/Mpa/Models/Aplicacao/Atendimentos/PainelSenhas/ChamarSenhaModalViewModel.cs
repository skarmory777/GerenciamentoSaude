using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PainelSenhas
{
    public class ChamarSenhaModalViewModel
    {
        public long? TipoLocalChamadaId { get; set; }
        public long? LocalChamadaId { get; set; }
        public long? SenhaId { get; set; }

        public TipoLocalChamadaDto TipoLocalChamada { get; set; }
        public LocalChamadaDto LocalChamada { get; set; }
        public SenhaDto Senha { get; set; }
    }
}