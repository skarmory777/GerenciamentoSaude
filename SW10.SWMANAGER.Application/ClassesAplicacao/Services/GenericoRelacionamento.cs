namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    public class GenericoRelacionamento
    {
        public long Id { get; set; }
        public long? RelacionamentoId { get; set; }
        public long? RelacionadoId { get; set; }
        public string Descricao { get; set; }
        public long IdGrid { get; set; }
    }
}
