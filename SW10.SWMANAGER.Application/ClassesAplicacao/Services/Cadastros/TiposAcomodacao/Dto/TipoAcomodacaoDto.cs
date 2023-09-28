using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto
{
    [AutoMap(typeof(TipoAcomodacao))]
    public class TipoAcomodacaoDto : CamposPadraoCRUDDto
    {
        public long? TabelaItemTissId { get; set; }

        public virtual TabelaDominioDto TabelaDominio { get; set; }

        public static TipoAcomodacao Mapear(TipoAcomodacaoDto tipoAcomodacaoDto)
        {
            if (tipoAcomodacaoDto == null) return null;

            TipoAcomodacao tipoAcomodacao = new TipoAcomodacao();

            tipoAcomodacao.Id = tipoAcomodacaoDto.Id;
            tipoAcomodacao.Codigo = tipoAcomodacaoDto.Codigo;
            tipoAcomodacao.Descricao = tipoAcomodacaoDto.Descricao;

            return tipoAcomodacao;
        }

        public static TipoAcomodacaoDto Mapear(TipoAcomodacao tipoAcomodacao)
        {
            if (tipoAcomodacao == null)
            {
                return null;
            }

            TipoAcomodacaoDto tipoAcomodacaoDto = MapearBase<TipoAcomodacaoDto>(tipoAcomodacao);

            tipoAcomodacaoDto.Id = tipoAcomodacao.Id;
            tipoAcomodacaoDto.Codigo = tipoAcomodacao.Codigo;
            tipoAcomodacaoDto.Descricao = tipoAcomodacao.Descricao;

            return tipoAcomodacaoDto;
        }

        public static List<TipoAcomodacaoDto> Mapear(List<TipoAcomodacao> lstTipoAcomodacao)
        {
            List<TipoAcomodacaoDto> lstTipoAcomodacaoDto = new List<TipoAcomodacaoDto>();

            foreach (var item in lstTipoAcomodacao)
            {
                lstTipoAcomodacaoDto.Add(TipoAcomodacaoDto.Mapear(item));
            }

            return lstTipoAcomodacaoDto;
        }

    }
}
