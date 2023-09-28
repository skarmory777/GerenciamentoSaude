using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias
{
    [AutoMap(typeof(FrequenciaDto))]
    public class CriarOuEditarFrequenciaViewModel : FrequenciaDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarFrequenciaViewModel(FrequenciaDto output)
        {
            output.MapTo(this);
        }
    }
}