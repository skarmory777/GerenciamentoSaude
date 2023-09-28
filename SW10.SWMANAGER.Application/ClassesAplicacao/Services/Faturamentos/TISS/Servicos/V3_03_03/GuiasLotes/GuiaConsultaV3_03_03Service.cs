using Abp.Runtime.Session;
using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Sessions;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03.GuiasLotes
{
    public class GuiaConsultaV3_03_03Service : GuiaV3_03_03Service
    {
        private readonly IAbpSession AbpSession;
        private readonly ISessionAppService _sessionService;

        protected DefaultReturn<ctm_consultaGuia[]> _retornoPadrao;


        public GuiaConsultaV3_03_03Service(IAbpSession abpSession
                                       , ISessionAppService sessionService)
        {
            AbpSession = abpSession;
            _sessionService = sessionService;
        }



        public DefaultReturn<ctm_consultaGuia[]> GerarGuiaConsulta(List<FaturamentoEntregaContaDto> faturamentoEntregaConta)
        {
            _retornoPadrao = new DefaultReturn<ctm_consultaGuia[]>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            var _faturamentoContaItemRepository = new SWRepository<FaturamentoContaItem>(AbpSession, _sessionService);
            ctm_consultaGuia[] consultaGuias = new ctm_consultaGuia[faturamentoEntregaConta.Count];

            int posicao = 0;

            foreach (var item in faturamentoEntregaConta)
            {
                var faturamentoConta = item.ContaMedica;


                var itensConta = _faturamentoContaItemRepository.GetAll()
                                                              .Where(w => w.FaturamentoContaId == item.ContaMedicaId)
                                                                .Include(i => i.FaturamentoItem)
                                                                .Include(s => s.FaturamentoItem.Grupo)
                                                                .Include(s => s.FaturamentoItem.Grupo.FaturamentoCodigoDespesa)
                                                                .Include(i => i.FaturamentoItem)
                                                                .Include(i => i.FaturamentoConfigConvenio);

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
                    ctm_consultaGuia consultaGuia = new ctm_consultaGuia();

                    consultaGuia.cabecalhoConsulta = GerarCabecalhoGuia(faturamentoConta); //GerarCabecalho(faturamentoConta);
                    consultaGuia.contratadoExecutante = GerarContratoExecutante(faturamentoConta);
                    consultaGuia.dadosAtendimento = GerarDadosAtendimento(faturamentoConta);
                    consultaGuia.indicacaoAcidente = FuncoesGlobais.ObterValueEnumType<dm_indicadorAcidente>(faturamentoConta.Atendimento.IndicacaoAcidente?.Codigo, false);
                    consultaGuia.dadosBeneficiario = GerarBeneficiarioDados(faturamentoConta.Atendimento);
                    consultaGuia.numeroGuiaOperadora = faturamentoConta.Atendimento.GuiaNumero;
                    consultaGuia.profissionalExecutante = GerarDadosProficionalExecutante(faturamentoConta.Atendimento);

                    if (!string.IsNullOrEmpty(faturamentoConta.Atendimento.Observacao))
                    {
                        consultaGuia.observacao = faturamentoConta.Atendimento.Observacao;
                    }

                    consultaGuias[posicao++] = consultaGuia;
                }
            }

            _retornoPadrao.ReturnObject = consultaGuias;

            return _retornoPadrao;
        }

        private ct_guiaCabecalho GerarCabecalho(FaturamentoContaDto faturamentoConta)
        {
            ct_guiaCabecalho cabecalho = new ct_guiaCabecalho();

            cabecalho.registroANS = faturamentoConta.Atendimento.Convenio.RegistroANS;

            return cabecalho;
        }

        private ctm_consultaGuiaContratadoExecutante GerarContratoExecutante(FaturamentoContaDto faturamentoConta)
        {
            ctm_consultaGuiaContratadoExecutante contratadoExecutante = new ctm_consultaGuiaContratadoExecutante();

            contratadoExecutante.nomeContratado = faturamentoConta.Atendimento.Empresa.RazaoSocial;
            contratadoExecutante.ItemElementName = ItemChoiceType1.cnpjContratado;
            contratadoExecutante.Item = faturamentoConta.Atendimento.Empresa.Cnpj;
            contratadoExecutante.CNES = faturamentoConta.Atendimento.Empresa.Cnes.ToString();


            return contratadoExecutante;
        }

        private ctm_consultaAtendimento GerarDadosAtendimento(FaturamentoContaDto faturamentoConta)
        {
            ctm_consultaAtendimento consultaAtendimento = new ctm_consultaAtendimento();

            consultaAtendimento.dataAtendimento = faturamentoConta.Atendimento.DataRegistro;
            consultaAtendimento.tipoConsulta = FuncoesGlobais.ObterValueEnumType<dm_tipoConsulta>(faturamentoConta.Atendimento.ServicoMedicoPrestado.Codigo, false);
            consultaAtendimento.procedimento = GerarProcedimentoConsulta(faturamentoConta);

            return consultaAtendimento;
        }

        private ctm_consultaAtendimentoProcedimento GerarProcedimentoConsulta(FaturamentoContaDto faturamentoConta)
        {
            ctm_consultaAtendimentoProcedimento consultaAtendimentoProcedimento = new ctm_consultaAtendimentoProcedimento();

            var procedimentoExecutado = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == null).FirstOrDefault();

            if (procedimentoExecutado != null)
            {
                consultaAtendimentoProcedimento.codigoProcedimento = procedimentoExecutado.FaturamentoItem.CodTuss;
                consultaAtendimentoProcedimento.valorProcedimento = (decimal)procedimentoExecutado.ValorItem;

                if (procedimentoExecutado.FaturamentoConfigConvenioDto != null && !string.IsNullOrEmpty(procedimentoExecutado.FaturamentoConfigConvenioDto.Codigo))
                {
                    consultaAtendimentoProcedimento.codigoTabela = (dm_tabela)FuncoesGlobais.ObterValueEnum(typeof(dm_tabela), procedimentoExecutado.FaturamentoConfigConvenioDto.Codigo, false);
                }
            }





            return consultaAtendimentoProcedimento;
        }

        private ct_contratadoProfissionalDados GerarDadosProficionalExecutante(AtendimentoDto atendimento)
        {
            ct_contratadoProfissionalDados contratadoProfissionalDados = new ct_contratadoProfissionalDados();

            contratadoProfissionalDados.nomeProfissional = atendimento.Medico.NomeCompleto;
            contratadoProfissionalDados.numeroConselhoProfissional = atendimento.Medico.NumeroConselho.ToString();

            //if (atendimento.Medico.Conselho != null && !string.IsNullOrEmpty(atendimento.Medico.Conselho.Codigo))
            //{
            contratadoProfissionalDados.conselhoProfissional = FuncoesGlobais.ObterValueEnumType<dm_conselhoProfissional>(atendimento.Medico.Conselho.Codigo, false);
            contratadoProfissionalDados.UF = FuncoesGlobais.ObterValueEnumType<dm_UF>(atendimento.Medico.Conselho.Uf, false);
            //}
            //else
            //{
            //    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0001", Parametros = new List<object> { atendimento.Medico.NomeCompleto } });
            //}


            if (!string.IsNullOrEmpty(atendimento.Especialidade.Cbo))
            {
                contratadoProfissionalDados.CBOS = FuncoesGlobais.ObterValueEnumType<dm_CBOS>(atendimento.Especialidade.Cbo, false);
            }


            return contratadoProfissionalDados;
        }

    }
}
