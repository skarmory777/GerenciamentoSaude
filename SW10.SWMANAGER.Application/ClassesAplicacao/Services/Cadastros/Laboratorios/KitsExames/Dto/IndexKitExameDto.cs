namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto
{
    public class IndexKitExameDto
    {
        public long KitExameId { get; set; }
        public long FaturamentoItemId { get; set; }
        public string ExameDescricao { get; set; }
        public long? MaterialId { get; set; }
            
        public string MaterialDescricao { get; set; }
    }
}
