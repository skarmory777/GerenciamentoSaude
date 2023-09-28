using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto
{
    [AutoMap(typeof(TabelaDominio))]
    public class CriarOuEditarTabelaDominio : CamposPadraoCRUDDto
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        public long? TipoTabelaDominioId { get; set; }

        public long? GrupoTipoTabelaDominioId { get; set; }

        [ForeignKey("TipoTabelaDominioId")]
        public virtual TipoTabelaDominioDto TipoTabelaDominio { get; set; }

        [ForeignKey("GrupoTipoTabelaDominioId")]
        public virtual GrupoTipoTabelaDominioDto GrupoTipoTabelaDominio { get; set; }

        public virtual ICollection<TabelaDominioVersaoTissDto> TabelaDominioVersoesTiss { get; set; }

    }
}
