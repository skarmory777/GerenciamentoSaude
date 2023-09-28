using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens;

using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Relatorios
{
    public class FiltroModel
    {
        // Cabecalho
        public string Titulo { get; set; }
        public string NomeHospital { get; set; }
        public string NomeUsuario { get; set; }
        public string DataHora { get; set; }
        public string Paciente { get; set; }
        public string Nascimento { get; set; }

        // Atendimento
        public string Convenio { get; set; }
        public string Plano { get; set; }
        public string Matricula { get; set; }
        public string Titular { get; set; }
        public string ValidCarteira { get; set; }
        public string DataInternacao { get; set; }
        public string Senha { get; set; }
        public string Guia { get; set; }
        public string Especialidade { get; set; }
        public string Medico { get; set; }
        public string CRM { get; set; }
        public string TipoAlta { get; set; }

        // Conta e Itens
        public IList<LauMovimentoReportModel> Contas { get; set; }

        public IList<Dictionary<string, string>> ContasDic { get; set; }
        public IList<LauMovimentoItemReportModel> Itens { get; set; }
        public Dictionary<string, IList<Dictionary<string, string>>> ListaTotal { get; set; }
    }
}


