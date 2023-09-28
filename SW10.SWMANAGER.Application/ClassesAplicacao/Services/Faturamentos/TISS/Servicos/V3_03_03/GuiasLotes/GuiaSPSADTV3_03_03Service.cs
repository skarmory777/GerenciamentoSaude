using Abp.Runtime.Session;

using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela;
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
    public class GuiaSPSADTV3_03_03Service : GuiaV3_03_03Service
    {
        private SWRepository<FaturamentoItemTabela> _faturamentoItemTabelaRepository;
        private SWRepository<FaturamentoContaItem> _faturamentoContaItemRepository;

        private readonly IAbpSession AbpSession;
        private readonly ISessionAppService _sessionService;

        protected DefaultReturn<ctm_spsadtGuia[]> _retornoPadrao;

        public GuiaSPSADTV3_03_03Service(IAbpSession abpSession
                                       , ISessionAppService sessionService)
        {
            AbpSession = abpSession;
            _sessionService = sessionService;
        }



        public DefaultReturn<ctm_spsadtGuia[]> GerarGuiaSPSADT(List<FaturamentoEntregaContaDto> faturamentoEntregaConta, string codigoPrestadorNaOperadora)
        {
            _retornoPadrao = new DefaultReturn<ctm_spsadtGuia[]>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();


            _faturamentoContaItemRepository = new SWRepository<FaturamentoContaItem>(AbpSession, _sessionService);

            ctm_spsadtGuia[] ctm_spsadtGuia = new ctm_spsadtGuia[faturamentoEntregaConta.Count];

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
                                                                 .Include(s => s.Anestesista.SisPessoa)
                                                                 .Include(s => s.Anestesista.Conselho)
                                                                 .Include(s => s.AnestesistaEspecialidade)
                                                                 .Include(s => s.AnestesistaEspecialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Auxiliar1)
                                                                 .Include(s => s.Auxiliar1.SisPessoa)
                                                                 .Include(s => s.Auxiliar1.Conselho)
                                                                 .Include(s => s.Auxiliar1Especialidade)
                                                                 .Include(s => s.Auxiliar1Especialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Auxiliar2)
                                                                 .Include(s => s.Auxiliar2.SisPessoa)
                                                                 .Include(s => s.Auxiliar2.Conselho)
                                                                 .Include(s => s.Auxiliar2Especialidade)
                                                                 .Include(s => s.Auxiliar2Especialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Auxiliar3)
                                                                 .Include(s => s.Auxiliar3.SisPessoa)
                                                                 .Include(s => s.Auxiliar3.Conselho)
                                                                 .Include(s => s.Auxiliar3Especialidade)
                                                                 .Include(s => s.Auxiliar3Especialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Instrumentador)
                                                                 .Include(s => s.Instrumentador.SisPessoa)
                                                                 .Include(s => s.Instrumentador.Conselho)
                                                                 .Include(s => s.InstrumentadorEspecialidade)
                                                                 .Include(s => s.InstrumentadorEspecialidade.Especialidade.SisCbo)
                                                                 .Include(s => s.Medico)
                                                                 .Include(s => s.Medico.SisPessoa)
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


                    ctm_spsadtGuia spsadtGuia = new ctm_spsadtGuia();

                    var _atendimentoRepository = new SWRepository<Atendimento>(AbpSession, _sessionService);

                    spsadtGuia.cabecalhoGuia = (ctm_spsadtGuiaCabecalhoGuia)GerarCabecalhoGuia(faturamentoConta);
                    spsadtGuia.dadosAtendimento = GerarAtendimento(faturamentoConta.Atendimento);
                    spsadtGuia.dadosBeneficiario = GerarBeneficiarioDados(faturamentoConta.Atendimento);
                    spsadtGuia.dadosAutorizacao = GerarAutorizacaoSADT(faturamentoConta.Atendimento);
                    spsadtGuia.valorTotal = GerarValorTotal(faturamentoConta);
                    spsadtGuia.outrasDespesas = GerarListaOutrasDespesas(faturamentoConta);
                    spsadtGuia.procedimentosExecutados = GerarProcedimentosExecutados(faturamentoConta);
                    spsadtGuia.dadosSolicitante = GerarDadosSolicitante(faturamentoConta, codigoPrestadorNaOperadora);
                    spsadtGuia.dadosSolicitacao = GerarDadosSolicitacao(faturamentoConta.Atendimento);
                    spsadtGuia.dadosExecutante = GerarDadosExecutante(faturamentoConta.Atendimento, codigoPrestadorNaOperadora);

                    ctm_spsadtGuia[posicao++] = spsadtGuia;

                    //spsadtGuia.dadosSolicitante
                    //    spsadtGuia.dadosSolicitacao
                    //    spsadtGuia.

                }
            }

            _retornoPadrao.ReturnObject = ctm_spsadtGuia;

            return _retornoPadrao;
        }


        private ctm_spsadtAtendimento GerarAtendimento(AtendimentoDto atendimento)
        {
            var dadosAtendimento = new ctm_spsadtAtendimento();

            if (atendimento.IsAmbulatorioEmergencia)
            {
                dadosAtendimento.tipoAtendimento = FuncoesGlobais.ObterValueEnumType<dm_tipoAtendimento>(atendimento.AtendimentoTipo.TabelaDominio.Codigo, false);
            }


            //Verificar a possibilidade d criar seed
            if (atendimento.MotivoAlta != null && atendimento.MotivoAlta.MotivoAltaTipoAlta != null && atendimento.MotivoAlta.MotivoAltaTipoAlta.Codigo == "002")
            {
                dadosAtendimento.motivoEncerramentoSpecified = true;
                dadosAtendimento.motivoEncerramento = (dm_motivoSaidaObito)FuncoesGlobais.ObterValueEnum(typeof(dm_motivoSaidaObito), atendimento.MotivoAlta.Codigo, false);
            }

            if (atendimento.IndicacaoAcidente != null && !string.IsNullOrEmpty(atendimento.IndicacaoAcidente.Codigo))
            {
                dadosAtendimento.indicacaoAcidente = (dm_indicadorAcidente)FuncoesGlobais.ObterValueEnum(typeof(dm_indicadorAcidente), atendimento.IndicacaoAcidente.Codigo, false);
            }

            return dadosAtendimento;
        }

        private ct_autorizacaoSADT GerarAutorizacaoSADT(AtendimentoDto atendimento)
        {
            if (!string.IsNullOrEmpty(atendimento.Senha))
            {
                ct_autorizacaoSADT autorizacaoSADT = new ct_autorizacaoSADT();

                autorizacaoSADT.dataAutorizacao = atendimento.DataAutorizacao ?? DateTime.MinValue;

                autorizacaoSADT.dataValidadeSenhaSpecified = atendimento.ValidadeSenha != null;
                if (autorizacaoSADT.dataValidadeSenhaSpecified)
                {
                    autorizacaoSADT.dataValidadeSenha = (DateTime)atendimento.ValidadeSenha;
                }

                autorizacaoSADT.numeroGuiaOperadora = atendimento.GuiaNumero;
                autorizacaoSADT.senha = atendimento.Senha;
                autorizacaoSADT.dataValidadeSenha = atendimento.ValidadeSenha ?? DateTime.MinValue;

                return autorizacaoSADT;
            }
            else
            {
                return null;
            }
        }

        private ct_procedimentoExecutadoSadt[] GerarProcedimentosExecutados(FaturamentoContaDto faturamentoConta)
        {
            var itensProcedimentosExecutados = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == null).ToList();
            ct_procedimentoExecutadoSadt[] listProcedimentosExecutados = null;


            if (itensProcedimentosExecutados.Count > 0)
            {
                listProcedimentosExecutados = new ct_procedimentoExecutadoSadt[itensProcedimentosExecutados.Count];

                int posicao = 0;

                foreach (var item in itensProcedimentosExecutados)
                {

                    ct_procedimentoExecutadoSadt procedimentoExecutado = new ct_procedimentoExecutadoSadt();

                    procedimentoExecutado.dataExecucao = item.Data ?? DateTimeOffset.MinValue;

                    procedimentoExecutado.horaInicialSpecified = item.HoraIncio != null;

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


                    //var tabelaId = item.FaturamentoConfigConvenioDto.TabelaId;

                    //var fatItemTabela = _faturamentoItemTabelaRepository.GetAll()
                    //                                .Where(w => w.ItemId == item.FaturamentoItemId
                    //                                        && w.TabelaId == tabelaId)
                    //                                .FirstOrDefault();

                    //if (fatItemTabela != null)
                    //{
                    //    procedimentoExecutado.valorUnitario = (decimal)(fatItemTabela.Preco);
                    //    procedimentoExecutado.valorTotal = (procedimentoExecutado.valorUnitario * (decimal)item.Qtde);
                    //}

                    procedimentoExecutado.valorUnitario = Math.Round((decimal)(item.ValorItem), 2);
                    procedimentoExecutado.valorTotal = Math.Round((procedimentoExecutado.valorUnitario * (decimal)item.Qtde), 2);
                    procedimentoExecutado.reducaoAcrescimo = 1 + (item.ValorItem != 0 ? ((decimal)(item.ValorTaxas / item.ValorItem)) : 0);

                    procedimentoExecutado.procedimento = GerarProcecimentoExecutado(item);
                    procedimentoExecutado.equipeSadt = GerarEquipeSADT(item);

                    procedimentoExecutado.tecnicaUtilizadaSpecified = !string.IsNullOrEmpty(item.Tecnica);
                    if (procedimentoExecutado.tecnicaUtilizadaSpecified)
                    {
                        procedimentoExecutado.tecnicaUtilizada = (dm_tecnicaUtilizada)FuncoesGlobais.ObterValueEnum(typeof(dm_tecnicaUtilizada), item.Tecnica, false);
                    }

                    listProcedimentosExecutados[posicao++] = procedimentoExecutado;
                }
            }
            return listProcedimentosExecutados;
        }

        private ct_identEquipeSADT[] GerarEquipeSADT(FaturamentoContaItemDto item)
        {
            List<ct_identEquipeSADT> listIdentEquipeSADT = new List<ct_identEquipeSADT>();

            listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Anestesista, item.EspecialidadeAnestesista));
            listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Auxiliar1, item.Auxiliar1Especialidade));
            listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Auxiliar2, item.Auxiliar2Especialidade));
            listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Auxiliar3, item.Auxiliar3Especialidade));
            listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Medico, item.MedicoEspecialidade));
            listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Instrumentador, item.InstrumentadorEspecialidade));

            listIdentEquipeSADT = listIdentEquipeSADT.Where(w => w != null).ToList();

            ct_identEquipeSADT[] identEquipeSADT = new ct_identEquipeSADT[listIdentEquipeSADT.Count];

            int posicao = 0;
            foreach (var itemEquipe in listIdentEquipeSADT)
            {
                identEquipeSADT[posicao++] = itemEquipe;
            }

            return identEquipeSADT;
        }

        private ct_identEquipeSADT GerarIntegranteEquipe(MedicoDto medico, MedicoEspecialidadeDto Medicoespecialidade)
        {
            ct_identEquipeSADT integranteEquipe = null;

            if (medico != null)
            {
                integranteEquipe = new ct_identEquipeSADT();

                if (medico.Conselho != null)
                {
                    integranteEquipe.conselho = FuncoesGlobais.ObterValueEnumType<dm_conselhoProfissional>(medico.Conselho.Codigo, false);
                    integranteEquipe.UF = FuncoesGlobais.ObterValueEnumType<dm_UF>(medico.Conselho.Uf, false);
                }
                integranteEquipe.nomeProf = medico.NomeCompleto;
                integranteEquipe.numeroConselhoProfissional = medico.NumeroConselho.ToString();
                integranteEquipe.codProfissional = new ct_identEquipeSADTCodProfissional { Item = medico.SisPessoa.Cpf, ItemElementName = ItemChoiceType4.cpfContratado };

            }

            if (Medicoespecialidade != null && integranteEquipe != null)
            {
                integranteEquipe.CBOS = FuncoesGlobais.ObterValueEnumType<dm_CBOS>(Medicoespecialidade.Especialidade.Codigo, false);
            }


            return integranteEquipe;
        }

        protected override ct_guiaCabecalho GerarCabecalhoGuia(FaturamentoContaDto faturamentoConta, ct_guiaCabecalho guiaCabecalho = null)
        {
            ctm_spsadtGuiaCabecalhoGuia cabecalhoSPSADT = new ctm_spsadtGuiaCabecalhoGuia();

            base.GerarCabecalhoGuia(faturamentoConta, cabecalhoSPSADT);

            cabecalhoSPSADT.guiaPrincipal = faturamentoConta.GuiaPrincipal;

            return cabecalhoSPSADT;
        }

        private ctm_spsadtGuiaDadosSolicitante GerarDadosSolicitante(FaturamentoContaDto faturamentoConta, string codigoPrestadorNaOperadora)
        {
            ctm_spsadtGuiaDadosSolicitante spsadtGuiaDadosSolicitante = new ctm_spsadtGuiaDadosSolicitante();

            ct_contratadoDados contratadoDados = new ct_contratadoDados();

            contratadoDados.nomeContratado = faturamentoConta.Empresa.NomeFantasia;
            contratadoDados.ItemElementName = ItemChoiceType1.codigoPrestadorNaOperadora;
            contratadoDados.Item = codigoPrestadorNaOperadora;

            spsadtGuiaDadosSolicitante.contratadoSolicitante = contratadoDados;



            ct_contratadoProfissionalDados contratadoProfissionalDados = new ct_contratadoProfissionalDados();

            if (faturamentoConta.Atendimento.Medico.Conselho != null && !string.IsNullOrEmpty(faturamentoConta.Atendimento.Medico.Conselho.Codigo))
            {
                contratadoProfissionalDados.conselhoProfissional = FuncoesGlobais.ObterValueEnumType<dm_conselhoProfissional>(faturamentoConta.Atendimento.Medico.Conselho.Codigo, false);
            }
            contratadoProfissionalDados.nomeProfissional = faturamentoConta.Atendimento.Medico.SisPessoa.NomeCompleto;
            contratadoProfissionalDados.numeroConselhoProfissional = faturamentoConta.Atendimento.Medico.NumeroConselho.ToString();

            if (faturamentoConta.Atendimento.Medico.Conselho != null && !string.IsNullOrEmpty(faturamentoConta.Atendimento.Medico.Conselho.Uf))
            {
                contratadoProfissionalDados.UF = FuncoesGlobais.ObterValueEnumType<dm_UF>(faturamentoConta.Atendimento.Medico.Conselho.Uf, false);
            }

            if (faturamentoConta.Atendimento.Especialidade != null && string.IsNullOrEmpty(faturamentoConta.Atendimento.Especialidade.Codigo))
            {
                contratadoProfissionalDados.CBOS = FuncoesGlobais.ObterValueEnumType<dm_CBOS>(faturamentoConta.Atendimento.Especialidade.Codigo, false);
            }

            spsadtGuiaDadosSolicitante.profissionalSolicitante = contratadoProfissionalDados;

            return spsadtGuiaDadosSolicitante;

        }

        private ct_contratadoDados GerarDadosContratados(AtendimentoDto atendimento, string codigoPrestadorNaOperadora)
        {
            ct_contratadoDados contratadoDados = new ct_contratadoDados();

            contratadoDados.ItemElementName = ItemChoiceType1.codigoPrestadorNaOperadora;
            contratadoDados.Item = codigoPrestadorNaOperadora;
            contratadoDados.nomeContratado = atendimento.Empresa.RazaoSocial;

            return contratadoDados;
        }

        private ctm_spsadtGuiaDadosSolicitacao GerarDadosSolicitacao(AtendimentoDto atendimento)
        {
            ctm_spsadtGuiaDadosSolicitacao solicitacao = new ctm_spsadtGuiaDadosSolicitacao();

            solicitacao.caraterAtendimento = FuncoesGlobais.ObterValueEnumType<dm_caraterAtendimento>(atendimento.CaraterAtendimento.Codigo, false);

            return solicitacao;
        }

        private ctm_spsadtGuiaDadosExecutante GerarDadosExecutante(AtendimentoDto atendimento, string codigoPrestadorNaOperadora)
        {
            ctm_spsadtGuiaDadosExecutante executante = new ctm_spsadtGuiaDadosExecutante();

            executante.CNES = atendimento.Empresa.Cnes.ToString();
            executante.contratadoExecutante = GerarDadosContratados(atendimento, codigoPrestadorNaOperadora);

            return executante;
        }

    }



}
