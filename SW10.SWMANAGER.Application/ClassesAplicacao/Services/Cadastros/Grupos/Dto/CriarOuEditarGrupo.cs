using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto
{
    [AutoMap(typeof(Grupo))]
    public class CriarOuEditarGrupo : CamposPadraoCRUDDto
    {
        [StringLength(255)]
        public string Descricao { get; set; }
    }
}