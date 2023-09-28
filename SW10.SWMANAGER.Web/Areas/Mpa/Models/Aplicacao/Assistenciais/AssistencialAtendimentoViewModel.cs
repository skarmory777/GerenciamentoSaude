namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes;

    public class AssistencialAtendimentoViewModel : IndexViewModel
    {
        /// <summary>
        /// The atendimento.
        /// </summary>
        public AtendimentoDto Atendimento;
        public ModeloPrescricaoDto ModeloPrescricao;
        public string Permission;
    }
}