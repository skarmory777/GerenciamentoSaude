using SW10.SWMANAGER.ClassesAplicacao.Controladorias.NotasFiscais;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Controladorias.NotasFiscais
{
    //[AutoMapFrom(typeof(NotaFiscal))]
    public class CriarOuEditarModalViewModel : NotaFiscal
    {
        public bool IsEditMode { get { return Id > 0; } }

        //public CriarOuEditarModalViewModel(NotaFiscal output)
        //{
        //    output = this;
        //    //output.MapTo(this);
        //}
    }
}