using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public class VWRptAtendimentoResumidoDto : EntityDto<long>
    {
        public string Empresa { get; set; }
        public long? EmpresaId { get; set; }
        public string Convenio { get; set; }
        public long? ConvenioId { get; set; }
        public string Plano { get; set; }
        public string Medico { get; set; }
        public long? MedicoId { get; set; }
        public string Especialidade { get; set; }
        public int Atendimentos { get; set; }
        public int Internacoes { get; set; }
        public int InternacoesAtivas { get; set; }
        public int HomeCare { get; set; }
        public int AmbulatorioEmergencia { get; set; }
        public int PreAtendimentos { get; set; }
        public int Indefinidos { get; set; }
        public int ComAlta { get; set; }
        public int SemAlta { get; set; }

        #region Mapeamento
        public static VWRptAtendimentoResumidoDto Mapear(VWRptAtendimentoResumido input)
        {
            var result = new VWRptAtendimentoResumidoDto
            {
                AmbulatorioEmergencia = input.AmbulatorioEmergencia,
                Atendimentos = input.Atendimentos,
                ComAlta = input.ComAlta,
                Convenio = input.Convenio,
                ConvenioId = input.ConvenioId,
                Empresa = input.Empresa,
                EmpresaId = input.EmpresaId,
                Especialidade = input.Especialidade,
                HomeCare = input.HomeCare,
                Id = input.Id,
                Indefinidos = input.Indefinidos,
                Internacoes = input.Internacoes,
                InternacoesAtivas = input.InternacoesAtivas,
                Medico = input.Medico,
                MedicoId = input.MedicoId,
                Plano = input.Plano,
                PreAtendimentos = input.PreAtendimentos,
                SemAlta = input.SemAlta
            };

            return result;
        }

        public static VWRptAtendimentoResumido Mapear(VWRptAtendimentoResumidoDto input)
        {
            var result = new VWRptAtendimentoResumido
            {
                AmbulatorioEmergencia = input.AmbulatorioEmergencia,
                Atendimentos = input.Atendimentos,
                ComAlta = input.ComAlta,
                Convenio = input.Convenio,
                ConvenioId = input.ConvenioId,
                Empresa = input.Empresa,
                EmpresaId = input.EmpresaId,
                Especialidade = input.Especialidade,
                HomeCare = input.HomeCare,
                Id = input.Id,
                Indefinidos = input.Indefinidos,
                Internacoes = input.Internacoes,
                InternacoesAtivas = input.InternacoesAtivas,
                Medico = input.Medico,
                MedicoId = input.MedicoId,
                Plano = input.Plano,
                PreAtendimentos = input.PreAtendimentos,
                SemAlta = input.SemAlta
            };

            return result;
        }

        public static IEnumerable<VWRptAtendimentoResumidoDto> Mapear(List<VWRptAtendimentoResumido> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }

        public static IEnumerable<VWRptAtendimentoResumido> Mapear(List<VWRptAtendimentoResumidoDto> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }
        #endregion
    }
}
