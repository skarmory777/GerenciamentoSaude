using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Lote
{
    public class FaturamentoEntregaLoteViewModel
    {
        public FaturamentoEntregaLoteViewModel(FaturamentoEntregaLoteDto lote)
        {
            Lote = lote;
        }

        public FaturamentoEntregaLoteDto Lote { get; set; }
    }
}