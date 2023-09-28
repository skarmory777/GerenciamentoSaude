using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto
{
    [AutoMap(typeof(FaturamentoGuia))]
    public class FaturamentoGuiaDto : CamposPadraoCRUDDto
    {
        public const int GuiaConsulta = 1;
        public const int GuiaSpSadt = 2;
        public const int GuaResumoInternacao = 3;
        public const int GuiaHonorarioIndividual = 4;
        public const int GuiaParticular = 5;
        public const int GuiaParticularEmergencia = 6;
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }

        public bool IsEditMode()
        {
            if (this.Id != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static FaturamentoGuiaDto Mapear(FaturamentoGuia faturamentoGuia)
        {
            if (faturamentoGuia == null)
            {
                return null;
            }
            FaturamentoGuiaDto faturamentoGuiaDto = MapearBase<FaturamentoGuiaDto>(faturamentoGuia);

            faturamentoGuiaDto.Id = faturamentoGuia.Id;
            faturamentoGuiaDto.Codigo = faturamentoGuia.Codigo;
            faturamentoGuiaDto.Descricao = faturamentoGuia.Descricao;
            faturamentoGuiaDto.IsAmbulatorio = faturamentoGuia.IsAmbulatorio;
            faturamentoGuiaDto.IsInternacao = faturamentoGuia.IsInternacao;

            return faturamentoGuiaDto;
        }
    }
}
