using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosTransferenciaLeito;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Dto
{
    [AutoMap(typeof(MotivoTransferenciaLeito))]
    public class MotivoTransferenciaLeitoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
