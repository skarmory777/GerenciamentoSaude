using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices._3_05_00
{
    public static partial class SpEntregaConvertTissExtensions3_05_00
    {
        public static ISpEntregaConvertTiss TissGuiasResumoInternacao(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            foreach (var guia in entrega.Guias)
            {
                convertTissInstance
                    .Adiciona(@"<ans:guiaResumoInternacao>")
                        .TissPrestadorParaOperadoraGuiasResumoInternacao(guia, entrega.Lote.CodigoPrestadorNaOperadora)
                    .Adiciona("</ans:guiaResumoInternacao>");
            }
            return convertTissInstance;
        }

        public static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasResumoInternacao(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia, string codigoPrestadorNaOperadora)
        {
            var sequencialItem = 1;
            return convertTissInstance
                .TissPrestadorParaOperadoraGuiasResumoInternacaoItemCabecalho(guia)
                .TissPrestadorParaOperadoraGuiasItemDadosAutorizacao(guia)
                .TissPrestadorParaOperadoraGuiasItemDadosBeneficiario(guia)
                .TissPrestadorParaOperadoraGuiasItemDadosExecutante(guia, codigoPrestadorNaOperadora)
                .TissPrestadorParaOperadoraGuiasResumoInternacaoDadosInternacao(guia)
                .TissPrestadorParaOperadoraGuiasResumoInternacaoDadosSaidaInternacao(guia)
                .TissPrestadorParaOperadoraGuiasResumoInternacaoItemProcedimentosExecutados(guia,ref sequencialItem)
                .TissPrestadorParaOperadoraGuiasItemOutrasDespesas(guia, ref sequencialItem)
                .TissPrestadorParaOperadoraGuiasItemValorTotal(guia.GuiaValorTotal);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasResumoInternacaoItemCabecalho(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:cabecalhoGuia>
              <ans:registroANS>{0}</ans:registroANS>
              <ans:numeroGuiaPrestador>{1}</ans:numeroGuiaPrestador>
              <ans:guiaPrincipal>{2}</ans:guiaPrincipal>
            </ans:cabecalhoGuia>", guia.RegistroANS, guia.NumeroGuiaPrestador, guia.NumeroGuiaPrincipal);
        }

        static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasResumoInternacaoDadosInternacao(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosInternacao>
              <ans:caraterAtendimento>{0}</ans:caraterAtendimento>
              <ans:tipoFaturamento>{1}</ans:tipoFaturamento>
              <ans:dataInicioFaturamento>{2}</ans:dataInicioFaturamento>
              <ans:horaInicioFaturamento>{3}</ans:horaInicioFaturamento>
              <ans:dataFinalFaturamento>{4}</ans:dataFinalFaturamento>
              <ans:horaFinalFaturamento>{5}</ans:horaFinalFaturamento>
              <ans:tipoInternacao>{6}</ans:tipoInternacao>
              <ans:regimeInternacao>{7}</ans:regimeInternacao>
            </ans:dadosInternacao>",
            guia.CaraterAtendimento, guia.TipoFaturamento, guia.DataInicioFaturamento,
            guia.HoraFinalFaturamento, guia.DataFinalFaturamento, guia.HoraFinalFaturamento,
            guia.TipoInternacao,guia.RegimeInternacao);
        }

        static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasResumoInternacaoDadosSaidaInternacao(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia) {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosSaidaInternacao>
              <ans:diagnostico>{0}</ans:diagnostico>
              <ans:indicadorAcidente>{1}</ans:indicadorAcidente>
              <ans:motivoEncerramento>{2}</ans:motivoEncerramento>
            </ans:dadosSaidaInternacao>", guia.CodigoDiagnostico, guia.IndicadorAcidente, guia.MotivoSaidaInternacao);
        }

        static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasResumoInternacaoItemProcedimentosExecutados(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia, ref int sequencialItem)
        {
            if (guia.ProcRealizados.IsNullOrEmpty())
            {
                return convertTissInstance;
            }
            convertTissInstance = convertTissInstance.Adiciona("<ans:procedimentosExecutados>");
            foreach (var procRealizado in guia.ProcRealizados)
            {
                procRealizado.SequencialItem = sequencialItem;
                ValorTotalProcedimentos(guia, procRealizado);
                convertTissInstance = convertTissInstance.TissPrestadorParaOperadoraGuiasResumoInternacaoItemProcedimentosExecutadosItem(procRealizado, guia.MembroEquipes);
                sequencialItem += 1;
            }
            convertTissInstance = convertTissInstance.Adiciona("</ans:procedimentosExecutados>");

            return convertTissInstance;
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasResumoInternacaoItemProcedimentosExecutadosItem(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasProcRealizadosDto procRealizado, IEnumerable<SpEntregaLoteGuiasMembroEquipeDto> membroEquipes)
        {
            var medico = membroEquipes.FirstOrDefault(x => x.SisMedicoId == procRealizado.IDMedico);
            var aux1 = membroEquipes.FirstOrDefault(x => x.SisMedicoId == procRealizado.IDAuxiliar1);
            var aux2 = membroEquipes.FirstOrDefault(x => x.SisMedicoId == procRealizado.IDAuxiliar2);
            var aux3 = membroEquipes.FirstOrDefault(x => x.SisMedicoId == procRealizado.IDAuxiliar3);
            var instr = membroEquipes.FirstOrDefault(x => x.SisMedicoId == procRealizado.IDInstrumentador);

            if (medico != null)
            {
                return CriaDados(
                    convertTissInstance,
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal),
                    procRealizado.PosicaoProfissional,
                    medico.Cpf, medico.NomeExecutante, medico.SiglaConselho, medico.NumeroConselho, medico.UfConselho);
            }

            if (aux1 != null)
            {
                return CriaDados(
                    convertTissInstance,
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal),
                    procRealizado.PosicaoProfissional,
                    aux1.Cpf, aux1.NomeExecutante, aux1.SiglaConselho, aux1.NumeroConselho, aux1.UfConselho);
            }

            if (aux2 != null)
            {
                return CriaDados(
                    convertTissInstance,
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal),
                    procRealizado.PosicaoProfissional,
                    aux2.Cpf, aux2.NomeExecutante, aux2.SiglaConselho, aux2.NumeroConselho, aux2.UfConselho);
            }

            if (aux3 != null)
            {
                return CriaDados(
                    convertTissInstance,
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal),
                    procRealizado.PosicaoProfissional,
                    aux3.Cpf, aux3.NomeExecutante, aux3.SiglaConselho, aux3.NumeroConselho, aux3.UfConselho);
            }

            if (instr != null)
            {
                return CriaDados(
                    convertTissInstance,
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal),
                    procRealizado.PosicaoProfissional,
                    instr.Cpf, instr.NomeExecutante, instr.SiglaConselho, instr.NumeroConselho, instr.UfConselho);
            }

            if (medico == null && aux1 == null && aux2 == null && aux3 == null && instr == null)
            {
                return CriaDadosSemEquipe(convertTissInstance,
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal));
            }

            return convertTissInstance;

            ISpEntregaConvertTiss CriaDados(ISpEntregaConvertTiss convertTissInstance,
                long sequencialItem, string data, string horaInicio, string horaFim, string tipoTabela, string codigoProcedimento,
                string descricao, string quantidade, string percTaxas, string valor, string valorTotal,
                string posicaoProfissional, string cpf, string nomeExecutante, string siglaConselho, string numeroConselho, string ufConselho)
            {
                return convertTissInstance
                .Adiciona("<ans:procedimentoExecutado>")
                    .AdicionaFormatado(@"
                    <ans:sequencialItem>{0}</ans:sequencialItem>
                    <ans:dataExecucao>{1}</ans:dataExecucao>
                    <ans:horaInicial>{2}</ans:horaInicial>
                    <ans:horaFinal>{3}</ans:horaFinal>
                    <ans:procedimento>
                        <ans:codigoTabela>{4}</ans:codigoTabela>
                        <ans:codigoProcedimento>{5}</ans:codigoProcedimento>
                        <ans:descricaoProcedimento>{6}</ans:descricaoProcedimento>
                    </ans:procedimento>
                    <ans:quantidadeExecutada>{7}</ans:quantidadeExecutada>
                    <ans:reducaoAcrescimo>{8}</ans:reducaoAcrescimo>
                    <ans:valorUnitario>{9}</ans:valorUnitario>
                    <ans:valorTotal>{10}</ans:valorTotal>",
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal))
                    .TissPrestadorParaOperadoraGuiasResumoInternacaoItemProcedimentosExecutadosItemEquipe(posicaoProfissional, cpf, nomeExecutante, siglaConselho, numeroConselho, ufConselho)
                .Adiciona("</ans:procedimentoExecutado>");
            }
            ISpEntregaConvertTiss CriaDadosSemEquipe(ISpEntregaConvertTiss convertTissInstance,
                long sequencialItem, string data, string horaInicio, string horaFim, string tipoTabela, string codigoProcedimento,
                string descricao, string quantidade, string percTaxas, string valor, string valorTotal)
            {
                return convertTissInstance
                .Adiciona("<ans:procedimentoExecutado>")
                    .AdicionaFormatado(@"
                    <ans:sequencialItem>{0}</ans:sequencialItem>
                    <ans:dataExecucao>{1}</ans:dataExecucao>
                    <ans:horaInicial>{2}</ans:horaInicial>
                    <ans:horaFinal>{3}</ans:horaFinal>
                    <ans:procedimento>
                        <ans:codigoTabela>{4}</ans:codigoTabela>
                        <ans:codigoProcedimento>{5}</ans:codigoProcedimento>
                        <ans:descricaoProcedimento>{6}</ans:descricaoProcedimento>
                    </ans:procedimento>
                    <ans:quantidadeExecutada>{7}</ans:quantidadeExecutada>
                    <ans:reducaoAcrescimo>{8}</ans:reducaoAcrescimo>
                    <ans:valorUnitario>{9}</ans:valorUnitario>
                    <ans:valorTotal>{10}</ans:valorTotal>",
                    procRealizado.SequencialItem, procRealizado.Data, procRealizado.HoraInicio, procRealizado.HoraFim,
                    procRealizado.TipoTabela, procRealizado.CodigoProcedimento, procRealizado.Descricao,
                    convertTissInstance.FormataNumero(procRealizado.Quantidade),
                    procRealizado.PercTaxas == "0" ? "1.00" : procRealizado.PercTaxas,
                    convertTissInstance.FormataNumero(procRealizado.Valor),
                    convertTissInstance.FormataNumero(procRealizado.ValorTotal))
                .Adiciona("</ans:procedimentoExecutado>");
            }
        }

        static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasResumoInternacaoItemProcedimentosExecutadosItemEquipe(this ISpEntregaConvertTiss convertTissInstance, string posicaoProfissional, string cpf, string nomeExecutante, string siglaConselho, string numeroConselho, string ufConselho)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:identEquipe>
                <ans:identificacaoEquipe>
                  <ans:grauPart>{0}</ans:grauPart>
                  <ans:codProfissional>
                    <ans:codigoPrestadorNaOperadora>{1}</ans:codigoPrestadorNaOperadora>
                  </ans:codProfissional>
                  <ans:nomeProf>{2}</ans:nomeProf>
                  <ans:conselho>{3}</ans:conselho>
                  <ans:numeroConselhoProfissional>{4}</ans:numeroConselhoProfissional>
                  <ans:UF>{5}</ans:UF>
                  <ans:CBOS>{6}</ans:CBOS>
                </ans:identificacaoEquipe>
            </ans:identEquipe>",
            posicaoProfissional,
            cpf,
            nomeExecutante,
            siglaConselho,
            numeroConselho,
            ufConselho,
            0); //procRealizado.cbos)
        }

    }
}
