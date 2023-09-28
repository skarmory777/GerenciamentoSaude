using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto
{
    public class ListarAtendimentosLeitosMovInput : ListarInput
    {

        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public DateTime? DataInclusao { get; set; }

        public string Nome { get; set; }

    }
}
