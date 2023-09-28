
namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class RetornoReceitaMemedInput
    {
        public long AtendimentoId { get; set; }
        public long ReceituarioId { get; set; }
        public string PacienteMemedId { get; set; }
        public string PrescricaoMemedId { get; set; }
        public string ReceitaDocumentoCompletoId { get; set; }
    }
}
