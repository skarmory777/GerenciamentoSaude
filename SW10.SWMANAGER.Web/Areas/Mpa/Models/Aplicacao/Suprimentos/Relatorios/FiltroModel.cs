using SW10.SWMANAGER.Web.Core;

using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Relatorios
{
    public class FiltroModel
    {
        public string Estoque { get; set; }
        public int? GrupoProduto { get; set; }
        public int? Classe { get; set; }
        public int? SubClasse { get; set; }
        public string Empresa { get; set; }

        public string Titulo { get; set; }
        public string NomeHospital { get; set; }
        public string NomeUsuario { get; set; }
        public string DataHora { get; set; }
        public string Data { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TipoRel { get; set; }

        public bool EhMovimentacao { get; set; }

        public IList<DataSetReports.ReportProdutosRow> Dados { get; set; }
        public IList<DataSetReports.RelatorioMovimentoRow> DadosMovimentacao { get; set; }
        public IList<SelectListItem> Grupos { get; set; }
        public IList<SelectListItem> Default { get; set; }

    }
}