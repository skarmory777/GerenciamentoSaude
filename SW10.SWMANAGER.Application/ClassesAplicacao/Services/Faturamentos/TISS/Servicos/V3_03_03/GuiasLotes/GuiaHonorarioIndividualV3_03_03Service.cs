using Abp.Runtime.Session;
using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
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
    public class GuiaHonorarioIndividualV3_03_03Service : GuiaV3_03_03Service
    {

        private readonly IAbpSession AbpSession;
        private readonly ISessionAppService _sessionService;


        protected DefaultReturn<ctm_honorarioIndividualGuia[]> _retornoPadrao;

        public GuiaHonorarioIndividualV3_03_03Service(IAbpSession abpSession
                                       , ISessionAppService sessionService)
        {
            AbpSession = abpSession;
            _sessionService = sessionService;
        }


        public DefaultReturn<ctm_honorarioIndividualGuia[]> GerarGuiaHonorarioIndividual(List<FaturamentoEntregaContaDto> faturamentoEntregaContas, string codigoPrestadorNaOperadora)
        {
            _retornoPadrao = new DefaultReturn<ctm_honorarioIndividualGuia[]>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();


            ctm_honorarioIndividualGuia[] honorarioIndividualGuias = new ctm_honorarioIndividualGuia[faturamentoEntregaContas.Count];

            var _faturamentoContaItemRepository = new SWRepository<FaturamentoContaItem>(AbpSession, _sessionService);

            int posicao = 0;

            foreach (var item in faturamentoEntregaContas)
            {
                var contaMedica = item.ContaMedica;


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

                contaMedica.Itens = itensContaListDto;


                ValidaDadosXML03_03_03Service validaDadosXML = new ValidaDadosXML03_03_03Service(AbpSession, _sessionService);
                var retorno = validaDadosXML.ValidarGuias(contaMedica);
                _retornoPadrao.Errors.AddRange(retorno.Errors);

                if (_retornoPadrao.Errors.Count == 0)
                {

                    ctm_honorarioIndividualGuia honorarioIndividualGuia = new ctm_honorarioIndividualGuia();

                    honorarioIndividualGuia.beneficiario = GerarBeneficiario(contaMedica.Atendimento);
                    honorarioIndividualGuia.cabecalhoGuia = GerarCabecalhoGuia(contaMedica);
                    honorarioIndividualGuia.dadosContratadoExecutante = GerarContratoExecutante(contaMedica.Atendimento, codigoPrestadorNaOperadora);
                    honorarioIndividualGuia.dadosInternacao = GerarDadosInternacao(contaMedica);
                    honorarioIndividualGuia.dataEmissaoGuia = contaMedica.Atendimento.DataRegistro;
                    // honorarioIndividualGuia.guiaSolicInternacao
                    honorarioIndividualGuia.localContratado = GerarLocalContratado(contaMedica.Atendimento);
                    honorarioIndividualGuia.observacao = contaMedica.Atendimento.Observacao;
                    honorarioIndividualGuia.senha = contaMedica.Atendimento.Senha;
                    honorarioIndividualGuia.procedimentosRealizados = GerarProcedimentosExecutados(contaMedica);
                    honorarioIndividualGuia.valorTotalHonorarios = GerarValorTotalHonorarios(contaMedica);

                    honorarioIndividualGuias[posicao++] = honorarioIndividualGuia;
                }
            }

            _retornoPadrao.ReturnObject = honorarioIndividualGuias;

            return _retornoPadrao;
        }

        private ctm_honorarioIndividualGuiaBeneficiario GerarBeneficiario(AtendimentoDto atendimento)
        {
            ctm_honorarioIndividualGuiaBeneficiario beneficiario = new ctm_honorarioIndividualGuiaBeneficiario();

            beneficiario.nomeBeneficiario = atendimento.Paciente.NomeCompleto;
            beneficiario.numeroCarteira = atendimento.Matricula;
            beneficiario.atendimentoRN = FuncoesGlobais.IsRN((DateTime)atendimento.Paciente.Nascimento) ? dm_simNao.S : dm_simNao.N;

            return beneficiario;
        }

        private ctm_honorarioIndividualGuiaDadosContratadoExecutante GerarContratoExecutante(AtendimentoDto atendimento, string codigoPrestadorNaOperadora)
        {
            ctm_honorarioIndividualGuiaDadosContratadoExecutante contratoExecutante = new ctm_honorarioIndividualGuiaDadosContratadoExecutante();

            contratoExecutante.cnesContratadoExecutante = atendimento.Empresa.Cnes.ToString();
            contratoExecutante.codigonaOperadora = codigoPrestadorNaOperadora;
            contratoExecutante.nomeContratadoExecutante = atendimento.Empresa.RazaoSocial;

            return contratoExecutante;
        }

        private ctm_honorarioIndividualGuiaDadosInternacao GerarDadosInternacao(FaturamentoContaDto contaMedica)
        {
            ctm_honorarioIndividualGuiaDadosInternacao internacao = new ctm_honorarioIndividualGuiaDadosInternacao();

            internacao.dataInicioFaturamento = (DateTime)contaMedica.DataInicio;
            internacao.dataFimFaturamento = contaMedica.DataFim ?? DateTime.Now;

            return internacao;
        }

        private ctm_honorarioIndividualGuiaLocalContratado GerarLocalContratado(AtendimentoDto atendimento)
        {
            ctm_honorarioIndividualGuiaLocalContratado localContratado = new ctm_honorarioIndividualGuiaLocalContratado();

            localContratado.cnes = atendimento.Empresa.Cnes.ToString();
            localContratado.nomeContratado = atendimento.Empresa.RazaoSocial;
            localContratado.codigoContratado = GerarCodigoContratado();

            return localContratado;
        }

        private ct_procedimentoExecutadoHonorIndiv[] GerarProcedimentosExecutados(FaturamentoContaDto contaMedica)
        {
            ct_procedimentoExecutadoHonorIndiv[] procedimentos = new ct_procedimentoExecutadoHonorIndiv[contaMedica.Itens.Count];

            int posicao = 0;

            foreach (var item in contaMedica.Itens)
            {
                var procedimento = new ct_procedimentoExecutadoHonorIndiv
                {
                    dataExecucao = item.Data,
                    HoraFinal = (DateTime)item.HoraIncio,
                    HoraInicial = (DateTime)item.HoraIncio,
                    quantidadeExecutada = item.Qtde.ToString(),
                    procedimento = GerarProcecimentoExecutado(item),
                    profissionais = GerarProfissionais(item),

                    tecnicaUtilizadaSpecified = !string.IsNullOrEmpty(item.Tecnica)
                };
                if (procedimento.tecnicaUtilizadaSpecified)
                {
                    procedimento.tecnicaUtilizada = (dm_tecnicaUtilizada)FuncoesGlobais.ObterValueEnum(typeof(dm_tecnicaUtilizada), item.Tecnica, false);
                }


                procedimentos[posicao++] = procedimento;
            }

            return procedimentos;
        }

        private ctm_honorarioIndividualGuiaLocalContratadoCodigoContratado GerarCodigoContratado()
        {
            ctm_honorarioIndividualGuiaLocalContratadoCodigoContratado codigoContratado = new ctm_honorarioIndividualGuiaLocalContratadoCodigoContratado();



            return codigoContratado;
        }

        private ct_procedimentoExecutadoHonorIndivProfissionais[] GerarProfissionais(FaturamentoContaItemDto item)
        {

            List<ct_procedimentoExecutadoHonorIndivProfissionais> listIdentEquipe = new List<ct_procedimentoExecutadoHonorIndivProfissionais>();
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Anestesista, item.EspecialidadeAnestesista));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Auxiliar1, item.Auxiliar1Especialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Auxiliar2, item.Auxiliar2Especialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Auxiliar2, item.Auxiliar3Especialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Medico, item.MedicoEspecialidade));
            listIdentEquipe.Add(GerarIntegranteEquipe(item.Instrumentador, item.InstrumentadorEspecialidade));

            ct_procedimentoExecutadoHonorIndivProfissionais[] profissionais = new ct_procedimentoExecutadoHonorIndivProfissionais[listIdentEquipe.Count];

            int posicao = 0;
            foreach (var itemEquipe in listIdentEquipe)
            {
                profissionais[posicao++] = itemEquipe;
            }
            return profissionais;
        }

        private ct_procedimentoExecutadoHonorIndivProfissionais GerarIntegranteEquipe(MedicoDto medico, MedicoEspecialidadeDto Medicoespecialidade)
        {
            ct_procedimentoExecutadoHonorIndivProfissionais identificadorEquipe = null;
            //ct_identEquipe integranteEquipe = null;

            if (medico != null)
            {
                identificadorEquipe = new ct_procedimentoExecutadoHonorIndivProfissionais();

                //integranteEquipe = new ct_identEquipe();
                //identificadorEquipe.identificacaoEquipe = integranteEquipe;




                if (medico.Conselho != null)
                {
                    identificadorEquipe.conselhoProfissional = (dm_conselhoProfissional)FuncoesGlobais.ObterValueEnum(typeof(dm_conselhoProfissional), medico.Conselho.Codigo, false);
                    identificadorEquipe.UF = (dm_UF)FuncoesGlobais.ObterValueEnum(typeof(dm_UF), medico.Conselho.Uf, false);
                }
                identificadorEquipe.nomeProfissional = medico.NomeCompleto;
                identificadorEquipe.numeroConselhoProfissional = medico.NumeroConselho.ToString();
                identificadorEquipe.codProfissional = new ct_procedimentoExecutadoHonorIndivProfissionaisCodProfissional { Item = medico.SisPessoa.Cpf, ItemElementName = ItemChoiceType7.cpfContratado };

            }

            if (Medicoespecialidade != null && identificadorEquipe != null)
            {
                identificadorEquipe.CBO = (dm_CBOS)FuncoesGlobais.ObterValueEnumType<dm_CBOS>(Medicoespecialidade.Especialidade.Codigo, false);

                //FuncoesGlobais.Preencher(identificadorEquipe.CBO, Medicoespecialidade.Especialidade.Codigo);

            }



            return identificadorEquipe;
        }

        private decimal GerarValorTotalHonorarios(FaturamentoContaDto contaMedica)
        {
            decimal valorTotal = 0;

            valorTotal = (decimal)contaMedica.Itens.Sum(s => s.ValorItem = s.Qtde);



            return valorTotal;
        }

    }
}
