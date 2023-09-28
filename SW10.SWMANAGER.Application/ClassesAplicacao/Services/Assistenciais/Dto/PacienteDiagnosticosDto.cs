using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Diagnosticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(PacienteDiagnosticos))]
    public class PacienteDiagnosticosDto : CamposPadraoCRUDDto
    {
        public DateTime DataDiagnostico { get; set; }

        public long GrupoCIDId { get; set; }

        public GrupoCIDDto GrupoCID { get; set; }

        public long PacienteId { get; set; }

        public long AtendimentoId { get; set; }

        public static PacienteDiagnosticosDto Mapear(PacienteDiagnosticos pacienteDiagnosticos)
        {
            if (pacienteDiagnosticos == null)
            {
                return null;
            }

            var pacienteDiagnosticosDto = MapearBase<PacienteDiagnosticosDto>(pacienteDiagnosticos);

            pacienteDiagnosticosDto.DataDiagnostico = pacienteDiagnosticos.DataDiagnostico;
            pacienteDiagnosticosDto.GrupoCIDId = pacienteDiagnosticos.GrupoCIDId;
            pacienteDiagnosticosDto.PacienteId = pacienteDiagnosticos.PacienteId;
            pacienteDiagnosticosDto.AtendimentoId = pacienteDiagnosticos.AtendimentoId;
            pacienteDiagnosticosDto.GrupoCID = GrupoCIDDto.Mapear(pacienteDiagnosticos.GrupoCID);


            return pacienteDiagnosticosDto;
        }
    }
}
