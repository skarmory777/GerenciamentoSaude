namespace Sefaz.Dto
{
    using System;
    using System.Globalization;

    public class NotasDataEmissaoEChaveDto : IOutputBuscaNotasDto
    {
        public DateTime DataEmissao { get; set; }
        public string NfeChave { get; set; }

        public NotasDataEmissaoEChaveDto()
        {

        }

        public NotasDataEmissaoEChaveDto(string[] args)
        {

        }

        public IOutputBuscaNotasDto MapArgs(string[] args)
        {
            if (args[0].CheckIfIsNullOrEmpty())
            {
                return null;
            }

            return new NotasDataEmissaoEChaveDto()
            {
                DataEmissao = DateTime.Parse(args[0], CultureInfo.GetCultureInfo("pt-BR")),
                NfeChave = args[1]
            };
        }
    }
}
