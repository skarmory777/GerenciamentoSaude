using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    [AutoMap(typeof(PrescricaoStatusDto))]
    public class CriarOuEditarPrescricaoStatusViewModel : PrescricaoStatusDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarPrescricaoStatusViewModel(PrescricaoStatusDto output)
        {
            output.MapTo(this);
        }
    }
}