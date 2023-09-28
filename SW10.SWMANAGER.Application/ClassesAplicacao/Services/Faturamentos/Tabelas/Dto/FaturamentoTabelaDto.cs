using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto
{
    [AutoMap(typeof(FaturamentoTabela))]
    public class FaturamentoTabelaDto : CamposPadraoCRUDDto
    {
        public bool IsAtualizaBrasindice { get; set; }

        //[ForeignKey("TabelaTissItemId")]
        //public virtual TabelaTissItem TabelaTissItem { get; set; }
        public long? TabelaTissItemId { get; set; }

        public bool IsCBHPM { get; set; }


        public static FaturamentoTabelaDto Mapear(FaturamentoTabela faturamentoTabela)
        {
            if (faturamentoTabela == null)
            {
                return null;
            }

            var faturamentoTabelaDto = MapearBase<FaturamentoTabelaDto>(faturamentoTabela);

            faturamentoTabelaDto.IsAtualizaBrasindice = faturamentoTabela.IsAtualizaBrasindice;
            faturamentoTabelaDto.TabelaTissItemId = faturamentoTabela.TabelaTissItemId;
            faturamentoTabelaDto.IsCBHPM = faturamentoTabela.IsCBHPM;

            return faturamentoTabelaDto;
        }

        public static FaturamentoTabela Mapear(FaturamentoTabelaDto faturamentoTabelaDto)
        {
            if (faturamentoTabelaDto == null)
            {
                return null;
            }

            var faturamentoTabela = MapearBase<FaturamentoTabela>(faturamentoTabelaDto);
            faturamentoTabela.IsAtualizaBrasindice = faturamentoTabelaDto.IsAtualizaBrasindice;
            faturamentoTabela.TabelaTissItemId = faturamentoTabelaDto.TabelaTissItemId;
            faturamentoTabela.IsCBHPM = faturamentoTabelaDto.IsCBHPM;

            return faturamentoTabela;
        }

    }
}
