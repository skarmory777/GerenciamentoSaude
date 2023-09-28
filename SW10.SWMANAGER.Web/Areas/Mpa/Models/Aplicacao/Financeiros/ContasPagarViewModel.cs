using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros
{
    [AutoMap(typeof(DocumentoDto))]
    public class ContasPagarViewModel : DocumentoDto
    {
        public ContasPagarViewModel(DocumentoDto output)
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