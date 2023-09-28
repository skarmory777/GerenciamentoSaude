using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services;

using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Guias
{
    public class CampoViewModel : GuiaCampoDto
    {
        public SelectList Propriedades { get; set; }

        public GenericoIdNome PropriedadeSelecionada { get; set; }

        public int Contador { get; set; }

        public string SubCamposJson { get; set; }

        public CampoViewModel(GuiaCampoDto output)
        {
            Descricao = output.Descricao;
            CoordenadaX = output.CoordenadaX;
            CoordenadaY = output.CoordenadaY;
            ConjuntoId = output.Id;
            IsConjunto = output.IsConjunto;
            IsSubItem = output.IsSubItem;
            MaximoElementos = output.MaximoElementos;
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var sub = jsonSerializer.Serialize(output.SubConjuntos);
            SubCamposJson = sub;
        }
    }
}