using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Setores
{
    [AutoMap(typeof(SetorDto))]
    public class CriarOuEditarSetorModalViewModel : SetorDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarSetorModalViewModel(SetorDto output)
        {
            output.MapTo(this);
        }
    }
}