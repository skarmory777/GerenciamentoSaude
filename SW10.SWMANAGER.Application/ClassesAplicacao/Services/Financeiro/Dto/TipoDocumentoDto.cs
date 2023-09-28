using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(TipoDocumento))]
    public class TipoDocumentoDto : CamposPadraoCRUDDto
    {
        public static TipoDocumentoDto Mapear(TipoDocumento tipoDocumento)
        {
            TipoDocumentoDto tipoDocumentoDto = new TipoDocumentoDto();

            tipoDocumentoDto.Id = tipoDocumento.Id;
            tipoDocumentoDto.Codigo = tipoDocumento.Codigo;
            tipoDocumentoDto.Descricao = tipoDocumento.Descricao;

            return tipoDocumentoDto;
        }

        public static TipoDocumento Mapear(TipoDocumentoDto tipoDocumentoDto)
        {
            TipoDocumento tipoDocumento = new TipoDocumento();

            tipoDocumento.Id = tipoDocumentoDto.Id;
            tipoDocumento.Codigo = tipoDocumentoDto.Codigo;
            tipoDocumento.Descricao = tipoDocumentoDto.Descricao;

            return tipoDocumento;
        }
    }
}
