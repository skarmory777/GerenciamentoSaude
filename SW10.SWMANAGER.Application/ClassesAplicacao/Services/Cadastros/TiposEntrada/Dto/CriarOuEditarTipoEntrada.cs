using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposEntrada;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada.Dto
{
    [AutoMap(typeof(TipoEntrada))]
    public class CriarOuEditarTipoEntrada : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
        public bool IsAtivo { get; set; }
    }
}
