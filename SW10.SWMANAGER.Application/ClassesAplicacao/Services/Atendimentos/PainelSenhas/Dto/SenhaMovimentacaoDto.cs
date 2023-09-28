using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    [AutoMap(typeof(SenhaMovimentacao))]
    public class SenhaMovimentacaoDto : CamposPadraoCRUDDto
    {
        public long SenhaId { get; set; }
        public long? LocalChamadaId { get; set; }
        public long? TipoLocalChamadaId { get; set; }
        public DateTime DataHora { get; set; }
        public DateTime? DataHoraInicial { get; set; }
        public DateTime? DataHoraFinal { get; set; }

        public SenhaDto Senha { get; set; }
        public LocalChamadaDto LocalChamada { get; set; }
        public TipoLocalChamadaDto TipoLocalChamada { get; set; }

        public static SenhaMovimentacaoDto Mapear(SenhaMovimentacao senhaMovimentacao)
        {
            SenhaMovimentacaoDto senhaMovimentacaoDto = new SenhaMovimentacaoDto();

            senhaMovimentacaoDto.Id = senhaMovimentacao.Id;
            senhaMovimentacaoDto.Codigo = senhaMovimentacao.Codigo;
            senhaMovimentacaoDto.Descricao = senhaMovimentacao.Descricao;
            senhaMovimentacaoDto.SenhaId = senhaMovimentacao.SenhaId;
            senhaMovimentacaoDto.LocalChamadaId = senhaMovimentacao.LocalChamadaId;
            senhaMovimentacaoDto.TipoLocalChamadaId = senhaMovimentacao.TipoLocalChamadaId;
            senhaMovimentacaoDto.DataHora = senhaMovimentacao.DataHora;
            senhaMovimentacaoDto.DataHoraInicial = senhaMovimentacao.DataHoraInicial;
            senhaMovimentacaoDto.DataHoraFinal = senhaMovimentacao.DataHoraFinal;

            if (senhaMovimentacao.Senha != null)
            {
                senhaMovimentacaoDto.Senha = SenhaDto.Mapear(senhaMovimentacao.Senha);
            }

            if (senhaMovimentacao.LocalChamada != null)
            {
                senhaMovimentacaoDto.LocalChamada = LocalChamadaDto.Mapear(senhaMovimentacao.LocalChamada);
            }

            if (senhaMovimentacao.TipoLocalChamada != null)
            {
                senhaMovimentacaoDto.TipoLocalChamada = TipoLocalChamadaDto.Mapear(senhaMovimentacao.TipoLocalChamada);
            }

            return senhaMovimentacaoDto;
        }
    }
}
