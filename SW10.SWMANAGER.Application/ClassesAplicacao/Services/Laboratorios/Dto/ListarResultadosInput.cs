namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Dto
{
    public class ListarResultadosInput : ListarInput
    {
        public long? AtendimentoId { get; set; }
        public string TipoAtendimento { get; set; }
    }
}
