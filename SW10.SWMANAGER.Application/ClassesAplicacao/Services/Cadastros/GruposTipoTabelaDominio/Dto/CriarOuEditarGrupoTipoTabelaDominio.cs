using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto
{
    [AutoMap(typeof(GrupoTipoTabelaDominio))]
    public class CriarOuEditarGrupoTipoTabelaDominio : CamposPadraoCRUDDto
    {

        public long? TipoTabelaDominioId { get; set; }

        [ForeignKey("TipoTabelaDominioId")]
        public virtual TipoTabelaDominioDto TipoTabelaDominio { get; set; }

    }
}
