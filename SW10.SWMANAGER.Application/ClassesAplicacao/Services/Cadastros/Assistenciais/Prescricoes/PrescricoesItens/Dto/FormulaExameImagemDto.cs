using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto
{
    [AutoMap(typeof(FormulaFaturamento))]
    public class FormulaExameImagemDto : CamposPadraoCRUDDto, IDescricao
    {
        public long? FaturamentoItemId { get; set; }

        public bool IsFatura { get; set; }

        public FaturamentoItemDto FaturamentoItem { get; set; }

        public long PrescricaoItemId { get; set; }

        public PrescricaoItemDto PrescricaoItem { get; set; }

        public long? MaterialId { get; set; }

        public Material Material { get; set; }

        public long? IdGridFormulasFaturamento { get; set; }

        public long? IdGridFormulasExameLaboratorial { get; set; }

        public long? IdGridFormulasExameImagem { get; set; }
    }
}
