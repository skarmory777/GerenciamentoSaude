using Abp.Runtime.Session;
using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03.GuiasLotes
{
    public class GuiaResumoInternacaoV3_03_03Service : GuiaV3_03_03Service
    {
        private readonly IAbpSession AbpSession;
        private readonly ISessionAppService _sessionService;

        protected DefaultReturn<ctm_internacaoResumoGuia[]> _retornoPadrao;
        public GuiaResumoInternacaoV3_03_03Service(IAbpSession abpSession, ISessionAppService sessionService)
        {
            AbpSession = abpSession;
            _sessionService = sessionService;
        }


        public DefaultReturn<ctm_internacaoResumoGuia[]> GerarGuiaResumoInternacoes(List<FaturamentoEntregaContaDto> faturamentoEntregaConta)
        {

            _retornoPadrao = new DefaultReturn<ctm_internacaoResumoGuia[]>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();


            var _faturamentoContaItemRepository = new SWRepository<FaturamentoContaItem>(AbpSession, _sessionService);


            ctm_internacaoResumoGuia[] resumoInternacaoGuias = new ctm_internacaoResumoGuia[faturamentoEntregaConta.Count];

            int posicao = 0;

            foreach (var item in faturamentoEntregaConta)
            {
                var faturamentoConta = item.ContaMedica;



                var itensConta = _faturamentoContaItemRepository.GetAll()
                                                               .Where(w => w.FaturamentoContaId == item.ContaMedicaId)
                                                                 .Include(i => i.FaturamentoItem)
                                                                 .Include(s => s.FaturamentoItem.Grupo)
                                                                 .Include(s => s.FaturamentoItem.Grupo.FaturamentoCodigoDespesa)
                                                                 .Include(s => s.Anestesista)
                                                                 .Include(s => s.Anestesista.Conselho)
                                                                 .Include(s => s.AnestesistaEspecialidade)
                                                                 .Include(s => s.AnestesistaEspecialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Auxiliar1)
                                                                 .Include(s => s.Auxiliar1.Conselho)
                                                                 .Include(s => s.Auxiliar1Especialidade)
                                                                 .Include(s => s.Auxiliar1Especialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Auxiliar2)
                                                                 .Include(s => s.Auxiliar2.Conselho)
                                                                 .Include(s => s.Auxiliar2Especialidade)
                                                                 .Include(s => s.Auxiliar2Especialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Auxiliar3)
                                                                 .Include(s => s.Auxiliar3.Conselho)
                                                                 .Include(s => s.Auxiliar3Especialidade)
                                                                 .Include(s => s.Auxiliar3Especialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Instrumentador)
                                                                 .Include(s => s.Instrumentador.Conselho)
                                                                 .Include(s => s.InstrumentadorEspecialidade)
                                                                 .Include(s => s.InstrumentadorEspecialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Medico)
                                                                 .Include(s => s.Medico.Conselho)
                                                                 .Include(s => s.MedicoEspecialidade)
                                                                 .Include(s => s.MedicoEspecialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.FaturamentoConfigConvenio);

                var itensContaList = itensConta.ToList();


                List<FaturamentoContaItemDto> itensContaListDto = new List<FaturamentoContaItemDto>();

                foreach (var itemConta in itensContaList)
                {
                    itensContaListDto.Add(FaturamentoContaItemDto.MapearFromCore(itemConta));
                }

                faturamentoConta.Itens = itensContaListDto;


                ValidaDadosXML03_03_03Service validaDadosXML = new ValidaDadosXML03_03_03Service(AbpSession, _sessionService);
                var retorno = validaDadosXML.ValidarGuias(faturamentoConta);
                _retornoPadrao.Errors.AddRange(retorno.Errors);

                if (_retornoPadrao.Errors.Count == 0)
                {

                    ctm_internacaoResumoGuia internacaoResumoGuia = new ctm_internacaoResumoGuia();

                    internacaoResumoGuia.cabecalhoGuia = GerarCabecalhoGuia(faturamentoConta);
                    internacaoResumoGuia.numeroGuiaSolicitacaoInternacao = faturamentoConta.Atendimento.GuiaNumero;
                    internacaoResumoGuia.outrasDespesas = GerarListaOutrasDespesas(faturamentoConta);
                    internacaoResumoGuia.valorTotal = GerarValorTotal(faturamentoConta);
                    internacaoResumoGuia.dadosAutorizacao = GerarAutorizacaoInternacao(faturamentoConta);
                    internacaoResumoGuia.dadosBeneficiario = GerarBeneficiarioDados(faturamentoConta.Atendimento);
                    internacaoResumoGuia.dadosExecutante = GerarDadosExecutante();
                    internacaoResumoGuia.dadosInternacao = GerarDadosInternacao(faturamentoConta);
                    internacaoResumoGuia.dadosSaidaInternacao = GerarDadosSaida(faturamentoConta.Atendimento);
                    internacaoResumoGuia.procedimentosExecutados = GerarProcedimentosExecutadosInternacao(faturamentoConta);

                    resumoInternacaoGuias[posicao++] = internacaoResumoGuia;
                }
            }

            _retornoPadrao.ReturnObject = resumoInternacaoGuias;

            return _retornoPadrao;
        }

        private ct_autorizacaoInternacao GerarAutorizacaoInternacao(FaturamentoContaDto faturamentoConta)
        {
            var atendimento = faturamentoConta.Atendimento;

            ct_autorizacaoInternacao autorizacaoInternacao = new ct_autorizacaoInternacao();

            autorizacaoInternacao.dataAutorizacao = atendimento.DataAutorizacao ?? DateTime.MinValue;

            autorizacaoInternacao.dataValidadeSenhaSpecified = atendimento.ValidadeSenha != null;
            if (autorizacaoInternacao.dataValidadeSenhaSpecified)
            {
                autorizacaoInternacao.dataValidadeSenha = (DateTime)atendimento.ValidadeSenha;
            }

            autorizacaoInternacao.numeroGuiaOperadora = faturamentoConta.GuiaOperadora; ;
            autorizacaoInternacao.senha = atendimento.Senha;


            return autorizacaoInternacao;
        }

        private ctm_internacaoResumoGuiaDadosExecutante GerarDadosExecutante()
        {
            ctm_internacaoResumoGuiaDadosExecutante dadosExecutante = new ctm_internacaoResumoGuiaDadosExecutante();

            return dadosExecutante;
        }

        private ctm_internacaoDados GerarDadosInternacao(FaturamentoContaDto faturamentoConta)
        {
            ctm_internacaoDados dadosInternacao = new ctm_internacaoDados();

            dadosInternacao.dataInicioFaturamento = ((DateTime)faturamentoConta.DataInicio).Date;
            dadosInternacao.dataFinalFaturamento = ((DateTime)faturamentoConta.DataFim).Date;
            dadosInternacao.HoraInicioFaturamento = (DateTime)faturamentoConta.DataInicio;
            dadosInternacao.HoraFinalFaturamento = (DateTime)faturamentoConta.DataFim;

            if (faturamentoConta.Atendimento.CaraterAtendimento != null && !string.IsNullOrEmpty(faturamentoConta.Atendimento.CaraterAtendimento.Codigo))
            {
                dadosInternacao.caraterAtendimento = (dm_caraterAtendimento)FuncoesGlobais.ObterValueEnum(typeof(dm_caraterAtendimento), faturamentoConta.Atendimento.CaraterAtendimento.Codigo, false);
            }

            dadosInternacao.declaracoes = GerarDeclaracoes(faturamentoConta.Atendimento);

            dadosInternacao.tipoFaturamento = faturamentoConta.DataFim < faturamentoConta.Atendimento.DataAlta ? dm_tipoFaturamento.Item1 : dm_tipoFaturamento.Item4;


            return dadosInternacao;
        }

        private ctm_internacaoDadosDeclaracoes[] GerarDeclaracoes(AtendimentoDto atendimento)
        {
            ctm_internacaoDadosDeclaracoes[] declaracoes = new ctm_internacaoDadosDeclaracoes[1];

            ctm_internacaoDadosDeclaracoes internacaoDadosDeclaracoes = new ctm_internacaoDadosDeclaracoes();

            internacaoDadosDeclaracoes.declaracaoObito = atendimento.NumeroObito;
            internacaoDadosDeclaracoes.diagnosticoObito = atendimento.AltaGrupoCID?.Codigo;

            declaracoes[0] = internacaoDadosDeclaracoes;

            return declaracoes;
        }

        private ctm_internacaoDadosSaida GerarDadosSaida(AtendimentoDto atendimento)
        {
            ctm_internacaoDadosSaida dadosSaida = new ctm_internacaoDadosSaida();

            if (atendimento.IndicacaoAcidente != null && !string.IsNullOrEmpty(atendimento.IndicacaoAcidente.Codigo))
            {
                dadosSaida.indicadorAcidente = (dm_indicadorAcidente)FuncoesGlobais.ObterValueEnum(typeof(dm_indicadorAcidente), atendimento.IndicacaoAcidente.Codigo, false);
            }

            if (atendimento.MotivoAlta != null && atendimento.MotivoAlta.MotivoAltaTipoAlta != null && !string.IsNullOrEmpty(atendimento.MotivoAlta.MotivoAltaTipoAlta.Codigo))
            {
                dadosSaida.motivoEncerramento = (dm_motivoSaida)FuncoesGlobais.ObterValueEnum(typeof(dm_motivoSaida), atendimento.MotivoAlta.Codigo, false);
            }

            var atendimentoGrupoCIDRepository = new SWRepository<AtendimentoGrupoCID>(AbpSession, _sessionService);

            var gruposCIDAtendimento = atendimentoGrupoCIDRepository.GetAll()
                                                         .Include(i => i.GrupoCID)
                                                         .Where(w => w.AtendimentoId == atendimento.Id)
                                                         .Select(s => s.GrupoCID.Codigo)
                                                         .ToList();

            if (atendimento.AltaGrupoCID != null && !string.IsNullOrEmpty(atendimento.AltaGrupoCID.Codigo) && !gruposCIDAtendimento.Any(a => a == atendimento.AltaGrupoCID.Codigo))
            {
                gruposCIDAtendimento.Add(atendimento.AltaGrupoCID.Codigo);
            }



            dadosSaida.diagnostico = gruposCIDAtendimento.ToArray();

            return dadosSaida;
        }

        private ct_procedimentoExecutadoInt[] GerarProcedimentosExecutadosInternacao(FaturamentoContaDto faturamentoConta)
        {
            var itensProcedimentosExecutados = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == null).ToList();
            ct_procedimentoExecutadoInt[] procedimentosExecutados = new ct_procedimentoExecutadoInt[itensProcedimentosExecutados.Count];

            int posicao = 0;

            foreach (var item in itensProcedimentosExecutados)
            {
                ct_procedimentoExecutadoInt procedimentoExecutado = new ct_procedimentoExecutadoInt
                {
                    dataExecucao = item.Data ?? DateTimeOffset.MinValue,

                    horaInicialSpecified = item.HoraIncio != null
                };

                if (procedimentoExecutado.horaInicialSpecified)
                {
                    procedimentoExecutado.HoraInicial = (DateTime)item.HoraIncio;
                }

                procedimentoExecutado.horaFinalSpecified = item.HoraFim != null;

                if (procedimentoExecutado.horaFinalSpecified)
                {
                    procedimentoExecutado.HoraFinal = (DateTime)item.HoraFim;
                }

                procedimentoExecutado.quantidadeExecutada = item.Qtde.ToString();

                procedimentoExecutado.viaAcessoSpecified = !string.IsNullOrEmpty(item.ViaAcesso);

                if (procedimentoExecutado.viaAcessoSpecified)
                {
                    procedimentoExecutado.viaAcesso = (dm_viaDeAcesso)FuncoesGlobais.ObterValueEnum(typeof(dm_viaDeAcesso), item.ViaAcesso, false);
                }

                procedimentoExecutado.valorUnitario = Math.Round((decimal)(item.ValorItem), 2);
                procedimentoExecutado.valorTotal = Math.Round((procedimentoExecutado.valorUnitario * (decimal)item.Qtde), 2);

                procedimentoExecutado.procedimento = GerarProcecimentoExecutado(item);
                procedimentoExecutado.identEquipe = GerarIdentificacaoEquipe(item);

                procedimentoExecutado.tecnicaUtilizadaSpecified = !string.IsNullOrEmpty(item.Tecnica);
                if (procedimentoExecutado.tecnicaUtilizadaSpecified)
                {
                    procedimentoExecutado.tecnicaUtilizada = (dm_tecnicaUtilizada)FuncoesGlobais.ObterValueEnum(typeof(dm_tecnicaUtilizada), item.Tecnica, false);
                }

                procedimentosExecutados[posicao++] = procedimentoExecutado;

            }

            return procedimentosExecutados;

        }

        private ct_procedimentoExecutadoIntIdentEquipe[] GerarIdentificacaoEquipe(FaturamentoContaItemDto item)
        {

            List<ct_procedimentoExecutadoIntIdentEquipe> listIdentEquipe = new List<ct_procedimentoExecutadoIntIdentEquipe>();
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Anestesista, item.EspecialidadeAnestesista));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Auxiliar1, item.Auxiliar1Especialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Auxiliar2, item.Auxiliar2Especialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Auxiliar2, item.Auxiliar3Especialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Medico, item.MedicoEspecialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Instrumentador, item.InstrumentadorEspecialidade));

            ct_procedimentoExecutadoIntIdentEquipe[] equipe = new ct_procedimentoExecutadoIntIdentEquipe[listIdentEquipe.Count];

            int posicao = 0;
            foreach (var itemEquipe in listIdentEquipe)
            {
                equipe[posicao++] = itemEquipe;
            }

            return equipe;
        }

        private ct_procedimentoExecutadoIntIdentEquipe GerarIntegranteEquipe(MedicoDto medico, MedicoEspecialidadeDto Medicoespecialidade)
        {
            ct_procedimentoExecutadoIntIdentEquipe identificadorEquipe = null;
            ct_identEquipe integranteEquipe = null;

            if (medico != null)
            {
                identificadorEquipe = new ct_procedimentoExecutadoIntIdentEquipe();

                integranteEquipe = new ct_identEquipe();
                identificadorEquipe.identificacaoEquipe = integranteEquipe;

                if (medico.Conselho != null)
                {
                    integranteEquipe.conselho = (dm_conselhoProfissional)FuncoesGlobais.ObterValueEnum(typeof(dm_conselhoProfissional), medico.Conselho.Codigo, false);
                    integranteEquipe.UF = (dm_UF)FuncoesGlobais.ObterValueEnum(typeof(dm_UF), medico.Conselho.Uf, false);
                }
                integranteEquipe.nomeProf = medico.NomeCompleto;
                integranteEquipe.numeroConselhoProfissional = medico.NumeroConselho.ToString();
                integranteEquipe.codProfissional = new ct_identEquipeCodProfissional { Item = medico.SisPessoa.Cpf, ItemElementName = ItemChoiceType5.cpfContratado };

            }

            if (Medicoespecialidade != null && integranteEquipe != null)
            {
                integranteEquipe.CBOS = (dm_CBOS)FuncoesGlobais.ObterValueEnum(typeof(dm_CBOS), Medicoespecialidade.Especialidade.Codigo, false);
            }



            return identificadorEquipe;
        }
    }
}
