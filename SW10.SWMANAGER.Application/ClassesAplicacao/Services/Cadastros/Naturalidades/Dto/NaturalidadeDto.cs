using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto
{
    [AutoMap(typeof(Naturalidade))]
    public class NaturalidadeDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static NaturalidadeDto Mapear( Naturalidade entity)
        {
            return MapearBase<NaturalidadeDto>(entity);
        }

        public static Naturalidade Mapear(NaturalidadeDto dto)
        {
            return MapearBase<Naturalidade>(dto);
        }

    }
}
