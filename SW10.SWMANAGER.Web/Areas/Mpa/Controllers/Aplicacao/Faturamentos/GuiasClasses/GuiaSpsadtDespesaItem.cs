using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos.GuiasClasses
{
    public class GuiaSpsadtDespesaItem
    {
        public string Cd { get; set; }
        public string Data { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public string Tabela { get; set; }
        public string CodigoItem { get; set; }
        public string Qtde { get; set; }
        public string UnidadeMedida { get; set; }
        public string RedAcres { get; set; }
        public string ValorUnitario { get; set; }
        public string ValorTotal { get; set; }
        public string RegistroAnvisa { get; set; }
        public string RefMaterialFabricante { get; set; }
        public string NumAutorizacaoFuncionamento { get; set; }
        public string Descricao { get; set; }

        public void LerContaItem(FaturamentoContaItem item)
        {
            Cd = "";
            Data = item.Data?.ToString("dd/MM/yyyy");
            HoraInicial = item.HoraIncio.ToString();
            HoraFinal = item.HoraFim.ToString();
            Tabela = "";
            CodigoItem = item.Codigo;
            Qtde = item.Qtde.ToString();
            UnidadeMedida = "";
            RedAcres = "";
            ValorUnitario = "";
            ValorTotal = "";
            RegistroAnvisa = "";
            RefMaterialFabricante = "";
            NumAutorizacaoFuncionamento = "";
            Descricao = item.Descricao;
        }
    }
}