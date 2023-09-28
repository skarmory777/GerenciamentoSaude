using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices._3_05_00
{
    public class SpEntregaConvertTiss3_05_00 : BaseSpEntregaConvertTiss, ISpEntregaConvertTiss
    {
        public override EntregaTissLoteGerado ConvertMensagemTISS(SpEntrega entrega)
        {
            switch (entrega.Lote.FatGuiaId)
            {
                case FaturamentoGuiaDto.GuiaSpSadt:
                    {
                        this.TissAbreTag()
                            .TissCabecalho(entrega)
                            .TissPrestadorParaOperadora(entrega, SpEntregaConvertTissExtensions3_05_00.TissGuiasSPSADT)
                            .TissEpilogo(entrega)
                        .TissFechaTag();
                        break;
                    }
                case FaturamentoGuiaDto.GuaResumoInternacao:
                    {
                        this.TissAbreTag()
                            .TissCabecalho(entrega)
                            .TissPrestadorParaOperadora(entrega, SpEntregaConvertTissExtensions3_05_00.TissGuiasResumoInternacao)
                            .TissEpilogo(entrega)
                        .TissFechaTag();
                        // Fazer
                        break;
                    }
                default: {
                        break;
                    }
            }
            
            return new EntregaTissLoteGerado(entrega, SbContent);
            
        }
    }
}