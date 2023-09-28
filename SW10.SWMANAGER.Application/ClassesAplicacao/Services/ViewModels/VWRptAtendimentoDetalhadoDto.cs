using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public class VWRptAtendimentoDetalhadoDto : EntityDto<long>
    {
        public string CodigoAtendimento { get; set; }
        public string Atendimento { get; set; }
        public long? PacienteId { get; set; }
        public string CodPaciente { get; set; }
        public string Paciente { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string Unidade { get; set; }
        public long? ConvenioId { get; set; }
        public string Convenio { get; set; }
        public long? MedicoId { get; set; }
        public string Medico { get; set; }
        public string Empresa { get; set; }
        public long? EmpresaId { get; set; }
        public string Origem { get; set; }
        public long? EspecialidadeId { get; set; }
        public string Especialidade { get; set; }
        public string Plano { get; set; }
        public string TipoAtendimento { get; set; }
        public string Guia { get; set; }
        public string NumeroGuia { get; set; }
        public DateTime? DataAlta { get; set; }
        public DateTime? DataAltaMedica { get; set; }
        public string Senha { get; set; }
        public DateTime? Nascimento { get; set; }
        public string IdadeAno { get; set; }

        #region Mapeamento
        public static VWRptAtendimentoDetalhadoDto Mapear(VWRptAtendimentoDetalhado input)
        {
            var result = new VWRptAtendimentoDetalhadoDto();
            result.Atendimento = input.Atendimento;
            result.CodigoAtendimento = input.CodigoAtendimento;
            result.CodPaciente = input.CodPaciente;
            result.Convenio = input.Convenio;
            result.DataAlta = input.DataAlta;
            result.DataAltaMedica = input.DataAltaMedica;
            result.DataAtendimento = input.DataAtendimento;
            result.Empresa = input.Empresa;
            result.EspecialidadeId = input.EspecialidadeId;
            result.Especialidade = input.Especialidade;
            result.Guia = input.Guia;
            result.Id = input.Id;
            result.IdadeAno = input.IdadeAno;
            result.Medico = input.Medico;
            result.Nascimento = input.Nascimento;
            result.NumeroGuia = input.NumeroGuia;
            result.Origem = input.Origem;
            result.Paciente = input.Paciente;
            result.Plano = input.Plano;
            result.Senha = input.Senha;
            result.TipoAtendimento = input.TipoAtendimento;
            result.Unidade = input.Unidade;
            result.EmpresaId = input.EmpresaId;
            result.MedicoId = input.MedicoId;
            result.ConvenioId = input.ConvenioId;
            result.PacienteId = input.Pacienteid;
            return result;
        }

        public static VWRptAtendimentoDetalhado Mapear(VWRptAtendimentoDetalhadoDto input)
        {
            var result = new VWRptAtendimentoDetalhado();
            result.Atendimento = input.Atendimento;
            result.CodigoAtendimento = input.CodigoAtendimento;
            result.CodPaciente = input.CodPaciente;
            result.Convenio = input.Convenio;
            result.DataAlta = input.DataAlta;
            result.DataAltaMedica = input.DataAltaMedica;
            result.DataAtendimento = input.DataAtendimento;
            result.Empresa = input.Empresa;
            result.EspecialidadeId = input.EspecialidadeId;
            result.Especialidade = input.Especialidade;
            result.Guia = input.Guia;
            result.Id = input.Id;
            result.IdadeAno = input.IdadeAno;
            result.Medico = input.Medico;
            result.Nascimento = input.Nascimento;
            result.NumeroGuia = input.NumeroGuia;
            result.Origem = input.Origem;
            result.Paciente = input.Paciente;
            result.Plano = input.Plano;
            result.Senha = input.Senha;
            result.TipoAtendimento = input.TipoAtendimento;
            result.Unidade = input.Unidade;
            result.EmpresaId = input.EmpresaId;
            result.MedicoId = input.MedicoId;
            result.ConvenioId = input.ConvenioId;
            result.Pacienteid = input.PacienteId;
            return result;
        }

        public static IEnumerable<VWRptAtendimentoDetalhadoDto> Mapear(List<VWRptAtendimentoDetalhado> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }

        public static IEnumerable<VWRptAtendimentoDetalhado> Mapear(List<VWRptAtendimentoDetalhadoDto> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }

        #endregion
    }
}
