using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Guias
{
    public class GuiaCampoViewModel
    {
        public SelectList Propriedades { get; set; }

        public string PropriedadeSelecionada { get; set; }

        public GuiaCampoViewModel(GuiaCampoDto output)
        {
            PropriedadeSelecionada = output.Descricao;
        }
    }
}