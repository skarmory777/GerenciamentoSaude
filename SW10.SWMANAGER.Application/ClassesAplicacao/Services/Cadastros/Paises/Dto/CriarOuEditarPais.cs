using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto
{
    [AutoMap(typeof(Pais))]
    public class CriarOuEditarPais : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
        public string Sigla { get; set; }

        //public virtual ICollection<EstadoDto> Estados { get; set; }
    }
}
