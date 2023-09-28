using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    public class IncluirItemsDoKitInputDto : FaturamentoContaKitDto {
        public List<FaturamentoContaItemDto> Items { get; set; }
    }
}