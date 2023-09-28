using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Relatorios.Models
{
    public class GuiaResumoInternacaoModel
    {
        public string NomePaciente { get; set; }
        public string Matricula { get; set; }
        public string RegistroANS { get; set; }
        public string ValidadeCarteira { get; set; }
        public string Senha { get; set; }
        public string DataAutorizacao { get; set; }
        public string CodCNES { get; set; }
        public string NomeContratado { get; set; }
        public string ValidadeSenha { get; set; }
        public string NumeroGuia { get; set; }
        public string Cid1 { get; set; }
        public string Cid2 { get; set; }
        public string Cid3 { get; set; }
        public string Cid4 { get; set; }
        public string CidObito { get; set; }
        public string CodOperadora { get; set; }
        public string CaraterAtendimento { get; set; }
        public string TipoFaturamento { get; set; }
        public string DataIniFaturamento { get; set; }
        public string DataFimFaturamento { get; set; }
        public string HoraIniFaturamento { get; set; }
        public string HoraFimFaturamento { get; set; }
        public string TipoInternacao { get; set; }
        public string RegimeInternacao { get; set; }
        public string TotalProcedimentos { get; set; }
        public string TotalDiaria { get; set; }
        public string TotalTaxasAlugueis { get; set; }
        public string TotalMateriais { get; set; }
        public string TotalOpme { get; set; }
        public string TotalMedicamentos { get; set; }
        public string TotalGasesMedicinais { get; set; }
        public string TotalGeral { get; set; }
        public bool RN { get; set; }
        public string CNS { get; set; }
        public string IndicadorAcidente { get; set; }
        public string MotivoEncerramento { get; set; }


        public List<string> Lista { get; set; }

        public GuiaResumoInternacaoModel()
        {
            Lista = new List<string>();
        }

        public static GuiaResumoInternacaoModel MapearFromAtendimento(AtendimentoDto atendimento, FaturamentoContaDto contaDto)
        {
            var model = new GuiaResumoInternacaoModel();



            model.NomePaciente = atendimento.Paciente?.NomeCompleto;
            model.RegistroANS = atendimento.Convenio?.RegistroANS;
            model.Matricula = atendimento.Matricula;
            model.Senha = atendimento.Senha;
            model.DataAutorizacao = atendimento.DataAutorizacao != null ? ((DateTime)atendimento.DataAutorizacao).ToString("dd/MM/yyyy") : "|_|_|/|_|_|/|_|_|_|_|";
            model.CodCNES = atendimento.Empresa?.Cnes.ToString();
            model.NomeContratado = atendimento.Empresa?.NomeFantasia;
            model.ValidadeSenha = atendimento.ValidadeSenha != null ? ((DateTime)atendimento.ValidadeSenha).ToString("dd/MM/yyyy") : "";
            model.NumeroGuia = atendimento.GuiaNumero;
            model.Cid1 = "A001";
            model.Cid2 = "B360";
            model.Cid3 = "C210";
            model.Cid4 = "C496";
            model.CidObito = "C496";

            var codigoNaOperadora = atendimento.Convenio.IdentificacoesPrestadoresNaOperadoraDto.Where(w => w.EmpresaId == atendimento.EmpresaId).FirstOrDefault();

            if (codigoNaOperadora != null)
            {
                model.CodOperadora = codigoNaOperadora.Codigo;
            }

            model.CaraterAtendimento = atendimento.CaraterAtendimento?.Codigo;
            model.TipoFaturamento = "4";
            model.DataIniFaturamento = contaDto.DataInicio != null ? string.Format("{0:dd/MM/yyyy}", contaDto.DataInicio) : "|_|_|/|_|_|/|_|_|_|_|";
            model.DataFimFaturamento = contaDto.DataFim != null ? string.Format("{0:dd/MM/yyyy}", contaDto.DataFim) : "|_|_|/|_|_|/|_|_|_|_|";
            model.HoraIniFaturamento = contaDto.DataInicio != null ? string.Format("{0:HH:mm}", contaDto.DataInicio) : "|_|_|:|_|_|";
            model.HoraFimFaturamento = contaDto.DataFim != null ? string.Format("{0:HH:mm}", contaDto.DataFim) : "|_|_|:|_|_|";
            model.TipoInternacao = atendimento.AtendimentoTipo?.Codigo;
            model.RegimeInternacao = "1";
            model.TotalProcedimentos = "";
            model.TotalDiaria = "";
            model.TotalTaxasAlugueis = "";
            model.TotalMateriais = "";
            model.TotalOpme = "";
            model.TotalMedicamentos = "";
            model.TotalGasesMedicinais = "";
            model.TotalGeral = "";
            model.CNS = atendimento.Paciente.Cns.ToString();
            model.IndicadorAcidente = (atendimento.IndicacaoAcidente != null && !string.IsNullOrEmpty(atendimento.IndicacaoAcidente.Codigo)) ? atendimento.IndicacaoAcidente.Codigo : "|_|";

            if (atendimento.ValidadeCarteira != null)
            {
                model.ValidadeCarteira = ((DateTime)atendimento.ValidadeCarteira).ToString("dd/MM/yyyy");
            }

            if (atendimento.Paciente.Nascimento.HasValue)
            {
                var idade = DateDifference.GetExtendedDifference((DateTime)atendimento.Paciente.Nascimento);
                model.RN = (idade.Ano == 0 && idade.Mes == 0 && idade.Dia <= 30);
            }

            model.MotivoEncerramento = atendimento.MotivoAlta != null ? atendimento.MotivoAlta.Codigo : "|_|_|";

            return model;
        }
    }
}
