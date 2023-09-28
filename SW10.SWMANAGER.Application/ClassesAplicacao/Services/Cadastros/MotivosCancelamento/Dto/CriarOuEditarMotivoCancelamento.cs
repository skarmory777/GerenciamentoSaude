using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCancelamento;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Dto
{
    [AutoMap(typeof(MotivoCancelamento))]
    public class CriarOuEditarMotivoCancelamento : CamposPadraoCRUDDto
    {
        public bool IsAtivo { get; set; }
    }
}