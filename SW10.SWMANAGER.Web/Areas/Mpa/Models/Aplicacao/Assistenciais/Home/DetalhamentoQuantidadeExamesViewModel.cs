namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home
{
    public class DetalhamentoQuantidadeExamesViewModel
    {
        public string Paciente { get; set; }

        public string Tipo { get; set; }

        public long TotalResultado { get; set; }

        public long TotalSolicitado { get; set; }
    }
}