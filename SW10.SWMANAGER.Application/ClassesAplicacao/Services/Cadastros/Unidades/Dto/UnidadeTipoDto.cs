using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto
{
    [AutoMap(typeof(UnidadeTipo))]
    public class UnidadeTipoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
