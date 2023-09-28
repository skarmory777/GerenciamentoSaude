using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Dto
{
    [AutoMap(typeof(FaturamentoBrasLaboratorio))]
    public class FaturamentoBrasLaboratorioDto : CamposPadraoCRUDDto
    {

        public static FaturamentoBrasLaboratorioDto Mapear(FaturamentoBrasLaboratorio faturamentoBrasLaboratorio)
        {
            return MapearBase<FaturamentoBrasLaboratorioDto>(faturamentoBrasLaboratorio);
        }

        public static FaturamentoBrasLaboratorio Mapear(FaturamentoBrasLaboratorioDto faturamentoBrasLaboratorioDto)
        {
            return MapearBase<FaturamentoBrasLaboratorio>(faturamentoBrasLaboratorioDto);
        }

    }
}
