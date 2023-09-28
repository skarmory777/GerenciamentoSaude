using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto
{
    public class RelatorioUltimasComprasVsAtualDto
    {
        public DateTime DataInicioBase { get; set; }

        public DateTime DataFimBase { get; set; }
        public DateTime DataInicioAtual { get; set; }
        public DateTime DataFimAtual { get; set; }
        public long CasasDecimaisCusto { get; set; }
        public long CasasDecimaisVariacao { get; set; }
        public long? EmpresaId { get; set; }
    }
}