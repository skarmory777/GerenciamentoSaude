using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    [AutoMapFrom(typeof(LaudoMovimentoItem))]
    public class ExameItemListDto : EntityDto<long>
    {
        public long? TecnicoId { get; set; }

        public virtual ExameItemFaturamentoItemDto FaturamentoItem { get; set; }
        public virtual ExameItemSolicitacaoExameItemDto SolicitacaoExameItem { get; set; }

        #region FaturamentoItemDto
        [AutoMapFrom(typeof(FaturamentoItem))]
        public class ExameItemFaturamentoItemDto
        {
            public string Codigo { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #region FaturamentoItemDto
        [AutoMapFrom(typeof(SolicitacaoExameItem))]
        public class ExameItemSolicitacaoExameItemDto
        {
            public string Codigo { get; set; }
        }
        #endregion
    }
}
