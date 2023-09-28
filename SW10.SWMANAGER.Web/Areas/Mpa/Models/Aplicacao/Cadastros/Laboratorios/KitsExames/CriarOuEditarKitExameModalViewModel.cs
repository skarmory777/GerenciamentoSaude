using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.KitsExames
{
    [AutoMap(typeof(KitExameDto))]
    public class CriarOuEditarKitExameModalViewModel : KitExameDto
    {
        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarKitExameModalViewModel(KitExameDto output)
        {
            output.MapTo(this);
        }
    }
}