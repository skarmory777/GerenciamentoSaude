using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Laboratorios
{
    public class LaboratorioPainelIndexViewModel
    {
        public List<UnidadeOrganizacionalDto> UnidadeOrganizacionais { get; internal set; }

        public List<LaboratorioPainelIndexTabsViewModel> IndexTabs { get; set; } = new List<LaboratorioPainelIndexTabsViewModel>
        {
            new LaboratorioPainelIndexTabsViewModel("urgente", "Urgentes", "red-sunglo"),
            new LaboratorioPainelIndexTabsViewModel("rotina", "Rotina", "blue"),
            new LaboratorioPainelIndexTabsViewModel("pendente", "Pendentes", "red-pink"),
            new LaboratorioPainelIndexTabsViewModel("cultura", "Culturas", "blue-ebonyclay")
        };
        
        public List<LaboratorioPainelIndexTabsViewModel> IndexStatusTabs { get; set; } = new List<LaboratorioPainelIndexTabsViewModel>
        {
            new LaboratorioPainelIndexTabsViewModel("Inicial", "Inicial", "grey-mint"),
            new LaboratorioPainelIndexTabsViewModel("Em Coleta", "Em coletas", "blue-hoki"),
            new LaboratorioPainelIndexTabsViewModel("Coletado", "Coletados", "blue-hoki"),
            new LaboratorioPainelIndexTabsViewModel("EnviadoEquipamento", "Enviado Equipamento", "yellow-crusta"),
            new LaboratorioPainelIndexTabsViewModel("Interfaceado", "Interfaceado", "yellow-crusta"),
            new LaboratorioPainelIndexTabsViewModel("Digitado", "digitado", "blue-hoki"),
            new LaboratorioPainelIndexTabsViewModel("Conferido", "Conferido", "green-haze")
        };

        public string FirstCharToUpper(string input) => input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };
    }

    public class LaboratorioPainelIndexTabsViewModel
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public string ClassName { get; set; }

        public LaboratorioPainelIndexTabsViewModel(string id,string label, string className, string title = null)
        {
            Id = id;
            Label = label;
            Title = title;
            ClassName = className;
        }
    }

    public class LaboratorioIndexDetalhamentoViewModel
    {
        public bool HasColeta => LabResultadoId.HasValue;

        public long AtendimentoId { get; set; }
        public long PacienteId { get; set; }
        public long SolicitacaoExameId { get; set; }
        public long? LabResultadoId { get; set; }
        public SolicitacaoExameDto SolicitacaoExame { get; set; }
        
        public ResultadoDto LabResultado { get; set; }
        
        public List<LaboratorioPainelIndexTabsViewModel> IndexTabs { get; set; } = new List<LaboratorioPainelIndexTabsViewModel>
        {
            new LaboratorioPainelIndexTabsViewModel("inicial", "Inicial", "grey-mint"),
            new LaboratorioPainelIndexTabsViewModel("emColeta", "Em Coleta", "blue-hoki"),
            new LaboratorioPainelIndexTabsViewModel("coletado", "Coletado", "blue-hoki"),
            new LaboratorioPainelIndexTabsViewModel("EnviadoEquipamento", "Enviado Equipamento", "yellow-crusta"),
            new LaboratorioPainelIndexTabsViewModel("interfaceado", "Interfaceado", "yellow-crusta"),
            new LaboratorioPainelIndexTabsViewModel("digitado", "Digitado", "blue-hoki"),
            new LaboratorioPainelIndexTabsViewModel("conferido", "Conferido", "green-haze"),
            new LaboratorioPainelIndexTabsViewModel("pendente", "Pendentes", "red-sunglo")
        };

        public int QtdExamesColeta { get; set; }

        public string GetDataColeta() => 
            LabResultado != null ? LabResultado.DataColeta.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";

        public string GetColetaResponsavel() => 
            LabResultado?.Responsavel != null ? LabResultado.Responsavel.Descricao : "";

        public string GetColetaStatus() => LabResultado?.ResultadoStatus != null ? LabResultado.ResultadoStatus.Descricao : "";

        public string GetColetaStatusCor() => LabResultado?.ResultadoStatus != null ? LabResultado.ResultadoStatus.CorFundo : "";

        public string GetPacienteNome() => 
            SolicitacaoExame?.Atendimento?.Paciente?.SisPessoa == null ? string.Empty : SolicitacaoExame.Atendimento.Paciente.SisPessoa.NomeCompleto;

        public string GetIdade()
        {
            if (SolicitacaoExame?.Atendimento?.Paciente == null)
            {
                return string.Empty;
            }
            
            var idade = DateDifference.GetExtendedDifference(SolicitacaoExame.Atendimento.Paciente.Nascimento ?? DateTime.Today, DateTime.Today);

            return idade != null ? $"{idade.Ano} anos, {idade.Mes} meses e {idade.Dia} dias" : string.Empty;
        }

        public string GetDataSolicitacao() => 
            SolicitacaoExame == null ? string.Empty : SolicitacaoExame.DataSolicitacao.ToString("dd/MM/yyyy HH:mm:ss");

        public bool GetIsUrgente() => 
            SolicitacaoExame != null && SolicitacaoExame.Prioridade == SolicitacaoExamePrioridadeDto.Urgencia;

        public string GetSolicitadoPor() => 
            SolicitacaoExame?.MedicoSolicitante == null ? string.Empty : SolicitacaoExame.MedicoSolicitante.NomeCompleto;

        public string GetProtocolo() => SolicitacaoExame == null ? string.Empty : "";

        public long QuantidadeExamesSolicitacao() => 
            SolicitacaoExame == null || SolicitacaoExame.SolicitacaoItens.IsNullOrEmpty() ? 0 : SolicitacaoExame.SolicitacaoItens.Count;
        
    }

    public class LaboratorioImprimirEtiquetaDetalhamentoViewModel
    {
        public long ResultadoId { get; set; }
        public IEnumerable<ResultadoExameDto> Items { get; set; }

        public string GetClass(long id)
        {
            var classes = new string[] { "flask", "vial", "vials", "prescription-bottle-alt", "prescription-bottle", "first-aid" };
            var random = new Random();
            if (classes[id-1] != null)
            {
                return classes[id-1];
            }
            return classes[random.Next(classes.Length)];
        }
    }
    
    public class LaboratorioBaixaDetalhamentoViewModel
    {
        public long ResultadoId { get; set; }
        public IEnumerable<ResultadoExameDto> Items { get; set; }
    }
    
    public class LaboratorioDetalhamentoExameViewModel
    {
        public long ResultadoId { get; set; }
        public ResultadoDto Resultado { get; set; }
        
        public long ResultadoExameId { get; set; }
        public ResultadoExameDto ResultadoExame { get; set; }
    }

    
    
}