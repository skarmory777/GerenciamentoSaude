using Castle.Core.Internal;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices._3_05_00
{
    public static partial class SpEntregaConvertTissExtensions3_05_00
    {

        public static ISpEntregaConvertTiss TissGuiasSPSADT(this ISpEntregaConvertTiss convertTissInstance, SpEntrega entrega)
        {
            foreach (var guia in entrega.Guias)
            {
                convertTissInstance
                    .Adiciona(@"<ans:guiaSP-SADT>")
                        .TissPrestadorParaOperadoraGuiasSPSADTItem(guia, entrega.Lote.CodigoPrestadorNaOperadora)
                    .Adiciona("</ans:guiaSP-SADT>");
            }
            return convertTissInstance;
        }


        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasSPSADTItem(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia, string codigoPrestadorNaOperadora)
        {
            var sequencialItem = 1;
            return convertTissInstance
                .TissPrestadorParaOperadoraGuiasItemCabecalho(guia)
                .TissPrestadorParaOperadoraGuiasItemDadosAutorizacao(guia)
                .TissPrestadorParaOperadoraGuiasItemDadosBeneficiario(guia)
                .TissPrestadorParaOperadoraGuiasSPSADTItemDadosSolicitante(guia, codigoPrestadorNaOperadora)
                .TissPrestadorParaOperadoraGuiasSPSADTItemDadosSolicitacao(guia)
                .TissPrestadorParaOperadoraGuiasItemDadosExecutante(guia, codigoPrestadorNaOperadora)
                .TissPrestadorParaOperadoraGuiasSPSADTItemDadosAtendimento(guia)
                .TissPrestadorParaOperadoraGuiasSPSADTItemProcedimentosExecutados(guia, ref sequencialItem)
                .TissPrestadorParaOperadoraGuiasItemOutrasDespesas(guia, ref sequencialItem)
                .TissPrestadorParaOperadoraGuiasItemValorTotal(guia.GuiaValorTotal);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasItemCabecalho(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:cabecalhoGuia>
              <ans:registroANS>{0}</ans:registroANS>
              <ans:numeroGuiaPrestador>{1}</ans:numeroGuiaPrestador>
              <ans:guiaPrincipal>{2}</ans:guiaPrincipal>
            </ans:cabecalhoGuia>", guia.RegistroANS, guia.NumeroGuiaPrestador, guia.NumeroGuiaPrincipal);
        }

        

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasSPSADTItemDadosSolicitante(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia, string codigoPrestadorNaOperadora)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosSolicitante>
              <ans:contratadoSolicitante>
                <ans:codigoPrestadorNaOperadora>{0}</ans:codigoPrestadorNaOperadora>
                <ans:nomeContratado>{1}</ans:nomeContratado>
              </ans:contratadoSolicitante>
              <ans:profissionalSolicitante>
                  <ans:nomeProfissional>{2}</ans:nomeProfissional>
                  <ans:conselhoProfissional>{3}</ans:conselhoProfissional>
                  <ans:numeroConselhoProfissional>{4}</ans:numeroConselhoProfissional>
                  <ans:UF>{5}</ans:UF>
                  <ans:CBOS>{6}</ans:CBOS> 
              </ans:profissionalSolicitante>
            </ans:dadosSolicitante>", codigoPrestadorNaOperadora, guia.NomeContratado, guia.NomeProfissional, guia.SiglaConselho, guia.NumeroConselho, guia.UfConselho, guia.Cbos);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasSPSADTItemDadosSolicitacao(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosSolicitacao>
              <ans:dataSolicitacao>{0}</ans:dataSolicitacao>
              <ans:caraterAtendimento>{1}</ans:caraterAtendimento>
            </ans:dadosSolicitacao>", guia.DataHoraAtendimento, guia.CaraterAtendimento);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasItemDadosExecutante(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia, string codigoPrestadorNaOperadora)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosExecutante>
              <ans:contratadoExecutante>
                <ans:codigoPrestadorNaOperadora>{0}</ans:codigoPrestadorNaOperadora>
                <ans:nomeContratado>{1}</ans:nomeContratado>
              </ans:contratadoExecutante>
              <ans:CNES>{2}</ans:CNES>
            </ans:dadosExecutante>", codigoPrestadorNaOperadora, guia.NomeContratado, guia.NumeroCNES);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasSPSADTItemDadosAtendimento(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:dadosAtendimento>
              <ans:tipoAtendimento>{0}</ans:tipoAtendimento>
              <ans:indicacaoAcidente>{1}</ans:indicacaoAcidente>
              <ans:tipoConsulta>{2}</ans:tipoConsulta>
            </ans:dadosAtendimento>", guia.TipoInternacao, guia.IndicadorAcidente, guia.TipoConsulta);
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasSPSADTItemProcedimentosExecutados(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasDto guia, ref int sequencialItem)
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
                convertTissInstance = convertTissInstance.TissPrestadorParaOperadoraGuiasSPSADTItemProcedimentosExecutadosItem(procRealizado, guia.MembroEquipes);
                sequencialItem += 1;
            }
            convertTissInstance = convertTissInstance.Adiciona("</ans:procedimentosExecutados>");

            return convertTissInstance;
        }

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasSPSADTItemProcedimentosExecutadosItem(this ISpEntregaConvertTiss convertTissInstance, SpEntregaLoteGuiasProcRealizadosDto procRealizado, IEnumerable<SpEntregaLoteGuiasMembroEquipeDto> membroEquipes)
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
                    .TissPrestadorParaOperadoraGuiasSPSADTItemProcedimentosExecutadosItemEquipe(posicaoProfissional, cpf, nomeExecutante, siglaConselho, numeroConselho, ufConselho)
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

        private static ISpEntregaConvertTiss TissPrestadorParaOperadoraGuiasSPSADTItemProcedimentosExecutadosItemEquipe(this ISpEntregaConvertTiss convertTissInstance, string posicaoProfissional, string cpf, string nomeExecutante, string siglaConselho, string numeroConselho, string ufConselho)
        {
            return convertTissInstance.AdicionaFormatado(@"
            <ans:equipeSadt>
                <ans:grauPart>{0}</ans:grauPart>
                <ans:codProfissional>
                    <ans:cpfContratado>{1}</ans:cpfContratado>
                </ans:codProfissional>
                <ans:nomeProf>{2}</ans:nomeProf>
                <ans:conselho>{3}</ans:conselho>
                <ans:numeroConselhoProfissional>{4}</ans:numeroConselhoProfissional>
                <ans:UF>{5}</ans:UF>
                <ans:CBOS>{6}</ans:CBOS>
            </ans:equipeSadt>",
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
