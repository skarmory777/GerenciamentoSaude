
//using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLeito;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto
{

    // [AutoMap(typeof(TipoLeito))]
    public class CriarOuEditarTipoLeito : CamposPadraoCRUDDto
    {

        public string TipoLeitoId { get; set; }

        public string CodigoTiss { get; set; }

        public bool IsLeitoExtra { get; set; }

    }
}
