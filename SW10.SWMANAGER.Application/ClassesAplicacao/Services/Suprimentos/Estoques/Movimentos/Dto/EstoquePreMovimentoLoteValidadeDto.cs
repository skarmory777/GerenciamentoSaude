using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(EstoquePreMovimentoLoteValidade))]
    public class EstoquePreMovimentoLoteValidadeDto : CamposPadraoCRUDDto
    {
        public long EstoquePreMovimentoItemId { get; set; }
        public long LoteValidadeId { get; set; }
        public EstoquePreMovimentoItemDto EstoquePreMovimentoItem { get; set; }
        public LoteValidadeDto LoteValidade { get; set; }
        public long? LaboratorioId { get; set; }
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public long ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public bool EntradaConfirmada { get; set; }
        public string ProdutoDescricao { get; set; }


        public static EstoquePreMovimentoLoteValidadeDto Mapear(EstoquePreMovimentoLoteValidade estoquePreMovimentoLoteValidade)
        {
            var estoquePreMovimentoLoteValidadeDto = MapearBase<EstoquePreMovimentoLoteValidadeDto>(estoquePreMovimentoLoteValidade);

            estoquePreMovimentoLoteValidadeDto.EstoquePreMovimentoItemId = estoquePreMovimentoLoteValidade.EstoquePreMovimentoItemId;
            estoquePreMovimentoLoteValidadeDto.LoteValidadeId = estoquePreMovimentoLoteValidade.LoteValidadeId;
            estoquePreMovimentoLoteValidadeDto.Quantidade = estoquePreMovimentoLoteValidade.Quantidade;

            if(estoquePreMovimentoLoteValidade.LoteValidade != null)
            {
                estoquePreMovimentoLoteValidadeDto.LoteValidade = LoteValidadeDto.Mapear(estoquePreMovimentoLoteValidade.LoteValidade);
            }


            return estoquePreMovimentoLoteValidadeDto;
        }
    }
}
 