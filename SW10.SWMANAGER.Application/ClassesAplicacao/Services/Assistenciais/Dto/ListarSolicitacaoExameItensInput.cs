namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class ListarSolicitacaoExameItensInput : ListarInput
    {
        public long SolicitacaoExameId { get; set; }
        public long? PacienteId { get; set; }
    }
}
