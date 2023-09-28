namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    public class RegistroExameDto
    {
        public long? Id { get; set; }
        public long ExameId { get; set; }
        public string ExameDescricao { get; set; }
        public long? IdGrid { get; set; }
        public long? FaturamentoContaItemId { get; set; }
        public long? SolicitacaoExameId { get; set; }
        public string AccessNumber { get; set; }
    }
}
