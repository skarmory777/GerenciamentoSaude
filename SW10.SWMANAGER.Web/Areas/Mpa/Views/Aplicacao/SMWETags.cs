using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Views.SMWEHelpers
{
    public static class SMWETags
    {
        private static string Javascript { get; set; }
        private static string ScriptExterno { get; set; }

        public static void LimparJavascript()
        {
            Javascript = string.Empty;
            ScriptExterno = string.Empty;
        }

        public static BlocoJavascript InjetarJavascript(this HtmlHelper htmlHelper)
        {
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            var script = new TagBuilder("script");
            script.InnerHtml += Javascript;
            htmlHelper.ViewContext.Writer.Write(script.ToString(TagRenderMode.Normal));
            htmlHelper.ViewContext.Writer.Write(ScriptExterno);
            LimparJavascript();
            return new BlocoJavascript(htmlHelper.ViewContext);
        }

        public static BlocoHtml InputFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string value, string _label, int? col = null, bool disabled = false, bool required = false)
        {
            #region Modelo
            /*
                <div class="form-group">
                    <label>@L("Descricao")</label>
                    <input name="Descricao" class="form-control input-sm" type="text" value="@Model.Descricao" disabled>
                </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");

            var input = new TagBuilder("input");
            // Futuramente talvez precise filtrar o expression.body.nodeType
            input.MergeAttribute("name", ((MemberExpression)expression.Body).Member.Name);
            input.MergeAttribute("value", value);
            input.MergeAttribute("type", "text");
            input.AddCssClass("form-control input-sm");

            if (disabled)
                input.MergeAttribute("disabled", "disabled");

            if (required)
                input.MergeAttribute("required", "required");

            var label = new TagBuilder("label");
            label.InnerHtml += _label;

            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));

                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext);
        }

        public static BlocoHtml InputFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, SWTag atributosHtml, string _label)
        {
            #region Modelo
            /*
                <div class="form-group">
                    <label>@L("Descricao")</label>
                    <input name="Descricao" class="form-control input-sm" type="text" value="@Model.Descricao" disabled>
                </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var input = new TagBuilder("input");
            // Futuramente talvez precise filtrar o expression.body.nodeType
            input.MergeAttribute("name", ((MemberExpression)expression.Body).Member.Name);
            input.MergeAttribute("value", atributosHtml.Value);
            input.MergeAttribute("type", "text");

            var classeCss = "form-control input-sm";
            if (!string.IsNullOrEmpty(atributosHtml.Classe))
                classeCss += " " + atributosHtml.Classe;

            if (!string.IsNullOrEmpty(atributosHtml.Id))
                input.MergeAttribute("id", atributosHtml.Id);

            if (atributosHtml.Disabled)
                input.MergeAttribute("disabled", "disabled");

            if (atributosHtml.Required)
                input.MergeAttribute("required", "required");

            input.AddCssClass(classeCss);
            var label = new TagBuilder("label");
            label.InnerHtml += _label;

            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            if (atributosHtml.Col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", atributosHtml.Col.ToString());
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext);
        }

        public static BlocoHtml SWInput(this HtmlHelper htmlHelper, SWTag atributos)
        {
            #region Modelo
            /*
                <div class="form-group">
                    <label>@L("Descricao")</label>
                    <input name="Descricao" class="form-control input-sm" type="text" value="@Model.Descricao" disabled>
                </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var input = new TagBuilder("input");
            // Futuramente talvez precise filtrar o expression.body.nodeType
            input.MergeAttribute("name", atributos.Name);
            input.MergeAttribute("value", atributos.Value);
            input.MergeAttribute("type", "text");

            var classeCss = "form-control input-sm";
            if (!string.IsNullOrEmpty(atributos.Classe))
                classeCss += " " + atributos.Classe;

            if (!string.IsNullOrEmpty(atributos.Id))
                input.MergeAttribute("id", atributos.Id);

            if (atributos.Disabled)
                input.MergeAttribute("disabled", "disabled");

            if (atributos.Required)
                input.MergeAttribute("required", "required");

            input.AddCssClass(classeCss);
            var label = new TagBuilder("label");
            if(!string.IsNullOrEmpty(atributos.ClasseLabel))
            {
                label.AddCssClass(atributos.ClasseLabel);
            }

            label.InnerHtml += atributos.Label;

            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            if (atributos.Col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", atributos.Col.ToString());
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext);
        }

        public static BlocoHtml CheckboxSW(this HtmlHelper htmlHelper, string name, string id, string _label, bool _checked, int? col = null)
        {
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            #region Modelo
            /*
             <div class="md-checkbox">
                 <input name="name" id="id" class="md-check" type="checkbox" value="true" checked ou nao) />
                 <label for="id">
                     <span class="inc"></span>
                     <span class="check"></span>
                     <span class="box"></span>
                     @L("_label")
                 </label>
             </div>
            */


            /* NOVO MODELO
             <div class="col-sm-4">
                    <div class="form-group">
                        <label for="IsContraste">&nbsp;@L("IsContraste")</label>
                        <input name="IsContraste" id="IsContraste" type="checkbox" class="form-control icheck" value="true" @Html.Raw(Model.Exame.IsContraste ? "checked=\"checked\"" : "") />
                    </div>
                </div>
            */

            // NOVO

            //StringBuilder sb = new StringBuilder();

            //sb.Append("<div class=\"col-sm-4\"><div class=\"form-group\">");
            //sb.Append("<label for=\"IsContraste\">\"IsContraste\"</label>");
            //sb.Append("<input name=\"IsContraste\" id=\"IsContraste\" type=\"checkbox\" class=\"form-control icheck\" value=\"true\" \"checked=\"checked\" />");
            //sb.Append("</div></div>");

            //htmlHelper.ViewContext.Writer.Write(sb.ToString());
            //return new BlocoHtml(htmlHelper.ViewContext, string.Empty);


            //var div = new TagBuilder("div");
            //div.AddCssClass("form-group");
            //var label = new TagBuilder("label");
            //label.MergeAttribute("for", id);



            //var input = new TagBuilder("input");
            //input.MergeAttribute("name", name);
            //input.MergeAttribute("id", id);
            //input.AddCssClass("form-control icheck");
            //input.MergeAttribute("type", "checkbox");
            //input.MergeAttribute("value", _checked.ToString());

            //if (_checked)
            //{
            //    input.MergeAttribute("checked", "checked");
            //}

            //div.ToString(TagRenderMode.StartTag);
            //input.ToString(TagRenderMode.SelfClosing);
            //label.ToString(TagRenderMode.StartTag);
            //label.ToString(TagRenderMode.EndTag);


            //div.ToString(TagRenderMode.EndTag);
            //div.InnerHtml += input.ToString(TagRenderMode.Normal);
            //label.InnerHtml += _label;
            //div.InnerHtml += label.ToString(TagRenderMode.Normal);


            //if (col.HasValue)
            //{
            //    var coluna = new TagBuilder("div");
            //    string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
            //    coluna.AddCssClass(classe);

            //    coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
            //    htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
            //    return new BlocoHtml(htmlHelper.ViewContext);
            //}

            //htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            //return new BlocoHtml(htmlHelper.ViewContext, string.Empty);

            // FIM NOVO MODELO
            #endregion modelo.

            var div = new TagBuilder("div");
            div.AddCssClass("md-checkbox");
            var input = new TagBuilder("input");
            input.MergeAttribute("name", name);
            input.MergeAttribute("id", id);
            input.AddCssClass("md-check");
            input.MergeAttribute("type", "checkbox");
            input.MergeAttribute("value", _checked.ToString());

            if (_checked)
            {
                input.MergeAttribute("checked", "checked");
            }
            //else
            //{
            //    input.MergeAttribute("checked", string.Empty);
            //}

            var label = new TagBuilder("label");
            label.MergeAttribute("for", id);
            var spanInc = new TagBuilder("span");
            spanInc.AddCssClass("inc");
            var spanCheck = new TagBuilder("span");
            spanCheck.AddCssClass("check");
            var spanBox = new TagBuilder("span");
            spanBox.AddCssClass("box");

            div.InnerHtml += input.ToString(TagRenderMode.Normal);
            label.InnerHtml += spanInc.ToString(TagRenderMode.Normal);
            label.InnerHtml += spanCheck.ToString(TagRenderMode.Normal);
            label.InnerHtml += spanBox.ToString(TagRenderMode.Normal);
            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml CheckboxSWFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string id, string _label, bool _checked, int? col = null)
        {
            //  var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            #region Modelo
            /*
             <div class="md-checkbox">
                 <input name="name" id="id" class="md-check" type="checkbox" value="true" checked ou nao) />
                 <label for="id">
                     <span class="inc"></span>
                     <span class="check"></span>
                     <span class="box"></span>
                     @L("_label")
                 </label>
             </div>
            */
            #endregion modelo.

            var div = new TagBuilder("div");
            div.AddCssClass("md-checkbox");
            var input = new TagBuilder("input");
            input.MergeAttribute("name", ((MemberExpression)expression.Body).Member.Name);
            input.MergeAttribute("id", id);
            input.AddCssClass("md-check");
            input.MergeAttribute("type", "checkbox");
            input.MergeAttribute("value", "true");

            if (_checked)
            {
                //   input.MergeAttribute("value", "true");
                input.MergeAttribute("checked", "checked");
            }
            //else
            //{
            //    input.MergeAttribute("checked", string.Empty);
            //}

            var label = new TagBuilder("label");
            label.MergeAttribute("for", id);
            var spanInc = new TagBuilder("span");
            spanInc.AddCssClass("inc");
            var spanCheck = new TagBuilder("span");
            spanCheck.AddCssClass("check");
            var spanBox = new TagBuilder("span");
            spanBox.AddCssClass("box");

            div.InnerHtml += input.ToString(TagRenderMode.Normal);
            label.InnerHtml += spanInc.ToString(TagRenderMode.Normal);
            label.InnerHtml += spanCheck.ToString(TagRenderMode.Normal);
            label.InnerHtml += spanBox.ToString(TagRenderMode.Normal);
            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml SWRadios(this HtmlHelper htmlHelper, int diferenciadorGrupo, SWTag[] elementos, int? col = null)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("row");
            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));
            var classeDiferenciada = "radioCheckSW" + diferenciadorGrupo.ToString();

            foreach (var elemento in elementos)
            {
                var divCheck = new TagBuilder("div");
                divCheck.AddCssClass("md-checkbox");
                var input = new TagBuilder("input");
                input.MergeAttribute("name", elemento.Name);
                input.MergeAttribute("id", elemento.Id);
                var complemementoClasse = " md-check " + classeDiferenciada;
                elemento.Classe += complemementoClasse;
                input.AddCssClass(elemento.Classe);
                input.MergeAttribute("type", "checkbox");
                input.MergeAttribute("value", "true");

                if (elemento.Checked)
                {
                    input.MergeAttribute("checked", "checked");
                }

                var label = new TagBuilder("label");
                label.MergeAttribute("for", elemento.Id);
                var spanInc = new TagBuilder("span");
                spanInc.AddCssClass("inc");
                var spanCheck = new TagBuilder("span");
                spanCheck.AddCssClass("check");
                var spanBox = new TagBuilder("span");
                spanBox.AddCssClass("box");
                spanBox.MergeAttribute("style", "border-radius:25px; border:1px solid #666;");

                divCheck.InnerHtml += input.ToString(TagRenderMode.Normal);
                label.InnerHtml += spanInc.ToString(TagRenderMode.Normal);
                label.InnerHtml += spanCheck.ToString(TagRenderMode.Normal);
                label.InnerHtml += spanBox.ToString(TagRenderMode.Normal);
                label.InnerHtml += elemento.Label;
                divCheck.InnerHtml += label.ToString(TagRenderMode.Normal);

                if (elemento.Col.HasValue)
                {
                    var coluna = new TagBuilder("div");
                    string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", elemento.Col.ToString());
                    coluna.AddCssClass(classe);
                    coluna.InnerHtml += divCheck.ToString(TagRenderMode.Normal);
                    htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                }

                div.InnerHtml += divCheck.ToString(TagRenderMode.Normal);
            }

            #region Modelo Javascript
            /*
                  $(function(){$('.radioCheckSW').on('change',function(e){if($(this).is(':checked')){desmarcarTodos();$(this).attr('checked', true);}});});
                  function desmarcarTodos(){$('.radioCheckSW').each(function(){$(this).attr('checked', false);});}
             */
            #endregion

            // Javascript
            var funcaoDesmarcarTodos = "desmarcarTodos" + diferenciadorGrupo.ToString();
            string scriptString = string.Format("{0}{1}", "$(function(){$('." + classeDiferenciada + "').on('change',function(e){if($(this).is(':checked')){" + funcaoDesmarcarTodos + "();$(this).attr('checked', true);}});});", "function " + funcaoDesmarcarTodos + "(){$('." + classeDiferenciada + "').each(function(){$(this).attr('checked', false);});}");

            //if (injetado)
            //{
            //    Javascript += scriptString;
            //}
            //else
            //{
            var script = new TagBuilder("script");
            script.InnerHtml += scriptString;

            htmlHelper.ViewContext.Writer.Write(script.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, "</div>");
        }

        public static BlocoHtml Select2(this HtmlHelper htmlHelper, string name, string id, string classeCustom, string _label, string value, string placeHolder, string servicoMetodo, int? col = null, bool required = false)
        {
            /*
                  <div class="form-group">
                      <label>@L("Empresa")</label>
                      <select name="EmpresaId" id="comboEmpresa" class="form-control select2 select2Empresa" style="width:auto">
                          <option value="@Model.EmpresaId">@(Model.Empresa != null ? Model.Empresa.NomeFantasia : "") </option>
                          @*<option value=""> @L("Empresa") </option>*@
                      </select>
                  </div>
            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var select = new TagBuilder("select");
            select.MergeAttribute("name", name);
            select.MergeAttribute("id", id);
            select.AddCssClass("form-control select2 " + classeCustom);
            select.MergeAttribute("style", "width:auto");
            var option = new TagBuilder("option");
            select.MergeAttribute("value", value);

            if (required)
            {
                select.MergeAttribute("required", "required");
            }

            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            select.InnerHtml += option.ToString(TagRenderMode.Normal);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            var script = new TagBuilder("script");
            string scriptString = string.Format("{0}{1}{2}{3}{4}", "selectSW('.", classeCustom, "', '/api/services/app/", servicoMetodo, "', '');");

            Javascript += scriptString;

            script.InnerHtml += scriptString;
            div.InnerHtml += script.ToString(TagRenderMode.Normal);

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string id, string classeCss, string _label, string value, string text, string servicoMetodo, dynamic idComboMestre, int? col = null, bool injetado = false, bool required = false, string dataServico = "")
        {
            #region Modelo
            /*
                  <div class="form-group">
                      <label>@L("Empresa")</label>
                      <select name="EmpresaId" id="comboEmpresa" class="form-control select2 select2Empresa" style="width:auto">
                          <option value="@Model.EmpresaId">@(Model.Empresa != null ? Model.Empresa.NomeFantasia : "") </option>
                          @*<option value=""> @L("Empresa") </option>*@
                      </select>
                  </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            label.MergeAttribute("id", "cbo-lbl" + id);
            var select = new TagBuilder("select");
            select.MergeAttribute("name", ((MemberExpression)expression.Body).Member.Name);
            select.MergeAttribute("id", id);
            if (!string.IsNullOrEmpty(dataServico))
            {
                select.MergeAttribute("data-servico", dataServico);
            }
            select.AddCssClass("form-control select2 " + classeCss);
            select.MergeAttribute("style", "width:auto");


            if (required)
            {
                select.MergeAttribute("required", "required");
            }

            var option = new TagBuilder("option");
            option.MergeAttribute("value", value);
            option.InnerHtml += text;

            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            select.InnerHtml += option.ToString(TagRenderMode.Normal);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            // Javascript
            string scriptString = ""; // string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servicoMetodo, "', $(\"#", idComboMestre.ToString(), "\"));");

            if (idComboMestre == null)
                idComboMestre = "";
            //     Type tipo = ((System.Runtime.Remoting.ObjectHandle)idComboMestre).Unwrap().GetType();

            if (idComboMestre is string)
            {
                if (!string.IsNullOrEmpty(idComboMestre))
                {
                    scriptString = string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servicoMetodo, "', $(\"#", idComboMestre.ToString(), "\"));");
                }
                else
                {
                    scriptString = string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servicoMetodo, "', null", "", ");");
                }
            }
            else
            {
                string filtros = "filtrosSel2" + id;
                scriptString += "var " + filtros + "=[]; ";
                foreach (var f in idComboMestre)
                {
                    scriptString += filtros + ".push('" + f + "');";
                }

                scriptString += string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSWMultiplosFiltros('.", classeCss, "', '/api/services/app/", servicoMetodo, "',", filtros, ");");
                //                                                     selectSWMultiplosFiltros(classe, url, filtros, oi)
            }

            if (injetado)
            {
                Javascript += scriptString;
            }
            else
            {
                var script = new TagBuilder("script");
                script.InnerHtml += scriptString;
                div.InnerHtml += script.ToString(TagRenderMode.Normal);
            }

            // Bootstrap col
            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}{4}", "col-", "sm", "-", col.ToString(), " div" + _label);
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml SWComboName(this HtmlHelper htmlHelper, string nome, string id, string classeCss, string _label, string value, string text, string servicoMetodo, dynamic idComboMestre, int? col = null, bool injetado = false, bool required = false)
        {
            #region Modelo
            /*
                  <div class="form-group">
                      <label>@L("Empresa")</label>
                      <select name="EmpresaId" id="comboEmpresa" class="form-control select2 select2Empresa" style="width:auto">
                          <option value="@Model.EmpresaId">@(Model.Empresa != null ? Model.Empresa.NomeFantasia : "") </option>
                          @*<option value=""> @L("Empresa") </option>*@
                      </select>
                  </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var select = new TagBuilder("select");
            select.MergeAttribute("name", nome);
            select.MergeAttribute("id", id);
            select.AddCssClass("form-control select2 " + classeCss);
            select.MergeAttribute("style", "width:auto");


            if (required)
            {
                select.MergeAttribute("required", "required");
            }

            var option = new TagBuilder("option");
            option.MergeAttribute("value", value);
            option.InnerHtml += text;

            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            select.InnerHtml += option.ToString(TagRenderMode.Normal);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            // Javascript
            string scriptString = ""; // string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servicoMetodo, "', $(\"#", idComboMestre.ToString(), "\"));");

            if (idComboMestre == null)
                idComboMestre = "";
            //     Type tipo = ((System.Runtime.Remoting.ObjectHandle)idComboMestre).Unwrap().GetType();

            if (idComboMestre is string)
            {
                if (!string.IsNullOrEmpty(idComboMestre))
                {
                    scriptString = string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servicoMetodo, "', $(\"#", idComboMestre.ToString(), "\"));");
                }
                else
                {
                    scriptString = string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servicoMetodo, "', null", "", ");");
                }
            }
            else
            {
                string filtros = "filtrosSel2" + id;
                scriptString += "var " + filtros + "=[]; ";
                foreach (var f in idComboMestre)
                {
                    scriptString += filtros + ".push('" + f + "');";
                }

                scriptString += string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSWMultiplosFiltros('.", classeCss, "', '/api/services/app/", servicoMetodo, "',", filtros, ");");
                //                                                     selectSWMultiplosFiltros(classe, url, filtros, oi)
            }

            if (injetado)
            {
                Javascript += scriptString;
            }
            else
            {
                var script = new TagBuilder("script");
                script.InnerHtml += scriptString;
                div.InnerHtml += script.ToString(TagRenderMode.Normal);
            }

            // Bootstrap col
            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}{4}", "col-", "sm", "-", col.ToString(), " div" + _label);
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string id, string classeCss, string _label, string value, string text, string servico, dynamic idComboMestre, string metodo, int? col = null, bool injetado = false, bool required = false, string setter = "", string resetter = "")
        {
            #region Modelo
            /*
                  <div class="form-group">
                      <label>@L("Empresa")</label>
                      <select name="EmpresaId" id="comboEmpresa" class="form-control select2 select2Empresa" style="width:auto">
                          <option value="@Model.EmpresaId">@(Model.Empresa != null ? Model.Empresa.NomeFantasia : "") </option>
                          @*<option value=""> @L("Empresa") </option>*@
                      </select>
                  </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var select = new TagBuilder("select");
            select.MergeAttribute("name", ((MemberExpression)expression.Body).Member.Name);
            select.MergeAttribute("id", id);
            select.AddCssClass("form-control select2 " + classeCss);
            select.MergeAttribute("style", "width:auto");


            if (required)
            {
                select.MergeAttribute("required", "required");
            }

            var option = new TagBuilder("option");
            option.MergeAttribute("value", value);
            option.InnerHtml += text;

            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            select.InnerHtml += option.ToString(TagRenderMode.Normal);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            // Javascript
            string scriptString = ""; // string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servicoMetodo, "', $(\"#", idComboMestre.ToString(), "\"));");

            if (idComboMestre == null)
                idComboMestre = "";
            //     Type tipo = ((System.Runtime.Remoting.ObjectHandle)idComboMestre).Unwrap().GetType();

            if (idComboMestre is string)
            {
                if (!string.IsNullOrEmpty(idComboMestre))
                {
                    scriptString = string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servico + "/" + metodo, "', $(\"#", idComboMestre.ToString(), "\"));");
                }
                else
                {
                    scriptString = string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", classeCss, "', '/api/services/app/", servico + "/" + metodo, "', null", "", ");");
                }
            }
            else
            {
                string filtros = "filtrosSel2" + id;
                scriptString += "var " + filtros + "=[]; ";
                foreach (var f in idComboMestre)
                {
                    scriptString += filtros + ".push('" + f + "');";
                }

                scriptString += string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSWMultiplosFiltros('.", classeCss, "', '/api/services/app/", servico + "/" + metodo, "',", filtros, ");");
                //                                                     selectSWMultiplosFiltros(classe, url, filtros, oi)
            }

            if (!string.IsNullOrEmpty(setter))
            {
                scriptString += "function " + setter + "(id) { abp.services.app." + servico + ".obter(id)";
                scriptString += ".done(function(data) { if(!data) return;  var option = new Option(data.descricao || data.nomeFantasia || data.name || data.nome, data.id, true, true);";
                scriptString += "var comboSel2 = $('#" + id + "'); comboSel2.append(option).trigger('change'); comboSel2.trigger({type: 'select2:select', params: {data: data } }); });}";
            }

            if (!string.IsNullOrEmpty(resetter))
            {
                scriptString += "function " + resetter + "() { $('#" + id + "').empty().trigger('change'); }";
            }

            if (injetado)
            {
                Javascript += scriptString;
            }
            else
            {
                var script = new TagBuilder("script");
                script.InnerHtml += scriptString;
                div.InnerHtml += script.ToString(TagRenderMode.Normal);
            }

            // Bootstrap col
            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, SWTag atributosHtml, string servicoMetodo, string filtro = null, string idComboMestre = null, int? col = null, bool injetado = false, bool required = false)
        {
            #region Modelo
            /*
                  <div class="form-group">
                      <label>@L("Empresa")</label>
                      <select name="EmpresaId" id="comboEmpresa" class="form-control select2 select2Empresa" style="width:auto">
                          <option value="@Model.EmpresaId">@(Model.Empresa != null ? Model.Empresa.NomeFantasia : "") </option>
                          @*<option value=""> @L("Empresa") </option>*@
                      </select>
                  </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var select = new TagBuilder("select");
            select.MergeAttribute("name", ((MemberExpression)expression.Body).Member.Name);
            select.MergeAttribute("id", atributosHtml.Id);
            select.AddCssClass("form-control select2 " + atributosHtml.Classe);
            select.MergeAttribute("style", "width:auto");


            if (required)
            {
                select.MergeAttribute("required", "required");
            }



            var option = new TagBuilder("option");
            option.MergeAttribute("value", atributosHtml.Value);
            option.InnerHtml += atributosHtml.Text;

            label.InnerHtml += atributosHtml.Label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            select.InnerHtml += option.ToString(TagRenderMode.Normal);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            // Javascript
            string scriptString = string.Format("{0}{1}{2}{3}{4}{5}{6}", "selectSW('.", atributosHtml.Classe, "', '/api/services/app/", servicoMetodo, "', $(\"#", idComboMestre, "\"));");

            if (injetado)
            {
                Javascript += scriptString;
            }
            else
            {
                var script = new TagBuilder("script");
                script.InnerHtml += scriptString;
                div.InnerHtml += script.ToString(TagRenderMode.Normal);
            }

            // Bootstrap col
            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}{4}", "col-", "sm", "-", col.ToString(), " div" + atributosHtml.Label);
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml Select2MestreFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string id, string classeCustom, string _label, string value, string placeHolder, string servicoMetodo, string dependenteClasse, string servicoMetodoDependente, int? col = null, bool injetado = false)
        {
            #region Modelo
            /*
                  <div class="form-group">
                      <label>@L("Empresa")</label>
                      <select name="EmpresaId" id="comboEmpresa" class="form-control select2 select2Empresa" style="width:auto">
                          <option value="@Model.EmpresaId">@(Model.Empresa != null ? Model.Empresa.NomeFantasia : "") </option>
                          @*<option value=""> @L("Empresa") </option>*@
                      </select>
                  </div>
            */
            #endregion modelo.

            #region Elementos
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var select = new TagBuilder("select");
            select.MergeAttribute("name", ((MemberExpression)expression.Body).Member.Name);
            select.MergeAttribute("id", id);
            select.AddCssClass("form-control select2 " + classeCustom);
            select.MergeAttribute("style", "width:auto");
            var option = new TagBuilder("option");
            option.MergeAttribute("value", value);

            div.ToString(TagRenderMode.StartTag);
            label.ToString(TagRenderMode.SelfClosing);
            select.ToString(TagRenderMode.StartTag);
            option.ToString(TagRenderMode.SelfClosing);
            select.ToString(TagRenderMode.EndTag);
            div.ToString(TagRenderMode.EndTag);
            #endregion elementos.

            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            select.InnerHtml += option.ToString(TagRenderMode.Normal);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            #region Modelo Javascript (dependencia entre combos)
            /*
            Modelo javascript para select2 dependente
            $("#combo-tipo").on("change", function () { var tipoId = $(this).val();
                selectSW(".select2Grupo", "/api/services/app/faturamentoGrupo/ListarDropdown", tipoId); });
            
            */
            #endregion modelo javascript.

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("{0}{1}{2}{3}{4}", "selectSW('.", classeCustom, "', '/api/services/app/", servicoMetodo, "', '');"));
            //    stringBuilder.Append("$(\"#" + id + "\").on(\"change\", function () { var mestreId = $(\"#" + id + "\").val();");
            stringBuilder.Append("selectSW(\"." + dependenteClasse + "\", \"/api/services/app/" + servicoMetodoDependente + "\", $(\"#" + id + "\"));");
            stringBuilder.Append("select2MestreFor('" + id + "', '" + dependenteClasse + "' , '" + servicoMetodoDependente + "');");

            string scriptString = stringBuilder.ToString();

            // Javascript
            if (injetado)
            {
                Javascript += scriptString;
            }
            else
            {
                var script = new TagBuilder("script");
                script.InnerHtml += scriptString;
                div.InnerHtml += script.ToString(TagRenderMode.Normal);
            }

            // Bootstrap col
            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml DatePicker(this HtmlHelper htmlHelper, string name, string value, string _label, int? col = null, bool injetado = false)
        {
            /*
              <div class="form-group">
                  <label>@L("DataFim")</label>
                  <input name="DataFim" class="form-control input-sm" type="text" value="@DateTime.Now.ToString("dd/MM/yyyy HH:mm")" ng-pattern="/^((0[1-9]|1[012])[\-](0[1-9]|[12][0-9]|3[01])[\-](19|20)[0-9]{2})*$/" />
              </div>
            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var input = new TagBuilder("input");
            input.MergeAttribute("name", name);
            input.AddCssClass("form-control input-sm");

            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            var script = new TagBuilder("script");
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("$('input[name=\"" + name + "\"]').daterangepicker({");
            stringBuilder.Append("\"singleDatePicker\": true, \"showDropdowns\": true, autoUpdateInput: false, maxDate: new Date(), changeYear: true, yearRange: 'c-10:c+10', showOn: \"both\",");
            stringBuilder.Append("\"locale\": { \"format\": moment.locale().toUpperCase() === 'PT-BR' ? \"DD/MM/YYYY\" : moment.locale().toUpperCase() === 'US' ? \"MM/DD/YYYY\" : \"YYYY-MM-DD\",");
            stringBuilder.Append("\"separator\": \" - \", \"applyLabel\": \"Apply\", \"cancelLabel\": \"Cancel\", \"fromLabel\": \"From\", \"toLabel\": \"To\", \"customRangeLabel\": \"Custom\", \"daysOfWeek\": [");
            stringBuilder.Append("app.localize('Dom'), app.localize('Seg'), app.localize('Ter'), app.localize('Qua'), app.localize('Qui'), app.localize('Sex'), app.localize('Sab')],");
            stringBuilder.Append("\"monthNames\": [app.localize(\"Jan\"),app.localize(\"Fev\"),app.localize(\"Mar\"),app.localize(\"Abr\"),app.localize(\"Mai\"),app.localize(\"Jun\"),app.localize(\"Jul\"),app.localize(\"Ago\"),app.localize(\"Set\"),app.localize(\"Out\"),app.localize(\"Nov\"),app.localize(\"Dez\"),],");
            stringBuilder.Append("\"firstDay\": 0 } }, function (selDate) { $('input[name=\"" + name + "\"]').val(selDate.format('L')).addClass('form-control edited'); });");

            string scriptString = stringBuilder.ToString();
            //script.InnerHtml += scriptString;
            //div.InnerHtml += script.ToString(TagRenderMode.Normal);

            if (injetado)
            {
                Javascript += scriptString;
            }
            else
            {
                script.InnerHtml += scriptString;
                div.InnerHtml += script.ToString(TagRenderMode.Normal);
            }

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml SWDatePicker(this HtmlHelper htmlHelper, SWTag atributos, bool injetado = false, string maxDate = "new Date()")
        {
            /*
              <div class="form-group">
                  <label>@L("DataFim")</label>
                  <input name="DataFim" class="form-control input-sm" type="text" value="@DateTime.Now.ToString("dd/MM/yyyy HH:mm")" ng-pattern="/^((0[1-9]|1[012])[\-](0[1-9]|[12][0-9]|3[01])[\-](19|20)[0-9]{2})*$/" />
              </div>
            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var input = new TagBuilder("input");
            input.MergeAttribute("name", atributos.Name);
            input.MergeAttribute("id", atributos.Id);
            input.AddCssClass("form-control input-sm");

            label.InnerHtml += atributos.Label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            var script = new TagBuilder("script");
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("$('input[name=\"" + atributos.Name + "\"]').daterangepicker({");
            stringBuilder.Append("\"singleDatePicker\": true, \"showDropdowns\": true, autoUpdateInput: false, maxDate: new Date('2080-01-01'), changeYear: true, yearRange: 'c-10:c+10', showOn: \"both\",");
            stringBuilder.Append("\"locale\": { \"format\": moment.locale().toUpperCase() === 'PT-BR' ? \"DD/MM/YYYY\" : moment.locale().toUpperCase() === 'US' ? \"MM/DD/YYYY\" : \"YYYY-MM-DD\",");
            stringBuilder.Append("\"separator\": \" - \", \"applyLabel\": \"Apply\", \"cancelLabel\": \"Cancel\", \"fromLabel\": \"From\", \"toLabel\": \"To\", \"customRangeLabel\": \"Custom\", \"daysOfWeek\": [");
            stringBuilder.Append("app.localize('Dom'), app.localize('Seg'), app.localize('Ter'), app.localize('Qua'), app.localize('Qui'), app.localize('Sex'), app.localize('Sab')],");
            stringBuilder.Append("\"monthNames\": [app.localize(\"Jan\"),app.localize(\"Fev\"),app.localize(\"Mar\"),app.localize(\"Abr\"),app.localize(\"Mai\"),app.localize(\"Jun\"),app.localize(\"Jul\"),app.localize(\"Ago\"),app.localize(\"Set\"),app.localize(\"Out\"),app.localize(\"Nov\"),app.localize(\"Dez\"),],");
            stringBuilder.Append("\"firstDay\": 0 } }, function (selDate) { $('input[name=\"" + atributos.Name + "\"]').val(selDate.format('L')).addClass('form-control edited'); });");

            script.InnerHtml += stringBuilder.ToString();

            if (injetado)
            {
                Javascript += script.InnerHtml;
            }
            else
            {
                div.InnerHtml += script.ToString(TagRenderMode.Normal);
            }

            if (atributos.Col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", atributos.Col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string value, string _label, int? col = null)
        {
            #region Modelo
            /*
              <div class="form-group">
                  <label>@L("DataFim")</label>
                  <input name="DataFim" class="form-control input-sm" type="text" value="@DateTime.Now.ToString("dd/MM/yyyy HH:mm")" ng-pattern="/^((0[1-9]|1[012])[\-](0[1-9]|[12][0-9]|3[01])[\-](19|20)[0-9]{2})*$/" />
              </div>
            */
            #endregion modelo.

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            // este input serve apenas para evitar que o datePicker "popupeie" automaticamente caso seja o 1o input da form
            var shield = new TagBuilder("input");
            shield.MergeAttribute("class", "datepicker-shield");
            var label = new TagBuilder("label");
            var input = new TagBuilder("input");
            var name = ((MemberExpression)expression.Body).Member.Name;
            input.MergeAttribute("name", name);
            input.MergeAttribute("value", value);
            input.MergeAttribute("type", "text");
            input.AddCssClass("form-control input-sm");

            //div.ToString(TagRenderMode.StartTag);
            //label.ToString(TagRenderMode.SelfClosing);
            //input.ToString(TagRenderMode.SelfClosing);
            //div.ToString(TagRenderMode.EndTag);

            label.InnerHtml += _label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += shield.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            var script = new TagBuilder("script");
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("$('input[name=\"" + name + "\"]').daterangepicker({");
            stringBuilder.Append("\"singleDatePicker\": true, \"showDropdowns\": true, autoUpdateInput: false, maxDate: new Date(\"2100/01/01\"), changeYear: true, yearRange: 'c-10:c+10', showOn: \"both\",");
            stringBuilder.Append("\"locale\": { \"format\": moment.locale().toUpperCase() === 'PT-BR' ? \"DD/MM/YYYY\" : moment.locale().toUpperCase() === 'US' ? \"MM/DD/YYYY\" : \"YYYY-MM-DD\",");
            stringBuilder.Append("\"separator\": \" - \", \"applyLabel\": \"Apply\", \"cancelLabel\": \"Cancel\", \"fromLabel\": \"From\", \"toLabel\": \"To\", \"customRangeLabel\": \"Custom\", \"daysOfWeek\": [");
            stringBuilder.Append("app.localize('Dom'), app.localize('Seg'), app.localize('Ter'), app.localize('Qua'), app.localize('Qui'), app.localize('Sex'), app.localize('Sab')],");
            stringBuilder.Append("\"monthNames\": [app.localize(\"Jan\"),app.localize(\"Fev\"),app.localize(\"Mar\"),app.localize(\"Abr\"),app.localize(\"Mai\"),app.localize(\"Jun\"),app.localize(\"Jul\"),app.localize(\"Ago\"),app.localize(\"Set\"),app.localize(\"Out\"),app.localize(\"Nov\"),app.localize(\"Dez\"),],");
            stringBuilder.Append("\"firstDay\": 0 } }, function (selDate) { $('input[name=\"" + name + "\"]').val(selDate.format('L')).addClass('form-control edited'); });");
            stringBuilder.Append("$('.datepicker-shield').hide();");

            string scriptString = stringBuilder.ToString();
            script.InnerHtml += scriptString;
            div.InnerHtml += script.ToString(TagRenderMode.Normal);

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml Row(this HtmlHelper htmlHelper, SWTag atributosHtml = null)
        {
            /*
                EXEMPLO:
                <div class="row">
            */
            //     var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            string classe = string.Format("{0}{1}{2}", "row", " ", atributosHtml != null ? atributosHtml.Classe : string.Empty);
            div.AddCssClass(classe);

            if (atributosHtml != null)
            {
                div.MergeAttribute("id", atributosHtml.Id);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));
            return new BlocoHtml(htmlHelper.ViewContext, "</div>");
        }

        public static BlocoHtml Col(this HtmlHelper htmlHelper, string device, int tamanho, SWTag atributosHtml = null)
        {
            /*
                EXEMPLO:
                <div class="col-sm-2">
            */
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            string classe = string.Format("{0}{1}{2}{3}{4}{5}", "col-", device, "-", tamanho.ToString(), " ", atributosHtml != null ? atributosHtml.Classe : string.Empty);
            div.AddCssClass(classe);

            if (atributosHtml != null)
            {
                div.MergeAttribute("id", atributosHtml.Id);
                //  div.MergeAttribute("class", " " + atributosHtml.Classe);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));
            return new BlocoHtml(htmlHelper.ViewContext, "</div>");
        }

        public static BlocoHtml Abas(this HtmlHelper htmlHelper)
        {
            /*
                EXEMPLO:
                <ul class="nav nav-tabs">
            */
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var ul = new TagBuilder("ul");
            string classe = "nav nav-tabs";
            ul.AddCssClass(classe);

            htmlHelper.ViewContext.Writer.Write(ul.ToString(TagRenderMode.StartTag));
            return new BlocoHtml(htmlHelper.ViewContext, "</ul>");
        }

        public static BlocoHtml AbasConteudo(this HtmlHelper htmlHelper)
        {
            /*
                EXEMPLO:
                <div class="tab-content container-fluid">
            */
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            string classe = "tab-content container-fluid";
            div.AddCssClass(classe);

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));
            return new BlocoHtml(htmlHelper.ViewContext, "</div>");
        }

        public static BlocoHtml Aba(this HtmlHelper htmlHelper, string href, string _label, bool active, string id = "")
        {
            /*
                EXEMPLO:
                <li class="active">
                    <a href="#tab_itens" data-toggle="tab" aria-expanded="?">@L("Itens")
                    </a>
            */
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var li = new TagBuilder("li");
            string ariaExpanded = "false";

            if (active)
            {
                string classe = "active";
                li.AddCssClass(classe);
                ariaExpanded = "true";
            }

            var a = new TagBuilder("a");
            href = "#" + href;
            a.MergeAttribute("href", href);
            a.MergeAttribute("data-toggle", "tab");
            a.MergeAttribute("aria-expanded", ariaExpanded);

            if (!string.IsNullOrEmpty(id))
            {
                a.MergeAttribute("id", id);
            }

            a.InnerHtml += _label;

            li.InnerHtml += a.ToString(TagRenderMode.Normal);

            htmlHelper.ViewContext.Writer.Write(li.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, "</li>");
        }

        public static BlocoHtml AbaConteudo(this HtmlHelper htmlHelper, string id, bool active)
        {
            /*
                EXEMPLO:
                <div class="tab-pane active" id="PrincipalInformationsTab">
            */
            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            string classe = "tab-pane";

            if (active)
            {
                classe += " active";
            }

            div.AddCssClass(classe);
            div.MergeAttribute("id", id);

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));
            return new BlocoHtml(htmlHelper.ViewContext, "</div>");
        }

        public static BlocoHtml DateRangePicker(this HtmlHelper htmlHelper, SWTag atributos)
        {
            #region Exemplo
            /*
              <div id="divDataRange">
                  <div class="form-group">
                      <label class="control-label">@L("DateRange")</label>
                      <input id="dateRange" type="text" class="form-control date-range-picker" />
                  </div>
              </div>
            */
            #endregion

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var input = new TagBuilder("input");
            div.MergeAttribute("id", atributos.Id);
            input.MergeAttribute("id", "date-range");
            input.MergeAttribute("type", "text");
            input.AddCssClass("form-control date-range-picker");

            label.InnerHtml += atributos.Label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            var script = new TagBuilder("script");
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("var _selectedDateRange = {startDate: moment().startOf('day'),endDate: moment().endOf('day')};");
            stringBuilder.Append("$('.date-range-picker");
            stringBuilder.Append("').daterangepicker($.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),");

            // maxDate: '2016/01/05 23:59:55'

            stringBuilder.Append("function (start, end, label) {_selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');");
            stringBuilder.Append("_selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');});");


            string scriptString = stringBuilder.ToString();

            Javascript += scriptString;

            if (atributos.Col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", atributos.Col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        // Date range picker com definificao de data inicial e final
        public static BlocoHtml DateRangePicker1aGuerra(this HtmlHelper htmlHelper, SWTag atributos)
        {
            #region Exemplo
            /*
              <div id="divDataRange">
                  <div class="form-group">
                      <label class="control-label">@L("DateRange")</label>
                      <input id="dateRange" type="text" class="form-control date-range-picker" />
                  </div>
              </div>
            */
            #endregion

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var div = new TagBuilder("div");
            div.AddCssClass("form-group");
            var label = new TagBuilder("label");
            var input = new TagBuilder("input");
            div.MergeAttribute("id", atributos.Id);
            input.MergeAttribute("id", "drx" + atributos.Id);
            input.MergeAttribute("type", "text");
            input.AddCssClass("form-control date-range-picker");

            label.InnerHtml += atributos.Label;
            div.InnerHtml += label.ToString(TagRenderMode.Normal);
            div.InnerHtml += input.ToString(TagRenderMode.Normal);

            var script = new TagBuilder("script");
            var stringBuilder = new StringBuilder();

            //stringBuilder.Append("var _selectedDateRange = {startDate: moment().subtract(80, 'years').calendar() ,endDate: moment().endOf('day')};");
            stringBuilder.Append("var _selectedDateRange = {startDate: moment().subtract(7, 'days').calendar() ,endDate: moment().endOf('day')};");
            stringBuilder.Append("$('.date-range-picker");
            stringBuilder.Append("').daterangepicker($.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),");

            // maxDate: '2016/01/05 23:59:55'

            stringBuilder.Append("function (start, end, label) {_selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');");
            stringBuilder.Append("_selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');});");


            string scriptString = stringBuilder.ToString();

            Javascript += scriptString;

            if (atributos.Col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", atributos.Col.ToString());
                coluna.AddCssClass(classe);

                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }


        //public static BlocoHtml DateRangePickerMaxDate (this HtmlHelper htmlHelper, SWTag atributos, string maxDate)
        //{
        //    #region Exemplo
        //    /*
        //      <div id="divDataRange">
        //          <div class="form-group">
        //              <label class="control-label">@L("DateRange")</label>
        //              <input id="dateRange" type="text" class="form-control date-range-picker" />
        //          </div>
        //      </div>
        //    */
        //    #endregion

        //    var htmlTextWriter = new HtmlTextWriter(new StringWriter());

        //    var div = new TagBuilder("div");
        //    div.AddCssClass("form-group");
        //    var label = new TagBuilder("label");
        //    var input = new TagBuilder("input");
        //    div.MergeAttribute("id", atributos.Id);
        //    input.MergeAttribute("id", "date-range");
        //    input.MergeAttribute("type", "text");
        //    input.AddCssClass("form-control date-range-picker");

        //    label.InnerHtml += atributos.Label;
        //    div.InnerHtml += label.ToString(TagRenderMode.Normal);
        //    div.InnerHtml += input.ToString(TagRenderMode.Normal);

        //    var script = new TagBuilder("script");
        //    var stringBuilder = new StringBuilder();

        //    //stringBuilder.Append("var _selectedDateRange = {startDate: moment().startOf('day'),endDate: moment().endOf('day')};");
        //    //stringBuilder.Append("$('.date-range-picker");
        //    //stringBuilder.Append("').daterangepicker($.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),");

        //    //// maxDate: '2016/01/05 23:59:55'

        //    //stringBuilder.Append("function (start, end, label) {_selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');");
        //    //stringBuilder.Append("_selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');});");





        //    stringBuilder.Append("$('input[name=\""+atributos.Name+"\"]').daterangepicker({\"singleDatePicker\": true, \"showDropdowns\": true, autoUpdateInput: false,                     ");
        //    stringBuilder.Append("maxDate: '"+maxDate+"', changeYear: true, yearRange: 'c-10:c+10', showOn: \"both\", \"locale\": {                                                      ");
        //    stringBuilder.Append("\"format\": moment.locale().toUpperCase() === 'PT-BR' ? \"DD/MM/YYYY\" : moment.locale().toUpperCase() === 'US' ? \"MM/DD/YYYY\" : \"YYYY-MM-DD\",  ");
        //    stringBuilder.Append("\"separator\": \" - \", \"applyLabel\": \"Apply\", \"cancelLabel\": \"Cancel\", \"fromLabel\": \"From\", \"toLabel\": \"To\",                       ");
        //    stringBuilder.Append("\"customRangeLabel\": \"Custom\", \"daysOfWeek\": [ app.localize('Dom'), app.localize('Seg'),                                                       ");
        //    stringBuilder.Append("app.localize('Ter'), app.localize('Qua'), app.localize('Qui'), app.localize('Sex'),                                                                 ");
        //    stringBuilder.Append("app.localize('Sab') ], \"monthNames\": [ app.localize(\"Jan\"), app.localize(\"Fev\"),                                                              ");
        //    stringBuilder.Append("app.localize(\"Mar\"), app.localize(\"Abr\"), app.localize(\"Mai\"), app.localize(\"Jun\"),                                                         ");
        //    stringBuilder.Append("app.localize(\"Jul\"), app.localize(\"Ago\"), app.localize(\"Set\"), app.localize(\"Out\"),                                                         ");
        //    stringBuilder.Append("app.localize(\"Nov\"), app.localize(\"Dez\"), ], \"firstDay\": 0 } }, function (selDate) {                                                          ");
        //    stringBuilder.Append("$('input[name=\""+atributos.Name+"\"]').val(selDate.format('L')).addClass('form-control edited'); });                                                     ");






        //    string scriptString = stringBuilder.ToString();

        //    Javascript += scriptString;

        //    if (atributos.Col.HasValue)
        //    {
        //        var coluna = new TagBuilder("div");
        //        string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", atributos.Col.ToString());
        //        coluna.AddCssClass(classe);

        //        coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
        //        htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
        //        return new BlocoHtml(htmlHelper.ViewContext);
        //    }

        //    htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
        //    return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        //}

        public static BlocoHtml SWMultiSelect(this HtmlHelper htmlHelper, string headerEsquerda, string headerDireita, string nomeServico, string metodo, MultiSelectItem[] selecionadosIds = null, int? col = 3, bool injetado = false)
        {
            #region Exemplo Html
            /*  HTML exemplo
                <div>
                    <input type="text" id="filtro" placeholder="Search for names..">
                    <select id='swmulti' multiple='multiple'></select>
                </div>
            */
            #endregion

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            var div = new TagBuilder("div");
            var select = new TagBuilder("select");
            select.MergeAttribute("id", "swmulti");
            select.MergeAttribute("multiple", "multiple");
            var input = new TagBuilder("input");
            input.MergeAttribute("type", "text");
            input.MergeAttribute("id", "filtro-multi");
            var scriptPre = new TagBuilder("script");

            // Array de ids dos selecionados
            string selecionados = "var selecionadosIds = [";

            if (selecionadosIds != null && selecionadosIds.Length > 0)
            {
                foreach (var s in selecionadosIds)
                {
                    selecionados += "{ id:";
                    selecionados += s.id.ToString() + ",";
                    selecionados += "checado:";
                    selecionados += s.checado.ToString().ToLower();
                    selecionados += "},";
                }

                selecionados = selecionados.Remove(selecionados.Length - 1);
            }

            selecionados += "];";
            scriptPre.InnerHtml += selecionados;
            scriptPre.InnerHtml += "var headerEsquerda = '" + headerEsquerda + "'; var headerDireita = '" + headerDireita + "';";
            scriptPre.InnerHtml += "function SMWETagschamarServico() {abp.services.app." + nomeServico + "." + metodo + "({}).done(function(data) { data.items.forEach(inserirOpcao);});}";
            var scriptExterno = new TagBuilder("script");
            scriptExterno.MergeAttribute("src", "/Scripts/SMWEHelpers/SMWEMultiSelect.js");
            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            if (injetado)
            {
                Javascript += scriptPre.InnerHtml;
                ScriptExterno = scriptExterno.ToString(TagRenderMode.Normal);
            }
            else
            {
                div.InnerHtml += scriptPre.ToString(TagRenderMode.Normal);
                div.InnerHtml += scriptExterno.ToString(TagRenderMode.Normal);
            }

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml SWMultiSelect<T>(this HtmlHelper htmlHelper, string headerEsquerda, string headerDireita, string nomeServico, string metodo, ICollection<T> selecionados = null, int? col = 3, bool injetado = false)
        {
            #region Exemplo Html
            /*  HTML exemplo
                <div>
                    <input type="text" id="filtro" placeholder="Search for names..">
                    <select id='swmulti' multiple='multiple'></select>
                </div>
            */
            #endregion

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            var div = new TagBuilder("div");
            var select = new TagBuilder("select");
            select.MergeAttribute("id", "swmulti");
            select.MergeAttribute("multiple", "multiple");
            var input = new TagBuilder("input");
            input.MergeAttribute("type", "text");
            input.MergeAttribute("id", "filtro-multi");
            var scriptPre = new TagBuilder("script");

            // Javascript: array de ids dos selecionados
            string selecionadosJavascript = "var selecionadosIds = [";
            Type tipo;

            if (selecionados != null && selecionados.Count > 0)
            {
                foreach (var s in selecionados)
                {
                    tipo = s.GetType();
                    PropertyInfo prop = tipo.GetProperty("id");
                    var id = prop.GetValue(s);
                    selecionadosJavascript += "{id:";
                    selecionadosJavascript += id.ToString() + ",";
                    selecionadosJavascript += "checado:";
                    PropertyInfo propChec = tipo.GetProperty("checado");
                    var chec = propChec.GetValue(s);
                    selecionadosJavascript += chec.ToString().ToLower();
                    selecionadosJavascript += "},";
                }
                selecionadosJavascript = selecionadosJavascript.Remove(selecionadosJavascript.Length - 1);
            }

            selecionadosJavascript += "];";
            scriptPre.InnerHtml += selecionadosJavascript;
            scriptPre.InnerHtml += "var headerEsquerda = '" + headerEsquerda + "'; var headerDireita = '" + headerDireita + "';";
            scriptPre.InnerHtml += "function SMWETagschamarServico() {abp.services.app." + nomeServico + "." + metodo + "({}).done(function(data) {data.items.forEach(inserirOpcao);});}";
            var scriptExterno = new TagBuilder("script");
            scriptExterno.MergeAttribute("src", "/Scripts/SMWEHelpers/SMWEMultiSelect.js");
            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing);
            div.InnerHtml += select.ToString(TagRenderMode.Normal);

            if (injetado)
            {
                Javascript += scriptPre.InnerHtml;
                ScriptExterno = scriptExterno.ToString(TagRenderMode.Normal);
            }
            else
            {
                div.InnerHtml += scriptPre.ToString(TagRenderMode.Normal);
                div.InnerHtml += scriptExterno.ToString(TagRenderMode.Normal);
            }

            if (col.HasValue)
            {
                var coluna = new TagBuilder("div");
                string classe = string.Format("{0}{1}{2}{3}", "col-", "sm", "-", col.ToString());
                coluna.AddCssClass(classe);
                coluna.InnerHtml += div.ToString(TagRenderMode.Normal);
                htmlHelper.ViewContext.Writer.Write(coluna.ToString(TagRenderMode.Normal));
                return new BlocoHtml(htmlHelper.ViewContext);
            }

            htmlHelper.ViewContext.Writer.Write(div.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }


        // LISTAGEM
        public static BlocoHtml ListagemCabecalho(this HtmlHelper htmlHelper, string titulo)
        {
            /*

            <div class="row margin-bottom-5">
                <div class="col-xs-6">
                    <div class="page-head">
                        <div class="page-title">
                            <h1>
                                <span>@L("Grupo")</span>

            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            var row = new TagBuilder("div");
            row.MergeAttribute("class", "row margin-bottom-5");
            var pageHead = new TagBuilder("div");
            pageHead.MergeAttribute("class", "page-head");
            var pageTitle = new TagBuilder("div");
            pageTitle.MergeAttribute("class", "page-title");
            var h1 = new TagBuilder("h1");
            var spanTitulo = new TagBuilder("span");

            spanTitulo.InnerHtml += titulo;
            h1.InnerHtml += spanTitulo.ToString(TagRenderMode.Normal);
            pageTitle.InnerHtml += h1.ToString(TagRenderMode.Normal);
            pageHead.InnerHtml += pageTitle.ToString(TagRenderMode.Normal);
            row.InnerHtml += pageHead.ToString(TagRenderMode.Normal);

            htmlHelper.ViewContext.Writer.Write(row.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml ListagemFiltros(this HtmlHelper htmlHelper, string idPrefixo, string funcaoGetJTable)
        {
            /*

            <div class="form">
                    <form id="doc-itens-filtros-form" class="horizontal-form">
                        <div class="form-body" enter-key="vm.getDocItens()">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="inputs inputs-full-width">
                                        <div class="portlet-input">
                                            <fieldset>
                                                <legend class="legendform" style="margin-top:15px; margin-bottom:15px;">Filtros</legend>
                                                <form>

            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var divForm = new TagBuilder("div");
            divForm.MergeAttribute("class", "form");
            var formFiltros = new TagBuilder("form");
            formFiltros.MergeAttribute("id", idPrefixo + "filtros-form");
            formFiltros.MergeAttribute("class", "horizontal-form");
            var formBody = new TagBuilder("div");
            formFiltros.MergeAttribute("class", "form-body");
            formBody.MergeAttribute("enter-key", "vm." + funcaoGetJTable + "()");
            var row = new TagBuilder("div");
            row.MergeAttribute("class", "row");
            var colMd12 = new TagBuilder("div");
            colMd12.MergeAttribute("class", "col-md-12");
            var inputsFullWidth = new TagBuilder("div");
            inputsFullWidth.MergeAttribute("class", "inputs inputs-full-width");
            var portletInput = new TagBuilder("div");
            portletInput.MergeAttribute("class", "portlet-input");
            var fieldSet = new TagBuilder("fieldset");
            var legend = new TagBuilder("legend");
            legend.MergeAttribute("class", "legendform");
            legend.MergeAttribute("style", "margin-top:15px; margin-bottom:15px;");
            legend.InnerHtml += "Filtros";
            var form = new TagBuilder("form");

            fieldSet.InnerHtml += form.ToString(TagRenderMode.Normal) + legend.ToString(TagRenderMode.Normal);
            portletInput.InnerHtml += fieldSet.ToString(TagRenderMode.Normal);
            inputsFullWidth.InnerHtml += portletInput.ToString(TagRenderMode.Normal);
            colMd12.InnerHtml += inputsFullWidth.ToString(TagRenderMode.Normal);
            row.InnerHtml += colMd12.ToString(TagRenderMode.Normal);
            formBody.InnerHtml += row.ToString(TagRenderMode.Normal);
            formFiltros.InnerHtml += formBody.ToString(TagRenderMode.Normal);
            divForm.InnerHtml += formFiltros.ToString(TagRenderMode.Normal);

            htmlHelper.ViewContext.Writer.Write(divForm.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml FiltroLupa(this HtmlHelper htmlHelper, string idComplemento, int col)
        {
            /*

                 <div class="col-sm-4">
                            <label>Pesquisa livre</label>
                            <div class="input-group">
                                <input id="doc-itens-filtro-tabela" class="form-control" placeholder="@L("SearchWithThreeDot")" type="text" value="">
                                <span class="input-group-btn">
                                    <button id="btn-get-doc-itens" class="btn default" type="submit"><i class="icon-magnifier"></i></button>

            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());

            var _col = new TagBuilder("div");
            _col.MergeAttribute("class", "col-sm-" + col.ToString());
            var label = new TagBuilder("label");
            var inputGroup = new TagBuilder("div");
            inputGroup.MergeAttribute("class", "input-group");
            var input = new TagBuilder("input");
            input.MergeAttribute("id", idComplemento + "filtro-tabela");
            input.MergeAttribute("class", "form-control");
            input.MergeAttribute("placeholder", "Pesquisar...");
            input.MergeAttribute("type", "text");
            var span = new TagBuilder("span");
            span.MergeAttribute("class", "input-group-btn");
            var button = new TagBuilder("button");
            button.MergeAttribute("id", "btn-get-" + idComplemento);
            button.MergeAttribute("class", "btn default");
            button.MergeAttribute("type", "submit");
            var icone = new TagBuilder("i");
            icone.MergeAttribute("class", "icon-magnifier");

            label.InnerHtml += "Pesquisa livre";
            button.InnerHtml += icone.ToString(TagRenderMode.Normal);
            span.InnerHtml += button.ToString(TagRenderMode.Normal);
            inputGroup.InnerHtml += input.ToString(TagRenderMode.Normal);
            inputGroup.InnerHtml += span.ToString(TagRenderMode.Normal);
            _col.InnerHtml += label.ToString(TagRenderMode.Normal);
            _col.InnerHtml += inputGroup.ToString(TagRenderMode.Normal);

            htmlHelper.ViewContext.Writer.Write(_col.ToString(TagRenderMode.Normal));
            return new BlocoHtml(htmlHelper.ViewContext, string.Empty);
        }

        public static BlocoHtml FormRetratilJt(this HtmlHelper htmlHelper, string idComplemento, string jTableId, string funcaoSalvarRegistro, string funcaoApagarRegistro, int col, bool injetado = true)
        {
            /*
                <fieldset>
                  <legend class="legendform" style="margin-top:15px; margin-bottom:15px;">
                      <span>Registro</span>
                      <span id="exibir-doc-item-form" class="clickable-item text-muted" style="float:right;">
                            <i class="glyphicon glyphicon-eye-close"></i> @L("Exibir")                      </span>
                      <span id="omitir-doc-item-form" class="clickable-item text-muted" style="display: none; float:right;">
                            <i class="glyphicon glyphicon-eye-open"></i> @L("Omitir")                       </span>
                  </legend>
                
                  <div id="sw-form-retratil" style="display:none; padding:20px; border: 1px solid #4fabff; border-radius: 2px; margin-bottom:10px;">

               CABECALHO
               <div class="col-sm-6 text-left">
                            <span id="sw-form-retratil-cabecalho">Novo</span>
                        </div>
                        <div class="col-sm-6 text-right" style="margin-bottom:8px;">
                            <span class="sw-form-btn" id="btn-remover-selecao-doc-item"><i class="fa fa-close sw-form-btn-icone"></i></span>
                            <span class="sw-form-btn" id="btn-apagar-doc-item"><i class="fa fa-trash-alt sw-form-btn-icone"></i></span>
                            <span class="sw-form-btn" id="btn-salvar-doc-item"><i class="glyphicon glyphicon-floppy-disk"></i></span>
                        </div>
            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            StringBuilder sb = new StringBuilder();

            // Corpo
            sb.Append("<fieldset>");
            sb.Append("<legend class=\"legendform\" style=\"margin-top:15px; margin-bottom:15px;\">");
            sb.Append("<span>Registro</span>");
            sb.Append("<span id=\"exibir-form-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"float:right;\"><i class=\"glyphicon glyphicon-eye-close\"></i></span>");
            sb.Append("<span id=\"omitir-form-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"display: none; float:right;\"><i class=\"glyphicon glyphicon-eye-open\"></i></span>");
            sb.Append("</legend>");
            sb.Append("<div id=\"sw-form-retratil-" + idComplemento + "\" style=\"display:none; padding:20px; border: 1px solid #4fabff; border-radius: 2px; margin-bottom:10px;\">");
            // Cabecalho e botoes
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-sm-6 text-left\">");
            sb.Append("<span id=\"sw-form-retratil-cabecalho-" + idComplemento + "\">Novo</span>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-sm-6 text-right\" style=\"margin-bottom:8px;\">");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-remover-selecao-" + idComplemento + "\"><i class=\"fa fa-close sw-form-btn-icone\"></i></span>");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-apagar-" + idComplemento + "\"><i class=\"fa fa-trash-alt sw-form-btn-icone\"></i></span>");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-salvar-" + idComplemento + "\"><i class=\"glyphicon glyphicon-floppy-disk\"></i></span>");
            sb.Append("</div>");
            sb.Append("</div>");

            // Css
            sb.Append("<style> .sw-form-retratil-btn { border: 1px solid #c2cad8; border-radius: 2px; margin: 3px; float: right; padding: 3px;");
            sb.Append("width: 25px; height: 25px; cursor: pointer; vertical-align: middle; text-align: center; } </style>");

            // Javascript, Jquery
            StringBuilder scriptString = new StringBuilder();

            // Toggle
            scriptString.Append("$('#exibir-form-retratil-" + idComplemento + "').click(function() { $('#exibir-form-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#omitir-form-retratil-" + idComplemento + "').show(); $('#sw-form-retratil-" + idComplemento + "').slideDown(); });");
            scriptString.Append("$('#omitir-form-retratil-" + idComplemento + "').click(function() { $('#omitir-form-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#exibir-form-retratil-" + idComplemento + "').show(); $('#sw-form-retratil-" + idComplemento + "').slideUp(); });");

            // Botoes
            // Remover selecao
            scriptString.Append("$('#sw-form-retratil-btn-remover-selecao-" + idComplemento + "').on('click', function(e) { var jTable = $('#" + jTableId + "'); jTable.find('.jtable-row-selected').click(); });");
            // Apagar selecionado
            scriptString.Append("$('#sw-form-retratil-btn-apagar-" + idComplemento + "').on('click', function(e) { var itemsSelecionados = $('#" + jTableId + "').jtable('selectedRows');");
            scriptString.Append("if (itemsSelecionados.length > 0) { itemsSelecionados.each(function() { var registro = $(this).data('record'); " + funcaoApagarRegistro + "(registro); }); } });");
            // Salvar registro
            scriptString.Append("$('#sw-form-retratil-btn-salvar-" + idComplemento + "').on('click', function(e) { e.preventDefault(); " + funcaoSalvarRegistro + "(); });");

            if (injetado)
            {
                Javascript += scriptString.ToString();
            }
            else
            {
                sb.Append("<script>" + scriptString.ToString() + "</script>");
            }

            htmlHelper.ViewContext.Writer.Write(sb.ToString());
            return new BlocoHtml(htmlHelper.ViewContext, "</div></fieldset>");
        }


        // Em desenvolvimento - Incompleto
        public static BlocoHtml FormRetratilJtHeadless(this HtmlHelper htmlHelper, string idComplemento, string jTableId, string funcaoSalvarRegistro, string funcaoApagarRegistro, int col, bool injetado = true)
        {
            /*
                <fieldset>
                  <legend class="legendform" style="margin-top:15px; margin-bottom:15px;">
                      <span>Registro</span>
                      <span id="exibir-doc-item-form" class="clickable-item text-muted" style="float:right;">
                            <i class="glyphicon glyphicon-eye-close"></i> @L("Exibir")                      </span>
                      <span id="omitir-doc-item-form" class="clickable-item text-muted" style="display: none; float:right;">
                            <i class="glyphicon glyphicon-eye-open"></i> @L("Omitir")                       </span>
                  </legend>
                
                  <div id="sw-form-retratil" style="display:none; padding:20px; border: 1px solid #4fabff; border-radius: 2px; margin-bottom:10px;">

               CABECALHO
               <div class="col-sm-6 text-left">
                            <span id="sw-form-retratil-cabecalho">Novo</span>
                        </div>
                        <div class="col-sm-6 text-right" style="margin-bottom:8px;">
                            <span class="sw-form-btn" id="btn-remover-selecao-doc-item"><i class="fa fa-close sw-form-btn-icone"></i></span>
                            <span class="sw-form-btn" id="btn-apagar-doc-item"><i class="fa fa-trash-alt sw-form-btn-icone"></i></span>
                            <span class="sw-form-btn" id="btn-salvar-doc-item"><i class="glyphicon glyphicon-floppy-disk"></i></span>
                        </div>
            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            StringBuilder sb = new StringBuilder();

            // Corpo
            sb.Append("<fieldset>");
            sb.Append("<legend class=\"legendform\" style=\"margin-top:15px; margin-bottom:15px;\">");
            sb.Append("<span>Registro</span>");
            sb.Append("<span id=\"exibir-form-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"float:right;\"><i class=\"glyphicon glyphicon-eye-close\"></i></span>");
            sb.Append("<span id=\"omitir-form-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"display: none; float:right;\"><i class=\"glyphicon glyphicon-eye-open\"></i></span>");
            sb.Append("</legend>");
            sb.Append("<div id=\"sw-form-retratil-" + idComplemento + "\" style=\"display:none; padding:20px; border: 1px solid #4fabff; border-radius: 2px; margin-bottom:10px;\">");
            // Cabecalho e botoes
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-sm-6 text-left\">");
            sb.Append("<span id=\"sw-form-retratil-cabecalho-" + idComplemento + "\">Novo</span>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-sm-6 text-right\" style=\"margin-bottom:8px;\">");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-remover-selecao-" + idComplemento + "\"><i class=\"fa fa-close sw-form-btn-icone\"></i></span>");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-apagar-" + idComplemento + "\"><i class=\"fa fa-trash-alt sw-form-btn-icone\"></i></span>");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-salvar-" + idComplemento + "\"><i class=\"glyphicon glyphicon-floppy-disk\"></i></span>");
            sb.Append("</div>");
            sb.Append("</div>");

            // Css
            sb.Append("<style> .sw-form-retratil-btn { border: 1px solid #c2cad8; border-radius: 2px; margin: 3px; float: right; padding: 3px;");
            sb.Append("width: 25px; height: 25px; cursor: pointer; vertical-align: middle; text-align: center; } </style>");

            // Javascript, Jquery
            StringBuilder scriptString = new StringBuilder();

            // Toggle
            scriptString.Append("$('#exibir-form-retratil-" + idComplemento + "').click(function() { $('#exibir-form-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#omitir-form-retratil-" + idComplemento + "').show(); $('#sw-form-retratil-" + idComplemento + "').slideDown(); });");
            scriptString.Append("$('#omitir-form-retratil-" + idComplemento + "').click(function() { $('#omitir-form-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#exibir-form-retratil-" + idComplemento + "').show(); $('#sw-form-retratil-" + idComplemento + "').slideUp(); });");

            // Botoes
            // Remover selecao
            scriptString.Append("$('#sw-form-retratil-btn-remover-selecao-" + idComplemento + "').on('click', function(e) { var jTable = $('#" + jTableId + "'); jTable.find('.jtable-row-selected').click(); });");
            // Apagar selecionado
            scriptString.Append("$('#sw-form-retratil-btn-apagar-" + idComplemento + "').on('click', function(e) { var itemsSelecionados = $('#" + jTableId + "').jtable('selectedRows');");
            scriptString.Append("if (itemsSelecionados.length > 0) { itemsSelecionados.each(function() { var registro = $(this).data('record'); " + funcaoApagarRegistro + "(registro); }); } });");
            // Salvar registro
            scriptString.Append("$('#sw-form-retratil-btn-salvar-" + idComplemento + "').on('click', function(e) { e.preventDefault(); " + funcaoSalvarRegistro + "(); });");

            if (injetado)
            {
                Javascript += scriptString.ToString();
            }
            else
            {
                sb.Append("<script>" + scriptString.ToString() + "</script>");
            }

            htmlHelper.ViewContext.Writer.Write(sb.ToString());
            return new BlocoHtml(htmlHelper.ViewContext, "</div></fieldset>");
        }

        // Em desenvolvimento - Incompleto
        public static BlocoHtml FormRetratilJtCabecalho(this HtmlHelper htmlHelper, string idComplemento, string jTableId, string funcaoSalvarRegistro, string funcaoApagarRegistro, int col, bool injetado = true)
        {
            /*
                <fieldset>
                  <legend class="legendform" style="margin-top:15px; margin-bottom:15px;">
                      <span>Registro</span>
                      <span id="exibir-doc-item-form" class="clickable-item text-muted" style="float:right;">
                            <i class="glyphicon glyphicon-eye-close"></i> @L("Exibir")                      </span>
                      <span id="omitir-doc-item-form" class="clickable-item text-muted" style="display: none; float:right;">
                            <i class="glyphicon glyphicon-eye-open"></i> @L("Omitir")                       </span>
                  </legend>
                
                  <div id="sw-form-retratil" style="display:none; padding:20px; border: 1px solid #4fabff; border-radius: 2px; margin-bottom:10px;">

               CABECALHO
               <div class="col-sm-6 text-left">
                            <span id="sw-form-retratil-cabecalho">Novo</span>
                        </div>
                        <div class="col-sm-6 text-right" style="margin-bottom:8px;">
                            <span class="sw-form-btn" id="btn-remover-selecao-doc-item"><i class="fa fa-close sw-form-btn-icone"></i></span>
                            <span class="sw-form-btn" id="btn-apagar-doc-item"><i class="fa fa-trash-alt sw-form-btn-icone"></i></span>
                            <span class="sw-form-btn" id="btn-salvar-doc-item"><i class="glyphicon glyphicon-floppy-disk"></i></span>
                        </div>
            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            StringBuilder sb = new StringBuilder();

            // Cabecalho e botoes
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"col-sm-6 text-left\">");
            sb.Append("<span id=\"sw-form-retratil-cabecalho-" + idComplemento + "\">Novo</span>");
            sb.Append("</div>");
            sb.Append("<div class=\"col-sm-6 text-right\" style=\"margin-bottom:8px;\">");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-remover-selecao-" + idComplemento + "\"><i class=\"fa fa-close sw-form-btn-icone\"></i></span>");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-apagar-" + idComplemento + "\"><i class=\"fa fa-trash-alt sw-form-btn-icone\"></i></span>");
            sb.Append("<span class=\"sw-form-retratil-btn\" id=\"sw-form-retratil-btn-salvar-" + idComplemento + "\"><i class=\"glyphicon glyphicon-floppy-disk\"></i></span>");
            sb.Append("</div>");
            sb.Append("</div>");

            // Css
            sb.Append("<style> .sw-form-retratil-btn { border: 1px solid #c2cad8; border-radius: 2px; margin: 3px; float: right; padding: 3px;");
            sb.Append("width: 25px; height: 25px; cursor: pointer; vertical-align: middle; text-align: center; } </style>");

            // Javascript, Jquery
            StringBuilder scriptString = new StringBuilder();

            // Toggle
            scriptString.Append("$('#exibir-form-retratil-" + idComplemento + "').click(function() { $('#exibir-form-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#omitir-form-retratil-" + idComplemento + "').show(); $('#sw-form-retratil-" + idComplemento + "').slideDown(); });");
            scriptString.Append("$('#omitir-form-retratil-" + idComplemento + "').click(function() { $('#omitir-form-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#exibir-form-retratil-" + idComplemento + "').show(); $('#sw-form-retratil-" + idComplemento + "').slideUp(); });");

            // Botoes
            // Remover selecao
            scriptString.Append("$('#sw-form-retratil-btn-remover-selecao-" + idComplemento + "').on('click', function(e) { var jTable = $('#" + jTableId + "'); jTable.find('.jtable-row-selected').click(); });");
            // Apagar selecionado
            scriptString.Append("$('#sw-form-retratil-btn-apagar-" + idComplemento + "').on('click', function(e) { var itemsSelecionados = $('#" + jTableId + "').jtable('selectedRows');");
            scriptString.Append("if (itemsSelecionados.length > 0) { itemsSelecionados.each(function() { var registro = $(this).data('record'); " + funcaoApagarRegistro + "(registro); }); } });");
            // Salvar registro
            scriptString.Append("$('#sw-form-retratil-btn-salvar-" + idComplemento + "').on('click', function(e) { e.preventDefault(); " + funcaoSalvarRegistro + "(); });");

            if (injetado)
            {
                Javascript += scriptString.ToString();
            }
            else
            {
                sb.Append("<script>" + scriptString.ToString() + "</script>");
            }

            htmlHelper.ViewContext.Writer.Write(sb.ToString());
            return new BlocoHtml(htmlHelper.ViewContext, "</div></fieldset>");
        }


        public static BlocoHtml SWDivRetratil(this HtmlHelper htmlHelper, string idComplemento, string rotulo, bool iniciaVisivel, int? col, bool injetado = true, bool row = false)
        {
            /*
                <fieldset>
                  <legend class="legendform" style="margin-top:15px; margin-bottom:15px;">
                      <span>rotulo</span>
                      <span id="exibir-doc-item-form" class="clickable-item text-muted" style="float:right;">
                            <i class="glyphicon glyphicon-eye-close"></i> @L("Exibir")                      </span>
                      <span id="omitir-doc-item-form" class="clickable-item text-muted" style="display: none; float:right;">
                            <i class="glyphicon glyphicon-eye-open"></i> @L("Omitir")                       </span>
                  </legend>
                  <div id="sw-area-retratil" style="display:none; padding:20px; border: 1px solid #4fabff; border-radius: 2px; margin-bottom:10px;">
            */

            var htmlTextWriter = new HtmlTextWriter(new StringWriter());
            StringBuilder sb = new StringBuilder();

            // Corpo
            if (row)
            {
                sb.Append("<div class=\"row\">");
            }

            if (col.HasValue)
            {
                sb.Append("<div class=\"col-sm-" + col.ToString() + "\">");
            }

            string iniciaVisivelStyle = "none";

            if (iniciaVisivel)
            {
                iniciaVisivelStyle = "block";
            }

            sb.Append("<fieldset>");
            sb.Append("<legend class=\"legendform\" style=\"margin-top:15px; margin-bottom:15px;\">");
            sb.Append("<span>" + rotulo + "</span>");
            if (iniciaVisivel)
            {
                sb.Append("<span id=\"exibir-sw-div-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"display: none; float:right;\"><i class=\"glyphicon glyphicon-eye-close\"></i></span>");
                sb.Append("<span id=\"omitir-sw-div-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"float:right;\"><i class=\"glyphicon glyphicon-eye-open\"></i></span>");
            }
            else
            {
                sb.Append("<span id=\"exibir-sw-div-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"float:right;\"><i class=\"glyphicon glyphicon-eye-close\"></i></span>");
                sb.Append("<span id=\"omitir-sw-div-retratil-" + idComplemento + "\" class=\"clickable-item text-muted\" style=\"display: none; float:right;\"><i class=\"glyphicon glyphicon-eye-open\"></i></span>");
            }


            sb.Append("</legend>");
            sb.Append("<div id=\"sw-div-retratil-" + idComplemento + "\" style=\"display:" + iniciaVisivelStyle + "; padding:20px; border: 1px solid #4fabff; border-radius: 2px; margin-bottom:10px;\">");

            // Javascript, Jquery
            StringBuilder scriptString = new StringBuilder();

            // Toggle
            scriptString.Append("$('#exibir-sw-div-retratil-" + idComplemento + "').click(function() { $('#exibir-sw-div-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#omitir-sw-div-retratil-" + idComplemento + "').show(); $('#sw-div-retratil-" + idComplemento + "').slideDown(); });");
            scriptString.Append("$('#omitir-sw-div-retratil-" + idComplemento + "').click(function() { $('#omitir-sw-div-retratil-" + idComplemento + "').hide();");
            scriptString.Append("$('#exibir-sw-div-retratil-" + idComplemento + "').show(); $('#sw-div-retratil-" + idComplemento + "').slideUp(); });");

            if (injetado)
            {
                Javascript += scriptString.ToString();
            }
            else
            {
                sb.Append("<script>" + scriptString.ToString() + "</script>");
            }

            string tagsFechamento = "</div></fieldset>";

            if (col.HasValue)
            {
                tagsFechamento += "</div>";
            }

            if (row)
            {
                tagsFechamento += "</div>";
            }

            htmlHelper.ViewContext.Writer.Write(sb.ToString());
            return new BlocoHtml(htmlHelper.ViewContext, tagsFechamento);
        }

    }

    /// <summary>
    /// Classe de retorno para os metodos de SMWETags
    /// </summary>
    public class BlocoHtml : IDisposable
    {
        private readonly TextWriter Writer;
        private string TagFechamento;

        public BlocoHtml(ViewContext viewContext, string tagFechamento = "")
        {
            Writer = viewContext.Writer;
            TagFechamento = tagFechamento;
        }

        public void Dispose()
        {
            Writer.Write(TagFechamento);
        }
    }

    /// <summary>
    /// Classe de retorno para os metodos de SMWETags
    /// </summary>
    public class BlocoJavascript : IDisposable
    {
        private readonly TextWriter Writer;

        public BlocoJavascript(ViewContext viewContext)
        {
            Writer = viewContext.Writer;
        }

        public void Dispose()
        {
            SMWETags.LimparJavascript();
        }
    }

    /// <summary>
    /// Classe cujas propriedades sao referentes a atributos utilizados em tags html e componentes SMWETags
    /// </summary>
    public class SWTag
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Classe { get; set; }
        public string Label { get; set; }

        public string ClasseLabel { get; set; }
        public bool Checked { get; set; }
        public bool Required { get; set; }
        public bool Disabled { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public int? Col { get; set; }

        //public SWTag(string id, string name, string label, string classeCss = "", bool _checked = false, int? col = 3, string value = "", string text = "", bool required = false, bool disabled = false)
        //{
        //    Id = id;
        //    Name = name;
        //    Classe = classeCss;
        //    Label = label;
        //    Checked = _checked;
        //    Col = col;
        //    Value = value;
        //    Text = text;
        //    Required = required;
        //    Disabled = disabled;
        //}

        public SWTag(string id, string name, string label, string classeCss = "", bool _checked = false, int? col = 3, string value = "", string text = "", bool required = false, bool disabled = false, string classeLabel = null)
        {
            Id = id;
            Name = name;
            Classe = classeCss;
            Label = label;
            Checked = _checked;
            Col = col;
            Value = value;
            Text = text;
            Required = required;
            Disabled = disabled;
            ClasseLabel = classeLabel;
        }
    }

    public class MultiSelectItem
    {
        public long id { get; set; }
        public bool checado { get; set; }

        public MultiSelectItem(long _id, bool _checado)
        {
            id = _id;
            checado = _checado;
        }
    }
}