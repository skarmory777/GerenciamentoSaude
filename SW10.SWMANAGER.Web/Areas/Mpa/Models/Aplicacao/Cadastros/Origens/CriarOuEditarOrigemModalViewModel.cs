using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Origens
{
    [AutoMapFrom(typeof(CriarOuEditarOrigem))]
    public class CriarOuEditarOrigemModalViewModel : CriarOuEditarOrigem
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }

        public SelectList UnidadesOrganizacionais { get; internal set; }

        public CriarOuEditarOrigemModalViewModel(CriarOuEditarOrigem output)
        {
            output.MapTo(this);
        }
    }
}