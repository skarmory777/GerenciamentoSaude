using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Cabecalho.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Cabecalho
{
    public class CriarOuEditarCabecalhoExameViewModel : CabecalhoDto
    {
        public UserEditDto UpdateUser { get; set; }

        //public bool IsEditMode
        //{
        //    get { return Id > 0; }
        //}
        public CriarOuEditarCabecalhoExameViewModel(CabecalhoDto output)
        {
            this.DescricaoCabecalho = output.DescricaoCabecalho;
            this.CodigoParamentroCabecalho = output.CodigoParamentroCabecalho;
            this.CodigoParamentroTextoCabecalho = output.CodigoParamentroTextoCabecalho;
            this.TextoCabecalho = output.TextoCabecalho;
        }
    }
}