using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    [Table("SisErro")]
    public class Erro : CamposPadraoCRUD
    {
        public string Message { get; set; }

        public string InnerException { get; set; }

        public string InnerMessage { get; set; }

        public string Stacktrace { get; set; }

        public string Conteudo { get; set; }

        public Erro(Exception e)
        {
            int pos = e.StackTrace.LastIndexOf("\\") + 1;
            string arq = e.StackTrace.Substring(pos, e.StackTrace.Length - pos);

            Message = e.Message;
            InnerException = e.InnerException?.ToString();
            InnerMessage = e.InnerException?.Message;
            Stacktrace = arq;
            Conteudo = e.ToString();
        }
    }
}
