using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto
{
    [AutoMap(typeof(Grupo))]
    public class GrupoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public string ClasseList { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }

    }
}