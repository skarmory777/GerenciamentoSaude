using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03.GuiasLotes
{
    public class GuiaV3_03_03Service
    {



        private SWRepository<FaturamentoContaItem> _faturamentoContaItemRepository;



        //protected ct_identEquipeSADT[] GerarEquipeSADT(FaturamentoContaItemDto item)
        //{
        //    List<ct_identEquipeSADT> listIdentEquipeSADT = new List<ct_identEquipeSADT>();

        //    listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Anestesista, item.EspecialidadeAnestesista));
        //    listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Auxiliar1, item.Auxiliar1Especialidade));
        //    listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Auxiliar2, item.Auxiliar2Especialidade));
        //    listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Auxiliar2, item.Auxiliar3Especialidade));
        //    listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Medico, item.MedicoEspecialidade));
        //    listIdentEquipeSADT.Add(GerarIntegranteEquipe(item.Instrumentador, item.InstrumentadorEspecialidade));

        //    listIdentEquipeSADT = listIdentEquipeSADT.Where(w => w != null).ToList();

        //    ct_identEquipeSADT[] identEquipeSADT = new ct_identEquipeSADT[listIdentEquipeSADT.Count];

        //    int posicao = 0;
        //    foreach (var itemEquipe in listIdentEquipeSADT)
        //    {
        //        identEquipeSADT[posicao++] = itemEquipe;
        //    }

        //    return identEquipeSADT;
        //}

        //protected ct_identEquipeSADT GerarIntegranteEquipe(MedicoDto medico, MedicoEspecialidadeDto Medicoespecialidade)
        //{
        //    ct_identEquipeSADT integranteEquipe = null;

        //    if (medico != null)
        //    {
        //        integranteEquipe = new ct_identEquipeSADT();

        //        if (medico.Conselho != null)
        //        {
        //            integranteEquipe.conselho = (dm_conselhoProfissional)FuncoesGlobais.ObterValueEnum(typeof(dm_conselhoProfissional), medico.Conselho.Codigo, false);
        //            integranteEquipe.UF = (dm_UF)FuncoesGlobais.ObterValueEnum(typeof(dm_UF), medico.Conselho.Uf, false);
        //        }
        //        integranteEquipe.nomeProf = medico.NomeCompleto;
        //        integranteEquipe.numeroConselhoProfissional = medico.NumeroConselho.ToString();


        //    }

        //    if (Medicoespecialidade != null && integranteEquipe != null)
        //    {
        //        integranteEquipe.CBOS = (dm_CBOS)FuncoesGlobais.ObterValueEnum(typeof(dm_CBOS), Medicoespecialidade.Especialidade.Codigo, false);
        //    }



        //    return integranteEquipe;
        //}

        protected virtual ct_guiaCabecalho GerarCabecalhoGuia(FaturamentoContaDto faturamentoConta, ct_guiaCabecalho guiaCabecalho = null)
        {
            if (guiaCabecalho == null)
            {
                guiaCabecalho = new ct_guiaCabecalho();
            }
            guiaCabecalho.registroANS = faturamentoConta.Convenio.RegistroANS;

            //if (!string.IsNullOrEmpty(faturamentoConta.Atendimento.GuiaNumero))
            //{
            guiaCabecalho.numeroGuiaPrestador = faturamentoConta.Atendimento.GuiaNumero;//.NumeroGuia;
            //}
            //else
            //{
            //    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0002", Parametros = new List<object> { faturamentoConta.Atendimento.Codigo } });
            //}

            return guiaCabecalho;
        }

        protected ct_outrasDespesasDespesa[] GerarListaOutrasDespesas(FaturamentoContaDto faturamentoConta)
        {
            var itensOutrasDespesas = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId != null).ToList();

            ct_outrasDespesasDespesa[] listOutrasDespesasDespes = null;

            if (itensOutrasDespesas.Count > 0)
            {
                listOutrasDespesasDespes = new ct_outrasDespesasDespesa[itensOutrasDespesas.Count];

                int posicao = 0;

                foreach (var item in itensOutrasDespesas)
                {
                    ct_outrasDespesasDespesa outraDespesa = new ct_outrasDespesasDespesa();

                    outraDespesa.codigoDespesa = (dm_outrasDespesas)FuncoesGlobais.ObterValueEnum(typeof(dm_outrasDespesas), item.FaturamentoItem.Grupo.FaturamentoCodigoDespesa.Codigo, false);
                    outraDespesa.servicosExecutados = GerarProcedimentoExecutadoOutras(item);

                    listOutrasDespesasDespes[posicao++] = outraDespesa;
                }
            }
            return listOutrasDespesasDespes;
        }

        protected ct_procedimentoExecutadoOutras GerarProcedimentoExecutadoOutras(FaturamentoContaItemDto item)
        {
            ct_procedimentoExecutadoOutras procedimentoExecutadoOutras = new ct_procedimentoExecutadoOutras();

            procedimentoExecutadoOutras.codigoRefFabricante = item.FaturamentoItem.Referencia;
            procedimentoExecutadoOutras.codigoTabela = (dm_tabela)FuncoesGlobais.ObterValueEnum(typeof(dm_tabela), item.FaturamentoConfigConvenioDto.Codigo, false);
            procedimentoExecutadoOutras.dataExecucao = item.Data ?? DateTimeOffset.MinValue;
            procedimentoExecutadoOutras.codigoProcedimento = item.FaturamentoItem.CodTuss;
            procedimentoExecutadoOutras.descricaoProcedimento = item.FaturamentoItem.DescricaoTuss;

            procedimentoExecutadoOutras.dataExecucao = item.Data ?? DateTimeOffset.MinValue;

            procedimentoExecutadoOutras.horaInicialSpecified = item.HoraIncio != null;

            if (procedimentoExecutadoOutras.horaInicialSpecified)
            {
                procedimentoExecutadoOutras.HoraInicial = (DateTime)item.HoraIncio;
            }

            procedimentoExecutadoOutras.horaFinalSpecified = item.HoraFim != null;

            if (procedimentoExecutadoOutras.horaFinalSpecified)
            {
                procedimentoExecutadoOutras.HoraFinal = (DateTime)item.HoraFim;
            }

            procedimentoExecutadoOutras.quantidadeExecutada = (decimal)item.Qtde;
            procedimentoExecutadoOutras.valorUnitario = Math.Round((decimal)(item.ValorItem), 2);
            procedimentoExecutadoOutras.valorTotal = Math.Round((procedimentoExecutadoOutras.valorUnitario * procedimentoExecutadoOutras.quantidadeExecutada), 2);

            return procedimentoExecutadoOutras;
        }

        protected ct_guiaValorTotal GerarValorTotal(FaturamentoContaDto faturamentoConta)
        {
            //_faturamentoItemTabelaRepository = new SWRepository<FaturamentoItemTabela>();
            ct_guiaValorTotal guiaValorTotal = new ct_guiaValorTotal();

            var itensOPME = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.OPME).ToList();
            guiaValorTotal.valorOPME = Math.Round(SomaValorContaItens(itensOPME), 2);
            guiaValorTotal.valorOPMESpecified = guiaValorTotal.valorOPME > 0;

            var itensMedicamentos = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Medicamentos).ToList();
            guiaValorTotal.valorMedicamentos = Math.Round(SomaValorContaItens(itensMedicamentos), 2);// 2
            guiaValorTotal.valorMedicamentosSpecified = guiaValorTotal.valorMedicamentos > 0;

            var itensDiarias = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Diarias).ToList();
            guiaValorTotal.valorDiarias = Math.Round(SomaValorContaItens(itensDiarias), 2);// 5
            guiaValorTotal.valorDiariasSpecified = guiaValorTotal.valorDiarias > 0;

            var itensGasesMedicinais = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.GasesMedicinais).ToList();
            guiaValorTotal.valorGasesMedicinais = Math.Round(SomaValorContaItens(itensGasesMedicinais), 2);// 1
            guiaValorTotal.valorGasesMedicinaisSpecified = guiaValorTotal.valorGasesMedicinais > 0;

            var itensMateriais = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Materiais).ToList();
            guiaValorTotal.valorMateriais = Math.Round(SomaValorContaItens(itensMateriais), 2);// 3
            guiaValorTotal.valorMateriaisSpecified = guiaValorTotal.valorMateriais > 0;

            var itensTaxasAlugueis = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.TaxasAluguéis).ToList();
            guiaValorTotal.valorTaxasAlugueis = Math.Round(SomaValorContaItens(itensTaxasAlugueis), 2);// 3
            guiaValorTotal.valorTaxasAlugueisSpecified = guiaValorTotal.valorTaxasAlugueis > 0;

            var itensProcedimentos = faturamentoConta.Itens.Where(w => w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == null).ToList();
            guiaValorTotal.valorProcedimentos = Math.Round(SomaValorContaItens(itensProcedimentos), 2);
            guiaValorTotal.valorProcedimentosSpecified = guiaValorTotal.valorProcedimentos > 0;


            guiaValorTotal.valorTotalGeral = Math.Round(guiaValorTotal.valorOPME
                                           + guiaValorTotal.valorMedicamentos
                                           + guiaValorTotal.valorDiarias
                                           + guiaValorTotal.valorGasesMedicinais
                                           + guiaValorTotal.valorMateriais
                                           + guiaValorTotal.valorTaxasAlugueis
                                           + guiaValorTotal.valorProcedimentos, 2);


            return guiaValorTotal;
        }

        private decimal SomaValorContaItens(List<FaturamentoContaItemDto> itens)
        {
            decimal valorTotal = 0;

            foreach (var item in itens)
            {
                valorTotal += ((decimal)item.ValorItem) * (decimal)item.Qtde;
            }

            return valorTotal;
        }

        protected ct_beneficiarioDados GerarBeneficiarioDados(AtendimentoDto atendimento)
        {
            ct_beneficiarioDados beneficiarioDados = new ct_beneficiarioDados();
            beneficiarioDados.nomeBeneficiario = atendimento.Paciente.NomeCompleto;
            beneficiarioDados.numeroCarteira = atendimento.Matricula;
            if (atendimento.Paciente.Nascimento != null)
            {
                beneficiarioDados.atendimentoRN = FuncoesGlobais.IsRN((DateTime)atendimento.Paciente.Nascimento) ? dm_simNao.S : dm_simNao.N;
            }

            if (!string.IsNullOrEmpty(atendimento.CNS))
            {
                beneficiarioDados.numeroCNS = atendimento.CNS;
            }

            return beneficiarioDados;
        }

        protected ct_procedimentoDados GerarProcecimentoExecutado(FaturamentoContaItemDto item)
        {
            ct_procedimentoDados procedimentoExecutado = new ct_procedimentoDados();

            procedimentoExecutado.codigoProcedimento = item.FaturamentoItem.CodTuss;

            if (!string.IsNullOrEmpty(item.FaturamentoItem.DescricaoTuss))
            {
                procedimentoExecutado.descricaoProcedimento = item.FaturamentoItem.DescricaoTuss;
            }

            if (item.FaturamentoConfigConvenioDto != null && !string.IsNullOrEmpty(item.FaturamentoConfigConvenioDto.Codigo))
            {
                procedimentoExecutado.codigoTabela = (dm_tabela)FuncoesGlobais.ObterValueEnum(typeof(dm_tabela), item.FaturamentoConfigConvenioDto.Codigo, false);
            }

            return procedimentoExecutado;
        }

    }
}
