using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Dto
{
    [AutoMap(typeof(Modalidade))]
    public class ModalidadeDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }

        public bool IsParecer { get; set; }

    }
}
