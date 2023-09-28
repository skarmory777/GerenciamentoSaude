using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto
{

    [AutoMap(typeof(Nacionalidade))]
    public class CriarOuEditarNacionalidade : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
