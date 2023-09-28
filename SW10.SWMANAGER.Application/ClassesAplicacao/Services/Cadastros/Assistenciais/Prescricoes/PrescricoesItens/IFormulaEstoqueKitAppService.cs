using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    public interface IFormulaEstoqueKitAppService : IApplicationService
    {
        List<FormulaEstoqueKitIndex> ListarPorPrescricaoItem(long prescricaoItemId);
        List<EstoqueKitItemDto> ListarItensKitPorPrescricaoItem(long prescricaoItemId);
    }
}
