using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Estados
{
    [AutoMapFrom(typeof(EstadoDto))]
    public class CriarOuEditarEstadoModalViewModel : EstadoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Paises { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarEstadoModalViewModel(EstadoDto output)
        {
            output.MapTo(this);
        }
    }
}