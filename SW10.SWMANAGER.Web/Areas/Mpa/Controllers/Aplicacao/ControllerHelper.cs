using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao
{
    /// <summary>
    /// Classe de apoio aos Controllers, para facilitar a população de elementos de ViewModels e outros
    /// </summary>
    public static class ControllerHelper
    {
        /// <summary>
        /// Retorna um List<SelectListItem> indicando qual deles está selecionado por padrão
        /// </summary>
        /// <param name="lista">testando descricao</param>
        /// <param name="idProp">Nome da propriedade que vai popular "id" do item da combo</param>
        /// <param name="descricaoProp">Nome da propriedade equivalente ao valor "text" do item da combo</param>
        /// <param name="selecionado">Valor do item que deve ser selecionado</param>
        /// <returns></returns>
        public static List<SelectListItem> ComboSelecionado(IReadOnlyList<object> lista, string idProp, string descricaoProp, string selecionado)
        {
            List<SelectListItem> opcoes = new List<SelectListItem>();

            foreach (var i in lista)
            {
                SelectListItem item = new SelectListItem { Value = i.GetType().GetProperty(idProp).GetValue(i, null).ToString(), Text = i.GetType().GetProperty(descricaoProp).GetValue(i, null).ToString() };

                if (item.Text.Equals(selecionado))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }

                opcoes.Add(item);
            }

            return opcoes;
        }
    }
}