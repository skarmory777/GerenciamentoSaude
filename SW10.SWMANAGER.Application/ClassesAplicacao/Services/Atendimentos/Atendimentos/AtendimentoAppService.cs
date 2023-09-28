using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using System.Threading.Tasks;

    using Abp.Application.Services.Dto;
    using Abp.Auditing;
    using Abp.Collections.Extensions;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;

    using Cadastros.Convenios;

    using ClassesAplicacao.Cadastros.Convenios;

    using Dapper;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;
    using RestSharp;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Diagnosticos;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.TiposAcompanhantes;
    using SW10.SWMANAGER.ClassesAplicacao.AtendimentosLeitosMov;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.TiposAtendimento;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
    using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Enumeradores;
    using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
    using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Enumeradores;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Exporting;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Enumeradores;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas;
    using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
    using SW10.SWMANAGER.ClassesAplicacao.Services.VisualASA;
    using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
    using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
    using SW10.SWMANAGER.Dto;
    using SW10.SWMANAGER.EntityFramework;
    using SW10.SWMANAGER.Helpers;

    public class AtendimentoAppService : SWMANAGERAppServiceBase, IAtendimentoAppService
    {
        #region Injecao e Construtor

        #endregion injecao e construtor.

        [UnitOfWork] // Atualizado (pablo 08/08/2017)
        public async Task<DefaultReturn<AtendimentoDto>> CriarOuEditar(AtendimentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<AtendimentoDto>
            {
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };

            try
            {
                var atendimento = AtendimentoDto.Mapear(input);
                long novaContaId = 0;


                using (var convenioAppService = IocManager.Instance.ResolveAsDisposable<IConvenioAppService>())
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var senhaMovimentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var fatContaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                {
                    var numeroGuiaDto = await convenioAppService.Object.ObterNumeroGuia(
                                            atendimento.ConvenioId ?? 0,
                                            atendimento.EmpresaId ?? 0,
                                            atendimento.FatGuiaId ?? 0).ConfigureAwait(false);

                    if (numeroGuiaDto != null)
                    {
                        if (numeroGuiaDto.IsFinal)
                        {
                            _retornoPadrao.Errors.Add(
                                new ErroDto
                                {
                                    Descricao = "O limite do número de guia foi atingido para esse convênio."
                                });

                            return _retornoPadrao;
                        }

                        if (numeroGuiaDto.IsAvisar)
                        {
                            _retornoPadrao.Warnings.Add(
                                new ErroDto
                                {
                                    CodigoErro = "ATND0001",
                                    Parametros = new List<object> { numeroGuiaDto.NumeroFinal }
                                });
                        }
                    }
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        // =========CRIAR MÉTODO PARA TRATAMENTO DE PRE-ATENDIMENTO===============
                        if (input.NomePreAtendimento != null)
                        {
                            // pesquisa paciente

                            PacienteDto paciente = null;

                            if (!string.IsNullOrEmpty(input.CpfPreAtendimento))
                            {
                                paciente = await pacienteAppService.Object.ObterPorCpf(input.CpfPreAtendimento)
                                               .ConfigureAwait(false);
                            }

                            if (paciente == null)
                            {
                                // CriarOuEditarPaciente paciente2 = new CriarOuEditarPaciente();
                                paciente = new PacienteDto
                                {
                                    NomeCompleto = input.NomePreAtendimento,
                                    Telefone1 = input.TelefonePreAtendimento,
                                    Nascimento = input.DataNascPreAtendimento,
                                    Cpf = input.CpfPreAtendimento,
                                    Rg = input.IdentPreAtendimento
                                };

                                // salva paciente
                                atendimento.PacienteId = await pacienteAppService.Object.CriarOuEditarOriginal(paciente)
                                                             .ConfigureAwait(false);
                            }
                            else
                            {
                                atendimento.PacienteId = paciente.Id;
                            }

                            atendimento.IsPreatendimento = true;
                            atendimento.AtendimentoTipoId = input.AtendimentoTipoId;
                            atendimento.DataRegistro = input.DataRegistro;
                            atendimento.DataPreatendimento = input.DataRegistro;
                            atendimento.Observacao = input.Observacao;


                            atendimento.ConvenioId = input.ConvenioId;

                            atendimento.PlanoId = input.PlanoId;
                            atendimento.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                            atendimento.AtendimentoOrigemId = input.AtendimentoOrigemId;

                        }
                        // =======FIM TRATAMENTO P/ PRE-ATENDIMENTO===============

                        // ========Cria método depois geração do codigo caso ñ seja passado============
                        if (atendimento.Codigo.IsNullOrWhiteSpace())
                        {
                            // Buscando 'UltimoId'

                            var nomeTabela = atendimento.IsAmbulatorioEmergencia ? "Emergencia" : "Internacao";

                            atendimento.Codigo = await ultimoIdAppService.Object.ObterProximoCodigo(nomeTabela)
                                                     .ConfigureAwait(false);

                            if (string.IsNullOrEmpty(atendimento.GuiaNumero))
                            {
                                if (atendimento.ConvenioId != null)
                                {

                                    var convenio = convenioRepository.Object.GetAll()
                                        .FirstOrDefault(w => w.Id == atendimento.ConvenioId);

                                    if (convenio != null && convenio.IsPreencheGuiaAutomaticamente)
                                    {
                                        atendimento.GuiaNumero = atendimento.Codigo;
                                    }
                                    else
                                    {
                                        atendimento.GuiaNumero = numeroGuiaDto?.NumeroGuia;
                                    }
                                }
                            }
                        }

                        // ========fim geração do codigo caso ñ seja passado============


                        if (input.Id.Equals(0))
                        {
                            atendimento.AtendimentoStatusId = AtendimentoStatus.Aguardando;
                            atendimento.FaturamentoAtendimentoStatusId = FaturamentoAtendimentoStatus.Pendente;

                            atendimento.Id = await atendimentoRepository.Object.InsertAndGetIdAsync(atendimento).ConfigureAwait(false);

                            // Nova conta medica
                            // Todo atendimento salvo deve gerar automaticamente uma nova conta associada a este atendimento

                            // Todos os campos de FatConta abaixo, devem ser preenchidos conforme possível
                            // obtendo dados do Atendimento que gera esta nova conta

                            var tamanho = atendimento.Matricula != null ? atendimento.Matricula.Length : 0;

                            if (tamanho > 20)
                            {
                                tamanho = 20;
                            }

                            var novaConta = new FaturamentoConta
                            {
                                AtendimentoId = atendimento.Id,
                                EmpresaId = atendimento.EmpresaId,
                                ConvenioId = atendimento.ConvenioId,
                                PlanoId = atendimento.PlanoId,
                                PacienteId = atendimento.PacienteId,
                                MedicoId = atendimento.MedicoId,
                                Matricula = atendimento.Matricula?.Substring(tamanho),
                                CodDependente = input.CodDependente,
                                NumeroGuia = atendimento.GuiaNumero,
                                Titular = atendimento.Titular,
                                FatGuiaId = atendimento.FatGuiaId,
                                UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId,
                                StatusId = 1,
                                DataInicio = DateTime.Now,
                                DataFim = null,
                                ValidadeCarteira = atendimento.ValidadeCarteira,
                                DataAutorizacao = atendimento.DataAutorizacao,
                                DiaSerie1 = null,
                                DiaSerie2 = null,
                                DiaSerie3 = null,
                                DiaSerie4 = null,
                                DiaSerie5 = null,
                                DiaSerie6 = null,
                                DiaSerie7 = null,
                                DiaSerie8 = null,
                                DiaSerie9 = null,
                                DiaSerie10 = null,
                                DataEntrFolhaSala = null,
                                DataEntrDescCir = null,
                                DataEntrBolAnest = null,
                                DataEntrCDFilme = null,
                                DataValidadeSenha = atendimento.ValidadeSenha,
                                GuiaOperadora = null,
                                GuiaPrincipal = null,
                                TipoAtendimento = atendimento.IsAmbulatorioEmergencia ? 1 : 2,
                                IsAutorizador = false,
                                Observacao = atendimento.Observacao,
                                SenhaAutorizacao = atendimento.Senha,
                                IdentAcompanhante = null,
                                DataPagamento = atendimento.DataUltimoPagamento
                            };

                            if (atendimento.IsInternacao)
                            {
                                novaConta.DataInicio = atendimento.DataRegistro;

                                var atendimentoLeitoMov = new AtendimentoLeitoMov
                                {
                                    AtendimentoId = atendimento.Id,
                                    LeitoId = atendimento.LeitoId,
                                    UserId = atendimento.CreatorUserId
                                };
                                atendimentoLeitoMov.DataInclusao =
                                    atendimentoLeitoMov.DataInicial = atendimento.DataRegistro;



                                await atendimentoLeitoMovRepository.Object.InsertAsync(atendimentoLeitoMov)
                                    .ConfigureAwait(false);

                                var leito = leitoRepository.Object.GetAll()
                                    .FirstOrDefault(w => w.Id == atendimento.LeitoId);

                                if (leito != null)
                                {
                                    leito.LeitoStatusId = 2;

                                    novaConta.TipoAcomodacaoId = leito.TipoAcomodacaoId;

                                    await leitoRepository.Object.UpdateAsync(leito).ConfigureAwait(false);
                                }
                            }

                            novaContaId = await fatContaRepository.Object.InsertAndGetIdAsync(novaConta).ConfigureAwait(false);

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();

                            await this.InserirItemConta(atendimento.Id, novaContaId).ConfigureAwait(false);

                            await this.InserirItensFaturamentoAgendamento(input.AgendamentoId, novaContaId, atendimento).ConfigureAwait(false);

                            // Fim - nova conta medica
                        }
                        else
                        {
                            var atendimentoAtual = atendimentoRepository.Object.GetAll().FirstOrDefault(w => w.Id == atendimento.Id);

                            if (atendimentoAtual != null)
                            {
                                atendimentoAtual.EmpresaId = input.EmpresaId;
                                atendimentoAtual.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                                atendimentoAtual.DataRegistro = input.DataRegistro;
                                atendimentoAtual.PacienteId = input.PacienteId;
                                atendimentoAtual.OrigemId = input.OrigemId;
                                atendimentoAtual.TipoAcomodacaoId = input.TipoAcomodacaoId;
                                atendimentoAtual.AtendimentoTipoId = input.AtendimentoTipoId;
                                atendimentoAtual.MedicoId = input.MedicoId;
                                atendimentoAtual.EspecialidadeId = input.EspecialidadeId;
                                atendimentoAtual.ServicoMedicoPrestadoId = input.ServicoMedicoPrestadoId;
                                atendimentoAtual.ConvenioId = input.ConvenioId;
                                atendimentoAtual.PlanoId = input.PlanoId;
                                atendimentoAtual.CaraterAtendimentoId = input.CaraterAtendimentoId;
                                atendimentoAtual.IndicacaoAcidenteId = input.IndicacaoAcidenteId;
                                atendimentoAtual.TipoAcompanhanteId = input.TipoAcompanhanteId;
                                atendimentoAtual.LeitoId = input.LeitoId;
                                atendimentoAtual.Titular = input.Titular;
                                atendimentoAtual.Matricula = input.Matricula;
                                atendimentoAtual.CodDependente = input.CodDependente;
                                atendimentoAtual.DataUltimoPagamento = input.DataUltimoPagamento;
                                atendimentoAtual.ValidadeCarteira = input.ValidadeCarteira;
                                atendimentoAtual.DataAutorizacao = input.DataAutorizacao;
                                atendimentoAtual.Senha = input.Senha;
                                atendimentoAtual.ValidadeSenha = input.ValidadeSenha;
                                atendimentoAtual.FatGuiaId = input.FatGuiaId;
                                atendimentoAtual.GuiaNumero = input.GuiaNumero;
                                atendimentoAtual.DiasAutorizacao = input.DiasAutorizacao;
                                atendimentoAtual.Responsavel = input.Responsavel;
                                atendimentoAtual.RgResponsavel = input.RgResponsavel;
                                atendimentoAtual.CpfResponsavel = input.CpfResponsavel;
                                atendimentoAtual.Parentesco = input.Parentesco;
                                atendimentoAtual.OrigemTitular = input.OrigemTitular;
                                atendimentoAtual.Observacao = input.Observacao;
                            }

                            await atendimentoRepository.Object.UpdateAsync(atendimentoAtual).ConfigureAwait(false);
                        }

                        var movimentacaoSenha = senhaMovimentacaoRepository.Object.GetAll().Include(i => i.Senha)
                            .Include(i => i.TipoLocalChamada)
                            .FirstOrDefault(w => w.Id == input.MovimentacaoSenhalId);


                        if (movimentacaoSenha != null)
                        {
                            movimentacaoSenha.Senha.AtendimentoId = atendimento.Id;
                            movimentacaoSenha.DataHoraFinal = DateTime.Now;

                            if (movimentacaoSenha.TipoLocalChamada != null)
                            {
                                // && movimentacaoSenha.TipoLocalChamada.TipoLocalChamadaProximoId != null)
                                var senhaMovimentacao = new SenhaMovimentacao
                                {
                                    SenhaId = movimentacaoSenha.Senha.Id,
                                    TipoLocalChamadaId =
                                                                    input.ProximoTipoLocalChamadaId,
                                    DataHora = DateTime.Now
                                };

                                // movimentacaoSenha.TipoLocalChamada.TipoLocalChamadaProximoId; 

                                await senhaMovimentacaoRepository.Object.InsertAsync(senhaMovimentacao)
                                    .ConfigureAwait(false);
                            }
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();

                        //var _visualAppService = this.iocResolver.Resolve<IVisualAppService>();

                        //_visualAppService.MigrarVisualASA(atendimento.Id);

                        _retornoPadrao.ReturnObject = new AtendimentoDto { Id = atendimento.Id };

                        return _retornoPadrao;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("AtendimentoAppService -> CriarOuEditar", ex);
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }
        }

        public async Task SetTomadaDecisao(SetAltaInput input)
        {
            try
            {
                if (!input.atendimentoId.HasValue)
                {
                    throw new Exception("InformarAtendimento");
                }

                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var atendimento = atendimentoRepository.Object.Get(input.atendimentoId.Value);
                    atendimento.DataTomadaDecisao = input.dataTomadaDecisao;
                    atendimento.UsuarioTomadaDecisao = this.AbpSession.UserId;

                    await atendimentoRepository.Object.UpdateAsync(atendimento).ConfigureAwait(false);

                    //await this.AlterarMedicoAtendimento(atendimento.Id).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(this.L("ErroAtribuirTomadaDecisao"), e);
            }
        }

        public async Task SetAlta(SetAltaInput input)
        {
            try
            {
                if (!input.atendimentoId.HasValue)
                {
                    throw new Exception("InformarAtendimento");
                }

                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoLeitoMovRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                {
                    var atendimento = atendimentoRepository.Object.Get(input.atendimentoId.Value);
                    atendimento.DataAlta = input.dataAlta;

                    atendimento.AtendimentoStatusId = AtendimentoStatus.Atendido;

                    if (input.motivoAltaId == 0)
                    {
                        input.motivoAltaId = null;
                    }

                    if (input.altaGrupoCidId == 0)
                    {
                        input.altaGrupoCidId = null;
                    }

                    atendimento.MotivoAltaId = input.motivoAltaId;
                    atendimento.AltaGrupoCIDId = input.altaGrupoCidId;
                    atendimento.NumeroObito = input.NumeroObito;

                    if (atendimento.LeitoId.HasValue)
                    {
                        var leito = leitoRepository.Object.Get(atendimento.LeitoId.Value);
                        leito.LeitoStatusId = 1; // status 1 -> VAGO (default Seed)
                        leitoRepository.Object.Update(leito);
                        atendimento.LeitoId = null;

                        // movimento de leito... inserindo a data final do movimento
                        var mov = atendimentoLeitoMovRepository.Object.GetAll().FirstOrDefault(
                            w => w.AtendimentoId == atendimento.Id && w.LeitoId == leito.Id && !w.DataFinal.HasValue);

                        if (mov != null)
                        {
                            mov.DataFinal = DateTime.Now;
                            await atendimentoLeitoMovRepository.Object.UpdateAsync(mov).ConfigureAwait(false);
                        }
                        else
                        {
                            mov = new AtendimentoLeitoMov
                            {
                                AtendimentoId = atendimento.Id,
                                LeitoId = leito.Id,
                                DataInclusao = DateTime.Now,
                                DataInicial = DateTime.Now,
                                DataFinal = DateTime.Now,
                                UserId = this.AbpSession.UserId.HasValue ? this.AbpSession.UserId.Value : 2
                            };
                            await atendimentoLeitoMovRepository.Object.InsertAsync(mov).ConfigureAwait(false);
                        }
                    }

                    await atendimentoRepository.Object.UpdateAsync(atendimento).ConfigureAwait(false);

                    //await this.AlterarMedicoAtendimento(atendimento.Id).ConfigureAwait(false);
                    await this.CriarPreAtendimento(AtendimentoDto.Mapear(atendimento)).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(this.L("ErroAtribuirAlta"), e);
            }
        }

        async Task CriarPreAtendimento(AtendimentoDto atendimento)
        {
            using (var tipoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MotivoAlta, long>>())
            using (var unidadeOrganizacionalRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
            {
                var tipoAlta = await tipoAltaRepository
                                   .Object
                                   .GetAll()
                                   .Include(i => i.MotivoAltaTipoAlta)
                                   .FirstOrDefaultAsync(w => w.Id == atendimento.MotivoAltaId).ConfigureAwait(false);

                if (tipoAlta != null && tipoAlta.MotivoAltaTipoAlta != null && tipoAlta.MotivoAltaTipoAlta.Codigo == "004")
                {

                    var preAtendimento = new AtendimentoDto();

                    var dataAtual = DateTime.Now;

                    preAtendimento.IsInternacao = true;
                    preAtendimento.IsPreatendimento = true;
                    preAtendimento.PacienteId = atendimento.PacienteId;
                    preAtendimento.DataRegistro = dataAtual;
                    preAtendimento.DataPreatendimento = dataAtual;
                    preAtendimento.ConvenioId = atendimento.ConvenioId;
                    preAtendimento.PlanoId = atendimento.PlanoId;
                    preAtendimento.MedicoId = atendimento.MedicoId;
                    preAtendimento.EspecialidadeId = atendimento.EspecialidadeId;
                    preAtendimento.AtendimentoOrigemId = atendimento.Id;

                    // preAtendimento.NomePreAtendimento = atendimento.Paciente.NomeCompleto;

                    var unidade = await unidadeOrganizacionalRepository.Object.GetAll().FirstOrDefaultAsync(w => w.IsInternacao)
                                      .ConfigureAwait(false);

                    if (unidade != null)
                    {
                        preAtendimento.UnidadeOrganizacionalId = unidade.Id;
                    }

                    var novoAtendimentoId = await this.CriarOuEditar(preAtendimento).ConfigureAwait(false);
                }
            }
        }

        public async Task SetAltaMedica(SetAltaInput input)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var atendimento = atendimentoRepository.Object.Get(input.atendimentoId.Value);
                    atendimento.DataAltaMedica = input.dataAltaMedica;
                    await atendimentoRepository.Object.UpdateAsync(atendimento).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(this.L("ErroAtribuirAlta"), e);
            }
        }

        public async Task SetDataPrevistaAlta(SetAltaInput input)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var atendimento = atendimentoRepository.Object.Get(input.atendimentoId.Value);
                    atendimento.DataPrevistaAlta = input.dataPrevisaoAlta;
                    await atendimentoRepository.Object.UpdateAsync(atendimento).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(this.L("ErroAtribuirAlta"), e);
            }
        }

        public async Task SetDataTomadaDecisao(SetAltaInput input)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var atendimento = atendimentoRepository.Object.Get(input.atendimentoId.Value);
                    atendimento.DataTomadaDecisao = input.dataPrevisaoAlta;
                    await atendimentoRepository.Object.UpdateAsync(atendimento).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(this.L("ErroAtribuirAlta"), e);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id, long? motivoCancelamentoId = null)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var atendimento = atendimentoRepository.Object.GetAll().FirstOrDefault(w => w.Id == id);

                    if (atendimento != null)
                    {
                        atendimento.AtendimentoMotivoCancelamentoId = motivoCancelamentoId ?? 1;

                        if (atendimento.LeitoId.HasValue)
                        {
                            var leito = leitoRepository.Object.Get(atendimento.LeitoId.Value);
                            leito.LeitoStatusId = 1; // status 1 -> VAGO (default Seed)
                            leitoRepository.Object.Update(leito);

                            var movimentoLeito = atendimentoLeitoMovRepository.Object.GetAll()
                                .FirstOrDefault(
                                  w => w.LeitoId == atendimento.LeitoId
                                       && w.AtendimentoId == atendimento.Id
                                       && w.DataFinal == null);

                            if (movimentoLeito != null)
                            {
                                movimentoLeito.DataFinal = DateTime.Now;
                            }

                            atendimento.LeitoId = null;
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork]
        public async Task Reativar(long id, long? leitoId)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var atendimento = await atendimentoRepository.Object
                                          .GetAll()
                                          .FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

                    if (atendimento != null)
                    {
                        atendimento.AtendimentoMotivoCancelamentoId = null;
                        if (leitoId.HasValue)
                        {
                            atendimento.LeitoId = leitoId;
                            var leito = leitoRepository.Object.Get(leitoId ?? 0);
                            leito.LeitoStatusId = 2; // status 2 -> OCUPADO (default Seed)
                            leitoRepository.Object.Update(leito);

                            var movimentoLeito = new AtendimentoLeitoMov
                            {
                                AtendimentoId = id,
                                DataInclusao = DateTime.Now,
                                DataInicial = DateTime.Now,
                                LeitoId = leitoId
                            };

                            atendimentoLeitoMovRepository.Object.Insert(movimentoLeito);
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<AtendimentoDto>> ListarTodos()
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).Include(m => m.Medico).Include(m => m.Medico.SisPessoa)
                        .Include(m => m.AtendimentoTipo).Include(m => m.Convenio).Include(m => m.Convenio.SisPessoa)
                        .Include(m => m.Empresa).Include(m => m.Especialidade).Include(m => m.Guia)
                        .Include(m => m.Leito).Include(m => m.MotivoAlta).Include(m => m.Nacionalidade)
                        .Include(m => m.Origem).Include(m => m.Plano).Include(m => m.ServicoMedicoPrestado)
                        .Include(m => m.UnidadeOrganizacional);


                    var contarAtendimentos = await query.CountAsync().ConfigureAwait(false);

                    return new PagedResultDto<AtendimentoDto>(contarAtendimentos, AtendimentoDto.Mapear(await query.ToListAsync().ConfigureAwait(false)).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<AtendimentoDto>> ListarParaInternacao()
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).Include(m => m.Medico).Include(m => m.Medico.SisPessoa)
                        .Include(m => m.AtendimentoTipo).Include(m => m.Convenio).Include(m => m.Convenio.SisPessoa)
                        .Include(m => m.Empresa).Include(m => m.Especialidade).Include(m => m.Guia)
                        .Include(m => m.Leito).Include(m => m.MotivoAlta).Include(m => m.Nacionalidade)
                        .Include(m => m.Origem).Include(m => m.Plano).Include(m => m.ServicoMedicoPrestado)
                        .Include(m => m.UnidadeOrganizacional).Where(a => a.IsInternacao);


                    var contarAtendimentos = await query.CountAsync().ConfigureAwait(false);

                    return new PagedResultDto<AtendimentoDto>(
                        contarAtendimentos,
                        AtendimentoDto.Mapear(await query.ToListAsync().ConfigureAwait(false)).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<AtendimentoIndexDto>> ListarFiltro(ListarAtendimentosInput input)
        {
            const string DefaultField = "ateAtendimento.Id";

            const string SelectClause = @"ateAtendimento.Id, 
                        unidadeOrganizacional.Descricao AS Unidade,
                        ateAtendimento.Codigo AS CodigoAtendimento,
                        ateAtendimento.DataRegistro,
                        AteAtendimento.DataAlta,
                        SisPessoaPaciente.NomeCompleto AS Paciente,
                        PessoaPaciente.CodigoPaciente AS CodigoPaciente,    
                        SisPessoaPaciente.Nascimento AS PacienteNascimento,
                        PessoaMedico.NomeCompleto AS Medico,
                        ateAtendimento.Matricula,
                        SisTipoAcomodacao.Descricao AS TipoLeito,
                        AteLeito.Descricao AS Leito,
                        AteClassificacaoAtendimento.Descricao AS Classificacao,
                        AteClassificacaoAtendimento.Cor AS CorClassificacao,
                        AteProtocoloAtendimento.Descricao AS Protocolo,
                        SisEmpresa.NomeFantasia AS Empresa,
                        SisPessoaConvenio.NomeFantasia AS Convenio,
                        AteSenha.Numero AS Senha,
                        AteSenhaMov.Id AS SenhaAtendimentoId,
                        ateAtendimento.AteAtendimentoStatusId,
                        AteAtendimentoStatus.Descricao AS Status,
                        AteAtendimentoStatus.CorFundo AS CorFundo,
                        AteAtendimentoStatus.CorTexto AS CorTexto,
                        AteAtendimento.IsControlaTev,
                        ateAtendimento.IsPendenteExames,
                        ateAtendimento.IsPendenteMedicacao,
                        ateAtendimento.IsPendenteProcedimento,
                        ateAtendimento.StatusAguardando,
                        ateAtendimento.StatusAtendido,
                        QtdLauAssSolicitacaoExame.Total As QtdLauAssSolicitacaoExame,
                        QtdLabAssSolicitacaoExame.Total As QtdLabAssSolicitacaoExame,
                        QtdLauMovimentoItem.Total As QtdLauMovimentoItem,
                        QtdLabResultadoExame.Total As QtdLabResultadoExame";

            var FromClause = $@"AteAtendimento AS ateAtendimento
                            LEFT JOIN SisUnidadeOrganizacional AS unidadeOrganizacional
                            ON ateAtendimento.SisUnidadeOrganizacionalId = unidadeOrganizacional.Id
                            LEFT JOIN SisPaciente AS PessoaPaciente ON PessoaPaciente.Id = ateAtendimento.SisPacienteId AND PessoaPaciente.IsDeleted = @deleted
                            LEFT JOIN SisPessoa SisPessoaPaciente ON PessoaPaciente.SisPessoaId = SisPessoaPaciente.Id AND SisPessoaPaciente.IsDeleted = @deleted
                            LEFT JOIN SisMedico AS PessoaMedico ON PessoaMedico.Id = ateAtendimento.SisMedicoId AND PessoaMedico.IsDeleted = @deleted
                            LEFT JOIN SisPessoa SisPessoaMedico ON SisPessoaMedico.Id = PessoaMedico.SisPessoaId AND SisPessoaMedico.IsDeleted = @deleted
                            LEFT JOIN AteLeito ON AteLeito.Id = ateAtendimento.AteLeitoId AND AteLeito.IsDeleted = @deleted
                            LEFT JOIN SisTipoAcomodacao ON SisTipoAcomodacao.Id = AteLeito.SisTipoAcomodacaoId AND SisTipoAcomodacao.IsDeleted = @deleted
                            LEFT JOIN AteClassificacaoAtendimento ON AteClassificacaoAtendimento.Id = ateAtendimento.ClassificacaoAtendimentoId AND AteClassificacaoAtendimento.IsDeleted = @deleted
                            LEFT JOIN AteProtocoloAtendimento ON AteProtocoloAtendimento.Id = ateAtendimento.ProtocoloAtendimentoId AND AteProtocoloAtendimento.IsDeleted = @deleted
                            LEFT JOIN AteAtendimentoStatus ON AteAtendimentoStatus.Id = ateAtendimento.AteAtendimentoStatusId AND AteAtendimentoStatus.IsDeleted = @deleted
                            LEFT JOIN SisEmpresa ON SisEmpresa.Id = ateAtendimento.SisEmpresaId AND SisEmpresa.IsDeleted = @deleted
                            LEFT JOIN AteSenha ON AteSenha.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN 
                            (SELECT max(AteSenhaMov.Id) as AteSenhaMovId, AteSenhaMov.SenhaId
                            FROM AteSenhaMov
                            GROUP BY(AteSenhaMov.SenhaId)) AS senhaMovMax ON senhaMovMax.SenhaId = AteSenha.Id
                            LEFT JOIN AteSenhaMov ON AteSenhaMov.Id = senhaMovMax.AteSenhaMovId AND AteSenhaMov.IsDeleted = @deleted
                            LEFT JOIN SisConvenio ON SisConvenio.Id = ateAtendimento.SisConveniolId AND SisConvenio.IsDeleted = @deleted
                            LEFT JOIN SisPessoa AS SisPessoaConvenio ON  SisPessoaConvenio.Id = SisConvenio.SisPessoaId AND SisPessoaConvenio.IsDeleted = @deleted
                            LEFT JOIN (
                                SELECT 
                                    COUNT(AssSolicitacaoExameItem.Id) AS Total, AteAtendimento.Id AS AtendimentoId
                                FROM AssSolicitacaoExameItem
                                    INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id 
                                    INNER JOIN AteAtendimento ON AssSolicitacaoExame.AtendimentoId = AteAtendimento.Id
                                    INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                                    LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                                    WHERE AssSolicitacaoExame.IsDeleted = @deleted AND (FatItem.IsLaboratorio = 1 OR FatGrupo.IsLaboratorio = 1)
                                GROUP BY  AteAtendimento.Id
                            ) AS QtdLabAssSolicitacaoExame ON QtdLabAssSolicitacaoExame.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN (
                                SELECT 
                                    COUNT(AssSolicitacaoExameItem.Id) AS Total, AteAtendimento.Id AS AtendimentoId
                                FROM AssSolicitacaoExameItem
                                    INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id 
                                    INNER JOIN AteAtendimento ON AssSolicitacaoExame.AtendimentoId = AteAtendimento.Id
                                    INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                                    LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                                    WHERE AssSolicitacaoExame.IsDeleted = @deleted AND (FatItem.IsLaudo = 1 OR FatGrupo.IsLaudo = 1)
                                GROUP BY  AteAtendimento.Id
                            ) AS QtdLauAssSolicitacaoExame ON QtdLauAssSolicitacaoExame.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN (
                                SELECT 
                                    COUNT(LauMovimentoItem.Id) AS Total, LauMovimento.AtendimentoId AS AtendimentoId 
                                FROM LauMovimentoItem 
                                    INNER JOIN LauMovimento ON LauMovimento.Id = LauMovimentoItem.LauMovimentoId
                                WHERE 
                                    -- LauMovimentoItem.Status IN({(int)EnumStatusLaudo.LaudoRevisado},{(int)EnumStatusLaudo.ComLaudo}) // Marcio Pediu para tirar por enquanto.
                                    LauMovimentoItem.IsDeleted = @deleted
                                GROUP BY LauMovimento.AtendimentoId) AS QtdLauMovimentoItem ON QtdLauMovimentoItem.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN (
                                SELECT COUNT(LabResultadoExame.Id) AS Total, LabResultado.AteAtendimentoId AS AtendimentoId 
                                FROM  LabResultadoExame 
                                    INNER JOIN LabResultado ON LabResultado.Id = LabResultadoExame.LabResultadoId
                                WHERE  
                                   LabResultadoExame.ExameStatusId IN({(int)EnumStatusExame.Digitado},{(int)EnumStatusExame.Conferido})
                                AND
                                    LabResultadoExame.IsDeleted = @deleted
                                GROUP BY LabResultado.AteAtendimentoId) AS QtdLabResultadoExame ON QtdLabResultadoExame.AtendimentoId = ateAtendimento.Id
                            ";

            const string WhereClause = @"ateAtendimento.IsDeleted = @deleted AND ateAtendimento.IsAmbulatorioEmergencia = @IsAmbulatorioEmergencia AND ateAtendimento.AtendimentoMotivoCancelamentoId IS NULL";

            var tableAtendimentoIndex = await this.CreateDataTable<AtendimentoIndexDto, ListarAtendimentosInput>()
                       .AddDefaultField(DefaultField)
                       .AddSelectClause(SelectClause)
                       .AddFromClause(FromClause)
                       .AddWhereClause(WhereClause)
                       .AddWhereMethod(ExecutaFiltroAtendimento)
                       .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                       .ExecuteAsync(input).ConfigureAwait(false);

            foreach (var item in tableAtendimentoIndex.Items)
            {
                if (item.PacienteNascimento.HasValue)
                {
                    item.PacienteIdade = FuncoesGlobais.ObterIdadeCompleto(item.PacienteNascimento);
                }
            }

            return tableAtendimentoIndex;
        }


        /// <summary>
        /// The executa filtro atendimento.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="dapperParameters">
        /// The dapper parameters.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ExecutaFiltroAtendimento(ListarAtendimentosInput input, Dictionary<string, object> dapperParameters)
        {
            var where = new StringBuilder();
            dapperParameters.Add("deleted", false);

            where.WhereIf(
                !input.Filtro.IsNullOrEmpty(),
            @" AND (
                    SisPessoaPaciente.NomeCompleto LIKE '%'+@Filtro +'%'
                    OR unidadeOrganizacional.Descricao LIKE '%'+@Filtro +'%'
                    OR ateAtendimento.Codigo LIKE '%'+@Filtro +'%'
                    OR PessoaPaciente.CodigoPaciente LIKE '%'+@Filtro +'%'
                    OR PessoaMedico.NomeCompleto LIKE '%'+@Filtro +'%'
                    OR AteLeito.Descricao LIKE '%'+@Filtro +'%'
                    OR SisEmpresa.NomeFantasia LIKE '%'+@Filtro +'%'
                    OR SisTipoAcomodacao.Descricao LIKE '%'+@Filtro +'%'
                    ) ");

            var atendimentoStatusId = new List<long>();
            if (input.AtendimentoStatusId != null && input.AtendimentoStatusId is JArray)
            {
                var atendimentoStatusArray = (JArray)input.AtendimentoStatusId;
                if (atendimentoStatusArray != null)
                {
                    atendimentoStatusId.AddRange(atendimentoStatusArray.ToObject<IList<long>>());
                }
            }
            else if (input.AtendimentoStatusId != null)
            {
                atendimentoStatusId.Add(Convert.ToInt64(input.AtendimentoStatusId));
            }

            if (!dapperParameters.ContainsKey("atendimentoStatusId"))
            {
                dapperParameters.Add("atendimentoStatusId", atendimentoStatusId);
            }
            else
            {
                dapperParameters["atendimentoStatusId"] = atendimentoStatusId;
            }

            where.WhereIf(!input.UnidadeOrganizacionais.IsNullOrEmpty(), " AND AteLeito.SisUnidadeOrganizacionalId IN @UnidadeOrganizacionais ");

            where.WhereIf(!atendimentoStatusId.IsNullOrEmpty(), " AND ateAtendimento.AteAtendimentoStatusId IN @atendimentoStatusId ");

            where.WhereIf(input.EmpresaId > 0, @" AND ateAtendimento.SisEmpresaId = @EmpresaId ");

            where.WhereIf(input.UnidadeOrganizacionalId > 0, @" AND ateAtendimento.SisUnidadeOrganizacionalId = @UnidadeOrganizacionalId ");

            where.WhereIf(input.ConvenioId > 0, @" AND ateAtendimento.SisConveniolId = @ConvenioId ");

            where.WhereIf(input.MedicoId > 0 && input.MedicoId != input.UserMedicoId, @" AND ateAtendimento.SisMedicoId = @MedicoId ");

            where.WhereIf(
                input.UserMedicoId > 0 && input.MedicoId == input.UserMedicoId,
                @" AND (
                            ateAtendimento.SisMedicoId = @UserMedicoId
                            OR PessoaMedico.IsIndeterminado = 1
                            OR @IsAtendimento = 1
                        )");

            where.WhereIf(input.PacienteId > 0, @" AND ateAtendimento.SisPacienteId = @PacienteId ");

            where.WhereIf(!input.NomePaciente.IsNullOrEmpty(), @" AND SisPessoaPaciente.NomeCompleto LIKE '%'+@NomePaciente +'%'");

            where.WhereIf(input.FiltroData == "Internado", @" AND ateAtendimento.DataAlta IS NULL");

            where.WhereIf(input.FiltroData == "Cancelado", @" AND ateAtendimento.AtendimentoMotivoCancelamentoId IS NOT NULL ");

            where.WhereIf(
                input.EndDate.HasValue && input.StartDate.HasValue && input.FiltroData == "Atendimento",
                @" AND ateAtendimento.DataRegistro >= @StartDate 
                                    AND ateAtendimento.DataRegistro <= @EndDate 
                                    AND ateAtendimento.AtendimentoMotivoCancelamentoId IS NULL ");

            where.WhereIf(
                input.EndDate.HasValue && input.StartDate.HasValue && input.FiltroData == "Alta",
                @" AND ateAtendimento.DataAlta >= @StartDate 
                                    AND ateAtendimento.DataAlta <= @EndDate 
                                    AND ateAtendimento.AtendimentoMotivoCancelamentoId  IS NULL ");

            where.WhereIf(input.NacionalidadeResponsavelId > 0, @" AND ateAtendimento.NacionalidadeResponsavelId = @NacionalidadeResponsavelId ");

            where.WhereIf(input.IsAmbulatorioEmergencia.HasValue, @" AND ateAtendimento.IsAmbulatorioEmergencia = @IsAmbulatorioEmergencia ");

            where.WhereIf(input.IsInternacao.HasValue, @" AND ateAtendimento.IsInternacao = @IsInternacao ");

            return where.ToString();
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        private IQueryable<Atendimento> ConsultaListarFiltro(ListarAtendimentosInput input)
        {
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                return atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                    .Include(m => m.Paciente.SisPessoa).Include(m => m.Medico).Include(m => m.Medico.SisPessoa)
                    .Include(m => m.AtendimentoTipo).Include(m => m.Convenio).Include(m => m.Convenio.SisPessoa)
                    .Include(m => m.Empresa).Include(m => m.Especialidade).Include(m => m.Guia).Include(m => m.Leito)
                    .Include(m => m.Leito.TipoAcomodacao).Include(m => m.MotivoAlta).Include(m => m.Nacionalidade)
                    .Include(m => m.Origem).Include(m => m.Plano).Include(m => m.ServicoMedicoPrestado)
                    .Include(m => m.UnidadeOrganizacional)

                    // === filtro generico pablo 
                    .WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Paciente.NomeCompleto.Contains(input.Filtro)
                             || m.UnidadeOrganizacional.Descricao.Contains(input.Filtro)
                             || m.Codigo.Contains(input.Filtro)
                             || m.Paciente.CodigoPaciente.ToString().Contains(input.Filtro)
                             || m.Convenio.NomeFantasia.Contains(input.Filtro)
                             || m.Medico.NomeCompleto.Contains(input.Filtro)
                             || m.Leito.TipoAcomodacao.Descricao.Contains(input.Filtro)
                             || m.Leito.Descricao.Contains(input.Filtro)
                             || m.Empresa.NomeFantasia.Contains(input.Filtro))

                    // ===
                    .Where(a => a.IsAmbulatorioEmergencia == true)
                    .WhereIf(
                        input.FiltroData == "Alta",
                        m => m.DataAlta >= input.StartDate && m.DataAlta <= input.EndDate)
                    .WhereIf(
                        (input.FiltroData == "Atendimento"),
                        m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate)
                    .WhereIf(input.EmpresaId > 0, m => m.EmpresaId == input.EmpresaId)
                    .WhereIf(
                        input.UnidadeOrganizacionalId > 0,
                        m => m.UnidadeOrganizacionalId == input.UnidadeOrganizacionalId)
                    .WhereIf(input.ConvenioId > 0, m => m.ConvenioId == input.ConvenioId)
                    .WhereIf(input.MedicoId > 0, m => m.MedicoId == input.MedicoId)
                    .WhereIf(input.PacienteId > 0, m => m.PacienteId == input.PacienteId).WhereIf(
                        input.NacionalidadeResponsavelId > 0,
                        m => m.NacionalidadeResponsavelId == input.NacionalidadeResponsavelId)

                    // .WhereIf(input.IsAmbulatorioEmergencia.HasValue, m => m.IsAmbulatorioEmergencia == input.IsAmbulatorioEmergencia.Value)
                    // .WhereIf(input.IsInternacao.HasValue, m => m.IsInternacao == input.IsInternacao.Value)
                    // .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()))
                    .WhereIf(
                        !input.NomePaciente.IsNullOrEmpty(),
                        m => m.Paciente.NomeCompleto.Contains(input.NomePaciente))

                    // .WhereIf(input.FiltroData == "Internado", m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate &&  m.DataAlta == null)
                    .WhereIf(input.FiltroData == "Internado", m => m.DataAlta == null);
            }

            // .WhereIf(input.Internados, m => m.DataAlta == null)
            // .WhereIf(input.Internados, a => DateTime.Compare((DateTime)a.DataAlta, DateTime.Now) >= 0)
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        private IQueryable<Atendimento> ConsultaListarFiltroInternacao(ListarAtendimentosInput input)
        {
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                return atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                    .Include(m => m.Paciente.SisPessoa).Include(m => m.Medico).Include(m => m.Medico.SisPessoa)
                    .Include(m => m.AtendimentoTipo).Include(m => m.Convenio).Include(m => m.Convenio.SisPessoa)
                    .Include(m => m.Empresa).Include(m => m.Especialidade).Include(m => m.Guia).Include(m => m.Leito)
                    .Include(m => m.Leito.TipoAcomodacao).Include(m => m.MotivoAlta).Include(m => m.Nacionalidade)
                    .Include(m => m.Origem).Include(m => m.Plano).Include(m => m.ServicoMedicoPrestado)
                    .Include(m => m.UnidadeOrganizacional)

                    // === filtro generico pablo 
                    .WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Paciente.NomeCompleto.Contains(input.Filtro)
                             || m.UnidadeOrganizacional.Descricao.Contains(input.Filtro)
                             || m.Codigo.Contains(input.Filtro)
                             || m.Paciente.CodigoPaciente.ToString().Contains(input.Filtro)
                             || m.Convenio.NomeFantasia.Contains(input.Filtro)
                             || m.Medico.NomeCompleto.Contains(input.Filtro)
                             || m.Leito.TipoAcomodacao.Descricao.Contains(input.Filtro)
                             || m.Leito.Descricao.Contains(input.Filtro.ToUpper())
                             || m.Empresa.NomeFantasia.Contains(input.Filtro))

                    // ===
                    .Where(a => a.IsInternacao == true)

                    // .WhereIf((input.FiltroData == "Internacao"), m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate)
                    // .Where(m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate)
                    .WhereIf(input.EmpresaId > 0, m => m.EmpresaId == input.EmpresaId)
                    .WhereIf(
                        input.UnidadeOrganizacionalId > 0,
                        m => m.UnidadeOrganizacionalId == input.UnidadeOrganizacionalId)
                    .WhereIf(input.ConvenioId > 0, m => m.ConvenioId == input.ConvenioId)
                    .WhereIf(input.MedicoId > 0, m => m.MedicoId == input.MedicoId)
                    .WhereIf(input.PacienteId > 0, m => m.PacienteId == input.PacienteId).WhereIf(
                        input.NacionalidadeResponsavelId > 0,
                        m => m.NacionalidadeResponsavelId == input.NacionalidadeResponsavelId)

                    // .WhereIf(input.IsAmbulatorioEmergencia.HasValue, m => m.IsAmbulatorioEmergencia == input.IsAmbulatorioEmergencia.Value)
                    // .WhereIf(input.IsInternacao.HasValue, m => m.IsInternacao == input.IsInternacao.Value)
                    // .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()))
                    .WhereIf(
                        !input.NomePaciente.IsNullOrEmpty(),
                        m => m.Paciente.NomeCompleto.Contains(input.NomePaciente))

                    // .WhereIf(input.Internados, a => DateTime.Compare((DateTime)a.DataAlta, DateTime.Now) >= 0) Internação
                    .WhereIf(
                        input.FiltroData == "Alta",
                        m => m.DataAlta >= input.StartDate && m.DataAlta <= input.EndDate)
                    .WhereIf(input.FiltroData == "Internado", m => m.DataAlta == null).WhereIf(
                        input.FiltroData == "Atendimento",
                        m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate).WhereIf(
                        input.FiltroData == "Cancelado",
                        m => m.AtendimentoMotivoCancelamentoId != null);
            }

            // .OrderByDescending(m => input.Sorting);
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<AtendimentoIndexDto>> ListarAtendimentos(ListarAtendimentosInput input)
        {
            var atendimentos = new List<AtendimentoIndexDto>();

            try
            {
                switch (input.TipoAtendimento)
                {
                    case "AMB":
                        var queryAmb = this.ConsultaListarFiltro(input);
                        var amb = await queryAmb.ToListAsync().ConfigureAwait(false); // ListarFiltro(input);
                        foreach (var item in amb)
                        {
                            atendimentos.Add(new AtendimentoIndexDto
                            {
                                CodigoAtendimento = item.Codigo,
                                CodigoPaciente =
                                                         item.Paciente != null ? item.Paciente.Codigo : string.Empty,
                                Convenio = item.Convenio != null
                                                                    ? item.Convenio.NomeFantasia
                                                                    : string.Empty,
                                DataAlta = item.DataAlta,

                                // DataFim=item.da
                                // DataInicioConta=item.DataAlta
                                DataRegistro = item.DataRegistro,
                                Empresa =
                                                         item.Empresa != null
                                                             ? item.Empresa.NomeFantasia
                                                             : string.Empty,
                                Guia = item.Guia != null ? item.Guia.Descricao : string.Empty,
                                LeitoAtual =
                                                         item.Leito != null ? item.Leito.Descricao : string.Empty,
                                Matricula = item.Matricula,
                                NumeroGuia = item.NumeroGuia,
                                Paciente =
                                                         item.Paciente != null
                                                             ? item.Paciente.NomeCompleto
                                                             : string.Empty,
                                Plano = item.Plano != null ? item.Plano.Descricao : string.Empty,
                                TipoLeito =
                                                         item.Leito != null
                                                             ? item.Leito.TipoAcomodacao.Descricao
                                                             : string.Empty,
                                Unidade =
                                                         item.Empresa != null ? item.Empresa.Codigo : string.Empty,
                                Id = item.Id,
                            });
                        }

                        break;
                    case "INT":
                        var queryInt = this.ConsultaListarFiltroInternacao(input);
                        var intern = await queryInt.ToListAsync().ConfigureAwait(false);
                        foreach (var item in intern)
                        {
                            atendimentos.Add(new AtendimentoIndexDto
                            {
                                CodigoAtendimento = item.Codigo,
                                CodigoPaciente =
                                                         item.Paciente != null ? item.Paciente.Codigo : string.Empty,
                                Convenio = item.Convenio != null
                                                                    ? item.Convenio.NomeFantasia
                                                                    : string.Empty,
                                DataAlta = item.DataAlta,

                                // DataFim=item.da
                                // DataInicioConta=item.DataAlta
                                DataRegistro = item.DataRegistro,
                                Empresa =
                                                         item.Empresa != null
                                                             ? item.Empresa.NomeFantasia
                                                             : string.Empty,
                                Guia = item.Guia != null ? item.Guia.Descricao : string.Empty,
                                LeitoAtual =
                                                         item.Leito != null ? item.Leito.Descricao : string.Empty,
                                Matricula = item.Matricula,
                                NumeroGuia = item.NumeroGuia,
                                Paciente =
                                                         item.Paciente != null
                                                             ? item.Paciente.NomeCompleto
                                                             : string.Empty,
                                Plano = item.Plano != null ? item.Plano.Descricao : string.Empty,
                                TipoLeito =
                                                         item.Leito != null
                                                             ? item.Leito.TipoAcomodacao.Descricao
                                                             : string.Empty,
                                Unidade =
                                                         item.Empresa != null ? item.Empresa.Codigo : string.Empty,
                                Id = item.Id,
                            });
                        }

                        break;
                    default:
                        var queryAmbAll = this.ConsultaListarFiltro(input);
                        var ambAll = await queryAmbAll.ToListAsync().ConfigureAwait(false);
                        foreach (var item in ambAll)
                        {
                            atendimentos.Add(new AtendimentoIndexDto
                            {
                                CodigoAtendimento = item.Codigo,
                                CodigoPaciente =
                                                         item.Paciente != null ? item.Paciente.Codigo : string.Empty,
                                Convenio = item.Convenio != null
                                                                    ? item.Convenio.NomeFantasia
                                                                    : string.Empty,
                                DataAlta = item.DataAlta,

                                // DataFim=item.da
                                // DataInicioConta=item.DataAlta
                                DataRegistro = item.DataRegistro,
                                Empresa =
                                                         item.Empresa != null
                                                             ? item.Empresa.NomeFantasia
                                                             : string.Empty,
                                Guia = item.Guia != null ? item.Guia.Descricao : string.Empty,
                                LeitoAtual =
                                                         item.Leito != null ? item.Leito.Descricao : string.Empty,
                                Matricula = item.Matricula,
                                NumeroGuia = item.NumeroGuia,
                                Paciente =
                                                         item.Paciente != null
                                                             ? item.Paciente.NomeCompleto
                                                             : string.Empty,
                                Plano = item.Plano != null ? item.Plano.Descricao : string.Empty,
                                TipoLeito =
                                                         item.Leito != null
                                                             ? item.Leito.TipoAcomodacao.Descricao
                                                             : string.Empty,
                                Unidade =
                                                         item.Empresa != null ? item.Empresa.Codigo : string.Empty,
                                Id = item.Id,
                            });
                        }

                        var queryIntAll = this.ConsultaListarFiltroInternacao(input);
                        var internAll = await queryIntAll.ToListAsync().ConfigureAwait(false);
                        foreach (var item in internAll)
                        {
                            atendimentos.Add(new AtendimentoIndexDto
                            {
                                CodigoAtendimento = item.Codigo,
                                CodigoPaciente =
                                                         item.Paciente != null ? item.Paciente.Codigo : string.Empty,
                                Convenio = item.Convenio != null
                                                                    ? item.Convenio.NomeFantasia
                                                                    : string.Empty,
                                DataAlta = item.DataAlta,

                                // DataFim=item.da
                                // DataInicioConta=item.DataAlta
                                DataRegistro = item.DataRegistro,
                                Empresa =
                                                         item.Empresa != null
                                                             ? item.Empresa.NomeFantasia
                                                             : string.Empty,
                                Guia = item.Guia != null ? item.Guia.Descricao : string.Empty,
                                LeitoAtual =
                                                         item.Leito != null ? item.Leito.Descricao : string.Empty,
                                Matricula = item.Matricula,
                                NumeroGuia = item.NumeroGuia,
                                Paciente =
                                                         item.Paciente != null
                                                             ? item.Paciente.NomeCompleto
                                                             : string.Empty,
                                Plano = item.Plano != null ? item.Plano.Descricao : string.Empty,
                                TipoLeito =
                                                         item.Leito != null
                                                             ? item.Leito.TipoAcomodacao.Descricao
                                                             : string.Empty,
                                Unidade =
                                                         item.Empresa != null ? item.Empresa.Codigo : string.Empty,
                                Id = item.Id,
                            });
                        }

                        break;
                }

                var contarAtendimentos = atendimentos.Distinct().Count();

                var atendimentosDto = atendimentos
                    .AsQueryable()
                    .AsNoTracking()
                    .PageBy(input)
                    .OrderBy(input.Sorting)
                    .Distinct()
                    .ToList();

                return new PagedResultDto<AtendimentoIndexDto>(contarAtendimentos, atendimentosDto);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<AtendimentoIndexDto>> ListarFiltroInternacao(ListarAtendimentosInput input)
        {
            const string DefaultField = "ateAtendimento.Id";

            const string SelectClause = @"ateAtendimento.Id, 
                        unidadeOrganizacional.Descricao AS Unidade,
                        ateAtendimento.Codigo AS CodigoAtendimento,
                        ateAtendimento.DataRegistro,
                        AteAtendimento.DataAlta,
                        SisPessoaPaciente.NomeCompleto AS Paciente,
                        PessoaPaciente.CodigoPaciente AS CodigoPaciente,
                        SisPessoaPaciente.Nascimento AS PacienteNascimento,
                        PessoaMedico.NomeCompleto AS Medico,
                        ateAtendimento.Matricula,
                        SisTipoAcomodacao.Descricao AS TipoLeito,
                        AteLeito.Descricao AS Leito,
                        AteClassificacaoAtendimento.Descricao AS Classificacao,
                        AteClassificacaoAtendimento.Cor AS CorClassificacao,
                        AteProtocoloAtendimento.Descricao AS Protocolo,
                        SisEmpresa.NomeFantasia AS Empresa,
                        SisPessoaConvenio.NomeFantasia AS Convenio,
                        AteSenha.Numero AS Senha,
                        AteSenhaMov.Id AS SenhaAtendimentoId,
                        AteAtendimento.IsControlaTev,
                        ateAtendimento.IsPendenteExames,
                        ateAtendimento.IsPendenteMedicacao,
                        ateAtendimento.IsPendenteProcedimento,
                        ateAtendimento.StatusAguardando,
                        ateAtendimento.StatusAtendido,
                        QtdLauAssSolicitacaoExame.Total As QtdLauAssSolicitacaoExame,
                        QtdLabAssSolicitacaoExame.Total As QtdLabAssSolicitacaoExame,
                        QtdLauMovimentoItem.Total As QtdLauMovimentoItem,
                        QtdLabResultadoExame.Total As QtdLabResultadoExame,
            CAST(DATEADD(DAY,QuantidadeAutorizada,ateAtendimento.DataRegistro) AS DATE) as DataAutorizada,
                        (CASE WHEN QuantidadeAutorizada <> 0 AND ateAtendimento.DataAlta IS NULL AND CAST(DATEADD(DAY,QuantidadeAutorizada,ateAtendimento.DataRegistro) AS DATE) < CAST(GETDATE() AS DATE) THEN
					       @Vermelho
					       WHEN QuantidadeAutorizada <> 0 AND ateAtendimento.DataAlta IS NULL AND CAST(DATEADD(DAY,QuantidadeAutorizada,ateAtendimento.DataRegistro) AS DATE) = CAST(GETDATE() AS DATE) THEN
					       @Amarelo
					       WHEN QuantidadeAutorizada <> 0 THEN 
					       @Verde
					       ELSE
					       ''
						   END) AS CorStatusAutorizacao";

            var FromClause = $@"AteAtendimento AS ateAtendimento
                            LEFT JOIN SisUnidadeOrganizacional AS unidadeOrganizacional
                            ON ateAtendimento.SisUnidadeOrganizacionalId = unidadeOrganizacional.Id
                            LEFT JOIN SisPaciente AS PessoaPaciente ON PessoaPaciente.Id = ateAtendimento.SisPacienteId AND PessoaPaciente.IsDeleted = @deleted
                            LEFT JOIN SisPessoa SisPessoaPaciente ON PessoaPaciente.SisPessoaId = SisPessoaPaciente.Id AND SisPessoaPaciente.IsDeleted = @deleted
                            LEFT JOIN SisMedico AS PessoaMedico ON PessoaMedico.Id = ateAtendimento.SisMedicoId AND PessoaMedico.IsDeleted = @deleted
                            LEFT JOIN SisPessoa SisPessoaMedico ON SisPessoaMedico.Id = PessoaMedico.SisPessoaId AND SisPessoaMedico.IsDeleted = @deleted
                            LEFT JOIN AteLeito ON AteLeito.Id = ateAtendimento.AteLeitoId AND AteLeito.IsDeleted = @deleted
                            LEFT JOIN SisTipoAcomodacao ON SisTipoAcomodacao.Id = AteLeito.SisTipoAcomodacaoId AND SisTipoAcomodacao.IsDeleted = @deleted
                            LEFT JOIN AteClassificacaoAtendimento ON AteClassificacaoAtendimento.Id = ateAtendimento.ClassificacaoAtendimentoId AND AteClassificacaoAtendimento.IsDeleted = @deleted
                            LEFT JOIN AteProtocoloAtendimento ON AteProtocoloAtendimento.Id = ateAtendimento.ProtocoloAtendimentoId AND AteProtocoloAtendimento.IsDeleted = @deleted
                            LEFT JOIN AteAtendimentoStatus ON AteAtendimentoStatus.Id = ateAtendimento.AteAtendimentoStatusId AND AteAtendimentoStatus.IsDeleted = @deleted
                            LEFT JOIN SisEmpresa ON SisEmpresa.Id = ateAtendimento.SisEmpresaId AND SisEmpresa.IsDeleted = @deleted
                            LEFT JOIN AteSenha ON AteSenha.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN 
                            (SELECT max(AteSenhaMov.Id) as AteSenhaMovId, AteSenhaMov.SenhaId
                            FROM AteSenhaMov
                            GROUP BY(AteSenhaMov.SenhaId)) AS senhaMovMax ON senhaMovMax.SenhaId = AteSenha.Id
                            LEFT JOIN AteSenhaMov ON AteSenhaMov.Id = senhaMovMax.AteSenhaMovId AND AteSenhaMov.IsDeleted = @deleted
                            LEFT JOIN (
								SELECT SUM(AteAutorizacaoProcedimentoItem.QuantidadeAutorizada) AS QuantidadeAutorizada, 
										AteAutorizacaoProcedimento.AtendimentoId
								FROM AteAutorizacaoProcedimentoItem INNER JOIN AteAutorizacaoProcedimento ON AteAutorizacaoProcedimento.Id = AteAutorizacaoProcedimentoItem.AutorizacaoProcedimentoId
								WHERE AteAutorizacaoProcedimentoItem.StatusId IN @ProcedimentoItemStatus GROUP BY AteAutorizacaoProcedimento.AtendimentoId) AS prorrogacoes ON ateAtendimento.Id = prorrogacoes.AtendimentoId
                            LEFT JOIN SisConvenio ON SisConvenio.Id = ateAtendimento.SisConveniolId AND SisConvenio.IsDeleted = @deleted
                            LEFT JOIN SisPessoa AS SisPessoaConvenio ON  SisPessoaConvenio.Id = SisConvenio.SisPessoaId AND SisPessoaConvenio.IsDeleted = @deleted

                            LEFT JOIN (
                                SELECT 
                                    COUNT(AssSolicitacaoExameItem.Id) AS Total, AteAtendimento.Id AS AtendimentoId
                                FROM AssSolicitacaoExameItem
                                    INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id 
                                    INNER JOIN AteAtendimento ON AssSolicitacaoExame.AtendimentoId = AteAtendimento.Id
                                    INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                                    LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                                    WHERE AssSolicitacaoExame.IsDeleted = @deleted AND (FatItem.IsLaboratorio = 1 OR FatGrupo.IsLaboratorio = 1)
                                GROUP BY  AteAtendimento.Id
                            ) AS QtdLabAssSolicitacaoExame ON QtdLabAssSolicitacaoExame.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN (
                                SELECT 
                                    COUNT(AssSolicitacaoExameItem.Id) AS Total, AteAtendimento.Id AS AtendimentoId
                                FROM AssSolicitacaoExameItem
                                    INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id 
                                    INNER JOIN AteAtendimento ON AssSolicitacaoExame.AtendimentoId = AteAtendimento.Id
                                    INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                                    LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                                    WHERE AssSolicitacaoExame.IsDeleted = @deleted AND (FatItem.IsLaudo = 1 OR FatGrupo.IsLaudo = 1)
                                GROUP BY  AteAtendimento.Id
                            ) AS QtdLauAssSolicitacaoExame ON QtdLauAssSolicitacaoExame.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN (
                                SELECT 
                                    COUNT(LauMovimentoItem.Id) AS Total, LauMovimento.AtendimentoId AS AtendimentoId 
                                FROM LauMovimentoItem 
                                    INNER JOIN LauMovimento ON LauMovimento.Id = LauMovimentoItem.LauMovimentoId
                                WHERE 
                                    -- LauMovimentoItem.Status IN({(int)EnumStatusLaudo.LaudoRevisado},{(int)EnumStatusLaudo.ComLaudo}) // Marcio Pediu para tirar por enquanto.
                                    LauMovimentoItem.IsDeleted = @deleted
                                GROUP BY LauMovimento.AtendimentoId) AS QtdLauMovimentoItem ON QtdLauMovimentoItem.AtendimentoId = ateAtendimento.Id
                            LEFT JOIN (
                                SELECT COUNT(LabResultadoExame.Id) AS Total, LabResultado.AteAtendimentoId AS AtendimentoId 
                                FROM  LabResultadoExame 
                                    INNER JOIN LabResultado ON LabResultado.Id = LabResultadoExame.LabResultadoId
                                WHERE  
                                   LabResultadoExame.ExameStatusId IN({(int)EnumStatusExame.Digitado},{(int)EnumStatusExame.Conferido})
                                AND
                                    LabResultadoExame.IsDeleted = @deleted
                                GROUP BY LabResultado.AteAtendimentoId) AS QtdLabResultadoExame ON QtdLabResultadoExame.AtendimentoId = ateAtendimento.Id";

            const string WhereClause = @"ateAtendimento.IsDeleted = @deleted AND ateAtendimento.AtendimentoMotivoCancelamentoId IS NULL";

            var tableAtendimentoIndex = await this.CreateDataTable<AtendimentoIndexDto, ListarAtendimentosInput>()
                       .AddDefaultField(DefaultField)
                       .AddSelectClause(SelectClause)
                       .AddFromClause(FromClause)
                       .AddWhereClause(WhereClause)
                       .AddWhereMethod(this.ExecutaFiltroInternacao)
                       .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                       .ExecuteAsync(input).ConfigureAwait(false);

            foreach (var item in tableAtendimentoIndex.Items)
            {
                if (item.PacienteNascimento.HasValue)
                {
                    item.PacienteIdade = FuncoesGlobais.ObterIdadeCompleto(item.PacienteNascimento);
                }
            }

            return tableAtendimentoIndex;
        }

        public async Task<PagedResultDto<SolicitacaoExameIndex>> GetDetalhamentoExamesSolicitacao(DetalhamentoExameInput input)
        {
            using (var sqlConnection = new SqlConnection(this.GetConnection()))
            {
                var result = new List<SolicitacaoExameIndex>();
                const string DefaultField = "AssSolicitacaoExame.Id";

                const string SelectClause = @"DISTINCT AssSolicitacaoExame.Id,
                            AssSolicitacaoExame.Codigo,
                            AssSolicitacaoExame.DataSolicitacao,
                            SisPessoaMedico.NomeCompleto AS MedicoSolicitante,
                            SisUnidadeOrganizacional.Descricao AS UnidadeOrganizacional,
                            AssSolicitacaoExame.IsDeleted AS IsDeleted";
                string FromClause = @"AssSolicitacaoExameItem
                            INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id
                            INNER JOIN AteAtendimento ON AssSolicitacaoExame.AtendimentoId = AteAtendimento.Id
                            INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                            LEFT JOIN SisPaciente ON AteAtendimento.SisPacienteId = SisPaciente.Id
                            LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                            LEFT JOIN SisMedico ON AssSolicitacaoExame.SisMedicoSolicitanteId = SisMedico.Id
                            LEFT JOIN SisPessoa AS SisPessoaMedico ON SisPessoaMedico.Id = SisMedico.SisPessoaId
                            LEFT JOIN SisUnidadeOrganizacional ON SisUnidadeOrganizacional.Id = AssSolicitacaoExame.SisUnidadeOrganizacionalId";

                string WhereClause = @"AssSolicitacaoExame.IsDeleted = 0 AND AteAtendimento.Id = @Id";
                if (input.Tipo == "Lab")
                {
                    WhereClause += @" AND(FatItem.IsLaboratorio = 1 OR FatGrupo.IsLaboratorio = 1)";
                }
                else if (input.Tipo == "Img")
                {
                    WhereClause += @" AND (FatItem.IsLaudo = 1 OR FatGrupo.IsLaudo = 1)";
                }
                else
                {
                    return new PagedResultDto<SolicitacaoExameIndex>();
                }


                return await this.CreateDataTable<SolicitacaoExameIndex, DetalhamentoExameInput>()
                       .AddDefaultField(DefaultField)
                       .AddSelectClause(SelectClause)
                       .AddFromClause(FromClause)
                       .AddWhereClause(WhereClause)
                       .AddOrderByClause("AssSolicitacaoExame.DataSolicitacao DESC")
                       .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                       .ExecuteAsync(input).ConfigureAwait(false);


            }
        }

        public async Task<PagedResultDto<SolicitacaoExameItemList>> GetDetalhamentoExameItemSolicitacao(DetalhamentoExameInput input)
        {
            using (var sqlConnection = new SqlConnection(this.GetConnection()))
            {
                var result = new PagedResultDto<SolicitacaoExameItemList>();

                var defaultField = "AssSolicitacaoExameItem.Id";
                var selectClause = @"FatItem.Descricao AS FaturamentoItem,
                        AssSolicitacaoExameItem.GuiaNumero,
                        AssSolicitacaoExameItem.Id,
                        AssSolicitacaoExameItem.IsDeleted,
                        AssSolicitacaoExameItem.IsSistema,
                        LabMaterial.Descricao AS Material,
                        AssSolicitacaoExameItem.AccessNumber";
                var fromClause = @"AssSolicitacaoExameItem
                        INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id
                        INNER JOIN AteAtendimento ON AssSolicitacaoExame.AtendimentoId = AteAtendimento.Id
                        LEFT JOIN SisPaciente ON AteAtendimento.SisPacienteId = SisPaciente.Id
                        INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                        LEFT JOIN LabMaterial ON LabMaterial.Id = AssSolicitacaoExameItem.LabMaterialId
                        LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                        LEFT JOIN SisMedico ON AssSolicitacaoExame.SisMedicoSolicitanteId = SisMedico.Id
                        LEFT JOIN SisPessoa AS SisPessoaMedico ON SisPessoaMedico.Id = SisMedico.SisPessoaId
                        LEFT JOIN SisUnidadeOrganizacional ON SisUnidadeOrganizacional.Id = AssSolicitacaoExame.SisUnidadeOrganizacionalId";
                var whereClause = input.Tipo == "Lab" ?
                         // Lab
                         @"AssSolicitacaoExame.IsDeleted = 0
	                            AND AssSolicitacaoExameItem.AssSolicitacaoExameId = @id
                            AND (FatItem.IsLaboratorio = 1 OR FatGrupo.IsLaboratorio = 1)" :

                         // Img
                         @"AssSolicitacaoExame.IsDeleted = 0
	                            AND AssSolicitacaoExameItem.AssSolicitacaoExameId = @id
                            AND (FatItem.IsLaudo = 1 OR FatGrupo.IsLaudo = 1)";

                result = await this.CreateDataTable<SolicitacaoExameItemList, DetalhamentoExameInput>()
                           .AddDefaultField(defaultField)
                           .AddSelectClause(selectClause)
                           .AddFromClause(fromClause)
                           .AddWhereClause(whereClause)
                           .AddOrderByClause("AssSolicitacaoExame.DataSolicitacao DESC")
                           .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                           .ExecuteAsync(input).ConfigureAwait(false);

                foreach (var item in result.Items)
                {
                    item.AccessNumber = SolicitacaoExameItemAppService.FormatAccessNumber(item.AccessNumber);
                }

                return result;
            }
        }



        public async Task<PagedResultDto<SolicitacaoExameIndex>> GetDetalhamentoExamesResultado(DetalhamentoExameInput input)
        {
            if (input.Tipo != "Img" && input.Tipo != "Lab")
            {
                return new PagedResultDto<SolicitacaoExameIndex>();
            }

            using (var sqlConnection = new SqlConnection(this.GetConnection()))
            {
                var result = new List<SolicitacaoExameIndex>();

                var defaultField = string.Empty;
                var selectClause = string.Empty;
                var fromClause = string.Empty;
                var whereClause = string.Empty;

                if (input.Tipo == "Img")
                {
                    defaultField = " LauMovimento.Id";

                    selectClause = @"
                        DISTINCT LauMovimento.Id,
                        LauMovimento.DataRegistro AS DataSolicitacao,
                        COALESCE(SisMedico.NomeCompleto,LauMovimento.MedicoSolicitante) AS MedicoSolicitante,
                        LauMovimentoStatus.Descricao AS Status
                        ";
                    fromClause = @"LauMovimento 
                    LEFT JOIN LauMovimentoStatus ON LauMovimento.LauMovimentoStatusId = LauMovimentoStatus.Id
                    LEFT JOIN SisMedico ON SisMedico.Id = LauMovimento.SisMedicoSolicitanteId";

                    whereClause = @"LauMovimento.IsDeleted = 0 AND LauMovimento.AtendimentoId = @Id";

                    return await this.CreateDataTable<SolicitacaoExameIndex, DetalhamentoExameInput>()
                           .AddDefaultField(defaultField)
                           .AddSelectClause(selectClause)
                           .AddFromClause(fromClause)
                           .AddWhereClause(whereClause)
                           .AddOrderByClause("LauMovimento.DataRegistro DESC")
                           .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                           .ExecuteAsync(input).ConfigureAwait(false);
                }

                defaultField = " LabResultado.Id";

                selectClause = @"
                        DISTINCT LabResultado.Id,
                        LabResultado.DataColeta AS DataSolicitacao,
                        LabResultado.Descricao,
                        COALESCE(SisMedico.NomeCompleto,LabResultado.NomeMedicoSolicitante) AS MedicoSolicitante,
                        LabResultadoStatus.Descricao AS Status
                        ";
                fromClause = @"
                        LabResultado -- INNER JOIN LabResultadoExame ON LabResultado.Id = LabResultadoExame.LabResultadoId
                        LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId
                        LEFT JOIN SisMedico ON SisMedico.Id = LabResultado.SisMedicoSolicitanteId";

                whereClause = @"LabResultado.IsDeleted = 0 AND LabResultado.AteAtendimentoId = @Id";

                return await this.CreateDataTable<SolicitacaoExameIndex, DetalhamentoExameInput>()
                       .AddDefaultField(defaultField)
                       .AddSelectClause(selectClause)
                       .AddFromClause(fromClause)
                       .AddWhereClause(whereClause)
                       .AddOrderByClause("LabResultado.DataColeta DESC")
                       .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                       .ExecuteAsync(input).ConfigureAwait(false);
            }
        }

        public async Task<PagedResultDto<SolicitacaoExameItemList>> GetDetalhamentoExameItemResultado(DetalhamentoExameInput input)
        {
            if (input.Tipo != "Img" && input.Tipo != "Lab")
            {
                return new PagedResultDto<SolicitacaoExameItemList>();
            }

            using (var sqlConnection = new SqlConnection(this.GetConnection()))
            {
                var result = new PagedResultDto<SolicitacaoExameItemList>();

                var defaultField = string.Empty;
                var selectClause = string.Empty;
                var fromClause = string.Empty;
                var whereClause = string.Empty;

                if (input.Tipo == "Img")
                {
                    defaultField = " LauMovimentoItem.Id";

                    selectClause = @"
                        LauMovimentoItem.Id,
						AssSolicitacaoExameItem.AccessNumber,
						FatItem.Descricao AS FaturamentoItem,
						LauMovimentoStatus.Descricao
                        ";
                    fromClause = @"LauMovimento INNER JOIN LauMovimentoItem ON LauMovimento.Id = LauMovimentoItem.LauMovimentoId
                     LEFT JOIN FatItem ON LauMovimentoItem.FatItemId = FatItem.Id
                     LEFT JOIN LauMovimentoStatus ON LauMovimentoStatus.Id = LauMovimentoItem.Status
                     LEFT JOIN AssSolicitacaoExameItem on LauMovimentoItem.AssSolicitacaoExameItemId = AssSolicitacaoExameItem.id";

                    whereClause = @"LauMovimentoItem.IsDeleted = 0 AND LauMovimentoItem.LauMovimentoId = @Id";

                    result = await this.CreateDataTable<SolicitacaoExameItemList, DetalhamentoExameInput>()
                           .AddDefaultField(defaultField)
                           .AddSelectClause(selectClause)
                           .AddFromClause(fromClause)
                           .AddWhereClause(whereClause)
                           .AddOrderByClause("LauMovimento.DataRegistro DESC")
                           .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                           .ExecuteAsync(input).ConfigureAwait(false);


                    foreach (var item in result.Items)
                    {
                        item.AccessNumber = SolicitacaoExameItemAppService.FormatAccessNumber(item.AccessNumber);
                    }

                    return result;
                }

                defaultField = " LabResultadoExame.Id";

                selectClause = @"
                           LabResultadoExame.Id,
					       FatItem.Descricao AS FaturamentoItem,
					       LabResultadoStatus.Descricao,
                           LabMaterial.Descricao AS Material
                        ";
                fromClause = @"
                        LabResultado  INNER JOIN LabResultadoExame ON LabResultado.Id = LabResultadoExame.LabResultadoId
						LEFT JOIN FatItem ON FatItem.Id = LabResultadoExame.LabFaturamentoItemId
                        LEFT JOIN LabMaterial ON LabResultadoExame.LabMaterialId = LabMaterial.Id
                        LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultadoExame.ExameStatusId";

                whereClause = @"LabResultado.IsDeleted = 0 AND LabResultadoExame.LabResultadoId= @id";

                return await this.CreateDataTable<SolicitacaoExameItemList, DetalhamentoExameInput>()
                       .AddDefaultField(defaultField)
                       .AddSelectClause(selectClause)
                       .AddFromClause(fromClause)
                       .AddWhereClause(whereClause)
                       .AddOrderByClause("LabResultado.DataColeta DESC")
                       .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                       .ExecuteAsync(input).ConfigureAwait(false);
            }
        }


        public async Task<long> TotalExamesResultados(long id, string tipo)
        {
            using (var conn = new SqlConnection(this.GetConnection()))
            {
                if (tipo == "Lab")
                {
                    return await conn.ExecuteScalarAsync<long>($@"
                                SELECT COUNT(LabResultadoExame.Id) AS Total
                                FROM  LabResultadoExame
                                    INNER JOIN LabResultado ON LabResultado.Id = LabResultadoExame.LabResultadoId
                                WHERE
                                   LabResultadoExame.ExameStatusId IN({ (int)EnumStatusExame.Digitado},{ (int)EnumStatusExame.Conferido})
                                AND
                                    LabResultadoExame.IsDeleted = 0
                                    AND LabResultado.AteAtendimentoId = @atendimentoId", new { atendimentoId = id });
                }

                if (tipo == "Img")
                {
                    return await conn.ExecuteScalarAsync<long>($@"SELECT
                                    COUNT(LauMovimentoItem.Id) AS Total
                                FROM LauMovimentoItem
                                    INNER JOIN LauMovimento ON LauMovimento.Id = LauMovimentoItem.LauMovimentoId
                                WHERE
                                    --LauMovimentoItem.Status IN({ (int)EnumStatusLaudo.LaudoRevisado},{ (int)EnumStatusLaudo.ComLaudo}) // Marcio Pediu para tirar por enquanto.
                                    LauMovimentoItem.IsDeleted = 0
                                    AND LauMovimento.AtendimentoId = @atendimentoId", new { atendimentoId = id });
                }


                return 0;
            }
        }

        public async Task<long> TotalExamesSolicitados(long id, string tipo)
        {
            using (var conn = new SqlConnection(this.GetConnection()))
            {
                if (tipo == "Lab")
                {
                    return await conn.ExecuteScalarAsync<long>($@"
                                SELECT
                                    COUNT(AssSolicitacaoExameItem.Id) AS Total
                                FROM AssSolicitacaoExameItem
                                    INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id
                                    INNER JOIN AteAtendimento ON AssSolicitacaoExame.AtendimentoId = AteAtendimento.Id
                                    INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                                    LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                                    WHERE AssSolicitacaoExame.IsDeleted = 0 
                                    AND (FatItem.IsLaboratorio = 1 OR FatGrupo.IsLaboratorio = 1)
                                    AND AssSolicitacaoExame.AtendimentoId = @atendimentoId", new { atendimentoId = id });
                }

                if (tipo == "Img")
                {
                    return await conn.ExecuteScalarAsync<long>($@"SELECT
                                    COUNT(AssSolicitacaoExameItem.Id) AS Total
                                    FROM AssSolicitacaoExameItem
                                    INNER JOIN AssSolicitacaoExame ON AssSolicitacaoExameItem.AssSolicitacaoExameId = AssSolicitacaoExame.Id
                                    INNER JOIN FatItem ON FatItem.Id = AssSolicitacaoExameItem.FatItemId
                                    LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                                    WHERE AssSolicitacaoExame.IsDeleted = 0 
                                    AND (FatItem.IsLaudo = 1 OR FatGrupo.IsLaudo = 1)
                                    AND AssSolicitacaoExame.AtendimentoId = @atendimentoId", new { atendimentoId = id });
                }


                return 0;
            }
        }



        private string ExecutaFiltroInternacao(ListarAtendimentosInput input, Dictionary<string, object> dapperParameters)
        {
            dapperParameters.Add("Vermelho", "#f90320");
            dapperParameters.Add("Amarelo", "#f1f904");
            dapperParameters.Add("Verde", "#09e226");

            dapperParameters.Add(
                "ProcedimentoItemStatus",
                new List<long>
                    {
                        (long)EnumStatusSolicitacao.Autorizado, (long)EnumStatusSolicitacao.AutorizadoParcialmente
                    });


            return ExecutaFiltroAtendimento(input, dapperParameters);
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<AtendimentoDto>> ListarFiltroPreAtendimento(ListarAtendimentosInput input)
        {

            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).Include(m => m.Medico).Include(m => m.Medico.SisPessoa)
                        .Include(m => m.AtendimentoTipo).Include(m => m.Convenio).Include(m => m.Convenio.SisPessoa)
                        .Include(m => m.Empresa).Include(m => m.Especialidade).Include(m => m.Guia)
                        .Include(m => m.Leito).Include(m => m.Leito.TipoAcomodacao).Include(m => m.MotivoAlta)
                        .Include(m => m.Nacionalidade).Include(m => m.Origem).Include(m => m.Plano)
                        .Include(m => m.ServicoMedicoPrestado).Include(m => m.UnidadeOrganizacional)
                        .Where(a => a.IsPreatendimento == true).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Paciente.NomeCompleto.Contains(input.Filtro)
                                 || m.UnidadeOrganizacional.Descricao.Contains(input.Filtro)
                                 || m.Codigo.Contains(input.Filtro)
                                 || m.Paciente.CodigoPaciente.ToString().Contains(input.Filtro)
                                 || m.Convenio.NomeFantasia.Contains(input.Filtro)
                                 || m.Medico.NomeCompleto.Contains(input.Filtro.ToUpper())
                                 || m.Leito.TipoAcomodacao.Descricao.ToUpper().Contains(input.Filtro)
                                 || m.Leito.Descricao.Contains(input.Filtro)
                                 || m.Empresa.NomeFantasia.Contains(input.Filtro)
                                 || m.AtendimentoTipo.Descricao.Contains(input.Filtro)
                                 || m.Observacao.Contains(input.Filtro));

                    var contarAtendimentos = await query.CountAsync().ConfigureAwait(false);
                    var atendimentosDto = AtendimentoDto.Mapear(await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false)).ToList();

                    return new PagedResultDto<AtendimentoDto>(contarAtendimentos, atendimentosDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<AtendimentoDto>> ListarPorPaciente(ListarInput input)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).Include(m => m.Medico).Include(m => m.Medico.SisPessoa)
                        .Include(m => m.AtendimentoTipo).Include(m => m.Convenio).Include(m => m.Convenio.SisPessoa)
                        .Include(m => m.Empresa).Include(m => m.Especialidade).Include(m => m.Guia)
                        .Include(m => m.Leito).Include(m => m.MotivoAlta).Include(m => m.Nacionalidade)
                        .Include(m => m.Origem).Include(m => m.Plano).Include(m => m.ServicoMedicoPrestado)
                        .Include(m => m.UnidadeOrganizacional).Where(m => m.PacienteId.Equals(input.Filtro))
                        .OrderByDescending(m => m.DataRegistro);

                    var contarAtendimentos = await query.CountAsync().ConfigureAwait(false);

                    var atendimentosDto = AtendimentoDto.Mapear(
                        await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false)).ToList();

                    return new PagedResultDto<AtendimentoDto>(contarAtendimentos, atendimentosDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<FileDto> ListarParaExcel(ListarAtendimentosInput input)
        {
            try
            {
                using (var listarAtendimentosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarAtendimentosExcelExporter>())
                {
                    // var result = await Listar(input);
                    var result = await this.ListarTodos().ConfigureAwait(false);
                    var atendimentos = result.Items;
                    return listarAtendimentosExcelExporter.Object.ExportToFile(atendimentos.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExportar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public List<Atendimento> ListarUltimos2Atendimentos(long? id)
        {
            dynamic reverseList = null;
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var at = atendimentoRepository.Object.GetAll().AsNoTracking().Where(a => a.PacienteId == id).ToList();
                    if (at != null && at.Any())
                    {
                        reverseList = at.OrderByDescending(o => o.DataRegistro).Take(2).ToList();
                    }

                    return reverseList;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }





        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<AtendimentoDto> ObterAtendimentoParaPrescricao(long id)
        {
            try
            {
                using (var atendimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var senhaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Senha, long>>())
                {
                    var m2 = atendimentoRepository.Object.GetAll().Include(a => a.Paciente)
                        .Include(a => a.Paciente.SisPessoa).Include(a => a.Paciente.SisPessoa.Sexo)
                        //.Include(a => a.Paciente.SisPessoa.Nacionalidade)
                        .Include(a => a.Paciente.PacientePesos).Include(a => a.Paciente.PacienteDiagnosticos)
                        .Include(a => a.Paciente.PacienteDiagnosticos.Select(x => x.GrupoCID))
                        .Include(a => a.Paciente.PacienteAlergias)

                        // .Include(a => a.Paciente.Sexo)
                        // .Include(a => a.Paciente.SisPessoa.Enderecos)
                        // .Include(a => a.Paciente.Pais)
                        // .Include(a => a.Paciente.Estado)
                        // .Include(a => a.Paciente.Cidade)
                        // .Include(a => a.Paciente.EstadoCivil)
                        // .Include(a => a.Paciente.Profissao)
                        .Include(a => a.Medico).Include(a => a.Medico.Conselho).Include(a => a.Medico.SisPessoa)
                        .Include(a => a.AtendimentoTipo).Include(a => a.Convenio)
                        .Include(a => a.Convenio.IdentificacoesPrestadoresNaOperadora)
                        .Include(a => a.Convenio.SisPessoa).Include(a => a.Empresa).Include(a => a.Especialidade)
                        .Include(a => a.Guia) // modelo antigo
                        .Include(a => a.FatGuia) // novo modelo FatGuia
                        .Include(a => a.TipoAcompanhante).Include(a => a.Leito).Include(a => a.Leito.TipoAcomodacao)
                        .Include(a => a.MotivoAlta).Include(a => a.Nacionalidade).Include(a => a.Origem)
                        .Include(a => a.Plano).Include(a => a.ServicoMedicoPrestado)
                        .Include(a => a.UnidadeOrganizacional).Include(a => a.TipoAcomodacao)
                        .Include(a => a.CaraterAtendimento).Include(a => a.IndicacaoAcidente).AsNoTracking();

                    var result = await m2.Select(
                                     s => new
                                     {
                                         s.Id,
                                         s.Codigo,
                                         s.GuiaNumero,
                                         s.Convenio.SisPessoa.NomeFantasia,
                                         s.EmpresaId,
                                         NomeEmpresa = s.Empresa.NomeFantasia,
                                         s.UnidadeOrganizacionalId,
                                         UnidadeDescricao = s.UnidadeOrganizacional.Descricao,
                                         DataRegistro = s.DataRegistro,
                                         PacienteId = s.Paciente.Id,
                                         PacienteNome = s.Paciente.SisPessoa.NomeCompleto,
                                         PacienteDataNascimento = s.Paciente.SisPessoa.Nascimento,
                                         PacienteFoto = s.Paciente.Foto,
                                         PacienteFotoMimeType = s.Paciente.FotoMimeType,
                                         SexoId = s.Paciente.SisPessoa.SexoId,
                                         SexoDescricao = s.Paciente.SisPessoa.Sexo.Descricao,
                                         //PacientePesos = new s.Paciente.PacientePesos { s.Paciente.PacientePesos }
                                         LeitoId = s.Leito != null ? s.Leito.Id : 0,
                                         LeitoDescricao = s.Leito != null ? s.Leito.Descricao : string.Empty
                                     }).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

                    var atendimento = new Atendimento
                    {
                        Id = result.Id,
                        GuiaNumero = result.GuiaNumero,
                        EmpresaId = result.EmpresaId,
                        Empresa =
                                                  new Empresa
                                                  {
                                                      Id = result.EmpresaId ?? 0,
                                                      NomeFantasia = result.NomeEmpresa
                                                  },
                        UnidadeOrganizacional =
                                                  new UnidadeOrganizacional
                                                  {
                                                      Id = result.UnidadeOrganizacionalId ?? 0,
                                                      Descricao = result.UnidadeDescricao
                                                  },
                        DataRegistro = result.DataRegistro,
                        PacienteId = result.PacienteId,
                        Paciente = new Paciente
                        {
                            Id = result.PacienteId,
                            NomeCompleto = result.PacienteNome,
                            Foto = result.PacienteFoto,
                            FotoMimeType = result.PacienteFotoMimeType,
                            SexoId = result.SexoId,
                            Sexo = new Sexo
                            {
                                Id = result.SexoId ?? 0,
                                Descricao = result.SexoDescricao
                            }
                        },
                        LeitoId = result.LeitoId != 0 ? result.LeitoId : (long?)null,
                        Leito = result.LeitoId != 0
                                                          ? new Leito
                                                          {
                                                              Id = result.LeitoId,
                                                              Descricao = result.LeitoDescricao
                                                          }
                                                          : null
                    };



                    var atendimentoDto = AtendimentoDto.Mapear(atendimento);

                    var senhaAtendimento = await senhaRepository.Object.GetAll().AsNoTracking()
                                               .FirstOrDefaultAsync(w => w.AtendimentoId == atendimentoDto.Id)
                                               .ConfigureAwait(false);

                    if (senhaAtendimento != null)
                    {
                        atendimentoDto.SenhaAtendimento = SenhaDto.Mapear(senhaAtendimento);
                    }

                    return atendimentoDto;
                }
            }

            // catch (DbEntityValidationException e)
            // {
            // foreach (var eve in e.EntityValidationErrors)
            // {
            // Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            // eve.Entry.Entity.GetType().Name, eve.Entry.State);
            // foreach (var ve in eve.ValidationErrors)
            // {
            // Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            // ve.PropertyName, ve.ErrorMessage);
            // }
            // }
            // throw;
            // }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }



        //[UnitOfWork(false)]
        //[DisableAuditing]
        //public async Task<AtendimentoDto> Obter(long id)
        //{
        //    try
        //    {
        //        using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
        //        using (var senhaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Senha, long>>())
        //        {
        //            var m = await atendimentoRepository.Object.GetAll()
        //                        .Include(a => a.Paciente) // OK
        //                        .Include(a => a.Paciente.SisPessoa) // OK
        //                        .Include(a => a.Paciente.SisPessoa.Sexo) // OK
        //                        .Include(a => a.Paciente.SisPessoa.Nacionalidade) // OK

        //                        .Include(a => a.Paciente.PacientePesos) // Pendente Lista
        //                        .Include(a => a.Paciente.PacienteDiagnosticos) // Pendente Lista
        //                        .Include(a => a.Paciente.PacienteDiagnosticos.Select(x => x.GrupoCID)) // Pendente Lista
        //                        .Include(a => a.Paciente.PacienteAlergias) // Pendente Lista

        //                        .Include(a => a.Medico) // OK
        //                        .Include(a => a.Medico.Conselho) // OK
        //                        .Include(a => a.Medico.SisPessoa) // OK
        //                        .Include(a => a.AtendimentoTipo) // OK
        //                        .Include(a => a.Convenio) // OK
        //                        .Include(a => a.Convenio.IdentificacoesPrestadoresNaOperadora) // Pendente Lista
        //                        .Include(a => a.Convenio.SisPessoa) // OK 
        //                        .Include(a => a.Empresa)
        //                        .Include(a => a.Especialidade) // OK
        //                        .Include(a => a.Guia) // modelo antigo
        //                        .Include(a => a.FatGuia) // novo modelo FatGuia
        //                        .Include(a => a.TipoAcompanhante) // OK
        //                        .Include(a => a.Leito) // OK
        //                        .Include(a => a.Leito.TipoAcomodacao) // OK
        //                        .Include(a => a.MotivoAlta) // OK
        //                        .Include(a => a.Nacionalidade) // OK
        //                        .Include(a => a.Origem) // OK
        //                        .Include(a => a.Plano) // OK
        //                        .Include(a => a.ServicoMedicoPrestado) // OK
        //                        .Include(a => a.UnidadeOrganizacional) // OK
        //                        .Include(a => a.TipoAcomodacao)
        //                        .Include(a => a.CaraterAtendimento) // OK
        //                        .Include(a => a.IndicacaoAcidente) // OK 
        //                        .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id)
        //                        .ConfigureAwait(false);

        //            var atendimento = AtendimentoDto.Mapear(m);

        //            var senhaAtendimento = await senhaRepository.Object.GetAll().AsNoTracking()
        //                                       .FirstOrDefaultAsync(w => w.AtendimentoId == atendimento.Id)
        //                                       .ConfigureAwait(false);

        //            if (senhaAtendimento != null)
        //            {
        //                atendimento.SenhaAtendimento = SenhaDto.Mapear(senhaAtendimento);
        //            }

        //            return atendimento;
        //        }
        //    }

        //    // catch (DbEntityValidationException e)
        //    // {
        //    // foreach (var eve in e.EntityValidationErrors)
        //    // {
        //    // Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //    // eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //    // foreach (var ve in eve.ValidationErrors)
        //    // {
        //    // Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //    // ve.PropertyName, ve.ErrorMessage);
        //    // }
        //    // }
        //    // throw;
        //    // }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
        //    }
        //}


        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<AtendimentoDto> Obter(long id)
        {
            try
            {
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    var atendimentoDto = (await connection.QueryAsync(
                                             QueryAtendimentoObter,
                                             types: new[]
                                                        {
                                                            typeof(Atendimento), // Atendimento - 0
                                                            typeof(Paciente), // Paciente - 1
                                                            typeof(SisPessoa), // Paciente SisPessoa - 2
                                                            typeof(Sexo), // Paciente SisPessoa Sexo - 3
                                                            typeof(Nacionalidade), // Paciente SisPessoa Nacionalidade - 4,
                                                            typeof(Medico), // Medico - 5
                                                            typeof(SisPessoa), // Medico SisPessoa - 6
                                                            typeof(Conselho), // Medico Conselho - 7
                                                            typeof(Convenio), // Convenio - 8
                                                            typeof(SisPessoa), // Convenio SisPessoa - 9
                                                            typeof(Plano), // Plano 10
                                                            typeof(Leito), // Leito - 11
                                                            typeof(TipoAcomodacao), // Leito TipoAcomodacao - 12
                                                            typeof(UnidadeOrganizacional), // UnidadeOrganizacional - 13
                                                            typeof(Empresa), // Empresa - 14
                                                            typeof(TipoAtendimento), // TipoAtendimento - 15
                                                            typeof(TabelaDominio), // TipoAtendimento TabelaDominio - 16
                                                            typeof(TipoAcomodacao), // TipoAcomodacao - 17
                                                            typeof(TipoAcompanhante), // TipoAcompanhante - 18
                                                            typeof(Especialidade), // Especialidade - 19
                                                            typeof(MotivoAlta), // MotivoAlta - 20,
                                                            typeof(Origem),  // Origem - 21
                                                            typeof(ServicoMedicoPrestado), // ServicoMedicoPrestado - 22
                                                            typeof(Nacionalidade), // Nacionalidade - 23
                                                            typeof(TabelaDominio), // Carater Atendimento - 24
                                                            typeof(TabelaDominio), // IndicacaoAcidente - 25
                                                            typeof(Guia), // Guia - 26
                                                            typeof(FaturamentoGuia), // FatGuia 27,
                                                            typeof(GrupoCID),  // GrupoCID 28,
                                                        },
                                            map: objects =>
                                                {
                                                    var atendimento = objects[0] as Atendimento;

                                                    if (atendimento == null)
                                                    {
                                                        return AtendimentoDto.Mapear(atendimento);
                                                    }

                                                    atendimento.Paciente = (objects[1] as Paciente) ?? new Paciente();

                                                    atendimento.Paciente.SisPessoa = (objects[2] as SisPessoa) ?? new SisPessoa();
                                                    atendimento.Paciente.SisPessoa.Sexo = (objects[3] as Sexo) ?? new Sexo();

                                                    atendimento.Paciente.Nacionalidade = (objects[4] as Nacionalidade) ?? new Nacionalidade();

                                                    atendimento.Medico = (objects[5] as Medico) ?? new Medico();
                                                    atendimento.Medico.SisPessoa = (objects[6] as SisPessoa) ?? new SisPessoa();
                                                    atendimento.Medico.Conselho = (objects[7] as Conselho) ?? new Conselho();

                                                    atendimento.Convenio = (objects[8] as Convenio) ?? new Convenio();
                                                    atendimento.Convenio.SisPessoa = (objects[9] as SisPessoa) ?? new SisPessoa();
                                                    atendimento.Plano = (objects[10] as Plano) ?? new Plano();

                                                    atendimento.Leito = (objects[11] as Leito) ?? new Leito();
                                                    atendimento.Leito.TipoAcomodacao = (objects[12] as TipoAcomodacao) ?? new TipoAcomodacao();

                                                    atendimento.UnidadeOrganizacional = (objects[13] as UnidadeOrganizacional) ?? new UnidadeOrganizacional();

                                                    atendimento.Empresa = (objects[14] as Empresa) ?? new Empresa();

                                                    atendimento.AtendimentoTipo = (objects[15] as TipoAtendimento) ?? new TipoAtendimento();

                                                    atendimento.AtendimentoTipo.TabelaDominio = (objects[16] as TabelaDominio) ?? new TabelaDominio();

                                                    atendimento.TipoAcomodacao = (objects[17] as TipoAcomodacao) ?? new TipoAcomodacao();

                                                    atendimento.TipoAcompanhante = (objects[18] as TipoAcompanhante) ?? new TipoAcompanhante();

                                                    atendimento.Especialidade = (objects[19] as Especialidade) ?? new Especialidade();

                                                    atendimento.MotivoAlta = (objects[20] as MotivoAlta) ?? new MotivoAlta();

                                                    atendimento.Origem = (objects[21] as Origem) ?? new Origem();

                                                    atendimento.ServicoMedicoPrestado = (objects[22] as ServicoMedicoPrestado) ?? new ServicoMedicoPrestado();

                                                    atendimento.Nacionalidade = (objects[23] as Nacionalidade) ?? new Nacionalidade();

                                                    atendimento.CaraterAtendimento = (objects[24] as TabelaDominio) ?? new TabelaDominio();

                                                    atendimento.IndicacaoAcidente = (objects[25] as TabelaDominio) ?? new TabelaDominio();

                                                    atendimento.Guia = (objects[26] as Guia) ?? new Guia();
                                                    atendimento.FatGuia = (objects[27] as FaturamentoGuia) ?? new FaturamentoGuia();

                                                    atendimento.AltaGrupoCID = (objects[28] as GrupoCID) ?? new GrupoCID();

                                                    return AtendimentoDto.Mapear(atendimento);
                                                }, new { Deleted = false, atendimentoId = id }).ConfigureAwait(false)).FirstOrDefault();

                    if (atendimentoDto == null || atendimentoDto.Id == 0)
                    {
                        return atendimentoDto;
                    }

                    using (var reader = await connection.QueryMultipleAsync(QueryListasAtendimentoObter, new { atendimentoDto.PacienteId, IsDeleted = false, atendimentoDto.ConvenioId }).ConfigureAwait(false))
                    {
                        if (atendimentoDto.Paciente == null)
                        {
                            atendimentoDto.Paciente = new PacienteDto();
                        }

                        if (atendimentoDto.Convenio == null)
                        {
                            atendimentoDto.Convenio = new ConvenioDto();
                        }

                        atendimentoDto.Paciente.PacientePesos = (await reader.ReadAsync<PacientePesoDto>().ConfigureAwait(false)).ToList();

                        atendimentoDto.Paciente.PacienteAlergias = (await reader.ReadAsync<PacienteAlergias>().ConfigureAwait(false))
                           .Select(x => PacienteAlergiasDto.Mapear(x))
                           .ToList();

                        atendimentoDto.Paciente.PacienteDiagnosticos = (await reader.ReadAsync<PacienteDiagnosticos>().ConfigureAwait(false))
                            .Select(x => PacienteDiagnosticosDto.Mapear(x))
                            .ToList();

                        atendimentoDto.Convenio.IdentificacoesPrestadoresNaOperadoraDto = (await reader.ReadAsync<IdentificacaoPrestadorNaOperadora>().ConfigureAwait(false))
                            .Select(x => IdentificacaoPrestadorNaOperadoraDto.Mapear(x))
                            .ToList();
                    }
                    return atendimentoDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        private const string QueryAtendimentoObter = @"
            SELECT 
            -- Atendimento 0
            Atendimento.[Id] AS [Id],
            Atendimento.[Codigo] AS [Codigo],
            Atendimento.[Descricao] AS [Descricao],
            Atendimento.[IsDeleted] AS [IsDeleted],
            Atendimento.[DeleterUserId] AS [DeleterUserId],
            Atendimento.[DeletionTime] AS [DeletionTime],
            Atendimento.[CreatorUserId] AS [CreatorUserId],
            Atendimento.[CreationTime] AS [CreationTime],
            Atendimento.[IsSistema] AS [IsSistema],
            Atendimento.LastModificationTime AS LastModificationTime,
            Atendimento.LastModifierUserId AS LastModifierUserId,
            Atendimento.ImportaId AS ImportaId,
            Atendimento.GuiaNumero AS GuiaNumero,
            Atendimento.NumeroGuia AS NumeroGuia,
            Atendimento.Matricula AS Matricula,
            Atendimento.Responsavel AS Responsavel,
            Atendimento.RgResponsavel AS RgResponsavel,
            Atendimento.CpfResponsavel AS CpfResponsavel,
            Atendimento.QtdSessoes AS QtdSessoes,
            Atendimento.DataRetorno AS DataRetorno,
            Atendimento.DataRevisao AS DataRevisao,
            Atendimento.DataPreatendimento AS DataPreatendimento,
            Atendimento.DataPrevistaAtendimento AS DataPrevistaAtendimento,
            Atendimento.DataRegistro AS DataRegistro,
            Atendimento.DataAlta AS DataAlta,
            Atendimento.DataPrevistaAlta AS DataPrevistaAlta,
            Atendimento.DataAltaMedica AS DataAltaMedica,
            Atendimento.NumeroObito AS NumeroObito,
            Atendimento.ValidadeCarteira AS ValidadeCarteira,
            Atendimento.ValidadeSenha AS ValidadeSenha,
            Atendimento.DataAutorizacao AS DataAutorizacao,
            Atendimento.DiasAutorizacao AS DiasAutorizacao,
            Atendimento.DataUltimoPagamento AS DataUltimoPagamento,
            Atendimento.DataTomadaDecisao AS DataTomadaDecisao,
            Atendimento.Senha AS Senha,
            Atendimento.Parentesco AS Parentesco,
            Atendimento.Titular AS Titular,
            Atendimento.Observacao AS Observacao,
            Atendimento.IsAmbulatorioEmergencia AS IsAmbulatorioEmergencia,
            Atendimento.IsInternacao AS IsInternacao,
            Atendimento.IsHomeCare AS IsHomeCare,
            Atendimento.IsPreatendimento AS IsPreatendimento,
            Atendimento.IsControlaTev AS IsControlaTev,
            Atendimento.CNS AS CNS,
            Atendimento.CodDependente AS CodDependente,
            Atendimento.OrigemTitular AS OrigemTitular,
            Atendimento.IsPendenteExames As IsPendenteExames,
            Atendimento.IsPendenteMedicacao AS IsPendenteMedicacao,
            Atendimento.IsPendenteProcedimento AS IsPendenteProcedimento,
            Atendimento.IsAtendidoInternado AS IsAtendidoInternado,
            Atendimento.IsAtendidoAlta AS IsAtendidoAlta,
            Atendimento.IsAtendidoAguardandoInternacao AS IsAtendidoAguardandoInternacao,
            Atendimento.StatusAguardando AS StatusAguardando,
            Atendimento.StatusAtendido AS StatusAtendido,
            Atendimento.AteAtendimentoStatusId AS AtendimentoStatusId,
            Atendimento.AtendimentoOrigemId AS AtendimentoOrigemId,
            Atendimento.AteGrupoCIDId AS AltaGrupoCIDId,
            Atendimento.AteMotivoAltaId AS MotivoAltaId,
            Atendimento.AteTipoAcompanhanteId AS TipoAcompanhanteId,
            Atendimento.AteLeitoId AS LeitoId,
            -- Guia
            Atendimento.SisGuiaId AS GuiaId,
            -- FatGuia
            Atendimento.FatGuiaId FatGuiaId,
            Atendimento.SisServicoMedicoPrestadoId AS ServicoMedicoPrestadoId,
            Atendimento.SisNacionalidadeResponsavelId AS NacionalidadeResponsavelId,
            Atendimento.AtendimentoMotivoCancelamentoId AS AtendimentoMotivoCancelamentoId,
            Atendimento.CaraterAtendimentoId AS CaraterAtendimentoId,
            Atendimento.IndicacaoAcidenteId AS IndicacaoAcidenteId,
            Atendimento.SisUnidadeOrganizacionalId AS UnidadeOrganizacionalId,
            Atendimento.SisPacienteId AS PacienteId,
            Atendimento.SisOrigemId AS OrigemId,
            Atendimento.SisMedicoId AS MedicoId,
            Atendimento.SisEspecialidadeId AS EspecialidadeId,
            Atendimento.SisEmpresaId AS EmpresaId,
            Atendimento.SisConveniolId AS ConvenioId,
            Atendimento.SisPlanoId AS PlanoId,
            Atendimento.SisTipoAcomodacaoId AS TipoAcomodacaoId,
            Atendimento.SisUnidadeOrganizacionalId AS UnidadeOrganizacionalId,
            Atendimento.SisAtendimentoTipoId AS AtendimentoTipoId,
            
            --Paciente 1
            Paciente.Id AS Id,
            Paciente.Codigo AS Codigo,
            Paciente.Descricao AS Descricao,
            Paciente.IsDeleted AS IsDeleted,
            Paciente.DeleterUserId AS DeleterUserId,
            Paciente.DeletionTime AS DeletionTime,
            Paciente.CreatorUserId AS CreatorUserId,
            Paciente.CreationTime AS CreationTime,
            Paciente.IsSistema AS IsSistema,
            Paciente.LastModificationTime AS LastModificationTime,
            Paciente.LastModifierUserId AS LastModifierUserId,
            Paciente.ImportaId AS ImportaId,
            Paciente.CodigoPaciente AS CodigoPaciente,
            Paciente.Prontuario AS Prontuario,
            Paciente.Observacao AS Observacao,
            Paciente.IsDoador AS IsDoador,
            Paciente.Cns AS Cns,
            Paciente.Indicacao AS Indicacao,
            Paciente.SisTipoSanguineoId AS TipoSanguineoId,
            Paciente.SisPessoaId AS SisPessoaId,

            -- SisPessoa 2
            SisPessoaPaciente.Id AS Id,
            SisPessoaPaciente.Codigo AS Codigo,
            SisPessoaPaciente.Descricao AS Descricao,
            SisPessoaPaciente.IsDeleted AS IsDeleted,
            SisPessoaPaciente.DeleterUserId AS DeleterUserId,
            SisPessoaPaciente.DeletionTime AS DeletionTime,
            SisPessoaPaciente.CreatorUserId AS CreatorUserId,
            SisPessoaPaciente.CreationTime AS CreationTime,
            SisPessoaPaciente.IsSistema AS IsSistema,
            SisPessoaPaciente.LastModificationTime AS LastModificationTime,
            SisPessoaPaciente.LastModifierUserId AS LastModifierUserId,
            SisPessoaPaciente.ImportaId AS ImportaId,
            SisPessoaPaciente.NomeCompleto AS NomeCompleto,
            SisPessoaPaciente.Foto AS Foto,
            SisPessoaPaciente.FotoMimeType AS FotoMimeType,
            SisPessoaPaciente.FisicaJuridica AS FisicaJuridica,
            SisPessoaPaciente.IsAtivo AS IsAtivo,
            SisPessoaPaciente.ImportacaoId AS ImportacaoId,
            SisPessoaPaciente.ImportacaoTabela AS ImportacaoTabela,
            SisPessoaPaciente.IsCredito AS IsCredito,
            SisPessoaPaciente.IsDebito AS IsDebito,
            SisPessoaPaciente.Observacao AS Observacao,
            SisPessoaPaciente.Rg AS Rg,
            SisPessoaPaciente.Emissor AS Emissor,
            SisPessoaPaciente.EmissaoRg AS EmissaoRg,
            SisPessoaPaciente.Nascimento AS Nascimento,
            SisPessoaPaciente.Cpf AS Cpf,
            SisPessoaPaciente.NomeMae AS NomeMae,
            SisPessoaPaciente.NomePai AS NomePai,
            SisPessoaPaciente.RazaoSocial AS RazaoSocial,
            SisPessoaPaciente.NomeFantasia AS NomeFantasia,
            SisPessoaPaciente.Cnpj AS Cnpj,
            SisPessoaPaciente.InscricaoEstadual AS InscricaoEstadual,
            SisPessoaPaciente.InscricaoMunicipal AS InscricaoMunicipal,
            SisPessoaPaciente.Telefone1 AS Telefone1,
            SisPessoaPaciente.DddTelefone1 AS DddTelefone1,
            SisPessoaPaciente.Telefone2 AS Telefone2,
            SisPessoaPaciente.DddTelefone2 AS DddTelefone2,
            SisPessoaPaciente.Telefone3 AS Telefone3,
            SisPessoaPaciente.DddTelefone3 AS DddTelefone3,
            SisPessoaPaciente.Telefone4 AS Telefone4,
            SisPessoaPaciente.DddTelefone4 AS DddTelefone4,
            SisPessoaPaciente.Email AS Email,
            SisPessoaPaciente.Email2 AS Email2,
            SisPessoaPaciente.Email3 AS Email3,
            SisPessoaPaciente.Email4 AS Email4,
            SisPessoaPaciente.TipoLogradouroId AS TipoLogradouroId,
            SisPessoaPaciente.TipoSanguineoId AS TipoSanguineoId,
            SisPessoaPaciente.ReligiaoId AS ReligiaoId,
            SisPessoaPaciente.EstadoCivilId AS EstadoCivilId,
            SisPessoaPaciente.EscolaridadeId AS EscolaridadeId,
            SisPessoaPaciente.NacionalidadeId AS NacionalidadeId,
            SisPessoaPaciente.NaturalidadeId AS NaturalidadeId,
            SisPessoaPaciente.TipoPessoaId AS TipoPessoaId,
            SisPessoaPaciente.ProfissaoId AS ProfissaoId,
            SisPessoaPaciente.TipoTelefone1Id AS TipoTelefone1Id,
            SisPessoaPaciente.TipoTelefone2Id AS TipoTelefone2Id,
            SisPessoaPaciente.TipoTelefone3Id AS TipoTelefone3Id,
            SisPessoaPaciente.TipoTelefone4Id AS TipoTelefone4Id,
            SisPessoaPaciente.SexoId AS SexoId,    
            -- SisPessoa Sexo 3
            SisSexoPessoa.Id AS Id,
            SisSexoPessoa.Codigo AS Codigo,
            SisSexoPessoa.Descricao AS Descricao,
            SisSexoPessoa.IsDeleted AS IsDeleted,
            SisSexoPessoa.DeleterUserId AS DeleterUserId,
            SisSexoPessoa.DeletionTime AS DeletionTime,
            SisSexoPessoa.CreatorUserId AS CreatorUserId,
            SisSexoPessoa.CreationTime AS CreationTime,
            SisSexoPessoa.IsSistema AS IsSistema,
            SisSexoPessoa.LastModificationTime AS LastModificationTime,
            SisSexoPessoa.LastModifierUserId AS LastModifierUserId,
            SisSexoPessoa.ImportaId AS ImportaId,

            -- SisPessoa Nacionalidade 4
            NacionalidadePessoa.Id AS Id,
            NacionalidadePessoa.Codigo AS Codigo,
            NacionalidadePessoa.Descricao AS Descricao,
            NacionalidadePessoa.IsDeleted AS IsDeleted,
            NacionalidadePessoa.DeleterUserId AS DeleterUserId,
            NacionalidadePessoa.DeletionTime AS DeletionTime,
            NacionalidadePessoa.CreatorUserId AS CreatorUserId,
            NacionalidadePessoa.CreationTime AS CreationTime,
            NacionalidadePessoa.IsSistema AS IsSistema,
            NacionalidadePessoa.LastModificationTime AS LastModificationTime,
            NacionalidadePessoa.LastModifierUserId AS LastModifierUserId,
            NacionalidadePessoa.ImportaId AS ImportaId,
            
            -- Medico 5
            Medico.Id AS Id,
            Medico.Codigo AS Codigo,
            Medico.Descricao AS Descricao,
            Medico.IsDeleted AS IsDeleted,
            Medico.DeleterUserId AS DeleterUserId,
            Medico.DeletionTime AS DeletionTime,
            Medico.CreatorUserId AS CreatorUserId,
            Medico.CreationTime AS CreationTime,
            Medico.IsSistema AS IsSistema,
            Medico.LastModificationTime AS LastModificationTime,
            Medico.LastModifierUserId AS LastModifierUserId,
            Medico.ImportaId AS ImportaId,
            Medico.NumeroConselho AS NumeroConselho,
            Medico.AssinaturaDigital AS AssinaturaDigital,
            Medico.AssinaturaDigitalMimeType AS AssinaturaDigitalMimeType,
            Medico.Cns AS Cns,
            Medico.IsAgendaConsulta AS IsAgendaConsulta,
            Medico.IsAgendaCirurgia AS IsAgendaCirurgia,
            Medico.IsAtendimentoConsulta AS IsAtendimentoConsulta,
            Medico.IsAtendimentoCirurgia AS IsAtendimentoCirurgia,
            Medico.IsAtendimentoInternacao AS IsAtendimentoInternacao,
            Medico.IsEspecialista AS IsEspecialista,
            Medico.IsExame AS IsExame,
            Medico.CorAgendamentoConsulta AS CorAgendamentoConsulta,
            Medico.CodigoCredenciamentoConvenio AS CodigoCredenciamentoConvenio,
            Medico.IsAtivo AS IsAtivo,
            Medico.IsCorpoClinico AS IsCorpoClinico,
            Medico.Apelido AS Apelido,
            Medico.SisTipoParticipacaoId AS TipoParticipacaoId,
            Medico.SisTipoVinculoEmpregaticioId AS TipoVinculoEmpregaticioId,
            Medico.IsIndeterminado AS IsIndeterminado,
            Medico.SisPessoaId AS SisPessoaId,
            Medico.SisConselhoId AS ConselhoId,
            
            -- Medico SisPessoa 6
            SisPessoaMedico.Id AS Id,
            SisPessoaMedico.Codigo AS Codigo,
            SisPessoaMedico.Descricao AS Descricao,
            SisPessoaMedico.IsDeleted AS IsDeleted,
            SisPessoaMedico.DeleterUserId AS DeleterUserId,
            SisPessoaMedico.DeletionTime AS DeletionTime,
            SisPessoaMedico.CreatorUserId AS CreatorUserId,
            SisPessoaMedico.CreationTime AS CreationTime,
            SisPessoaMedico.IsSistema AS IsSistema,
            SisPessoaMedico.LastModificationTime AS LastModificationTime,
            SisPessoaMedico.LastModifierUserId AS LastModifierUserId,
            SisPessoaMedico.ImportaId AS ImportaId,
            SisPessoaMedico.NomeCompleto AS NomeCompleto,
            SisPessoaMedico.Foto AS Foto,
            SisPessoaMedico.FotoMimeType AS FotoMimeType,
            SisPessoaMedico.FisicaJuridica AS FisicaJuridica,
            SisPessoaMedico.IsAtivo AS IsAtivo,
            SisPessoaMedico.ImportacaoId AS ImportacaoId,
            SisPessoaMedico.ImportacaoTabela AS ImportacaoTabela,
            SisPessoaMedico.IsCredito AS IsCredito,
            SisPessoaMedico.IsDebito AS IsDebito,
            SisPessoaMedico.Observacao AS Observacao,
            SisPessoaMedico.Rg AS Rg,
            SisPessoaMedico.Emissor AS Emissor,
            SisPessoaMedico.EmissaoRg AS EmissaoRg,
            SisPessoaMedico.Nascimento AS Nascimento,
            SisPessoaMedico.Cpf AS Cpf,
            SisPessoaMedico.NomeMae AS NomeMae,
            SisPessoaMedico.NomePai AS NomePai,
            SisPessoaMedico.RazaoSocial AS RazaoSocial,
            SisPessoaMedico.NomeFantasia AS NomeFantasia,
            SisPessoaMedico.Cnpj AS Cnpj,
            SisPessoaMedico.InscricaoEstadual AS InscricaoEstadual,
            SisPessoaMedico.InscricaoMunicipal AS InscricaoMunicipal,
            SisPessoaMedico.Telefone1 AS Telefone1,
            SisPessoaMedico.DddTelefone1 AS DddTelefone1,
            SisPessoaMedico.Telefone2 AS Telefone2,
            SisPessoaMedico.DddTelefone2 AS DddTelefone2,
            SisPessoaMedico.Telefone3 AS Telefone3,
            SisPessoaMedico.DddTelefone3 AS DddTelefone3,
            SisPessoaMedico.Telefone4 AS Telefone4,
            SisPessoaMedico.DddTelefone4 AS DddTelefone4,
            SisPessoaMedico.Email AS Email,
            SisPessoaMedico.Email2 AS Email2,
            SisPessoaMedico.Email3 AS Email3,
            SisPessoaMedico.Email4 AS Email4,
            SisPessoaMedico.TipoLogradouroId AS TipoLogradouroId,
            SisPessoaMedico.TipoSanguineoId AS TipoSanguineoId,
            SisPessoaMedico.ReligiaoId AS ReligiaoId,
            SisPessoaMedico.EstadoCivilId AS EstadoCivilId,
            SisPessoaMedico.EscolaridadeId AS EscolaridadeId,
            SisPessoaMedico.NacionalidadeId AS NacionalidadeId,
            SisPessoaMedico.NaturalidadeId AS NaturalidadeId,
            SisPessoaMedico.TipoPessoaId AS TipoPessoaId,
            SisPessoaMedico.ProfissaoId AS ProfissaoId,
            SisPessoaMedico.TipoTelefone1Id AS TipoTelefone1Id,
            SisPessoaMedico.TipoTelefone2Id AS TipoTelefone2Id,
            SisPessoaMedico.TipoTelefone3Id AS TipoTelefone3Id,
            SisPessoaMedico.TipoTelefone4Id AS TipoTelefone4Id,

            -- Conselho 7
            ConselhoMedico.Id AS Id,
            ConselhoMedico.Codigo AS Codigo,
            ConselhoMedico.Descricao AS Descricao,
            ConselhoMedico.IsDeleted AS IsDeleted,
            ConselhoMedico.DeleterUserId AS DeleterUserId,
            ConselhoMedico.DeletionTime AS DeletionTime,
            ConselhoMedico.CreatorUserId AS CreatorUserId,
            ConselhoMedico.CreationTime AS CreationTime,
            ConselhoMedico.IsSistema AS IsSistema,
            ConselhoMedico.LastModificationTime AS LastModificationTime,
            ConselhoMedico.LastModifierUserId AS LastModifierUserId,
            ConselhoMedico.ImportaId AS ImportaId,
            ConselhoMedico.Sigla AS Sigla,
            ConselhoMedico.Uf AS Uf,
            ConselhoMedico.NomeEStado AS NomeEstado,
            ConselhoMedico.TabelaItemTissId AS TabelaItemTissId,

            -- Convenio 8
            Convenio.Id AS Id,
            Convenio.Codigo AS Codigo,
            Convenio.Descricao AS Descricao,
            Convenio.IsDeleted AS IsDeleted,
            Convenio.DeleterUserId AS DeleterUserId,
            Convenio.DeletionTime AS DeletionTime,
            Convenio.CreatorUserId AS CreatorUserId,
            Convenio.CreationTime AS CreationTime,
            Convenio.IsSistema AS IsSistema,
            Convenio.LastModificationTime AS LastModificationTime,
            Convenio.LastModifierUserId AS LastModifierUserId,
            Convenio.ImportaId AS ImportaId,
            Convenio.Nome AS Nome,
            Convenio.IsAtivo AS IsAtivo,
            Convenio.Logotipo AS Logotipo,
            Convenio.LogotipoMimeType AS LogotipoMimeType,
            Convenio.IsFilotranpica AS IsFilotranpica,
            Convenio.LogradouroCobranca AS LogradouroCobranca,
            Convenio.SisCepCobrancaId AS CepCobrancaId,
            Convenio.SisTipoLogradouroCobrancaId AS TipoLogradouroCobrancaId,
            Convenio.NumeroCobranca AS NumeroCobranca,
            Convenio.ComplementoCobranca AS ComplementoCobranca,
            Convenio.BairroCobranca AS BairroCobranca,
            Convenio.SisCidadeCobrancaId AS CidadeCobrancaId,
            Convenio.SisEstadoCobrancaId AS EstadoCobrancaId,
            Convenio.Cargo AS Cargo,
            Convenio.DataInicioContrato AS DataInicioContrato,
            Convenio.Vigencia AS Vigencia,
            Convenio.DataProximaRenovacaoContratual AS DataProximaRenovacaoContratual,
            Convenio.DataInicialContrato AS DataInicialContrato,
            Convenio.DataUltimaRenovacaoContrato AS DataUltimaRenovacaoContrato,
            Convenio.RegistroANS AS RegistroANS,
            Convenio.VersaoTissId AS VersaoTissId,
            Convenio.IsPreencheCodigoCredenciadoCodigoOperadora AS IsPreencheCodigoCredenciadoCodigoOperadora,
            Convenio.IsImprimeTratamento AS IsImprimeTratamento,
            Convenio.IsImprimeObsConta AS IsImprimeObsConta,
            Convenio.IsAgrupaGuiaXml AS IsAgrupaGuiaXml,
            Convenio.Is09e10 AS Is09e10,
            Convenio.IsFatorMultiplicador AS IsFatorMultiplicador,
            Convenio.IsEquipeMedicaBranco AS IsEquipeMedicaBranco,
            Convenio.IsObrigaEspecialidade AS IsObrigaEspecialidade,
            Convenio.Is41a45BrancoPJ AS Is41a45BrancoPJ,
            Convenio.IsSomarFilmeTaxa AS IsSomarFilmeTaxa,
            Convenio.IsImprimeObsContaGuiaConsulta AS IsImprimeObsContaGuiaConsulta,
            Convenio.IsImportaAgudaCronica AS IsImportaAgudaCronica,
            Convenio.Is38e39GuiaConsulta AS Is38e39GuiaConsulta,
            Convenio.Is86e89GuiaSPSADT AS Is86e89GuiaSPSADT,
            Convenio.IsFilmeGuiaOutrasDespesas AS IsFilmeGuiaOutrasDespesas,
            Convenio.Is22GuiaSPSADT AS Is22GuiaSPSADT,
            Convenio.XmlUltimosDigitosMatricula AS XmlUltimosDigitosMatricula,
            Convenio.XmlPrimeirosDigitosMatricula AS XmlPrimeirosDigitosMatricula,
            Convenio.IsEletivo AS IsEletivo,
            Convenio.IsUrgencia AS IsUrgencia,
            Convenio.VerificaElegibilidadeHomologacao AS VerificaElegibilidadeHomologacao,
            Convenio.ComunicacaoBeneficiarioHomologacao AS ComunicacaoBeneficiarioHomologacao,
            Convenio.CancelaGuiaHomologacao AS CancelaGuiaHomologacao,
            Convenio.SolicitacaoProcedimentoHomologacao AS SolicitacaoProcedimentoHomologacao,
            Convenio.SolicitastatusAutorizacaoHomologacao AS SolicitastatusAutorizacaoHomologacao,
            Convenio.LoteAnexoHomologacao AS LoteAnexoHomologacao,
            Convenio.LoteGuiasHomologacao AS LoteGuiasHomologacao,
            Convenio.SolicitaStatusProtocoloHomologacao AS SolicitaStatusProtocoloHomologacao,
            Convenio.SolicitacaoDemonstrativoRetornoHomologacao AS SolicitacaoDemonstrativoRetornoHomologacao,
            Convenio.RecursoGlosaHomologacao AS RecursoGlosaHomologacao,
            Convenio.VerificaElegibilidade AS VerificaElegibilidade,
            Convenio.ComunicacaoBeneficiario AS ComunicacaoBeneficiario,
            Convenio.CancelaGuia AS CancelaGuia,
            Convenio.SolicitacaoProcedimento AS SolicitacaoProcedimento,
            Convenio.SolicitastatusAutorizacao AS SolicitastatusAutorizacao,
            Convenio.LoteAnexo AS LoteAnexo,
            Convenio.LoteGuias AS LoteGuias,
            Convenio.SolicitaStatusProtocolo AS SolicitaStatusProtocolo,
            Convenio.SolicitacaoDemonstrativoRetorno AS SolicitacaoDemonstrativoRetorno,
            Convenio.RecursoGlosa AS RecursoGlosa,
            Convenio.WebService AS WebService,
            Convenio.Usuario AS Usuario,
            Convenio.Senha AS Senha,
            Convenio.Homologacao AS Homologacao,
            Convenio.Certificado AS Certificado,
            Convenio.CaCerfts AS CaCerfts,
            Convenio.SenhaCertificado AS SenhaCertificado,
            Convenio.SenhaCacerts AS SenhaCacerts,
            Convenio.IsCaixa AS IsCaixa,
            Convenio.FormaAutorizacaoId AS FormaAutorizacaoId,
            Convenio.DadosContato AS DadosContato,
            Convenio.IsPreencheGuiaAutomaticamente AS IsPreencheGuiaAutomaticamente,
            Convenio.EmpresaPadraoEmergenciaId AS EmpresaPadraoEmergenciaId,
            Convenio.MedicoPadraoEmergenciaId AS MedicoPadraoEmergenciaId,
            Convenio.EspecialidadePadraoEmergenciaId AS EspecialidadePadraoEmergenciaId,
            Convenio.EmpresaPadraoInternacaoId AS EmpresaPadraoInternacaoId,
            Convenio.MedicoPadraoInternacaoId AS MedicoPadraoInternacaoId,
            Convenio.EspecialidadePadraoInternacaoId AS EspecialidadePadraoInternacaoId,
            Convenio.IsParticular AS IsParticular,
            Convenio.SisPessoaId AS SisPessoaId,

            -- Convenio SisPessoaId 9
            SisPessoaConvenio.Id AS Id,
            SisPessoaConvenio.Codigo AS Codigo,
            SisPessoaConvenio.Descricao AS Descricao,
            SisPessoaConvenio.IsDeleted AS IsDeleted,
            SisPessoaConvenio.DeleterUserId AS DeleterUserId,
            SisPessoaConvenio.DeletionTime AS DeletionTime,
            SisPessoaConvenio.CreatorUserId AS CreatorUserId,
            SisPessoaConvenio.CreationTime AS CreationTime,
            SisPessoaConvenio.IsSistema AS IsSistema,
            SisPessoaConvenio.LastModificationTime AS LastModificationTime,
            SisPessoaConvenio.LastModifierUserId AS LastModifierUserId,
            SisPessoaConvenio.ImportaId AS ImportaId,
            SisPessoaConvenio.NomeCompleto AS NomeCompleto,
            SisPessoaConvenio.Foto AS Foto,
            SisPessoaConvenio.FotoMimeType AS FotoMimeType,
            SisPessoaConvenio.FisicaJuridica AS FisicaJuridica,
            SisPessoaConvenio.IsAtivo AS IsAtivo,
            SisPessoaConvenio.ImportacaoId AS ImportacaoId,
            SisPessoaConvenio.ImportacaoTabela AS ImportacaoTabela,
            SisPessoaConvenio.IsCredito AS IsCredito,
            SisPessoaConvenio.IsDebito AS IsDebito,
            SisPessoaConvenio.Observacao AS Observacao,
            SisPessoaConvenio.Rg AS Rg,
            SisPessoaConvenio.Emissor AS Emissor,
            SisPessoaConvenio.EmissaoRg AS EmissaoRg,
            SisPessoaConvenio.Nascimento AS Nascimento,
            SisPessoaConvenio.Cpf AS Cpf,
            SisPessoaConvenio.NomeMae AS NomeMae,
            SisPessoaConvenio.NomePai AS NomePai,
            SisPessoaConvenio.RazaoSocial AS RazaoSocial,
            SisPessoaConvenio.NomeFantasia AS NomeFantasia,
            SisPessoaConvenio.Cnpj AS Cnpj,
            SisPessoaConvenio.InscricaoEstadual AS InscricaoEstadual,
            SisPessoaConvenio.InscricaoMunicipal AS InscricaoMunicipal,
            SisPessoaConvenio.Telefone1 AS Telefone1,
            SisPessoaConvenio.DddTelefone1 AS DddTelefone1,
            SisPessoaConvenio.Telefone2 AS Telefone2,
            SisPessoaConvenio.DddTelefone2 AS DddTelefone2,
            SisPessoaConvenio.Telefone3 AS Telefone3,
            SisPessoaConvenio.DddTelefone3 AS DddTelefone3,
            SisPessoaConvenio.Telefone4 AS Telefone4,
            SisPessoaConvenio.DddTelefone4 AS DddTelefone4,
            SisPessoaConvenio.Email AS Email,
            SisPessoaConvenio.Email2 AS Email2,
            SisPessoaConvenio.Email3 AS Email3,
            SisPessoaConvenio.Email4 AS Email4,
            SisPessoaConvenio.TipoLogradouroId AS TipoLogradouroId,
            SisPessoaConvenio.TipoSanguineoId AS TipoSanguineoId,
            SisPessoaConvenio.ReligiaoId AS ReligiaoId,
            SisPessoaConvenio.EstadoCivilId AS EstadoCivilId,
            SisPessoaConvenio.EscolaridadeId AS EscolaridadeId,
            SisPessoaConvenio.NaturalidadeId AS NaturalidadeId,
            SisPessoaConvenio.TipoPessoaId AS TipoPessoaId,
            SisPessoaConvenio.ProfissaoId AS ProfissaoId,
            SisPessoaConvenio.TipoTelefone1Id AS TipoTelefone1Id,
            SisPessoaConvenio.TipoTelefone2Id AS TipoTelefone2Id,
            SisPessoaConvenio.TipoTelefone3Id AS TipoTelefone3Id,
            SisPessoaConvenio.TipoTelefone4Id AS TipoTelefone4Id,

            -- Plano 10
            Plano.Id AS Id,
            Plano.Codigo AS Codigo,
            Plano.Descricao AS Descricao,
            Plano.IsDeleted AS IsDeleted,
            Plano.DeleterUserId AS DeleterUserId,
            Plano.DeletionTime AS DeletionTime,
            Plano.CreatorUserId AS CreatorUserId,
            Plano.CreationTime AS CreationTime,
            Plano.IsSistema AS IsSistema,
            Plano.LastModificationTime AS LastModificationTime,
            Plano.LastModifierUserId AS LastModifierUserId,
            Plano.ImportaId AS ImportaId,
            Plano.SisConvenioId AS ConvenioId,
            Plano.IsDespesasAcompanhante AS IsDespesasAcompanhante,
            Plano.IsValidadeCarteiraIndeterminada AS IsValidadeCarteiraIndeterminada,
            Plano.isAtivo AS IsAtivo,
            Plano.IsPlanoEmpresa AS IsPlanoEmpresa,

            -- Leito 11
            Leito.Id AS Id,
            Leito.Codigo AS Codigo,
            Leito.Descricao AS Descricao,
            Leito.IsDeleted AS IsDeleted,
            Leito.DeleterUserId AS DeleterUserId,
            Leito.DeletionTime AS DeletionTime,
            Leito.CreatorUserId AS CreatorUserId,
            Leito.CreationTime AS CreationTime,
            Leito.IsSistema AS IsSistema,
            Leito.LastModificationTime AS LastModificationTime,
            Leito.LastModifierUserId AS LastModifierUserId,
            Leito.ImportaId AS ImportaId,
            Leito.DescricaoResumida AS DescricaoResumida,
            Leito.LeitoAih AS LeitoAih,
            Leito.Ramal AS Ramal,
            Leito.Sexo AS Sexo,
            Leito.TabelaItemSusId AS TabelaItemSusId,
            Leito.Extra AS Extra,
            Leito.HospitalDia AS HospitalDia,
            Leito.Ativo AS Ativo,
            Leito.SisTabelaItemTissId AS TabelaItemTissId,
            Leito.SisTipoAcomodacaoId AS TipoAcomodacaoId,
            Leito.AteLeitoStatusId AS LeitoStatusId,
            Leito.SisTipoAcomodacaoId AS TipoAcomodacaoId,
            -- Leito Status

            -- Tipo Acomodacao 12
            
            TipoAcomodacaoLeito.Id AS Id,
            TipoAcomodacaoLeito.Codigo AS Codigo,
            TipoAcomodacaoLeito.Descricao AS Descricao,
            TipoAcomodacaoLeito.IsDeleted AS IsDeleted,
            TipoAcomodacaoLeito.DeleterUserId AS DeleterUserId,
            TipoAcomodacaoLeito.DeletionTime AS DeletionTime,
            TipoAcomodacaoLeito.CreatorUserId AS CreatorUserId,
            TipoAcomodacaoLeito.CreationTime AS CreationTime,
            TipoAcomodacaoLeito.IsSistema AS IsSistema,
            TipoAcomodacaoLeito.LastModificationTime AS LastModificationTime,
            TipoAcomodacaoLeito.LastModifierUserId AS LastModifierUserId,
            TipoAcomodacaoLeito.ImportaId AS ImportaId,

            -- Tabela Item Tiss

            -- Unidade Organizacional 13
            UnidadeOrganizacional.Id AS Id,
            UnidadeOrganizacional.Codigo AS Codigo,
            UnidadeOrganizacional.Descricao AS Descricao,
            UnidadeOrganizacional.IsDeleted AS IsDeleted,
            UnidadeOrganizacional.DeleterUserId AS DeleterUserId,
            UnidadeOrganizacional.DeletionTime AS DeletionTime,
            UnidadeOrganizacional.CreatorUserId AS CreatorUserId,
            UnidadeOrganizacional.CreationTime AS CreationTime,
            UnidadeOrganizacional.IsSistema AS IsSistema,
            UnidadeOrganizacional.LastModificationTime AS LastModificationTime,
            UnidadeOrganizacional.LastModifierUserId AS LastModifierUserId,
            UnidadeOrganizacional.ImportaId AS ImportaId,
            UnidadeOrganizacional.Localizacao AS Localizacao,
            UnidadeOrganizacional.IsAtivo AS IsAtivo,
            UnidadeOrganizacional.ControlaAlta AS ControlaAlta,
            UnidadeOrganizacional.IsInternacao AS IsInternacao,
            UnidadeOrganizacional.IsAmbulatorioEmergencia AS IsAmbulatorioEmergencia,
            UnidadeOrganizacional.IsHospitalDia AS IsHospitalDia,
            UnidadeOrganizacional.IsSetorExames AS IsSetorExames,
            UnidadeOrganizacional.IsEstoque AS IsEstoque,
            UnidadeOrganizacional.IsLocalUtilizacao AS IsLocalUtilizacao,
            UnidadeOrganizacional.HoraInicioPrescricao AS HoraInicioPrescricao,
            UnidadeOrganizacional.AteUnidadeInternacaoTipoId AS UnidadeInternacaoTipoId,
            UnidadeOrganizacional.SisOrganizationUnitId AS OrganizationUnitId,
            UnidadeOrganizacional.CentroCustoId AS CentroCustoId,
            UnidadeOrganizacional.EstEstoqueId AS EstoqueId,
            UnidadeOrganizacional.ImportaLocalUtilizacaoId AS ImportaLocalUtilizacaoId,
            UnidadeOrganizacional.IsControlaLeito AS IsControlaLeito,

            -- Empresa 14   
            Empresa.Id AS Id,
            Empresa.Codigo AS Codigo,
            Empresa.Descricao AS Descricao,
            Empresa.IsDeleted AS IsDeleted,
            Empresa.DeleterUserId AS DeleterUserId,
            Empresa.DeletionTime AS DeletionTime,
            Empresa.CreatorUserId AS CreatorUserId,
            Empresa.CreationTime AS CreationTime,
            Empresa.LastModificationTime AS LastModificationTime,
            Empresa.LastModifierUserId AS LastModifierUserId,
            Empresa.IsSistema AS IsSistema,
            Empresa.Logotipo AS Logotipo,
            Empresa.LogotipoMimeType AS LogotipoMimeType,
            Empresa.CodigoSus AS CodigoSus,
            Empresa.Cnes AS Cnes,
            Empresa.IsAtiva AS IsAtiva,
            Empresa.IsComprasUnificadas AS IsComprasUnificadas,
            Empresa.SisConvenioId AS ConvenioId,
            Empresa.SisPlanoId AS PlanoId,
            Empresa.EstoqueId AS EstoqueId,
            Empresa.RazaoSocial AS  RazaoSocial,
            Empresa.NomeFantasia AS  NomeFantasia,
            Empresa.Cnpj AS  Cnpj,
            Empresa.InscricaoEstadual AS  InscricaoEstadual,
            Empresa.InscricaoMunicipal AS  InscricaoMunicipal,
            Empresa.Cep AS  Cep,
            Empresa.TipoLogradouroId AS  TipoLogradouroId,
            Empresa.Logradouro AS  Logradouro,
            Empresa.Complemento AS  Complemento,
            Empresa.Numero AS  Numero,
            Empresa.Bairro AS  Bairro,
            Empresa.CidadeId AS  CidadeId,
            Empresa.EstadoId AS  EstadoId,
            Empresa.PaisId AS  PaisId,
            Empresa.Telefone1 AS  Telefone1,
            Empresa.TipoTelefone1Id AS  TipoTelefone1Id,
            Empresa.DddTelefone1 AS  DddTelefone1,
            Empresa.Telefone2 AS  Telefone2,
            Empresa.TipoTelefone2Id AS  TipoTelefone2Id,
            Empresa.DddTelefone2 AS  DddTelefone2,
            Empresa.Telefone3 AS  Telefone3,
            Empresa.TipoTelefone3Id AS  TipoTelefone3Id,
            Empresa.DddTelefone3 AS  DddTelefone3,
            Empresa.Telefone4 AS  Telefone4,
            Empresa.TipoTelefone4Id AS  TipoTelefone4Id,
            Empresa.DddTelefone4 AS  DddTelefone4,
            Empresa.Email AS  Email,
            Empresa.Email2 AS  Email2,
            Empresa.Email3 AS  Email3,
            Empresa.Email4 AS  Email4,

            -- Atendimento Tipo 15
            AtendimentoTipo.[Id] AS Id,
            AtendimentoTipo.[Codigo] AS [Codigo],
            AtendimentoTipo.[Descricao] AS [Descricao],
            AtendimentoTipo.[IsDeleted] AS [IsDeleted],
            AtendimentoTipo.[DeleterUserId] AS [DeleterUserId],
            AtendimentoTipo.[DeletionTime] AS [DeletionTime],
            AtendimentoTipo.[CreatorUserId] AS [CreatorUserId],
            AtendimentoTipo.[CreationTime] AS [CreationTime],
            AtendimentoTipo.[IsSistema] AS [IsSistema],
            AtendimentoTipo.[LastModificationTime] AS [LastModificationTime],
            AtendimentoTipo.[LastModifierUserId] AS [LastModifierUserId],
            AtendimentoTipo.[ImportaId] AS [ImportaId],
            AtendimentoTipo.[IsInternacao] AS [IsInternacao],
            AtendimentoTipo.[IsAmbulatorioEmergencia] AS [IsAmbulatorioEmergencia],
            AtendimentoTipo.[SisTabelaItemTissId] AS [TabelaItemTissId],

            -- AtendimentoTipoTabelaDominio 16
            AtendimentoTipoTabelaDominio.Id AS Id,
            AtendimentoTipoTabelaDominio.Codigo AS Codigo,
            AtendimentoTipoTabelaDominio.Descricao AS Descricao,
            AtendimentoTipoTabelaDominio.IsDeleted AS IsDeleted,
            AtendimentoTipoTabelaDominio.DeleterUserId AS DeleterUserId,
            AtendimentoTipoTabelaDominio.DeletionTime AS DeletionTime,
            AtendimentoTipoTabelaDominio.CreatorUserId AS CreatorUserId,
            AtendimentoTipoTabelaDominio.CreationTime AS CreationTime,
            AtendimentoTipoTabelaDominio.IsSistema AS IsSistema,
            AtendimentoTipoTabelaDominio.LastModificationTime AS LastModificationTime,
            AtendimentoTipoTabelaDominio.LastModifierUserId AS LastModifierUserId,
            AtendimentoTipoTabelaDominio.GrupoTipoTabelaDominioId AS GrupoTipoTabelaDominioId,
            AtendimentoTipoTabelaDominio.TipoTabelaDominioId AS TipoTabelaDominioId,

            -- Tipo Acomodacao 17
            TipoAcomodacao.Id AS Id,
            TipoAcomodacao.Codigo AS Codigo,
            TipoAcomodacao.Descricao AS Descricao,
            TipoAcomodacao.IsDeleted AS IsDeleted,
            TipoAcomodacao.DeleterUserId AS DeleterUserId,
            TipoAcomodacao.DeletionTime AS DeletionTime,
            TipoAcomodacao.CreatorUserId AS CreatorUserId,
            TipoAcomodacao.CreationTime AS CreationTime,
            TipoAcomodacao.IsSistema AS IsSistema,
            TipoAcomodacao.LastModificationTime AS LastModificationTime,
            TipoAcomodacao.LastModifierUserId AS LastModifierUserId,
            TipoAcomodacao.ImportaId AS ImportaId,

            -- TipoAcompanhante 18 
            TipoAcompanhante.Id AS Id,
            TipoAcompanhante.Codigo AS Codigo,
            TipoAcompanhante.Descricao AS Descricao,
            TipoAcompanhante.IsDeleted AS IsDeleted,
            TipoAcompanhante.DeleterUserId AS DeleterUserId,
            TipoAcompanhante.DeletionTime AS DeletionTime,
            TipoAcompanhante.CreatorUserId AS CreatorUserId,
            TipoAcompanhante.CreationTime AS CreationTime,
            TipoAcompanhante.IsSistema AS IsSistema,
            TipoAcompanhante.LastModificationTime AS LastModificationTime,
            TipoAcompanhante.LastModifierUserId AS LastModifierUserId,
            TipoAcompanhante.ImportaId AS ImportaId,

            -- Especialidade 19
            Especialidade.Id AS Id,
            Especialidade.Codigo AS Codigo,
            Especialidade.Descricao AS Descricao,
            Especialidade.IsDeleted AS IsDeleted,
            Especialidade.DeleterUserId AS DeleterUserId,
            Especialidade.DeletionTime AS DeletionTime,
            Especialidade.CreatorUserId AS CreatorUserId,
            Especialidade.CreationTime AS CreationTime,
            Especialidade.IsSistema AS IsSistema,
            Especialidade.LastModificationTime AS LastModificationTime,
            Especialidade.LastModifierUserId AS LastModifierUserId,
            Especialidade.ImportaId AS ImportaId,
            Especialidade.Nome AS Nome,
            Especialidade.Cbo AS Cbo,
            Especialidade.CboSus AS CboSus,
            Especialidade.IsAtivo AS IsAtivo,
            Especialidade.SisCboId AS CboId,
            
            -- Motivo Alta 20
            MotivoAlta.Id AS Id,
            MotivoAlta.Codigo AS Codigo,
            MotivoAlta.Descricao AS Descricao,
            MotivoAlta.IsDeleted AS IsDeleted,
            MotivoAlta.DeleterUserId AS DeleterUserId,
            MotivoAlta.DeletionTime AS DeletionTime,
            MotivoAlta.CreatorUserId AS CreatorUserId,
            MotivoAlta.CreationTime AS CreationTime,
            MotivoAlta.IsSistema AS IsSistema,
            MotivoAlta.LastModificationTime AS LastModificationTime,
            MotivoAlta.LastModifierUserId AS LastModifierUserId,
            MotivoAlta.ImportaId AS ImportaId,
            MotivoAlta.AssMotivoAltaTipoAltaId AS MotivoAltaTipoAltaId,

            --Origem 21
            Origem.Id AS Id,
            Origem.Codigo AS Codigo,
            Origem.Descricao AS Descricao,
            Origem.IsDeleted AS IsDeleted,
            Origem.DeleterUserId AS DeleterUserId,
            Origem.DeletionTime AS DeletionTime,
            Origem.CreatorUserId AS CreatorUserId,
            Origem.CreationTime AS CreationTime,
            Origem.IsSistema AS IsSistema,
            Origem.LastModificationTime AS LastModificationTime,
            Origem.LastModifierUserId AS LastModifierUserId,
            Origem.ImportaId AS ImportaId,
            Origem.SisUnidadeOrganizacionalId AS UnidadeOrganizacionalId,
            Origem.IsAtivo AS IsAtivo,

            -- ServicoMedicoPrestado 22
            ServicoMedicoPrestado.Id AS Id,
            ServicoMedicoPrestado.Codigo AS Codigo,
            ServicoMedicoPrestado.Descricao AS Descricao,
            ServicoMedicoPrestado.IsDeleted AS IsDeleted,
            ServicoMedicoPrestado.DeleterUserId AS DeleterUserId,
            ServicoMedicoPrestado.DeletionTime AS DeletionTime,
            ServicoMedicoPrestado.CreatorUserId AS CreatorUserId,
            ServicoMedicoPrestado.CreationTime AS CreationTime,
            ServicoMedicoPrestado.IsSistema AS IsSistema,
            ServicoMedicoPrestado.LastModificationTime AS LastModificationTime,
            ServicoMedicoPrestado.LastModifierUserId AS LastModifierUserId,
            ServicoMedicoPrestado.ImportaId AS ImportaId,
            ServicoMedicoPrestado.ModeloAnamnese AS ModeloAnamnese,
            ServicoMedicoPrestado.Caucao AS Caucao,
            ServicoMedicoPrestado.SisEspecialidadeId AS EspecialidadeId,

            -- Nacionalidade 23
            NacionalidadeResponsavel.Id AS Id,
            NacionalidadeResponsavel.Codigo AS Codigo,
            NacionalidadeResponsavel.Descricao AS Descricao,
            NacionalidadeResponsavel.IsDeleted AS IsDeleted,
            NacionalidadeResponsavel.DeleterUserId AS DeleterUserId,
            NacionalidadeResponsavel.DeletionTime AS DeletionTime,
            NacionalidadeResponsavel.CreatorUserId AS CreatorUserId,
            NacionalidadeResponsavel.CreationTime AS CreationTime,
            NacionalidadeResponsavel.IsSistema AS IsSistema,
            NacionalidadeResponsavel.LastModificationTime AS LastModificationTime,
            NacionalidadeResponsavel.LastModifierUserId AS LastModifierUserId,
            NacionalidadeResponsavel.ImportaId AS ImportaId,

            -- Atendimento Motivo Cancelamento

            -- Carater Atendimento 24
            CaraterAtendimento.Id AS Id,
            CaraterAtendimento.Codigo AS Codigo,
            CaraterAtendimento.Descricao AS Descricao,
            CaraterAtendimento.IsDeleted AS IsDeleted,
            CaraterAtendimento.DeleterUserId AS DeleterUserId,
            CaraterAtendimento.DeletionTime AS DeletionTime,
            CaraterAtendimento.CreatorUserId AS CreatorUserId,
            CaraterAtendimento.CreationTime AS CreationTime,
            CaraterAtendimento.IsSistema AS IsSistema,
            CaraterAtendimento.LastModificationTime AS LastModificationTime,
            CaraterAtendimento.LastModifierUserId AS LastModifierUserId,
            CaraterAtendimento.ImportaId AS ImportaId,
            CaraterAtendimento.TipoTabelaDominioId AS TipoTabelaDominioId,
            CaraterAtendimento.GrupoTipoTabelaDominioId AS GrupoTipoTabelaDominioId,
            CaraterAtendimento.GrupoTipoTabelaDominioId AS GrupoTipoTabelaDominioId,

            -- Indicacao Acidente 25
            IndicacaoAcidente.Id AS Id,
            IndicacaoAcidente.Codigo AS Codigo,
            IndicacaoAcidente.Descricao AS Descricao,
            IndicacaoAcidente.IsDeleted AS IsDeleted,
            IndicacaoAcidente.DeleterUserId AS DeleterUserId,
            IndicacaoAcidente.DeletionTime AS DeletionTime,
            IndicacaoAcidente.CreatorUserId AS CreatorUserId,
            IndicacaoAcidente.CreationTime AS CreationTime,
            IndicacaoAcidente.IsSistema AS IsSistema,
            IndicacaoAcidente.LastModificationTime AS LastModificationTime,
            IndicacaoAcidente.LastModifierUserId AS LastModifierUserId,
            IndicacaoAcidente.ImportaId AS ImportaId,
            IndicacaoAcidente.TipoTabelaDominioId AS TipoTabelaDominioId,
            IndicacaoAcidente.GrupoTipoTabelaDominioId AS GrupoTipoTabelaDominioId,
            IndicacaoAcidente.GrupoTipoTabelaDominioId AS GrupoTipoTabelaDominioId,

            -- Guia 26
            Guia.Id AS Id,
            Guia.Codigo AS Codigo,
            Guia.Descricao AS Descricao,
            Guia.IsDeleted AS IsDeleted,
            Guia.DeleterUserId AS DeleterUserId,
            Guia.DeletionTime AS DeletionTime,
            Guia.CreatorUserId AS CreatorUserId,
            Guia.CreationTime AS CreationTime,
            Guia.IsSistema AS IsSistema,
            Guia.LastModificationTime AS LastModificationTime,
            Guia.LastModifierUserId AS LastModifierUserId,
            Guia.SisOriginariaId AS OriginariaId,
            Guia.ModeloPDF AS ModeloPDF,
            Guia.ModeloPDFMimeType AS ModeloPDFMimeType,
            Guia.ModeloPNG AS ModeloPNG,
            Guia.ModeloPNGMimeType AS ModeloPNGMimeType,
            Guia.CamposJson AS CamposJson,

            --FatGuia 27
            FatGuia.Id AS Id,
            FatGuia.Codigo AS Codigo,
            FatGuia.Descricao AS Descricao,
            FatGuia.IsDeleted AS IsDeleted,
            FatGuia.DeleterUserId AS DeleterUserId,
            FatGuia.DeletionTime AS DeletionTime,
            FatGuia.CreatorUserId AS CreatorUserId,
            FatGuia.CreationTime AS CreationTime,
            FatGuia.IsSistema AS IsSistema,
            FatGuia.LastModificationTime AS LastModificationTime,
            FatGuia.LastModifierUserId AS LastModifierUserId,
            FatGuia.IsAmbulatorio AS IsAmbulatorio,
            FatGuia.IsInternacao AS IsInternacao,

            --GRUPOCID 28
            GrupoCID.Id,
            GrupoCID.Descricao

        FROM 
            AteAtendimento AS Atendimento
            LEFT JOIN SisPaciente AS Paciente ON Atendimento.SisPacienteId = Paciente.Id AND Paciente.IsDeleted = @deleted
            LEFT JOIN SisPessoa AS SisPessoaPaciente ON Paciente.SisPessoaId = SisPessoaPaciente.Id AND SisPessoaPaciente.IsDeleted = @deleted
            LEFT JOIN SisSexo AS SisSexoPessoa ON SisPessoaPaciente.SexoId = SisSexoPessoa.Id AND SisSexoPessoa.IsDeleted =  @deleted
            LEFT JOIN SisNacionalidade AS NacionalidadePessoa ON NacionalidadePessoa.Id = SisPessoaPaciente.NacionalidadeId AND SisPessoaPaciente.IsDeleted = @deleted
            LEFT JOIN SisEmpresa AS Empresa ON Empresa.Id = Atendimento.SisEmpresaId ANd Empresa.IsDeleted = @deleted
            LEFT JOIN SisMedico AS Medico ON Atendimento.SisMedicoId = Medico.Id AND Medico.IsDeleted = @deleted
            LEFT JOIN SisConselho AS ConselhoMedico ON  ConselhoMedico.Id = Medico.SisConselhoId AND ConselhoMedico.IsDeleted = @deleted
            LEFT JOIN SisPessoa AS SisPessoaMedico ON SisPessoaMedico.Id = Medico.SisPessoaId AND SisPessoaMedico.IsDeleted = @deleted
            LEFT JOIN AteAtendimentoTipo AS AtendimentoTipo ON AtendimentoTipo.Id = Atendimento.SisAtendimentoTipoId AND AtendimentoTipo.IsDeleted = @deleted
            LEFT JOIN SisTabelaDominio AS AtendimentoTipoTabelaDominio ON AtendimentoTipoTabelaDominio.Id = AtendimentoTipo.SisTabelaItemTissId  AND AtendimentoTipoTabelaDominio.IsDeleted =  @deleted
            LEFT JOIN SisConvenio AS Convenio ON Convenio.Id = Atendimento.SisConveniolId AND Convenio.IsDeleted = @deleted
            LEFT JOIN SisPessoa AS SisPessoaConvenio ON Convenio.SisPessoaId = SisPessoaConvenio.Id AND SisPessoaConvenio.IsDeleted = @deleted
            LEFT JOIN SisEspecialidade AS Especialidade ON Especialidade.Id = Atendimento.SisEspecialidadeId AND Especialidade.IsDeleted = @deleted
            LEFT JOIN SisGuia AS Guia ON Guia.Id = Atendimento.SisGuiaId AND Guia.IsDeleted =  @deleted
            LEFT JOIN FatGuia AS FatGuia ON FatGuia.Id = Atendimento.FatGuiaId AND FatGuia.IsDeleted =  @deleted
            LEFT JOIN AteTipoAcompanhante AS TipoAcompanhante ON TipoAcompanhante.Id = Atendimento.AteTipoAcompanhanteId AND TipoAcompanhante.IsDeleted =  @deleted
            LEFT JOIN AteLeito AS Leito ON Leito.Id = Atendimento.AteLeitoId  AND Leito.IsDeleted =  @deleted
            LEFT JOIN SisTipoAcomodacao AS TipoAcomodacaoLeito ON TipoAcomodacaoLeito.Id = Leito.SisTipoAcomodacaoId AND TipoAcomodacaoLeito.IsDeleted =  @deleted
            LEFT JOIN AssMotivoAlta AS MotivoAlta ON MotivoAlta.Id = Atendimento.AteMotivoAltaId AND MotivoAlta.IsDeleted =  @deleted
            LEFT JOIN SisNacionalidade AS NacionalidadeResponsavel ON NacionalidadeResponsavel.Id = Atendimento.SisNacionalidadeResponsavelId AND NacionalidadeResponsavel.IsDeleted =  @deleted
            LEFT JOIN SisOrigem AS Origem ON Origem.Id = Atendimento.SisOrigemId AND Origem.IsDeleted =  @deleted
            LEFT JOIN SisPlano AS Plano ON Plano.Id = Atendimento.SisPlanoId AND Plano.IsDeleted =  @deleted
            LEFT JOIN ServicoMedicoPrestado AS ServicoMedicoPrestado ON ServicoMedicoPrestado.Id = Atendimento.SisServicoMedicoPrestadoId  AND ServicoMedicoPrestado.IsDeleted =  @deleted
            LEFT JOIN SisUnidadeOrganizacional AS UnidadeOrganizacional ON UnidadeOrganizacional.Id = Atendimento.SisUnidadeOrganizacionalId AND UnidadeOrganizacional.IsDeleted =  @deleted
            LEFT JOIN SisTabelaDominio AS CaraterAtendimento ON CaraterAtendimento.Id = Atendimento.CaraterAtendimentoId AND CaraterAtendimento.IsDeleted =  @deleted
            LEFT JOIN SisTabelaDominio AS IndicacaoAcidente ON IndicacaoAcidente.Id = Atendimento.IndicacaoAcidenteId  AND IndicacaoAcidente.IsDeleted =  @deleted
            LEFT JOIN SisTipoAcomodacao AS TipoAcomodacao ON TipoAcomodacao.Id = Atendimento.SisTipoAcomodacaoId AND TipoAcomodacao.IsDeleted =  @deleted
            LEFT JOIN GrupoCID AS GrupoCID ON GrupoCID.Id = Atendimento.AteGrupoCIDId AND GrupoCID.IsDeleted =  @deleted
            WHERE Atendimento.Id = @atendimentoId";

        private const string QueryListasAtendimentoObter = @"
            SELECT [Id]
              ,[PacienteId]
              ,[DataPesagem]
              ,[Valor]
              ,[Altura]
              ,[PerimetroCefalico]
              ,[IsSistema]
              ,[IsDeleted]
              ,[DeleterUserId]
              ,[DeletionTime]
              ,[LastModificationTime]
              ,[LastModifierUserId]
              ,[CreationTime]
              ,[CreatorUserId]
              ,[Codigo]
              ,[Descricao]
              ,[ImportaId]
            FROM [PacientePeso]
            WHERE [PacienteId] = @PacienteId AND IsDeleted = @IsDeleted;

            SELECT [Id]
              ,[DataCadastro]
              ,[Alergia]
              ,[PacienteId]
              ,[AtendimentoId]
              ,[IsSistema]
              ,[Codigo]
              ,[Descricao]
              ,[ImportaId]
              ,[IsDeleted]
              ,[DeleterUserId]
              ,[DeletionTime]
              ,[LastModificationTime]
              ,[LastModifierUserId]
              ,[CreationTime]
              ,[CreatorUserId]
            FROM [PacienteAlergias]
            WHERE [PacienteId] = @PacienteId AND IsDeleted = @IsDeleted;

            SELECT  [PacienteDiagnosticos].[Id]
              ,[PacienteDiagnosticos].[DataDiagnostico]
              ,[PacienteDiagnosticos].[GrupoCIDId]
              ,[PacienteDiagnosticos].[PacienteId]
              ,[PacienteDiagnosticos].[AtendimentoId]
              ,[PacienteDiagnosticos].[IsSistema]
              ,[PacienteDiagnosticos].[Codigo]
              ,[PacienteDiagnosticos].[Descricao]
              ,[PacienteDiagnosticos].[ImportaId]
              ,[PacienteDiagnosticos].[IsDeleted]
              ,[PacienteDiagnosticos].[DeleterUserId]
              ,[PacienteDiagnosticos].[DeletionTime]
              ,[PacienteDiagnosticos].[LastModificationTime]
              ,[PacienteDiagnosticos].[LastModifierUserId]
              ,[PacienteDiagnosticos].[CreationTime]
              ,[PacienteDiagnosticos].[CreatorUserId]

              ,[GrupoCID].[Id]
              ,[GrupoCID].[IsSistema]
              ,[GrupoCID].[Codigo]
              ,[GrupoCID].[Descricao]
              ,[GrupoCID].[ImportaId]
              ,[GrupoCID].[IsDeleted]
              ,[GrupoCID].[DeleterUserId]
              ,[GrupoCID].[DeletionTime]
              ,[GrupoCID].[LastModificationTime]
              ,[GrupoCID].[LastModifierUserId]
              ,[GrupoCID].[CreationTime]
              ,[GrupoCID].[CreatorUserId]
            FROM [PacienteDiagnosticos]
            LEFT JOIN [GrupoCID]
            ON [GrupoCID].id = GrupoCIDId AND [GrupoCID].IsDeleted = @IsDeleted
            WHERE PacienteDiagnosticos.IsDeleted = @IsDeleted
            AND PacienteId = @PacienteId;

            SELECT [Id]
              ,[ConvenioId]
              ,[EmpresaId]
              ,[IsSistema]
              ,[Codigo]
              ,[Descricao]
              ,[ImportaId]
              ,[IsDeleted]
              ,[DeleterUserId]
              ,[DeletionTime]
              ,[LastModificationTime]
              ,[LastModifierUserId]
              ,[CreationTime]
              ,[CreatorUserId]
            FROM [SisIdentificacaoPrestadorNaOperadora]
            WHERE [ConvenioId] = @ConvenioId AND IsDeleted = @IsDeleted";

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<AtendimentoDto> ObterIQ(IQueryable<Atendimento> iQueryableAtendimento)
        {
            try
            {
                using (var senhaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Senha, long>>())
                {
                    var m = await iQueryableAtendimento.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false);

                    var atendimento = AtendimentoDto.Mapear(m);

                    var senhaAtendimento = await senhaRepository.Object.GetAll().AsNoTracking()
                                               .FirstOrDefaultAsync(w => w.AtendimentoId == atendimento.Id)
                                               .ConfigureAwait(false);

                    if (senhaAtendimento != null)
                    {
                        atendimento.SenhaAtendimento = SenhaDto.Mapear(senhaAtendimento);
                    }

                    return atendimento;
                }
            }

            // catch (DbEntityValidationException e)
            // {
            // foreach (var eve in e.EntityValidationErrors)
            // {
            // Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            // eve.Entry.Entity.GetType().Name, eve.Entry.State);
            // foreach (var ve in eve.ValidationErrors)
            // {
            // Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            // ve.PropertyName, ve.ErrorMessage);
            // }
            // }
            // throw;
            // }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<AtendimentoDto> ObterAssistencial(long id)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var senhaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Senha, long>>())
                {
                    var m = await atendimentoRepository.Object.GetAll().Include(a => a.Paciente)
                                .Include(a => a.Paciente.SisPessoa).Include(a => a.Paciente.SisPessoa.Sexo)

                                // .Include(a => a.Paciente.SisPessoa.Nacionalidade)
                                .Include(a => a.Paciente.Sexo)

                                // .Include(a => a.Paciente.SisPessoa.Enderecos)
                                // .Include(a => a.Paciente.Pais)
                                // .Include(a => a.Paciente.Estado)
                                // .Include(a => a.Paciente.Cidade)
                                // .Include(a => a.Paciente.EstadoCivil)
                                // .Include(a => a.Paciente.Profissao)
                                .Include(a => a.Medico).Include(a => a.Medico.Conselho).Include(a => a.Medico.SisPessoa)

                                // .Include(a => a.AtendimentoTipo)
                                .Include(a => a.Convenio).Include(a => a.Convenio.SisPessoa).Include(a => a.Empresa)
                                .Include(a => a.Especialidade)

                                // .Include(a => a.Guia) // modelo antigo
                                // .Include(a => a.FatGuia) // novo modelo FatGuia
                                .Include(a => a.Leito).Include(a => a.Leito.TipoAcomodacao).Include(a => a.MotivoAlta)

                                // .Include(a => a.Nacionalidade)
                                // .Include(a => a.Origem)
                                .Include(a => a.Plano)

                                // .Include(a => a.ServicoMedicoPrestado)
                                .Include(a => a.UnidadeOrganizacional).Include(a => a.ProtocoloAtendimento)
                                .Include(a => a.ClassificacaoAtendimento).Include(a => a.AtendimentoStatus)
                                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

                    var atendimento = AtendimentoDto.Mapear(m);

                    var senhaAtendimento = await senhaRepository.Object.GetAll().AsNoTracking()
                                               .FirstOrDefaultAsync(w => w.AtendimentoId == atendimento.Id)
                                               .ConfigureAwait(false);

                    if (senhaAtendimento != null)
                    {
                        atendimento.SenhaAtendimento = SenhaDto.Mapear(senhaAtendimento);
                    }

                    return atendimento;
                }
            }

            // catch (DbEntityValidationException e)
            // {
            // foreach (var eve in e.EntityValidationErrors)
            // {
            // Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            // eve.Entry.Entity.GetType().Name, eve.Entry.State);
            // foreach (var ve in eve.ValidationErrors)
            // {
            // Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            // ve.PropertyName, ve.ErrorMessage);
            // }
            // }
            // throw;
            // }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public AtendimentoDto ObterPorLeito(long id)
        {
            try
            {
                using (var atendimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var result = atendimentoRepository.Object.GetAll().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).Include(m => m.AtendimentoTipo).Include(m => m.Convenio)
                        .Include(m => m.Convenio.SisPessoa).Include(m => m.Empresa).Include(m => m.Especialidade)
                        .Include(m => m.Guia).Include(m => m.Leito).Include(m => m.MotivoAlta)
                        .Include(m => m.Nacionalidade).Include(m => m.Origem).Include(m => m.Plano)
                        .Include(m => m.ServicoMedicoPrestado).Include(m => m.UnidadeOrganizacional).AsNoTracking()
                        .FirstOrDefault(m => m.LeitoId == id);

                    var atendimento = new AtendimentoDto();

                    if (result != null)
                    {
                        atendimento = AtendimentoDto.MapearFromCore(result);
                        return atendimento;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork]
        public async Task<long> CriarNovoAtendimento()
        {
            try
            {
                var novoAtendimento = new Atendimento
                {
                    Codigo = string.Empty,
                    IsSistema = false,
                    IsDeleted = false,
                    CreationTime = DateTime.Now,
                    DataRegistro = DateTime.Now
                };
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var result = await atendimentoRepository.Object.InsertAndGetIdAsync(novoAtendimento)
                                     .ConfigureAwait(false);
                    novoAtendimento.Codigo =
                        await ultimoIdAppService.Object.ObterProximoCodigo("Atendimento").ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<GenericoIdNome>> ListarAtendimentoPaciente()
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = atendimentoRepository.Object.GetAll().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).AsNoTracking();

                    return new PagedResultDto<GenericoIdNome>(
                        await query.CountAsync().ConfigureAwait(false),
                        (await query.ToListAsync().ConfigureAwait(false)).Select(
                            item => new GenericoIdNome
                            {
                                Id = item.Id,
                                Nome = string.Concat(item.Codigo, " - ", item.Paciente?.NomeCompleto)
                            }).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    // get com filtro
                    var query = from p in atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                                    .Include(m => m.Paciente.SisPessoa).WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(), 
                                        m => m.Codigo.Contains(dropdownInput.search) || m.Paciente.NomeCompleto.Contains(dropdownInput.search))
                                orderby p.Paciente.NomeCompleto ascending
                                select new DropdownItems<long>
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Paciente.NomeCompleto)
                                };

                    // paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>()
                    {
                        Items = await queryResultPage.ToListAsync().ConfigureAwait(false),
                        TotalCount = total
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<IResultDropdownList<long>> ListarAtendimentosEmAbertoDropdown(DropdownInput dropdownInput)
        {
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                var dataAtual = DateTime.Now.Date.AddDays(-1);
                var tipo = dropdownInput.filtros[0];
                var isAtendimentoEmgergicia = dropdownInput.filtros[0] == "0";
                var isInternacao = dropdownInput.filtros[0] == "1";
                var unidadeOrganizacionalId = 0;
                if (dropdownInput.filtros.Length >= 1 && !string.IsNullOrEmpty(dropdownInput.filtros[1]))
                {
                    unidadeOrganizacionalId = int.Parse(dropdownInput.filtros[1]);
                }

                return await this.ListarDropdownLambda(
                           dropdownInput,
                           atendimentoRepository.Object,
                           m => (string.IsNullOrEmpty(dropdownInput.search)
                                 || m.Paciente.NomeCompleto.Contains(dropdownInput.search)
                                 || m.Codigo.Contains(dropdownInput.search))
                                && (m.DataAlta == null || m.DataAlta >= dataAtual)
                                && (string.IsNullOrEmpty(tipo) || m.IsAmbulatorioEmergencia == isAtendimentoEmgergicia)
                                && (string.IsNullOrEmpty(tipo) || m.IsInternacao == isInternacao)
                                && (
                                    unidadeOrganizacionalId == 0 
                                    || 
                                    ( (unidadeOrganizacionalId == 7 && !m.LeitoId.HasValue) 
                                    ||
                                      (m.LeitoId.HasValue && m.Leito.UnidadeOrganizacionalId == unidadeOrganizacionalId)
                                    )),
                           p => new DropdownItems<long>
                           {
                               id = p.Id,
                               text = string.Concat(
                                            p.Codigo.ToString(),
                                            " - ",
                                            p.Paciente.NomeCompleto,
                                            p.DataAlta != null
                                                ? string.Concat(
                                                    " - Dt. alta: ",
                                                    ((DateTime)p.DataAlta).Day,
                                                    "/",
                                                    ((DateTime)p.DataAlta).Month,
                                                    "/",
                                                    ((DateTime)p.DataAlta).Year)
                                                : string.Empty)
                           },
                           o => o.Paciente.NomeCompleto).ConfigureAwait(false);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<IResultDropdownList<long>> ListarAtendimentosComSaidaDropdown(DropdownInput dropdownInput)
        {
            using (var _atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            using (var _estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            {
                // DateTime dataAtual = DateTime.Now.Date.AddDays(-1);
                // bool isAtendimentoEmgergicia = dropdownInput.filtros[0] == "0";
                var estoque = dropdownInput.filtros[0];

                long estoqueId;

                long.TryParse(estoque, out estoqueId);

                var saidas = _estoqueMovimentoRepository.Object.GetAll()
                    .Where(w => !w.IsEntrada && w.EstoqueId == estoqueId);

                return await this.ListarDropdownLambda(
                           dropdownInput,
                           _atendimentoRepository.Object,
                           m => (string.IsNullOrEmpty(dropdownInput.search)
                                 || m.Descricao.Contains(dropdownInput.search)
                                 || m.Codigo.Contains(dropdownInput.search)
                                 || m.Paciente.NomeCompleto.Contains(dropdownInput.search))
                                && saidas.Any(a => a.AtendimentoId == m.Id),
                           p => new DropdownItems
                           {
                               id = p.Id,
                               text = string.Concat(
                                            p.Codigo.ToString(),
                                            " - ",
                                            p.Paciente.NomeCompleto,
                                            p.DataAlta != null
                                                ? string.Concat(
                                                    " - Dt. alta: ",
                                                    ((DateTime)p.DataAlta).Day,
                                                    "/",
                                                    ((DateTime)p.DataAlta).Month,
                                                    "/",
                                                    ((DateTime)p.DataAlta).Year)
                                                : string.Empty)
                           },
                           o => o.Paciente.NomeCompleto).ConfigureAwait(false);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<IResultDropdownList<long>> ListarAtendimentosAmbulatorioInternacao(DropdownInput dropdownInput)
        {
            var isAmbulatorio = dropdownInput.filtros[0] == "true";
            var isInternacao = dropdownInput.filtros[1] == "true";

            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var atendimentoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = atendimentoRepository.Object.GetAll().AsNoTracking()
                        .Where(
                            m => m.IsAmbulatorioEmergencia == isAmbulatorio && m.IsInternacao == isInternacao
                                                                            && m.DataAlta == null
                                                                            && m.AtendimentoMotivoCancelamentoId
                                                                            == null).WhereIf(
                            !dropdownInput.search.IsNullOrWhiteSpace(),
                            m => m.Codigo.Contains(dropdownInput.search)

                                 // || m.Descricao.ToUpper().Contains(dropdownInput.search.ToUpper())
                                 || m.Paciente.Codigo.Contains(dropdownInput.search)
                                 || m.Paciente.NomeCompleto.Contains(dropdownInput.search))
                        .OrderBy(o => o.Paciente.NomeCompleto).ThenByDescending(o => o.Id).Select(
                            p => new DropdownItems<long>
                            {
                                id = p.Id,
                                text = string.Concat(
                                             "Ate: ",
                                             p.Codigo,
                                             " - ",
                                             p.DataRegistro.Day,
                                             "/",
                                             p.DataRegistro.Month,
                                             "/",
                                             p.DataRegistro.Year,
                                             " - Pac: ",
                                             p.Paciente.Codigo.ToString(),
                                             " - ",
                                             p.Paciente.NomeCompleto)
                            })

                        // .Select(p => new DropdownItems { id = p.Id, text = string.Concat(p.Paciente.Codigo.ToString(), " - ", p.Paciente.NomeCompleto) })
                        .Distinct();

                    var queryResultPage = query.OrderBy(o => o.text).Skip(numberOfObjectsPerPage * pageInt)
                        .Take(numberOfObjectsPerPage);


                    var total = await query.CountAsync().ConfigureAwait(false);
                    var list = await queryResultPage.ToListAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = list, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<IResultDropdownList<long>> ListarAtendimentosSemAlta(DropdownInput dropdownInput)
        {
            using (var _atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                return await this.ListarDropdownLambda(
                           dropdownInput,
                           _atendimentoRepository.Object,
                           m => (string.IsNullOrEmpty(dropdownInput.search)
                                 || m.Paciente.NomeCompleto.Contains(dropdownInput.search)
                                 || m.Paciente.Codigo.Contains(dropdownInput.search)) && m.DataAlta == null,
                           p => new DropdownItems<long>
                           {
                               id = p.Id,
                               text = string.Concat(
                                            p.Paciente.Codigo.ToString(),
                                            " - ",
                                            p.Paciente.NomeCompleto,
                                            " - Dt.:",
                                            p.DataRegistro.Day,
                                            "/",
                                            p.DataRegistro.Month,
                                            "/",
                                            p.DataRegistro.Year)
                           },
                           o => o.Paciente.NomeCompleto).ConfigureAwait(false);
            }
        }

        public async Task<IResultDropdownList<long>> ListarPacientesSemAlta(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<DropdownInput>()
            .EnableDistinct(true)
            .AddIdField("Atendimento.SisPacienteId")
            .AddTextField("CONCAT(Paciente.Codigo,' - ', Paciente.NomeCompleto, ' Último Atendimento: ', format(Ultimo.DataRegistro,'dd/MM/yyyy HH:mm:ss'))")
            .AddFromClause(@"
                AteAtendimento AS Atendimento  
                INNER JOIN 
                    SisPaciente AS Paciente ON Atendimento.SisPacienteId = Paciente.Id
                INNER JOIN (
                    SELECT Max(AteAtendimento.DataRegistro) AS DataRegistro, AteAtendimento.SisPacienteId 
                    FROM AteAtendimento WHERE AteAtendimento.IsDeleted = 0 
                    GROUP BY AteAtendimento.SisPacienteId) AS Ultimo ON Ultimo.DataRegistro = Atendimento.DataRegistro AND Atendimento.SisPacienteId = Ultimo.SisPacienteId")
            .AddWhereClause(@"Atendimento.DataAlta is null AND Atendimento.IsDeleted = 0 AND Paciente.IsDeleted = 0  AND (Paciente.Codigo like '%' + @search + '%' OR Paciente.NomeCompleto like '%' + @search + '%' OR @search is null OR @search like '')")

            .AddOrderByClause("CONCAT(Paciente.Codigo,' - ', Paciente.NomeCompleto, ' Último Atendimento: ', format(Ultimo.DataRegistro,'dd/MM/yyyy HH:mm:ss'))").ExecuteAsync(dropdownInput).ConfigureAwait(false);





            //using (var _atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            //{
            //    return await this.ListarDropdownLambda(
            //               dropdownInput,
            //               _atendimentoRepository.Object,
            //               m => (string.IsNullOrEmpty(dropdownInput.search)
            //                     || m.Paciente.NomeCompleto.Contains(dropdownInput.search)
            //                     || m.Paciente.Codigo.Contains(dropdownInput.search)) && m.DataAlta == null,
            //               p => new DropdownItems
            //               {
            //                   id = p.Paciente.Id,
            //                   text = string.Concat(p.Paciente.Codigo.ToString(), " - ", p.Paciente.NomeCompleto)
            //               }, o => o.Paciente.NomeCompleto).ConfigureAwait(false);
            //}
        }

        private async Task InserirItemConta(long atendimentoId, long contaId)
        {
            using (var _movimentoAutomaticoAppService = IocManager.Instance.ResolveAsDisposable<IMovimentoAutomaticoAppService>())
            using (var _faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            {
                var movimentoAutomatico = _movimentoAutomaticoAppService.Object.ObterMovimentoAutomaticoParaAtendimento(atendimentoId).Result;

                if (movimentoAutomatico == null)
                {
                    return;
                }

                var faturamentoContaItemInsertDto = new FaturamentoContaItemInsertDto
                {
                    AtendimentoId = atendimentoId,
                    Data = DateTime.Now,
                    ItensFaturamento = new List<FaturamentoContaItemDto>()
                };

                var item = new FaturamentoContaItemDto
                {
                    Id = movimentoAutomatico.MovimentosAutomaticosFaturamentosItens[0].FaturamentoItemId,
                    CentroCustoId = movimentoAutomatico.CentroCustoId,
                    Data = DateTime.Now,
                    FaturamentoContaId = contaId,
                    HoraIncio = DateTime.Now,
                    HoraFim = DateTime.Now,
                    Qtde = movimentoAutomatico.Quantidade,
                    TipoLeitoId = movimentoAutomatico.TipoAcomodacaoId,
                    TurnoId = movimentoAutomatico.TurnoId
                }; // FaturamentoContaItemDto.MapearFromCore(_exame.FaturamentoContaItem);

                faturamentoContaItemInsertDto.ItensFaturamento.Add(item);

                await _faturamentoContaItemAppService.Object.InserirItensContaFaturamento(faturamentoContaItemInsertDto).ConfigureAwait(false);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ListResultDto<VWRptAtendimentoDetalhadoDto>> ListarAtendimentoDetalhadoReport(DateTime startDate, DateTime endDate, long empresaId, long pacienteId, long convenioId, long medicoId, long especialidadeId, long unidadeOrganizacionalId, int tipoAtendimento, int tipoRel = 2, int tipoPeriodo = 1)
        {
            try
            {
                var tenant = this.GetCurrentTenant();
                var connectionString = ConfigurationManager.ConnectionStrings
                    .Cast<ConnectionStringSettings>()
                    .FirstOrDefault(v => string.Compare(v.Name, tenant.TenancyName, StringComparison.OrdinalIgnoreCase) == 0)
                    ?.ConnectionString;

                if (tenant != null)
                {
                    using (var db = new SWMANAGERDbContext(connectionString))
                    {
                        var strSql = new StringBuilder();
                        strSql.AppendLine("SELECT ID, CODIGOATENDIMENTO, ATENDIMENTOID, ATENDIMENTO, PACIENTEID, CODPACIENTE, PACIENTE, DATAATENDIMENTO, UNIDADE, CONVENIOID, CONVENIO, MEDICOID, MEDICO, EMPRESAID, EMPRESA, ORIGEM, ESPECIALIDADEID, ESPECIALIDADE, PLANO, TIPOATENDIMENTO, GUIA, NUMEROGUIA, DATAALTA, DATAALTAMEDICA, SENHA, NASCIMENTO, IDADEANO");
                        strSql.AppendLine("FROM dbo.vwRptAteDetalhado");
                        if (tipoPeriodo == 1)
                        {
                            strSql.AppendLine(string.Format("WHERE DATAATENDIMENTO BETWEEN CAST('{0}' AS DATE) AND DATEADD(DAY,1,CAST('{1}' AS DATE))", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd")));
                        }
                        else
                        {
                            strSql.AppendLine(string.Format("WHERE DATAALTA IS NOT NULL AND DATAALTA BETWEEN CAST('{0}' AS DATE) AND DATEADD(DAY,1,CAST('{1}' AS DATE))", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd")));

                        }

                        if (empresaId > 0)
                        {
                            strSql.AppendLine(string.Format("AND EMPRESAID={0}", empresaId));
                        }
                        else
                        {
                            ///Alterar
                            // strSql.AppendLine(string.Format("AND EMPRESAID in .........", empresaId));
                        }

                        if (medicoId > 0)
                        {
                            strSql.AppendLine(string.Format("AND MEDICOID={0}", medicoId));
                        }

                        if (especialidadeId > 0)
                        {
                            strSql.AppendLine(string.Format("AND ESPECIALIDADEID={0}", especialidadeId));
                        }

                        if (unidadeOrganizacionalId > 0)
                        {
                            strSql.AppendLine(string.Format("AND UNIDADEORGANIZACIONALID={0}", unidadeOrganizacionalId));
                        }
                        else
                        {
                            ///Alterar
                            //strSql.AppendLine(string.Format("AND UNIDADEORGANIZACIONALID in .........", unidadeOrganizacionalId));
                        }

                        if (pacienteId > 0)
                        {
                            strSql.AppendLine(string.Format("AND PACIENTEID={0}", pacienteId));
                        }

                        if (convenioId > 0)
                        {
                            strSql.AppendLine(string.Format("AND CONVENIOID={0}", convenioId));
                        }

                        switch (tipoAtendimento)
                        {
                            case 0:
                                // strSql.AppendLine(string.Format("AND ATENDIMENTO={0}", convenioId));
                                break;
                            case 1:
                                strSql.AppendLine(string.Format("AND ATENDIMENTO='{0}'", "AMBULATÓRIO/EMERGÊNCIA"));
                                break;
                            default:
                                strSql.AppendLine(string.Format("AND ATENDIMENTO='{0}'", "INTERNAÇÃO"));
                                break;
                        }
                        switch (tipoRel)
                        {
                            case 1:
                            case 5:
                                strSql.AppendLine(string.Format("ORDER BY EMPRESA,CONVENIO"));
                                break;
                            case 2:
                            case 6:
                                strSql.AppendLine(string.Format("ORDER BY EMPRESA,MEDICO"));
                                break;
                            case 3:
                                strSql.AppendLine(string.Format("ORDER BY EMPRESA,ESPECIALIDADE"));
                                break;
                            case 4:
                                if (tipoPeriodo == 1)
                                {
                                    strSql.AppendLine(string.Format("ORDER BY EMPRESA,DATAATENDIMENTO"));
                                }
                                else
                                {
                                    strSql.AppendLine(string.Format("ORDER BY EMPRESA,DATAALTA"));
                                }

                                break;
                            default:
                                break;
                        }
                        db.Database.CommandTimeout = 120;
                        var dr = db.Database.SqlQuery<VWRptAtendimentoDetalhado>(strSql.ToString()).ToList();
                        db.Dispose();
                        var listDto = VWRptAtendimentoDetalhadoDto.Mapear(dr).ToList();
                        return new ListResultDto<VWRptAtendimentoDetalhadoDto> { Items = listDto };
                    }
                }
                else
                {
                    throw new Exception(this.L("TenantNaoEncontrado"));
                }

                // var query = _atendimentoDetalhadoRepository
                // .GetAll()
                // .WhereIf(convenioId > 0, m => m.ConvenioId == convenioId)
                // .WhereIf(medicoId > 0, m => m.MedicoId == medicoId)
                // .WhereIf(pacienteId > 0, m => m.Pacienteid == pacienteId)
                // .WhereIf(empresaId > 0, m => m.EmpresaId == empresaId)
                // .Where(m => m.DataAtendimento >= startDate && m.DataAtendimento <= endDate);

                // var queryList = await query.ToListAsync();

                // var listaProdutoSaldoDto = VWRptAtendimentoDetalhadoDto.Mapear(queryList).ToList();

                // return new ListResultDto<VWRptAtendimentoDetalhadoDto> { Items = listaProdutoSaldoDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ListResultDto<VWRptAtendimentoResumidoDto>> ListarAtendimentoResumidoReport(DateTime startDate, DateTime endDate, long empresaId, long pacienteId, long convenioId, long medicoId, long especialidadeId, long unidadeOrganizacionalId, int tipoAtendimento, int tipoRel = 2, int tipoPeriodo = 1)
        {
            try
            {
                var tenant = this.GetCurrentTenant();
                var connectionString = ConfigurationManager.ConnectionStrings
                    .Cast<ConnectionStringSettings>()
                    .FirstOrDefault(v => string.Compare(v.Name, tenant.TenancyName, StringComparison.OrdinalIgnoreCase) == 0)
                    ?.ConnectionString;

                if (tenant != null)
                {
                    using (var db = new SWMANAGERDbContext(connectionString))
                    {
                        var strSql = new StringBuilder();
                        strSql.AppendLine("SELECT CAST(ROW_NUMBER() OVER(ORDER BY EMPRESAID, EMPRESA, CONVENIOID, CONVENIO, PLANO, MEDICOID, MEDICO, ESPECIALIDADE) AS BIGINT) AS ID, EMPRESAID, EMPRESA,");
                        strSql.AppendLine("CONVENIOID, CONVENIO, PLANO, MEDICOID, MEDICO, ESPECIALIDADE, COUNT(PACIENTEID) ATENDIMENTOS, SUM(CASE WHEN(ATENDIMENTO = 'INTERNAÇÃO') THEN 1 ELSE 0 END) INTERNACOES, SUM(CASE WHEN(DATAALTA IS NULL AND ATENDIMENTO = 'INTERNAÇÃO') THEN 1 ELSE 0 END) INTERNACOESATIVAS,");
                        strSql.AppendLine("SUM(CASE WHEN(ATENDIMENTO = 'HOMECARE') THEN 1 ELSE 0 END) HOMECARE, SUM(CASE WHEN(ATENDIMENTO = 'AMBULATÓRIO/EMERGÊNCIA') THEN 1 ELSE 0 END) AMBULATORIOEMERGENCIA,SUM(CASE WHEN(ATENDIMENTO = 'PRÉ-ATENDIMENTO') THEN 1 ELSE 0 END) PREATENDIMENTOS,");
                        strSql.AppendLine("SUM(CASE WHEN(ATENDIMENTO = 'NÃO DEFINIDO') THEN 1 ELSE 0 END) INDEFINIDOS, SUM(CASE WHEN(DATAALTA IS NOT NULL) THEN 1 ELSE 0 END) COMALTA, SUM(CASE WHEN(DATAALTA IS NULL) THEN 1 ELSE 0 END) SEMALTA");
                        strSql.AppendLine("FROM vwRptAteDetalhado VW");
                        if (tipoPeriodo == 1)
                        {
                            strSql.AppendLine(string.Format("WHERE DATAATENDIMENTO BETWEEN CAST('{0}' AS DATE) AND DATEADD(DAY,1,CAST('{1}' AS DATE))", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd")));
                        }
                        else
                        {
                            strSql.AppendLine(string.Format("WHERE DATAALTA IS NOT NULL AND DATAALTA BETWEEN CAST('{0}' AS DATE) AND DATEADD(DAY,1,CAST('{1}' AS DATE))", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd")));

                        }
                        if (empresaId > 0)
                        {
                            strSql.AppendLine(string.Format("AND EMPRESAID={0}", empresaId));
                        }
                        else
                        {
                            ///Alterar
                            // strSql.AppendLine(string.Format("AND EMPRESAID in .........", empresaId));
                        }

                        if (medicoId > 0)
                        {
                            strSql.AppendLine(string.Format("AND MEDICOID={0}", medicoId));
                        }

                        if (especialidadeId > 0)
                        {
                            strSql.AppendLine(string.Format("AND ESPECIALIDADEID={0}", especialidadeId));
                        }

                        if (unidadeOrganizacionalId > 0)
                        {
                            strSql.AppendLine(string.Format("AND UNIDADEORGANIZACIONALID={0}", unidadeOrganizacionalId));
                        }
                        else
                        {
                            ///Alterar
                            //strSql.AppendLine(string.Format("AND UNIDADEORGANIZACIONALID in .........", unidadeOrganizacionalId));
                        }

                        if (pacienteId > 0)
                        {
                            strSql.AppendLine(string.Format("AND PACIENTEID={0}", pacienteId));
                        }

                        if (convenioId > 0)
                        {
                            strSql.AppendLine(string.Format("AND CONVENIOID={0}", convenioId));
                        }

                        switch (tipoAtendimento)
                        {
                            case 0:
                                // strSql.AppendLine(string.Format("AND ATENDIMENTO={0}", convenioId));
                                break;
                            case 1:
                                strSql.AppendLine(string.Format("AND ATENDIMENTO='{0}'", "AMBULATÓRIO/EMERGÊNCIA"));
                                break;
                            default:
                                strSql.AppendLine(string.Format("AND ATENDIMENTO='{0}'", "INTERNAÇÃO"));
                                break;
                        }

                        strSql.AppendLine("GROUP BY EMPRESAID,EMPRESA,CONVENIOID,CONVENIO,PLANO,MEDICOID,MEDICO,ESPECIALIDADE");


                        db.Database.CommandTimeout = 120;
                        var dr = db.Database.SqlQuery<VWRptAtendimentoResumido>(strSql.ToString()).ToList();
                        db.Dispose();
                        var listDto = VWRptAtendimentoResumidoDto.Mapear(dr).ToList();
                        //db.Dispose();
                        //var result = listDto
                        //    .WhereIf(empresaId > 0, m => m.EmpresaId == empresaId)
                        //    .WhereIf(convenioId > 0, m => m.ConvenioId == convenioId)
                        //    .WhereIf(medicoId > 0, m => m.MedicoId == medicoId)
                        //    .ToList();
                        return new ListResultDto<VWRptAtendimentoResumidoDto> { Items = listDto };
                    }
                }
                else
                {
                    throw new Exception(this.L("TenantNaoEncontrado"));
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<long?> ObterAtendindimentoAbertoPaciente(long pacienteId)
        {
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                var atedimento = await atendimentoRepository.Object.GetAll().AsNoTracking()
                                     .FirstOrDefaultAsync(
                                         w => w.PacienteId == pacienteId && w.DataAlta == null && w.IsInternacao)
                                     .ConfigureAwait(false);

                if (atedimento != null)
                {
                    return atedimento.Id;
                }
            }

            return null;
        }

        public async Task InserirItensFaturamentoAgendamento(long? agendamentoId, long contaId, Atendimento atendimento)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            using (var faturamentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
            using (var medicoEspecialidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<MedicoEspecialidade, long>>())
            using (var faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            using (var parametroAppService = IocManager.Instance.ResolveAsDisposable<IParametroAppService>())
            {
                var agendamento = agendamentoCirurgicoRepository
                    .Object.GetAll().Include(i => i.Cirurgias)
                    .Include(i => i.AgendamentoConsulta).FirstOrDefault(w => w.AgendamentoConsultaId == agendamentoId);
                if (agendamento == null)
                {
                    return;
                }

                var medicoEspecialidade = medicoEspecialidadeRepository.Object.GetAll().FirstOrDefault(
                    w => w.MedicoId == atendimento.MedicoId && w.EspecialidadeId == atendimento.EspecialidadeId);

                var itensFaturamento = new List<FaturamentoContaItemDto>();

                foreach (var item in agendamento.Cirurgias)
                {
                    var itemfaturamentoDto = new FaturamentoContaItemDto
                    {
                        FaturamentoContaId = contaId,
                        Id = item.FaturamentoItemId ?? 0,
                        Qtde = 1
                    };

                    var terceirizadoParamentro = parametroAppService.Object.ObterPorCodigo("AGETERCE");
                    if (terceirizadoParamentro != null)
                    {
                        long terceiradoId;

                        if (long.TryParse(terceirizadoParamentro.Descricao, out terceiradoId))
                        {
                            itemfaturamentoDto.TerceirizadoId = terceiradoId;
                        }
                    }
                    var faturamentoItem = faturamentoItemRepository.Object.GetAll().Include(i => i.Grupo)
                        .Include(i => i.Grupo.TipoGrupo).FirstOrDefault(w => w.Id == item.FaturamentoItemId);

                    if (faturamentoItem != null && faturamentoItem.Grupo.TipoGrupo.Descricao == "Honorários")
                    {
                        itemfaturamentoDto.MedicoId = atendimento.MedicoId;

                        itemfaturamentoDto.MedicoEspecialidadeId = medicoEspecialidade?.Id;
                    }

                    itensFaturamento.Add(itemfaturamentoDto);
                }

                var faturamentoContaItemInsertDto = new FaturamentoContaItemInsertDto
                {
                    AtendimentoId = atendimento.Id,
                    ContaId = contaId,
                    Data = agendamento.AgendamentoConsulta.DataAgendamento,
                    MedicoId = atendimento.MedicoId
                };

                var turnoParamentro = parametroAppService.Object.ObterPorCodigo("AGETURNO");
                if (turnoParamentro != null)
                {
                    long turnoId;

                    if (long.TryParse(turnoParamentro.Descricao, out turnoId))
                    {
                        faturamentoContaItemInsertDto.TurnoId = turnoId;
                    }
                }

                agendamento.AgendamentoConsulta.StatusId = 6; // Atendido

                faturamentoContaItemInsertDto.ItensFaturamento = itensFaturamento;

                await faturamentoContaItemAppService.Object.InserirItensContaFaturamento(faturamentoContaItemInsertDto).ConfigureAwait(false);
            }
        }

        public async Task AtualizarAssistencial(long id, long? protocoloAtendimentoId, long? classificacaoRiscoId)
        {
            try
            {
                using (var visualAppService = IocManager.Instance.ResolveAsDisposable<IVisualAppService>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var atendimento = await atendimentoRepository.Object
                                          .GetAll()
                                          .FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

                    if (atendimento != null)
                    {
                        atendimento.ClassificacaoAtendimentoId = classificacaoRiscoId;
                        atendimento.ProtocoloAtendimentoId = protocoloAtendimentoId;
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    visualAppService.Object.MigrarVisualASA(atendimento.Id);
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task AtualizarStatusAssistencial(
            long id,
            long? stausId,
            bool isPedenteExame,
            bool isPedenteMedicacao,
            bool isPedenteProcedimento,
            int? statusAguardando,
            int? statusAtendido)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var atendimento = atendimentoRepository.Object.GetAll().FirstOrDefault(w => w.Id == id);

                    if (atendimento != null)
                    {
                        atendimento.AtendimentoStatusId = stausId;
                        atendimento.IsPendenteExames = isPedenteExame;
                        atendimento.IsPendenteMedicacao = isPedenteMedicacao;
                        atendimento.IsPendenteProcedimento = isPedenteProcedimento;
                        atendimento.StatusAguardando = statusAguardando;
                        atendimento.StatusAtendido = statusAtendido;
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    // _visualAppService.MigrarVisualASA(atendimento.Id);
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task CancelarAlta(long atedimentoId)
        {
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
            {
                var atedimento = atendimentoRepository.Object
                                                       .GetAll()
                                                       .FirstOrDefault(w => w.Id == atedimentoId);

                if (atedimento != null)
                {
                    atedimento.DataAlta = null;



                    var ultimoMovimentoLeito = atendimentoLeitoMovRepository.Object.GetAll().Include(i => i.Leito)
                        .Where(w => w.AtendimentoId == atedimento.Id).OrderByDescending(o => o.CreationTime)
                        .FirstOrDefault();

                    // Status 1 = Vago
                    if (ultimoMovimentoLeito != null && ultimoMovimentoLeito.Leito != null && ultimoMovimentoLeito.Leito.LeitoStatusId == 1)
                    {
                        ultimoMovimentoLeito.DataFinal = null;
                        ultimoMovimentoLeito.Leito.LeitoStatusId = 2;
                        atedimento.LeitoId = ultimoMovimentoLeito.Leito.Id;
                    }
                    else
                    {
                        atedimento.LeitoId = null;
                    }
                }
            }
        }

        public async Task<AtendimentoDto> ObterAtendimentoLeitoPaciente(long id)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var m = await atendimentoRepository.Object.GetAll().AsNoTracking().Include(a => a.Paciente)
                                .Include(a => a.Paciente.SisPessoa).Include(a => a.Leito).AsNoTracking()
                                .FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

                    var atendimento = AtendimentoDto.Mapear(m);

                    return atendimento;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task AlterarMedicoAtendimento(long atendimentoId)
        {
            try
            {
                using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Authorization.Users.User, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var usuario = usuarioRepository.Object.GetAll().Include(i => i.Medico)
                        .Include(i => i.Medico.MedicoEspecialidades)
                        .AsNoTracking()
                        .FirstOrDefault(w => w.Id == this.AbpSession.UserId);

                    if (usuario != null && usuario.MedicoId != null && usuario.Medico.MedicoEspecialidades != null && usuario.Medico.MedicoEspecialidades.Count > 0)
                    {
                        var especialidadeId = usuario.Medico.MedicoEspecialidades.First().EspecialidadeId;

                        var atendimento = atendimentoRepository.Object.GetAll().FirstOrDefault(w => w.Id == atendimentoId);

                        if (atendimento != null)
                        {
                            atendimento.MedicoId = usuario.MedicoId;
                            atendimento.EspecialidadeId = especialidadeId;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }


        }


        [UnitOfWork(false)]
        public async Task<AtendimentoDto> ObterComPacienteEndereco(long id)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var senhaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Senha, long>>())
                {
                    var aQuery = atendimentoRepository.Object.GetAll().Include(a => a.Paciente)
                                .Include(a => a.Paciente.SisPessoa.Enderecos)
                                .Include(a => a.Paciente.SisPessoa.Enderecos.Select(x => x.Estado))
                                .Include(a => a.Paciente.SisPessoa.Enderecos.Select(x => x.Cidade))
                                .Include(a => a.Paciente.SisPessoa.Enderecos.Select(x => x.Pais))
                                .Include(a => a.Paciente.SisPessoa).Include(a => a.Paciente.SisPessoa.Sexo)
                                .Include(a => a.Paciente.SisPessoa.Nacionalidade).Include(a => a.Paciente.PacientePesos)
                                //.Include(a => a.Paciente.PacienteDiagnosticos)
                                //.Include(a => a.Paciente.PacienteDiagnosticos.Select(x => x.GrupoCID))
                                //.Include(a => a.Paciente.PacienteAlergias)

                                // .Include(a => a.Paciente.Sexo)
                                //.Include(a => a.Paciente.SisPessoa.Enderecos).Include(a => a.Paciente.Pais)
                                //.Include(a => a.Paciente.Estado).Include(a => a.Paciente.Cidade)
                                .Include(a => a.Paciente.EstadoCivil).Include(a => a.Paciente.Profissao)
                                .Include(a => a.Medico).Include(a => a.Medico.Conselho).Include(a => a.Medico.SisPessoa)
                                .Include(a => a.AtendimentoTipo).Include(a => a.Convenio)
                                .Include(a => a.Convenio.IdentificacoesPrestadoresNaOperadora)
                                .Include(a => a.Convenio.SisPessoa).Include(a => a.Empresa)
                                .Include(a => a.Especialidade)
                                // .Include(a => a.Guia) // modelo antigo
                                .Include(a => a.FatGuia) // novo modelo FatGuia
                                .Include(a => a.TipoAcompanhante).Include(a => a.Leito)
                                .Include(a => a.Leito.TipoAcomodacao)
                                // .Include(a => a.MotivoAlta).Include(a => a.Nacionalidade).Include(a => a.Origem)
                                .Include(a => a.Plano).Include(a => a.ServicoMedicoPrestado)
                                .Include(a => a.UnidadeOrganizacional).Include(a => a.TipoAcomodacao)
                                .Include(a => a.CaraterAtendimento).Include(a => a.IndicacaoAcidente).AsNoTracking();

                    var atendimento = AtendimentoDto.Mapear(await aQuery.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false));

                    var senhaAtendimento = await senhaRepository.Object.GetAll().AsNoTracking()
                                               .FirstOrDefaultAsync(w => w.AtendimentoId == atendimento.Id)
                                               .ConfigureAwait(false);

                    if (senhaAtendimento != null)
                    {
                        atendimento.SenhaAtendimento = SenhaDto.Mapear(senhaAtendimento);
                    }

                    return atendimento;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<AtendimentoDto> ObterParaProrrogacao(long id)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var m = await atendimentoRepository.Object.GetAll().Include(a => a.Paciente)
                                .Include(a => a.Paciente.SisPessoa).Include(a => a.Medico)
                                .Include(a => a.Medico.SisPessoa).Include(a => a.Convenio)
                                .Include(a => a.Convenio.FormaAutorizacao).Include(a => a.Convenio.SisPessoa)
                                .AsNoTracking().FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

                    var atendimento = AtendimentoDto.Mapear(m);
                    return atendimento;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PacienteMedicoDto> ObterPacienteMedico(long id)
        {
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                var pacienteMedicoDto = new PacienteMedicoDto();

                var atendimento = await atendimentoRepository.Object.GetAll()
                    .Include(i => i.Paciente.SisPessoa)
                    .Include(i => i.Leito)
                    .Include(i => i.Leito.UnidadeOrganizacional)
                    .Include(i => i.UnidadeOrganizacional)
                    .Include(i => i.Medico.SisPessoa).AsNoTracking().FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

                if (atendimento == null)
                {
                    return pacienteMedicoDto;
                }

                if (atendimento.Paciente != null && atendimento.Paciente.SisPessoa != null)
                {
                    pacienteMedicoDto.PacienteId = atendimento.PacienteId;
                    pacienteMedicoDto.PacienteNome = atendimento.Paciente.SisPessoa.NomeCompleto;
                }

                if (atendimento.Medico != null && atendimento.Medico.SisPessoa != null)
                {
                    pacienteMedicoDto.MedicoId = atendimento.MedicoId;
                    pacienteMedicoDto.MedicoNome = atendimento.Medico.SisPessoa.NomeCompleto;
                }

                if (atendimento.IsAmbulatorioEmergencia)
                {
                    pacienteMedicoDto.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
                    pacienteMedicoDto.UnidadeOrganizacional = atendimento.UnidadeOrganizacional.Descricao;
                }
                else
                {
                    if (atendimento.Leito != null)
                    {
                        pacienteMedicoDto.UnidadeOrganizacionalId = atendimento.Leito.UnidadeOrganizacionalId;
                        pacienteMedicoDto.UnidadeOrganizacional = atendimento.Leito.UnidadeOrganizacional.Descricao;
                    }
                }

                return pacienteMedicoDto;
            }
        }
    }

    public class SetAltaInput
    {
        public long? atendimentoId { get; set; }
        public long? altaMedicaLeitoId { get; set; }
        public long? altaGrupoCidId { get; set; }
        public long? motivoAltaId { get; set; }
        public DateTime dataAltaMedica { get; set; }
        public DateTime dataAlta { get; set; }
        public DateTime? dataPrevisaoAlta { get; set; }

        public DateTime? dataTomadaDecisao { get; set; }
        public string NumeroObito { get; set; }
    }
}