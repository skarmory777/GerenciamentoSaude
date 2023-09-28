using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto
{
    [AutoMap(typeof(Modalidade))]
    public class ModalidadeDto : CamposPadraoCRUD
    {
        public bool IsParecer { get; set; }
        public string Filtro { get; set; }
    }
}

