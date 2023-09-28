using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto
{

    [AutoMap(typeof(Naturalidade))]
    public class CriarOuEditarNaturalidade : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
