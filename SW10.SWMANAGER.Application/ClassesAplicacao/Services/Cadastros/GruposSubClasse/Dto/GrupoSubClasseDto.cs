using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto
{
    [AutoMap(typeof(GrupoSubClasse))]
    public class GrupoSubClasseDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}