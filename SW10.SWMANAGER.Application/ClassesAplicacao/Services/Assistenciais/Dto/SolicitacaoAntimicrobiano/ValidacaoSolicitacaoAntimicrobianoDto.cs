using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class ValidacaoSolicitacaoDto
    {
        public bool NecessitaSolicitacao { get; set; }

        public List<long> PrescricaoItemIds { get; set; }
    }
}
