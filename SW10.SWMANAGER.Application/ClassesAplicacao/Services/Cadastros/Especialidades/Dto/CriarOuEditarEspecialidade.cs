using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto
{
    [AutoMap(typeof(Especialidade))]
    public class CriarOuEditarEspecialidade : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
        public string Cbo { get; set; }
        public string CboSus { get; set; }

        public long? CboId { get; set; }
        public Cbo SisCbo { get; set; }
    }
}
