using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    [AutoMap(typeof(PrescricaoItemStatusDto))]
    public class CriarOuEditarPrescricaoItemStatusViewModel : PrescricaoItemStatusDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarPrescricaoItemStatusViewModel(PrescricaoItemStatusDto output)
        {
            output.MapTo(this);
        }
    }
}