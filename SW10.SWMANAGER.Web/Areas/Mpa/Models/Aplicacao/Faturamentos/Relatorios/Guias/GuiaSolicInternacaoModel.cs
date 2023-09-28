using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios.Guias
{
    public class GuiaSolicInternacaoModel
    {
        // Cabecalho
        public string Titulo { get; set; }
        public string RegistroAns { get; set; }
        public string NumeroGuiaPrestador { get; set; }
        public string NumeroGuiaOperadora { get; set; }
        public string DataAutorizacao { get; set; }
        public string Senha { get; set; }
        public string DataValidadeSenha { get; set; }
        public string AtendimentoRn { get; set; }
        public string NomePaciente { get; set; }
        public string CartaoNacionalSaude { get; set; }
        public string CodigoOperadora { get; set; }
        public string NomeContratado { get; set; }
        public string NomeProfissionalSolicitante { get; set; }
        public string ConselhoProfissional { get; set; }
        public string NumeroConselho { get; set; }
        public string UF { get; set; }
        public string CodigoCbo { get; set; }
        public string CodigoOperadoraCnpj { get; set; }
        public string NomeHospital { get; set; }
        public string DataSugeridaInterncao { get; set; }
        public string CaraterAtendimento { get; set; }
        public string TipoInterncao { get; set; }
        public string RegimeInternacao { get; set; }
        public string QtdDiariasSolictadas { get; set; }
        public string PrevisaoUsoOpme { get; set; }
        public string PrevisaoUsoQuioteraptico { get; set; }
        public string IndicaoClinica { get; set; }
        public string SequenciaRef2 { get; set; }
        public string SequenciaRef3 { get; set; }
        public string SequenciaRef4 { get; set; }
        public string GrauPart1 { get; set; }
        public string GrauPart2 { get; set; }




        public string ValidadeCarteira { get; set; }
       
        public string NumeroCns { get; set; }
      
        
       
       
      

        public string AssinaturaProfissionalSolicitante { get; set; }
        public string DataSolicitacao { get; set; }
        public string IndicacaoClinica { get; set; }

        // FALTA 5 CAMPOS DE PROCEDIMENTOS

      

        // LISTA DE PROCEDIMENTOS 2 FALTANDO

        // Identificacao Equipe
       
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
        public string TotalTaxasAlugueis { get; set; }
        public string TotalMateriais { get; set; }
        public string TotalOpme { get; set; }
        public string TotalMedicamentos { get; set; }
        public string TotalGeral { get; set; }
        
        
        public IList<ContaMedicaReportModel> Contas { get; set; }

        public void LerAtendimento (AtendimentoDto atendimento)
        {
            this.NumeroGuiaPrestador = atendimento.NumeroGuia; // ou atendimento.GuiaNumero;
            this.RegistroAns = "";
            this.DataAutorizacao = ""; //  atendimento.DataAutorizacao;
            this.Senha = ""; // atendimento.Senha;
            this.DataValidadeSenha = ""; // atendimento.DataValidadeSenha;
            this.NumeroGuiaOperadora = ""; // atendimento.GuiaNumeroOperadora;
            this.NumeroCarteira = atendimento.Matricula;
            this.ValidadeCarteira = ""; // atendimento.ValidadeCarteira;
            this.NomePaciente = atendimento.Paciente.NomeCompleto;
            this.NumeroCns = atendimento.Paciente.Cns.ToString();
            TimeSpan diferenca = DateTime.Now - atendimento.Paciente.Nascimento;
            var dias = diferenca.TotalDays;
            this.AtendimentoRn = dias > 30 ? "N" : "S";
            this.CodigoOperadora = "";
            this.NomeContratado = atendimento.Empresa.NomeFantasia;
            this.NomeProfissionalSolicitante = atendimento.Medico.NomeCompleto;
            this.ConselhoProfissional = atendimento.Medico.Conselho?.Descricao; // "cons prof";
            this.NumeroConselho = atendimento.Medico.NumeroConselho.ToString();
            this.UF = "";
            this.CodigoCbo = "";
            this.CaraterAtendimento = "";
            this.DataSolicitacao = atendimento.DataRegistro.ToString("dd/MM/yyyy");
            this.CodigoOperadora = "";
            this.NomeContratado = atendimento.Empresa.NomeFantasia;
            this.CodigoCne = atendimento.Empresa.Cnes.ToString();
        }
    }
}


