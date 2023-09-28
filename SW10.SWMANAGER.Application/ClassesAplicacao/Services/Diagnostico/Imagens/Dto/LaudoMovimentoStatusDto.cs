using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    [AutoMapFrom(typeof(LaudoMovimentoStatus))]
    public class LaudoMovimentoStatusDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static LaudoMovimentoStatusDto Mapear(LaudoMovimentoStatus laudoMovimentoStatus)
        {
            LaudoMovimentoStatusDto laudoMovimentoStatusDto = new LaudoMovimentoStatusDto();

            laudoMovimentoStatusDto.Id = laudoMovimentoStatus.Id;
            laudoMovimentoStatusDto.Codigo = laudoMovimentoStatus.Codigo;
            laudoMovimentoStatusDto.Descricao = laudoMovimentoStatus.Descricao;

            return laudoMovimentoStatusDto;
        }
    }
}
