using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    public class CriarOuEditarSubItensPrescricaoViewModel
    {
        public long PrescricaoItemId { get; set; }
        
        public string PrescricaoItemDescricao { get; set; }
        
        
        public long? SubPrescricaoItemId { get; set; }
        
        public PrescricaoItemDto SubPrescricaoItem { get; set; }

        public string SubItemPrescricaoDescricao()
        {
            if (SubPrescricaoItem != null)
            {
                return SubPrescricaoItem.Descricao;
            }

            return PrescricaoItemDescricao;
        }
    }
}