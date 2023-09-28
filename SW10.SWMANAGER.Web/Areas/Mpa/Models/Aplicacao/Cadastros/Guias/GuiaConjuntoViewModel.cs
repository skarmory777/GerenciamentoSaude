using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;

using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Guias
{
    public class GuiaConjuntoViewModel // : GuiaCampoDto
    {
        public SelectList Propriedades { get; set; }

        public string PropriedadeSelecionada { get; set; }

        //public List<RelacaoGuiaCampoDto> Campos { get; set; }
        public List<GuiaCampoDto> Campos { get; set; }

        public long ConjuntoId { get; set; }

        public int Contador { get; set; }

        public bool IsConjunto { get; set; }

        public bool IsSubItem { get; set; }

        public int? MaximoElementos { get; set; }

        public string SubCamposJson { get; set; }

        public GuiaConjuntoViewModel(GuiaCampoDto output)
        {
            ConjuntoId = output.Id;
            IsConjunto = output.IsConjunto;
            IsSubItem = output.IsSubItem;
            MaximoElementos = output.MaximoElementos;
        }
    }
}