namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto
{
    public class ConvenioPlanoIndex
    {
        public long Id { get; set; }
        public long? ConvenioId { get; set; }
        public string ConvenioDescricao { get; set; }
        public long? PlanoId { get; set; }
        public string PlanoDescricao { get; set; }
        public long? IdGrid { get; set; }
    }
}
