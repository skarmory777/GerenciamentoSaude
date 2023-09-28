using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    [AutoMap(typeof(Senha))]
    public class SenhaDto : CamposPadraoCRUDDto
    {
        public DateTime DataHora { get; set; }
        public int Numero { get; set; }
        public long FilaId { get; set; }
        public long? AtendimentoId { get; set; }

        public FilaDto Fila { get; set; }

        public AtendimentoDto Atendimento { get; set; }

        public static SenhaDto Mapear(Senha senha)
        {
            if (senha != null)
            {
                SenhaDto senhaDto = new SenhaDto();

                senhaDto.Id = senha.Id;
                senhaDto.Codigo = senha.Codigo;
                senhaDto.Descricao = senha.Descricao;

                senhaDto.DataHora = senha.DataHora;
                senhaDto.Numero = senha.Numero;
                senhaDto.FilaId = senha.FilaId;
                senhaDto.AtendimentoId = senha.AtendimentoId;

                if (senha.Fila != null)
                {
                    senhaDto.Fila = FilaDto.Mapear(senha.Fila);
                }

                if (senha.Atendimento != null)
                {
                    senhaDto.Atendimento = AtendimentoDto.Mapear(senha.Atendimento);
                }

                return senhaDto;
            }
            return null;
        }
    }
}
