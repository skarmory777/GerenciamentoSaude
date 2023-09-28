using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    [AutoMapFrom(typeof(LaudoMovimentoItem))]
    public class ExameItemEditDto
    {
        public long? Id { get; set; }
        public long LaudoMovimentoId { get; set; }
        public long FaturamentoItemId { get; set; }
        public virtual FaturamentoItemDto FaturamentoItem { get; set; }
        public long? SolicitacaoExameItemId { get; set; }
        public long? TecnicoId { get; set; }

        public string Parecer { get; set; }
        public long? UsuarioParecerId { get; set; }
        public DateTime? ParecerData { get; set; }
        public string Laudo { get; set; }
        public long? UsuarioLaudoId { get; set; }
        public DateTime? LaudoData { get; set; }
        public string ConcordanciaLaudo { get; set; }
        public string JustificativaConcoLaudo { get; set; }
        public string Revisao { get; set; }
        public long? UsuarioRevisaoId { get; set; }
        public DateTime? RevisaoData { get; set; }
        public string Retificacao { get; set; }
        public long? UsuarioRetificacaoId { get; set; }
        public DateTime? RetificacaoData { get; set; }
        public int Status { get; set; }
    }
}