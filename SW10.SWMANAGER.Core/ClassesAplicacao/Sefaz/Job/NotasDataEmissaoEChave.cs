using Sefaz;
using System;
using System.Globalization;

namespace SW10.SWMANAGER.ClassesAplicacao.Sefaz
{
    public class NotasDataEmissaoEChave : IOutputBuscaNotasDto
    {
        public DateTime DataEmissao { get; set; }
        public string NfeChave { get; set; }

        public NotasDataEmissaoEChave()
        {

        }

        public NotasDataEmissaoEChave(string[] args)
        {

        }

        public IOutputBuscaNotasDto MapArgs(string[] args)
        {
            if (args[0].CheckIfIsNullOrEmpty())
            {
                return null;
            }
            return new NotasDataEmissaoEChave()
            {
                DataEmissao = DateTime.Parse(args[0], CultureInfo.GetCultureInfo("pt-BR")),
                NfeChave = args[1]
            };
        }
    }
}
