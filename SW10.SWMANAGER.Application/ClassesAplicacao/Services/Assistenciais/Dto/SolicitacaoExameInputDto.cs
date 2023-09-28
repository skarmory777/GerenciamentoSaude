using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class SolicitacaoExameInputDto
    {
        public SolicitacaoExameDto Solicitacao { get; set; }
        public virtual ICollection<SolicitacaoExameItemDto> SolicitacaoExameItens { get; set; }
    }
}
