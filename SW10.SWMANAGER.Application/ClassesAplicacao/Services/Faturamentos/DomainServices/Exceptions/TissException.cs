using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices
{
    public class TissException : Exception
    {
        public TissException()
        {

        }
        public TissException(string message) : base(message)
        {
        }

        public TissException(string message, Exception e) : base(message, e)
        {
        }
    }
}