using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Materiais
{
    [AutoMap(typeof(MaterialDto))]
    public class CriarOuEditarMaterialModalViewModel : MaterialDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarMaterialModalViewModel(MaterialDto output)
        {
            output.MapTo(this);
        }
    }
}