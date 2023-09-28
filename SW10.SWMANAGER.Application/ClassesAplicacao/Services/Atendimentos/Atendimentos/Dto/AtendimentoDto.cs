using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ServicosMedicosPrestados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAcompanhantes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;
using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    [AutoMap(typeof(Atendimento))]
    public class AtendimentoDto : CamposPadraoCRUDDto
    {
        #region campos
        public string Titular { get; set; }
        public string GuiaNumero { get; set; }
        public string Matricula { get; set; }
        public string Responsavel { get; set; }
        public string RgResponsavel { get; set; }
        public string CpfResponsavel { get; set; }
        public string NumeroGuia { get; set; }
        public int? QtdSessoes { get; set; }
        public string Senha { get; set; }
        public string Parentesco { get; set; }
        public string Observacao { get; set; }
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
        public long? FatGuiaId { get; set; }
        public long? TipoAcompanhanteId { get; set; }
        public long? LeitoId { get; set; }
        public long? MotivoAltaId { get; set; }
        public long? NacionalidadeResponsavelId { get; set; }
        public long? TipoAcomodacaoId { get; set; }
        public bool IsAmbulatorioEmergencia { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsHomeCare { get; set; }
        public bool IsPreatendimento { get; set; }
        public bool IsControlaTev { get; set; }
        public long? ServicoMedicoPrestadoId { get; set; }
        public string TelefonePreAtendimento { get; set; }
        public string IdentPreAtendimento { get; set; }
        public string CpfPreAtendimento { get; set; }
        public string NomePreAtendimento { get; set; }
        public long? AltaGrupoCIDId { get; set; }
        public string CodDependente { get; set; }

        public DateTime? DataRetorno { get; set; }
        public DateTime? DataRevisao { get; set; }
        public DateTime? DataPreatendimento { get; set; }
        public DateTime? DataPrevistaAtendimento { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataAlta { get; set; }
        public DateTime? DataAltaMedica { get; set; }
        public DateTime? ValidadeCarteira { get; set; }
        public DateTime? ValidadeSenha { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public DateTime? DataNascPreAtendimento { get; set; }
        public DateTime? DataPrevistaAlta { get; set; }
        public long? DiasAutorizacao { get; set; }
        public DateTime? DataUltimoPagamento { get; set; }

        public NacionalidadeDto Nacionalidade { get; set; }
        public PacienteDto Paciente { get; set; }
        public OrigemDto Origem { get; set; }
        public MedicoDto Medico { get; set; }
        public EspecialidadeDto Especialidade { get; set; }
        public EmpresaDto Empresa { get; set; }
        public ConvenioDto Convenio { get; set; }
        public PlanoDto Plano { get; set; }
        public TipoAtendimentoDto AtendimentoTipo { get; set; }
        public GuiaDto Guia { get; set; }
        public FaturamentoGuiaDto FatGuia { get; set; }
        public TipoAcompanhanteDto TipoAcompanhante { get; set; }
        public ServicoMedicoPrestadoDto ServicoMedicoPrestado { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public LeitoDto Leito { get; set; }
        public TipoAcomodacaoDto TipoAcomodacao { get; set; }
        public MotivoAltaDto MotivoAlta { get; set; }
        public GrupoCIDDto AltaGrupoCID { get; set; }
        public AtendimentoStatusDto AtendimentoStatus { get; set; }

        public string NumeroObito { get; set; }

        public SenhaDto SenhaAtendimento { get; set; }
        public long? MovimentacaoSenhalId { get; set; }
        public long? ProximoTipoLocalChamadaId { get; set; }
        public long? LocalChamadaId { get; set; }
        public long? TipoLocalChamadaId { get; set; }
        public LocalChamadaDto LocalChamada { get; set; }
        // public TipoLocalChamadaDto TipoLocalChamada { get; set; }

        public List<PrescricaoStatusDto> ListaStatus { get; set; }

        public long? AtendimentoMotivoCancelamentoId { get; set; }


        public string CNS { get; set; }
        public long? CaraterAtendimentoId { get; set; }
        public TabelaDominioDto CaraterAtendimento { get; set; }
        public long? IndicacaoAcidenteId { get; set; }
        public TabelaDominioDto IndicacaoAcidente { get; set; }

        public long? AgendamentoId { get; set; }


        public long? ClassificacaoAtendimentoId { get; set; }

        public ClassificacaoAtendimentoDto ClassificacaoAtendimento { get; set; }

        public long? ProtocoloAtendimentoId { get; set; }

        public ProtocoloAtendimentoDto ProtocoloAtendimento { get; set; }

        public bool IsPendenteExames { get; set; }
        public bool IsPendenteMedicacao { get; set; }
        public bool IsPendenteProcedimento { get; set; }
        public bool IsAtendidoInternado { get; set; }
        public bool IsAtendidoAlta { get; set; }
        public bool IsAtendidoAguardandoInternacao { get; set; }
        public int? StatusAguardando { get; set; }
        public int? StatusAtendido { get; set; }

        public long? AtendimentoOrigemId { get; set; }

        public AtendimentoDto AtendimentoOrigem { get; set; }

        public int OrigemTitular { get; set; }
        public DateTime? DataTomadaDecisao { get; set; }
        
        public long? FaturamentoAtendimentoStatusId { get; set; }
        
        public FaturamentoAtendimentoStatusDto FaturamentoAtendimentoStatus { get; set; }

        #endregion

        #region Mapeamento

        public static Atendimento Mapear(AtendimentoDto atendimentoDto)
        {
            Atendimento atendimento = new Atendimento
            {
                Id = atendimentoDto.Id,
                Codigo = atendimentoDto.Codigo,
                GuiaNumero = atendimentoDto.GuiaNumero,
                Matricula = atendimentoDto.Matricula,
                Responsavel = atendimentoDto.Responsavel,
                RgResponsavel = atendimentoDto.RgResponsavel,
                CpfResponsavel = atendimentoDto.CpfResponsavel,
                NumeroGuia = atendimentoDto.NumeroGuia,
                QtdSessoes = atendimentoDto.QtdSessoes,
                Senha = atendimentoDto.Senha,
                Parentesco = atendimentoDto.Parentesco,
                Observacao = atendimentoDto.Observacao,
                PacienteId = atendimentoDto.PacienteId,
                OrigemId = atendimentoDto.OrigemId,
                MedicoId = atendimentoDto.MedicoId,
                EspecialidadeId = atendimentoDto.EspecialidadeId,
                EmpresaId = atendimentoDto.EmpresaId,
                ConvenioId = atendimentoDto.ConvenioId,
                PlanoId = atendimentoDto.PlanoId,
                AtendimentoStatusId = atendimentoDto.AtendimentoStatusId,
                UnidadeOrganizacionalId = atendimentoDto.UnidadeOrganizacionalId,
                AtendimentoTipoId = atendimentoDto.AtendimentoTipoId,
                GuiaId = atendimentoDto.GuiaId,
                TipoAcompanhanteId = atendimentoDto.TipoAcompanhanteId,
                FatGuiaId = atendimentoDto.FatGuiaId,
                LeitoId = atendimentoDto.LeitoId,
                MotivoAltaId = atendimentoDto.MotivoAltaId,
                NacionalidadeResponsavelId = atendimentoDto.NacionalidadeResponsavelId,
                IsAmbulatorioEmergencia = atendimentoDto.IsAmbulatorioEmergencia,
                IsInternacao = atendimentoDto.IsInternacao,
                IsHomeCare = atendimentoDto.IsHomeCare,
                IsPreatendimento = atendimentoDto.IsPreatendimento,
                ServicoMedicoPrestadoId = atendimentoDto.ServicoMedicoPrestadoId,
                AltaGrupoCIDId = atendimentoDto.AltaGrupoCIDId,
                DataRetorno = atendimentoDto.DataRetorno,
                DataRevisao = atendimentoDto.DataRevisao,
                DataPreatendimento = atendimentoDto.DataPreatendimento,
                DataPrevistaAtendimento = atendimentoDto.DataPrevistaAtendimento,
                DataRegistro = atendimentoDto.DataRegistro,
                DataAlta = atendimentoDto.DataAlta,
                DataAltaMedica = atendimentoDto.DataAltaMedica,
                ValidadeCarteira = atendimentoDto.ValidadeCarteira,
                ValidadeSenha = atendimentoDto.ValidadeSenha,
                DataAutorizacao = atendimentoDto.DataAutorizacao,
                DiasAutorizacao = atendimentoDto.DiasAutorizacao,
                DataPrevistaAlta = atendimentoDto.DataPrevistaAlta,
                TipoAcomodacaoId = atendimentoDto.TipoAcomodacaoId,
                CNS = atendimentoDto.CNS,
                CaraterAtendimentoId = atendimentoDto.CaraterAtendimentoId,
                IndicacaoAcidenteId = atendimentoDto.IndicacaoAcidenteId,
                Titular = atendimentoDto.Titular,
                DataUltimoPagamento = atendimentoDto.DataUltimoPagamento,
                CodDependente = atendimentoDto.CodDependente,
                AtendimentoOrigemId = atendimentoDto.AtendimentoOrigemId,
                OrigemTitular = atendimentoDto.OrigemTitular,
                DataTomadaDecisao = atendimentoDto.DataTomadaDecisao,
                FaturamentoAtendimentoStatusId = atendimentoDto.FaturamentoAtendimentoStatusId
            };


            return atendimento;
        }

        public static AtendimentoDto Mapear(Atendimento atendimento)
        {
            if (atendimento == null)
            {
                return null;
            }

            var atendimentoDto = MapearBase<AtendimentoDto>(atendimento);

            atendimentoDto.Id = atendimento.Id;
            atendimentoDto.Codigo = atendimento.Codigo;
            atendimentoDto.GuiaNumero = atendimento.GuiaNumero;
            atendimentoDto.Matricula = atendimento.Matricula;
            atendimentoDto.Responsavel = atendimento.Responsavel;
            atendimentoDto.RgResponsavel = atendimento.RgResponsavel;
            atendimentoDto.CpfResponsavel = atendimento.CpfResponsavel;
            atendimentoDto.NumeroGuia = atendimento.NumeroGuia;
            atendimentoDto.QtdSessoes = atendimento.QtdSessoes;
            atendimentoDto.Senha = atendimento.Senha;
            atendimentoDto.Parentesco = atendimento.Parentesco;
            atendimentoDto.Observacao = atendimento.Observacao;
            atendimentoDto.PacienteId = atendimento.PacienteId;
            atendimentoDto.OrigemId = atendimento.OrigemId;
            atendimentoDto.MedicoId = atendimento.MedicoId;
            atendimentoDto.EspecialidadeId = atendimento.EspecialidadeId;
            atendimentoDto.EmpresaId = atendimento.EmpresaId;
            atendimentoDto.ConvenioId = atendimento.ConvenioId;
            atendimentoDto.PlanoId = atendimento.PlanoId;
            atendimentoDto.AtendimentoStatusId = atendimento.AtendimentoStatusId;
            atendimentoDto.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
            atendimentoDto.AtendimentoTipoId = atendimento.AtendimentoTipoId;
            atendimentoDto.GuiaId = atendimento.GuiaId;
            atendimentoDto.TipoAcompanhanteId = atendimento.TipoAcompanhanteId;
            atendimentoDto.FatGuiaId = atendimento.FatGuiaId;
            atendimentoDto.LeitoId = atendimento.LeitoId;
            atendimentoDto.MotivoAltaId = atendimento.MotivoAltaId;
            atendimentoDto.NacionalidadeResponsavelId = atendimento.NacionalidadeResponsavelId;
            atendimentoDto.IsAmbulatorioEmergencia = atendimento.IsAmbulatorioEmergencia;
            atendimentoDto.IsInternacao = atendimento.IsInternacao;
            atendimentoDto.IsHomeCare = atendimento.IsHomeCare;
            atendimentoDto.IsPreatendimento = atendimento.IsPreatendimento;
            atendimentoDto.ServicoMedicoPrestadoId = atendimento.ServicoMedicoPrestadoId;
            atendimentoDto.AltaGrupoCIDId = atendimento.AltaGrupoCIDId;
            atendimentoDto.DataRetorno = atendimento.DataRetorno;
            atendimentoDto.DataRevisao = atendimento.DataRevisao;
            atendimentoDto.DataPreatendimento = atendimento.DataPreatendimento;
            atendimentoDto.DataPrevistaAtendimento = atendimento.DataPrevistaAtendimento;
            atendimentoDto.DataRegistro = atendimento.DataRegistro;
            atendimentoDto.DataAlta = atendimento.DataAlta;
            atendimentoDto.DataAltaMedica = atendimento.DataAltaMedica;
            atendimentoDto.ValidadeCarteira = atendimento.ValidadeCarteira;
            atendimentoDto.ValidadeSenha = atendimento.ValidadeSenha;
            atendimentoDto.DataAutorizacao = atendimento.DataAutorizacao;
            atendimentoDto.DiasAutorizacao = atendimento.DiasAutorizacao;
            atendimentoDto.DataPrevistaAlta = atendimento.DataPrevistaAlta;
            atendimentoDto.TipoAcomodacaoId = atendimento.TipoAcomodacaoId;
            atendimentoDto.AtendimentoTipoId = atendimento.AtendimentoTipoId;
            atendimentoDto.CNS = atendimento.CNS;
            atendimentoDto.CaraterAtendimentoId = atendimento.CaraterAtendimentoId;
            atendimentoDto.IndicacaoAcidenteId = atendimento.IndicacaoAcidenteId;

            atendimentoDto.Titular = atendimento.Titular;
            atendimentoDto.DataUltimoPagamento = atendimento.DataUltimoPagamento;
            atendimentoDto.CodDependente = atendimento.CodDependente;
            atendimentoDto.NumeroObito = atendimento.NumeroObito;
            atendimentoDto.DataTomadaDecisao = atendimento.DataTomadaDecisao;
            atendimentoDto.FaturamentoAtendimentoStatusId = atendimento.FaturamentoAtendimentoStatusId;
            //atendimentoDto.Descricao = atendimento.Descricao;
            //atendimentoDto.IsDeleted = atendimento.IsDeleted;
            //atendimentoDto.DeleterUserId = atendimento.DeleterUserId;
            //atendimentoDto.DeletionTime = atendimento.DeletionTime;
            //atendimentoDto.CreatorUserId = atendimento.CreatorUserId;
            //atendimentoDto.CreationTime = atendimento.CreationTime;
            //atendimentoDto.IsSistema = atendimento.IsSistema;
            //atendimentoDto.LastModificationTime = atendimento.LastModificationTime;
            //atendimentoDto.LastModifierUserId = atendimento.LastModifierUserId;



            if (atendimento.Paciente != null)
            {
                atendimentoDto.Paciente = PacienteDto.Mapear(atendimento.Paciente);
            }

            if (atendimento.Empresa != null)
            {
                atendimentoDto.Empresa = EmpresaDto.Mapear(atendimento.Empresa);
            }

            if (atendimento.Medico != null)
            {
                atendimentoDto.Medico = MedicoDto.Mapear(atendimento.Medico);
            }

            if (atendimento.Leito != null)
            {
                atendimentoDto.Leito = LeitoDto.Mapear(atendimento.Leito);
            }

            if (atendimento.Convenio != null)
            {
                atendimentoDto.Convenio = ConvenioDto.Mapear(atendimento.Convenio);
            }

            if (atendimento.FatGuia != null)
            {
                atendimentoDto.FatGuia = FaturamentoGuiaDto.Mapear(atendimento.FatGuia);
            }

            if (atendimento.TipoAcompanhante != null)
            {
                atendimentoDto.TipoAcompanhante = TipoAcompanhanteDto.Mapear(atendimento.TipoAcompanhante);
            }

            if (atendimento.AtendimentoTipo != null)
            {
                atendimentoDto.AtendimentoTipo = TipoAtendimentoDto.Mapear(atendimento.AtendimentoTipo);
            }

            if (atendimento.UnidadeOrganizacional != null)
            {
                atendimentoDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(atendimento.UnidadeOrganizacional);
            }

            if (atendimento.TipoAcomodacao != null)
            {
                atendimentoDto.TipoAcomodacao = TipoAcomodacaoDto.Mapear(atendimento.TipoAcomodacao);
            }

            if (atendimento.Plano != null)
            {
                atendimentoDto.Plano = PlanoDto.Mapear(atendimento.Plano);
            }

            if (atendimento.Origem != null)
            {
                atendimentoDto.Origem = OrigemDto.Mapear(atendimento.Origem);
            }

            if (atendimento.Especialidade != null)
            {
                atendimentoDto.Especialidade = EspecialidadeDto.Mapear(atendimento.Especialidade);
            }

            if (atendimento.CaraterAtendimento != null)
            {
                atendimentoDto.CaraterAtendimento = TabelaDominioDto.Mapear(atendimento.CaraterAtendimento);
            }

            if (atendimento.IndicacaoAcidente != null)
            {
                atendimentoDto.IndicacaoAcidente = TabelaDominioDto.Mapear(atendimento.IndicacaoAcidente);
            }

            if (atendimento.AltaGrupoCID != null)
            {
                atendimentoDto.AltaGrupoCID = GrupoCIDDto.Mapear(atendimento.AltaGrupoCID);
            }

            if (atendimento.AtendimentoStatus != null)
            {
                atendimentoDto.AtendimentoStatus = MapearBase<AtendimentoStatusDto>(atendimento.AtendimentoStatus);
            }

            if (atendimento.FaturamentoAtendimentoStatus != null)
            {
                atendimentoDto.FaturamentoAtendimentoStatus =
                    FaturamentoAtendimentoStatusDto.Mapear(atendimento.FaturamentoAtendimentoStatus);
            }

            if (atendimento.MotivoAlta != null)
            {
                atendimentoDto.MotivoAlta = MotivoAltaDto.Mapear(atendimento.MotivoAlta);
            }

            if (atendimento.Nacionalidade != null)
            {
                atendimentoDto.Nacionalidade = NacionalidadeDto.Mapear(atendimento.Nacionalidade);
            }

            if (atendimento.ServicoMedicoPrestado != null)
            {
                atendimentoDto.ServicoMedicoPrestado = ServicoMedicoPrestadoDto.Mapear(atendimento.ServicoMedicoPrestado);
            }

            atendimentoDto.ClassificacaoAtendimentoId = atendimento.ClassificacaoAtendimentoId;
            if (atendimento.ClassificacaoAtendimento != null)
            {
                atendimentoDto.ClassificacaoAtendimento = ClassificacaoAtendimentoDto.Mapear(atendimento.ClassificacaoAtendimento);
            }

            atendimentoDto.ProtocoloAtendimentoId = atendimento.ProtocoloAtendimentoId;
            if (atendimento.ProtocoloAtendimento != null)
            {
                atendimentoDto.ProtocoloAtendimento = new ProtocoloAtendimentoDto { Id = atendimento.ProtocoloAtendimento.Id, Codigo = atendimento.ProtocoloAtendimento.Codigo, Descricao = atendimento.ProtocoloAtendimento.Descricao };
            }

            atendimentoDto.IsPendenteExames = atendimento.IsPendenteExames;
            atendimentoDto.IsPendenteMedicacao = atendimento.IsPendenteMedicacao;
            atendimentoDto.IsPendenteProcedimento = atendimento.IsPendenteProcedimento;
            atendimentoDto.IsAtendidoInternado = atendimento.IsAtendidoInternado;
            atendimentoDto.IsAtendidoAlta = atendimento.IsAtendidoAlta;
            atendimentoDto.IsAtendidoAguardandoInternacao = atendimento.IsAtendidoAguardandoInternacao;
            atendimentoDto.StatusAguardando = atendimento.StatusAguardando;
            atendimentoDto.StatusAtendido = atendimento.StatusAtendido;

            atendimentoDto.AtendimentoOrigemId = atendimento.AtendimentoOrigemId;
            atendimentoDto.OrigemTitular = atendimento.OrigemTitular;

            return atendimentoDto;
        }

        public static IEnumerable<Atendimento> Mapear(List<AtendimentoDto> atendimentoDto)
        {
            foreach (var item in atendimentoDto)
            {
                Atendimento atendimento = new Atendimento();

                atendimento.Id = item.Id;
                atendimento.Codigo = item.Codigo;
                atendimento.GuiaNumero = item.GuiaNumero;
                atendimento.Matricula = item.Matricula;
                atendimento.Responsavel = item.Responsavel;
                atendimento.RgResponsavel = item.RgResponsavel;
                atendimento.CpfResponsavel = item.CpfResponsavel;
                atendimento.NumeroGuia = item.NumeroGuia;
                atendimento.QtdSessoes = item.QtdSessoes;
                atendimento.Senha = item.Senha;
                atendimento.Parentesco = item.Parentesco;
                atendimento.Observacao = item.Observacao;
                atendimento.PacienteId = item.PacienteId;
                atendimento.OrigemId = item.OrigemId;
                atendimento.MedicoId = item.MedicoId;
                atendimento.EspecialidadeId = item.EspecialidadeId;
                atendimento.EmpresaId = item.EmpresaId;
                atendimento.ConvenioId = item.ConvenioId;
                atendimento.PlanoId = item.PlanoId;
                atendimento.AtendimentoStatusId = item.AtendimentoStatusId;
                atendimento.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                atendimento.AtendimentoTipoId = item.AtendimentoTipoId;
                atendimento.GuiaId = item.GuiaId;
                atendimento.TipoAcompanhanteId = item.TipoAcompanhanteId;
                atendimento.FatGuiaId = item.FatGuiaId;
                atendimento.LeitoId = item.LeitoId;
                atendimento.MotivoAltaId = item.MotivoAltaId;
                atendimento.NacionalidadeResponsavelId = item.NacionalidadeResponsavelId;
                atendimento.IsAmbulatorioEmergencia = item.IsAmbulatorioEmergencia;
                atendimento.IsInternacao = item.IsInternacao;
                atendimento.IsHomeCare = item.IsHomeCare;
                atendimento.IsPreatendimento = item.IsPreatendimento;
                atendimento.ServicoMedicoPrestadoId = item.ServicoMedicoPrestadoId;
                atendimento.AltaGrupoCIDId = item.AltaGrupoCIDId;
                atendimento.DataRetorno = item.DataRetorno;
                atendimento.DataRevisao = item.DataRevisao;
                atendimento.DataPreatendimento = item.DataPreatendimento;
                atendimento.DataPrevistaAtendimento = item.DataPrevistaAtendimento;
                atendimento.DataRegistro = item.DataRegistro;
                atendimento.DataAlta = item.DataAlta;
                atendimento.DataAltaMedica = item.DataAltaMedica;
                atendimento.ValidadeCarteira = item.ValidadeCarteira;
                atendimento.ValidadeSenha = item.ValidadeSenha;
                atendimento.DataAutorizacao = item.DataAutorizacao;
                atendimento.DiasAutorizacao = item.DiasAutorizacao;
                atendimento.DataPrevistaAlta = item.DataPrevistaAlta;
                atendimento.TipoAcomodacaoId = item.TipoAcomodacaoId;
                atendimento.CNS = item.CNS;
                atendimento.CaraterAtendimentoId = item.CaraterAtendimentoId;
                atendimento.IndicacaoAcidenteId = item.IndicacaoAcidenteId;

                yield return atendimento;
            }
        }

        public static IEnumerable<AtendimentoDto> Mapear(List<Atendimento> atendimento)
        {
            foreach (var item in atendimento)
            {
                AtendimentoDto atendimentoDto = new AtendimentoDto();

                atendimentoDto.Id = item.Id;
                atendimentoDto.Codigo = item.Codigo;
                atendimentoDto.GuiaNumero = item.GuiaNumero;
                atendimentoDto.Matricula = item.Matricula;
                atendimentoDto.Responsavel = item.Responsavel;
                atendimentoDto.RgResponsavel = item.RgResponsavel;
                atendimentoDto.CpfResponsavel = item.CpfResponsavel;
                atendimentoDto.NumeroGuia = item.NumeroGuia;
                atendimentoDto.QtdSessoes = item.QtdSessoes;
                atendimentoDto.Senha = item.Senha;
                atendimentoDto.Parentesco = item.Parentesco;
                atendimentoDto.Observacao = item.Observacao;
                atendimentoDto.PacienteId = item.PacienteId;
                atendimentoDto.OrigemId = item.OrigemId;
                atendimentoDto.MedicoId = item.MedicoId;
                atendimentoDto.EspecialidadeId = item.EspecialidadeId;
                atendimentoDto.EmpresaId = item.EmpresaId;
                atendimentoDto.ConvenioId = item.ConvenioId;
                atendimentoDto.PlanoId = item.PlanoId;
                atendimentoDto.AtendimentoStatusId = item.AtendimentoStatusId;
                atendimentoDto.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                atendimentoDto.AtendimentoTipoId = item.AtendimentoTipoId;
                atendimentoDto.GuiaId = item.GuiaId;
                atendimentoDto.TipoAcompanhanteId = item.TipoAcompanhanteId;
                atendimentoDto.FatGuiaId = item.FatGuiaId;
                atendimentoDto.LeitoId = item.LeitoId;
                atendimentoDto.MotivoAltaId = item.MotivoAltaId;
                atendimentoDto.NacionalidadeResponsavelId = item.NacionalidadeResponsavelId;
                atendimentoDto.IsAmbulatorioEmergencia = item.IsAmbulatorioEmergencia;
                atendimentoDto.IsInternacao = item.IsInternacao;
                atendimentoDto.IsHomeCare = item.IsHomeCare;
                atendimentoDto.IsPreatendimento = item.IsPreatendimento;
                atendimentoDto.ServicoMedicoPrestadoId = item.ServicoMedicoPrestadoId;
                atendimentoDto.AltaGrupoCIDId = item.AltaGrupoCIDId;
                atendimentoDto.DataRetorno = item.DataRetorno;
                atendimentoDto.DataRevisao = item.DataRevisao;
                atendimentoDto.DataPreatendimento = item.DataPreatendimento;
                atendimentoDto.DataPrevistaAtendimento = item.DataPrevistaAtendimento;
                atendimentoDto.DataRegistro = item.DataRegistro;
                atendimentoDto.DataAlta = item.DataAlta;
                atendimentoDto.DataAltaMedica = item.DataAltaMedica;
                atendimentoDto.ValidadeCarteira = item.ValidadeCarteira;
                atendimentoDto.ValidadeSenha = item.ValidadeSenha;
                atendimentoDto.DataAutorizacao = item.DataAutorizacao;
                atendimentoDto.DiasAutorizacao = item.DiasAutorizacao;
                atendimentoDto.DataPrevistaAlta = item.DataPrevistaAlta;
                atendimentoDto.TipoAcomodacaoId = item.TipoAcomodacaoId;
                atendimentoDto.AtendimentoTipoId = item.AtendimentoTipoId;
                atendimentoDto.CNS = item.CNS;
                atendimentoDto.CaraterAtendimentoId = item.CaraterAtendimentoId;
                atendimentoDto.IndicacaoAcidenteId = item.IndicacaoAcidenteId;

                if (item.Paciente != null)
                {
                    atendimentoDto.Paciente = PacienteDto.Mapear(item.Paciente);
                }

                if (item.Medico != null)
                {
                    atendimentoDto.Medico = MedicoDto.Mapear(item.Medico);
                }

                if (item.Empresa != null)
                {
                    atendimentoDto.Empresa = EmpresaDto.Mapear(item.Empresa);
                }

                if (item.Leito != null)
                {
                    atendimentoDto.Leito = LeitoDto.Mapear(item.Leito);
                }

                if (item.Convenio != null)
                {
                    atendimentoDto.Convenio = ConvenioDto.Mapear(item.Convenio);
                }

                if (item.FatGuia != null)
                {
                    atendimentoDto.FatGuia = FaturamentoGuiaDto.Mapear(item.FatGuia);
                }

                if (item.AtendimentoTipo != null)
                {
                    atendimentoDto.AtendimentoTipo = TipoAtendimentoDto.Mapear(item.AtendimentoTipo);
                }

                if (item.UnidadeOrganizacional != null)
                {
                    atendimentoDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(item.UnidadeOrganizacional);
                }

                if (item.TipoAcomodacao != null)
                {
                    atendimentoDto.TipoAcomodacao = TipoAcomodacaoDto.Mapear(item.TipoAcomodacao);
                }

                if (item.Plano != null)
                {
                    atendimentoDto.Plano = PlanoDto.Mapear(item.Plano);
                }

                if (item.Origem != null)
                {
                    atendimentoDto.Origem = OrigemDto.Mapear(item.Origem);
                }

                if (item.CaraterAtendimento != null)
                {
                    atendimentoDto.CaraterAtendimento = TabelaDominioDto.Mapear(item.CaraterAtendimento);
                }

                if (item.IndicacaoAcidente != null)
                {
                    atendimentoDto.IndicacaoAcidente = TabelaDominioDto.Mapear(item.IndicacaoAcidente);
                }

                yield return atendimentoDto;
            }
        }

        public static AtendimentoDto MapearFromCore(Atendimento atendimento)
        {
            var atendimentoDto = new AtendimentoDto();

            atendimentoDto.Id = atendimento.Id;
            atendimentoDto.Titular = atendimento.Titular;
            atendimentoDto.GuiaNumero = atendimento.GuiaNumero;
            atendimentoDto.Matricula = atendimento.Matricula;
            atendimentoDto.Responsavel = atendimento.Responsavel;
            atendimentoDto.RgResponsavel = atendimento.RgResponsavel;
            atendimentoDto.CpfResponsavel = atendimento.CpfResponsavel;
            atendimentoDto.NumeroGuia = atendimento.NumeroGuia;
            atendimentoDto.QtdSessoes = atendimento.QtdSessoes;
            atendimentoDto.Senha = atendimento.Senha;
            atendimentoDto.Parentesco = atendimento.Parentesco;
            atendimentoDto.Observacao = atendimento.Observacao;
            atendimentoDto.PacienteId = atendimento.PacienteId;
            atendimentoDto.OrigemId = atendimento.OrigemId;
            atendimentoDto.MedicoId = atendimento.MedicoId;
            atendimentoDto.EspecialidadeId = atendimento.EspecialidadeId;
            atendimentoDto.EmpresaId = atendimento.EmpresaId;
            atendimentoDto.ConvenioId = atendimento.ConvenioId;
            atendimentoDto.PlanoId = atendimento.PlanoId;
            atendimentoDto.AtendimentoStatusId = atendimento.AtendimentoStatusId;
            atendimentoDto.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
            atendimentoDto.AtendimentoTipoId = atendimento.AtendimentoTipoId;
            atendimentoDto.GuiaId = atendimento.GuiaId;
            atendimentoDto.TipoAcompanhanteId = atendimento.TipoAcompanhanteId;
            atendimentoDto.FatGuiaId = atendimento.FatGuiaId;
            atendimentoDto.LeitoId = atendimento.LeitoId;
            atendimentoDto.MotivoAltaId = atendimento.MotivoAltaId;
            atendimentoDto.NacionalidadeResponsavelId = atendimento.NacionalidadeResponsavelId;
            atendimentoDto.IsAmbulatorioEmergencia = atendimento.IsAmbulatorioEmergencia;
            atendimentoDto.IsInternacao = atendimento.IsInternacao;
            atendimentoDto.IsHomeCare = atendimento.IsHomeCare;
            atendimentoDto.IsPreatendimento = atendimento.IsPreatendimento;
            atendimentoDto.ServicoMedicoPrestadoId = atendimento.ServicoMedicoPrestadoId;
            atendimentoDto.TelefonePreAtendimento = "";
            atendimentoDto.IdentPreAtendimento = "";
            atendimentoDto.CpfPreAtendimento = "";
            atendimentoDto.NomePreAtendimento = "";
            atendimentoDto.AltaGrupoCIDId = atendimento.AltaGrupoCIDId;
            atendimentoDto.DataRetorno = atendimento.DataRetorno;
            atendimentoDto.DataRevisao = atendimento.DataRevisao;
            atendimentoDto.DataPreatendimento = atendimento.DataPreatendimento;
            atendimentoDto.DataPrevistaAtendimento = atendimento.DataPrevistaAtendimento;
            atendimentoDto.DataRegistro = atendimento.DataRegistro;
            atendimentoDto.DataAlta = atendimento.DataAlta;
            atendimentoDto.DataAltaMedica = atendimento.DataAltaMedica;
            atendimentoDto.ValidadeCarteira = atendimento.ValidadeCarteira;
            atendimentoDto.ValidadeSenha = atendimento.ValidadeSenha;
            atendimentoDto.DataAutorizacao = atendimento.DataAutorizacao;
            atendimentoDto.DiasAutorizacao = atendimento.DiasAutorizacao;
            atendimentoDto.DataNascPreAtendimento = null;
            atendimentoDto.DataPrevistaAlta = atendimento.DataPrevistaAlta;
            atendimentoDto.Nacionalidade = atendimento.Nacionalidade?.MapTo<NacionalidadeDto>();
            atendimentoDto.Paciente = atendimento.Paciente?.MapTo<PacienteDto>();
            atendimentoDto.Origem = atendimento.Origem?.MapTo<OrigemDto>();
            atendimentoDto.Medico = atendimento.Medico?.MapTo<MedicoDto>();
            atendimentoDto.Especialidade = atendimento.Especialidade?.MapTo<EspecialidadeDto>();
            atendimentoDto.Empresa = atendimento.Empresa?.MapTo<EmpresaDto>();
            atendimentoDto.Convenio = atendimento.Convenio?.MapTo<ConvenioDto>();
            atendimentoDto.Plano = atendimento.Plano?.MapTo<PlanoDto>();
            atendimentoDto.AtendimentoTipo = null;
            atendimentoDto.Guia = null;
            atendimentoDto.FatGuia = atendimento.FatGuia?.MapTo<FaturamentoGuiaDto>();
            atendimentoDto.ServicoMedicoPrestado = atendimento.ServicoMedicoPrestado.MapTo<ServicoMedicoPrestadoDto>();
            atendimentoDto.UnidadeOrganizacional = atendimento.UnidadeOrganizacional?.MapTo<UnidadeOrganizacionalDto>();
            atendimentoDto.CaraterAtendimentoId = atendimento.CaraterAtendimentoId;
            atendimentoDto.IndicacaoAcidenteId = atendimento.IndicacaoAcidenteId;
            atendimentoDto.CNS = atendimento.CNS;

            if (atendimento.CaraterAtendimento != null)
            {
                atendimentoDto.CaraterAtendimento = TabelaDominioDto.Mapear(atendimento.CaraterAtendimento);
            }

            if (atendimento.IndicacaoAcidente != null)
            {
                atendimentoDto.IndicacaoAcidente = TabelaDominioDto.Mapear(atendimento.IndicacaoAcidente);
            }

            if (atendimento.Leito != null)
            {
                atendimentoDto.Leito = LeitoDto.MapearFromCore(atendimento.Leito);
            }

            atendimentoDto.MotivoAlta = atendimento.MotivoAlta?.MapTo<MotivoAltaDto>();
            atendimentoDto.AltaGrupoCID = null;

            return atendimentoDto;
        }






        #endregion




    }
}
