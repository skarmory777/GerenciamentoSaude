using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(EstMovimentoBaixaItem))]
    public class EstMovimentoBaixaItemDto : CamposPadraoCRUDDto
    {
        public long EstoqueMovimentoBaixaId { get; set; }
        public long EstoqueMovimentoItemId { get; set; }
        public decimal Quantidade { get; set; }

        public static EstMovimentoBaixaItemDto Mapear(EstMovimentoBaixaItem estMovimentoBaixaItem)
        {
            if (estMovimentoBaixaItem == null)
            {
                return null;
            }

            var estMovimentoBaixaItemDto = new EstMovimentoBaixaItemDto();

            estMovimentoBaixaItemDto.Id = estMovimentoBaixaItem.Id;
            estMovimentoBaixaItemDto.Codigo = estMovimentoBaixaItem.Codigo;
            estMovimentoBaixaItemDto.Descricao = estMovimentoBaixaItem.Descricao;
            estMovimentoBaixaItemDto.EstoqueMovimentoBaixaId = estMovimentoBaixaItem.EstoqueMovimentoBaixaId;
            estMovimentoBaixaItemDto.EstoqueMovimentoItemId = estMovimentoBaixaItem.EstoqueMovimentoItemId;
            estMovimentoBaixaItemDto.Quantidade = estMovimentoBaixaItem.Quantidade;

            return estMovimentoBaixaItemDto;
        }


        public static EstMovimentoBaixaItem Mapear(EstMovimentoBaixaItemDto estMovimentoBaixaItemDto)
        {
            if (estMovimentoBaixaItemDto == null)
            {
                return null;
            }

            var estMovimentoBaixaItem = new EstMovimentoBaixaItem();

            estMovimentoBaixaItem.Id = estMovimentoBaixaItemDto.Id;
            estMovimentoBaixaItem.Codigo = estMovimentoBaixaItemDto.Codigo;
            estMovimentoBaixaItem.Descricao = estMovimentoBaixaItemDto.Descricao;
            estMovimentoBaixaItem.EstoqueMovimentoBaixaId = estMovimentoBaixaItemDto.EstoqueMovimentoBaixaId;
            estMovimentoBaixaItem.EstoqueMovimentoItemId = estMovimentoBaixaItemDto.EstoqueMovimentoItemId;
            estMovimentoBaixaItem.Quantidade = estMovimentoBaixaItemDto.Quantidade;

            return estMovimentoBaixaItem;
        }

    }
}
