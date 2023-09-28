using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Imagens
{
    [AutoMapFrom(typeof(GetExameItemForEditOutput))]
    public class CreateOrEditExameItemModalViewModel : GetExameItemForEditOutput
    {
        public string ModeloLaudo { get; set; }

        public CreateOrEditExameItemModalViewModel(GetExameItemForEditOutput output)
        {
            output.MapTo(this);
        }

        public bool IsEditMode
        {
            get { return ExameItem.Id.HasValue; }
        }
    }
}