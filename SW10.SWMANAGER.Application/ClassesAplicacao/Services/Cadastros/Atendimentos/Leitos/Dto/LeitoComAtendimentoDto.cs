using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto
{
    public class LeitoComAtendimentoDto
    {
        public long Id { get; set; }

        public LeitoDto Leito { get; set; }

        public long? AtendimentoLeitoMovId { get; set; }

        public AtendimentoLeitoMovDto AtendimentoLeitoMov { get; set; }

        public long? AtendimentoId { get; set; }

        public AtendimentoDto AtendimentoAtual { get; set; }

    }
}
