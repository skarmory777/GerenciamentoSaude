using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto
{
    [AutoMap(typeof(TipoLogradouro))]
    public class CriarOuEditarTipoLogradouroDto : CamposPadraoCRUDDto
    {
        public string Abreviacao { get; set; }

        public string Descricao { get; set; }
    }
}
