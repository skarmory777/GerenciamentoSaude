using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento.Dto;
using System.Web.Mvc;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposEntrada;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposDocumento
{
    [AutoMap(typeof(CriarOuEditarTipoDocumento))]
    public class CriarOuEditarTipoDocumentoModalViewModel : CriarOuEditarTipoDocumento
    {
        public UserEditDto UpdateUser { get; set; }
        public SelectList TiposEntrada { get; set; }
      
        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarTipoDocumentoModalViewModel(CriarOuEditarTipoDocumento output)
        {
            output.MapTo(this);
        }
    }
}