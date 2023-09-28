namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class IntervaloGuiasConvenioIndex
    {
        public long? Id { get; set; }
        public long IdGrid { get; set; }
        public long ConvenioId { get; set; }
        public long EmpresaId { get; set; }
        public long FaturamentoGuiaId { get; set; }

        public string Inicio { get; set; }
        public string Final { get; set; }
        public int? NumeroGuiasParaAviso { get; set; }

        public string ConvenioDescricao { get; set; }
        public string EmpresaDescricao { get; set; }
        public string FaturamentoGuiaDescricao { get; set; }
    }
}
