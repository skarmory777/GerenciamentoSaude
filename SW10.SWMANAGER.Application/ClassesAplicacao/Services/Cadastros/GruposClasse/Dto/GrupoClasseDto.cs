using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto
{
    [AutoMap(typeof(GrupoClasse))]
    public class GrupoClasseDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public long? IdGridClasse { get; set; }

        public long GrupoId { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }
    }
}