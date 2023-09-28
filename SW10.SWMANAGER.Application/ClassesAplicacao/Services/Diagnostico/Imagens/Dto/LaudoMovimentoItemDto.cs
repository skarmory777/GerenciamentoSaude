using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    [AutoMap(typeof(LaudoMovimentoItem))]
    public class LaudoMovimentoItemDto : CamposPadraoCRUDDto
    {
        public long LaudoMovimentoId { get; set; }
        public long FaturamentoItemId { get; set; }
        public long? SolicitacaoExameItemId { get; set; }

        public LaudoMovimentoDto LaudoMovimento { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
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

        public long? FaturamentocontaItemId { get; set; }
        public FaturamentoContaItemDto FaturamentoContaItemDto { get; set; }

        public string UsuarioParecer { get; set; }
        public string UsuarioLaudo { get; set; }
        public string UsuarioRevisao { get; set; }
        public bool IsEditarLaudo { get; set; }
        public bool IsIndicativo { get; set; }
        public bool IsSolicitacaoRevisao { get; set; }
        public string ComentarioLaudo { get; set; }
        public string JustificativaContraste { get; set; }
        public string MotivoDiscordancia { get; set; }

        public string AccessNumber { get; set; }


        public static LaudoMovimentoItemDto Mapear(LaudoMovimentoItem laudoMovimentoItem)
        {
            LaudoMovimentoItemDto laudoMovimentoItemDto = new LaudoMovimentoItemDto();

            laudoMovimentoItemDto.Id = laudoMovimentoItem.Id;
            laudoMovimentoItemDto.Codigo = laudoMovimentoItem.Codigo;
            laudoMovimentoItemDto.Descricao = laudoMovimentoItem.Descricao;



            laudoMovimentoItemDto.LaudoMovimentoId = laudoMovimentoItem.LaudoMovimentoId ?? 0;
            laudoMovimentoItemDto.FaturamentoItemId = laudoMovimentoItem.FaturamentoItemId;
            laudoMovimentoItemDto.SolicitacaoExameItemId = laudoMovimentoItem.SolicitacaoExameItemId;

            if (laudoMovimentoItem.LaudoMovimento != null)
            {
                laudoMovimentoItemDto.LaudoMovimento = LaudoMovimentoDto.Mapear(laudoMovimentoItem.LaudoMovimento);
            }

            if (laudoMovimentoItem.FaturamentoItem != null)
            {
                laudoMovimentoItemDto.FaturamentoItem = FaturamentoItemDto.Mapear(laudoMovimentoItem.FaturamentoItem);
            }
            laudoMovimentoItemDto.TecnicoId = laudoMovimentoItem.TecnicoId;
            laudoMovimentoItemDto.Parecer = laudoMovimentoItem.Parecer;
            laudoMovimentoItemDto.UsuarioParecerId = laudoMovimentoItem.UsuarioParecerId;
            laudoMovimentoItemDto.ParecerData = laudoMovimentoItem.ParecerData;
            laudoMovimentoItemDto.Laudo = laudoMovimentoItem.Laudo;
            laudoMovimentoItemDto.UsuarioLaudoId = laudoMovimentoItem.UsuarioLaudoId;
            laudoMovimentoItemDto.LaudoData = laudoMovimentoItem.LaudoData;
            laudoMovimentoItemDto.ConcordanciaLaudo = laudoMovimentoItem.ConcordanciaLaudo;
            laudoMovimentoItemDto.JustificativaConcoLaudo = laudoMovimentoItem.JustificativaConcoLaudo;
            laudoMovimentoItemDto.Revisao = laudoMovimentoItem.Revisao;
            laudoMovimentoItemDto.UsuarioRevisaoId = laudoMovimentoItem.UsuarioRevisaoId;
            laudoMovimentoItemDto.RevisaoData = laudoMovimentoItem.RevisaoData;
            laudoMovimentoItemDto.Retificacao = laudoMovimentoItem.Retificacao;
            laudoMovimentoItemDto.UsuarioRetificacaoId = laudoMovimentoItem.UsuarioRetificacaoId;
            laudoMovimentoItemDto.RetificacaoData = laudoMovimentoItem.RetificacaoData;
            laudoMovimentoItemDto.Status = laudoMovimentoItem.Status;
            laudoMovimentoItemDto.FaturamentocontaItemId = laudoMovimentoItem.FaturamentocontaItemId;
            //  laudoMovimentoItemDto.FaturamentoContaItemDto = laudoMovimentoItem.FaturamentoContaItemDto;

            laudoMovimentoItemDto.IsIndicativo = laudoMovimentoItem.IsIndicativo;
            laudoMovimentoItemDto.IsSolicitacaoRevisao = laudoMovimentoItem.IsSolicitacaoRevisao;
            laudoMovimentoItemDto.ComentarioLaudo = laudoMovimentoItem.ComentarioLaudo;
            laudoMovimentoItemDto.JustificativaContraste = laudoMovimentoItem.JustificativaContraste;
            laudoMovimentoItemDto.MotivoDiscordancia = laudoMovimentoItem.MotivoDiscordancia;

            laudoMovimentoItemDto.AccessNumber = laudoMovimentoItem.AccessNumber;

            return laudoMovimentoItemDto;
        }
    }
}
