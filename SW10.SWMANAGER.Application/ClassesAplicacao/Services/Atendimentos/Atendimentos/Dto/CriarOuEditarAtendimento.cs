using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    [AutoMap(typeof(Atendimento))]
    public class CriarOuEditarAtendimento : CamposPadraoCRUDDto
    {
        public string GuiaNumero { get; set; }

        public string Matricula { get; set; }

        public string Responsavel { get; set; }

        public string RgResponsavel { get; set; }

        public string CpfResponsavel { get; set; }

        public string NumeroGuia { get; set; }

        public int? QtdSessoes { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? DataRetorno { get; set; }

        //aqui
        [DataType(DataType.DateTime)]
        public DateTime? DataRevisao { get; set; }


        public long? NacionalidadeResponsavelId { get; set; }
        [ForeignKey("NacionalidadeResponsavelId")]
        public virtual NacionalidadeDto Nacionalidade { get; set; }

        public string Observacao { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataPreatendimento { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataPrevistaAtendimento { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataRegistro { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataAlta { get; set; }

        public long? PacienteId { get; set; }

        public long? OrigemId { get; set; }

        public long? MedicoId { get; set; }

        public long? EspecialidadeId { get; set; }

        public long? EmpresaId { get; set; }

        public long? ConvenioId { get; set; }

        public long? PlanoId { get; set; }

        public long? AtendimentoStatusId { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public long? AtendimentoTipoId { get; set; }

        public long? GuiaId { get; set; }

        public long? LeitoId { get; set; }

        public long? MotivoAltaId { get; set; }

        public bool IsAmbulatorioEmergencia { get; set; }

        public bool IsInternacao { get; set; }

        public bool IsHomeCare { get; set; }

        public bool IsPreatendimento { get; set; }

        public long? ServicoMedicoPrestadoId { get; set; }

        [ForeignKey("PacienteId")]
        public virtual PacienteDto Paciente { get; set; }

        [ForeignKey("OrigemId")]
        public virtual OrigemDto Origem { get; set; }

        [ForeignKey("MedicoId")]
        public virtual MedicoDto Medico { get; set; }

        [ForeignKey("EspecialidadeId")]
        public virtual EspecialidadeDto Especialidade { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual EmpresaDto Empresa { get; set; }

        [ForeignKey("ConvenioId")]
        public virtual ConvenioDto Convenio { get; set; }

        [ForeignKey("PlanoId")]
        public virtual PlanoDto Plano { get; set; }

        [ForeignKey("AtendimentoTipoId")]
        public virtual TipoAtendimentoDto AtendimentoTipo { get; set; }

        [ForeignKey("GuiaId")]
        public virtual GuiaDto Guia { get; set; }

        [ForeignKey("ServicoMedicoPrestadoId")]
        public virtual ServicoMedicoPrestadoDto ServicoMedicoPrestado { get; set; }

        //[ForeignKey ("UnidadeOrganizacionalId")]
        //public virtual OrganizationUnitDto UnidadeOrganizacional { get; set; }

        [ForeignKey("UnidadeOrganizacionalId")]
        public virtual UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        [ForeignKey("LeitoId")]
        public virtual LeitoDto Leito { get; set; }

        [ForeignKey("MotivoAltaId")]
        public virtual MotivoAltaDto MotivoAlta { get; set; }
    }
}
