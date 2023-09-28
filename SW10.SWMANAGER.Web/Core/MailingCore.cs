using FluentEmail;
using SW10.SWMANAGER.ClassesAplicacao.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.MailingTemplate;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnvioEmail.Core
{
    public class MailingCore
    {
        public ClassDetails SetTypeFields(Type type)
        {
            ClassDetails result = new ClassDetails();

            if (type != null)
            {
                result.Name = type.Name;
                result.FullName = type.FullName;
                result.PropriertyTemplate = $"Class: {type.Name} ; Campos: ";
                var proprieties = type.GetProperties();
                var limit = proprieties.Count();
                for (int i = 0; i < limit; i++)
                {
                    result.PropriertyTemplate += $"@Model.{proprieties[i].Name}" + (i < (limit - 1) ? ", " : string.Empty);
                }

            }

            return result;
        }

        public Task<bool> ProcessarEnvioAsync<T>(EnvioModel model, T sources, MailingTemplate template, string emailDestino)
        {
            return Task.Run(() => ProcessarEnvio(model, sources, template, emailDestino));
        }

        /// <summary>
        /// Processa o envio de E-mail
        /// </summary>
        /// <typeparam name="T">Classe com os dados que serão enviados</typeparam>
        /// <param name="model">Model de envio de e-mail</param>
        /// <param name="sources">Dados com o conteúdo da mensagem</param>
        /// <param name="template">Template que será usada</param>
        /// <param name="emailDestino">E-mail de destido</param>
        /// <returns></returns>
        private bool ProcessarEnvio<T>(EnvioModel model, T sources, MailingTemplate template, string emailDestino)
        {

            if (template != null && !string.IsNullOrEmpty(emailDestino) && sources != null)
            {
                var emailCliente = new Email(template.EmailSaida, name: template.NomeSaida)
                            .BodyAsHtml()
                            .Subject(template.Titulo)
                            .UsingTemplate(template.ContentTemplate, sources)
                            .To(emailDestino)
                            .Send();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Classe de apoio para motar as propriedades de qualquer classe
        /// </summary>
        public class ClassDetails
        {
            /// <summary>
            /// Nome da Classe
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Nomde da Classe com a NameSpace
            /// </summary>
            public string FullName { get; set; }

            /// <summary>
            /// Propriedades dos campos dispodineis para serem usados no template
            /// </summary>
            public string PropriertyTemplate { get; set; }
        }
    }
}