using SW10.SWMANAGER.Web.Core;

using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamento.Relatorios
{
    public class FaturamentoFiltroModel
    {
        public string Titulo { get; set; }
        public string NomeHospital { get; set; }
        public string NomeUsuario { get; set; }
        public string DataHora { get; set; }
        public IList<SelectListItem> Empresas { get; set; }
        public IList<CamposRelatorioDs> Lista { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public long Empresa { get; set; }
        public IList<DataSetReports.ReportProdutosRow> Dados { get; set; }
        public long? TabelaId { get; set; }
    }

    public class CamposRelatorioDs
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string moeda { get; set; }
        public string tabela { get; set; }
        public string dataVigencia { get; set; }
        public string preco { get; set; }
        public string filme { get; set; }
        public string coch { get; set; }
        public string valoTotal { get; set; }
        public string auxilia { get; set; }
        public string porte { get; set; }
        public string ativo { get; set; }
        public object item { get; internal set; }
    }
}