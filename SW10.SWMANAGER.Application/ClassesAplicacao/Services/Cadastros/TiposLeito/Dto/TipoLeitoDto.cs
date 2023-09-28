
//using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLeito;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto
{
    // [AutoMap(typeof(TipoLeito))]
    public class TipoLeitoDto : CamposPadraoCRUDDto
    {
        public string CodigoTiss { get; set; }

        public string Descricao { get; set; }
    }

}
