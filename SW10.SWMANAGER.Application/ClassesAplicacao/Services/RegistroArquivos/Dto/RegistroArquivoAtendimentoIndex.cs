using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto
{
    public class RegistroArquivoAtendimentoIndex
    {
        public long RegistroId { get; set; }
        public string OperacaoDescricao { get; set; }
        public DateTime DataRegistro { get; set; }
        public bool IsPDF { get; set; }

    }
}
