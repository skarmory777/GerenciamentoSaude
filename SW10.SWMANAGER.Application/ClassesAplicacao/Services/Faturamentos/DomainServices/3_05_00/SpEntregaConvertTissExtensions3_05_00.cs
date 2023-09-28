using Castle.Core.Internal;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices._3_05_00
{
    public static partial class SpEntregaConvertTissExtensions3_05_00 {
        

        #region Cabecalho
        public static ISpEntregaConvertTiss TissCabecalho(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            if (entrega == null || entrega.Lote == null)
            {
                throw new TissException("Cabeçalho não definido");
            }

            return convertTissInstance
                .Adiciona("<ans:cabecalho>")
                    .TissCabecalhoIdentificacaoTransacao(entrega)
                    .TissCabecalhoOrigem(entrega)
                    .TissCabecalhoDestino(entrega)
                    .TissCabecalhoPadrao(entrega)
                .Adiciona("</ans:cabecalho>");
        }

        private static ISpEntregaConvertTiss TissCabecalhoIdentificacaoTransacao(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:identificacaoTransacao>
                <ans:tipoTransacao>{0}</ans:tipoTransacao>
                <ans:sequencialTransacao>{1}</ans:sequencialTransacao>
                <ans:dataRegistroTransacao>{2}</ans:dataRegistroTransacao>
                <ans:horaRegistroTransacao>{3}</ans:horaRegistroTransacao>
            </ans:identificacaoTransacao>", 
            entrega.Lote.TipoTransacao, entrega.Lote.SequencialTransacao, entrega.Lote.DataRegistroTransacao, entrega.Lote.HoraRegistroTransacao);
        }

        private static ISpEntregaConvertTiss TissCabecalhoOrigem(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            return convertTissInstance.AdicionaFormatado(@"
                <ans:origem>
                    <ans:identificacaoPrestador>
                    <ans:{1}>{0}</ans:{1}>
                    </ans:identificacaoPrestador>
                </ans:origem>", entrega.Lote.CodigoPrestadorNaOperadora,entrega.Lote.ContratadoDados);
        }

        private static ISpEntregaConvertTiss TissCabecalhoDestino(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            return convertTissInstance.AdicionaFormatado(@"<ans:destino><ans:registroANS>{0}</ans:registroANS></ans:destino>", entrega.Lote.RegistroANS);
        }

        private static ISpEntregaConvertTiss TissCabecalhoPadrao(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            return convertTissInstance.AdicionaFormatado(@"<ans:Padrao>{0}</ans:Padrao>", entrega.Lote.VersaoPadrao);
        }
        #endregion


        public static ISpEntregaConvertTiss TissPrestadorParaOperadora(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega, Func<ISpEntregaConvertTiss, SpEntrega, ISpEntregaConvertTiss> action)
        {
            return convertTissInstance
                .Adiciona(@"<ans:prestadorParaOperadora> <ans:loteGuias>")
                .TissPrestadorParaOperadoraNumeroLote(entrega.Lote.NumeroLote)
                .TissPrestadorParaOperadoraGuias(entrega, action)
                .Adiciona("</ans:loteGuias></ans:prestadorParaOperadora>");
        }


        #region PrestadorParaOperadora

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraNumeroLote(this ISpEntregaConvertTiss convertTissInstance, string numeroLote)
        {
            return convertTissInstance.AdicionaFormatado("<ans:numeroLote>{0}</ans:numeroLote>", numeroLote);
        }
        #endregion

        #region Guias
        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuias(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega,Func<ISpEntregaConvertTiss,SpEntrega, ISpEntregaConvertTiss> action)
        {
            convertTissInstance = convertTissInstance.Adiciona(@"<ans:guiasTISS>");
            return action(convertTissInstance, entrega).Adiciona(@"</ans:guiasTISS>");
        }
        
        private static void ValorTotalProcedimentos(SpEntregaLoteGuiasDto guia, SpEntregaLoteGuiasProcRealizadosDto procRealizado)
        {
            if(procRealizado == null)
            {
                return;
            }

            if (guia.GuiaValorTotal == null)
            {
                guia.GuiaValorTotal = new SpEntregaLoteGuiaValorTotalDto();
            }

            guia.GuiaValorTotal.ValorProcedimentos += procRealizado.ValorTotal;
        }
        private static void ValorTotalOutrasDespesas(SpEntregaLoteGuiasDto guia, SpEntregaLoteGuiasOutrasDespesasDto despesa)
        {
            if (despesa == null)
            {
                return;
            }

            if (guia.GuiaValorTotal == null)
            {
                guia.GuiaValorTotal = new SpEntregaLoteGuiaValorTotalDto();
            }

            switch (despesa.TipoDespesa)
            {
                //Gases Medicinais
                case "01":
                    {
                        guia.GuiaValorTotal.ValorGasesMedicinais += despesa.ValorTotal;
                        break;
                    }

                //Medicamentos
                case "02":
                    {
                        guia.GuiaValorTotal.ValorMedicamentos += despesa.ValorTotal;
                        break;
                    }

                //Materiais
                case "03":
                    {
                        guia.GuiaValorTotal.ValorMateriais += despesa.ValorTotal;
                        break;
                    }

                //Diarias
                case "05":
                    {
                        guia.GuiaValorTotal.ValorDiarias += despesa.ValorTotal;
                        break;
                    }

                //Taxas e Alugueis
                case "07":
                    {
                        guia.GuiaValorTotal.ValorTaxasAlugueis += despesa.ValorTotal;
                        break;
                    }

                //OPME
                case "08":
                    {
                        guia.GuiaValorTotal.ValorOPME += despesa.ValorTotal;
                        break;
                    }
            }
        }



        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasItemDadosAutorizacao(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosAutorizacao>
                <ans:dataAutorizacao>{0}</ans:dataAutorizacao>
                <ans:senha>{1}</ans:senha>
                <ans:dataValidadeSenha>{2}</ans:dataValidadeSenha>
            </ans:dadosAutorizacao>", guia.DataAutorizacao, guia.SenhaAutorizacao, guia.ValidadeSenha);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasItemDadosBeneficiario(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosBeneficiario>
              <ans:numeroCarteira>{0}</ans:numeroCarteira>
              <ans:atendimentoRN>{1}</ans:atendimentoRN>
              <ans:nomeBeneficiario>{2}</ans:nomeBeneficiario>
            </ans:dadosBeneficiario>", guia.NumeroCarteira, guia.AtendimentoRN, guia.NomeBeneficiario);
        }


        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasItemOutrasDespesas(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia, ref int sequencialItem)
        {
            if (guia.OutrasDespesas.IsNullOrEmpty())
            {
                return convertTissInstance;
            }
            convertTissInstance = convertTissInstance.Adiciona("<ans:outrasDespesas>");
            foreach (var despesa in guia.OutrasDespesas)
            {
                despesa.SequencialItem = sequencialItem;
                ValorTotalOutrasDespesas(guia, despesa);
                convertTissInstance = convertTissInstance
                        .TissPrestadorParaOperadoraGuiasItemOutrasDespesasItem(despesa);
                sequencialItem += 1;
            }
            convertTissInstance = convertTissInstance.Adiciona("</ans:outrasDespesas>");

            return convertTissInstance;
        }


        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasItemOutrasDespesasItem(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasOutrasDespesasDto despesa)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:despesa>
                <ans:sequencialItem>{0}</ans:sequencialItem>
                <ans:codigoDespesa>{1}</ans:codigoDespesa>
                <ans:servicosExecutados>
                    <ans:dataExecucao>{2}</ans:dataExecucao>
                    <ans:codigoTabela>{3}</ans:codigoTabela>
                    <ans:codigoProcedimento>{4}</ans:codigoProcedimento>
                    <ans:quantidadeExecutada>{5}</ans:quantidadeExecutada>
                    <ans:unidadeMedida>{6}</ans:unidadeMedida>
                    <ans:reducaoAcrescimo>{7}</ans:reducaoAcrescimo>
                    <ans:valorUnitario>{8}</ans:valorUnitario>
                    <ans:valorTotal>{9}</ans:valorTotal>
                    <ans:descricaoProcedimento>{10}</ans:descricaoProcedimento>
                </ans:servicosExecutados>
            </ans:despesa>",
              despesa.SequencialItem, despesa.TipoDespesa, despesa.DataRealizacao,
              despesa.TipoTabela, despesa.Codigo,
              convertTissInstance.FormataNumero(despesa.Quantidade),
              despesa.CodUnidadeANS,
              despesa.PercTaxas == "0" ? "1.00" : despesa.PercTaxas,
              convertTissInstance.FormataNumero(despesa.ValorUnitario),
              convertTissInstance.FormataNumero(despesa.ValorTotal), despesa.Descricao);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasItemValorTotal(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiaValorTotalDto dto)
        {

            return convertTissInstance.AdicionaFormatado(@"
            <ans:valorTotal>
              <ans:valorProcedimentos>{0}</ans:valorProcedimentos>
              <ans:valorDiarias>{1}</ans:valorDiarias>
              <ans:valorTaxasAlugueis>{2}</ans:valorTaxasAlugueis>
              <ans:valorMateriais>{3}</ans:valorMateriais>
              <ans:valorMedicamentos>{4}</ans:valorMedicamentos>
              <ans:valorOPME>{5}</ans:valorOPME>
              <ans:valorGasesMedicinais>{6}</ans:valorGasesMedicinais>
              <ans:valorTotalGeral>{7}</ans:valorTotalGeral>
            </ans:valorTotal>",
            convertTissInstance.FormataNumero(dto.ValorProcedimentos),
            convertTissInstance.FormataNumero(dto.ValorDiarias),
            convertTissInstance.FormataNumero(dto.ValorTaxasAlugueis),
            convertTissInstance.FormataNumero(dto.ValorMateriais),
            convertTissInstance.FormataNumero(dto.ValorMedicamentos),
            convertTissInstance.FormataNumero(dto.ValorOPME),
            convertTissInstance.FormataNumero(dto.ValorGasesMedicinais),
            convertTissInstance.FormataNumero(dto.ValorTotalGeral)
            );
        }


        #endregion



        //TODO: Calculo do hash
        public static ISpEntregaConvertTiss TissEpilogo(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            entrega.EpilogoHash = convertTissInstance.CalculaHashMD5();
            convertTissInstance.Adiciona(string.Format(@"<ans:epilogo> <ans:hash>{0}</ans:hash> </ans:epilogo>"
                .Replace(Environment.NewLine, "")
                .Replace("  ", " "), entrega.EpilogoHash));
            return convertTissInstance;
        }

        public static ISpEntregaConvertTiss TissAbreTag(this ISpEntregaConvertTiss convertTissInstance)
        {
            return convertTissInstance.Adiciona(@"<ans:mensagemTISS xmlns:ans=""http://www.ans.gov.br/padroes/tiss/schemas"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.ans.gov.br/padroes/tiss/schemas http://www.ans.gov.br/padroes/tiss/schemas/tissV3_05_00.xsd"">");
        }

        public static ISpEntregaConvertTiss TissFechaTag(this ISpEntregaConvertTiss convertTissInstance)
        {
            return convertTissInstance.Adiciona("</ans:mensagemTISS>");
        }
    }
}