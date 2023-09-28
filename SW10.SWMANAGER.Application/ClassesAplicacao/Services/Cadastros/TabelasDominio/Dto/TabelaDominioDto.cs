using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto
{
    [AutoMap(typeof(TabelaDominio))]
    public class TabelaDominioDto : CamposPadraoCRUDDto
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        public long? TipoTabelaDominioId { get; set; }

        //    public long? VersaoTissId { get; set; }

        public long? GrupoTipoTabelaDominioId { get; set; }

        [ForeignKey("TipoTabelaDominioId")]
        public virtual TipoTabelaDominioDto TipoTabelaDominio { get; set; }

        [ForeignKey("GrupoTipoTabelaDominioId")]
        public virtual GrupoTipoTabelaDominioDto GrupoTipoTabelaDominio { get; set; }

        //public virtual ICollection<TabelaDominioVersaoTissDto> TabelaDominioVersoesTiss { get; set; }

        public static TabelaDominioDto Mapear(TabelaDominio tabelaDominio)
        {
            if(tabelaDominio == null)
            {
                return null;
            }

            var tabelaDominioDto = MapearBase<TabelaDominioDto>(tabelaDominio);

            tabelaDominioDto.Id = tabelaDominio.Id;
            tabelaDominioDto.Codigo = tabelaDominio.Codigo;
            tabelaDominioDto.Descricao = tabelaDominio.Descricao;
            tabelaDominioDto.TipoTabelaDominioId = tabelaDominio.TipoTabelaDominioId;
            tabelaDominioDto.GrupoTipoTabelaDominioId = tabelaDominio.GrupoTipoTabelaDominioId;

            return tabelaDominioDto;
        }

        public static TabelaDominio Mapear(TabelaDominioDto tabelaDominio)
        {
            var tabelaDominioDto = new TabelaDominio();

            tabelaDominioDto.Id = tabelaDominio.Id;
            tabelaDominioDto.Codigo = tabelaDominio.Codigo;
            tabelaDominioDto.Descricao = tabelaDominio.Descricao;
            tabelaDominioDto.TipoTabelaDominioId = tabelaDominio.TipoTabelaDominioId;
            tabelaDominioDto.GrupoTipoTabelaDominioId = tabelaDominio.GrupoTipoTabelaDominioId;

            return tabelaDominioDto;
        }

    }
}
