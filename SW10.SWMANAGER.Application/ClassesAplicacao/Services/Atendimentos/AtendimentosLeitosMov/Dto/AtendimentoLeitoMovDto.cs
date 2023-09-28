using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.AtendimentosLeitosMov;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto
{
    [AutoMap(typeof(AtendimentoLeitoMov))]
    public class AtendimentoLeitoMovDto : CamposPadraoCRUDDto
    {
        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public DateTime? DataInclusao { get; set; }

        public long? UserId { get; set; }
        public User User { get; set; }

        public long? AtendimentoLeitoMovId { get; set; }
        public AtendimentoLeitoMovDto AtendimentoLeitoMov { get; set; }

        public long? AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }

        public long? LeitoId { get; set; }
        public LeitoDto Leito { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }


        public static AtendimentoLeitoMovDto Mapear(AtendimentoLeitoMov atendimentoLeitoMov)
        {
            if (atendimentoLeitoMov == null)
            {
                return null;
            }

            var atendimentoLeitoMovDto = MapearBase<AtendimentoLeitoMovDto>(atendimentoLeitoMov);

            atendimentoLeitoMovDto.DataInicial = atendimentoLeitoMov.DataInicial;
            atendimentoLeitoMovDto.DataFinal = atendimentoLeitoMov.DataFinal;
            atendimentoLeitoMovDto.DataInclusao = atendimentoLeitoMov.DataInclusao;
            atendimentoLeitoMovDto.UserId = atendimentoLeitoMov.UserId;
            atendimentoLeitoMovDto.AtendimentoId = atendimentoLeitoMov.AtendimentoId;
            atendimentoLeitoMovDto.LeitoId = atendimentoLeitoMov.LeitoId;

            if (atendimentoLeitoMov.Atendimento != null)
            {
                atendimentoLeitoMovDto.Atendimento = AtendimentoDto.MapearFromCore(atendimentoLeitoMov.Atendimento);
            }

            if (atendimentoLeitoMov.Leito != null)
            {
                atendimentoLeitoMovDto.Leito = LeitoDto.MapearFromCore(atendimentoLeitoMov.Leito);
            }


            return atendimentoLeitoMovDto;
        }


        public static AtendimentoLeitoMov Mapear(AtendimentoLeitoMovDto atendimentoLeitoMovDto)
        {
            if (atendimentoLeitoMovDto == null)
            {
                return null;
            }

            var atendimentoLeitoMov = MapearBase<AtendimentoLeitoMov>(atendimentoLeitoMovDto);

            atendimentoLeitoMov.DataInicial = atendimentoLeitoMovDto.DataInicial;
            atendimentoLeitoMov.DataFinal = atendimentoLeitoMovDto.DataFinal;
            atendimentoLeitoMov.DataInclusao = atendimentoLeitoMovDto.DataInclusao;
            atendimentoLeitoMov.UserId = atendimentoLeitoMovDto.UserId;
            atendimentoLeitoMov.AtendimentoId = atendimentoLeitoMovDto.AtendimentoId;
            atendimentoLeitoMov.LeitoId = atendimentoLeitoMovDto.LeitoId;


            if (atendimentoLeitoMovDto.Atendimento != null)
            {
                atendimentoLeitoMov.Atendimento = AtendimentoDto.Mapear(atendimentoLeitoMovDto.Atendimento);
            }

            if (atendimentoLeitoMovDto.Leito != null)
            {
                atendimentoLeitoMov.Leito = LeitoDto.Mapear(atendimentoLeitoMovDto.Leito);
            }


            return atendimentoLeitoMov;
        }

    }
}


