using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios.Guias
{
    public class GuiaSpsadtModel
    {
        // Cabecalho
        public string Titulo { get; set; }
        public string NumeroGuiaPrestador { get; set; }

        // Guia                                       
        public string RegistroAns { get; set; }
        public string NumeroGuiaPrincipal { get; set; }
        public string DataAutorizacao { get; set; }
        public string Senha { get; set; }
        public string DataValidadeSenha { get; set; }
        public string NumeroGuiaOperadora { get; set; }
        public string NumeroCarteira { get; set; }
        public string ValidadeCarteira { get; set; }
        public string NomePaciente { get; set; }
        public string NumeroCns { get; set; }
        public string AtendimentoRn { get; set; }
        public string CodigoOperadora { get; set; }
        public string NomeContratado { get; set; }
        public string NomeProfissionalSolicitante { get; set; }
        public string ConselhoProfissional { get; set; }
        public string NumeroConselho { get; set; }
        public string UF { get; set; }
        public string CodigoCbo { get; set; }


        public string AssinaturaProfissionalSolicitante { get; set; }
        public string CaraterAtendimento { get; set; }
        public string DataSolicitacao { get; set; }
        public string IndicacaoClinica { get; set; }

        // FALTA 5 CAMPOS DE PROCEDIMENTOS

        public string CodigoCne { get; set; }
        public string TipoAtendimento { get; set; }
        public string IndicacaoAcidente { get; set; }
        public string TipoConsulta { get; set; }
        public string MotivoEncerramentoAtendimento { get; set; }

        // LISTA DE PROCEDIMENTOS 2 FALTANDO

        // Identificacao Equipe
        public string SequenciaRef1 { get; set; }
        public string SequenciaRef2 { get; set; }
        public string SequenciaRef3 { get; set; }
        public string SequenciaRef4 { get; set; }
        public string GrauPart1 { get; set; }
        public string GrauPart2 { get; set; }
        public string GrauPart3 { get; set; }
        public string GrauPart4 { get; set; }
        public string CodigoOperadoraCpf1 { get; set; }
        public string CodigoOperadoraCpf2 { get; set; }
        public string CodigoOperadoraCpf3 { get; set; }
        public string CodigoOperadoraCpf4 { get; set; }
        public string NomeProfissional1 { get; set; }
        public string NomeProfissional2 { get; set; }
        public string NomeProfissional3 { get; set; }
        public string NomeProfissional4 { get; set; }
        public string ConselhoProfissional1 { get; set; }
        public string ConselhoProfissional2 { get; set; }
        public string ConselhoProfissional3 { get; set; }
        public string ConselhoProfissional4 { get; set; }

        public string NumeroConselho1 { get; set; }
        public string NumeroConselho2 { get; set; }
        public string NumeroConselho3 { get; set; }
        public string NumeroConselho4 { get; set; }

        public string Uf1 { get; set; }
        public string Uf2 { get; set; }
        public string Uf3 { get; set; }
        public string Uf4 { get; set; }
        public string CodigoCbo1 { get; set; }
        public string CodigoCbo2 { get; set; }
        public string CodigoCbo3 { get; set; }
        public string CodigoCbo4 { get; set; }

        // Datas e Assinaturas (procedimentos em serie)
        public string DataRealizacaoProcedimentoSerie1 { get; set; }
        public string DataRealizacaoProcedimentoSerie2 { get; set; }
        public string DataRealizacaoProcedimentoSerie3 { get; set; }
        public string DataRealizacaoProcedimentoSerie4 { get; set; }
        public string DataRealizacaoProcedimentoSerie5 { get; set; }
        public string DataRealizacaoProcedimentoSerie6 { get; set; }
        public string DataRealizacaoProcedimentoSerie7 { get; set; }
        public string DataRealizacaoProcedimentoSerie8 { get; set; }
        public string DataRealizacaoProcedimentoSerie9 { get; set; }
        public string DataRealizacaoProcedimentoSerie10 { get; set; }
        public string ObservacaoJustificativa { get; set; }
        public string TotalProcedimentos { get; set; }
        public string TotalDiaria { get; set; }
        public string TotalTaxasAlugueis { get; set; }
        public string TotalMateriais { get; set; }
        public string TotalOpme { get; set; }
        public string TotalMedicamentos { get; set; }
        public string TotalGasesMedicinais { get; set; }
        public string TotalGeral { get; set; }
        public bool RN { get; set; }
        public string CNES { get; set; }

        public IList<ContaMedicaReportModel> Contas { get; set; }

        // TODOS ESTES DADOS DEVEM SER PUXADOS DA CONTA E NAO DO ATENDIMENTO. SO PUXA DO ATENDIMENTO O CAMPO QUE NAO EXISTIR NA CONTA EM QUESTAO
        public void LerAtendimento(AtendimentoDto atendimento, List<FaturamentoContaItemDto> fatItens = null)
        {
            this.NumeroGuiaPrestador = atendimento.GuiaNumero; // ou atendimento.GuiaNumero;
            this.RegistroAns = atendimento.Convenio?.RegistroANS;
            this.NumeroCarteira = atendimento.Matricula;
            this.DataAutorizacao = atendimento.DataAutorizacao == null ? "" : ((DateTime)atendimento.DataAutorizacao).ToString("dd/MM/yyyy"); // 
            this.Senha = atendimento.Senha;
            this.DataValidadeSenha = atendimento.ValidadeSenha == null ? "" : ((DateTime)atendimento.ValidadeSenha).ToString("dd/MM/yyyy"); // 
            this.NumeroGuiaOperadora = atendimento.GuiaNumero;
            this.NumeroCarteira = atendimento.Matricula;
            this.ValidadeCarteira = atendimento.ValidadeCarteira == null ? "" : ((DateTime)atendimento.ValidadeCarteira).ToString("dd/MM/yyyy"); // 
            this.NomePaciente = atendimento.Paciente?.NomeCompleto;
            this.NumeroCns = atendimento.CNS;
            //TimeSpan diferenca = DateTime.Now - (DateTime)atendimento.Paciente.Nascimento;
            //var dias = diferenca.TotalDays;
            // this.AtendimentoRn = FuncoesGlobais.IsRN(atendimento.Paciente.Nascimento?? DateTime.Now)? "S" : "N";
            //this.CodigoOperadora             = "";

            var codigoNaOperadora = atendimento.Convenio.IdentificacoesPrestadoresNaOperadoraDto.Where(w => w.EmpresaId == atendimento.EmpresaId).FirstOrDefault();

            if (codigoNaOperadora != null)
            {
                this.CodigoOperadora = codigoNaOperadora.Codigo;
            }
            this.CNES = atendimento.Empresa.Cnes.ToString();

            this.NomeContratado = atendimento.Empresa?.NomeFantasia;
            this.NomeProfissionalSolicitante = atendimento.Medico?.NomeCompleto;
            this.ConselhoProfissional = atendimento.Medico?.Conselho?.Codigo;
            this.NumeroConselho = atendimento.Medico?.NumeroConselho.ToString();
            this.UF = atendimento.Medico?.Conselho?.Uf;
            this.CodigoCbo = atendimento.Especialidade?.Codigo;
            this.CaraterAtendimento = atendimento.CaraterAtendimento?.Codigo;
            this.DataSolicitacao = atendimento.DataRegistro.ToString("dd/MM/yyyy");
            // this.CodigoOperadora             = "";
            this.NomeContratado = atendimento.Empresa?.NomeFantasia;
            this.CodigoCne = atendimento.Empresa?.Cnes.ToString();
            this.TipoAtendimento = atendimento.AtendimentoTipo?.Codigo?.ToString();
            this.IndicacaoClinica = "";
            this.TotalProcedimentos = "";
            this.TotalTaxasAlugueis = "";
            this.TotalMateriais = "";
            this.TotalOpme = "";
            this.TotalMedicamentos = "";
            this.TotalGasesMedicinais = "";
            this.TotalGeral = "";
            this.IndicacaoAcidente = atendimento.IndicacaoAcidente?.Codigo;
            this.TipoConsulta = atendimento.AtendimentoTipo?.Codigo;

            if (atendimento.Paciente.Nascimento.HasValue)
            {
                var idade = DateDifference.GetExtendedDifference((DateTime)atendimento.Paciente.Nascimento);
                this.RN = (idade.Ano == 0 && idade.Mes == 0 && idade.Dia <= 30);
            }


            if (fatItens != null)
            {

                var valorTotal = fatItens.Sum(s => s.ValorItem * s.Qtde);
                this.TotalGeral = string.Format("{0:#,##0.00}", valorTotal);

                //var totalDiarias = fatItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "05" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Diarias)).Sum(s => s.ValorItem * s.Qtde);
                //this.TotalTaxasAlugueis = string.Format("{0:#,##0.00}", totalDiarias);

                var totalTaxasAlugueis = fatItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "07" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.TaxasAluguéis)).Sum(s => s.ValorItem * s.Qtde);
                this.TotalTaxasAlugueis = string.Format("{0:#,##0.00}", totalTaxasAlugueis);

                var totalMateriais = fatItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "03" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Materiais)).Sum(s => s.ValorItem * s.Qtde);
                this.TotalMateriais = string.Format("{0:#,##0.00}", totalMateriais);

                var totalOPME = fatItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "08" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.OPME)).Sum(s => s.ValorItem * s.Qtde);
                this.TotalOpme = string.Format("{0:#,##0.00}", totalOPME);

                var totalMedicamentos = fatItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "02" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Medicamentos)).Sum(s => s.ValorItem * s.Qtde);
                this.TotalMedicamentos = string.Format("{0:#,##0.00}", totalMedicamentos);

                var totalGasesMedicinais = fatItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "01" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.GasesMedicinais)).Sum(s => s.ValorItem * s.Qtde);
                this.TotalGasesMedicinais = string.Format("{0:#,##0.00}", totalGasesMedicinais);

                var totalProcedimentos = fatItens.Where(w => string.IsNullOrEmpty(w.FaturamentoItem.Grupo.CodTipoOutraDespesa) && w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == null).Sum(s => s.ValorItem * s.Qtde);
                this.TotalProcedimentos = string.Format("{0:#,##0.00}", totalProcedimentos);

            }

        }
    }
}


