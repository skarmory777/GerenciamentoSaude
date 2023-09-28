using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos
{
    public class FaturamentoIncluirPacoteDto : FaturamentoPacoteDto
    {
        public List<FaturamentoContaItemDto> Items { get; set; }
    }
}
