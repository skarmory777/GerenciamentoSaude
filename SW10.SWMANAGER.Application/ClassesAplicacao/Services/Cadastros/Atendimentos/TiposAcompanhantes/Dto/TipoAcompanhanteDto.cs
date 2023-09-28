using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.TiposAcompanhantes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.TiposAtendimento;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAcompanhantes.Dto
{
    [AutoMap(typeof(TipoAtendimento))]
    public class TipoAcompanhanteDto : CamposPadraoCRUDDto
    {
        public bool IsInternacao { get; set; }

        public static TipoAcompanhanteDto Mapear(TipoAcompanhante tipoAcompanhante)
        {
            if (tipoAcompanhante == null)
            {
                return null;
            }

            TipoAcompanhanteDto tipoAcompanhanteDto = MapearBase<TipoAcompanhanteDto>(tipoAcompanhante);

            tipoAcompanhanteDto.Id = tipoAcompanhante.Id;
            tipoAcompanhanteDto.Codigo = tipoAcompanhante.Codigo;
            tipoAcompanhanteDto.Descricao = tipoAcompanhante.Descricao;
            tipoAcompanhanteDto.IsInternacao = tipoAcompanhante.IsInternacao;

            return tipoAcompanhanteDto;
        }
    }
}
