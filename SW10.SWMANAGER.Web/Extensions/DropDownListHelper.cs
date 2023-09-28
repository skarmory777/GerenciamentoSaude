﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SW10.SWMANAGER.Web.Extensions
{
    public static class EnumEditorHtmlHelper
    {
        /// <summary>
        /// Creates the DropDown List (HTML Select Element) from LINQ 
        /// Expression where the expression returns an Enum type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
            where TModel : class
        {
            TProperty value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);
            string selected = value == null ? string.Empty : value.ToString();
            return htmlHelper.DropDownListFor(expression, createSelectList(expression.ReturnType, selected));
        }

        /// <summary>
        /// Creates the select list.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <returns></returns>
        private static IEnumerable<SelectListItem> createSelectList(Type enumType, string selectedItem)
        {
            var query =
             (from object item in Enum.GetValues(enumType)
              let fi = enumType.GetField(item.ToString())
              let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()
              let title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description
              select new SelectListItem
              {
                  Value = Convert.ToInt32(item).ToString(),//.ToString(),
                  Text = title,
                  Selected = selectedItem == Convert.ToInt32(item).ToString()
              }).ToList();
            return query;
        }

        /*
         Exemplo de uso:

         @Html.DropDownListFor(m => m.FavouriteColor) ==> Na view cujo o model contém o enum

         Neste exemplo, é necessário decorar os itens do enum com a anotação Description
         Exemplo:

    using System.ComponentModel;
    .
    .
    .
    public enum Sexo
    {
        [Description("Masculino")]
        Masculino,
        [Description("Feminino")]
        Feminino
    }
    .
    .
    .
         */
    }
}
