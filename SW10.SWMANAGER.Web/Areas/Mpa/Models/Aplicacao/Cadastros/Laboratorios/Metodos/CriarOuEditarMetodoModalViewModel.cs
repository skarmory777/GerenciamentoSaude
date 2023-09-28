using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Metodos
{
    [AutoMap(typeof(MetodoDto))]
    public class CriarOuEditarMetodoModalViewModel : MetodoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarMetodoModalViewModel(MetodoDto output)
        {
            output.MapTo(this);
        }
    }
}