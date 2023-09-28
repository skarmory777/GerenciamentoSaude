using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros.Bancarios.TipoConta
{
    [AutoMap(typeof(TipoContaCorrenteDto))]
    public class TipoContaCorrenteViewModel : TipoContaCorrenteDto
    {
        public TipoContaCorrenteViewModel(TipoContaCorrenteDto output)
        {
            output.MapTo(this);
        }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public string Filtro { get; set; }
    }
}