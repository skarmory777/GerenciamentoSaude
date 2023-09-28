using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaConvenioCodigo;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TabelasPrecosItens.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TabelaConvenioCodigos.Dto
{
    public class TabelaConvenioCodigoDto : CamposPadraoCRUDDto
    {
        public bool IsFromTuss { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long? TabelaPrecoItemId { get; set; }
        public FaturamentoTabelaPrecoItemDto TabelaPrecoItem { get; set; }

        #region Mapeamento
        public static TabelaConvenioCodigoDto Mapear(TabelaConvenioCodigo input)
        {
            var result = new TabelaConvenioCodigoDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.ConvenioId = input.ConvenioId;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.IsFromTuss = input.IsFromTuss;
            result.TabelaPrecoItemId = input.TabelaPrecoItemId;

            if (input.Convenio != null)
            {
                result.Convenio = ConvenioDto.Mapear(input.Convenio);
            }

            if (input.TabelaPrecoItem != null)
            {
                result.TabelaPrecoItem = FaturamentoTabelaPrecoItemDto.Mapear(input.TabelaPrecoItem);
            }
            return result;
        }

        public static TabelaConvenioCodigo Mapear(TabelaConvenioCodigoDto input)
        {
            var result = new TabelaConvenioCodigo();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.ConvenioId = input.ConvenioId;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.IsFromTuss = input.IsFromTuss;
            result.TabelaPrecoItemId = input.TabelaPrecoItemId;

            return result;
        }

        public static IEnumerable<TabelaConvenioCodigoDto> Mapear(List<TabelaConvenioCodigo> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }

        public static IEnumerable<TabelaConvenioCodigo> Mapear(List<TabelaConvenioCodigoDto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        #endregion
    }
}
