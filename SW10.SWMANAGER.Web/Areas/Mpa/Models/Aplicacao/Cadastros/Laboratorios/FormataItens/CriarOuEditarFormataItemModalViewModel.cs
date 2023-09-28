using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.FormataItems
{
    [AutoMap(typeof(FormataItemDto))]
    public class CriarOuEditarFormataItemModalViewModel : FormataItemDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFormataItemModalViewModel(FormataItemDto output)
        {
            output.MapTo(this);
        }
    }
}