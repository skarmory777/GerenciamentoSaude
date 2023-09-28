using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Manutencoes.BIs
{
    [AutoMap(typeof(BIDto))]
    public class CriarOuEditarBIViewModel : BIDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarBIViewModel(BIDto output)
        {
            output.MapTo(this);
        }
    }
}