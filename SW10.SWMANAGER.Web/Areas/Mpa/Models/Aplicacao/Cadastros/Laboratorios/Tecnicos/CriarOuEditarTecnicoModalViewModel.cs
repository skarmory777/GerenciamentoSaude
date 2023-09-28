using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Tecnicos
{
    [AutoMap(typeof(TecnicoDto))]
    public class CriarOuEditarTecnicoModalViewModel : TecnicoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarTecnicoModalViewModel(TecnicoDto output)
        {
            output.MapTo(this);
        }
    }
}