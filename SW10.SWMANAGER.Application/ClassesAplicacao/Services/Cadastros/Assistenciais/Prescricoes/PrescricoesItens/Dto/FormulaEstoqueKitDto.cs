using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto
{
    public class FormulaEstoqueKitDto : CamposPadraoCRUDDto
    {
        public long PrescricaoItemId { get; set; }
        public PrescricaoItemDto PrescricaoItem { get; set; }
        public long EstoqueKitId { get; set; }
        public EstoqueKitDto EstoqueKit { get; set; }

        public static FormulaEstoqueKitDto Mapear(FormulaEstoqueKit formulaEstoqueKit)
        {
            FormulaEstoqueKitDto formulaEstoqueKitDto = new FormulaEstoqueKitDto();

            formulaEstoqueKitDto.Id = formulaEstoqueKit.Id;
            formulaEstoqueKitDto.Codigo = formulaEstoqueKit.Codigo;
            formulaEstoqueKitDto.Descricao = formulaEstoqueKit.Descricao;
            formulaEstoqueKitDto.PrescricaoItemId = formulaEstoqueKit.PrescricaoItemId;
            formulaEstoqueKitDto.EstoqueKitId = formulaEstoqueKit.EstoqueKitId;

            if (formulaEstoqueKit.EstoqueKit != null)
            {
                formulaEstoqueKitDto.EstoqueKit = EstoqueKitDto.Mapear(formulaEstoqueKit.EstoqueKit);
            }

            return formulaEstoqueKitDto;
        }

        public static FormulaEstoqueKit Mapear(FormulaEstoqueKitDto formulaEstoqueKitDto)
        {
            FormulaEstoqueKit formulaEstoqueKit = new FormulaEstoqueKit();

            formulaEstoqueKit.Id = formulaEstoqueKitDto.Id;
            formulaEstoqueKit.Codigo = formulaEstoqueKitDto.Codigo;
            formulaEstoqueKit.Descricao = formulaEstoqueKitDto.Descricao;
            formulaEstoqueKit.PrescricaoItemId = formulaEstoqueKitDto.PrescricaoItemId;
            formulaEstoqueKit.EstoqueKitId = formulaEstoqueKitDto.EstoqueKitId;

            if (formulaEstoqueKit.EstoqueKit != null)
            {
                formulaEstoqueKit.EstoqueKit = EstoqueKitDto.Mapear(formulaEstoqueKitDto.EstoqueKit);
            }

            return formulaEstoqueKit;
        }

    }
}
