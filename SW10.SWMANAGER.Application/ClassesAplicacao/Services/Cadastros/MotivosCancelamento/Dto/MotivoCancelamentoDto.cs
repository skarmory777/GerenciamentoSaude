using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCancelamento;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Dto
{
    [AutoMap(typeof(MotivoCancelamento))]
    public class MotivoCancelamentoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
        public bool IsAtivo { get; set; }
    }
}
