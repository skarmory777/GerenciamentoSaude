using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto
{
    [AutoMap(typeof(GrupoClasse))]
    public class CriarOuEditarGrupoClasse : CamposPadraoCRUDDto
    {
        public long? GrupoId { get; set; }
    }
}