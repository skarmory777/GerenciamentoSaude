using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Convenios
{
    [AutoMapFrom(typeof(ConvenioDto))]
    public class CriarOuEditarConvenioModalViewModel : ConvenioDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }

        public long Tabelas { get; set; }

        //public SelectList Estados { get; set; }

        //public SelectList Cidades { get; set; }

        //public SelectList Paises { get; set; }

        //public SelectList TiposTelefone { get; set; }

        public CriarOuEditarConvenioModalViewModel(ConvenioDto output)
        {
            output.MapTo(this);
        }
    }
}