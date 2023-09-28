namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto
{
    public class EspecialidadeMedicoDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long IdGridMedicoEspecialidade { get; set; }
        public long IdEspecialidade { get; set; }
    }
}
