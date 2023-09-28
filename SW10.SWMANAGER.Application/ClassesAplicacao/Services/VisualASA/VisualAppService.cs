using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Religioes;
using SW10.SWMANAGER.ClassesAplicacao.VisualASA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.VisualASA
{
    public class VisualAppService : SWMANAGERAppServiceBase, IVisualAppService
    {
        private readonly IRepository<Sis_Atendimento, long> _sis_AtendimentoRepository;
        private readonly IRepository<Sis_Ambulatorio, long> _sis_AmbulatorioRepository;
        private readonly IRepository<Sis_ContaMedica, long> _sis_ContaMedicaRepository;
        private readonly IRepository<Sis_Internacao, long> _sis_InternacaoRepository;
        private readonly IRepository<Sis_Pessoa, long> _sis_PessoaRepository;
        private readonly IRepository<Sis_Paciente, long> _sis_PacienteRepository;
        private readonly IRepository<Atendimento, long> _atendimentoRepository;
        private readonly IRepository<SisPessoa, long> _sisPessoaRepository;
        private readonly IRepository<Religiao, long> _ReligiaoRepository;
        private readonly IRepository<Escolaridade, long> _EscolaridadeRepository;
        private readonly IRepository<FaturamentoConta, long> _faturamentoContaRepository;
        private readonly IRepository<TipoAcomodacao, long> _tipoAcomodacaoRepository;
        private readonly IRepository<Pro_ReqExameMov, long> _pro_ReqExameMovRepository;
        private readonly IRepository<Pro_ReqExameMovItem, long> _pro_ReqExameMovItemRepository;
        private readonly IRepository<SolicitacaoExame, long> _solicitacaoExameRepository;
        private readonly IRepository<SolicitacaoExameItem, long> _solicitacaoExameItemRepository;
        IRepository<Resultado, long> _resultadoRepository;
        IRepository<ResultadoExame, long> _resultadoExameItemRepository;

        public VisualAppService(
            IRepository<Sis_Atendimento, long> sis_AtendimentoRepository,
            IRepository<Sis_Ambulatorio, long> sis_AmbulatorioRepository,
            IRepository<Sis_ContaMedica, long> sis_ContaMedicaRepository,
            IRepository<Sis_Internacao, long> sis_InternacaoRepository,
            IRepository<Sis_Pessoa, long> sis_PessoaRepository,
            IRepository<Sis_Paciente, long> sis_PacienteRepository,
            IRepository<Atendimento, long> atendimentoRepository,
            IRepository<SisPessoa, long> pessoaRepository,
            IRepository<Religiao, long> religiaoRepository,
            IRepository<Escolaridade, long> escolaridadeRepository,
            IRepository<FaturamentoConta, long> faturamentoContaRepository,
            IRepository<TipoAcomodacao, long> tipoAcomodacaoRepository,
            IRepository<Pro_ReqExameMov, long> pro_ReqExameMovRepository,
            IRepository<Pro_ReqExameMovItem, long> pro_ReqExameMovItemRepository,
            IRepository<SolicitacaoExame, long> solicitacaoExameRepository,
            IRepository<SolicitacaoExameItem, long> solicitacaoExameItemRepository,
            IRepository<Resultado, long> resultadoRepository,
            IRepository<ResultadoExame, long> resultadoExameItemRepository
        )
        {
            _sis_AtendimentoRepository = sis_AtendimentoRepository;
            _sis_AmbulatorioRepository = sis_AmbulatorioRepository;
            _sis_ContaMedicaRepository = sis_ContaMedicaRepository;
            _sis_InternacaoRepository = sis_InternacaoRepository;
            _sis_PessoaRepository = sis_PessoaRepository;
            _sis_PacienteRepository = sis_PacienteRepository;
            _atendimentoRepository = atendimentoRepository;
            _sisPessoaRepository = pessoaRepository;
            _ReligiaoRepository = religiaoRepository;
            _EscolaridadeRepository = escolaridadeRepository;
            _faturamentoContaRepository = faturamentoContaRepository;
            _tipoAcomodacaoRepository = tipoAcomodacaoRepository;
            _pro_ReqExameMovRepository = pro_ReqExameMovRepository;
            _pro_ReqExameMovItemRepository = pro_ReqExameMovItemRepository;
            _solicitacaoExameRepository = solicitacaoExameRepository;
            _solicitacaoExameItemRepository = solicitacaoExameItemRepository;
            _resultadoRepository = resultadoRepository;
            _resultadoExameItemRepository = resultadoExameItemRepository;
        }


        public void MigrarVisualASA(long atendimentosId)
        {
            CarregarSisAtendimento(atendimentosId);
        }

        public void MigrarSisPessoa(long pessoaId)
        {
            CarregarSisPessoa(pessoaId);
        }

        private Sis_Atendimento CarregarSisAtendimento(long atendimentoId)
        {
            try
            {
                var atendimento = _atendimentoRepository.GetAll()
                                                        .Include(m => m.AtendimentoStatus)
                                                        .Include(m => m.Empresa)
                                                        .Include(m => m.Especialidade)
                                                        .Include(m => m.UnidadeOrganizacional)
                                                        .Include(m => m.Leito)
                                                        .Include(m => m.Medico)
                                                        .Include(m => m.Origem)
                                                        .Include(m => m.Paciente)
                                                        .Include(m => m.Convenio)
                                                        .Include(m => m.Plano)
                                                        .Include(m => m.TipoAcomodacao)
                                                        .Include(m => m.ServicoMedicoPrestado)
                                                        .Include(m => m.AtendimentoTipo)
                                                        .Include(m => m.FatGuia)
                                                        .Where(w => w.Id == atendimentoId)
                                                        .FirstOrDefault();
                Sis_Atendimento sis_Atendimento = null;

                if (atendimento != null)
                {
                    sis_Atendimento = _sis_AtendimentoRepository.GetAll()
                        .Where(w => w.IDImportado == atendimentoId)
                        .FirstOrDefault();

                    if (sis_Atendimento == null)
                    {
                        sis_Atendimento = new Sis_Atendimento();
                    }
                    sis_Atendimento.AgudaCronica = null;
                    sis_Atendimento.Ano = null;
                    sis_Atendimento.CodAtendimento = null;  //atendimento.Codigo;
                    sis_Atendimento.Codigo = atendimento.Codigo;
                    //sis_Atendimento.CreationTime = null;
                    sis_Atendimento.CreatorUserId = null;
                    sis_Atendimento.DataAlteracao = null;
                    sis_Atendimento.DataAtendimento = atendimento.DataRegistro;
                    sis_Atendimento.DataCancelamento = null;
                    sis_Atendimento.DataCancelaRecebimento = null;
                    sis_Atendimento.DataConclusao = null;
                    sis_Atendimento.DataInclusao = atendimento.DataRegistro;
                    sis_Atendimento.DataMedicoConsulta = null; //atendimento.DataRegistro;
                    sis_Atendimento.DataObsRecebimento = null;
                    sis_Atendimento.DataRecebimento = null;
                    sis_Atendimento.DataRetorno = atendimento.DataRetorno;
                    sis_Atendimento.DeleterUserId = null;
                    sis_Atendimento.DeletionTime = null;
                    sis_Atendimento.Desativado = null;
                    sis_Atendimento.Descricao = atendimento.Descricao;
                    sis_Atendimento.Hidden = null;
                    //sis_Atendimento.Id = "";
                    sis_Atendimento.Idade = null;
                    sis_Atendimento.IDAteMotCancelamento = null;
                    sis_Atendimento.IDAtendimento = null;//antigo
                    sis_Atendimento.IDSW = (int)atendimento.Id;//novo
                    //sis_Atendimento.IDAtendimentoInicial = (int)atendimento.AtendimentoTipoId; //verificar com márcio não faz sentido este relacionamento.
                    if (atendimento.AtendimentoStatusId.HasValue)
                    {
                        sis_Atendimento.IDAtendimentoStatus = atendimento.AtendimentoStatus.ImportaId; // (int)atendimento.AtendimentoStatusId.Value;// esse aqui que apresenta inconsistência
                    }
                    sis_Atendimento.IDClinica = null;
                    sis_Atendimento.IDConvenioAtend = atendimento.Convenio.ImportaId;
                    sis_Atendimento.IDEmpresa = atendimento.Empresa.ImportaId; //(int)atendimento.EmpresaId;
                    sis_Atendimento.IDEspecialidade = atendimento.Especialidade?.ImportaId; //(int)atendimento.EspecialidadeId;
                    sis_Atendimento.IDEspecialidadeMedIndica = null;
                    sis_Atendimento.IDFilial = atendimento.UnidadeOrganizacional.ImportaId; //(int)atendimento.UnidadeOrganizacionalId;
                    sis_Atendimento.IDFilialSin = null;
                    sis_Atendimento.IDImportado = atendimento.ImportaId; //(int)atendimento.Id;
                    sis_Atendimento.IDIndicadorAcidente = null;
                    sis_Atendimento.IDMedico = atendimento.Medico.ImportaId; //(int)atendimento.MedicoId;
                    sis_Atendimento.IDMedicoConsulta = null; // atendimento.Medico.ImportaId; //(int)atendimento.MedicoId;
                    sis_Atendimento.IDMedicoIndica = null;
                    sis_Atendimento.IDOrigem = atendimento.Origem.ImportaId; //(int)atendimento.OrigemId;
                    sis_Atendimento.IDPaciente = atendimento.Paciente.ImportaId; //(int)atendimento.PacienteId;
                    sis_Atendimento.IDRevisaoEntrega = null;
                    sis_Atendimento.IDUltUsuConfEmail = false;
                    sis_Atendimento.IDUsuarioAlteracao = null;
                    sis_Atendimento.IDUsuarioCancelamento = null;
                    sis_Atendimento.IDUsuarioCancelaRecebimento = null;
                    sis_Atendimento.IDUsuarioInclusao = null;
                    sis_Atendimento.IDUsuarioObsRecebimento = null;
                    sis_Atendimento.IDUsuarioRecebimento = null;
                    sis_Atendimento.IDUsuarioRetorno = null;
                    sis_Atendimento.IsAlterado = false;
                    sis_Atendimento.IsDeleted = false;
                    sis_Atendimento.IsEncaminhado = null;
                    sis_Atendimento.IsInternou = atendimento.IsInternacao;
                    sis_Atendimento.IsSincronizado = false;
                    sis_Atendimento.IsSistema = false;
                    sis_Atendimento.IsSMSConfirmado = false;
                    sis_Atendimento.IsSMSEnviado = false;
                    sis_Atendimento.JustificativaNumDeclNascVivo = "";
                    sis_Atendimento.LastModificationTime = null;
                    sis_Atendimento.LastModifierUserId = null;
                    sis_Atendimento.Mes = null;
                    sis_Atendimento.ObsRecebimento = "";
                    sis_Atendimento.ObsRetorno = null;
                    sis_Atendimento.PacienteCaixa = "";
                    sis_Atendimento.System = null;
                    sis_Atendimento.TenantId = AbpSession.TenantId;
                    //var db = new SWMANAGERDbContext();

                    //var query = db.Database.SqlQuery<string>("SELECT CAST(IMPORTAID AS VARCHAR(18)) FROM ATEATENDIMENTO WHERE ID=" + atendimento.Id).FirstOrDefault();
                    //var idAtendimento = 0;
                    //db.Dispose();

                    //if (int.TryParse(query, out idAtendimento))
                    //{
                    //    sis_Atendimento.IDAtendimento = idAtendimento;
                    //}

                    long atendId = 0;

                    if (sis_Atendimento.IDEspecialidade != null)
                    {
                        if (sis_Atendimento.Id == 0)
                        {
                            atendId = _sis_AtendimentoRepository.InsertAndGetId(sis_Atendimento);
                        }
                        else
                        {
                            _sis_AtendimentoRepository.Update(sis_Atendimento);
                            atendId = sis_Atendimento.Id;
                        }

                        if (atendimento.IsAmbulatorioEmergencia)
                        {
                            CarregarSisAmbulatorio(atendimento, atendId);
                        }

                        if (atendimento.IsInternacao)
                        {
                            CarregarSisInternacao(atendimento, atendId);
                        }

                        CarregarSisContaMedica(atendimento, atendId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
            return null;
        }

        private Sis_Ambulatorio CarregarSisAmbulatorio(Atendimento ambulatorio, long sis_ambulatorioId)
        {
            Sis_Ambulatorio sis_Ambulatorio = null;

            if (ambulatorio != null)
            {
                sis_Ambulatorio = _sis_AmbulatorioRepository.GetAll()
                    .Where(w => w.IDAmbulatorio == sis_ambulatorioId)
                    .FirstOrDefault();

                if (sis_Ambulatorio == null)
                {
                    sis_Ambulatorio = new Sis_Ambulatorio();
                }

                sis_Ambulatorio.CodAmbulatorio = null; // ambulatorio.Codigo;
                sis_Ambulatorio.CodAmbulatorioSUS = null;
                sis_Ambulatorio.Codigo = ambulatorio.Codigo;
                //sis_Ambulatorio.CreationTime = null;
                sis_Ambulatorio.CreatorUserId = null;
                sis_Ambulatorio.DadosClinicos = null;
                sis_Ambulatorio.DataAltaAdministrativa = null;
                sis_Ambulatorio.DataAltaAmbulatorial = null;
                sis_Ambulatorio.DataAltaMedica = ambulatorio.DataAltaMedica;
                sis_Ambulatorio.DataAtendAmbulatorial = null;
                sis_Ambulatorio.DataExame = null;
                sis_Ambulatorio.DataFimInfoClinicas = null;
                sis_Ambulatorio.DataFimPreAtend = null;
                sis_Ambulatorio.DataInicio = null; // ambulatorio.DataRegistro;
                sis_Ambulatorio.DataIniInfoClinicas = ambulatorio.DataRegistro;
                sis_Ambulatorio.DataIniPreAtend = ambulatorio.DataPreatendimento;
                sis_Ambulatorio.DataLiberacao = null;
                sis_Ambulatorio.DataPreAtend = null;
                sis_Ambulatorio.DataRetorno = ambulatorio.DataRetorno;
                sis_Ambulatorio.DataSolicitacao = null;
                sis_Ambulatorio.DeleterUserId = null;
                sis_Ambulatorio.DeletionTime = null;
                sis_Ambulatorio.Descricao = ambulatorio.Descricao;
                sis_Ambulatorio.Diagnostico = "";
                //sis_Ambulatorio.Id = ambulatorio.Id;
                //if (ambulatorio.MotivoAltaId.HasValue)
                //{
                sis_Ambulatorio.IDAlta = 5; //ambulatorio.MotivoAlta.ImportaId; //(int)ambulatorio.MotivoAltaId;
                //}
                sis_Ambulatorio.IDAltaAmbulatorial = null;
                sis_Ambulatorio.IDAmbulatorio = ambulatorio.ImportaId; //null;//antigo
                sis_Ambulatorio.IDSW = (int)ambulatorio.Id;//novo
                sis_Ambulatorio.IDAtendRevisao = null;
                sis_Ambulatorio.IDMedicoAtendendo = null; // ambulatorio.Medico.ImportaId; //(int)ambulatorio.MedicoId;
                sis_Ambulatorio.IDMedPreAtend = null; //ambulatorio.Medico.ImportaId; //(int)ambulatorio.MedicoId;
                sis_Ambulatorio.IDPrioridadeAtendimento = null;
                sis_Ambulatorio.IDProtocoloEmergencia = null;
                sis_Ambulatorio.IDSetor = null;
                sis_Ambulatorio.IDUsuarioAltaInc = null;
                sis_Ambulatorio.IDUsuarioLiberacao = null;
                sis_Ambulatorio.IDUsuarioRevelia = null;
                sis_Ambulatorio.IsAlergiaSzn = null;
                sis_Ambulatorio.IsAltaRevelia = null;
                sis_Ambulatorio.IsAtendendo = null;
                sis_Ambulatorio.IsDeleted = false;
                sis_Ambulatorio.IsHoraMarcada = null; // ambulatorio.IsAmbulatorioEmergencia;
                sis_Ambulatorio.IsRevisao = null;
                sis_Ambulatorio.IsSistema = false;
                sis_Ambulatorio.IsVacina = null;
                sis_Ambulatorio.LastModificationTime = null;
                sis_Ambulatorio.LastModifierUserId = null;
                sis_Ambulatorio.NumeroSeq = "";
                sis_Ambulatorio.PrimConsulta = null;
                sis_Ambulatorio.QualAlergiaSzn = null;
                if (ambulatorio.ServicoMedicoPrestadoId.HasValue)
                {
                    sis_Ambulatorio.TipoConsulta = ambulatorio.ServicoMedicoPrestado.ImportaId.Value.ToString();
                }
                sis_Ambulatorio.Tratamento = null;
                sis_Ambulatorio.StatusProntoAtend = null;
                sis_Ambulatorio.TenantId = AbpSession.TenantId;

                //sis_Ambulatorio.TipoConsulta = ambulatorio.AtendimentoTipo;

                //var db = new SWMANAGERDbContext();

                //var query = db.Database.SqlQuery<string>("SELECT CAST(IMPORTAID AS VARCHAR(18)) FROM ATEATENDIMENTO WHERE ID=" + ambulatorio.Id).FirstOrDefault();

                //var idAtendimento = 0;
                //db.Dispose();

                //if (int.TryParse(query, out idAtendimento))
                //{
                //    sis_Ambulatorio.IDAmbulatorio = idAtendimento;
                //}

                if (sis_Ambulatorio.Id == 0)
                {
                    _sis_AmbulatorioRepository.Insert(sis_Ambulatorio);

                }
                else
                {
                    _sis_AmbulatorioRepository.Update(sis_Ambulatorio);
                }

            }

            return null;
        }

        private Sis_Internacao CarregarSisInternacao(Atendimento internacao, long sis_internacaoId)
        {
            try
            {
                Sis_Internacao sis_Internacao = null;

                if (internacao != null)
                {
                    sis_Internacao = _sis_InternacaoRepository.GetAll()
                        .Where(w => w.IDInternacao == sis_internacaoId)
                        .FirstOrDefault();

                    if (sis_Internacao == null)
                    {
                        sis_Internacao = new Sis_Internacao();
                    }


                    sis_Internacao.CEPResponsa = null;
                    sis_Internacao.CGCResponsa = null;
                    sis_Internacao.Cobertura = null;
                    sis_Internacao.Codigo = internacao.Codigo;
                    sis_Internacao.CodInternacao = null; // internacao.Codigo;
                    sis_Internacao.CompResponsa = null;
                    sis_Internacao.CPFResponsa = internacao.CpfResponsavel;
                    //sis_Internacao.CreationTime = null;
                    sis_Internacao.CreatorUserId = null;
                    sis_Internacao.DataAlta = internacao.DataAlta;
                    sis_Internacao.DataAltaAlt = null;
                    sis_Internacao.DataAltaDel = null;
                    sis_Internacao.DataAltaInc = null;
                    sis_Internacao.DataPrevAltaAlt = internacao.DataPrevistaAlta;
                    sis_Internacao.DataPrevAltaDel = null;
                    sis_Internacao.DataPrevAltaInc = null;
                    //sis_Internacao.DataPrevisaoAlta = internacao.DataPrevistaAlta;
                    sis_Internacao.DataPront = null;
                    sis_Internacao.DeleterUserId = null;
                    sis_Internacao.DeletionTime = null;
                    sis_Internacao.Descricao = internacao.Descricao;
                    sis_Internacao.DietaAtual = null;
                    sis_Internacao.EmisIdtResponsa = null;
                    sis_Internacao.EndResponsa = null;
                    //sis_Internacao.Id = internacao.Id;
                    sis_Internacao.IDAcompanhante = null;
                    if (internacao.MotivoAltaId.HasValue)
                    {
                        sis_Internacao.IDAlta = internacao.MotivoAlta.ImportaId; //(int)internacao.MotivoAltaId;

                    }
                    sis_Internacao.IDBairroResponsa = null;
                    sis_Internacao.IDCidadeResponsa = null;
                    if (internacao.AltaGrupoCIDId.HasValue)
                    {
                        sis_Internacao.IDCIDObito = internacao.AltaGrupoCID.ImportaId; //(int)internacao.AltaGrupoCIDId;
                    }
                    sis_Internacao.IDEstadoPac = null;
                    sis_Internacao.IDEstadoResponsa = null;
                    sis_Internacao.IDInternacao = internacao.ImportaId; //null;//antigo
                    sis_Internacao.IDSW = (int)internacao.Id;//novo
                    if (internacao.LeitoId.HasValue)
                    {
                        sis_Internacao.IDLeito = internacao.Leito.ImportaId; //(int)internacao.LeitoId;
                    }
                    if (internacao.TipoAcomodacaoId.HasValue)
                    {
                        sis_Internacao.IDLeitoTipo = internacao.TipoAcomodacao.ImportaId; //(int)internacao.TipoAcomodacaoId;
                    }
                    sis_Internacao.IdtResponsa = internacao.RgResponsavel;
                    sis_Internacao.IDUsuarioAltaAlt = null;
                    sis_Internacao.IDUsuarioAltaDel = null;
                    sis_Internacao.IDUsuarioAltaInc = null;
                    sis_Internacao.IDUsuarioPrevAltaAlt = null;
                    sis_Internacao.IDUsuarioPrevAltaDel = null;
                    sis_Internacao.IDUsuarioPrevAltaInc = null;
                    sis_Internacao.IDUsuarioPront = null;
                    sis_Internacao.IsAborto = null;
                    sis_Internacao.IsAlergiaSzn = null;
                    sis_Internacao.IsAtendRNSalaParto = null;
                    sis_Internacao.IsBxPeso = null;
                    sis_Internacao.IsCesarea = null;
                    sis_Internacao.IsCompNeoNatal = null;
                    sis_Internacao.IsCompPuerperio = null;
                    sis_Internacao.IsDeleted = false;
                    sis_Internacao.IsEletiva = null;
                    sis_Internacao.IsGestacao = null;
                    sis_Internacao.IsInternacaoObstetrica = null;
                    sis_Internacao.IsNormal = null;
                    sis_Internacao.IsObitoNeoNatal = null;
                    sis_Internacao.IsSistema = false;
                    sis_Internacao.IsTransMat = null;
                    sis_Internacao.JustificativaSUS20 = null;
                    sis_Internacao.JustificativaSUS21 = null;
                    sis_Internacao.JustificativaSUS22 = null;
                    sis_Internacao.LastModificationTime = null;
                    sis_Internacao.LastModifierUserId = null;
                    sis_Internacao.NumDeclNascVivos1 = null;
                    sis_Internacao.NumDeclNascVivos2 = null;
                    sis_Internacao.NumDeclNascVivos3 = null;
                    sis_Internacao.NumDeclNascVivos4 = null;
                    sis_Internacao.NumDeclNascVivos5 = null;
                    sis_Internacao.NumObito = null;
                    sis_Internacao.OrgEmisResponsa = null;
                    sis_Internacao.PaisResponsa = null;
                    sis_Internacao.QtdeAlta = null;
                    sis_Internacao.QtdeNascMortos = null;
                    sis_Internacao.QtdeNascVivosPrematuro = null;
                    sis_Internacao.QtdeNascVivosTermo = null;
                    sis_Internacao.QtdeObitoNeonatalPrecoce = null;
                    sis_Internacao.QtdeObitoNeonatalTardio = null;
                    sis_Internacao.QtdeTransf = null;
                    sis_Internacao.QualAlergiaSzn = null;
                    sis_Internacao.QuantFralda = null;
                    sis_Internacao.Responsavel = null;
                    sis_Internacao.SeObitoMulher = null;
                    sis_Internacao.SisPreNatal = null;
                    sis_Internacao.StatusPront = null;
                    sis_Internacao.TemAcompanhante = null;
                    sis_Internacao.TemCafe = null;
                    sis_Internacao.TemFralda = null;
                    sis_Internacao.TemPernoite = null;
                    sis_Internacao.TemRefeicao = null;
                    sis_Internacao.TemRefeicaoAcompanhante = null;
                    sis_Internacao.TvTelefone = null;
                    sis_Internacao.TenantId = AbpSession.TenantId;
                    //var db = new SWMANAGERDbContext();

                    //var query = db.Database.SqlQuery<string>("SELECT CAST(IMPORTAID AS VARCHAR(18)) FROM ATEATENDIMENTO WHERE ID=" + internacao.Id).FirstOrDefault();

                    //var idAtendimento = 0;
                    //db.Dispose();

                    //if (int.TryParse(query, out idAtendimento))
                    //{
                    //    sis_Internacao.IDInternacao = idAtendimento;
                    //}

                    if (sis_Internacao.Id == 0)
                    {
                        _sis_InternacaoRepository.Insert(sis_Internacao);
                    }
                    else
                    {
                        _sis_InternacaoRepository.Update(sis_Internacao);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
            return null;
        }

        private void CarregarSisPessoa(long pessoaId)
        {
            try
            {
                var pessoa = _sisPessoaRepository.GetAll()
                    .Where(m => m.Id == pessoaId)
                    .Include(m => m.Naturalidade)
                    .Include(m => m.TipoPessoa)
                    .Include(m => m.Profissao)
                    .Include(m => m.TipoLogradouro)
                    .Include(m => m.Escolaridade)
                    .Include(m => m.Religiao)
                    .FirstOrDefault();

                if (pessoa.Escolaridade == null)
                {
                    var escorlaridade = _EscolaridadeRepository.GetAll()
                        .Where(e => e.Id == pessoa.EscolaridadeId)
                        .FirstOrDefault();
                    pessoa.Escolaridade = escorlaridade;
                }

                if (pessoa.Religiao == null)
                {
                    var religiao = _ReligiaoRepository.GetAll()
                        .Where(r => r.Id == pessoa.ReligiaoId).FirstOrDefault();
                    pessoa.Religiao = religiao;
                }

                Sis_Pessoa sis_Pessoa = null;

                if (pessoa != null)
                {
                    sis_Pessoa = _sis_PessoaRepository.GetAll()
                        .Where(w => w.Id == pessoaId)
                        .FirstOrDefault();

                    if (sis_Pessoa == null)
                    {
                        sis_Pessoa = new Sis_Pessoa();
                    }

                    sis_Pessoa.Agencia1 = null;
                    sis_Pessoa.Agencia2 = null;
                    sis_Pessoa.CEP = null;
                    sis_Pessoa.CGC = null;
                    sis_Pessoa.Codigo = pessoa.Codigo;
                    sis_Pessoa.CodPessoa = null; //pessoa.Codigo;
                    sis_Pessoa.Complemento = null;
                    sis_Pessoa.ContaCorrente1 = null;
                    sis_Pessoa.ContaCorrente2 = null;
                    sis_Pessoa.ContaPadrao = null;
                    sis_Pessoa.CPF = pessoa.Cpf;
                    //sis_Pessoa.CreationTime = null;
                    sis_Pessoa.CreatorUserId = null;
                    sis_Pessoa.DataExclusao = null;
                    sis_Pessoa.DataInclusao = null;
                    sis_Pessoa.DataUltimaAlteracao = null;
                    sis_Pessoa.DataUltimoLancamento = null;
                    sis_Pessoa.DeleterUserId = null;
                    sis_Pessoa.DeletionTime = null;
                    sis_Pessoa.Desativado = null;
                    sis_Pessoa.Descricao = pessoa.NomeCompleto;
                    sis_Pessoa.EmissaoIdentidade = null;
                    sis_Pessoa.Endereco = null;
                    sis_Pessoa.EstadoCivil = null;
                    sis_Pessoa.Foto = null;
                    sis_Pessoa.Hidden = null;
                    sis_Pessoa.HomePage = null;
                    //sis_Pessoa.Id = null;
                    sis_Pessoa.IDBairro = null;
                    sis_Pessoa.IDBanco1 = null;
                    sis_Pessoa.IDBanco2 = null;
                    sis_Pessoa.IDCentroCustoLocal = null;
                    sis_Pessoa.IDCidade = null;
                    sis_Pessoa.IDCNAE = null;
                    sis_Pessoa.IDCobranca = null;
                    sis_Pessoa.IDContaTesouraria = null;
                    sis_Pessoa.IDDocumentoTipo = null;
                    sis_Pessoa.Identidade = null;
                    sis_Pessoa.IDEstado = null;
                    sis_Pessoa.IDExterno = null;
                    sis_Pessoa.IDFilialSin = null;
                    sis_Pessoa.IDImportado = null;
                    sis_Pessoa.IDInstrucao = null;
                    sis_Pessoa.IDMeioPagamento = null;
                    //if (pessoa.NacionalidadeId.HasValue)
                    //{
                    //    sis_Pessoa.IDNaturalidade = pessoa.Naturalidade.ImportaId; //(int)pessoa.NaturalidadeId;
                    //}
                    sis_Pessoa.IDNFDescricao = null;
                    sis_Pessoa.IDPessoa = pessoa.ImportaId; //null;//antigo
                    sis_Pessoa.IDSW = (int)pessoa.Id;//novo
                    sis_Pessoa.IDPessoaTipo = pessoa.TipoPessoa.ImportaId; //(int)pessoa.TipoPessoaId;
                    //if (pessoa.ProfissaoId.HasValue)
                    //{
                    //    sis_Pessoa.IDProfissao = pessoa.Profissao.ImportaId; //(int)pessoa.ProfissaoId;
                    //}
                    //if (pessoa.TipoLogradouroId.HasValue)
                    //{
                    //    sis_Pessoa.IDTipoLogradouro = pessoa.TipoLogradouro.ImportaId; //(int)pessoa.TipoLogradouroId;
                    //}
                    sis_Pessoa.IDUsuarioAlteracao = null;
                    sis_Pessoa.IDUsuarioExclusao = null;
                    sis_Pessoa.IDUsuarioInclusao = null;
                    sis_Pessoa.InscricaoEstadual = pessoa.InscricaoEstadual;
                    sis_Pessoa.InscricaoMunicipal = pessoa.InscricaoMunicipal;
                    sis_Pessoa.IsAgendaTel = null;
                    sis_Pessoa.IsAlterado = null;
                    sis_Pessoa.IsDeleted = false;
                    sis_Pessoa.IsFuncionario = null;
                    sis_Pessoa.IsMalaDireta = null;
                    sis_Pessoa.IsRecolheISS = null;
                    sis_Pessoa.IsSincronizado = null;
                    sis_Pessoa.IsSistema = false;
                    sis_Pessoa.Juridico = null;
                    sis_Pessoa.LastModificationTime = null;
                    sis_Pessoa.LastModifierUserId = null;
                    sis_Pessoa.Nacionalidade = null;
                    sis_Pessoa.Nascimento = pessoa.Nascimento;
                    sis_Pessoa.Nominal = null;
                    sis_Pessoa.Numero = null;
                    sis_Pessoa.NumeroLancamentos = null;
                    sis_Pessoa.ObsPessoa = null;
                    sis_Pessoa.OrgaoEmissor = null;
                    sis_Pessoa.Pais = null;
                    sis_Pessoa.Pessoa = pessoa.NomeCompleto;
                    sis_Pessoa.RazaoSocial = pessoa.RazaoSocial;
                    sis_Pessoa.SaldoAtual = null;
                    sis_Pessoa.Sexo = pessoa.SexoId == 1 ? "M" : "F";
                    sis_Pessoa.System = null;
                    sis_Pessoa.Titular1 = null;
                    sis_Pessoa.Titular2 = null;
                    sis_Pessoa.TotalPrevisto = null;
                    sis_Pessoa.TotalQuitado = null;
                    sis_Pessoa.TenantId = AbpSession.TenantId;

                    //var db = new SWMANAGERDbContext();

                    //var query = db.Database.SqlQuery<string>("SELECT CAST(IMPORTAID AS VARCHAR(18)) FROM SISPESSOA WHERE ID=" + pessoa.Id).FirstOrDefault();

                    //var idPessoa = 0;

                    //db.Dispose();

                    //if (int.TryParse(query, out idPessoa))
                    //{
                    //    sis_Pessoa.IDPessoa = idPessoa;
                    //}

                    long sisPessoa = 0;

                    if (sis_Pessoa.Id == 0)
                    {
                        sisPessoa = _sis_PessoaRepository.InsertAndGetId(sis_Pessoa);
                    }
                    else
                    {
                        _sis_PessoaRepository.Update(sis_Pessoa);
                        pessoaId = sis_Pessoa.Id;

                    }

                    CarregarSisPaciente(pessoa, pessoa.Id);

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
            //return null;
        }

        private Sis_Paciente CarregarSisPaciente(SisPessoa pessoa, long sis_pessoaId)
        {
            try
            {
                Sis_Paciente sis_Paciente = null;

                if (pessoa != null)
                {
                    sis_Paciente = _sis_PacienteRepository.GetAll()
                        .Where(w => w.IDPaciente == sis_pessoaId)
                        .FirstOrDefault();

                    if (sis_Paciente == null)
                    {
                        sis_Paciente = new Sis_Paciente();
                    }
                    sis_Paciente.Categoria = null;
                    sis_Paciente.CNS = null;
                    sis_Paciente.Codigo = pessoa.Codigo;
                    sis_Paciente.CodPaciente = null; // paciente.Codigo;
                    //sis_Paciente.CreatorUserId = null;
                    sis_Paciente.DataUltimaMalaDir = null;
                    sis_Paciente.DeleterUserId = null;
                    sis_Paciente.DeletionTime = null;
                    sis_Paciente.Descricao = pessoa.NomeCompleto;
                    if (pessoa.EscolaridadeId.HasValue)
                    {
                        sis_Paciente.Escolaridade = pessoa.Escolaridade.ImportaId; //(int)pessoa.EscolaridadeId;
                    }
                    sis_Paciente.GrauDependente = null;
                    //sis_Paciente.Id = null;
                    sis_Paciente.IDEmpresaPac = null;
                    sis_Paciente.IDEtnia = null;
                    sis_Paciente.IDExterno = null;
                    sis_Paciente.IDPaciente = pessoa.ImportaId; //null;
                    sis_Paciente.IDSW = (int)sis_pessoaId;
                    if (pessoa.ReligiaoId.HasValue)
                    {
                        sis_Paciente.IDReligiao = pessoa.Religiao.ImportaId; //(int)pessoa.ReligiaoId;
                    }
                    sis_Paciente.IsCartao = null;
                    sis_Paciente.IsDeleted = false;
                    sis_Paciente.IsRecebeEmail = null;
                    sis_Paciente.IsSistema = false;
                    sis_Paciente.IsSUS = null;
                    sis_Paciente.JustificativaNumDeclNascVivo = null;
                    sis_Paciente.LastModificationTime = null;
                    sis_Paciente.LastModifierUserId = null;
                    sis_Paciente.Mae = null;
                    sis_Paciente.Matricula = null;
                    sis_Paciente.NumDeclNascVivo = null;
                    sis_Paciente.Observacao = null;
                    sis_Paciente.Pai = pessoa.NomePai;
                    sis_Paciente.RacaCor = null;
                    sis_Paciente.SenhaAgendaWeb = null;
                    sis_Paciente.SenhaWeb = null;
                    sis_Paciente.UsuarioAgendaWeb = null;
                    sis_Paciente.UsuarioWeb = null;
                    sis_Paciente.ValorEscala = null;
                    sis_Paciente.TenantId = AbpSession.TenantId;

                    //var db = new SWMANAGERDbContext();

                    //var query = db.Database.SqlQuery<string>("SELECT CAST(IMPORTAID AS VARCHAR(18)) FROM SISPESSOA WHERE ID=" + pessoa.Id).FirstOrDefault();

                    //var idPaciente = 0;
                    //db.Dispose();

                    //if (int.TryParse(query, out idPaciente))
                    //{
                    //    sis_Paciente.IDPaciente = idPaciente;
                    //}

                    if (sis_Paciente.Id == 0)
                    {
                        _sis_PacienteRepository.Insert(sis_Paciente);
                    }
                    else
                    {
                        _sis_PacienteRepository.Update(sis_Paciente);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
            return null;
        }

        private Sis_ContaMedica CarregarSisContaMedica(Atendimento atendimento, long sis_ContaMedicaId)
        {
            try
            {
                var fatconta = _faturamentoContaRepository.GetAll()
                        .Include(m => m.Empresa)
                        .Include(m => m.UnidadeOrganizacional)
                        // .Include(m => m.TipoLeito)
                        .Include(m => m.TipoAcomodacao)
                        .Where(w => w.AtendimentoId == atendimento.Id)
                        .FirstOrDefault();

                Sis_ContaMedica sis_ContaMedica = null;

                if (atendimento != null && fatconta != null)
                {
                    sis_ContaMedica = _sis_ContaMedicaRepository.GetAll()
                        .Where(w => w.IDContaMedica == sis_ContaMedicaId)
                        .FirstOrDefault();

                    if (sis_ContaMedica == null)
                    {
                        sis_ContaMedica = new Sis_ContaMedica();
                    }

                    sis_ContaMedica.CodDependente = fatconta.CodDependente;
                    sis_ContaMedica.Codigo = fatconta.Codigo;
                    //sis_ContaMedica.CreationTime = null;
                    sis_ContaMedica.CreatorUserId = fatconta.CreatorUserId;
                    sis_ContaMedica.DataAlteracao = //null;
                    sis_ContaMedica.DataAutorizacao = atendimento.DataAutorizacao; //null;
                    sis_ContaMedica.DataEntrBolAnest = null;
                    sis_ContaMedica.DataEntrCDFilme = null;
                    sis_ContaMedica.DataEntrDescCir = fatconta.DataEntrDescCir;
                    sis_ContaMedica.DataEntrFolhaSala = fatconta.DataEntrFolhaSala;
                    sis_ContaMedica.DataFim = fatconta.DataFim;
                    sis_ContaMedica.DataInicio = fatconta.DataInicio;
                    sis_ContaMedica.DataUltimaConferencia = null;
                    sis_ContaMedica.DataUsuarioResponsavel = null;
                    sis_ContaMedica.DataValidadeSenha = atendimento.ValidadeSenha;
                    sis_ContaMedica.DeleterUserId = null;
                    sis_ContaMedica.DeletionTime = null;
                    sis_ContaMedica.Descricao = null;
                    if (atendimento.DiasAutorizacao.HasValue)
                    {
                        sis_ContaMedica.DiasAutorizados = (int)atendimento.DiasAutorizacao;
                    }
                    sis_ContaMedica.DiaSerie1 = fatconta.DiaSerie1;
                    sis_ContaMedica.DiaSerie10 = fatconta.DiaSerie10;
                    sis_ContaMedica.DiaSerie2 = fatconta.DiaSerie2;
                    sis_ContaMedica.DiaSerie3 = fatconta.DiaSerie3;
                    sis_ContaMedica.DiaSerie4 = fatconta.DiaSerie3;
                    sis_ContaMedica.DiaSerie5 = fatconta.DiaSerie4;
                    sis_ContaMedica.DiaSerie6 = fatconta.DiaSerie5;
                    sis_ContaMedica.DiaSerie7 = fatconta.DiaSerie7;
                    sis_ContaMedica.DiaSerie8 = fatconta.DiaSerie8;
                    sis_ContaMedica.DiaSerie9 = fatconta.DiaSerie9;
                    sis_ContaMedica.DtPagamento = fatconta.DataPagamento;
                    sis_ContaMedica.GuiaOperadora = fatconta.GuiaOperadora;
                    sis_ContaMedica.GuiaPrincipal = fatconta.GuiaPrincipal;
                    //sis_ContaMedica.Id = null;
                    if (atendimento.MotivoAltaId.HasValue)
                    {
                        sis_ContaMedica.IDAlta = atendimento.MotivoAlta.ImportaId; //(int)atendimento.MotivoAltaId;
                    }
                    sis_ContaMedica.IDAtendimento = atendimento.ImportaId; // (int)atendimento.Id;
                    //sis_ContaMedica.IDContaMedica = null;
                    sis_ContaMedica.IDSW = (int)fatconta.Id;
                    sis_ContaMedica.IDConvenio = atendimento.Convenio.ImportaId; //(int)atendimento.ConvenioId;

                    sis_ContaMedica.IDEmpresaPac = null; // fatconta.Empresa.ImportaId; //(int)fatconta.EmpresaId;
                    sis_ContaMedica.IdentAcompanhante = fatconta.IdentAcompanhante;
                    sis_ContaMedica.IDFilialSin = fatconta.UnidadeOrganizacionalId.HasValue ? fatconta.UnidadeOrganizacional.ImportaId : atendimento.UnidadeOrganizacional.ImportaId; //(int)fatconta.UnidadeOrganizacionalId;
                    sis_ContaMedica.IDFormatoMatricula = null;
                    sis_ContaMedica.IDGuia = atendimento.FatGuia.ImportaId; //(int)atendimento.FatGuiaId;
                    sis_ContaMedica.IDImportado = null;
                    //if (fatconta.TipoLeitoId.HasValue)
                    //{
                    //    sis_ContaMedica.IDLeitoTipo = fatconta.TipoLeito.ImportaId; //(int)atendimento.LeitoId;
                    //}
                    if (fatconta.TipoAcomodacaoId.HasValue)
                    {
                        var tipoAcomodacao = Task.Run(() => _tipoAcomodacaoRepository.FirstOrDefault(fatconta.TipoAcomodacaoId.Value)).Result;
                        if (tipoAcomodacao == null)
                        {
                            sis_ContaMedica.IDLeitoTipo = atendimento.TipoAcomodacao.ImportaId;
                        }
                        else
                        {
                            sis_ContaMedica.IDLeitoTipo = tipoAcomodacao.ImportaId; //(int)atendimento.LeitoId;
                        }
                    }
                    else if (atendimento.IsInternacao)
                    {
                        sis_ContaMedica.IDLeitoTipo = atendimento.TipoAcomodacao.ImportaId;
                    }
                    sis_ContaMedica.IDMedico = atendimento.Medico.ImportaId; // (int)atendimento.MedicoId;
                    sis_ContaMedica.IDPendenciaMotivo = null;
                    sis_ContaMedica.IDPlano = atendimento.Plano.ImportaId; //(int)atendimento.PlanoId;
                    sis_ContaMedica.IDUsuarioAlteracao = null;
                    //if (fatconta.UsuarioConferenciaId.HasValue)
                    //{
                    //    sis_ContaMedica.IDUsuarioConferencia = (int)fatconta.UsuarioConferenciaId;
                    //}
                    sis_ContaMedica.IDUsuarioResponsavel = null;
                    sis_ContaMedica.IndicacaoClinica = null;
                    sis_ContaMedica.IsAlterado = null;
                    sis_ContaMedica.IsAutorizado = null;
                    sis_ContaMedica.IsAutorizador = fatconta.IsAutorizador;
                    sis_ContaMedica.IsComplementar = null;
                    sis_ContaMedica.IsDeleted = false;
                    sis_ContaMedica.IsImprimeGuia = null;
                    sis_ContaMedica.IsSemAutorizacao = null;
                    sis_ContaMedica.IsSincronizado = null;
                    sis_ContaMedica.IsSistema = false;
                    sis_ContaMedica.LastModificationTime = null;
                    sis_ContaMedica.LastModifierUserId = null;
                    sis_ContaMedica.Matricula = atendimento.Matricula;
                    sis_ContaMedica.NumeroGuia = atendimento.GuiaNumero;
                    sis_ContaMedica.DtPagamento = atendimento.DataUltimoPagamento;
                    sis_ContaMedica.NumeroSeq = null;
                    sis_ContaMedica.Observacao = null;//mudar tipo de dados
                    sis_ContaMedica.Ordem = null;
                    sis_ContaMedica.SenhaAutorizacao = atendimento.Senha;
                    sis_ContaMedica.StatusEntrega = null; //mudar tipo de dados
                    if (atendimento.AtendimentoTipoId.HasValue)
                    {
                        sis_ContaMedica.TipoAtendimento = atendimento.AtendimentoTipoId.Value.ToString("00"); //null;//mudar tipo de dados
                    }
                    else if (fatconta.TipoAtendimento > 0)
                    {
                        sis_ContaMedica.TipoAtendimento = fatconta.TipoAtendimento.ToString("00"); //null;//mudar tipo de dados
                    }
                    sis_ContaMedica.Titular = atendimento.Titular;
                    sis_ContaMedica.TrilhaCartao = null;
                    sis_ContaMedica.ValCarteira = atendimento.ValidadeCarteira;
                    sis_ContaMedica.DataValidadeSenha = atendimento.ValidadeSenha;
                    sis_ContaMedica.TenantId = AbpSession.TenantId;

                    if (sis_ContaMedica.Id == 0)
                    {
                        _sis_ContaMedicaRepository.Insert(sis_ContaMedica);
                    }
                    else
                    {
                        _sis_ContaMedicaRepository.Update(sis_ContaMedica);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
            return null;
        }

        public async Task MigrarProRegExame(long exameId)
        {
            await CarregarProRegExame(exameId);
        }

        private async Task CarregarProRegExame(long exameId)
        {
            var resultado = await _resultadoRepository
                .GetAll()
                .Include(i => i.Atendimento)
                .Include(i => i.Atendimento.Leito)
                .Include(i => i.MedicoSolicitante)
                .Include(i => i.MedicoSolicitante.SisPessoa)
                .Include(i => i.Atendimento.Origem)
                .Include(i => i.LeitoAtual)
                .Include(i => i.LocalAtual)
                .Include(i => i.AutorizacaoProcedimento)
                .Include(i => i.CentroCusto)
                .Include(i => i.Convenio)
                .Include(i => i.FaturamentoConta)
                .Include(i => i.LocalUtilizacao)
                .Include(i => i.Responsavel)
                .Include(i => i.ResultadoStatus)
                .Include(i => i.Tecnico)
                .Include(i => i.TecnicoColeta)
                .Include(i => i.Terceirizado)
                .Include(i => i.TipoAcomodacao)
                .Include(i => i.Turno)
                .Where(w => w.Id == exameId)
                .FirstOrDefaultAsync();

            var itens = await _resultadoExameItemRepository
                .GetAll()
                .Include(i => i.ExameStatus)
                .Include(i => i.Material)
                .Where(w => w.ResultadoId == exameId)
                .ToListAsync();

            var proRegExame = new Pro_ReqExameMov();
            proRegExame.DataRequisicao = resultado.DataColeta.Value;
            proRegExame.IdAtendimento = resultado.Atendimento?.ImportaId ?? 0;
            proRegExame.Hidden = false;
            proRegExame.IdRequisicaoMov = resultado.ImportaId ?? 0;
            proRegExame.IdCcRequisitado = resultado.CentroCusto?.ImportaId ?? 0;
            proRegExame.IdCentroCusto = resultado.CentroCusto?.ImportaId ?? 0;
            proRegExame.IdLocalUtilizacao = resultado.LocalUtilizacao?.ImportaLocalUtilizacaoId ?? 0;
            proRegExame.IdMedico = resultado.MedicoSolicitante?.ImportaId ?? 0;
            proRegExame.IdReqExameStatus = resultado.ResultadoStatus?.ImportaId ?? 5;
            proRegExame.IdUsuario = 0;
            proRegExame.IsEncerrada = resultado.DataEntregaExame.HasValue ? true : false;
            proRegExame.IsSemanal = resultado.IsRotina;
            proRegExame.TenantId = AbpSession.TenantId;
            proRegExame.IDSW = resultado.Id;
            proRegExame.TipoRequisicao = Convert.ToInt32(resultado.IsRotina);
            proRegExame.IdTerceirizado = resultado.Terceirizado?.ImportaId ?? 0;
            proRegExame.NumeroDocumento = resultado.Nic.ToString();
            proRegExame.Obs = resultado.ObsEntrega;
            proRegExame.DataAutorizacao = resultado.AutorizacaoProcedimento?.DataSolicitacao ?? resultado.DataColeta.Value;

            //campos nullable
            //proRegExame.IdClinica = resultado.Atendimento.Empresa.ImportaId.Value;

            var proRegExameItens = new List<Pro_ReqExameMovItem>();

            foreach (var item in itens)
            {
                var proRegExameItem = new Pro_ReqExameMovItem();

                proRegExameItem.QtdeRequisitada = item.Quantidade ?? 0;
                proRegExameItem.DataAtualizacao = item.DataAlteradoExame ?? DateTime.Now;
                proRegExameItem.IsEncerrada = item.DataConferidoExame.HasValue ? true : false;
                proRegExameItem.IsAtendida = false;
                proRegExameItem.IdUsuario = 0;
                proRegExameItem.IdItemRequisitado = Convert.ToInt32(item.FaturamentoItem?.ImportaId ?? 0);
                proRegExameItem.IdItem = Convert.ToInt32(item.FaturamentoItem?.ImportaId ?? 0);
                proRegExameItem.TenantId = AbpSession.TenantId;
                proRegExameItem.IDSW = item.Id;

                //proRegExameItem.DataAutorizacao = item.AutorizacaoProcedimentoItem?.DataAutorizacao ?? resultado.DataColeta.Value;
                //proRegExameItem.IdAutorizacao = resultado.AutorizacaoProcedimento?.ImportaId ?? 0;
                //proRegExameItem.IdFatKit = Convert.ToInt32(item.FaturamentoContaItem?.FaturamentoContaKitId ?? 0);
                //proRegExameItem.IdMaterial = Convert.ToInt32(item.Material?.ImportaId ?? 0);
                //proRegExameItem.IdRequisicaoMov = (int)proRegExame.Id;
                //proRegExameItem.NomeAutorizacao = resultado.AutorizacaoProcedimento.
                //proRegExameItem.ObsAutorizacao = item.AutorizacaoProcedimentoItem?.Observacao;
                //proRegExameItem.ObsRequisicao = resultado.AutorizacaoProcedimento?.Observacao;
                //proRegExameItem.SenhaAutorizacao = item.AutorizacaoProcedimentoItem?.Senha;

                proRegExameItens.Add(proRegExameItem);
            }
            proRegExame.Itens = proRegExameItens;


            await _pro_ReqExameMovRepository.InsertAsync(proRegExame);
        }
    }
}
