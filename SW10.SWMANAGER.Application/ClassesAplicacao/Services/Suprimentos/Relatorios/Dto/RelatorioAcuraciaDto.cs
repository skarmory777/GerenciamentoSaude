using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto
{
    public class RelatorioAcuraciaDto
    {
        public long? EmpresaId { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }

        public long? EstoqueId { get; set; }

    }
}
