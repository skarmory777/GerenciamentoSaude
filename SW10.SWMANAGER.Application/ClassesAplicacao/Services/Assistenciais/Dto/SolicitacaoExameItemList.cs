namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class SolicitacaoExameItemList : CamposPadraoCRUDDto
    {
        public string FaturamentoItem { get; set; }
        public string Material { get; set; }
        public string GuiaNumero { get; set; }

        public string AccessNumber { get; set; }
    }
}
