using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    [AutoMapFrom(typeof(CriarOuEditarAtendimento))]
    public class AmbulatorioEmergenciasModalViewModel : CriarOuEditarAtendimento
    {
        public UserEditDto UpdateUser { get; set; }

        public Empresa UserEmpresa { get; set; }

        public SelectList Pacientes { get; set; }

        public SelectList Medicos { get; set; }

        public SelectList Empresas { get; set; }

        public SelectList Origens { get; set; }

        public SelectList Convenios { get; set; }

        public SelectList UnidadesOrganizacionais { get; set; }

        public List<CriarOuEditarAtendimento> Atendimentos { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public AmbulatorioEmergenciasModalViewModel(CriarOuEditarAtendimento output)
        {
            this.GuiaNumero = output.GuiaNumero;
            this.Matricula = output.Matricula;
            this.Responsavel = output.Responsavel;
            this.RgResponsavel = output.RgResponsavel;
            this.CpfResponsavel = output.CpfResponsavel;
            this.NumeroGuia = output.NumeroGuia;
            this.QtdSessoes = output.QtdSessoes;
            this.DataRetorno = output.DataRetorno;
            this.DataRevisao = output.DataRevisao;
            this.NacionalidadeResponsavelId = output.NacionalidadeResponsavelId;
            this.Nacionalidade = output.Nacionalidade;
            this.Observacao = output.Observacao;
            this.DataPreatendimento = output.DataPreatendimento;
            this.DataPrevistaAtendimento = output.DataPrevistaAtendimento;
            this.DataRegistro = output.DataRegistro;
            this.DataAlta = output.DataAlta;
            this.PacienteId = output.PacienteId;
            this.OrigemId = output.OrigemId;
            this.MedicoId = output.MedicoId;
            this.EspecialidadeId = output.EspecialidadeId;
            this.EmpresaId = output.EmpresaId;
            this.ConvenioId = output.ConvenioId;
            this.PlanoId = output.PlanoId;
            this.AtendimentoStatusId = output.AtendimentoStatusId;
            this.UnidadeOrganizacionalId = output.UnidadeOrganizacionalId;
            this.AtendimentoTipoId = output.AtendimentoTipoId;
            this.GuiaId = output.GuiaId;
            this.LeitoId = output.LeitoId;
            this.MotivoAltaId = output.MotivoAltaId;
            this.IsAmbulatorioEmergencia = output.IsAmbulatorioEmergencia;
            this.IsInternacao = output.IsInternacao;
            this.IsHomeCare = output.IsHomeCare;
            this.IsPreatendimento = output.IsPreatendimento;
            this.ServicoMedicoPrestadoId = output.ServicoMedicoPrestadoId;
            this.Paciente = output.Paciente;
            this.Origem = output.Origem;
            this.Medico = output.Medico;
            this.Especialidade = output.Especialidade;
            this.Empresa = output.Empresa;
            this.Convenio = output.Convenio;
            this.Plano = output.Plano;
            this.AtendimentoTipo = output.AtendimentoTipo;
            this.Guia = output.Guia;
            this.ServicoMedicoPrestado = output.ServicoMedicoPrestado;
            this.UnidadeOrganizacional = output.UnidadeOrganizacional;
            this.Leito = output.Leito;
            this.MotivoAlta = output.MotivoAlta;
        }

        public string Filtro { get; set; }
    }
}