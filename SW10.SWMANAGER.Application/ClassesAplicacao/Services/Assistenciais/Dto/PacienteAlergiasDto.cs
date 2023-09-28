using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(PacienteAlergias))]
    public class PacienteAlergiasDto : CamposPadraoCRUDDto
    {
        public DateTime DataCadastro { get; set; }

        public string Alergia { get; set; }

        /*
         * //TODO: Fazer depois poder pegar de uma lista de alergias.
         * Caso seja Principio ativo deverá buscar na lista dos principios dos medicamentos
         * Caso seja Outros ou alimento o usuário poderá cadastrar uma alergia caso não tenha na lista.
         */

        //public long? PrincipioAtivo { get; set; }
        // public PrincipoAtivo {get;set;}

        //public long? AlergiaId {get;set;
        //public Alergias Alergia { get; set; }

        public long PacienteId { get; set; }

        public long? AtendimentoId { get; set; }

        public static PacienteAlergiasDto Mapear(PacienteAlergias pacienteAlergias)
        {
            if (pacienteAlergias == null)
            {
                return null;
            }

            var pacienteAlergiasDto = MapearBase<PacienteAlergiasDto>(pacienteAlergias);

            pacienteAlergiasDto.DataCadastro = pacienteAlergias.DataCadastro;
            pacienteAlergiasDto.Alergia = pacienteAlergias.Alergia;
            pacienteAlergiasDto.PacienteId = pacienteAlergias.PacienteId;
            pacienteAlergiasDto.AtendimentoId = pacienteAlergias.AtendimentoId;

            return pacienteAlergiasDto;
        }

        public static PacienteAlergias Mapear(PacienteAlergiasDto pacienteAlergiasDto)
        {
            if (pacienteAlergiasDto == null)
            {
                return null;
            }

            var pacienteAlergias = MapearBase<PacienteAlergias>(pacienteAlergiasDto);

            pacienteAlergias.DataCadastro = pacienteAlergiasDto.DataCadastro;
            pacienteAlergias.Alergia = pacienteAlergiasDto.Alergia;
            pacienteAlergias.PacienteId = pacienteAlergiasDto.PacienteId;
            pacienteAlergias.AtendimentoId = pacienteAlergiasDto.AtendimentoId;

            return pacienteAlergias;
        }

    }
}
